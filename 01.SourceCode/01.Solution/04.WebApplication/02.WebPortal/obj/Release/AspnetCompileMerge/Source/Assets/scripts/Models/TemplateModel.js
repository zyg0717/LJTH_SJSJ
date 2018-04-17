$(function () {
    if (utils.getQueryString("templateId").length > 0) {
        LoadData(utils.getQueryString("templateId"), BuildPage);
    } else {
        defaultModel.sheets = excelHelper.DefaultStruct();
        BuildPage(defaultModel);
    }
});
function LoadData(templateId, callback) {
    Vue.ajaxGet({
        url: api_url + "api/templates/load/" + templateId,
        success: function (result) {
            var data = result.Model;
            data.AttachmentId = result.Attachment.ID;
            data.modelChecked = data.IsImport ? "2" : "0";
            data.cloneModelBtn = false;
            data.excelEdit = true;
            data.modelUpdate = false;
            data.fileUpload = false;
            data.prevBtn = false;
            data.previewBtn = true;
            data.Edit = true;
            for (var i = 0; i < data.sheets.length; i++) {
                var sheet = data.sheets[i];
                for (var j = 0; j < sheet.columns.length; j++) {
                    sheet.columns[j].code = excelHelper.ParseCode(j + 1);
                }
            }
            callback(data);
        }, error: function (status, statusText, msg) {
            //alert('提交错误');
        }
    });
}
function BuildPage(data) {
    data.dialog = { show: false, search: { name: '' }, title: "选择模板", content: { data: new Array(), total: 0 } };
    data.isMobile = utils.mobileBrower();
    data.options = excelHelper.LoadOptions(data.modelChecked, data.Edit);
    task = new Vue({
        el: '#templateModel',
        data: data,
        created: function () {
            var self = this;
            setTimeout(function () {
                $(self.$el).find("#fileUpload").ajaxupload({
                    formData: { "action": 'UploadTemplateAttach' },
                    paramName: "FileData",
                    url: api_url + 'api/templates/analysis',
                    finish: function (data) {
                        self.uploadFile(data);
                    }
                })
            }, 0);

        },
        mounted: function () {

        },
        watch: {
            modelChecked: function (n) {
                var self = this;
                switch (n) {
                    case "0":
                        {
                            self.reload(excelHelper.DefaultStruct());
                            self.excelEdit = true;
                            self.cloneModelBtn = false;
                            self.modelUpdate = false;
                            self.prevBtn = false;
                            self.previewBtn = true;

                        }
                        break;
                    case "1":
                        {
                            self.excelEdit = true;
                            self.cloneModelBtn = true;
                            self.modelUpdate = false;
                            self.prevBtn = false;
                            self.previewBtn = true;
                        }
                        break;
                    case "2":
                        {
                            self.excelEdit = false;
                            self.cloneModelBtn = false;
                            self.modelUpdate = true;
                            self.fileUpload = false;
                            self.prevBtn = false;
                            self.previewBtn = true;
                        }
                        break;
                }
                self.options = excelHelper.LoadOptions(n, self.Edit);
            },
            fileUpload: function (n) {
                console.log(n);
            }
        },
        methods: {
            dialogReload: function () {
                var self = this;
                self.$refs.pager.$emit("paging-first");
            },
            loadTemplates: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.ajaxPost({
                    url: api_url + "api/templates/query",
                    args: {
                        "TemplateName": self.dialog.search.name,
                        "TemplateCreateLoginOrName": null,
                        "IsOnlySelf": true,
                        "TimeRange": null,
                        "WithImport": false,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.dialog.content.total = res.TotalCount;
                        self.dialog.content.data = res.Data;
                        //$.each(res.Data, function (i, item) {
                        //    self.templates.push(item);
                        //});
                        callback();
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });

            },
            dialogClose: function () {
                var self = this;
                self.dialog.show = false;
                $("html,body").css({ 'overflow-y': 'visible' });
            },
            selectRow: function (node) {
                var self = this;
                //self.TaskTemplateName = node.TemplateName;
                //self.TaskTemplateID = node.TemplateID;
                LoadData(node.TemplateID, function (data) {
                    //self.modelChecked = data.modelChecked;
                    self.Edit = false;
                    self.options = excelHelper.LoadOptions(self.modelChecked, self.Edit);
                    self.reload(data.sheets);
                    self.dialog.show = false;
                    $("html,body").css({ 'overflow-y': 'visible' });
                });
            },
            selectTemplate: function () {
                var self = this;
                self.dialog.show = true;
                $("html,body").css({ 'overflow-y': 'hidden', 'height': '100%' });
            },
            reload: function (sheets) {
                var self = this;
                self.sheets.splice(0, self.sheets.length);
                for (var i = 0; i < sheets.length; i++) {
                    self.sheets.push(sheets[i]);
                }
                if (self.sheets.length > 0) {
                    self.sheets[0].select = true;
                }
            },
            uploadFile: function (result) {
                var self = this;
                if (result != null) {
                    self.reload(result.Data);
                    self.AttachmentId = result.Attachment.ID;
                    self.TemplateName = result.Attachment.Name;
                }
                self.fileUpload = true;
            },
            removeSheet: function (sheet) {
                var self = this;
                self.$refs.excel.$emit("excel-remove-sheet", sheet);
            },
            goPrevStep: function () {
                var self = this;
                self.fileUpload = false;
            },
            goNextStep: function () {
                var self = this;
                var ranges = new Array();
                var hasError = false;
                for (var j = 0; j < self.sheets.length; j++) {
                    var sheet = self.sheets[j];
                    var area = sheet.rangePos.split("");
                    if (area.length < 2) {
                        hasError = true;
                        utils.alertMessage("数据区域填写错误", function () {
                        });
                        break;
                    }
                    var column = '';
                    var columnRegx = /^[A-Za-z]*$/;

                    for (var i = 0; i < area.length; i++) {
                        if (columnRegx.test(area[i])) {
                            column += area[i];
                        }
                        else {
                            break;
                        }
                    }
                    var row = '';
                    var rowRegx = /^[0-9]*$/;
                    for (var i = 0; i < area.length; i++) {
                        if (rowRegx.test(area[i])) {
                            row += area[i];
                        }
                    }
                    sheet.firstcolumn = excelHelper.ConvertCode(column);
                    sheet.firstrow = parseInt(row);
                    ranges.push({ name: sheet.name, row: sheet.firstrow, column: sheet.firstcolumn });
                }
                if (hasError) {
                    return;
                }
                self.ajaxPost({
                    url: api_url + "api/templates/setrange/" + self.AttachmentId,
                    args: ranges,
                    success: function (result) {
                        self.reload(result.Data);
                        self.fileUpload = false;
                        self.modelUpdate = false;
                        self.excelEdit = true;
                        self.prevBtn = true;
                        self.previewBtn = true;
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });
            },
            goDataRange: function () {
                var self = this;
                self.prevBtn = false;
                self.excelEdit = false;
                self.modelUpdate = true;
                self.fileUpload = true;
                self.previewBtn = false;
            },
            PreView: function () {
                var self = this;
                self.AttachmentId = (self.AttachmentId == null || self.AttachmentId.length == 0) ? "" : self.AttachmentId;
                //这里需要改成新窗口post  同时要考虑AttachmentId问题
                //utils.postToWin({
                //    url: api_url + "api/templates/preview/" + self.modelChecked,
                //    args: self.$data
                //});
                //for owa
                self.ajaxPost({
                    url: api_url + "api/templates/preview/online/" + self.modelChecked,
                    args: self.$data,
                    success: function (data) {
                        //window.open(data);
                        if (data.IsUseV1) {
                            if (!LinkToV2) {
                                commons.OpenOWAPreview(data.result);
                            } else {
                                window.open(data.result);
                            }
                        } else {
                            commons.OWA_PreviewAttachment(data.result);
                        }
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });


            },
            Save: function () {
                var self = this;
                if (!self.cellformulaFilter(self.$data,"formula")) {
                    return false;
                };
                if (!self.cellformulaFilter(self.$data, "require")) {
                    return false;
                }
                var action = self.Edit ? "update" : "save";
                var url = '';
                self.AttachmentId = (self.AttachmentId == null || self.AttachmentId.length == 0) ? "" : self.AttachmentId;
                if (self.Edit) {
                    url = api_url + "api/templates/update/" + self.TemplateID;
                } else {
                    url = api_url + "api/templates/save/" + self.modelChecked
                }
                self.ajaxPost({
                    url: url,
                    args: self.$data,
                    success: function (data) {
                        utils.alertMessage("保存成功", function () {
                            window.location.href = '/Application/Models/TemplateModel.aspx?templateId=' + data;
                        });
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });
            },
            Delete: function () {
                var self = this;
                utils.confirmMessage("确定要删除吗?", function (ret) {
                    if (!ret) {
                        return false;
                    }
                    self.ajaxDelete({
                        url: api_url + "api/templates/delete/" + self.TemplateID,
                        success: function (res) {
                            utils.alertMessage('删除成功', function () {
                                window.location.href = '/Application/Models/TemplateList.aspx';
                            });
                        }, error: function (status, statusText, msg) {
                            //alert('提交错误');
                        }
                    });
                })
            },
            Clear: function () {
                var self = this;
                if (!self.cellformulaFilter(self.$data, "require")) {
                    return false;
                }
                self.columnNameFilter("submit", false);
            },
            columnNameFilter: function (name, bol) {
                var self = this;
                if (name == "show") {
                    var sheets = self.$data.sheets.filter(function (one) {
                        var columnsName = one.columns.filter(function (val) {
                            if ($.trim(val.name)) {
                                return val
                            }
                        })
                        if (columnsName.length) {
                            return columnsName;
                        }
                    })
                    if (sheets.length) {
                        return true;
                    } else {
                        return false;
                    }
                }
                if (name == "submit") {
                    for (var i = 0; i < self.$data.sheets.length; i++) {
                        var one = self.$data.sheets[i];
                        if (one.startSort >= 0 && one.endSort >= 0) {
                            for (var j = 0; j < one.columns.length; j++) {
                                var val = one.columns[j];
                                var name = val.name.replace(/\s+/g, "");
                                if (name && val.sort >= one.startSort && val.sort <= one.endSort) {
                                    if (bol) {
                                        val.required = true;
                                    } else {
                                        val.required = false;
                                        one.startSort = -1;
                                        one.endSort = -1;
                                    }
                                }
                                if (one.columns.length - 1 == j) {
                                    if (!bol) {
                                        utils.alertMessage("取消必填成功", function () { })
                                    }
                                }
                            }
                        }
                    }
                }
            },
            cellformulaFilter: function (data,name) {
                var bol = true;
                for (var i = 0; i < data.sheets.length; i++) {
                    var one = data.sheets[i];

                    for (var j = 0; j < one.columns.length; j++) {
                        var val = one.columns[j];
                        if (val.isformula && name == "formula") {
                            if (val.cellformula == "") {
                                utils.alertMessage("请输入" + one.name + " 下 " + val.code + "列 所对应的公式", function () {
                                });
                                bol = false;
                                return bol;
                            }
                        }
                        if (name == "require") {
                            if (one.startSort == -1 && one.startSort < one.endSort) {
                                var msg = "请设置" + one.name + "必填选项起始列"
                                utils.alertMessage(msg, function () { })
                                bol = false;
                                return bol;
                            }
                            if (one.endSort == -1 && one.startSort > one.endSort) {
                                var msg = "请设置" + one.name + "必填选项终止列"
                                utils.alertMessage(msg, function () { })
                                bol = false;
                                return bol;
                            }
                        }
                    }
                }
                return bol;
            }
        }
    });
}
var defaultModel = {
    modelChecked: "0",
    cloneModelBtn: false,
    excelEdit: true,
    modelUpdate: false,
    fileUpload: false,
    prevBtn: false,
    previewBtn: true,
    sheets: [],
    TemplateName: '',
    Edit: false,
    AttachmentId: null
};
$(function () {
    $("#navbar-menu li").removeClass('active').filter('.all-template').addClass("active");
    var template = new Vue({
        el: "#templateList",
        data: {
            templateList: [],
            keyWord: '',
            createName: null,
            selectedOption: ' ',
            isPrivate: false,
            total: 0,
            loading: false,
            pageSize: 10,
            isMobile: utils.mobileBrower()
        },
        methods: {
            reload: function () {
                var self = this;
                self.templateList = [];
                self.$refs.pager.$emit("paging-first");
            },
            load: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.ajaxPost({
                    url: api_url + "api/templates/query",
                    args: {
                        "TemplateName": self.keyWord,
                        "TemplateCreateLoginOrName": self.createName,
                        "IsOnlySelf": self.isPrivate,
                        "TimeRange": self.selectedOption,
                        "WithImport": true,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.total = res.TotalCount;
                        utils.log(res);
                        //如果不是滚动分页模式 要清空原有数据
                        if (!isScroll) {
                            self.templateList = res.Data;
                        } else {
                            $.each(res.Data, function (i, item) {
                                self.templateList.push(item);
                            });
                        }
                        if (callback != undefined && typeof callback === 'function') {
                            callback();
                        }
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });
            },
            deleteTemplate: function (templateId) {
                var self = this;
                utils.confirmMessage("确定要删除吗?", function (ret) {
                    if (!ret) {
                        return false;
                    }
                    self.ajaxDelete({
                        url: api_url + "api/templates/delete/" + templateId,
                        success: function (res) {
                            utils.alertMessage('删除成功', function () {
                                self.reload();
                            });
                        }, error: function (status, statusText, msg) {
                            //alert('提交错误');
                        }
                    });
                })
            },
            downLoadTemplate: function (templateId) {
                DownloadTemplate(templateId);
            }
        }
    });
});
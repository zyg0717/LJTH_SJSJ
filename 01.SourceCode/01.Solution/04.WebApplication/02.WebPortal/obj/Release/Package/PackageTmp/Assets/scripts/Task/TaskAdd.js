
var task = null;
var newTaskID = (new GUID()).newGUID();
var defaultModel = {
    IsEdit: false,
    TaskID: newTaskID,
    TaskName: null,
    TaskCreateDate: null,
    TaskType: 1,
    TaskStatus: 0,
    TaskTemplateName: null,
    TaskTemplateID: null,
    TaskTemplateAttachmentID: null,
    IsNeedApprove: false,
    Remark: '',
    Attachments: [],
    TaskCircleType: 1,
    PlanStart: null,
    PlanEnd: null,
    PlanHour: 9,
    PlanHourAndMinute: '9:00',
    PlanMinute: 0,
    TaskTimeNodes: [],
    UserNodes: [],
    TaskAttachmentCount: 0,
    TaskTakingTime: 0,
    OwnerTaskID: null,
};




function CalcDateTable($picker) {
    var nodes = task.TaskTimeNodes;
    var dates = $picker.datepicker("getDates");
    dates.sort(function (a, b) {
        return a - b;
    });
    var result = new Array();
    for (var i = 0; i < dates.length; i++) {
        var date = utils.ChangeDateFormat(dates[i], false);
        var obj = { TimeNode: date, TimeNodeStatus: 1, OwnTaskID: utils.getQueryString("taskId"), SubTaskID: null };
        for (var j = 0; j < timeNodes.length; j++) {
            if (utils.ChangeDateFormat(timeNodes[j].TimeNode, false) == date) {
                obj.TimeNodeStatus = timeNodes[j].TimeNodeStatus;
                obj.SubTaskID = timeNodes[j].SubTaskID;
                obj.OwnTaskID = timeNodes[j].OwnTaskID;
                obj.TimeNode = date;
                break;
            }
        }
        result.push(obj);
    }
    task.TaskTimeNodes = result;
}

var timeNodes = null;
function BindPage(data) {
    data.isMobile = utils.mobileBrower();
    data.selectedAll = '';
    data.checkedLength = 0;
    for (var i = 0; i < data.TaskTimeNodes.length; i++) {
        data.TaskTimeNodes[i].TimeNode = utils.ChangeDateFormat(data.TaskTimeNodes[i].TimeNode, false);
    }
    timeNodes = data.TaskTimeNodes;
    Vue.directive('datepck', {

        // 当绑定元素插入到 DOM 中。
        inserted: function (el) {


            setTimeout(function () {
                var times = timeNodes;
                var array = new Array();
                for (var i = 0; i < times.length; i++) {
                    array.push(utils.ChangeDateFormat(times[i].TimeNode));
                }
                var $picker = $(el)
                    .datepicker({
                        weekStart: 0,
                        maxViewMode: 2,
                        clearBtn: true,
                        language: "zh-CN",
                        multidate: true,
                        daysOfWeekHighlighted: "1,2,3,4,5",
                        todayHighlight: true
                    })
                    .datepicker('setDates', array)
                    .on("changeDate", function (e) {
                        CalcDateTable($picker);
                    })
            }, 0);
        }
    });

    data.counter = "";
    data.CreateType = "0";
    data.dialog = { show: false, search: { name: '' }, title: "选择模板", content: { data: new Array(), total: 0 } };
    data.taskDialog = { show: false, search: { name: '' }, title: "选择任务", content: { data: new Array(), total: 0 } };
    data.TaskTemplateType = Number(utils.getQueryString("TaskTemplateType"));
    task = new Vue({
        el: '#taskInfo',
        data: data,
        created: function () {
            var self = this;
            if (self.TaskType == 3 && self.TaskStatus == 0) {
                setInterval(function () {
                    self.counter = self.formatcounter(self.TaskSubmitDate);
                }, 800)
            }
            if (self.IsEdit) {
                self.loadUserNodes(utils.getQueryString("taskId"));
            }
        },
        mounted: function () {
            var self = this;
            self.CalcSelectDayCount();
            setTimeout(function () {
                $("#fileAttachment").ajaxupload({
                    formData: { "action": 'UploadTaskAttachment' },
                    paramName: "FileData",
                    url: api_url + 'api/attachments/upload/' + self.TaskID,
                    finish: function (data) {
                        self.Attachments.push(data);
                    }
                });

                $("#appenduseruploader").ImportUser({
                    businessid: self.TaskID,
                    callback: function (result) {
                        for (var i = 0; i < result.successuserlist.length; i++) {
                            var node = result.successuserlist[i];
                            self.UserNodes.push(node);
                        }
                    }
                });
            }, 0);


            //wanda_wf_tool.bindUserSelect($(".choose-member"),
            //    function (data) {
            //        var errorUsers = new Array();
            //        for (var i = 0; i < data.length; i++) {
            //            var user = data[i];
            //            for (var j = 0; j < self.UserNodes.length; j++) {
            //                var exist = self.UserNodes[j];
            //                if (exist.TaskUserLoginName == user.UserLoginID) {
            //                    errorUsers.push(user.UserLoginID);
            //                }
            //            }
            //        }
            //        if (errorUsers.length > 0) {
            //            utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
            //            return;
            //        } else {
            //            for (var i = 0; i < data.length; i++) {
            //                self.UserNodes.push({
            //                    TaskUserDeparment: data[i].UserOrgPathName,
            //                    TaskUserPosition: data[i].UserJobName,
            //                    TaskUserLoginName: data[i].UserLoginID,
            //                    TaskUserName: data[i].UserName,
            //                    ReceiveDate: data[i].null,
            //                    ApproveDate: data[i].null,
            //                    BusinessID: data[i].null
            //                });
            //            }
            //        }

            //    }, "YY_sjsj", true, true,
            //    [],
            //    [],
            //    []);
        },
        watch: {
            TaskCircleType: function (n) {
                var self = this;
                switch (n + "") {
                    case "1":
                        {
                            self.PlanStart = null;
                            self.PlanEnd = null;
                            self.PlanHour = '9:00';
                            self.PlanMinute = 0;
                        }
                        break;
                    case "2":
                        {
                            self.PlanStart = null;
                            self.PlanEnd = null;
                            self.PlanHour = '9:00';
                            self.PlanMinute = 0;
                        }
                        break;
                    case "3":
                        {
                            self.PlanStart = null;
                            self.PlanEnd = null;
                            self.PlanHour = '9:00';
                            self.PlanMinute = 0;
                        }
                        break;
                }
            },
            TaskType: function (n) {
                var self = this;
                switch (n + "") {
                    case "1":
                        self.TaskCircleType = 1;
                        break;
                    case "2":
                        self.TaskCircleType = 2;
                        break;
                    case "3":
                        self.TaskCircleType = 1;
                        break;
                }
            }
        },
        methods: {
            SelectUser: function () {
                //var self = this;
                //user_select_ctrl.show(function (data) {
                //    var errorUsers = new Array();
                //    for (var i = 0; i < data.length; i++) {
                //        var user = data[i];
                //        for (var j = 0; j < self.UserNodes.length; j++) {
                //            var exist = self.UserNodes[j];
                //            if (exist.TaskUserLoginName == user.LoginName) {
                //                errorUsers.push(user.LoginName);
                //            }
                //        }
                //    }
                //    if (errorUsers.length > 0) {
                //        utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
                //        return;
                //    } else {
                //        for (var i = 0; i < data.length; i++) {
                //            self.UserNodes.push({
                //                TaskUserDeparment: data[i].Department,
                //                TaskUserPosition: data[i].JobTitle,
                //                TaskUserLoginName: data[i].LoginName,
                //                TaskUserName: data[i].DisplayName,
                //                ReceiveDate: data[i].null,
                //                ApproveDate: data[i].null,
                //                BusinessID: data[i].null
                //            });
                //        }
                //    }
                //});



                var self = this;
                bpf_userselect_client.selectUser({
                    isNeedHiddenNav: utils.mobileBrower(),
                    topValue: 14,
                    func: function (data) {
                        var errorUsers = new Array();
                        for (var i = 0; i < data.length; i++) {
                            var user = data[i];
                            for (var j = 0; j < self.UserNodes.length; j++) {
                                var exist = self.UserNodes[j];
                                if (exist.TaskUserLoginName == user.UserLoginID) {
                                    errorUsers.push(user.UserLoginID);
                                }
                            }
                        }
                        if (errorUsers.length > 0) {
                            utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
                            return;
                        } else {
                            for (var i = 0; i < data.length; i++) {
                                self.UserNodes.push({
                                    TaskUserDeparment: data[i].UserOrgPathName,
                                    TaskUserPosition: data[i].UserJobName,
                                    TaskUserLoginName: data[i].UserLoginID,
                                    TaskUserName: data[i].UserName,
                                    ReceiveDate: data[i].null,
                                    ApproveDate: data[i].null,
                                    BusinessID: data[i].null,
                                    TaskTemplateTypeVal: data[i].TaskTemplateTypeVal,
                                    TaskTemplateType: self.TaskTemplateType,
                                    UpdateArea: "",
                                    AreaValue: ""
                                });
                            }
                        }
                    }
                });

            },
            StopPlanTask: function () {
                var self = this;
                utils.confirmMessage("是否确定进行该操作？", function (ret) {
                    if (!ret) {
                        return;
                    }
                    self.ajaxDelete({
                        url: api_url + "api/tasks/stop/" + self.TaskID,
                        success: function (result) {
                            window.location.href = window.location.href;
                        }, error: function (status, statusText, msg) {
                        }
                    });
                });
            },
            CalcTimeRangeDayCount: function (start, end) {
                if (typeof start === 'string') {
                    start = start.replace(/-/g, "/");
                    start = new Date(start);
                }
                if (typeof end === 'string') {
                    end = end.replace(/-/g, "/");
                    end = new Date(end);
                }
                //var start_date = new Date(start .replace(/-/g, "/"));
                //var end_date = new Date(end .replace(/-/g, "/"));
                //var days = end_date.getTime() - start_date.getTime();
                var days = end.getTime() - start.getTime();
                var time = parseInt(days / (1000 * 60 * 60 * 24)) + 1;
                return time;
            },
            changeStartDate: function (value) {
                var self = this;
                self.PlanStart = value;
                self.CalcSelectDayCount();
            },
            changeEndDate: function (value) {
                var self = this;
                self.PlanEnd = value;
                self.CalcSelectDayCount();
            },
            CalcSelectDayCount: function () {
                var self = this;
                $("#spncount").text("");
                var result = 0;
                var start = self.PlanStart;
                var end = self.PlanEnd;
                if (start && end) {
                    var time = self.CalcTimeRangeDayCount(start, end);
                    if (time >= 1) {
                        $("#spncount").text("已选择" + time + "天");
                        result = time;
                    }
                }
                return result;
            },
            selectTask: function () {
                var self = this;
                self.taskDialog.show = true;
                $("html,body").css({ 'overflow-y': 'hidden', 'height': '100%' });
            },
            selectAll: function () {
                var self = this;
                if (!self.selectedAll) {
                    self.checkedLength = 0;
                    $.each(self.UserNodes, function (index, item) {
                        item.checked = false;
                    });
                } else {
                    self.checkedLength = self.UserNodes.length;
                    $.each(self.UserNodes, function (index, item) {
                        item.checked = true;
                    });
                }
            },
            select: function (item) {
                var self = this,
                    chksLength = self.UserNodes.length;
                self.checkedLength = $(".selectSub:checked").length;

                if (!item.checked) {
                    self.selectedAll = false;
                }

                if (chksLength == self.checkedLength) {
                    self.selectedAll = true;
                } else {
                    self.selectedAll = false;
                }
            },
            deleteTask: function () {
                var self = this;
                var arr = [];
                var curUser = $(".selectSub:checked");
                if (curUser.length > 0) {
                    for (var i = 0; i < self.UserNodes.length; i++) {
                        if (self.UserNodes[i].checked == false) {
                            arr.push(self.UserNodes[i]);
                        }
                    }
                    self.UserNodes = arr;

                } else {
                    utils.alertMessage("你还没有选择任何人");
                }
            },
            removeAttachment: function (attachmentId) {
                var self = this;
                commons.DeleteAttachment(attachmentId, function () {
                    for (var i = 0; i < self.Attachments.length; i++) {
                        if (self.Attachments[i].ID == attachmentId) {
                            self.Attachments.splice(i, 1);
                            break;
                        }
                    }
                });
            },
            selectTaskRow: function (node) {
                var self = this;
                self.loadTask(node.TaskID, function () {
                    self.taskDialog.show = false;
                    $("html,body").css({ 'overflow-y': 'visible' });
                });
                //self.TaskTemplateName = node.TemplateName;
                //self.TaskTemplateID = node.TemplateID;
            },
            reload: function () {
                var self = this;
                self.$refs.pager.$emit("paging-first");
            },
            taskReload: function () {
                var self = this;
                self.$refs.taskPager.$emit("paging-first");
            },
            loadTask: function (taskId, callback) {
                var self = this;
                LoadData(taskId, function (data) {
                    self.PlanStart = data.PlanStart;//utils.ChangeDateFormat(data.PlanStart, false);
                    self.PlanEnd = data.PlanEnd;//utils.ChangeDateFormat(data.PlanEnd, false);
                    self.IsEdit = false;
                    self.TaskName = data.TaskName;
                    if (data.TaskType == 3) {
                        self.TaskType = 1;
                    }
                    else {
                        self.TaskType = data.TaskType;
                    }
                    self.TaskTemplateName = data.TaskTemplateName;
                    self.TaskTemplateID = data.TaskTemplateID;
                    self.IsNeedApprove = data.IsNeedApprove;
                    self.Remark = data.Remark;
                    self.TaskCircleType = task.TaskCircleType;
                    self.PlanHour = data.PlanHour;
                    self.PlanMinute = data.PlanMinute;
                    self.TaskTemplateAttachmentID = data.TaskTemplateAttachmentID;
                    self.TaskTimeNodes = data.TaskTimeNodes;
                    self.loadUserNodes(data.TaskID);
                    callback();
                });
            },
            loadTasks: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.ajaxPost({
                    url: api_url + "api/tasks/query",
                    args: {
                        "TaskTitle": self.taskDialog.search.name,
                        "TaskCreatorLoginName": null,
                        "IsOnlySelf": true,
                        "TimeRange": null,
                        "TaskType": null,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.taskDialog.content.total = res.TotalCount;
                        self.taskDialog.content.data = res.Data;

                        callback();
                    }, error: function (status, statusText, msg) {
                    }
                });
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
                        "WithImport": true,
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
            dialogSave: function () {
                var self = this;
                self.dialog.show = false;
                $("html,body").css({ 'overflow-y': 'visible' });
            },
            dialogClose: function () {
                var self = this;
                self.dialog.show = false;
                $("html,body").css({ 'overflow-y': 'visible' });
            },
            taskDialogClose: function () {
                var self = this;
                self.taskDialog.show = false;
                $("html,body").css({ 'overflow-y': 'visible' });
            },
            selectRow: function (node) {
                var self = this;
                self.TaskTemplateName = node.TemplateName;
                self.TaskTemplateID = node.TemplateID;
                self.TaskTemplateAttachmentID = node.AttachmentId;
                self.dialog.show = false;
                $("html,body").css({ 'overflow-y': 'visible' });
            },
            selectTemplate: function () {
                var self = this;
                self.dialog.show = true;
                $("html,body").css({ 'overflow-y': 'hidden', 'height': '100%' });
            },
            formatcounter: function (start) {
                var date = new Date(start);
                var t = date.getTime() - (new Date()).getTime();
                var d = 0;
                var h = 0;
                var m = 0;
                var s = 0;
                var msg = "";
                if (t <= 0) {
                    window.location.href = 'TaskInfo.aspx?taskId=' + utils.getQueryString("taskId");
                }
                d = Math.floor(t / 1000 / 60 / 60 / 24);
                h = Math.floor(t / 1000 / 60 / 60 % 24);
                m = Math.floor(t / 1000 / 60 % 60);
                s = Math.floor(t / 1000 % 60); if (h > 0 || d > 0) {
                    msg += h + (d * 24) + "小时";
                }
                msg += (m < 10 ? ("0" + m) : m) + "分";
                msg += (s < 10 ? ("0" + s) : s) + "秒";

                return "该任务将于：" + msg + " 后自动发起";
            },
            CommonSaveTask: function (action, callback) {
                var self = this;
                //bool 类型绑定到表单上以后获取的内容会有问题
                self.IsNeedApprove = self.IsNeedApprove == "1";
                var str = self.PlanHourAndMinute;
                var splits = str.split(':');
                self.PlanHour = splits[0];
                self.PlanMinute = splits[1];
                self.PlanStart = utils.ChangeDateFormat(self.PlanStart, false);
                self.PlanEnd = utils.ChangeDateFormat(self.PlanEnd, false);
                if (self.TaskTemplateType == 2 && self.UserNodes.length) {
                    for (var i = 0; i < self.UserNodes.length; i++){
                        var one = self.UserNodes[i];
                        var TaskTemplateTypeVal = one.TaskTemplateTypeVal;
                        if (!TaskTemplateTypeVal) {
                            utils.alertMessage("请设置数据区域", function () { });
                            return
                        }
                        var indexOf = TaskTemplateTypeVal.indexOf("=") + 1;
                        var AreaValueTxt = TaskTemplateTypeVal.slice(indexOf, TaskTemplateTypeVal.length).replace(/\s/g, "");
                        TaskTemplateTypeVal = TaskTemplateTypeVal.split("=");
                        var regT = /^[a-zA-Z]+列$/;
                        if (!regT.test(TaskTemplateTypeVal[0]) || indexOf <= 0) {
                            utils.alertMessage("数据区域格式不正确", function () { });
                            return
                        }
                        if (!AreaValueTxt) {
                            utils.alertMessage("数据区域值不能为空", function () { });
                            return  
                        }
                        var columnTxt = TaskTemplateTypeVal[0].match(/[a-zA-Z]+/)[0].toUpperCase();
                        if (self.ConvertCode(columnTxt) > 18278 || self.ConvertCode(columnTxt) < 1) {
                            utils.alertMessage("数据区域列数不在范围内", function () { });
                            return
                        }
                        one.UpdateArea = columnTxt;
                        one.AreaValue = AreaValueTxt;
                    }
                }
                self.ajaxPost({
                    url: api_url + 'api/tasks/' + action + '/',
                    args: self.$data,
                    success: function (result) {
                        callback(result);

                    }, error: function (status, statusText, msg) {
                    }
                });
            },
            SaveTask: function () {
                var self = this;
                self.CommonSaveTask('save', function (result) {
                    window.location.href = 'TaskAdd.aspx?taskId=' + result + '&TaskTemplateType=' + self.TaskTemplateType;
                });
            },
            SubmitTask: function () {
                var self = this;
                self.CommonSaveTask('submit', function (result) {
                    var msg = "已给" + self.UserNodes.length + "人发送待办";
                    utils.alertMessage(msg, function () {
                        window.location.href = 'TaskInfo.aspx?taskId=' + result + '&TaskTemplateType=' + self.TaskTemplateType;
                    });

                });
            },
            removeUserNode: function (user) {
                var self = this;
                for (var i = 0; i < self.UserNodes.length; i++) {
                    if (self.UserNodes[i].TaskUserLoginName == user) {
                        self.UserNodes.splice(i, 1);
                    }
                }
            },
            loadUserNodes: function (taskId) {
                var self = this;

                self.ajaxPost({
                    url: api_url + 'api/usernodes/query/',
                    args: {
                        TaskID: taskId
                    },
                    success: function (result) {
                        $.each(result.Nodes, function (index, item) {
                            item.checked = false;
                            self.UserNodes.push(item);
                        });
                    }, error: function (status, statusText, msg) {
                    }
                });

            },
            ConvertCode: function (code) {
                code = code.toUpperCase();
                var num = 0;
                num = 0;
                for (var i = code.length - 1, j = 1; i >= 0; i-- , j *= 26) {
                    num += (code[i].charCodeAt() - 64) * j;
                }
                return num;
            },
        }
    });

}
function LoadData(taskId, callback) {
    Vue.ajaxGet({
        url: api_url + 'api/tasks/load/' + taskId,
        success: function (data) {
            callback(data);
        }, error: function (status, statusText, msg) {
        }
    });
}
$(function () {
    if (utils.getQueryString("taskId").length > 0) {
        LoadData(utils.getQueryString("taskId"), function (data) {
            data.IsEdit = true;
            data.UserNodes = [];
            data.PlanStart = utils.ChangeDateFormat(data.PlanStart, false);
            data.PlanEnd = utils.ChangeDateFormat(data.PlanEnd, false);
            data.PlanHourAndMinute = data.PlanHour + ":" + (data.PlanMinute > 0 ? data.PlanMinute : ('0' + data.PlanMinute));
            BindPage(data);
        });
    } else {
        if (utils.getQueryString("templateId").length > 0) {
            Vue.ajaxGet({
                url: api_url + 'api/templates/load/' + utils.getQueryString("templateId"),
                success: function (data) {
                    defaultModel.TaskTemplateName = data.Model.TemplateName;
                    defaultModel.TaskTemplateID = data.Model.TemplateID;
                    BindPage(defaultModel);
                }, error: function (status, statusText, msg) {
                }
            });

        } else {
            BindPage(defaultModel);
        }
    }
});

$(function () {
    var businessId = utils.getQueryString("businessID");
    Task.Init(businessId);
});
var model = null;
var Task = {
    Init: function (businessId) {
        Task.LoadData(businessId, Task.BuildData);
    },
    BuildData: function (data) {
        data.TaskUploadAction = "UploadTaskData";
        data.TaskUploadUrl = api_url + 'api/todos/upload/' + data.BusinessID;
        data.TaskUploadData = { ResultType: 0, Attachment: null, Sheets: new Array(), Message: "" };

        data.UploadTaskAttachAction = "UploadTaskAttach";
        data.UploadTaskAttachUrl = api_url + 'api/attachments/upload/' + data.BusinessID;

        if (data.TaskAttachment != null) {
            var obj = { ResultType: 3, Attachment: data.TaskAttachment, Sheets: new Array() };
            data.TaskUploadData = obj;
        }

        model = new Vue({
            el: '#taskCollectionMobile',
            data: data,
            created: function () {
                var self = this;
                if (!self.IsNeedApprove) {
                    $('.bpf_workflow_result_list').hide();
                    var $container = $("#SJSJ_LC").closest(".approval-process");
                    $container.find("#bpf-wfclient-log-textarea").val("无需审批");
                    var $wfButtons = $container.find("#bpf-wfclient-button-content");
                    var $wfButtonClone = $wfButtons.clone();
                    $wfButtonClone.appendTo("body");
                    $container.hide();
                    $wfButtonClone.find(".bpf-wfclient-button-btn").click(function () {
                        var op = $(this).attr("optype");
                        $("[optype='" + op + "']", $("#bpf-wfclient-button-content")).click();
                    });
                    //    if (Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting != null) {
                    //        Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.AllowNewCC = false;
                    //        Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.ShowApprovalLog = false;
                    //        Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.ShowApprovalTextArea = false;
                    //        //Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.ShowButtonBar = false;
                    //        Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.ShowCCBar = false;
                    //        Wanda_Workflow_Model.workflowContext.CurrentUserSceneSetting.ShowNavigationBar = false;
                    //        Wanda_Workflow_Model.approveText = "无需审批"
                    //    }
                }

            },
            mounted: function () {
            },
            methods: {
                deleteAttachment: function (id) {
                    var self = this;
                    commons.DeleteAttachment(id, function () {
                        for (var i = 0; i < self.TaskReportAttachments.length; i++) {
                            if (self.TaskReportAttachments[i].ID == id) {
                                self.TaskReportAttachments.splice(i, 1);
                            }
                        }
                    });
                },
                taskattachmentuploaded: function (result) {
                    data.TaskReportAttachments.push(result);
                },
                taskdatauploaded: function (result) {
                    data.TaskUploadData.ResultType = result.ResultType;
                    data.TaskUploadData.Attachment = result.Attachment;
                    data.TaskUploadData.Message = result.Message;
                    data.TaskUploadData.Sheets = new Array();
                    if (result.Sheets != undefined && result.Sheets != null) {
                        for (var i = 0; i < result.Sheets.length; i++) {
                            data.TaskUploadData.Sheets.push(result.Sheets[i]);
                        }
                    }
                }
            }
        });

    },
    BeforeAction: function (args, func, data) {
        WFParam_SJSJ.DyRoleList = [data.ReceiveLoginName];
        WFParam_SJSJ.ApprovalRoleList = [data.AssignLoginName];
        WFOperator_SJSJ.StartPage.BeforeAction(args, func, {
            BusinessID: data.BusinessID,
            TaskName: data.TaskTitle
        });
    },
    SaveApplicationData: function (argsT, funcT) {
        WFOperator_SJSJ.StartPage.SaveAction(argsT, funcT, {
            Submit: function (args, func) { Task.Submit(args, func); setTimeout(function () { location.href = '/todoListMobile'; }, 1000) },
            Save: function (args, func) { Task.Save(args, func); setTimeout(function () { location.href = '/todoListMobile'; }, 1000) }
        });
    },
    Submit: function (args, func) {
        Task.CommonSave("submit", args, func);
    },
    CommonSave: function (action, args, func) {
        var businessID = utils.getQueryString("businessID");
        var url = api_url + 'api/todos/workflow/' + action + '/' + businessID;
        utils.ajax({
            type: 'POST',
            url: url,
            args: { BusinessID: businessID, TaskReportRemark: model.TaskReportRemark },
            success: function (data) {
                func();
            }
        });
    },
    Save: function (args, func) {
        Task.CommonSave("save", args, func);
    },
    Approve: function (args) {
        Task.CommonSave("approve", args, function () {
            window.close();
        });
    },
    Reject: function (args) {
        Task.CommonSave("reject", args, function () {
            //WFOperator_SJSJ.AfterActionRedirect(args);
            window.close();
        });
    },
    AfterAction: function (argsT) {
        if (argsT.OperatorType != 2) {
            if (argsT.WorkflowContext.ProcessInstance.Status == 3) {
                Task.Approve(argsT)
            }
            else {
                if (argsT.OperatorType == 0) {
                    return;
                }
                window.close();
            }
        }
        else {
            window.close();
        }
    },
    CheckValue: function () {
        var isPending = model.TaskUploadData.ResultType != 3;
        if (isPending) {
            utils.alertMessage("请先上传填报文件", function () {
                return false;
            });
        }
        return true;
    },
    LoadData: function (businessId, callback) {
        var url = api_url + 'api/todos/load/' + businessId;
        utils.ajax({
            type: 'GET',
            url: url,
            success: function (data) {

                //手机端填报时，提示用户在PC端操作  任务状态 0 未上报 1 审批中 2 批准 4 退回
                if (data.TaskStatus == 0 || data.TaskStatus == 4) {
                    window.location.href = 'TaskCollectionMobileNew.aspx?IsFromWeb=' + Task.getQueryStringNew("IsFromWeb");
                    return;
                }
                //工作流操作开始
                WFOperator_SJSJ.InitSetting({
                    UserSelectSetting: {
                        IsNeedHiddenNav: utils.mobileBrower(),
                        TopValue: 14
                    },
                    OnBeforeExecute: function (args, func) { Task.BeforeAction(args, func, data) },//执行前(准备参数)
                    OnSaveApplicationData: Task.SaveApplicationData,//执行中，此处进行提交业务数据
                    OnAfterExecute: Task.AfterAction,  //执行后调用（进行回滚或其它操作（例如跳转））
                    OnExecuteCheck: Task.CheckValue  //客户端JS校验
                });
                WFOperator_SJSJ.StartPage.LoadProcess({
                    ApprovalPage: "../Task/TaskCollectionView.aspx",
                    FlowCode: data.WorkflowCode
                }, function () {
                    if (callback != null && typeof callback === 'function') {
                        Task.BuildData(data);
                    }
                });
            }
        });
    },
    getQueryStringNew: function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return '';
    }
};
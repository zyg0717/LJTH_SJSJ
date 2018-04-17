$(function () {
    var businessId = utils.getQueryString("businessID");
    Task.Init(businessId);
});
var Task = {
    Init: function (businessId) {
        Task.LoadData(businessId, Task.BuildData);
    },
    BuildData: function (data) {
        $(".tc-title").text(data.TaskTitle);
        $(".tc-createdate").text(utils.ChangeDateFormat(data.AssignDate, true));
        $(".tc-createname").text(data.AssignName);
        $(".tc-remark").text(data.TaskRemark);
        if (data.TaskAttachments.length > 0) {
            $(".tc-attachments-title").removeClass("hidden");
            $(".tc-attachments").removeClass("hidden");
            $.each(data.TaskAttachments, function (i, d) {
                var $html = Task.BuildAttachItem(d);
                $(".tc-attachments").append($html);
            });
        } else {
            $(".tc-attachments-title").addClass("hidden");
            $(".tc-attachments").addClass("hidden");
        }
        if (data.TaskReportAttachments.length > 0) {
            $.each(data.TaskReportAttachments, function (i, d) {
                var $html = Task.BuildTaskReportAttachItem(d);
                $(".tc-task-report-attachments").append($html);
            });
        }

        $(".tc-template").click(function () {
            //commons.DownLoadTaskTemplate_OWA(data.BusinessID);
            commons.DownLoadTaskTemplate(data.BusinessID);
        });
        $("#fileTaskUpload").ajaxupload({
            formData: { "action": 'UploadTaskData' },
            paramName: "FileData",
            url: api_url + 'api/todos/upload/' + data.BusinessID,
            finish: function (data) {
                Task.UploadTaskFile(data);
            }
        });
        $("#attachment-file").ajaxupload({
            formData: { "action": 'UploadTaskAttach' },
            paramName: "FileData",
            url: api_url + 'api/attachments/upload/' + data.BusinessID,
            finish: function (data) {
                var $html = Task.BuildTaskReportAttachItem(data);
                $(".tc-task-report-attachments").append($html);
                Task.BuildTaskReportAttachItemLength();
            }
        });
        if (data.TaskAttachment != null) {
            var obj = { ResultType: 3, Attachment: data.TaskAttachment, Sheets: new Array() };
            Task.UploadTaskFile(obj);
        }
        $(".tc-report-remark").val(data.TaskReportRemark);
        Task.BuildTaskReportAttachItemLength();


        $("body>#wanda-wfclient-button-content").remove();
        if (!data.IsNeedApprove) {
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
        } else {
            var $container = $("#SJSJ_LC").closest(".approval-process");
            $container.show();
        }

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
            Submit: function (args, func) { Task.Submit(args, func); },
            Save: function (args, func) { Task.Save(args, func); }
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
            args: { BusinessID: businessID, TaskReportRemark: $(".tc-report-remark").val() },
            success: function (data) {
                func();
                var isPending = $(".tc-upload.result-3").hasClass("hidden")
                if (action == 'save' && data == true && !isPending) {
                    utils.alertMessage("保存成功！", function () {
                        return false;
                    });
                }
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
        var isPending = $(".tc-upload.result-3").hasClass("hidden")
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
                //工作流操作开始
                WFOperator_SJSJ.InitSetting({
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
                        //Task.BuildData(data);
                        callback(data);
                    }
                });
            }
        });
    },
    UploadTaskFile: function (data) {
        $(".tc-upload").addClass("hidden");
        $(".tc-upload-block").removeClass("hidden");
        $(".tc-sheet-length").empty();
        $(".tc-upload-result-2-msg").text("");
        switch (data.ResultType) {
            case 1:
                //使用系统模板
                {
                    $(".result-1").removeClass("hidden");
                }
                break;
            case 2:
                //验证未通过
                {
                    $(".tc-upload-result-2-msg").text(data.Message);
                    $(".result-2").removeClass("hidden");
                }
                break;
            case 3:
                //上传成功
                {
                    $(".tc-result-filename").text(data.Attachment.FileName).unbind("click").click(function () {
                        commons.DownloadAttachment(data.Attachment.ID);
                    });
                    $(".result-3").removeClass("hidden");
                    $(".tc-upload-block").addClass("hidden");
                    $.each(data.Sheets, function (i, d) {
                        var $html = $($("#script_sheet_length").html());
                        $html.text("【{0}】有效数据行：{1}".formatBy(d.SheetName, d.SheetRowLength));
                        $(".tc-sheet-length").append($html);
                    });
                }
                break;
        }
    },
    BuildAttachItem: function (attach) {
        var $html = $($("#script_attachment").html());
        $html.find(".tc-attach-item").text(attach.FileName).click(function () {
            commons.DownloadAttachment(attach.ID);
        });
        return $html;
    },
    BuildTaskReportAttachItem: function (attach) {
        var $html = $($("#script_report_attachment").html());
        $html.find(".tc-report-attachment").text(attach.FileName).click(function () {
            commons.DownloadAttachment(attach.ID);
        });
        var $del = $html.find(".tc-report-attachment-del");
        $del.click(function () {
            commons.DeleteAttachment(attach.ID, function () {
                $html.remove();
                Task.BuildTaskReportAttachItemLength();
            });
        });
        return $html;
    },
    BuildTaskReportAttachItemLength: function () {
        var attachlength = $(".tc-task-report-attachments .tc-report-attachment").length;
        if (attachlength > 0) {
            $(".tc-task-report-attachlength").text("您已选择 {0} 个附件".formatBy(attachlength));
        }
    }
};
$(function () {
    var vm = null;
    var taskFileDialog = { show: false, title: "下载汇总文件", content: { data: { total: 0, finish: 0, taskID: '', chk1: false, chk2: false, chk3: false, chk4: false } } };
    var taskTemplate = {
        TaskAttachmentCount: 0,
        TaskStatus: null,
        userNodes: [],
        taskId: utils.getQueryString("taskId"),
        count: 0,
        chkSelected: '',
        selectedAll: '',
        receiverName: '',
        checkedLength: 0,
        tabName: 'total',
        taskFileDialog: taskFileDialog,
        isMobile: utils.mobileBrower(),
        OwnerTaskID: '',
        ApproveContent: '',
        Remark: '',
        TaskType: 1,
        TaskCreatorName: '',
        TaskCreatorLoginName: '',
        TaskCreateDate: null,
        Attachments: [],
        TaskName: '',
        TaskTemplateName: '',
        TaskProcess: '',
        TaskUserNodeCount: 0,
        CompleteUserNodeCount: 0,
        Processing: '',
        TaskTemplateType: Number(utils.getQueryString("TaskTemplateType"))
    };
    vm = new Vue({
        el: "#vue",
        data: taskTemplate,
        mounted: function () {
            var self = this;
            console.log(self)
            setTimeout(function () {
                $("#appenduseruploader").ImportUser({
                    businessid: self.taskId,
                    callback: function (result) {
                        var arr = new Array();
                        for (var i = 0; i < result.successuserlist.length; i++) {
                            var node = result.successuserlist[i];
                            arr.push(node.TaskUserLoginName);
                        }
                        vm.appendUser(arr);
                        //for (var i = 0; i < result.successuserlist.length; i++) {
                        //    var node = result.successuserlist[i];
                        //    self.UserNodes.push(node);
                        //}
                    }
                });
            }, 0);
        },
        methods: {
            SelectUser: function () {
                //var self = this;

                //user_select_ctrl.show(function (data) {
                //    var errorUsers = new Array();
                //    for (var i = 0; i < data.length; i++) {
                //        var user = data[i];
                //        for (var j = 0; j < vm.userNodes.length; j++) {
                //            var exist = vm.userNodes[j];
                //            if (exist.TaskUserLoginName == user.LoginName) {
                //                errorUsers.push(user.LoginName);
                //            }
                //        }
                //    }
                //    if (errorUsers.length > 0) {
                //        utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
                //        return;
                //    } else {
                //        var arr = new Array();
                //        for (var i = 0; i < data.length; i++) {
                //            arr.push(data[i].LoginName);
                //        }
                //        vm.appendUser(arr);
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
                            for (var j = 0; j < vm.userNodes.length; j++) {
                                var exist = vm.userNodes[j];
                                if (exist.TaskUserLoginName == user.UserLoginID) {
                                    errorUsers.push(user.UserLoginID);
                                }
                            }
                        }
                        if (errorUsers.length > 0) {
                            utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
                            return;
                        } else {
                            var arr = new Array();
                            for (var i = 0; i < data.length; i++) {
                                arr.push(data[i].UserLoginID);
                            }
                            vm.appendUser(arr);
                        }
                    }
                });
            },
            downAttachmentZip: function (task) {
                commons.DownloadTaskAttachments(task.TaskID);
            },
            downTaskFileZip: function (task) {
                commons.DownloadTaskFiles(task.TaskID);
            },
            downTaskApprovedList: function (task) {
                commons.DownloadTaskApprovedList(task.TaskID);
            },
            downloadTaskFile: function (task) {
                var self = this;
                self.taskFileDialog.content.data.taskID = task.TaskID;
                self.taskFileDialog.content.data.total = task.TaskUserNodeCount;
                self.taskFileDialog.content.data.finish = task.CompleteUserNodeCount;
                self.taskFileDialog.show = true;
            },
            closeTaskFile: function () {
                var self = this;
                self.taskFileDialog.show = false;
            },
            download: function () {
                var self = this;
                self.closeTaskFile();
                commons.DownloadSummary(
                    self.taskFileDialog.content.data.taskID
                    , self.taskFileDialog.content.data.chk1 ? 1 : 0
                    , self.taskFileDialog.content.data.chk2 ? 1 : 0
                    , self.taskFileDialog.content.data.chk3 ? 1 : 0
                    , self.taskFileDialog.content.data.chk4 ? 1 : 0
                    );
            },
            preview: function () {
                var self = this;
                self.closeTaskFile();
                commons.PreviewSummary(
                    self.taskFileDialog.content.data.taskID
                    , self.taskFileDialog.content.data.chk1 ? 1 : 0
                    , self.taskFileDialog.content.data.chk2 ? 1 : 0
                    , self.taskFileDialog.content.data.chk3 ? 1 : 0
                    , self.taskFileDialog.content.data.chk4 ? 1 : 0
                );
            },
            select: function (item) {
                var self = this,
                    chksLength = self.userNodes.length;
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
            resendTask: function (tabName, taskId, userName) {
                var self = this;
                var arr = [];
                var curUser = $(".selectSub:checked");
                if (curUser.length > 0) {
                    $.each(curUser, function (index, item) {
                        arr.push(item.value);
                    });
                    self.ajaxPost({
                        url: api_url + "api/usernodes/repeat/" + taskId,
                        args: arr,
                        success: function (data) {
                            if (data) {
                                utils.alertMessage('重发成功', function () {
                                    vm.getUserNodes(tabName, taskId, userName);
                                });

                            }
                        }, error: function (status, statusText, msg) {
                            utils.alertMessage('提交错误');
                        }
                    });

                } else {
                    utils.alertMessage("你还没有选择任何人");
                }
            },
            deleteTask: function (tabName, taskId, userName) {
                var self = this;
                var arr = [];
                var curUser = $(".selectSub:checked");
                if (curUser.length > 0) {
                    $.each(curUser, function (index, item) {
                        arr.push(item.value);
                    });

                    self.ajaxPost({
                        url: api_url + "api/usernodes/remove/" + taskId,
                        args: arr,
                        success: function (data) {
                            if (data) {
                                utils.alertMessage('删除成功', function () {
                                    vm.getUserNodes(tabName, taskId, userName);
                                });
                            }
                        }, error: function (status, statusText, msg) {
                            //alert('提交错误');
                        }
                    });

                } else {
                    utils.alertMessage("你还没有选择任何人");
                }
            },
            downloadSummary: function (templateId) {
                commons.DownloadSummary(templateId);
            },
            calcProcess: function (Total, Pending) {
                var self = this;
                self.TaskUserNodeCount = Total;
                self.CompleteUserNodeCount = Total - Pending;
                var percentPending = (((Total - Pending) / Total) * 100).toFixed(0);
                var percentFinish = 100 - percentPending;
                self.TaskProcess = percentPending + '%';
                self.Processing = percentFinish + '%';
            },
            getTaskTemplate: function (taskId) {
                var self = this;
                self.ajaxGet({
                    url: api_url + "api/tasks/load/" + taskId,
                    success: function (result) {
                        if (result.IsNeedApprove) {
                            result.ApproveContent = "需要审批流程";
                        } else {
                            result.ApproveContent = "不需要审批流程";
                        }
                        result.TaskCreateDateFormat = utils.ChangeDateFormat(result.TaskCreateDate, true);
                        result.length = result.Attachments.length;
                        self.taskTemplate = result;

                        self.TaskAttachmentCount = result.TaskAttachmentCount;
                        self.TaskStatus = result.TaskStatus;
                        self.OwnerTaskID = result.OwnerTaskID;
                        self.ApproveContent = result.ApproveContent;
                        self.Remark = result.Remark;
                        self.TaskCreatorName = result.TaskCreatorName;
                        self.TaskCreatorLoginName = result.TaskCreatorLoginName;
                        self.TaskCreateDate = result.TaskCreateDate;
                        self.TaskName = result.TaskName;
                        self.TaskTemplateName = result.TaskTemplateName;
                        self.TaskID = result.TaskID;
                        self.TaskType = result.TaskType;
                        self.TaskTemplateAttachmentID = result.TaskTemplateAttachmentID;
                        self.TaskUserNodeCount = result.TaskUserNodeCount;
                        self.CompleteUserNodeCount = result.CompleteUserNodeCount;
                        self.Attachments = [];
                        for (var i = 0; i < result.Attachments.length; i++) {
                            self.Attachments.push(result.Attachments[i]);
                        }
                        self.length = self.Attachments.length;
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });

            },
            getUserNodes: function (tabName, taskId, userName) {
                var self = this;
                self.selectedAll = false;
                self.chkSelected = false;
                self.checkedLength = 0;
                var status = $("." + tabName).find("input[type=hidden]").val();

                self.ajaxPost({
                    url: api_url + "api/usernodes/query",
                    args: {
                        'LoginOrName': userName,
                        'Status': status,
                        'TaskID': taskId
                    },
                    success: function (result) {
                        self.userNodes = [];
                        if (result.Nodes.length > 0) {
                            $.each(result.Nodes, function (index, item) {
                                item.checked = false;
                                self.userNodes.push(item);
                            });
                        }
                        self.calcProcess(result.Total, result.Pending);
                    }, error: function (status, statusText, msg) {
                    }
                });
            },
            selectAll: function () {
                var self = this;
                if (!self.selectedAll) {
                    self.checkedLength = 0;
                    $.each(self.userNodes, function (index, item) {
                        item.checked = false;
                    });
                } else {
                    self.checkedLength = self.userNodes.length;
                    $.each(self.userNodes, function (index, item) {
                        item.checked = true;
                    });
                }
            },
            downloadTemplate: function (templateID) {
                var templateId = utils.getQueryString(templateID);
                commons.DownloadTemplate(templateId);
            },
            downLoadAtttachment: function (attachmentId) {
                commons.DownloadAttachment(attachmentId);
            },
            searchReceiverName: function (tabName, taskId, keyWord) {
                var self = this;
                self.getUserNodes(tabName, taskId, keyWord);
            },
            stopEditTask: function (taskId) {
                var self = this;
                utils.confirmMessage("是否确定进行该操作？", function (ret) {
                    if (!ret) {
                        return;
                    }
                    self.ajaxDelete({
                        url: api_url + "api/tasks/stop/" + taskId,
                        //args: {
                        //    'LoginOrName': keyWord,
                        //    'Status': status,
                        //    'TaskID': taskId
                        //},
                        success: function (result) {
                            window.location.href = window.location.href;
                        }, error: function (status, statusText, msg) {
                        }
                    });
                });

            },
            appendUser: function (users) {
                if (users.length == 0) {
                    utils.alertMessage("请至少追加一个节点", function () {

                    });
                    return;
                }
                Vue.ajaxPost({
                    url: api_url + "api/usernodes/append/" + vm.taskId,
                    args: users,
                    success: function (result) {
                        utils.alertMessage('追加成功', function () {
                            vm.getUserNodes(vm.tabName, vm.TaskID, vm.receiverName);
                        });
                    }, error: function (status, statusText, msg) {
                    }
                });
            }
        }
    });

    vm.getTaskTemplate(vm.taskId);
    vm.getUserNodes(vm.tabName, vm.taskId, vm.TaskCreatorLoginName);

    //wanda_wf_tool.bindUserSelect($(".add-member"),
    //            function (data) {
    //                var errorUsers = new Array();
    //                for (var i = 0; i < data.length; i++) {
    //                    var user = data[i];
    //                    for (var j = 0; j < vm.userNodes.length; j++) {
    //                        var exist = vm.userNodes[j];
    //                        if (exist.TaskUserLoginName == user.UserLoginID) {
    //                            errorUsers.push(user.UserLoginID);
    //                        }
    //                    }
    //                }
    //                if (errorUsers.length > 0) {
    //                    utils.alertMessage("用户：" + errorUsers.join('、') + "重复添加");
    //                    return;
    //                } else {
    //                    var arr = new Array();
    //                    for (var i = 0; i < data.length; i++) {
    //                        arr.push(data[i].UserLoginID);
    //                    }
    //                    vm.appendUser(arr);
    //                }

    //            }, "YY_sjsj", true, true,
    //            [],
    //            [],
    //            []);
});
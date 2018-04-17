$(function () {
    $("#navbar-menu li").removeClass('active').filter('.delivery-list').addClass("active");
    var taskFileDialog = { show: false, title: "下载汇总文件", content: { data: { total: 0, finish: 0, taskID: '', chk1: false, chk2: false, chk3: false, chk4: false } } };

    var vm = new Vue({
        el: "#vue",
        data: {
            tab: false,
            taskFileDialog: taskFileDialog,
            taskList: [],
            keyWord: null,
            TimeRange: "",
            currentType: 1,
            total: 0,
            loading: false,
            pageSize: 10,
            status: '',
            isPrivate: true,
            currentTypeValue: 1,
            isMobile: utils.mobileBrower(),
            isLoading: false,
            isTask: false
        },
        methods: {
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
            FilingTask: function (taskId) {
                var self = this;
                utils.confirmMessage("是否确定进行该操作？", function (ret) {
                    if (!ret) {
                        return;
                    }
                    self.ajaxPost({
                        url: api_url + "api/tasks/filing/" + taskId,
                        success: function (res) {
                            utils.alertMessage('归档成功', function () {
                                self.reload();
                            });
                        }, error: function (status, statusText, msg) {
                            //alert('提交错误');
                        }
                    });
                });

            },
            search: function () {
                var t = "";
                var self = this;
                switch (self.status) {
                    case "":
                        t = 'total';
                        break;
                    case "0":
                        t = 'created';
                        break;
                    case "1":
                        t = 'processing';
                        break;
                    case "3":
                        t = 'completed';
                        break;
                    case "2":
                        t = 'terminated';
                        break;
                }
                self.reload(t);
            },
            reload: function (tabName) {
                var self = this;
                if (tabName) {
                    self.status = $("." + tabName).find("input[type=hidden]").val();
                } else {
                    self.status = '';
                }
                self.taskList = [];

                self.$refs.pager.$emit("paging-first");
                self.currentTypeValue = self.currentType;
            },
            load: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.tab = false;
                self.isLoading = true;
                self.ajaxPost({
                    url: api_url + "api/tasks/query",
                    args: {
                        "TaskTitle": self.keyWord,
                        "TaskCreatorLoginName": null,
                        "IsOnlySelf": self.isPrivate,
                        "TimeRange": self.TimeRange,
                        "TaskType": self.currentType,
                        "TaskStatus": self.status,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.total = res.TotalCount;
                        self.isLoading = false;
                        //如果不是滚动分页模式 要清空原有数据
                        if (!isScroll) {
                            self.taskList = res.Data;
                        } else {
                            $.each(res.Data, function (i, item) {
                                self.taskList.push(item);
                            });
                        }
                        self.tab = true;
                        if (callback != undefined && typeof callback === 'function') {
                            callback();
                        }
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                        self.isLoading = false;

                    }
                });

            },
            taskMask: function (event) {
                console.log(event);
            }
        }
    });
});
$(function () {
    $("#navbar-menu li").removeClass('active').filter('.my-fill').addClass("active");
    var todoList = new Vue({
        el: "#todoList",
        data: {
            todos: [],
            keyWord: '',
            selectedOption: ' ',
            taskType: '1',
            total: 0,
            loading: false,
            isLoading: false,
            pageSize: 10,
            isMobile: utils.mobileBrower()
        },
        methods: {
            reload: function () {
                var self = this;
                self.todos = [];
                self.$refs.pager.$emit("paging-first");
            },
            load: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.isLoading = true;
                self.ajaxPost({
                    url: api_url + "api/todos/query",
                    args: {
                        "TaskType": self.taskType,
                        "TaskTitle": self.keyWord,
                        "TimeRange": self.selectedOption,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.total = res.TotalCount;
                        utils.log(res);
                        self.isLoading = false;
                        //如果不是滚动分页模式 要清空原有数据
                        if (!isScroll) {
                            self.todos = res.Data;
                        } else {
                            $.each(res.Data, function (i, item) {
                                self.todos.push(item);
                            });
                        }
                        if (callback != undefined && typeof callback === 'function') {
                            callback();
                        }
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                        self.isLoading = false;
                    }
                });
            }

        }
    });
});
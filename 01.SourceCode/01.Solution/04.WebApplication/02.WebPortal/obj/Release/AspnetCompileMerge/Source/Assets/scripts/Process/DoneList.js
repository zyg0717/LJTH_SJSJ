$(function () {
    $("#navbar-menu li").removeClass('active').filter('.my-fill').addClass("active");
    var doneList = new Vue({
        el: "#doneList",
        data: {
            doneList: [],
            keyWord: '',
            selectedOption: ' ',
            total: 0,
            loading: false,
            pageSize: 10
        },
        methods: {
            reload: function () {
                var self = this;
                self.doneList = [];
                self.$refs.pager.$emit("paging-first");
            },
            load: function (isScroll, pageIndex, pageSize, callback) {
                var self = this;
                self.ajaxPost({
                    url: api_url + "api/todos/query",
                    args: {
                        "TaskType": 2,
                        "TaskTitle": self.keyWord,
                        "TimeRange": self.selectedOption,
                        "PageIndex": pageIndex,
                        "PageSize": pageSize
                    },
                    success: function (res) {
                        self.total = res.TotalCount;
                        utils.log(res);
                        //如果不是滚动分页模式 要清空原有数据
                        if (!isScroll) {
                            self.doneList = res.Data;
                        } else {
                            $.each(res.Data, function (i, item) {
                                self.doneList.push(item);
                            });
                        }
                        if (callback != undefined && typeof callback === 'function') {
                            callback();
                        }
                    }, error: function (status, statusText, msg) {
                        //alert('提交错误');
                    }
                });
            }

        }
    });
});
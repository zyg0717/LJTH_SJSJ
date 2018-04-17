var defaultScrollCount = 3;
Vue.component('vue-pager', {
    template: "#pager",
    props: ['pageSize', 'total', 'loading', 'scroll'],
    data: function () {
        var self = this;
        return {
            pageIndex: 1,
            gotoPageIndex: 1,
            scrollCount: defaultScrollCount
        };
    },
    created: function () {
        var self = this;

        if (self.scroll) {
            $(window).scroll(function () {
                var srollPos = $(window).scrollTop(); //滚动条距顶部距离(页面超出窗口的高度)  
                var totalheight = parseFloat($(window).height()) + parseFloat(srollPos);
                var range = 50;
                if (($(document).height() - range) < totalheight && self.pageIndex < self.pagerCount) {
                    if (!self.loading && self.total > 0 && self.isScroll) {
                        var newPageIndex = self.pageIndex + 1;
                        self.paging(newPageIndex);
                    }
                }
            });
        }

        self.$on('paging-first', function () {
            self.firstPaging();
        });
        self.firstPaging();
    },
    methods: {
        gotoPage: function () {
            var self = this;
            //var goto = self.gotoPageIndex;
            var goto = self.pageIndex;
            var rex = /^[0-9]+$/;
            if (!rex.test(goto)) {
                goto = 1;
            }
            else {
                goto = parseInt(goto);
            }
            if (goto < 1) {
                goto = 1;
            }
            if (goto > self.pagerCount) {
                goto = self.pagerCount;
            }
            //self.gotoPageIndex = goto;
            self.pageIndex = goto;
            //self.paging(self.gotoPageIndex);
            self.paging(self.pageIndex);
        },
        firstPaging: function () {
            var self = this;
            self.scrollCount = defaultScrollCount;
            self.paging(1);
        },
        paging: function (pageIndex) {
            var self = this;
            self.loading = true;
            self.$emit("pager", self.isScroll, pageIndex, self.pageSize, function () {
                if (!self.isScroll) {
                    setTimeout(function () {
                        $('html,body').animate({ scrollTop: '0px' }, 300);
                    }, 0);
                }
                self.pageIndex = pageIndex;
                self.scrollCount = self.scrollCount - 1;
                self.loading = false;
            });
        }
    },
    computed: {
        isScroll: function () {
            return this.scrollCount > 0 && this.scroll == true;
        },
        pagerCount: function () {
            var fix = this.total % this.pageSize > 0 ? 1 : 0;
            return parseInt(this.total / this.pageSize) + fix;
        },
        isPrevDisabled: function () {
            return this.pageIndex == 1;
        },
        isNextDisabled: function () {
            return this.pageIndex == this.pagerCount;
        },
        isFirst: function () {
            return this.pageIndex > 1;
        },
        isLast: function () {
            return this.pageIndex < this.pagerCount;
        },
        isPrevious: function () {
            return this.pageIndex > 1;
        },
        isNext: function () {
            return this.pageIndex < this.pagerCount;
        }
    }
});


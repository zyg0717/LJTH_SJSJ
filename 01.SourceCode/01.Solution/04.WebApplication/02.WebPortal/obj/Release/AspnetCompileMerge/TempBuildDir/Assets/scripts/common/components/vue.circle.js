
Vue.component('vue-circle', {
    template: "#circle",
    props: ['completeCount', 'sumCount', 'status','taskId','type','isMobile'],
    created: function () {
        var self = this;
        self.drawCircle();
    },
    updated: function () {
        var self = this;
        self.destory();
        self.drawCircle();
    },
    destoryed: function () {
        var self = this;
        self.distory();
    },
    methods: {
        drawCircle: function () {
            var self = this;
            var color = "#5cb85c";
            if (self.status == 2) {
                color = "#f63146";
            }
            setTimeout(function () {
                $(self.$el).svgCircle({
                    parent: $(self.$el)[0],
                    w: 80,
                    R: 34,
                    sW: 6,
                    color: [color, color, color],
                    perent: [100, self.percent],
                    speed: 150,
                    delay: 400,
                    bgColor: self.status == 2 ? '#f63146' : ''
                })
            }, 0);
        },
        destory: function () {
            var self = this;
            setTimeout(function () {
                $(self.$el).find("svg").remove();
            }, 0);
        },
        SubmitTask: function () {
            var self = this;
            utils.confirmMessage("是否确定发起流程？", function (ret) {
                if (!ret) {
                    return;
                }
                window.location.href = '/Application/Task/TaskAdd.aspx?taskId=' + self.taskId;
            });
        },
    },
    computed: {
        percent: function () {
            var ProcessCount = 0;
            var self = this;
            if (self.sumCount != 0 && self.completeCount != 0) {
                ProcessCount = (self.completeCount / self.sumCount) * 100;
            } else {
                ProcessCount = 0;
            }
            return ProcessCount.toFixed(0);
        }
    }
});
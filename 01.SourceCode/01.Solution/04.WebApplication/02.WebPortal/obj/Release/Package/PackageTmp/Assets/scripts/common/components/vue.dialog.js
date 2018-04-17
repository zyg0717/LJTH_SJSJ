
Vue.component('vue-dialog', {
    template: "#dialog",
    props: ['title'],
    data: function () {
        return {
            show: false
        };
    },
    created: function () {
        var self = this;
        self.$on("open", function () {
            self.init();
            self.open();
        });
    },
    methods: {
        save: function () {
            var self = this;
            self.$emit("save", self);
            self.show = false;
        },
        close: function () {
            var self = this;
            self.$emit("close", self);
            self.show = false;
        },
        init: function () {
            var self = this;
            self.$emit("init", self);
        },
        open: function () {
            var self = this;
            self.show = true;
        }
    }
});


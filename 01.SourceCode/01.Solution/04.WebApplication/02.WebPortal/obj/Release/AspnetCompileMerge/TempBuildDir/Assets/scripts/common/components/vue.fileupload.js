Vue.component('vue-fileupload', {
    props: ['id','action','url'],
    template: '#fileupload-template',
    mounted: function () {
        var vm = this;
        var $uploader = $(vm.$el).ajaxupload({
            formData: { "action": vm.action },
            paramName: "FileData",
            url: vm.url,
            finish: function (data) {
                vm.$emit('uploaded', data);
            }
        });
    },
    watch: {
    },
    destroyed: function () {
    }
});
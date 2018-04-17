Vue.component('vue-select', {
    props: ['options', 'value'],
    template: '#bootstrap-select',
    mounted: function () {
        var vm = this;

        $(this.$el)
          .val(this.value)
          // init select2
          .selectpicker()
          // emit event on change.
          .on('change', function () {
              vm.$emit('input', this.value)
          })
    },
    watch: {
        value: function (value) {
            // update value
            $(this.$el).selectpicker('val', value)
        },
        options: function (options) {
            var self = this;
            self.options = [];
            for (var i = 0; i < options.length; i++) {
                self.options.push(options[i]);
            }
            // update options
            //$(this.$el).select2({
            //    data: options,
            //})
        }
    },
    destroyed: function () {
        $(this.$el).off().selectpicker('destroy')
    }
});
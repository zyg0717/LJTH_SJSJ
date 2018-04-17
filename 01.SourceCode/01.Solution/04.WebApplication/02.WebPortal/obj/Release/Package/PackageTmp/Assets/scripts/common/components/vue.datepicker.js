Vue.component('vue-datepicker', {
    props: [ 'value', 'placeholder'],
    template: '#datepicker-template',
    mounted: function () {
        var vm = this;
       
        $(vm.$el)
          .val(vm.value)
        var $picker = $(vm.$el)
         .datepicker({
             weekStart: 0,
             maxViewMode: 2,
             clearBtn: false,
             language: "zh-CN",
             multidate: false,
             daysOfWeekHighlighted: "1,2,3,4,5",
             todayHighlight: true,
             autoclose: true,
             format: "yyyy-mm-dd"
         })
         .on("changeDate", function (e) {
             //this.value = e.data;
             vm.$emit('changedate',e.date);
             //CalcSelectDayCount();
             //alert('change date');
         });
          // init select2
          //.select2({ data: this.options })
          //// emit event on change.
          //.on('change', function () {
          //    vm.$emit('input', this.value)
          //})
    },
    watch: {
        value: function (value) {
            // update value
            var self = this;
            $(self.$el).datepicker('update', value);
        }
    },
    destroyed: function () {
        //$(this.$el).off().select2('destroy')
        var self = this;
        $(self.$el).datepicker('destroy');
    }
});
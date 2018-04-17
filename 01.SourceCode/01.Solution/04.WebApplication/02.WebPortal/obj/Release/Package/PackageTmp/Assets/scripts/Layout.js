var common_header = new Vue({
    el: "#header",
    data: {
        isMobile: utils.mobileBrower()
    }
});

var common_footer = new Vue({
    el: "#footer",
    data: {
        isMobile: utils.mobileBrower()
    }
});

var back_top = new Vue({
    el: "#navigate-top",
    data: {
        isMobile: utils.mobileBrower()
    }
});
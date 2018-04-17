var vm = new Vue({
    el: "#preview",
    data: {
        isMobile: utils.mobileBrower(),
        DocumentFileName: null,
        DocumentByteLength: null,
        DocumentLength: '',
        DocumentLink: null,
        IframeLink: null,
        BusinessID: null,
        isPreview: false,
        kb: 1024
    },
    methods: {
        getPreviewInfo: function () {
            var self = this;
            Vue.ajaxGet({
                url: api_url + 'api/wopi/files/preview/' + utils.getQueryString("businessId"),
                success: function (result) {

                    self.DocumentLink = result.DocumentLink;
                    self.DocumentFileName = result.DocumentFileName;
                    self.DocumentByteLength = (result.DocumentByteLength / 1024) + "KB";
                    self.BusinessID = result.BusinessID;
                    self.DocumentLength = result.DocumentLength;

                }, error: function (status, statusText, msg) {
                    utils.alertMessage("获取文档预览信息失败", function () {

                    });
                }
            });
        },
        downLoadAttachment: function (businessId) {
            commons.DownloadAttachment(businessId);
        },
        previewAttachment: function () {
            var self = this;
            self.isPreview = true;
            self.IframeLink = self.DocumentLink;
        }
    }
});

vm.getPreviewInfo();
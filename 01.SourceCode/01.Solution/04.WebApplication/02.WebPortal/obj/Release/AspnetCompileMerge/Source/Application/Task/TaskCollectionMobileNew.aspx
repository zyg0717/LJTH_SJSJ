<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
    <link href="../../Assets/styles/main.css" rel="stylesheet" />
    <title></title>
    <style>
        body,html{
            width: 100%;
            height: 100%;
            background: #fff;
        }
        .vue-message{
            top: 45%;
        }
    </style>
</head>
<body>
    <section>
        <div class="weui-mask" style="display: block;"></div>
        <div class="vue-message" style="display: block;">
            <div class="dialog__hd">
                <span class="dialog__title">
                    <img src="../../Assets/images/action-icon.png" width="18">
                    提示
                </span>
            </div>
            <div class="weui-dialog__bd dialog__bd">
                <div>请回PC端处理</div>
            </div>
            <div class="weui-dialog__ft dialog__ft">
                <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" id="back">确定</a>
            </div>
        </div>
    </section>
</body>
<script src="/Assets/vendors/jquery/jquery.min.js"></script>
<script type="text/javascript">
    jQuery(function () {
        $('#back').off('click').on('click', function () {
            var isFromWeb = getQueryStringNew("IsFromWeb");
            if (isFromWeb && isFromWeb > 0) {
                window.location.href = '/Application/Task/TaskList.aspx';
            }
            else {
                window.location.href = '/todoListMobile.html';
            }
        })
    })
    function getQueryStringNew(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return '';
    }
</script>
</html>

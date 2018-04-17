<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.Master" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="WebApplication.WebPortal.Application.OWA.Preview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="preview">
        <div class="pc-preview" v-if="!isMobile">        
            <div id="preview-cont" v-cloak>
                <iframe id="ifrm" v-bind:src="DocumentLink"></iframe>
            </div>
            <div class="task-footer">
                <div class="task-footer-btn text-center clear">
                    <button class="btn btn-blue preview-btn" id="btn_download" type="button" v-on:click="downLoadAttachment(BusinessID)">下载</button>
                </div>
            </div>
        </div>
        <div class="mobile-preview" v-if="isMobile">   
            <div v-show="!isPreview" class="vm-attachments" v-cloak>
                <div class="vm-attachments-info text-center">
                    <img class="mgb10" style="cursor:pointer" src="<%=ResolveUrl("~/Assets/images/shenpiexcel.png") %>" alt="" />
                    <p class="mgb10" v-text="DocumentFileName"></p>
                    <div class="vm-attachment-actions">
                        <span class="btn btn-white btn-view" v-on:click="previewAttachment()">预览</span>
                        <span class="btn btn-blue preview-btn" v-on:click="downLoadAttachment(BusinessID)" v-text="'下载 ('+ DocumentLength + ')'"></span>
                    </div>
                </div>
            </div>    
            <div v-show="isPreview" v-cloak>
                <div id="preview-cont">
                    <iframe id="ifrm" v-bind:src="IframeLink"></iframe>
                </div>
                <div class="task-footer">
                    <div class="task-footer-btn text-center clear">
                        <button class="btn btn-blue preview-btn" id="btn_download" type="button" v-on:click="downLoadAttachment(BusinessID)">下载</button>
                    </div>
                </div>
           </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/OWA/Preview.js") %>"></script>
    <script defer="defer" type="text/javascript">
        var total = $(document).height();
        var pdTop = parseInt($("#content").css("paddingTop")),
            pdBottom = parseInt($("#content").css("paddingBottom"));
        var currentHeight = (total - pdTop - pdBottom) + "px";
        $("#preview-cont").css({ height: currentHeight });

        $(function () {
            var timer = null;
            $(window).resize(function () {
                clearTimeout(timer);
                timer = setTimeout(function () {
                    $("#preview-cont").css({ height: "0px" });
                    var total = $(document).height();
                    var pdTop = parseInt($("#content").css("paddingTop")),
                        pdBottom = parseInt($("#content").css("paddingBottom"));
                    var height = total - pdTop - pdBottom;
                    var currentHeight = height + "px";
                    $("#preview-cont").css({ height: currentHeight });
                }, 1);
            });
        });
    </script>
</asp:Content>

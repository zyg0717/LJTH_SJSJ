<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/CollectionLayoutMobile.Master" AutoEventWireup="true" CodeBehind="TaskCollectionViewMobile.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskCollectionViewMobile" %>



<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="taskCollectionViewMobile" class="white-bg clear" v-cloak>
        <div class="white-wrap col-md-12 col-sm-12">
            <!-- 标题 top -->
            <div class="collection-top text-center">
                <h1 class="collection-t" v-text="TaskTitle"></h1>
                <div class="collection-time clearfix">
                    <span class="title-key">发起时间：<span class="title-val" v-text="utils.ChangeDateFormat(AssignDate, true)"></span></span>
                    <span class="title-key">下发人：<span class="title-val" v-text="AssignName"></span></span>
                </div>
            </div>

            <!-- 正文 content -->
            <div class="collection-cont col-md-12 col-sm-12">
                <ul>
                    <li class="active">
                        <h3>任务要求<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <p v-text="TaskRemark"></p>
                        </div>
                    </li>
                    <li class="active" v-if="TaskAttachments.length>0">
                        <h3>任务相关附件<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <p v-for="attachment in TaskAttachments"><a class="attachment-t" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(attachment.ID)" v-text="attachment.FileName"></a></p>
                        </div>
                    </li>

                    <li class="active" v-if="TaskAttachment!=null">
                        <h3>填报文件<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <img class="attachment-img" src="/Assets/images/shenpiexcel.png" alt="">
                            <div class="attachment-info">
                                <p>附件名称：<span class="val" v-text="TaskAttachment.FileName"></span></p>
                                <p>上传时间：<span class="val" v-text="utils.ChangeDateFormat(TaskAttachment.CreateDate, true)"></span></p>
                                <p>上传人员：<span class="val" v-text="TaskAttachment.CreateName+'（'+ TaskAttachment.CreateLoginName+'）'"></span></p>
                            </div>
                            <span class="attachment-view" v-on:click="commons.DownloadAttachment(TaskAttachment.ID)">下载（<span v-text="TaskAttachment.FileSize"></span>）</span>
                        </div>
                    </li>

                    <li class="active">
                        <h3>填报说明<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <%--v-text="TaskReportRemark"--%>
                            <p v-text="TaskReportRemark">
                            </p>
                            <div class="des-attachment" v-if="TaskReportAttachments.length>0">
                                <span>相关附件:</span>
                                <div class="des-attachment-info">
                                    <span class="attachment-length" v-if="TaskReportAttachments.length>0" v-text="TaskReportAttachments.length+'个附件'"></span>
                                    <p v-for="attachment in TaskReportAttachments">
                                        <a class="attachment-t" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(attachment.ID)" v-text="attachment.FileName"></a>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
            <!-- 正文 content end -->

        </div>
    </div>

    <div id="SJSJ_LC"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskCollectionViewMobile.js?v=")+System.Guid.NewGuid() %>"></script>


</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/CollectionLayoutMobile.Master" AutoEventWireup="true" CodeBehind="TaskCollectionMobile.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskCollectionMobile" %>






<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="taskCollectionMobile" class="white-bg clear" v-cloak>
        <div class="white-wrap col-md-12 col-sm-12">
            <!-- 标题 top -->
            <div class="collection-top col-md-12 col-sm-12 text-center">
                <h1 class="collection-t" v-text="TaskTitle"></h1>
                <div class="collection-time clearfix">
                    <span class="title-key">发起时间：<span class="title-val" v-text="utils.ChangeDateFormat(AssignDate, true)"></span></span>
                    <span class="title-key">下发人：<span class="title-val" v-text="AssignName"></span></span>
                </div>
            </div>

            <!-- 正文 content -->
            <div class="collection-cont collection-pdb col-md-12 col-sm-12">
                <ul>
                    <li class="active">
                        <h3>任务要求<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <p v-text="TaskRemark">
                            </p>
                        </div>
                    </li>
                    <li class="active" v-if="TaskAttachments.length>0">
                        <h3>任务相关附件<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <p v-for="attachment in TaskAttachments"><a class="attachment-t" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(attachment.ID)" v-text="attachment.FileName"></a></p>
                        </div>
                    </li>
                </ul>
                <div class="collections clear">
                    <dl class="col-sm-12 col-md-4 collection-step text-center">
                        <dt class="mgb15">
                            <img src="/Assets/images/num1.png" alt="" />
                            <img src="/Assets/images/down1.png" alt="" />
                        </dt>
                        <dd>
                            <button type="button" class="btn-template download-template" v-on:click="commons.DownLoadTaskTemplate(BusinessID)">下载模板</button>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>点击上面按钮下载【填报模板Excel文件】</p>
                            <p>请选择保存至电脑</p>
                        </dd>
                    </dl>

                    <dl class="col-sm-12 col-md-4 collection-step text-center">
                        <dt class="mgb15">
                            <img src="/Assets/images/num2.png" alt="" />
                            <img src="/Assets/images/leave.png" alt="" />
                        </dt>
                        <dd>
                            <button type="button" class="btn-template line-template" disabled="disabled">线下填报</button>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>打开下载的【填报模板Excel文件】</p>
                            <p>填报数据后保存关闭</p>
                            <p>(表格列和页面格式不允许修改)</p>
                        </dd>
                    </dl>

                    <dl class="col-sm-12 col-md-4 collection-step text-center">
                        <dt class="mgb15">
                            <img src="/Assets/images/num3.png" alt="" />
                            <img src="/Assets/images/up.png" alt="" />
                        </dt>
                        <dd>
                            <div class="upload btn-template upload-template">
                                <span>上传数据</span>
                                <vue-fileupload id="fileTaskUpload" v-bind:action="TaskUploadAction" v-bind:url="TaskUploadUrl" v-on:uploaded="taskdatauploaded"></vue-fileupload>
                            </div>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>点击上面按钮上传</p>
                            <p>【填报完成的Excel文件】</p>
                        </dd>
                    </dl>
                </div>
                <div class="tipsbox" v-if="TaskUploadData.ResultType==0">
                    <p class="tips-t">系统提示!</p>
                    <p class="tips-con">请先下载模版，填写并上传数据后，再进行后续操作。</p>
                </div>
                <div class="tipsbox tipsfail" v-if="TaskUploadData.ResultType==1">
                    <p class="tips-t">上传失败!</p>
                    <p class="tips-con">请使用系统下载的文件上传。</p>
                </div>
                <div class="tipsbox tipsfail" v-if="TaskUploadData.ResultType==2">
                    <p class="tips-t">上传失败!</p>
                    <p class="tips-con" v-text="TaskUploadData.Message"></p>
                </div>
                <div class="tipsbox" v-if="TaskUploadData.ResultType==3">
                    <p class="tips-t">上传成功!</p>
                    <p class="tips-con">
                        点击链接查看已上传成功的文件，如果要修改，请修改文件后重新上传。
                     <a class="attachment-t" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(TaskUploadData.Attachment.ID)" v-text="TaskUploadData.Attachment.FileName"></a>

                    </p>
                    <p class="tips-con sheet-length">
                        <span v-for="sheet in TaskUploadData.Sheets">
                            <span v-text="'【{0}】有效数据行：{1}'.formatBy(sheet.SheetName, sheet.SheetRowLength)"></span>
                        </span>
                    </p>
                </div>
                <ul>
                    <li class="active">
                        <div class="mask" v-if="TaskUploadData.ResultType!=3">
                            <p>请先上传数据</p>
                        </div>
                        <h3>填报说明<span class="collection-updown-icon"></span></h3>
                        <div class="collection-result">
                            <div class="fill-des">
                                <textarea class="form-control" v-model="TaskReportRemark" cols="30" rows="10"></textarea>
                                <p>可上传其他补充说明的业务附件，不参与自动汇总</p>
                            </div>
                            <div class="des-attachment upload-attachment">
                                <span>相关附件:</span>
                                <div class="upload">
                                    <span>选择文件</span>
                                    <vue-fileupload id="attachment-file" v-bind:action="UploadTaskAttachAction" v-bind:url="UploadTaskAttachUrl" v-on:uploaded="taskattachmentuploaded"></vue-fileupload>

                                </div>
                                <div class="des-attachment-info">
                                    <span class="attachment-length" v-if="TaskReportAttachments.length>0" v-text="TaskReportAttachments.length+'个附件'"></span>
                                    <p v-for="attachment in TaskReportAttachments">
                                        <a class="attachment-t" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(attachment.ID)" v-text="attachment.FileName"></a><span v-on:click="deleteAttachment(attachment.ID)"></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
            <!-- 正文 content end -->
            <%--<div class="collection-action text-center">
                <span class="collection-btn btn-save">保存</span>
                <span class="collection-btn btn-submit">提交</span>
                <span class="collection-btn btn-add-tag">加签</span>
            </div>--%>
        </div>
    </div>

    <div id="SJSJ_LC"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/template" id="fileupload-template">
        <input type="file" value="" v-bind:id="id" v-bind:action="action" v-bind:url="url" />
    </script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskCollectionMobile.js?v=")+System.Guid.NewGuid()  %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.fileupload.js") %>"></script>


</asp:Content>

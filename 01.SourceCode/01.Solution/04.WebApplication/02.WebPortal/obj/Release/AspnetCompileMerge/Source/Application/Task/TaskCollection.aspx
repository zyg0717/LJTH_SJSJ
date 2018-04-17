<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/CollectionLayout.Master" AutoEventWireup="true" CodeBehind="TaskCollection.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskCollection" %>

<%@ Register Src="~/SiteMaster/userSelectCtrl.ascx" TagPrefix="uc1" TagName="userSelectCtrl" %>
<%@ Register Src="~/SiteMaster/wfCtrl.ascx" TagPrefix="uc1" TagName="wfCtrl" %>




<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
    <link href="../../Assets/vendors/message/css/message.css" rel="stylesheet" />
    <style type="text/css">
        .attachments .attachment-tt {
            display: inline;
        }

        .img-point {
            cursor: pointer;
        }

        .process-plugin {
            min-height: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="taskCollection">
        <div class="common-wrap task-wrap">

            <h1 class="title text-center tc-title"></h1>
            <div class="create-times clear text-center">
                <p>发起时间：<span class="mgr20 tc-createdate"></span> 下发人：<span class="tc-createname"></span></p>
            </div>
            <div class="collection-content clear">

                <h3 class="sub-title col-md-12">任务要求</h3>
                <div class="clear  mg">
                    <div class="col-md-12 request-wrap mgb20">
                        <p class="request-info tc-remark">
                        </p>
                    </div>
                </div>

                <h3 class="sub-title col-md-12 tc-attachments-title">任务相关附件</h3>
                <div class="clear  mg">
                    <div class="col-md-12 request-wrap fill-wrap attachments tc-attachments">
                    </div>
                </div>

                <div class="row clear  mg">
                    <dl class="col-md-4 text-center collection-step step-01">
                        <dt class="mgb15">
                            <img src="<%=ResolveUrl("~/Assets/images/num1.png") %>" alt="" />
                            <img src="<%=ResolveUrl("~/Assets/images/down1.png") %>" alt="" />
                        </dt>
                        <dd class="mgb15">
                            <button type="button" class="btn download-template tc-template">下载模板</button>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>点击上面按钮下载【填报模板Excel文件】</p>
                            <p>请选择保存至电脑</p>
                        </dd>
                    </dl>
                    <dl class="col-md-4 text-center collection-step  step-02">
                        <dt class="mgb15">
                            <img src="<%=ResolveUrl("~/Assets/images/num2.png") %>" alt="" />
                            <img src="<%=ResolveUrl("~/Assets/images/leave.png") %>" alt="" />
                        </dt>
                        <dd class="mgb15">
                            <span class="btn fill-line disabled">线下填报</span>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>打开下载的【填报模板Excel文件】</p>
                            <p>填报数据后保存关闭</p>
                            <p>(表格列和页面格式不允许修改)</p>
                        </dd>
                    </dl>
                    <dl class="col-md-4 text-center collection-step step-03">
                        <dt class="mgb15">
                            <img src="<%=ResolveUrl("~/Assets/images/num3.png") %>" alt="" />
                            <img src="<%=ResolveUrl("~/Assets/images/up.png") %>" alt="" />
                        </dt>
                        <dd class="mgb15">
                            <div class="upload-data">
                                <span>上传数据</span>
                                <input type="file" id="fileTaskUpload" value=" " />
                            </div>
                        </dd>
                        <dd class="text-center collection-step-desc">
                            <p>点击上面按钮上传</p>
                            <p>【填报完成的Excel文件】</p>
                            <%--<p>或【<a class="fill-online" href="#">在线填报</a>】</p>--%>
                        </dd>
                    </dl>
                </div>
                <!--<div class="tipsbox col-md-12">
                 <p class="tips-t">上传失败!</p>
                 <p class="tips-con"></p>
             </div>
             <div class="tipsbox col-md-12">
                 <p class="tips-t">上传失败!</p>
                 <p class="tips-con">未识别到有效的数据行。</p>
             </div>
             <div class="tipsbox col-md-12">
                 <p class="tips-t">上传失败!</p>
                 <p class="tips-con">请使用系统下载的文件上传。</p>
             </div>
             -->

                <div class="tipsbox col-md-12 tc-upload  mg">
                    <p class="tips-t">系统提示!</p>
                    <p class="tips-con">请先下载模版，填写并上传数据后，再进行后续操作。</p>
                </div>
                <div class="tipsbox col-md-12 tc-upload result-1 hidden  mg tipsfail">
                    <p class="tips-t">上传失败!</p>
                    <p class="tips-con">请使用系统下载的文件上传。</p>
                </div>
                <div class="tipsbox col-md-12 tc-upload result-2 hidden  mg tipsfail">
                    <p class="tips-t">上传失败!</p>
                    <p class="tips-con tc-upload-result-2-msg"></p>
                </div>
                <div class="tipsbox col-md-12 tc-upload result-3 hidden mg">
                    <p class="tips-t">上传成功!</p>
                    <p class="tips-con">
                        <span style="color: red;">点击链接查看已上传成功的文件，如果要修改，请修改文件后重新上传。</span>
                        <a class="attachment-tt tc-result-filename" href="javascript:void(0)"></a>

                    </p>
                    <p class="tips-con tc-sheet-length">
                    </p>
                </div>
                <div class="fill-instruction row clear mg">
                    <div class="mask col-md-12 tc-upload-block">
                        <div>请先上传数据</div>
                    </div>
                    <div class="fill-instr-info clear mgb20">
                        <p class="col-md-12 instr-t">填报说明</p>
                        <div class="col-md-12">
                            <textarea class="form-control tc-report-remark" cols="5"></textarea>
                        </div>
                        <p class="col-md-12 mgb10">可上传其他补充说明的业务附件，不参与自动汇总</p>
                        <div class="col-md-12 attachment-desc">
                            <span class="pull-left">相关附件：</span>
                            <div class="instr-file">
                                <span>选择文件</span>
                                <input type="file" id="attachment-file" />
                            </div>
                            <div class="tc-task-report-attachlength"></div>
                            <div class="tc-task-report-attachments">
                            </div>
                        </div>
                    </div>
                    <div class="approval-process clear " >
                        <p class="process-t">审批流程</p>
                        <div class="process-cont">
                            <div id="SJSJ_LC" class="process-plugin"></div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
                            

    <%--<div class="task-footer" id="task-footer">
        <table class="task-footer-btn collection-w">
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <button class="btn btn-blue forward">保存</button>
                    <button class="btn btn-orange forward">提交</button>
                    <button class="btn btn-blue forward" style="display: none;">加签</button>
                </td>
                <td>&nbsp;&nbsp;</td>
            </tr>
        </table>
    </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskCollection.js?v=")+System.Guid.NewGuid()  %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>
    <uc1:wfCtrl runat="server" ID="wfCtrl" />
    <uc1:userSelectCtrl runat="server" ID="userSelectCtrl" />
    
    <script src="<%=ResolveUrl("~/Assets/vendors/workflow/js/Workflow.js?v=") +System.Guid.NewGuid()%>"></script>
    <script type="text/html" id="script_attachment">
        <p><a class="attachment-tt tc-attach-item" href="javascript:void(0)"></a></p>
    </script>
    <script type="text/html" id="script_report_attachment">
        <p class="clear report-attachment-info">
            <a class="attachment-tt tc-report-attachment pull-left" href="javascript:void(0)"></a>
            <img class="tc-report-attachment-del img-point pull-left" src="<%=ResolveUrl("~/Assets/images/icon-delet2.png") %>" />
        </p>
    </script>
    <script type="text/html" id="script_sheet_length">
        <p class="tc-sheet-name-length"></p>
    </script>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/CollectionLayout.Master" AutoEventWireup="true" CodeBehind="TaskCollectionView.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskCollectionView" %>

<%@ Register Src="~/SiteMaster/wfCtrl.ascx" TagPrefix="uc1" TagName="wfCtrl" %>
<%@ Register Src="~/SiteMaster/userSelectCtrl.ascx" TagPrefix="uc1" TagName="userSelectCtrl" %>




<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
    <style type="text/css">
        .wanda-wfclient-log-quicklog {
            line-height: 19px;
            height: 19px;
        }

        .attachments .attachment-tt {
            display: inline;
        }

        .report-attachment-info {
            padding-left: 70px;
            text-indent: 2em;
        }

        .process-plugin {
            min-height: 100px;
        }

        #wanda-wfclient-log-content pre {
            color: #333;
            word-break: break-all;
            word-wrap: break-word;
            background-color: transparent;
            border: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="taskCollectionView">
        <div class="common-wrap task-wrap">
            <h1 class="title text-center tc-title"></h1>
            <div class="create-times clear text-center">
                <p>发起时间：<span class="mgr20 tc-createdate"></span> 下发人：<span class="tc-assignMember"></span></p>
            </div>
            <div class="collection-content clear tc-content">
                <h3 class="sub-title">任务要求</h3>
                <div class="request-wrap fill-wrap mgb20 mg">
                    <p class="request-info tc-request">
                    </p>
                </div>

                <div class="task-attachments">
                    <h3 class="sub-title tc-attachments-title">任务相关附件</h3>
                    <div class="request-wrap fill-wrap attachments tc-attachments mg"></div>
                </div>

                <h3 class="sub-title tc-taskAttachment-title hidden">填报文件</h3>
                <div class="request-wrap text-center fill-wrap mg tc-taskAttachment-container hidden">
                    <div title="" class="tc-taskAttachment-fileName">
                        <img style="cursor: pointer" src="<%=ResolveUrl("~/Assets/images/shenpiexcel.png") %>" alt="" />
                        <p class="download-wrap"><a href="javascript:void(0)" class="btn btn-white tc-download">下载（<span class="tc-download-size"></span>）</a></p>
                    </div>
                </div>

                <div class="task-fill-desc clear">
                    <h3 class="sub-title">填报说明</h3>
                    <div class="request-wrap fill-wrap fill-desc mg">
                        <p class="tc-fill-desc tc-report-remark request-info"></p>
                    </div>
                    <div class="col-md-12 filebox tc-report-attachment-container hidden">
                        <p class="pull-left" style="position: absolute">相关附件：</p>
                        <div class="tc-task-report-attachlength report-attachment-info"></div>
                        <div class="tc-task-report-attachments">
                        </div>
                        <!--<a class="attachment-tt tc-attachments-info" href="#">问题反馈+-+副本-zhangaizhen-王键（wangjian115）-20160929191151.xlsx</a>-->
                    </div>
                </div>
                <div class="approval-process mg tc-approve" style="display:none">
                    <p class="process-t">审批流程</p>
                    <div class="process-cont">
                        <div id="SJSJ_LC" class="process-plugin"></div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <%--<div class="task-footer" id="task-footer">
        <table class="task-footer-btn ">
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <button class="btn btn-orange forward">提交</button>
                    <button class="btn btn-blue forward">加签</button>
                    <button class="btn btn-blue forward" style="display: none;">转发</button>
                </td>
                <td>&nbsp;&nbsp;</td>
            </tr>
        </table>
    </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskCollectionView.js?v=")+System.Guid.NewGuid() %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>
    <uc1:wfCtrl runat="server" ID="wfCtrl" />
    <uc1:userSelectCtrl runat="server" ID="userSelectCtrl" />
    
    <script src="<%=ResolveUrl("~/Assets/vendors/workflow/js/Workflow.js?v=")+System.Guid.NewGuid() %>"></script>

    <script type="text/html" id="script_attachment">
        <p><a class="attachment-tt tc-attach-item" href="javascript:void(0)"></a></p>
    </script>
    <script type="text/html" id="script_report_attachment">
        <p class="clear report-attachment-info">
            <a class="attachment-tt tc-report-attachment pull-left" href="javascript:void(0)"></a>
        </p>
    </script>
</asp:Content>

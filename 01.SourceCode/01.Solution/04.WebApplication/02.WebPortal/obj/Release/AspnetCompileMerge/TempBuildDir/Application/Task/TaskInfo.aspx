<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.Master" AutoEventWireup="true" CodeBehind="TaskInfo.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskInfo" %>

<%@ Register Src="~/SiteMaster/userSelectCtrl.ascx" TagPrefix="uc1" TagName="userSelectCtrl" %>


<%--<%@ Register Src="~/Application/Task/Control/ucSelectUserCtrl.ascx" TagPrefix="uc1" TagName="ucSelectUserCtrl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
    <uc1:userSelectCtrl runat="server" ID="userSelectCtrl1" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div class="wrap" id="vue" v-cloak>
        <div class="task-cont clear">
            <div class="task-tt task-top" v-cloak><span class="icon-text"><i class="fa fa-file-text-o"></i>&nbsp;{{ TaskName }}</span></div>
            <div class="task-info mgb15" v-cloak>
                <div class="task-info-lsit clear">
                    <label class="label-t pull-left">模板名称：</label>
                    <div class="list-cont white-space full-name pull-left"><span class="color-gray template-name mgr5" v-text="TaskTemplateName"></span>
                        <%--for owa--%>
                        <span class="color-blue" v-on:click="commons.OWA_PreviewAttachment(TaskTemplateAttachmentID)">预览</span>
                        <%--<span class="color-blue" v-on:click="commons.DownloadTemplate(TaskTemplateID)">预览</span>--%>
                    </div>
                </div>
                <div v-if="TaskType==3" class="task-info-lsit clear" v-cloak>
                    <label class="label-t pull-left">所属任务：</label>
                    <div class="list-cont pull-left" >
                         <a v-bind:href="'TaskAdd.aspx?taskId='+OwnerTaskID" target="_blank">点击查看</a>
                    </div>
                </div>
                <div class="task-info-lsit clear" v-cloak>
                    <label class="label-t pull-left">上报方式：</label>
                    <div class="list-cont pull-left" v-text="ApproveContent"></div>
                </div>
                <div class="task-info-lsit clear" v-cloak>
                    <label class="label-t pull-left">任务要求：</label>
                    <div class="list-cont pull-left">
                        <p class="task-request" v-text="Remark ? Remark : '- -'"></p>
                    </div>
                </div>
                <div class="task-info-lsit clear" v-cloak>
                    <label class="label-t pull-left">下发信息：</label>
                    <div class="list-cont pull-left full-name">{{TaskCreatorName}}（{{TaskCreatorLoginName}}） 于 {{TaskCreateDate | format-datetime}} 下发</div>
                </div>
                <div class="task-info-lsit clear" v-show="length > 0" v-cloak>
                    <label class="label-t pull-left">相关附件：</label>
                    <div class="list-cont pull-left full-name">
                        <p>您已经选择&nbsp;&nbsp;<span v-text="length"></span>&nbsp;&nbsp;个附件</p>
                        <div class="list-attachments">
                            <p v-for="attachment in Attachments">
                                <a class="attachment-tt" href="javascript:void(0)" v-text="attachment.FileName" v-on:click="downLoadAtttachment(attachment.ID)"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <ul class="nav nav-tabs border bd-bottom ">
                    <li class="tab total active" v-on:click="getUserNodes('total',TaskID,receiverName)" v-cloak>
                        <span class="nav-icon"></span><i class="fa fa-circle"></i>全&nbsp;部 <span class="percent" v-cloak id="spTotal" v-text="TaskUserNodeCount"></span>
                        <input type="hidden" name="total" value="0" />
                    </li>
                    <li class="tab completed" v-on:click="getUserNodes('completed',TaskID,receiverName)" v-cloak>
                        <span class="nav-icon"></span><i class="fa fa-circle"></i>已完成 <span class="percent" v-cloak id="spFinish" v-text="CompleteUserNodeCount+'（'+TaskProcess+'）'"></span>
                        <input type="hidden" name="total" value="1" />
                    </li>
                    <li class="tab processing" v-on:click="getUserNodes('processing',TaskID,receiverName)" v-cloak>
                        <span class="nav-icon"></span><i class="fa fa-circle"></i>进行中 <span class="percent" v-cloak id="spPending" v-text="(TaskUserNodeCount - CompleteUserNodeCount)+'（'+Processing+'）'"></span>
                        <input type="hidden" name="total" value="2" />
                    </li>
                </ul>
                <div class="delivery-progress" v-cloak>
                    <span class="ongoingbar"></span>
                    <span class="finishedbar" v-bind:style="{'width': TaskProcess}"></span>
                </div>
                <div class="task-behavior clear">
                    <div class="mgb col-md-6 col-sm-6 pd0 col-xs-12">
                        <button v-on:click="SelectUser()" class="add-member choose-member btn btn-default btn-white pull-left">追加</button>
                        <%--<button class="append-user btn btn-default btn-white pull-left">批量追加</button>--%>
                        <div v-if="!isMobile" class="attachment-btn btn btn-default btn-white pull-left" title="请将填报人登陆账号每行一个写到excel文件中， 点击按钮选择文件并上传">
                            <span>批量追加</span><input type="file" name="file" id="appenduseruploader" />
                        </div>
                        <button v-if="userNodes.length > 0" class="resend-btn choose-member btn pull-left"  v-bind:class="{'btn-default': checkedLength > 0, 'btn-disabled': checkedLength == 0 }" v-on:click="resendTask(tabName,TaskID,receiverName)">重发</button>
                        <button v-if="userNodes.length > 0" class="delete-btn append-user btn pull-left" v-bind:class="{'btn-default': checkedLength > 0, 'btn-disabled': checkedLength == 0 }" v-on:click="deleteTask(tabName,TaskID,receiverName)">删除</button>
                        <p class="inline pull-left">当前人数&nbsp;<span class="choose-num" v-text="userNodes.length"></span>&nbsp;人</p>
                    </div>
                    <div class="mgb col-md-3 col-sm-4 col-sm-offset-2 col-xs-12 col-md-offset-3 pd0">
                        <input type="text" class="form-control" name="search-user" v-bind:value="receiverName" v-model="receiverName" placeholder="搜索接收人姓名或用户名..." v-on:keyup="searchReceiverName(tabName,TaskID,receiverName)" onkeydown="if(event.keyCode==13)return false;" />
                    </div>
                </div>
            </div>


            <div class="tabs-content col-xs-12" v-cloak>
                <div class="tabs">
                    <ul class="fill-info-area" v-if="isMobile">
                        <li class="fill-select-all clear" v-show="userNodes.length > 0">
                            <input class="selectAll chk" id="selectAll" type="checkbox" name="check-all" v-on:change="selectAll()" v-model="selectedAll" />
                            <label class="control-label chk-label mobile-chk-all" for="selectAll"><span>全选</span></label>
                        </li>
                        <li class="fill-list-info fill-list clear" v-for="(user,index) in userNodes">
                            <p class="chk-p">
                                <input class="selectSub chk" v-bind:id="'task'+index" type="checkbox" v-bind:value="user.TaskUserLoginName" v-model="user.checked" v-bind:name=" 'chk_'+ index" v-on:change="select(user)" />
                                <label class="chk-label empty-label" v-bind:for="'task'+index"></label>
                            </p>
                            <p class="clear"><span class="fill-c" v-text="user.TaskUserDeparment"></span></p>
                            <p class="clear"><span class="fill-c" v-text="user.TaskUserName+'('+user.TaskUserLoginName+')'"></span></p>
                            <span class="current-status" v-if="user.NodeStatus==0">进行中</span>
                            <span class="current-status" v-if="user.NodeStatus==1">审批中</span>
                            <span class="current-status" v-if="user.NodeStatus==2">已完成</span>
                            <span class="current-status" v-if="user.NodeStatus==4">已退回</span>
                        </li>
                    </ul>
                    <div class="conts table-responsive" v-if="!isMobile && userNodes.length > 0">
                        <table class="table table-hover border">
                            <thead>
                                <tr>
                                    <th>
                                        <input class="selectAll chk" id="selectAll" type="checkbox" name="check-all" v-on:change="selectAll()" v-model="selectedAll" />
                                        <label class="chk-label empty-label" for="selectAll"></label>
                                    </th>
                                    <th>序号</th>
                                    <th style="width:45%">接收人部门</th>
                                    <th>接收人</th>
                                    <th>填报日期</th>
                                    <th>审批日期</th>
                                    <th v-if="TaskTemplateType == 2">数据区域</th>
                                    <th>查看流程</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(user,index) in userNodes">
                                    <td>
                                        <input class="selectSub chk" v-bind:id="'task'+index" type="checkbox" v-bind:value="user.TaskUserLoginName" v-model="user.checked" v-bind:name=" 'chk_'+ index" v-on:change="select(user)" />
                                        <label class="chk-label empty-label" v-bind:for="'task'+index"></label>
                                    </td>
                                    <td v-text="index+1"></td>
                                    <td v-text="user.TaskUserDeparment"></td>
                                    <td>{{ user.TaskUserName }}（{{ user.TaskUserLoginName }}）</td>
                                    <td>{{ user.ReceiveDate | format-datetime }}</td>
                                    <td>{{ user.ApproveDate | format-datetime }}</td>
                                    <td v-if="TaskTemplateType == 2">{{user.TaskTemplateTypeVal}}</td>
                                    <td><span class="color-blue"><a v-if="user.BusinessID!=null&&user.BusinessID.length>0" target="_blank" v-bind:href="'/Application/Task/TaskCollectionView.aspx?businessID='+user.BusinessID+'&v=1'">查看</a></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </div>
            </div>
        </div>

        <div class="mdl-dialog-container" v-cloak v-if="taskFileDialog.show">
            <div class="mdl-dialog" style="max-width: 600px;">
                <div class="task-tt task-top mgb0">
                    <span v-text="taskFileDialog.title">下载汇总文件</span>
                    <span class="btn-close" v-on:click="closeTaskFile"></span>
                </div>
                <div class="mdl-dialog__content">
                    <div class="row line-height">
                        <label class="control-label label-md col-md-2 col-sm-2">任务进度：</label>
                        <div class="col-md-9 col-sm-9 mgb10" v-text="'本次共有'+taskFileDialog.content.data.total+'人填报，'+taskFileDialog.content.data.finish+'人已完成填报'"></div>
                    </div>
                    <div class="row line-height">
                        <label class="control-label label-md col-md-2 col-sm-2">追加信息：</label>
                        <div class="col-md-9 col-sm-9 mgb10">
                            <div class="chk-group pull-left">
                                <input class="chk hidden" type="checkbox" name="chk1" id="chk1" v-model="taskFileDialog.content.data.chk1" />
                                <label class="control-label chk-label hidden" for="chk1">
                                    <span>公司</span>
                                </label>
                            </div>
                            <div class="chk-group pull-left">
                                <input class="chk " type="checkbox" name="chk2" id="chk2" v-model="taskFileDialog.content.data.chk2" />
                                <label class="control-label chk-label " for="chk2">
                                    <span>组织架构</span>
                                </label>
                            </div>
                            <div class="chk-group pull-left">
                                <input class="chk " type="checkbox" name="chk3" id="chk3" v-model="taskFileDialog.content.data.chk3" />
                                <label class="control-label chk-label " for="chk3">
                                    <span>姓名</span>
                                </label>
                            </div>
                            <div class="chk-group pull-left">
                                <input class="chk " type="checkbox" name="chk4" id="chk4" v-model="taskFileDialog.content.data.chk4" />
                                <label class="control-label chk-label " for="chk4">
                                    <span>账号</span>
                                </label>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="mdl-dialog__actions text-center">
                    <button class=" btn_lg btn-default btn-white" v-on:click="download">下载汇总文件</button>
                </div>
            </div>
        </div>
        <div class="task-footer" v-cloak>
            <table class="task-footer-btn">
                <tr>
                    <td>&nbsp;&nbsp;</td>
                    <td>
                        <button class="btn btn-orange" v-on:click="downloadTaskFile(taskTemplate)" v-text="utils.mobileBrower()?'下载汇总':'下载汇总文件'"></button>
                        <button class="btn btn-orange" v-on:click="downTaskApprovedList(taskTemplate)" v-text="utils.mobileBrower()?'导出审批':'导出审批流程'"></button>
                        <button class="btn btn-orange" v-if="CompleteUserNodeCount>0" v-on:click="downTaskFileZip(taskTemplate)" v-text="utils.mobileBrower()?'打包源文件':'打包源文件'"></button>
                        <button class="btn btn-orange" v-if="TaskAttachmentCount>0" v-on:click="downAttachmentZip(taskTemplate)" v-text="utils.mobileBrower()?'打包附件':'打包相关附件'"></button>
                        <button class="btn btn-red" v-if="!(TaskStatus==2||TaskStatus==3||CompleteUserNodeCount==TaskUserNodeCount)" v-on:click="stopEditTask(TaskID)"  v-text="utils.mobileBrower()?'终止填报':'终止填报任务'"></button>
                    </td>
                    <td>&nbsp;&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <uc1:userSelectCtrl runat="server" ID="userSelectCtrl" />
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskInfo.js?v=")+System.Guid.NewGuid() %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("click",".tab", function () {
                $(this).addClass("active").siblings("li").removeClass("active");
            });
        });
    </script>
</asp:Content>

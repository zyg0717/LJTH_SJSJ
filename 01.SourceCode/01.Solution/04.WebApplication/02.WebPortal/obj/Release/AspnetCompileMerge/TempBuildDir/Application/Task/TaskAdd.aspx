<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.Master" AutoEventWireup="true" CodeBehind="TaskAdd.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskAdd" %>

<%@ Register Src="~/SiteMaster/userSelectCtrl.ascx" TagPrefix="uc1" TagName="userSelectCtrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
    <link href="<%=ResolveUrl("~/Assets/vendors/datepicker/css/bootstrap-datepicker.standalone.min.css") %>" rel="stylesheet" />
    <style type="text/css">
        .img_pointer {
            cursor: pointer;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="taskInfo" v-cloak>
        <div class="task-cont task-wrap" v-bind:style="{'margin-bottom':TaskType==3&&TaskStatus==0?'20px':'0px'}">
            <div class="task-tt task-top"><span class="icon-tag"><i class="fa fa-tag"></i>&nbsp;任务详情</span></div>

            <div class="form-group clear" v-if="!IsEdit">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">创建方式:</label>
                <div class="col-md-9 col-sm-10">
                    <input type="radio" name="CreateType1" id="CreateType1" value="0" v-model="CreateType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="CreateType1">
                        创建新任务
                    </label>
                    <input type="radio" name="CreateType2" id="CreateType2" value="1" v-model="CreateType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="CreateType2">
                        克隆已有任务
                        <a href="javascript:void(0)" v-if="CreateType==1" v-on:click="selectTask">选择任务</a>
                    </label>
                </div>
            </div>
            <div class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" for="task-title" v-bind:class="{'label-lg': TaskType+ '' === '2'}">任务标题:</label>
                <div class="col-md-9 col-sm-10">
                    <input class="form-control" type="text" name="task-title" v-model="TaskName" onkeydown="if(event.keyCode==13)return false;" />
                </div>
            </div>
            <div class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">选择模板:</label>
                <div class="choose-model col-md-9 col-sm-10">
                    <span class="color-blue" v-on:click="selectTemplate">选择模板</span>
                    <div class="template-attachment">
                        <span class="color-gray template-name" v-show="TaskTemplateName!=null&&TaskTemplateName.length>0" v-text="'【'+TaskTemplateName+'】'"></span>
                        <%--for owa--%>
                        <span class="color-blue" v-show="TaskTemplateAttachmentID!=null" v-on:click="commons.OWA_PreviewAttachment(TaskTemplateAttachmentID)">预览</span>
                    </div>
                </div>
            </div>
            <div class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">上报方式:</label>
                <div class="col-md-9 col-sm-10">
                    <input type="radio" name="inlineRadioOptions" id="inlineRadio1" value="0" v-model="IsNeedApprove" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="inlineRadio1">
                        不需要流程审批
                    </label>
                    <input type="radio"  name="inlineRadioOptions" id="inlineRadio2" value="1" v-model="IsNeedApprove" onkeydown="if(event.keyCode==13)return false;" />
                    <label  class="radio-inline" for="inlineRadio2">
                        需要审批
                    </label>
                </div>
            </div>
            <div class="form-group clear" v-show="TaskType!=3">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">任务类型:</label>
                <div class="col-md-9 col-sm-10">
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskType" id="rdoTaskType1" value="1" v-model="TaskType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="rdoTaskType1" v-bind:class="{'radio-disabled': IsEdit}">
                        一次性任务
                    </label>
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskType" id="rdoTaskType2" value="2" v-model="TaskType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="rdoTaskType2" v-bind:class="{'radio-disabled': IsEdit}">
                        重复性任务
                    </label>
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskType" id="rdoTaskType3" value="3" v-model="TaskType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline hidden" for="rdoTaskType3" v-bind:class="{'radio-disabled': IsEdit}">
                        子任务
                    </label>
                </div>
            </div>
            <div class="form-group clear" v-show="TaskType==3">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">所属任务:</label>
                <div class="col-md-9 col-sm-10">
                    <a v-bind:href="'TaskAdd.aspx?taskId='+OwnerTaskID" target="_blank">点击查看</a>
                </div>
            </div>
            
            <div class="form-group clear" v-show="TaskType==2">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">执行频率:</label>
                <div class="col-md-9 col-sm-10">
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskCircleType" id="rdoTaskCircleType2" value="2" v-model="TaskCircleType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="rdoTaskCircleType2" v-bind:class="{'radio-disabled': IsEdit}">
                        每天
                    </label>
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskCircleType" id="rdoTaskCircleType3" value="3" v-model="TaskCircleType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="rdoTaskCircleType3" v-bind:class="{'radio-disabled': IsEdit}">
                        自定义
                    </label>
                    <input v-bind:disabled="IsEdit" type="radio" name="rdoTaskCircleType" id="rdoTaskCircleType1" value="1" v-model="TaskCircleType" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline hidden" for="rdoTaskCircleType1" v-bind:class="{'radio-disabled': IsEdit}">
                        一次性任务
                    </label>
                </div>
            </div>
            <div v-show="TaskCircleType==2" class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">任务起止日期:</label>
                <div class="col-md-9 col-sm-8" style="padding-left: 0px; padding-right: 0px;">
                    <div class="col-md-3 col-sm-4 col-xs-6">
                        <vue-datepicker class="form-control" v-on:changedate="changeStartDate" v-bind:value="PlanStart" placeholder="开始时间"></vue-datepicker>
                    </div>
                    <div class="col-md-3 col-sm-4 col-xs-6">
                        <vue-datepicker class="form-control" v-on:changedate="changeEndDate" v-bind:value="PlanEnd" placeholder="结束时间"></vue-datepicker>
                    </div>
                    <div class="col-md-3 col-sm-4 col-xs-6">
                        <span id="spncount" v-show="TaskCircleType==2"></span>
                    </div>
                </div>
            </div>
            <div v-show="TaskCircleType!=1" class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">任务执行时间:</label>
                <div class="col-md-9 col-sm-8" style="padding-left: 0px; padding-right: 0px;">
                    <div class="col-md-6 col-sm-6 col-xs-12" v-cloak>
                        <div class="select-container mobile-select">
                            <select class="form-control" v-model="PlanHourAndMinute">
                                <option value="1:00">1:00</option>
                                <option value="1:30">1:30</option>
                                <option value="2:00">2:00</option>
                                <option value="2:30">2:30</option>
                                <option value="3:00">3:00</option>
                                <option value="3:30">3:30</option>
                                <option value="4:00">4:00</option>
                                <option value="4:30">4:30</option>
                                <option value="5:00">5:00</option>
                                <option value="5:30">5:30</option>
                                <option value="6:00">6:00</option>
                                <option value="6:30">6:30</option>
                                <option value="7:00">7:00</option>
                                <option value="7:30">7:30</option>
                                <option value="8:00">8:00</option>
                                <option value="8:30">8:30</option>
                                <option value="9:00">9:00</option>
                                <option value="9:30">9:30</option>
                                <option value="10:00">10:00</option>
                                <option value="10:30">10:30</option>
                                <option value="11:00">11:00</option>
                                <option value="11:30">11:30</option>
                                <option value="12:00">12:00</option>
                                <option value="12:30">12:30</option>
                                <option value="13:00">13:00</option>
                                <option value="13:30">13:30</option>
                                <option value="14:00">14:00</option>
                                <option value="14:30">14:30</option>
                                <option value="15:00">15:00</option>
                                <option value="15:30">15:30</option>
                                <option value="16:00">16:00</option>
                                <option value="16:30">16:30</option>
                                <option value="17:00">17:00</option>
                                <option value="17:30">17:30</option>
                                <option value="18:00">18:00</option>
                                <option value="18:30">18:30</option>
                                <option value="19:00">19:00</option>
                                <option value="19:30">19:30</option>
                                <option value="20:00">20:00</option>
                                <option value="20:30">20:30</option>
                                <option value="21:00">21:00</option>
                                <option value="21:30">21:30</option>
                                <option value="22:00">22:00</option>
                                <option value="22:30">22:30</option>
                                <option value="23:00">23:00</option>
                                <option value="23:30">23:30</option>
                            </select>
                            <span class="icon-select"></span>
                        </div>
                    </div>

                    <!--<div class="col-md-3 col-sm-4 col-xs-6">
                        <div class="select-container mobile-select">
                            <select class="form-control" v-model="PlanMinute">
                                <option>0</option>
                                <option>30</option>
                            </select>
                            <span class="icon-select"></span>
                        </div>
                    </div>-->
                </div>
            </div>

            <div v-show="TaskType==2&&((IsEdit&&TaskCircleType==2)||TaskCircleType==3)" class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">任务记录:</label>
                <div class="col-md-10 col-sm-10" style="padding-right: 0px; padding-left: 0;">
                    <div v-if="TaskCircleType==3" class="col-md-3 visible-lg-block  visible-md-block" style="padding: 0px; border: 1px solid #ecebeb; clear: both; margin-left: 15px; width: 214px;">
                        <div class="datepicker" v-datepck></div>
                    </div>
                    <div v-bind:class="[TaskCircleType==3?'col-md-8':'col-md-12', 'col-sm-12']">
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped">
                                <thead class="fix">
                                    <tr>
                                        <th>序号</th>
                                        <th>日期</th>
                                        <th>星期</th>
                                        <th>操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(node,i) in TaskTimeNodes">
                                        <td v-text="(i+1)"></td>
                                        <td v-text="utils.ChangeDateFormat(node.TimeNode,false)"></td>
                                        <td v-text="utils.ToDayWeekString(node.TimeNode,false)"></td>
                                        <td>
                                            <span v-if="node.TimeNodeStatus==1">待发起</span>
                                            <a v-if="node.TimeNodeStatus==2" target="_blank" v-bind:href="'TaskAdd.aspx?taskId='+node.SubTaskID">待提交</a>
                                            <a v-if="node.TimeNodeStatus==3" target="_blank" v-bind:href="'TaskInfo.aspx?taskId='+node.SubTaskID">已提交</a>
                                            <a v-if="node.TimeNodeStatus==4" target="_blank" v-bind:href="'TaskInfo.aspx?taskId='+node.SubTaskID">已作废</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group clear">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">详细要求:</label>
                <div class="col-md-9 col-sm-10">
                    <textarea class="form-control" rows="7" v-model="Remark"></textarea>
                </div>
            </div>
            <div class="form-group clear" style="margin-bottom: 0;">
                <label class="control-label col-md-2 col-sm-2 label-tt" v-bind:class="{'label-lg': TaskType+ '' === '2'}">相关附件:</label>
                <div class="col-md-9 col-sm-10">
                    <div class="attachment-btn pull-left"><span class="color-blue">选择文件</span><input type="file" name="file" id="fileAttachment" onkeydown="if(event.keyCode==13)return false;" /></div>
                    <div class="attachment-info pull-left">
                        <p>您已经选择&nbsp;<span class="attachment-num" v-text="Attachments.length"></span>&nbsp;个附件</p>
                    </div>
                    <div class="col-md-12 col-sm-12 clear mgb5" v-for="attachment in Attachments" style="padding-right: 0;">
                        <div class="clear" style="padding-left:48px; line-height: 16px;">
                            <a class="attachment-link" v-text="attachment.FileName" href="javascript:void(0)" v-on:click="commons.DownloadAttachment(attachment.ID)"></a>
                            <img class="img_pointer" src="<%=ResolveUrl("~/Assets/images/icon-delet2.png") %>" v-on:click="removeAttachment(attachment.ID)" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group clear" style="margin-bottom: 0;">
                <div class="col-sm-12">
                    <button  v-on:click="SelectUser()"  class="choose-member btn btn-default btn-white pull-left mgb15 mgt10"><span class="user-avatar"><i class="fa fa-user"></i>&nbsp;&nbsp;选择填报人</span></button>
                    <%--<button class="append-user btn btn-default btn-white pull-left">批量追加</button>--%>
                    <div v-if="!isMobile"  class="attachment-btn btn btn-default btn-white pull-left mgb15 mgt10" title="请将填报人登陆账号每行一个写到excel文件中， 点击按钮选择文件并上传">
                        <span>批量追加</span><input type="file" name="file" id="appenduseruploader" onkeydown="if(event.keyCode==13)return false;" />
                    </div>
                    <button v-if="UserNodes.length > 0" class="delete-btn append-user btn pull-left mgb15 mgt10" v-bind:class="{'btn-default': checkedLength > 0, 'btn-disabled': checkedLength == 0 }" v-on:click="deleteTask()">删除</button>
                </div>
            </div>

            <div class="fill-info clear">
                <ul v-if="isMobile" class="fill-info-wrap col-md-12 col-sm-12 col-xs-12" v-cloak>
                    <li class="fill-select-all clear" v-show="UserNodes.length > 0">
                        <input class="selectAll chk" id="selectAll" type="checkbox" name="check-all" v-on:change="selectAll()" onkeydown="if(event.keyCode==13)return false;" v-model="selectedAll" />
                        <label class="control-label chk-label mobile-chk-all" for="selectAll"><span>全选</span></label>
                    </li>
                    <li class="fill-list-info clear" v-for="(node,index) in UserNodes">
                        <p class="chk-p">
                            <input class="selectSub chk" v-bind:id="'task'+index" type="checkbox" v-bind:value="node.TaskUserLoginName" v-model="node.checked" v-bind:name=" 'chk_'+ index" onkeydown="if(event.keyCode==13)return false;" v-on:change="select(node)" />
                            <label class="chk-label empty-label" v-bind:for="'task'+index"></label>
                        </p>
                        <p class="pull-left"><span class="fill-t">填报人部门：</span><span class="fill-c" v-text="node.TaskUserDeparment"></span></p>
                        <p class="pull-left"><span class="fill-t">填报人岗位：</span><span class="fill-c" v-text="node.TaskUserPosition"></span></p>
                        <p class="pull-left"><span class="fill-t">填报人：</span><span class="fill-c" v-text="node.TaskUserName+'（'+node.TaskUserLoginName+'）'"></span></p>
                        <span class="icon-delete" v-on:click="removeUserNode(node.TaskUserLoginName)"></span>
                    </li>
                </ul>
                <div class="fill-table clear" v-if="!isMobile">
                    <div class="table-responsive">
                        <table class="table table-hover table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th>
                                        <input class="selectAll chk" id="selectAll" type="checkbox" name="check-all" v-on:change="selectAll()" v-model="selectedAll" onkeydown="if(event.keyCode==13)return false;" />
                                        <label class="chk-label empty-label" for="selectAll"></label>
                                    </th>
                                    <th>序号</th>
                                    <th>填报人部门</th>
                                    <th>填报人岗位</th>
                                    <th>填报人</th>
                                    <th v-if="TaskTemplateType == 2">设置数据区域</th>
                                    <th>编辑</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(node,index) in UserNodes">
                                    <td>
                                        <input class="selectSub chk" v-bind:id="'task'+index" type="checkbox" v-bind:value="node.TaskUserLoginName" v-model="node.checked" v-bind:name=" 'chk_'+ index" v-on:change="select(node)" onkeydown="if(event.keyCode==13)return false;" />
                                        <label class="chk-label empty-label" v-bind:for="'task'+index"></label>
                                    </td>
                                    <td v-text="index+1"></td>
                                    <td v-text="node.TaskUserDeparment"></td>
                                    <td v-text="node.TaskUserPosition"></td>
                                    <td v-text="node.TaskUserName+'（'+node.TaskUserLoginName+'）'"></td>
                                    <td style="width:130px;" v-if="TaskTemplateType == 2"><input type="text" class="form-control" style="text-align:center" placeholder="例：B列=一公司" v-model="node.TaskTemplateTypeVal"/></td>
                                    <td><span class="color-blue" v-on:click="removeUserNode(node.TaskUserLoginName)">删除</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="task-footer" v-bind:style="{height:TaskType==3&&TaskStatus==0?'60px':'38px'}" v-cloak>
            <div class="task-footer-btn clear">
                <table class="task-footer-btn">
                    <tr v-show="TaskType==3&&TaskStatus==0">
                        <td>&nbsp;&nbsp;</td>
                        <td><span v-text="counter" style="color: white; font-weight: bold;"></span></td>
                        <td>&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;&nbsp;</td>
                        <td>
                            <button class="btn btn-blue" v-on:click="SaveTask()" v-if="TaskStatus==0">保存任务</button>
                            <button class="btn btn-orange" v-on:click="SubmitTask()" v-if="TaskType!=2&&TaskStatus==0">发起任务</button>
                            <button class="btn btn-red" v-on:click="StopPlanTask()" v-if="TaskType==2&&TaskStatus==0&&IsEdit">终止计划</button>
                        </td>
                        <td>&nbsp;&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="mdl-dialog-container" v-cloak v-if="dialog.show">
            <div class="mdl-dialog">
                <div class="task-tt task-top mgb0">
                    <span v-text="dialog.title">选择模板</span>
                    <span class="btn-close" v-on:click="dialogClose"></span>
                </div>
                <div class="mdl-dialog__content" v-bind:class="{'mobile-dialog': isMobile}">
                    <div class="template-search clear">
                        <div class="row">
                            <label class="control-label label-md col-md-2 col-sm-2">模板名称：</label>
                            <div class="col-md-5 col-sm-5 mgb10">
                                <input type="text" v-model="dialog.search.name" placeholder="请输入模板名称…" class="form-control" onkeydown="if(event.keyCode==13)return false;" />
                            </div>
                            <div class="col-md-3 col-sm-3 mgb10">
                                <a href="javascript:void(0);" v-on:click="reload" class="btn btn-info action-search form-control" style="margin-top: -3px;"><span><i class="fa fa-search"></i>&nbsp;&nbsp;搜索</span></a>
                            </div>
                        </div>
                    </div>
                    <ul v-if="isMobile" class="template-dialog fill-info-wrap" v-cloak>
                        <li class="fill-list clear" v-for="(node,i) in dialog.content.data" style="cursor: pointer;" v-on:click="selectRow(node)">
                            <p class="pull-left"><span class="fill-t">模板名称：</span><span class="fill-c" v-text="node.TemplateName"></span></p>
                            <p class="pull-left"><span class="fill-t">创建人：</span><span class="fill-c" v-text="node.CreateName"></span></p>
                            <p class="pull-left"><span class="fill-t">创建时间：</span><span class="fill-c">{{ node.CreateDate | format-datetime }}</span></p>
                        </li>
                    </ul>
                    <div class="table-responsive" v-if="!isMobile">
                        <table class="table table-hover table-bordered table-striped">
                            <thead class="fix">
                                <tr>
                                    <th>模板名称</th>
                                    <th>创建人</th>
                                    <th>创建时间</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(node,i) in dialog.content.data" style="cursor: pointer;" v-on:click="selectRow(node)">
                                    <td v-text="node.TemplateName"></td>
                                    <td v-text="node.CreateName"></td>
                                    <td>{{node.CreateDate | format-datetime}}</td>

                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div v-show="dialog.content.total>0">
                        <vue-pager ref="pager" v-bind:scroll="false" page-size="15" v-bind:total="dialog.content.total" v-on:pager="loadTemplates"></vue-pager>
                    </div>
                </div>
                <%--<div class="mdl-dialog__actions">
                    <button class=" btn btn-default btn-white pull-left" v-on:click="dialogClose">关闭</button>
                    <button  class=" btn btn-default btn-white pull-left" v-on:click="dialogSave">保存</button>
                </div>--%>
            </div>
        </div>
        <div class="mdl-dialog-container" v-cloak v-if="taskDialog.show">
            <div class="mdl-dialog">
                <div class="task-tt task-top mgb0">
                    <span v-text="taskDialog.title">选择模板</span>
                    <span class="btn-close" v-on:click="taskDialogClose"></span>
                </div>
                <div class="mdl-dialog__content" v-bind:class="{'mobile-dialog': isMobile}">
                    <div class="template-search clear">
                        <div class="row">
                            <label class="control-label label-md col-md-2 col-sm-2">任务名称：</label>
                            <div class="col-md-5 col-sm-5 mgb10">
                                <input type="text" v-model="taskDialog.search.name" placeholder="请输入模板名称…" class="form-control" onkeydown="if(event.keyCode==13)return false;" />
                            </div>
                            <div class="col-md-3 col-sm-3 mgb10">
                                <a href="javascript:void(0);" v-on:click="taskReload" class="btn btn-info action-search form-control" style="margin-top: -3px;"><span><i class="fa fa-search"></i>&nbsp;&nbsp;搜索</span></a>
                            </div>
                        </div>
                    </div>
                    <ul v-if="isMobile" class="template-dialog fill-info-wrap" v-cloak>
                        <li class="fill-list clear" v-for="(node,i) in taskDialog.content.data" style="cursor: pointer;" v-on:click="selectTaskRow(node)">
                            <p class="pull-left"><span class="fill-t">任务名称：</span><span class="fill-c" v-text="node.TaskName"></span></p>
                            <p class="pull-left"><span class="fill-t">任务发布时间：</span><span class="fill-c">{{ node.TaskCreateDate | format-datetime }}</span></p>
                            <p class="pull-left"><span class="fill-t">创建人：</span><span class="fill-c" v-text="node.TaskCreatorName"></span></p>
                        </li>
                    </ul>
                    <div class="table-responsive" v-if="!isMobile">
                        <table class="table table-hover table-bordered table-striped">
                            <thead class="fix">
                                <tr>
                                    <th>任务名称</th>
                                    <th>任务发布时间</th>
                                    <th>创建人</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(node,i) in taskDialog.content.data" style="cursor: pointer;" v-on:click="selectTaskRow(node)">
                                    <td v-text="node.TaskName"></td>
                                    <td>{{node.TaskCreateDate | format-datetime}}</td>
                                    <td v-text="node.TaskCreatorName"></td>

                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div v-show=" taskDialog.content.total > 0">
                        <vue-pager ref="taskPager" v-bind:scroll="false" page-size="15" v-bind:total="taskDialog.content.total" v-on:pager="loadTasks"></vue-pager>
                    </div>
                </div>
                <%--<div class="mdl-dialog__actions">
                    <button class=" btn btn-default btn-white pull-left" v-on:click="taskDialogClose">关闭</button>
                   <button  class=" btn btn-default btn-white pull-left" v-on:click="dialogSave">保存</button>
                </div>--%>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">

    <uc1:userSelectCtrl runat="server" ID="userSelectCtrl" />
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskAdd.js?v=")+System.Guid.NewGuid() %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/datepicker/js/bootstrap-datepicker.min.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/datepicker/js/locales/bootstrap-datepicker.zh-CN.min.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.dialog.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.datepicker.js") %>"></script>

    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>

    <script type="text/template" id="datepicker-template">
        <input type="text" class="form-control picker" v-bind:placeholder="placeholder" readonly="readonly" onkeydown="if(event.keyCode==13)return false;" />
    </script>
</asp:Content>

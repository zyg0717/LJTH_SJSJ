<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/PageLayout.master" AutoEventWireup="true" CodeBehind="AdminTemplateList.aspx.cs" Inherits="WebApplication.WebPortal.Application.Admin.AdminTemplateList" %>
<asp:Content ContentPlaceHolderID="page_styles" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="page_sidebar" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="page_contents" runat="server">
    <div id="templateList" v-cloak>
        <a v-if="isMobile" href="<%=ResolveUrl("~/Application/Models/TemplateModel.aspx") %>" class="icon-add"></a>
        <div class="template-wrap gray-wrap" v-cloak>
            <div class="task-tt"  v-if="!isMobile" >
                <span class="icon-tag"><i class="fa fa-tag"></i>&nbsp;全部模板</span>
                <div class="view-all ">
                    <a class="icon-myTemplate" href="<%=ResolveUrl("~/Application/Models/TemplateList.aspx") %>">我的模板</a>
                </div>
            </div>        
            <div v-if="!isMobile" class="template-search clear" v-bind:class="{'pdt15': isMobile, 'pdt3': !isMobile }">
                <div class="col-md-4 col-xs-6 col-sm-4 mgb15">
                    <input type="text" class="form-control input-search" name="template-name" v-model="keyWord" value="" placeholder="请输入模板名称…"/>
                </div>
                <div class="col-md-4 col-xs-6 col-sm-4 mgb15">
                    <input type="text" class="form-control input-search" name="search-user" v-model="createName" value="" placeholder="请输入创建人账号或姓名…"/>
                </div>
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15" v-cloak>
                    <div class="select-container">
                        <select class="form-control select-item mobile-select" id="timeRange" v-model="selectedOption">
                            <option value=" ">全部日期</option>
                            <option value="1">今天</option>
                            <option value="2">过去24小时 </option>
                            <option value="3">过去一周 </option>
                            <option value="4">过去一个月 </option>
                            <option value="5">过去三个月 </option>
                            <option value="6">过去一年 </option>
                        </select>
                        <span class="icon-select"></span>
                    </div>                   
                </div>
                <!--<div class="col-md-2 col-xs-6 col-sm-2 text-center template-chk">
                    <input type="checkbox" name="my-template" class="chk" id="myTemplate" value="" v-model="isPrivate" />
                    <label class="control-label chk-label" for="myTemplate"><span class="view-myTemplate">只看我的模板</span></label>
                </div>-->
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15">
                    <a href="javascript:void(0);" class="btn btn-info action-search form-control" v-on:click="reload()" style="margin-top: -2px;"><span><i class="fa fa-search"></i>&nbsp;搜索</span></a>                 
                </div>
            </div>

            <div v-if="isMobile" class="template-search clear" v-bind:class="{'pdt15': isMobile, 'pdt3': !isMobile }">
                <div class="col-md-5 col-xs-12 col-sm-5 mgb15">
                    <input type="text" class="form-control input-search" name="template-name" v-model="keyWord" v-on:change="reload()" value="" placeholder="请输入模板名称…"/>
                </div>
                <div class="col-md-5 col-xs-6 col-sm-5 mgb15">
                    <input type="text" class="form-control input-search" name="search-user" v-model="createName" v-on:change="reload()" value="" placeholder="请输入创建人账号或姓名…"/>
                </div>
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15" v-cloak>
                    <div class="select-container">
                        <select class="form-control select-item mobile-select" id="timeRange" v-model="selectedOption" v-on:change="reload()">
                            <option value=" ">全部日期</option>
                            <option value="1">今天</option>
                            <option value="2">过去24小时 </option>
                            <option value="3">过去一周 </option>
                            <option value="4">过去一个月 </option>
                            <option value="5">过去三个月 </option>
                            <option value="6">过去一年 </option>
                        </select>
                        <span class="icon-select"></span>
                    </div>                   
                </div>
                <!--<div class="col-md-2 col-xs-6 col-sm-2 text-center template-chk">
                    <input type="checkbox" name="my-template" class="chk" id="myTemplate" value="" v-model="isPrivate" />
                    <label class="control-label chk-label" for="myTemplate"><span class="view-myTemplate">只看我的模板</span></label>
                </div>
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15">
                    <a href="javascript:void(0);" class="btn btn-info action-search form-control" v-on:click="reload()" style="margin-top: -2px;"><span><i class="fa fa-search"></i>&nbsp;搜索</span></a>                 
                </div>-->
            </div>

            <!-- 模板列表 -->
            <ul class="template-list list-style">
                <li class="clear" v-for="template in templateList"  v-cloak>
                    <div class="template-info col-md-10 col-sm-9 col-xs-12 mgb10">
                        <h2 class="template-info-t"><a v-bind:href="'/Application/Models/TemplateModel.aspx?templateId='+template.TemplateID" v-text="template.TemplateName"></a></h2>
                        <div class="create-info clear">
                            <p class="pull-left template-area">适用范围：<span v-text="template.IsPrivate ? '私有模板' : '共有模板' "></span></p>
                            <p class="pull-left create-member">创建人：<span v-text="template.CreateName"></span>（<span v-text="template.CreateLoginName"></span>）</p>
                            <div class="clear">
                                <p>创建日期：<span>{{ template.CreateDate | format-datetime}}</span></p>
                            </div>
                        </div>
                    </div>
                    <div class="template-list-behivar col-md-2 col-sm-3 col-xs-12">
                        <%--for owa--%>
                        <a href="#" class="btn-download form-control" v-on:click="commons.OWA_PreviewAttachment(template.AttachmentId)">预览</a>
                        <%--<a href="#" class="btn-download form-control" v-on:click="commons.DownloadTemplate(template.TemplateID)">下载</a>--%>
                        <a v-bind:href="'/Application/Task/TaskAdd.aspx?templateId='+template.TemplateID" class="btn-delete form-control">发起任务</a>
                    </div>
                </li>
            </ul>
            <div class="list-empty text-center" v-show="total === 0" v-cloak>
                <p><span class="mgr20">您还没有创建模板!</span></p>
            </div>            
            <div class="page-data-pager text-center" v-show="total > 0">
                <vue-pager ref="pager" v-bind:scroll="true" v-bind:page-size="pageSize" v-bind:total="total" v-bind:loading="loading" v-on:pager="load"></vue-pager>
            </div>
        </div>
   </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="page_scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Admin/AdminTemplateList.js") %>"></script>
</asp:Content>

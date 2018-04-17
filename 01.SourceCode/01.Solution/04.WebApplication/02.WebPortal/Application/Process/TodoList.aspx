<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/PageLayout.master" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="WebApplication.WebPortal.Application.Process.TodoList" %>

<asp:Content ContentPlaceHolderID="page_styles" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="page_sidebar" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="page_contents" runat="server">

    <div id="todoList">
        <div class="template-wrap gray-wrap" v-cloak>

            <div class="task-tt" v-if="!isMobile">
                <span class="icon-tag"><i class="fa fa-tag"></i>&nbsp;<span v-cloak v-text="taskType === '1' ? '我的待办' : (taskType==='2'?'我的已办':'我的办结')"></span></span>
            </div>

            <div class="todoList-search clear" v-if="!isMobile">
                <div class="col-md-6 col-xs-6 col-sm-6 mgb15">
                    <input type="text" class="form-control input-search" name="template-name" v-model="keyWord" value="" placeholder="请输入待办/已办名称…" />

                </div>
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15">
                    <div class="select-container" v-cloak>
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
                <div class="col-md-2 col-xs-6 col-sm-2 mgb15">
                    <div class="select-container" v-cloak>
                        <select class="form-control select-item mobile-select" id="listType" v-model="taskType">
                            <option value="1">待办</option>
                            <option value="2">已办 </option>
                            <option value="3">办结 </option>
                        </select>
                        <span class="icon-select"></span>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-6 mgb15">
                    <a href="javascript:void(0);" class="btn btn-info action-search form-control" v-on:click="reload()"><span><i class="fa fa-search"></i>&nbsp;搜索</span></a>
                </div>
            </div>

            <div class="todoList-search clear pdt18" v-if="isMobile">
                <div class="col-md-6 col-xs-12 col-sm-6 mgb15">
                    <input type="text" class="form-control input-search" name="template-name" v-model="keyWord" value="" placeholder="请输入待办/已办名称…" v-on:change="reload()" />

                </div>
                <div class="col-md-3 col-xs-6 col-sm-3 mgb15">
                    <div class="select-container" v-cloak>
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
                <div class="col-md-3 col-xs-6 col-sm-3 mgb15">
                    <div class="select-container" v-cloak>
                        <select class="form-control select-item mobile-select" id="listType" v-model="taskType" v-on:change="reload()">
                            <option value="1">待办</option>
                            <option value="2">已办 </option>
                            <option value="3">办结 </option>
                        </select>
                        <span class="icon-select"></span>
                    </div>
                </div>
            </div>

            <!-- To do 列表 -->
            <ul class="template-list list-style">
                <li class="clear" v-for="todo in todos" v-cloak>
                    <div class="template-info col-md-10 col-sm-8 col-xs-12 mgb15">
                        <h2 class="template-info-t"><a target="_blank" v-bind:href="taskType==='1'? ( todo.TaskAction ==1  ?'/Application/Task/TaskCollection.aspx?IsFromWeb=1&businessId='+todo.BusinessID :'/Application/Task/TaskCollectionView.aspx?businessId='+todo.BusinessID ) : '/Application/Task/TaskCollectionView.aspx?businessId='+todo.BusinessID " v-text="todo.TaskTitle"></a></h2>
                        <div class="create-info clear">
                            <p class="pull-left create-member">创建人：<span v-text="todo.CreatorName"></span></p>
                            <p class="pull-left create-dateTime">创建日期：<span>{{ todo.CreateDate | format-datetime }}</span></p>
                            <p class="pull-left receive-date">接收日期：<span>{{ todo.ReceiveDate | format-datetime }}</span></p>
                        </div>
                    </div>
                </li>
            </ul>
            <div class="list-empty" v-show="total === 0 && !isLoading" v-cloak>
                <p>您还没有待办的填报任务！</p>
            </div>
            <div class="page-data-pager text-center" v-show="total > 0">
                <vue-pager ref="pager" v-bind:scroll="true" v-bind:page-size="pageSize" v-bind:total="total" v-bind:loading="loading" v-on:pager="load"></vue-pager>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="page_scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.select2.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Process/TodoList.js") %>"></script>
</asp:Content>

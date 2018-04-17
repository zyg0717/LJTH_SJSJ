<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.master" AutoEventWireup="true" CodeBehind="DoneList.aspx.cs" Inherits="WebApplication.WebPortal.Application.Process.DoneList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="doneList">
        <div class="template-wrap common-wrap gray-wrap">
            <div class="task-tt">
                <span class="icon-tag"><i class="fa fa-tag"></i>&nbsp;&nbsp;我的已办</span>
                <a href="<%=ResolveUrl("~/Application/Process/TodoList.aspx") %>" class="view-doList">查看我的待办</a>
            </div>

            <div class="todoList-search clear">
                <form action="/" method="post" class="form-line">
                    <div class="col-md-4 col-xs-12 col-sm-4 mgb20">
                        <div class="row">
                            <label class="control-label col-md-3 col-sm-3" style="padding-right: 0;">标题：</label>
                            <div class="col-md-9 col-sm-9">
                                <input type="text" class="form-control input-search" name="template-name" v-model="keyWord" value="" v-on:change="reload()" placeholder="请输入模板名称…" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-xs-12 col-sm-4 mgb20">
                        <div class="row">
                            <label class="control-label col-md-3 col-sm-4" style="padding-right: 0;">接收期间：</label>
                            <div class="select-container col-md-9 col-sm-8" style="padding-left:15px;">
                                <span class="select-t"></span>
                                <select class="form-control  select-item" id="timeRange" v-model="selectedOption" v-on:change="reload()">
                                     <option value=" ">全部 </option>
                                     <option value="1">今天</option>
                                     <option value="2">过去24小时 </option>
                                     <option value="3">过去一周 </option>
                                     <option value="4">过去一个月 </option>
                                     <option value="5">过去三个月 </option>
                                     <option value="6">过去一年 </option>
                                </select>
                            </div>  
                        </div>                  
                    </div>
                    <div class="col-md-4 col-xs-12 col-sm-4 mgb20">
                        <div class="row">
                            <label class="control-label col-md-3 col-sm-3" style="padding-right:0;">查看：</label>
                            <div class="select-container col-md-9 col-sm-9 select-list-view">
                                <span class="select-t"></span>
                                <select class="form-control  select-item">
                                     <option value="1" selected="selected">我的待办</option>
                                     <option value="2">我的已办</option>
                                </select>
                            </div>  
                        </div>                  
                    </div>
                </form>                
            </div>

            <!-- Done 列表 -->
            <ul class="template-list list-style">
                <li class="clear" v-for="item in doneList">
                    <div class="template-info col-md-10 col-sm-8 col-xs-12 mgb15">
                        <h2 class="template-info-t"><a target="_blank" v-bind:href="'/Application/Task/TaskCollectionView.aspx?businessId='+item.BusinessID" v-text="item.TaskTitle"></a></h2>
                        <div class="create-info">
                            <p class="pull-left create-member">创建人：<span v-text="item.CreatorName"></span></p>
                            <p class="pull-left create-dateTime">创建日期：<span>{{ item.CreateDate | format-datetime }}</span></p>
                            <p class="pull-left receive-date">接收日期：<span>{{ item.ReceiveDate | format-datetime }}</span></p>
                        </div>
                    </div>
                </li>
            </ul>
            <div class="list-empty" v-show="total === 0">
               <p>您还没有已办的填报任务！<!--<a class="btn btn-blue new-task" href="/Application/Task/TaskAdd.aspx"><span><i class="fa fa-plus"></i> 新增任务</span></a>--></p>
            </div>         
            <div class="page-data-pager text-center" v-show="total > 0">
              <vue-pager ref="pager" v-bind:scroll="true"  v-bind:page-size="pageSize" v-bind:total="total" v-bind:loading="loading" v-on:pager="load"></vue-pager>
            </div>  
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Process/DoneList.js") %>"></script>
</asp:Content>

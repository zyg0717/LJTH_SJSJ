<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/PageLayout.master" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="WebApplication.WebPortal.Application.Task.TaskList" %>

<asp:Content ContentPlaceHolderID="page_styles" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="page_sidebar" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="page_contents" runat="server">
    <div id="vue" v-cloak>
        <a v-if="isMobile" href="<%=ResolveUrl("~/Application/Task/TaskAdd.aspx") %>" class="icon-add"></a>
        <div v-if="!isMobile" class="widget-header">
            <span class="widget-caption"><i class="fa fa-search"></i>&nbsp;我的任务</span>
            <div class="view-all <%=IsHasAllTask?"":"hidden" %>">
                <a class="icon-task" href="<%=ResolveUrl("~/Application/Admin/AdminTaskList.aspx") %>">全部任务</a>
            </div>
        </div>
        <div class="widget-body clear">
            <div class="row" v-cloak>
                <div v-if="!isMobile" class="search-wrap clear">
                    <div class="col-md-4 col-sm-4 col-xs-4 search-item-input mgb15">
                        <input id="TaskTitle" type="text" v-model="keyWord" autocomplete="off" class="input-search form-control" placeholder="请输入任务标题…" />
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-2 select-feed mgb15" v-cloak>
                        <div class="select-container">
                            <select class="form-control select-item mobile-select" v-model="TimeRange">
                                <option value="">全部日期</option>
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
                    <div class="col-md-2 col-sm-2 col-xs-2 select-status mgb15" v-cloak>
                        <div class="select-container">
                            <select class="form-control select-item mobile-select" id="taskType" v-model="currentType">
                                <option value="1">当前任务</option>
                                <option value="3">计划任务</option>
                                <option value="2">归档任务</option>
                            </select>
                            <span class="icon-select"></span>
                        </div>
                    </div>

                    <div class="col-md-2 col-sm-2 col-xs-2 btn-search mgb15">
                        <a v-on:click="search" class="btn btn-info action-search pull-right" href="javascript:void(0);"><span><i class="fa fa-search"></i>&nbsp;搜索</span></a>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-2 btn-task mgb15">
                        <%--<a class="btn btn-info pull-right create-task" href="/Application/Task/TaskAdd.aspx"><span><i class="fa fa-plus"></i>&nbsp;新增任务</span></a>--%>
                        <a class="btn btn-info pull-right create-task" href="#" v-on:click="isTask = true"><span><i class="fa fa-plus"></i>&nbsp;新增任务</span></a>
                    </div>

                </div>
                <div v-if="isMobile" class="search-wrap mobile-search-wrap clear pdt5">
                    <div class="col-md-6 col-sm-6 col-xs-12 mgb15">
                        <input id="TaskTitle" type="text" v-model="keyWord" autocomplete="off" class="input-search form-control" placeholder="请输入任务标题…" v-on:change="reload()" />
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-6 mgb15" v-cloak>
                        <div class="select-container">
                            <select class="form-control select-item mobile-select" v-model="TimeRange" v-on:change="reload()">
                                <option value="">全部日期</option>
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
                    <div class="col-md-3 col-sm-3 col-xs-6 mgb15" v-cloak>
                        <div class="select-container">
                            <select class="form-control select-item mobile-select" id="taskType" v-model="currentType" v-on:change="reload()">
                                <option value="1">当前任务</option>
                                <option value="3">计划任务</option>
                                <option value="2">归档任务</option>
                            </select>
                            <span class="icon-select"></span>
                        </div>
                    </div>
                </div>

                <div class="list-tags clear" v-if="currentTypeValue == 1" v-bind:class="{'mobile-tags': isMobile}">
                    <ul class="col-md-12 col-sm-12 col-xs-12" v-cloak>
                        <li class="tag total active mgb15" v-on:click="reload('total')"><span>全部</span><span class="task-count" v-cloak v-if="tab">({{ total > 99 ? "99+" : total }})</span>
                            <input type="hidden" value=""></li>
                        <li class="tag created mgb15" v-on:click="reload('created')"><span>未发起</span><span class="task-count" v-cloak v-if="tab">({{ total > 99 ? "99+" : total }})</span>
                            <input type="hidden" value="0"></li>
                        <li class="tag processing mgb15" v-on:click="reload('processing')"><span>进行中</span><span class="task-count" v-cloak v-if="tab">({{ total > 99 ? "99+" : total }})</span>
                            <input type="hidden" value="1"></li>
                        <li class="tag completed mgb15" v-on:click="reload('completed')"><span>已完成</span><span class="task-count" v-cloak v-if="tab">({{ total > 99 ? "99+" : total }})</span>
                            <input type="hidden" value="3"></li>
                        <li class="tag terminated mgb15" v-on:click="reload('terminated')"><span>已终止</span><span class="task-count" v-cloak v-if="tab">({{ total > 99 ? "99+" : total }})</span>
                            <input type="hidden" value="2"></li>
                    </ul>
                </div>
            </div>

            <div class="page-body">
                <div class="page-data-list">
                    <ul class="tasklist clear" id="MyTask">
                        <li class="taskitem clear" v-for="task in taskList" v-cloak>
                            <div class="template-info col-md-10 col-sm-9 col-xs-12">
                                <div class="row">
                                    <div class="task-schedule col-md-2 col-sm-3 col-xs-3">
                                        <vue-circle v-bind:complete-count="task.CompleteUserNodeCount" v-bind:type="task.TaskType" v-bind:status="task.TaskStatus" v-bind:is-mobile="isMobile" v-bind:sum-count="task.TaskUserNodeCount" v-bind:task-id="task.TaskID"></vue-circle>
                                    </div>
                                    <div class="tasktitle col-md-9 col-sm-8 col-xs-8">
                                        <h2 class="text">
                                            <a v-bind:href="'/Application/Task/TaskAdd.aspx?taskId='+task.TaskID + '&TaskTemplateType=' + task.TaskTemplateType" v-if="task.TaskStatus==0||task.TaskType==2" v-text="task.TaskName"></a>
                                            <a v-bind:href="'/Application/Task/TaskInfo.aspx?taskId='+task.TaskID + '&TaskTemplateType=' + task.TaskTemplateType" v-if="!(task.TaskStatus==0||task.TaskType==2)" v-text="task.TaskName"></a>
                                        </h2>
                                        <div class="cost-time">
                                            <span class="current-status" v-if="task.TaskStatus==0">未发起</span>
                                            <span class="current-status" v-if="task.TaskStatus==1">进行中</span>
                                            <span class="current-status" v-if="task.TaskStatus==2">已终止</span>
                                            <span class="current-status" v-if="task.TaskStatus==3">已完成</span>
                                            <span class="current-status" v-if="task.TaskStatus==4">已归档</span>
                                            <span class="creat-time" v-cloak>创建时间：{{task.TaskCreateDate | format-dateminute }}</span>
                                            <span class="desc" v-if="task.TaskStatus!=2&&task.TaskType!=2&&task.TaskType!=0" v-cloak>执行耗时：{{task.TaskTakingTime | format-datediff}}</span>
                                            <span class="creat-time" v-if="task.TaskType==2&&(task.TaskStatus==0||task.TaskStatus==1)" v-cloak>下次执行时间：{{task.NextTaskApplyDate | format-dateminute}}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="template-list-behivar col-md-2 col-sm-3 col-xs-12">
                                <button type="button" class="btn-download form-control" v-bind:class="{'btn-disabled': !(task.TaskType!=2&&task.TaskStatus!=0)}" v-bind:disabled="!(task.TaskType!=2&&task.TaskStatus!=0)" v-on:click="downloadTaskFile(task)"><span>下载汇总</span></button>
                                <button class="btn-download form-control" v-bind:class="{'btn-disabled':!(task.TaskStatus!=1)}" v-bind:disabled="!(task.TaskStatus!=1)" v-on:click="FilingTask(task.TaskID)"><span>任务归档</span></button>
                            </div>
                            <input type="hidden" name="taskId" v-bind:value="task.TaskID" />
                        </li>
                    </ul>
                </div>
                <div class="list-empty" v-show="total === 0 && !isLoading" v-cloak>
                    <p>您还没有创建任务！</p>
                </div>
                <div class="page-data-pager">
                    <vue-pager v-bind:scroll="true" ref="pager" v-bind:page-size="pageSize" v-bind:total="total" v-bind:loading="loading" v-on:pager="load"></vue-pager>
                </div>
            </div>
            <div v-if="isTask" v-clock>
                <div class="task-mask" v-on:click="taskMask(event)"></div>
                <div class="task-model">
                    <div class="task-model-hd">
                        <span class="task-model-title">任务类型</span>
                        <span class="task-model-close" v-on:click="isTask = false">关闭</span>
                    </div>
                    <div class="task-model-bd">
                        <a class="btn btn-info pweui-dialog__bd ull-right create-task" href="/Application/Task/TaskAdd.aspx?TaskTemplateType=0"><span>&nbsp;&nbsp;数据汇总&nbsp;&nbsp;</span></a>
                    </div>
                    <div class="task-model-bd">
                        <a class="btn btn-info pweui-dialog__bd ull-right create-task" href="/Application/Task/TaskAdd.aspx?TaskTemplateType=2"><span>&nbsp;&nbsp;数据更新&nbsp;&nbsp;</span></a>
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
                        <div class="col-md-9 col-sm-9 mgb10" v-text="'本次共有'+taskFileDialog.content.data.total+'人填报，'+taskFileDialog.content.data.finish+'人已完成填报'">
                        </div>
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
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="page_scripts" runat="server">
    <script type="text/template" id="circle">

        <div class='processingbar'>
            <span v-if="status==0||type==2">
                <span class="task-initiate" v-if="status==0&&type!=2" v-on:click="SubmitTask()"></span>
                <a v-if="type==2" v-bind:href="'/Application/Task/TaskAdd.aspx?taskId='+taskId"><span class="task-scheduled"></span></a>
            </span>
            <span v-if="!(status==0||type==2)">
                <a v-bind:href="'/Application/Task/TaskInfo.aspx?taskId='+taskId">
                    <span v-text="percent+'%'" v-if="status!=0"></span>
                </a>
            </span>
        </div>
    </script>

    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.circle.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Task/TaskList.js?v=")+System.Guid.NewGuid() %>"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("click", '.tag', function () {
                $(this).addClass("active").siblings(".tag").removeClass("active");
            });
        });
    </script>
</asp:Content>

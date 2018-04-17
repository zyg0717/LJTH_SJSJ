<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.Master" AutoEventWireup="true" CodeBehind="TemplateModel.aspx.cs" Inherits="WebApplication.WebPortal.Application.Models.TemplateModel" %>

<%@ Register Src="~/Application/Models/Control/ucExcel.ascx" TagPrefix="uc1" TagName="ucExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
    <link href="<%=ResolveUrl("~/Assets/styles/sheet.css") %>" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/Assets/styles/excel.css") %>" rel="stylesheet" />
    <style type="text/css">
        input[disabled] {
            background-color: transparent;
        }
    </style>
    <uc1:ucExcel runat="server" ID="ucExcel" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div id="templateModel" v-cloak>
        <div class="task-wrap common-wrap template-wrap">
            <div class="task-tt task-top">编辑模板</div>
            <div class="form-group clear" v-if="!Edit && !isMobile">
                <label class="label-control col-md-1 label-control-tt">创建方式：</label>
                <div class="col-md-10">
                    <input type="radio" name="templateModel" id="createModel" value="0" v-model="modelChecked" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="createModel">
                        创建新模板
                    </label>
                    <input type="radio" name="templateModel" id="cloneModel" value="1" v-model="modelChecked" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="cloneModel">
                        克隆系统模板
                    </label>
                    <input type="radio" name="templateModel" id="localModel" value="2" v-model="modelChecked" onkeydown="if(event.keyCode==13)return false;" />
                    <label class="radio-inline" for="localModel">
                        使用本地模板
                    </label>
                    <a href="#" v-on:click="selectTemplate" v-show="cloneModelBtn" class="btn btn-purple system-model">克隆系统模板</a>
                </div>
            </div>
            <div v-if="isMobile">
                <div class="form-group clear" v-show="Edit">
                    <label class="label-control col-md-1 label-control-tt">模板标题：</label>
                    <div class="col-md-10">
                        <input type="text" v-bind:disabled="Edit" class="form-control" v-bind:class="{'form-disabled': Edit}" placeholder="仅在系统内显示，用于模版管理、任务创建等" v-model="TemplateName" onkeydown="if(event.keyCode==13)return false;" />
                    </div>
                </div>
            </div>
            <div v-if="!isMobile">
                <div class="form-group clear">
                    <label class="label-control col-md-1 label-control-tt">模板标题：</label>
                    <div class="col-md-10">
                        <input type="text" v-bind:disabled="Edit" class="form-control" v-bind:class="{'form-disabled': Edit}" placeholder="仅在系统内显示，用于模版管理、任务创建等" v-model="TemplateName" onkeydown="if(event.keyCode==13)return false;" />
                    </div>
                </div>
            </div>

            <!-- excel表格 -->
            <div class="mobile-excelcontainer" v-if="isMobile">
                <div class="mobile-exceltips" v-show="Edit">
                    <p>暂不支持移动端在线编辑，请使用预览功能进行查看!</p>
                </div>
                <div class="mobile-exceltips mobile-edit-exceltips" v-show="!Edit">
                    <p>暂不支持移动端在线创建！</p>
                </div>
            </div>
            <div id="div_container" v-if="!isMobile">
                <div id="div_excelcontainer" v-show="excelEdit">
                    <div class="mgb10">
                        <vue-excel ref="excel" v-bind:sheets="sheets" v-bind:options="options"></vue-excel>
                    </div>
                </div>
                <!--上传模板-->
                <div id="div_upload" class="clear mgb15" v-show="modelUpdate">
                    <div class="col-md-12">
                        <div class="clone-cont">
                            <div class="upload-des" style="background:none">
                                <p class="des-t" style="color: #cb5c61">上传自定义模板说明：</p>
                                <p class="des-c" style="color: #cb5c61">自定义模板相对于“在线模板”功能，您可以从您的计算机上传Excel文件，我们会帮您将它发给填报用户。您只需要在设计模板的时候预留好空格即可。让我们开始吧！</p>
                            </div>
                            <div class="clone-step step-one" v-show="!fileUpload">
                                <h2 class="step-t clear"><span class="mgr20 pull-left">第一步 新建Excel模板并上传 </span><span class="pull-left color-red">注：这一步需要您在自己的计算机上完成</span></h2>
                                <div class="row">
                                    <div class="col-md-8">
                                        <img class="excel-img" src="<%=ResolveUrl("~/Assets/images/step1.png") %>" />
                                    </div>
                                    <div class="col-md-4 mgb20">
                                        <ul class="step-des">
                                            <li>按您熟悉的方式设计模板
                                                <span>>会excel操作就行</span>
                                                <span>>全程可视化操作</span>
                                            </li>
                                            <li>确定好数据区域</li>
                                        </ul>
                                        <p class="step-tips color-red">本地保存后，点此上传Excel</p>
                                        <div class="file-upload">
                                            <input type="file" id="fileUpload" name="file" value="file" onkeydown="if(event.keyCode==13)return false;" />
                                            <span class="file-upload-btn">上传模板</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clone-step step-two" v-show="fileUpload">
                                <h2 class="step-t clear"><span class="mgr20 pull-left">第二步 设定数据区域 </span></h2>
                                <div class="row">
                                    <div class="col-md-8">
                                        <img class="excel-img" src="<%=ResolveUrl("~/Assets/images/step2.png") %>" />
                                    </div>
                                    <div class="col-md-4 mgb20">
                                        <ul class="step-des mgb5">
                                            <li style="font-weight: bold;">查看上传的模板</li>
                                            <li style="font-weight: bold;">找到数据区域的起点</li>
                                        </ul>
                                        <div class="clear" style="position: relative;">
                                            <p class="data-start" style="font-weight: bold;">数据区域起点</p>
                                            <br />
                                            <div class="data-area clear mgb10" v-bind:class="'data-area-'+index+1" v-for="(sheet,index) in sheets">
                                                <span class="sheet-name pull-left" v-bind:title="sheet.name" v-text="sheet.name+':'"></span>
                                                <input v-model="sheet.rangePos" class="form-control txt-area pull-left" type="text" placeholder="如：B4" style="margin-left: 10px; display: inline; width: 80px;" onkeydown="if(event.keyCode==13)return false;" />
                                                <span class="pull-left">
                                                    <a href="javascript:void(0);" class="img-remove pull-left" title="删除后该sheet页不参与数据汇总，但还会显示在模板中" v-on:click="removeSheet(sheet)">删除</a>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="step-action">
                                            <%--<span class="btn btn-white prev-step" v-on:click="goPrevStep()">上一步</span>
                                            <span class="btn btn-blue next-step" v-on:click="goNextStep()">下一步</span>--%>
                                            <span class="btn prev-step" style="border: 1px solid #cb5c61;color:#cb5c61;" v-on:click="goPrevStep()">上一步</span>
                                            <span class="btn btn-red next-step" style="border:0px solid;" v-on:click="goNextStep()">下一步</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div v-if="isMobile">
                <div class="template-action text-center clear" v-show="Edit">
                    <button v-if="Edit" v-on:click="Delete()" class="btn btn-blue btn-delete">删除</button>
                    <button v-on:click="goDataRange()" v-if="prevBtn" class="btn prev-step" style="border: 1px solid #cb5c61;color:#cb5c61; background: #fff">上一步</button>
                    <%--<button v-on:click="goDataRange()" v-if="prevBtn" class="btn btn-white prev-step">上一步</button>--%>
                    <%--<button v-on:click="PreView()" v-if="previewBtn" class="btn btn-white btn-view">预览</button>--%>
                </div>
            </div>
            <div v-if="!isMobile">
                <div class="template-action text-center clear">
                    <button v-if="Edit" v-on:click="Delete()" class="btn btn-blue btn-delete">删除</button>
                    <button v-on:click="goDataRange()" v-if="prevBtn" class="btn prev-step" style="border: 1px solid #cb5c61;color:#cb5c61;background: #fff;">上一步</button>
                    <%--<button v-on:click="goDataRange()" v-if="prevBtn" class="btn btn-white prev-step">上一步</button>--%>
                    <%--<button v-on:click="PreView()" v-if="previewBtn" class="btn btn-white btn-view">预览</button>--%>
                    <%--<button v-on:click="Save()" class="btn btn-blue btn-submit">保存</button>--%>
                    <button v-on:click="Save()" class="btn btn-red btn-submit" style="border:1px solid #cb5c61;">保存</button>
                    <button v-on:click="Clear()" class="btn prev-step" style="border: 1px solid #cb5c61;color:#cb5c61;background:#fff;opacity:1;"  v-if="columnNameFilter('show')">清除</button>
                </div>
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
                                <a href="javascript:void(0);" v-on:click="dialogReload" class="btn btn-info action-search form-control"><span><i class="fa fa-search"></i>&nbsp;&nbsp;搜索</span></a>
                            </div>
                        </div>
                    </div>
                    <div class="template-dialog table-responsive" v-if="!isMobile">
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

                    <ul v-if="isMobile" class="template-dialog fill-info-wrap" v-cloak>
                        <li class="fill-list clear" v-for="(node,i) in dialog.content.data" style="cursor: pointer;" v-on:click="selectRow(node)">
                            <p class="pull-left"><span class="fill-t">模板名称：</span><span class="fill-c" v-text="node.TemplateName"></span></p>
                            <p class="pull-left"><span class="fill-t">创建人：</span><span class="fill-c" v-text="node.CreateName"></span></p>
                            <p class="pull-left"><span class="fill-t">创建时间：</span><span class="fill-c">{{ node.CreateDate | format-datetime }}</span></p>
                        </li>
                    </ul>
                    <div v-show="dialog.content.total>0">
                        <vue-pager ref="pager" v-bind:scroll="false" page-size="15" v-bind:total="dialog.content.total" v-on:pager="loadTemplates"></vue-pager>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.ui.widget.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.iframe-transport.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/vendors/ajax-fileupload/js/jquery.fileupload.ext.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.excel.js") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/Models/TemplateModel.js?v=3") %>"></script>
    <script src="<%=ResolveUrl("~/Assets/scripts/common/components/vue.pager.js") %>"></script>
</asp:Content>

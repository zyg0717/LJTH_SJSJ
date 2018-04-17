<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucExcel.ascx.cs" Inherits="WebApplication.WebPortal.Application.Models.Control.ucExcel" %>

<script type="text/template" id="ucExcel">
    <div id="div_Excel" class="clear">
        <div class="tabs-box clear">
            <div class="col-md-12 clear">
                <ul class="nav pull-left" style="position: relative; top: -1px; z-index: 2;">
                    <li v-bind:class="{active:sheet.select}" v-for="sheet in excel" v-on:click="tabSheet(sheet)">
                        <div class="btn1 btn1-nav">
                            <input type="text" v-on:click.stop="" v-on:keyup.esc="undoRenameSheet(sheet)" v-on:blur="doRenameSheet(sheet,$event)" v-bind:value="sheet.changeflag?sheet.name:''" v-if="sheet.edit" onkeydown="if(event.keyCode==13)return false;" />
                            <span v-text="sheet.name" v-if="!sheet.edit"></span>
                            <img class="img-edit" v-if="options.renamesheet&&!sheet.edit" src="<%=ResolveUrl("~/Assets/images/edit.png") %>" style="position: relative; z-index: 9999;" width="13" height="auto" v-on:click.stop="editSheet(sheet)" />
                            <img class="img-del" v-if="options.deletesheet&&!sheet.edit" src="<%=ResolveUrl("~/Assets/images/icon-delet2.png") %>" v-on:click.stop="removeSheet(sheet)" style="position: relative; z-index: 9999;" width="10" />
                        </div>
                    </li>

                </ul>
                <a class="addicon" href="javascript:void(0)" style="width: 21px; display: block; float: left; font-weight: bold; text-align: center; color: #666; margin-top: 8px;" v-on:click="addSheet()" v-if="options.addsheet">
                    <div style="width: 21px; height: 21px; position: relative; text-align: center; line-height: 22px;">+</div>
                </a>
            </div>
            <!--填写 excel 表格-->
            <div class="tabs-content col-md-12">
                <div v-bind:class="{tabs:true,hidden:!sheet.select}" v-for="sheet in sheets">
                    <div class="content-left text-right" style="padding-top: 72px; line-height: 24px; font-size: 12px; text-align: right;width:100px;">
                        表头：<br>
                        示例：<br>
                        数据区域第
                        <%--<span v-text="excelHelper.ParseCode(sheet.firstcolumn)+sheet.firstrow"></span>--%>
                        <span v-text="sheet.firstrow+'行：'"></span>
                    </div>
                    <div class="table-responsive">
                        <table class="table excel">
                            <colgroup>
                                <col style="width: 20px;" />
                                <template v-for="column in sheet.columns">
                                <col style="width: 65px;" />
                                </template>
                                <col style="width: 20px;" />
                            </colgroup>
                            <caption></caption>
                            <thead>
                                <tr style="background-color: #d9d9d9;">
                                    <th style=""></th>
                                    <template v-for="(column,c) in sheet.columns">
                                        <th v-bind:class="{'th-col':true,'th-col-selected':column.select}" v-on:click="defineColumn(sheet,column)">{{column.code}}<img v-on:click.stop="removeColumn(sheet,column)" v-if="options.deletecolumn" src="<%=ResolveUrl("~/Assets/images/icon-delet2.png") %>" width="10" /></th>
                                    </template>
                                    <th v-if="options.addcolumn" style="background-color: #666; cursor: pointer; color: #fff; vertical-align: middle" v-on:click="appendColumn(sheet)"><span style="position: relative; top: -2px;">+</span></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="width: 22px;">1</td>
                                    <td colspan="12" v-bind:colspan="sheet.columns.length" class="text-center title-td" style="position: relative; height: 36px;">
                                        <input type="text" class="moban-title text-center" v-if="options.sheettitle" placeholder="请在此输入标题" v-model="sheet.title" v-bind:disabled="!options.sheettitle" onkeydown="if(event.keyCode==13)return false;" />
                                        <%--<div class="delicon" style="position: absolute; height: 34px; width: 36px; background-color: #fff; right: 1px; top: 1px;"></div>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>2</td>
                                    <template v-for="(column,c) in sheet.columns">
                                        <td >
                                            <input  style="background-color:#d9d9d9;"  type="text"  v-on:click="defineColumn(sheet,column)" v-bind:disabled="!options.designcolumn" v-model="column.name" v-bind:title="column.name"  v-bind:placeholder="'列'+(c+1)" onkeydown="if(event.keyCode==13)return false;" />
                                        </td>
                                    </template>
                                </tr>
                                <tr v-for="(row,r) in sheet.rows">
                                    <td v-text="r+3"></td>
                                    <template v-for="(cell,index) in row.cells">
                                        <td>
                                        <input type="text" v-bind:disabled="!options.designdatarow" v-model="cell.content" onkeydown="if(event.keyCode==13)return false;" /></td>
                                    </template>
                                </tr>
                                <tr v-if="options.viewremark">
                                    <td>...</td>
                                    <td v-bind:colspan="sheet.columns.length" style="padding-bottom: 20px;">
                                        <ul class="mobanbox clear row">
                                            <li class="strong mobanbox-t col-md-2 mgb15">填表说明：</li>
                                            <li class="col-md-9 mobanbox-c">
                                                <textarea class="form-control" v-bind:disabled="!options.sheetremark" style="height: 98px;" placeholder="如有填表说明请在此输入" v-model="sheet.remark"></textarea></li>
                                        </ul>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!--编辑excel表格属性-->
        <template v-for="(sheet,s) in excel">
            <template v-for="(column,c) in sheet.columns">
                <div class="col-md-12 tabs-box div-column-define" v-if="column.select&&sheet.select&&$.trim(column.name)!=''">
            <ul class="sr-only clear">
                <li :class="{'tab-active':isActive}" v-on:click="tabActive('code')">单元格属性&nbsp;<span class="col-name" style="color: red;" v-text="column.code"></span></li>
                <li :class="{'tab-active':!isActive}" v-on:click="tabActive()"><span class="col-name">批量必填设置</span></li>
            </ul>
            <div class="tabs-content clear excel-config">
                <div class="tabs-border table-responsive">
                    <table class="table" v-if="isActive">
                        <tbody>
                            <tr>
                                <td class="td-t">字段类型：</td>
                                <td class="field-type" style="width: 300px;">
                                    <input v-bind:disabled="!options.designcolumndefine" type="radio" name="field-type" id="field-1" value="Text" v-model="column.type" onkeydown="if(event.keyCode==13)return false;" />
                                    <label class="radio-inline" for="field-1" v-bind:class="{'radio-disabled': !options.designcolumndefine}">
                                        文本
                                    </label>
                                    <input v-bind:disabled="!options.designcolumndefine" type="radio" name="field-type" id="field-2" value="DateTime"  v-model="column.type" onkeydown="if(event.keyCode==13)return false;" />
                                    <label class="radio-inline" for="field-2" v-bind:class="{'radio-disabled': !options.designcolumndefine}">
                                        日期
                                    </label>
                                    <input v-bind:disabled="!options.designcolumndefine" type="radio" name="field-type" id="field-3" value="Number"  v-model="column.type" onkeydown="if(event.keyCode==13)return false;" />
                                    <label class="radio-inline" for="field-3" v-bind:class="{'radio-disabled': !options.designcolumndefine}">
                                        数字
                                    </label>
                                </td>
                                <td class="td-t">小数位数：</td>
                                <td style="width: 180px;" v-cloak>
                                   <div class="select-container" style="width: 180px; float:left;">
                                       <select v-bind:disabled="!options.designcolumndefine" class="width200 field-digit form-control mobile-select" v-bind:class="{'form-disabled': !options.designcolumndefine}" style="width: 180px;" v-model="column.digit" >
                                            <option value="0">0</option>
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            <option value="10">10</option>
                                        </select>
                                       <span class="icon-select"></span>
                                    </div>
                                </td>
                                <td class="td-t">是否必填：</td>
                                <td>
                                    <input id="chkRequire" v-bind:disabled="!options.designcolumndefine&&!options.designisrequired" type="checkbox" class="field-required control-label chk"  v-model="column.required" onkeydown="if(event.keyCode==13)return false;" />
                                    <label class="chk-label empty-label" for="chkRequire" v-bind:class="{'chk-disabled': !options.designcolumndefine&&!options.designisrequired }"></label>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-t">可选值：</td>
                                <td>
                                    <input v-bind:disabled="!options.designcolumndefine" class="width200 field-range form-control" v-bind:class="{'form-disabled': !options.designcolumndefine}"  type="text" v-model="column.range" placeholder="例如：A,B,C" onkeydown="if(event.keyCode==13)return false;" />
                                </td>
                                <td class="td-t">背景色：</td>
                                <td colspan="3" v-cloak>
                                    <div class="select-container pull-left" style="width: 180px;">
                                        <select v-bind:disabled="!options.designcolumndefine" class="width200 field-bgcolor form-control mobile-select" v-bind:class="{'form-disabled': !options.designcolumndefine }" style="width: 180px; vertical-align: middle;display:inline-block" v-model="column.bgcolor">
                                            <option value="255,255,255">无</option>
                                            <option value="255,165,0">橙色</option>
                                            <option value="255,0,0">红色</option>
                                            <option value="255,255,0">黄色</option>
                                            <option value="144,238,144">浅绿色</option>
                                            <option value="0,255,0">绿色</option>
                                            <option value="193,210,240">浅蓝色</option>
                                            <option value="0,0,255">蓝色</option>
                                            <option value="0,31,86">深蓝色</option>
                                            <option value="160,32,240">紫色</option>
                                            <option value="216,0 ,5">深红</option>
                                        </select>
                                        <span class="icon-select"></span>
                                    </div>
                                    <div class="bgcolor-tips" v-bind:style="{backgroundColor: 'rgb('+column.bgcolor+')'}"></div>
                                    <!--<div class="bgcolor-tips" v-bind:class="'bgcolor'+ column.bgcolor "></div>-->
                                </td>
                            </tr>

                            <tr v-if="column.isformula">
                                <td class="td-t" style="width: 100px !important">原模板公式：</td>
                                <td colspan="3">
                                    <input v-bind:disabled="true" class="width200 field-range form-control form-disabled" type="text" v-model="column.tempformula"/>
                                </td>
                            </tr>
                            <tr v-if="column.isformula">
                                <td class="td-t" style="width: 100px !important">设置公式：</td>
                                <td colspan="3">
                                    <input class="width200 field-range form-control" type="text" v-model="column.cellformula"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table class="table" v-if="!isActive">
                        <tbody>
                            <tr>
                                <td class="td-t">选择列：</td>
                                <td colspan="5">
                                    <div class="select-container pull-left" style="width: 100px;">
                                        <select  class="width200 field-bgcolor form-control mobile-select" v-model="sheet.startSort" v-on:change="sortChange(sheet.startSort)" style="width: 100px; vertical-align: middle;display:inline-block">
                                            <option value="-1">请选择</option>
                                            <template v-for="(startCode, i) in sheet.columns">
                                                <option v-if="startCode.name.replace(/\s+/g,'')" :value="startCode.sort">{{startCode.code}}</option>
                                           </template>
                                        </select>
                                        <span class="icon-select"></span>
                                    </div>
                                    <div class="select-container pull-left" style="line-height:30px;width:30px;text-align:center;">~</div>
                                    <div class="select-container pull-left" style="width: 100px;">
                                        <select  class="width200 field-bgcolor form-control mobile-select" v-model="sheet.endSort" style="width: 100px; vertical-align: middle;display:inline-block">
                                            <option value="-1">请选择</option>
                                            <template v-for="(endCode, i) in sheet.columns">
                                                <option v-if="endCode.sort >= sort && endCode.name.replace(/\s+/g,'')" :value="endCode.sort">{{endCode.code}}</option>
                                           </template>
                                        </select>
                                        <span class="icon-select"></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-t">必填列：</td>
                                <td colspan="6">
                                     <div class="select-container pull-left">
                                        {{checkedRequireFilter(sheet) ? checkedRequireFilter(sheet) : "无"}}
                                     </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
            </template>
        </template>
    </div>
</script>

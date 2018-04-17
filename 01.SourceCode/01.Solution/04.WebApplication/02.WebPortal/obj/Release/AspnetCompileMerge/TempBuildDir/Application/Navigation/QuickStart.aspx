<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster/Layout.Master" AutoEventWireup="true" CodeBehind="QuickStart.aspx.cs" Inherits="WebApplication.WebPortal.Application.Navigation.QuickStart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contents" runat="server">
    <div class="wrapper">
	    <div class="common-inner">
		    <p class="stepline-tips">
			    <span>让我们开始数据收集之旅吧！</span>
		    </p>
		    <ul class="stepline">
				<span class="line"></span>
			    <li>
				    <div class="stepline-badge badge1 active current">
                        <div class="inner-badge">                            
                            <a href="/Application/Models/TemplateModel.aspx"></a>
                        </div>
				    </div>
				    <div class="stepline-panel panel1">
					    <div class="stepline-panel-c clear">
                            <div class="step-action active">
							    <span class="step-t">第一步 新增模板</span>
						    </div>
						    <p class="clear"><span class="fl"> 发起任务前，需先创建一个模板。</span><a class="btn-create fr btn btn-success white" href="/Application/Models/TemplateModel.aspx"><span><i class="fa fa-plus"></i> 新增模板</span></a></p>
					    </div>
					    <span class="arrow"></span>
				    </div>
			    </li>

			    <li class="stepline-inverted">
				    <div class="stepline-badge badge2 current">
                        <div class="inner-badge"> 
                            <a href="/Application/Task/TaskAdd.aspx"></a>
                        </div>
				    </div>
				    <div class="stepline-panel panel2">
					    <div class="stepline-panel-c clear">
                            <div class="step-action active">
							    <span class="step-t">第二步 新增任务</span>
						    </div>
						    <p class="clear"><span class="fl">有了模板，我们可以使用模板创建一个任务。</span><a class="btn-create fr btn btn-info" href="/Application/Task/TaskAdd.aspx"><span><i class="fa fa-plus"></i> 新增任务</span></a></p>
					    </div>
					    <span class="arrow"></span>
				    </div>
			    </li>

			    <li>
				    <div class="stepline-badge badge3">
                        <div class="inner-badge"><a href="javascript: void(0)"></a></div>
				    </div>
				    <div class="stepline-panel panel3">
					    <div class="stepline-panel-c clear">
                            <div class="step-action">
							    <span class="step-t">第三步 下发任务</span>
						    </div>
						    <p class="clear">选择模板发起任务后接收人会收到待办。</p>
					    </div>
					    <span class="arrow"></span>
				    </div>
			    </li>

			    <li class="stepline-inverted">
					<div class="stepline-badge badge4">
					    <div class="inner-badge"><a href="javascript: void(0)"></a></div>
				    </div>
				    <div class="stepline-panel panel4">
					    <div class="stepline-panel-c clear">
                            <div class="step-action">
							    <span class="step-t">第四步 等待填报</span>
						    </div>
						    <p class="clear">接收人下载模板，填报数据，再上传到系统中。</p>
					    </div>
					    <span class="arrow"></span>
				    </div>
			    </li>

			    <li class="last-list">
				    <div class="stepline-badge badge5">
                        <div class="inner-badge"><a href="javascript: void(0)"></a></div>
				    </div>
				    <div class="stepline-panel panel5">
					    <div class="stepline-panel-c clear">
                            <div class="step-action">
						        <span class="step-t">第五步 下载汇总文件</span>
						    </div>
						    <p class="clear">下发人可以随时下载汇总文件。</p>
					    </div>
					    <span class="arrow"></span>
				    </div>
			    </li>

		    </ul>
	    </div>
	</div>
	<!-- wrapper end -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#navbar-menu li").removeClass('active').filter('.quick-start').addClass("active");
        })
    </script>
</asp:Content>

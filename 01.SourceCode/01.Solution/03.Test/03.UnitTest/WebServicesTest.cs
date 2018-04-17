using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin.OAMessage;

namespace Test.UnitTest
{
    /// <summary>
    /// WebServicesTest 的摘要说明
    /// </summary>
    [TestClass]
    public class WebServicesTest
    {


        [TestMethod]
        public void TestTodo()
        {
            string flowid = Guid.NewGuid().ToString();
            string flowtitle = "测试发送待办5";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            var url = "/Application/Task/TaskList.aspx";
            string creator = "chengeng";
            string receiver1 = "wangxin";
            string receiver2 = "yangxiaogang";
            OAMessageBuilder.ReceiveTodo(flowid, flowtitle, workflowname, nodename1, url, url, creator, receiver1);
            OAMessageBuilder.ReceiveTodo(flowid, flowtitle, workflowname, nodename2, url, url, creator, receiver2);
        }
        [TestMethod]
        public void TestDone()
        {
            string flowid = "f06c7113-ef64-4983-be56-4ade5160a629";
            string receiver = "yangxiaogang";
            string flowtitle = "测试发送待办1";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            OAMessageBuilder.ReceiveDone(flowid, flowtitle, nodename2, receiver, "", "");
        }
        [TestMethod]
        public void TestToDo1()
        {
            string flowid = "3fff5873-b9c4-4b7d-9f8b-f972ed21073";
            string flowtitle = "测试发送待办2";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            var url = "/Application/Task/TaskList.aspx";
            string creator = "chengeng";
            string receiver1 = "wangxin";
            string receiver2 = "yangxiaogang";
            OAMessageBuilder.ReceiveTodo(flowid, flowtitle, workflowname, nodename2, url, url, creator, receiver2);
        }
        [TestMethod]
        public void TestOver()
        {
            string flowid = "f06c7113-ef64-4983-be56-4ade5160a629";
            string receiver = "yangxiaogang";
            string flowtitle = "测试发送待办1";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            OAMessageBuilder.ReceiveOver(flowid, flowtitle, nodename2, receiver, "", "");
        }

        [TestMethod]
        public void TestCancel()
        {
            string flowid = "3fff5873-b9c4-4b7d-9f8b-f972ed21073b";
            string flowtitle = "测试发送待办2";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            var url = "/Application/Task/TaskList.aspx";
            string creator = "chengeng";
            string receiver1 = "wangxin";
            string receiver2 = "yangxiaogang";
            OAMessageBuilder.Cancel(flowid, receiver2);
        }
        [TestMethod]
        public void TestCancelProcess()
        {
            string flowid = "d87a7991-843e-4c70-abdc-007a435b32ef";
            string flowtitle = "测试发送待办2";
            string workflowname = "收集任务填报";
            string nodename1 = "填报";
            string nodename2 = "审批";
            var url = "/Application/Task/TaskList.aspx";
            string creator = "chengeng";
            string receiver1 = "wangxin";
            string receiver2 = "yangxiaogang";
            OAMessageBuilder.CancelProcess(flowid);
        }
    }
}

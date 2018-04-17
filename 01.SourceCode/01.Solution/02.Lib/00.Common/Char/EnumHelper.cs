using Framework.Core.Cache;
using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Lib.Common
{
    public static class EnumHelper
    {
        /// <summary>
        /// 绑定记录类型到下拉框
        /// </summary>
        /// <param name="ddl">要绑定的控件</param>
        /// <param name="enumType">要绑定的枚举类型</param>
        /// <param name="hasAll">是否需要添加第一项，第一项的value为0</param>
        /// <param name="titleOfAll">第一项所对应的名称</param>
        public static void BindDropDownList(this ListControl ddl, List<ProcessModel> modelList, bool hasAll, string titleOfAll = "所有")
        {
            ExceptionHelper.TrueThrow(modelList == null, "流程为空");
            if (hasAll)
                ddl.Items.Add(new ListItem(titleOfAll, "0"));
            modelList.ForEach(p =>
            {
                ddl.Items.Add(new ListItem(p.ProcessName, p.ProcessCode));
            });
        }

        public static void BindDropDownList(this ListControl ddl, Type enumType, bool hasAll, string titleOfAll = "所有")
        {
            ExceptionHelper.FalseThrow(enumType.IsEnum, "传入的类型是枚举！");
            if (hasAll)
                ddl.Items.Add(new ListItem(titleOfAll, "0"));
            ListItem[] array = EnumItemDescriptionAttribute.GetDescriptionList(enumType)
                .Select(q => new ListItem(q.Description, q.EnumValue.ToString()))
                .ToArray();
            ddl.Items.AddRange(array);
        }

        public static void BindDropDownList(this ListControl ddl, Type enumType, bool sortAsc)
        {
            ExceptionHelper.FalseThrow(enumType.IsEnum, "传入的类型是枚举！");
            if (sortAsc)
            {
                var array = EnumItemDescriptionAttribute.GetDescriptionList(enumType)
              .Select(q => new ListItem(q.Description, q.EnumValue.ToString()))
               .OrderBy<ListItem, string>(s => s.Value).ToArray();

                ddl.Items.AddRange(array);
            }
            else
            {
                var array = EnumItemDescriptionAttribute.GetDescriptionList(enumType)
                     .Select(q => new ListItem(q.Description, q.EnumValue.ToString()))
                     .OrderByDescending<ListItem, string>(s => s.Value).ToArray();

                ddl.Items.AddRange(array);
            }
        }

        /// <summary>
        /// 根据描述找到枚举对象的值
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static int GetEnumValue(Type enumType, string description)
        {
            EnumItemDescriptionList enumList = EnumItemDescriptionAttribute.GetDescriptionList(enumType);

            EnumItemDescription found = null;

            foreach (EnumItemDescription item in enumList)
            {
                if (item.Description == description)
                {
                    found = item;
                    break;
                }
            }

            if (found == null)
            {
                throw new ArgumentException(string.Format("无法在类型为{0}的枚举中找到描述为{1}的枚举对象", enumType.Name, description));
            }

            return found.EnumValue;
        }


        /// <summary>
        /// 根据值找到枚举对象的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, int value)
        {
            Enum item = (Enum)Enum.ToObject(enumType, value);
            return EnumItemDescriptionAttribute.GetDescription(item);
        }

    }

    public sealed class ProcessModel
    {
        public ProcessModel()
        {
            NodeList = new List<ProcessNodeModel>();
        }
        public string ID { get; set; }

        public string ProcessCode { get; set; }

        public string ProcessType { get; set; }

        public string ProcessName { get; set; }

        public List<ProcessNodeModel> NodeList { get; set; }
    }

    public sealed class ProcessNodeModel
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string NodeName { get; set; }

        public string ID { get; set; }
        /// <summary>
        /// 节点号
        /// </summary>
        public string NodeSeq { get; set; }

        /// <summary>
        /// 节点类型 1、上报 2、审批
        /// </summary>
        public string NodeType { get; set; }
    }

    public sealed class WorkFlowCache : CacheQueue<string, List<ProcessModel>>
    {
        private WorkFlowCache()
        {

        }
        public static WorkFlowCache Instance = new WorkFlowCache();
    }

    /// <summary>
    /// 决定流程的上报、审批处理表单
    /// </summary>
    public enum WorkFlowOperatorType
    {
        [EnumItemDescription(Description = "项目净往来专用")]
        Project = 1,

        [EnumItemDescription(Description = "二级指标")]
        HasSub = 2,

        [EnumItemDescription(Description = "一级指标")]
        Common = 3,

        [EnumItemDescription(Description = "各系统专用")]
        EachSys = 4,

        [EnumItemDescription(Description = "计划外填报")]
        OutPlan = 5,

        [EnumItemDescription(Description = "用户变更")]
        UserReplace = 60,

        [EnumItemDescription(Description = "融资收入")]
        Rongzishouru = 7,

        [EnumItemDescription(Description = "融资支出")]
        Rongzizhichu= 8,

        [EnumItemDescription(Description = "财务费用")]
        Caiwufeiyong = 9,

        [EnumItemDescription(Description = "外币并购贷款")]
        Binggoudaikuan = 10,

        [EnumItemDescription(Description = "委托贷款")]
        Weituodaikuan = 11,

        [EnumItemDescription(Description = "优惠政策上报")]
        PreferentialPolicyApply = 20,

        [EnumItemDescription(Description = "初设设置")]
        InitialSettingApply = 21,

        [EnumItemDescription(Description = "历史已返数据上报")]
        HistoryTransferedDataReportApply = 22,

        [EnumItemDescription(Description = "年度计划上报")]
        AnnualPlanApply = 24,

        [EnumItemDescription(Description = "月度计划上报")]
        MonthPlanApply = 25,

        [EnumItemDescription(Description = "财政返还明细上报")]
        FiscalReturnExecuteApply = 26,

        [EnumItemDescription(Description = "参数调整上报")]
        InitialSettingModifyApply = 27,

        [EnumItemDescription(Description = "年度计划调整上报")]
        AnnualPlanModifyApply = 28,

        [EnumItemDescription(Description = "资金调拨申请")]
        FundsAllocationApply = 30,

        [EnumItemDescription(Description = "资金调拨执行")]
        FundsAllocationExecute = 31,

        [EnumItemDescription(Description = "资金上缴申请")]
        FundsDeliveringApply = 32,

        [EnumItemDescription(Description = "资金上缴执行")]
        FundsDeliveringExecute = 33,

        [EnumItemDescription(Description = "计划外临时调款申请")]
        ExceptionTempApply = 34,
        [EnumItemDescription(Description = "计划外临时调款收款")]
        ExceptionTempReceive = 35,
        [EnumItemDescription(Description = "计划外临时调款还款")]
        ExceptionTempRepay = 36,

        [EnumItemDescription(Description = "其他资金调拨申请")]
        OtherFundsAllocationApply = 37,
        [EnumItemDescription(Description = "其他资金调拨执行")]
        OtherFundsAllocationExecute = 38,

        [EnumItemDescription(Description = "项目公司考核")]
        ProjectAssessmentApply = 40,
        [EnumItemDescription(Description = "集团总部考核")]
        GroupAssessmentApply = 41,

        [EnumItemDescription(Description = "项目公司融资收入")]
        ProjectRongZiApply = 42,

        [EnumItemDescription(Description = "集团资金调拨申请")]
        HQFundsAllocationApply = 43,

        [EnumItemDescription(Description = "集团资金调拨执行")]
        HQFundsAllocationExecute = 44,

        [EnumItemDescription(Description = "集团资金上缴申请")]
        HQFundsDeliveringApply = 45,

        [EnumItemDescription(Description = "集团资金上缴执行")]
        HQFundsDeliveringExecute = 46,


        [EnumItemDescription(Description = "其他资金调拨申请")]
        BizOtherFundsAllocationApply = 50,
        [EnumItemDescription(Description = "其他资金调拨执行")]
        BizOtherFundsAllocationExecute = 51,
        [EnumItemDescription(Description = "临时存款申请")]
        TemporaryDeposit = 52
    }

    public class ProcessConst
    {
        public const string Year = "BP01";
        public const string Project = "BP02";
        public const string Rent = "BP03";
        public const string Finance = "BP04";
        public const string Stock = "BP05";
        public const string EachSys = "BP06";
        public const string NewProject = "BP07";
        public const string OutPlan = "BP08";
        public const string DailyOut = "BP09";
        public const string OtherIn = "BP10";
        public const string OtherOut = "BP11";

        public const string PreferentialPolicyApply = "BP20";
        public const string InitialSettingApply = "BP21";
        public const string HistoryTransferedDataReportApply = "BP22";
        public const string AnnualPlanApply = "BP24";
        public const string MonthPlanApply = "BP25";
        public const string FiscalReturnExecuteApply = "BP26";
        public const string InitialSettingModifyApply = "BP27";
        public const string AnnualPlanModifyApply = "BP28";
        public const string FundsTransferApply = "BP30";
        public const string FundsTransferExecute = "BP31";

    }

}

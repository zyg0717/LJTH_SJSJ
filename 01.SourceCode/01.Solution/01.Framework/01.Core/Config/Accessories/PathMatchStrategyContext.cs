
using System.Collections.Generic;
using Framework.Core;

namespace Framework.Core.Config
{
    /// <summary>
    /// ·��ƥ���㷨������̶��� Context
    /// </summary>
    internal class PathMatchStrategyContext : StrategyContextBase<BestPathMatchStrategyBase, string>
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public PathMatchStrategyContext() { }


        /// <summary>
        /// ���ݳ����㷨������֯�������
        /// </summary>
        /// <returns></returns>
        public override string DoAction()
        {
			IList<KeyValuePair<string, string>> data = innerStrategy.Candidates;
			return innerStrategy.Calculate(data);
        }
    }
}

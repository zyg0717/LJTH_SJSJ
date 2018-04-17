using System.Collections.Generic;

namespace Framework.Core
{
    /// <summary>
    /// ���������࣬�û���ʵ����IFilter�ӿڡ�
    /// </summary>
    /// <typeparam name="TFilter">������������</typeparam>
    /// <typeparam name="TTarget">��Ҫ���˵�ʵ��������</typeparam>
    /// <remarks>���磺�ڹ������д���˺ܶ��������������ʵ������������е������������������ʵ����Ҫ����ĳ�ִ������������һ�ִ���
    /// </remarks>
    public abstract class FilterPipelineBase<TFilter, TTarget> 
        where TFilter : IFilter<TTarget>
    {
        /// <summary>
        /// ���������ж���
        /// </summary>
		/// <remarks>
		/// ���������ж������ڴ����ڲ�����������
		/// </remarks>
        protected List<TFilter> pipeline;

        /// <summary>
        /// �������������µ�һ��������
        /// </summary>
        /// <param name="filter">������</param>
        /// <remarks>
        /// �������������µ�һ�������
        /// </remarks>
        public abstract void Add(TFilter filter);

        /// <summary>
        /// �ӹ��������Ƴ�һ��������
        /// </summary>
        /// <param name="filter">������</param>
        /// <remarks>�ӹ��������Ƴ�һ��������
        /// </remarks>
		public abstract void Remove(TFilter filter);

        /// <summary>
        /// �ж�ʵ���Ƿ������������ÿһ�������������
        /// </summary>
        /// <param name="target">��Ҫ���й��˵�ʵ��</param>
        /// <returns>��ʵ�������������ÿһ����������������򷵻��棬���򷵻ؼ١�</returns>
        /// <remarks>�ж�ʵ���Ƿ������������ÿһ��������������������򷵻��棬���򷵻ؼ١�
        /// </remarks>
        public virtual bool IsMatch(TTarget target)
        {
			if (this.pipeline != null)
			{
				foreach (TFilter item in this.pipeline)
				{
					if (item.IsMatch(target))
						return true;
				}
			}
            return false;
        }
    }
}

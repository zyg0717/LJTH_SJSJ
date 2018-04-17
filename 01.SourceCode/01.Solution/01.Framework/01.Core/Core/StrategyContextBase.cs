

namespace Framework.Core
{
    /// <summary>
    /// ����ģʽ��ʵ����Strategy Context ��
    /// </summary>
    /// <remarks>
    /// ��� IStragegy ���������̡�
    /// </remarks>
	/// <typeparam name="TStrategy">�����������</typeparam>
	/// <typeparam name="TResult">����������</typeparam>
    public abstract class StrategyContextBase<TStrategy, TResult>
    {
        #region Private field
        /// <summary>
        /// Context ��Ҫ�������㷨����
        /// </summary>
        protected TStrategy innerStrategy;
        #endregion

        /// <summary>
        /// ���Ը��ǵĳ��� IStragegy ���͸�����������̣���Ϊ���� Context �����������ɻ����� Calculate �������á�
        /// </summary>
        /// <returns>�ڲ�����Ľ��ֵ</returns>
		/// <remarks>
		/// ���Ը��ǵĳ��� IStragegy ���͸�����������̣���Ϊ���� Context �����������ɻ����� Calculate �������á�
		/// </remarks>
        public abstract TResult DoAction();

        /// <summary>
        /// ���������㷨����
        /// </summary>
		/// <remarks>
		/// ���������㷨����
		/// </remarks>
        public TStrategy Strategy
        {
			get 
			{
				return this.innerStrategy;
			}
            set 
			{
				this.innerStrategy = value; 
			}
        }
    }
}

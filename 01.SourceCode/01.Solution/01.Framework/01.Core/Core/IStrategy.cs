
namespace Framework.Core
{
    /// <summary>
    /// ����ģʽ��ʵ����IStrategy�ӿڡ�
    /// </summary>
    /// <remarks>
    /// ��������Stragegy���������̣�ʵ�ʿ����п��Ը��ݼ����Ҫ����� StrategyContextBase ��������֯��صļ������ݡ�
    /// </remarks>
	/// <typeparam name="TData">�����������</typeparam>
	/// <typeparam name="TResult">����������</typeparam>
    public interface IStrategy<TData, TResult>
    {
        /// <summary>
        /// ����������Ϣ���������㷨����
        /// </summary>
        /// <param name="data">��������</param>
        /// <returns>�㷨������</returns>
		/// <remarks>����������Ϣ���������㷨����</remarks>
        TResult Calculate(TData data);
    }
}

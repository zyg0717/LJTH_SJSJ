
namespace Framework.Core
{
    /// <summary>
    /// ���ڷ���Ĵ�������ӿڣ��ýӿ�ʵ���˴�����ģʽ��
    /// </summary>
    /// <typeparam name="T">ʵ��������</typeparam>
    /// <remarks>���ڷ���Ĵ�������ӿڣ��ýӿ�ʵ���˴�����ģʽ��
    /// </remarks>
    internal interface IBuilder<T>
    {
        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <param name="target">��Ҫ������ʵ��</param>
        /// <returns>�Ѵ�����ʵ����</returns>
        /// <remarks>����ָ�����͵�ʵ����</remarks>
        T BuildUp(T target);

        /// <summary>
        /// ��ָ�����͵�ʵ����
        /// </summary>
        /// <param name="target">��Ҫ�𿪵�ʵ��</param>
        /// <returns>�Ѳ𿪵�ʵ��</returns>
        /// <remarks>��ָ�����͵�ʵ����</remarks>
        T TearDown(T target);
    }
}

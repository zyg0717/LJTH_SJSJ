using System.Collections;

namespace Framework.Core
{
    /// <summary>
    /// ְ����ģʽ�ӿڣ��ýӿ�ʵ����ְ����ģʽ��
    /// </summary>
    /// <typeparam name="T">ְ������ÿһ���ڵ������</typeparam>
    /// <remarks>
    /// �ýӿ��ṩ��ְ�����Ļ����������������һ���ڵ㣬���һ��ڵ�ȵȡ�   
    /// </remarks>
    internal interface IChain<T>
        where T : class
    {
        /// <summary>
        /// ְ�����ĵ�һ���ڵ�
        /// </summary>
        /// <remarks>
        /// ��������ֻ����
        /// </remarks>
        T Head { get;}

        /// <summary>
        /// ��ְ���������һ���ڵ�
        /// </summary>
        /// <param name="item">��Ҫ��ְ��������ӵĽڵ�</param>
        /// <remarks>
        /// ��ְ���������һ���ڵ�
        /// </remarks>
        void Add(T item);

        /// <summary>
        /// ��ְ���������һ��ڵ㡣
        /// </summary>
        /// <param name="items">�ڵ���</param>
        /// <remarks>
        /// ��ְ���������һϵ�нڵ㡣
        /// </remarks>
        void AddRange(IEnumerable items);

        /// <summary>
        /// ��ְ�����л�õ�ǰ�ڵ����һ���ڵ㡣
        /// </summary>
        /// <param name="current">��ǰ�Ľڵ�</param>
        /// <returns>��ȡְ������ָ���ڵ����һ���ڵ㣬���ָ���Ľڵ�Ϊ�ջ�ǰ�ڵ����һ���ڵ�Ϊ�գ��򷵻ؿա�</returns>
        /// <remarks>
        /// ���ְ������ָ���ڵ����һ���ڵ㡣
        /// </remarks>
        T GetNext(T current);
    }
}

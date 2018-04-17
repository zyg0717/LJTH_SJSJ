using System;

namespace Framework.Data
{
    /// <summary>
    /// 进行Mapping时忽略的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NoMappingAttribute : System.Attribute
    {
    }
}

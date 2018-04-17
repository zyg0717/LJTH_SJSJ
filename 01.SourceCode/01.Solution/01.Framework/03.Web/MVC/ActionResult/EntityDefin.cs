using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Web.MVC
{
    /// <summary>
    /// 返回一个集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityList<T>
    {
        public EntityList()
        {
            Status = StatusConst.SUCCESS;
            Message = MessagesConst.SUCCESS;
        }
        /// <summary>数据部分</summary>
        public IList<T> Data { get; set; }

        /// <summary>总记录数</summary>
        public int TotalCount { get; set; }

        /// <summary>状态编号</summary>
        public int Status { get; set; }
        /// <summary>消息</summary>
        public string Message { get; set; }

    }
    /// <summary>
    /// 返回一个简单实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Entity<T>
    {
        public Entity()
        {
            Status = StatusConst.SUCCESS;
            Message = MessagesConst.SUCCESS;
        }
        /// <summary>数据部分</summary>
        public T Data { get; set; }

        /// <summary>状态编号</summary>
        public int Status { get; set; }
        /// <summary>消息</summary>
        public string Message { get; set; }
    }


    public class StatusConst
    {
        /// <summary>成功</summary>
        public const int SUCCESS = 200;
        /// <summary>未找到</summary>
        public const int NOTFIND = 400;
        /// <summary>参数异常</summary>
        public const int PARAMETERERROR = 401;
        /// <summary>未知</summary>
        public const int UNKNOWN = 402;
    }
    public class MessagesConst
    {
        /// <summary>成功</summary>
        public const string SUCCESS = "操作成功";
        /// <summary>未找到</summary>
        public const string NOTFIND = "未找到对象";
        /// <summary>参数异常</summary>
        public const string PARAMETERERROR = "参数异常";
        /// <summary>未知</summary>
        public const string UNKNOWN = "位置异常";
    }

}

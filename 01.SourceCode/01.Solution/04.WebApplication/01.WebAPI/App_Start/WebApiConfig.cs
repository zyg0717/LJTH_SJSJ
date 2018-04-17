using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;
using WebApplication.WebAPI.Models.Helper;
using System.Web.Http.Tracing;
using Framework.Web.Utility;
using Framework.Web.Security.Authentication;
using Lib.BLL;

namespace WebApplication.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.Filters.Add(new ExceptionAttribute());
            config.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );

            WebHelper.Instance.GetUser += UserInfoOperator.Instance.GetWDUserInfoByLoginName;
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //formatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            formatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            //formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //formatter.SerializerSettings.Converters.Add(new CustomerDateTimeConverter(typeof(DateTime)));
            //formatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());

        }
    }
    //public class CustomerDateTimeConverter : IsoDateTimeConverter
    //{
    //    private readonly Type[] _types;

    //    public CustomerDateTimeConverter(params Type[] types)
    //    {
    //        this._types = types;
    //    }
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Date)
    //        {

    //        }
    //        return base.ReadJson(reader, objectType, existingValue, serializer);
    //    }
    //    //private static string ConvertJsonDateToDateString(Match m)
    //    //{
    //    //    string result = string.Empty;
    //    //    DateTime dt = new DateTime(1970, 1, 1);
    //    //    dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
    //    //    dt = dt.ToLocalTime();
    //    //    result = dt.ToString("yyyy-MM-dd HH:mm:ss");
    //    //    return result;
    //    //}
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        base.WriteJson(writer, value, serializer);
    //    }
    //    public override bool CanRead
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return _types.Any(t => t == objectType);
    //    }
    //}
}

using Framework.Core.Log;
using Framework.Web.Json;
using Framework.Web.MVC;
using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Framework.Web.Download
{
    public class ExcelHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public virtual bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {

             ExcelGenerator.GenerateExcel(context);



        }






    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AEPApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            string sLogPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            string sPath = HttpContext.Current.Server.MapPath(sLogPath);
            DirectoryInfo _dir = null;
            _dir = new DirectoryInfo(sPath);
            if (_dir.Exists == false)
            {
                _dir.Create();
                Directory.CreateDirectory(_dir.ToString() + "log\\");
            }
            SatoLib.SatoLogger _obj = new SatoLib.SatoLogger();
            _obj.ChangeInterval = SatoLib.SatoLogger.ChangeIntervals.ciHourly;
            _obj.EnableLogFiles = true;
            _obj.LogDays = 10;
            _obj.LogFilesExt = "Log";
            _obj.LogFilesPath = sPath;
            _obj.LogFilesPrefix = "AISIN";
            _obj.StartLogging();
            _obj.LogMessage(SatoLib.EventNotice.EventTypes.evtInfo, "SatoAppsInitialize" + "  ::  Main", "Initializing Application.......on " + "");
            GlobalVar.Logger = _obj;
            _obj.StopLogging();
            _obj = null;
            GlobalVar.Logger.LogMessage(SatoLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name + "  " + "", "AppInitialize");
            GlobalVar.Logger.StartLogging();
        }
    }
}

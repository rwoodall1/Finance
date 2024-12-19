using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BindingModels;
namespace Utilities
{
    public class ApplicationConfig
    {
        //********************* DO NOT FORGET TO SET THE VALUE IN THE STARTUP FILE *******************************
        //********************************************************************************************************
        //********************************************************************************************************
        //********************************************************************************************************
        //********************************************************************************************************
        //********************************************************************************************************
        //********************************************************************************************************
        public static string DefaultConnectionString { get; set; }
        public static string BaseUrl { get; set; }
        public static string AppVersion { get; set; }
    
        public static string Environment { get; set; }
        public static string ErrorEmailAddress { get; set; }
        public static bool IsDeveloperMachine { get; set; }
        public static SMTPConfig SMTPConfig { get; set; }
        public static string SqlPassPhrase { get; set; }
        public static string APISecretKey { get; set; }
        public static string TokenDomain { get; set; }

    }
    public class SMTPConfig
    {
        public string From { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        public string User { get; set; }

    }
}

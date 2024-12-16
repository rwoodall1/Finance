using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Core;


using Utilities;
using ApiBindingModels;
//using Microsoft.AspNetCore.Http;

namespace Services {
    public class BaseDataService {
        protected Logger log { get; set; }
        public LoggedInUser LoggedInUser { get; set; }
        public BaseDataService(LoggedInUser _LoggedInUser)
        {
            log = LogManager.GetCurrentClassLogger();
            LoggedInUser = _LoggedInUser;
        }
        public BaseDataService()
        {
            log = LogManager.GetCurrentClassLogger();

        }

    }
}



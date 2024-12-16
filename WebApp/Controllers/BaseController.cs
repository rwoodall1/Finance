using Microsoft.IdentityModel.Tokens;
using WebApp.Models;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Microsoft.AspNetCore.Mvc;
using ApiBindingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Principal;
using NLog;
namespace WebApp.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        
        public LoggedInUser LoggedInUser { get; set; }
        public BaseController()
        {
         var httpContext = new HttpContextAccessor().HttpContext;
            LoggedInUser = (LoggedInUser)httpContext.Items["User"];
            
        }
       
    }

    }
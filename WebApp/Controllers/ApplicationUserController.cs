using Microsoft.IdentityModel.Tokens;
using WebApp.Models;
using Core;

using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Microsoft.AspNetCore.Mvc;
using ApiBindingModels;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class ApplicationUserController : BaseController
    {


        [AuthorizeAdminAttribute]
        [HttpGet, Route("deleteUser")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var processingResult = new ApiProcessingResult();
            var result = await new ApplicationUserDataService(LoggedInUser).DeleteUser(userId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }


            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAdminAttribute]
        [HttpPost, Route("addAdminUser")]
        public async Task<ActionResult> AddAdminUser(AdminUser user)
        {
            var processingResult = new ApiProcessingResult();
            //var result = await new ApplicationUserDataService(LoggedInUser).AddAdminUser(user);
            //if (result.IsError)
            //{
            //    processingResult.IsError = true;
            //    processingResult.Errors = result.Errors;
            //    return Ok(new { ApiProcessingResult = processingResult });
            //}
        
            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAdminAttribute]
        [HttpPost, Route("updateAdminUser")]
        public async Task<ActionResult> UpdateAdminUser(AdminUser user)
        {
            var processingResult = new ApiProcessingResult();
            //var result = await new ApplicationUserDataService(LoggedInUser).UpdateAdminUser(user);
            //if (result.IsError)
            //{
            //    processingResult.IsError = true;
            //    processingResult.Errors = result.Errors;
            //    return Ok(new { ApiProcessingResult = processingResult });
            //}
           

            return Ok(new { ApiProcessingResult = processingResult });
        }


        [AuthorizeAdminAttribute]
        [HttpGet, Route("getAdminUsers")]
        public async Task<ActionResult> GetAdminUsers()
        {
            var processingResult = new ApiProcessingResult<List<AdminUser>>();
            

            //var result = await new ApplicationUserDataService(LoggedInUser).GetAdminUsers();

           
            //if (result.IsError)
            //{
            //    processingResult.IsError = true;
            //    processingResult.Errors = result.Errors;
            //    return Ok(new { ApiProcessingResult = processingResult });
            //}
            //processingResult.Data = result.Data;

            return Ok(new { ApiProcessingResult = processingResult });
        }



        

        [AuthorizeAttribute]
        [HttpGet, Route("getUserProfile")]
        public async Task<ActionResult> GetUserProfile(string userId)
        {

            var processingResult = new ApiProcessingResult<UserProfile>();

            var result = await new ApplicationUserDataService(LoggedInUser).GetUserProfile(userId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            processingResult.Data = result.Data;
            return Ok(new { ApiProcessingResult = processingResult });
        }
        [AuthorizeAttribute]
        [HttpPost, Route("updateUserProfile")]
        public async Task<ActionResult> UpdateUserProfile(UserProfile model)
        {

            var processingResult = new ApiProcessingResult();

            var result = await new ApplicationUserDataService(LoggedInUser).UpdateUserProfile( model);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
          

            return Ok(new { ApiProcessingResult = processingResult });
        }

   
    }
}
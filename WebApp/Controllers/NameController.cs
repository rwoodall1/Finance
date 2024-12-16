using BindingModels;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataService;
using System.Security.Principal;
using System.Data;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/names")]
    public class NameController : BaseController
    {
        
        [AuthorizeAttribute]
        [HttpGet, Route("getNames")]
        public async Task<ActionResult> GetNames()
        {
            var processingResult = new ApiProcessingResult<List<DropDownNames>>();
            var result = await new SysDataService().GetNames();
            if (result.IsError)
            {
                
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var rawNames = result.Data;
            var finalList=CreatDropNames(rawNames);

            processingResult.Data=finalList;

            return Ok(new { ApiProcessingResult = processingResult });
        }


       private List<DropDownNames> CreatDropNames(List<NameLkpModel> rawNames)
        {
            var nameDropDownList = new List<DropDownNames>();
            var vendor = new List<NameList>();
            var other = new List<NameList>();
            foreach (var row in rawNames)
            {
                switch (row.NameType.ToUpper())
                {
                    case "OTHER NAME":
                        var rec = new NameList()
                        {
                            FullName = row.FullName,
                            Id = row.Id,

                        };
                        other.Add(rec);

                        break;

                    case "VENDOR":
                        var rec1 = new NameList()
                        {
                            FullName = row.FullName,
                            Id = row.Id,

                        };
                       vendor.Add(rec1);

                        break;

                }
            }
            var otherGroup = new DropDownNames() { NameType = "OTHER NAME", Names=other };
            var vendorGroup = new DropDownNames() { NameType = "VENDOR", Names = vendor };
            nameDropDownList.Add(otherGroup);
            nameDropDownList.Add(vendorGroup);


            return nameDropDownList;
        }
    }
}

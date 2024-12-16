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
using ApiBindingModels;
using System.Reflection;
using System.Xml.Linq;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/feed")]
    public class BankFeedController : BaseController
    {
        
        [AuthorizeAttribute]
        [HttpPost, Route("readBankFeed")]
        public async Task<ActionResult> ReadBankFeed([FromForm] BankUploadFeedModel model)
        {
            var processingResult = new ApiProcessingResult<List<STMTTRN>>();
            var result = await new BankFeedDataService().ReadBankFeed(model);
            if (result.IsError)
            {

                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var data=result.Data;
            var insertResult = await new TransActionDataService().InsertImportedParentData(data);
            if (insertResult.IsError)
            {

                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var importDataResult = await new TransActionDataService().GetImportedData("458783");
            if (importDataResult.IsError)
            {

                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            processingResult.Data = importDataResult.Data;


            return Ok(new { ApiProcessingResult = processingResult });
        }
      
          [AuthorizeAttribute]
        [HttpGet, Route("getImportedData")]
        public async Task<ActionResult> GetImportedData(string bankId)
        {
            var processingResult = new ApiProcessingResult<List<TransactionBankFeedModel>>();
            var result = await new TransActionDataService().GetImportedData(bankId);
            if (result.IsError)
            {

                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var rawData=result.Data;
            var bankModelResult = await new BankFeedDataService().StmttrnToTransActionCrud(rawData);
            if (bankModelResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = bankModelResult.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var a = 1;
            processingResult.Data = bankModelResult.Data;


            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpPost, Route("importData")]
        public async Task<ActionResult> ImportData(List<TransactionBankFeedModel> model)
        {
            var processingResult = new ApiProcessingResult();
            foreach (TransactionBankFeedModel row in model)
            {
                if (row.ChildAccountId == 0)
                {
                    row.Import = false;
                    continue;
                }
                if (row.Status == "REV" && row.PayeeId > 0)
                {
                    //no rule if no payee Id
                     var ruleResult=new TransActionDataService().AddNewRule(row);
                    if (ruleResult.Result.IsError)
                    {
                        //log and go on
                        
                    }

                  
                }
                if (row.Import == true)
                {
                    var parentRow = new TransActionCrudModel()
                    {
                        PayeeId = row.PayeeId,
                        AccountId = row.AccountId,
                        Debit = row.Debit,
                        Credit = row.Credit,
                        ChildAccountId = row.ChildAccountId,
                        TransType = row.Debit > 0 ? "CHK" : "DEP",
                        RefNumber = row.RefNumber,
                        TransDate = row.TransDate,



                    };
                    var insertList = new List<TransActionCrudModel>();
                    insertList.Add(new TransActionCrudModel()
                    {
                        PayeeId = row.PayeeId,
                        AccountId = row.ChildAccountId,
                        Debit = row.Credit,
                        Credit = row.Debit,
                        ChildAccountId = row.AccountId,
                        TransType = row.Debit > 0 ? "CHK" : "DEP",
                        RefNumber = row.RefNumber,
                        TransDate = row.TransDate,


                    });
                    
                    var data = BankFeedDataService.MatchData(parentRow, insertList);


                    var retval = await new TransActionDataService().Insert(data);
                    if (retval.IsError)
                    {
                        processingResult.IsError = true;
                        processingResult.Errors = retval.Errors;
                        return Ok(new { ApiProcessingResult = processingResult });

                    }
                    var updateResult = await new BankFeedDataService().SetImported(row.FITID);
                                      
                }

            }


            return Ok(new { ApiProcessingResult = processingResult });
        }

    }
  
}

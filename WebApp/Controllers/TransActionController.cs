using BindingModels;
using Core;
using DataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services;
using System.ComponentModel;
using System.Linq;
using System.Transactions;

namespace WebApp.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransActionController : ControllerBase
    {
        
        [AuthorizeAttribute]
        [HttpGet, Route("getLastStatementBalance")]
        public async Task<ActionResult> getLastStatementBalance(int accountId)
        {
            var processingResult = new ApiProcessingResult<Balances>();
            var result = await new TransActionDataService().GetLastStatementBalance(accountId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            var vData = (Balances)result.Data;
            processingResult.Data = vData;

            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpGet, Route("getUnReconciledTransActions")]
        public async Task<ActionResult> GetUnReconciledTransActions(int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            var result = await new TransActionDataService().GetTransactions(accountId,UnReconciled.Yes);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            decimal? vbalance = 0;
            var vData=result.Data.OrderByDescending(x => x.TransDate).ToList<TransActionCrudModel>();
            processingResult.Data = vData;

            return Ok(new { ApiProcessingResult = processingResult });
        }
        [AuthorizeAttribute]
        [HttpGet, Route("getTransActions")]
        public async Task<ActionResult> GetTransActions(int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            var result = await new TransActionDataService().GetTransactions(accountId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            decimal? vbalance = 0;
            var vData = result.Data.OrderByDescending(x => x.TransDate).ToList<TransActionCrudModel>();
            for (int i = vData.Count - 1; i >= 0; i--)
            {
                decimal? _credit = 0;
                decimal? _debit = 0;
                if (vData[i].Credit != null)
                {
                    _credit = vData[i].Credit;
                }
                if (vData[i].Debit != null)
                {
                    _debit = vData[i].Debit;
                }
                vbalance += (_credit - _debit);
                vData[i].Balance = vbalance;

            }


            processingResult.Data = vData;

            return Ok(new { ApiProcessingResult = processingResult });
        }
        [AuthorizeAttribute]
        [HttpGet, Route("getChildTransActions")]
        public async Task<ActionResult> GetChildTransActions(int transactionId,int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            var result = await new TransActionDataService().GetChildTransactions(transactionId, accountId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
           

            processingResult.Data =result.Data;

            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpGet, Route("deleteTransAction")]
        public async Task<ActionResult> DeleteTransAction(int transId)
        {
            var processingResult = new ApiProcessingResult();


            var result = await new TransActionDataService().DeleteTransAction(transId);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpPost, Route("update")]
        public async Task<ActionResult> Update(List<TransActionCrudModel> model)
        {
            var processingResult = new ApiProcessingResult();


            var result = await new TransActionDataService().Update(model);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpPost, Route("newTransaction")]
        public async Task<ActionResult> NewTransaction(List<TransActionCrudModel> model)
        {
            var processingResult = new ApiProcessingResult();


            var result = await new TransActionDataService().Insert(model);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            return Ok(new { ApiProcessingResult = processingResult });
        }
        [AuthorizeAttribute]
        [HttpPost, Route("setCleared")]
        public async Task<ActionResult> SetCleared(List<TransActionCrudModel> model)
        {
            var processingResult = new ApiProcessingResult();


            var result = await new TransActionDataService().SetCleared(model);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }

            return Ok(new { ApiProcessingResult = processingResult });
        }
        [AuthorizeAttribute]
        [HttpPost, Route("insertUpdateReconciliation")]
        public async Task<ActionResult> InsertUpdateReconciliation(ReconciledData model)
        {
            var processingResult = new ApiProcessingResult<int>();
           var result=await new TransActionDataService().InsertUpdateReconciliation(model);
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
        [HttpPost, Route("setReconciled")]
        public async Task<ActionResult> SetReconciled(List<TransActionCrudModel> model)
        {
            var processingResult = new ApiProcessingResult();
            var result = await new TransActionDataService().SetReconciled(model);
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

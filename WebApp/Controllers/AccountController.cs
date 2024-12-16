using BindingModels;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataService;
using System.Security.Principal;
using System.Data;
using System.Security.Cryptography;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : BaseController
    {
        
        [AuthorizeAttribute]
        [HttpGet, Route("getSysAccounts")]
        public async Task<ActionResult> GetSysAccounts()
        {
            var processingResult = new ApiProcessingResult<List<AccountDropDown>>();
            var result = await new SysDataService().GetSysAccounts();
            if (result.IsError)
            {
                
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var rawAccounts = result.Data;
            var finalList=CreatDropDownAccounts(rawAccounts);

            processingResult.Data= finalList;

            return Ok(new { ApiProcessingResult = processingResult });
        }



        [AuthorizeAttribute]
        [HttpPost, Route("saveAccount")]
        public async Task<ActionResult> SaveAccounts(AccountModel model)
        {
            var processingResult = new ApiProcessingResult();
            var result = await new AccountDataService().SaveAccount(model);
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
          

            return Ok(new { ApiProcessingResult = processingResult });
        }

        [AuthorizeAttribute]
        [HttpGet, Route("getSubAccounts")]
        public async Task<ActionResult> GetSubAccounts(string type)
        {
            var processingResult = new ApiProcessingResult<List<SubAccount>>();
            var result = await new AccountDataService().GetSubAccounts(type);
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
        [HttpGet, Route("getChartOfAccounts")]
        public async Task<ActionResult> GetWebChartOfAccounts()
        {
        var processingResult = new ApiProcessingResult<List<AccountNode>>();
            var result = await new SysDataService().GetWebChartOfAccounts();
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
        [HttpGet, Route("getAccount")]
        public async Task<ActionResult> GetAccount(string id)
        {
            var processingResult = new ApiProcessingResult<AccountModel>();
            var result = await new AccountDataService().GetAccount(id);
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
        [HttpGet, Route("getAccountTypes")]
        public async Task<ActionResult> GetAccountTypes()
        {
            var processingResult = new ApiProcessingResult<List<AccountTypeModel>>();
            var result = await new AccountDataService().GetAccountTypes();
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
        [HttpGet, Route("getBankAccounts")]
        public async Task<ActionResult> GetBankAccounts()
        {
            var processingResult = new ApiProcessingResult<List<AccountModel>>();
            var result = await new AccountDataService().GetBankAccounts();
            if (result.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return Ok(new { ApiProcessingResult = processingResult });
            }
            processingResult.Data = result.Data;


            return Ok(new { ApiProcessingResult = processingResult });

        }
        private List<AccountDropDown> CreatDropDownAccounts(List<AccountModel>data)
        {

            var accountDropDownList = new List<AccountDropDown>();
            var bankType = new List<AccountNameList>();
            var creditCardType = new List<AccountNameList>();
            var accountReceivableType = new List<AccountNameList>();
            var otherCurrentAssetType = new List<AccountNameList>();
            var fixedAssestType = new List<AccountNameList>();
            var otherAssetType = new List<AccountNameList>();
            var otherCurrentLiabillityType = new List<AccountNameList>();
            var longTermLiablilityType = new List<AccountNameList>();
            var equityType = new List<AccountNameList>();
            var incomeType = new List<AccountNameList>();
            var otherIncomeType = new List<AccountNameList>();
            var expenseType = new List<AccountNameList>();
            var costOfGoodsSoldType = new List<AccountNameList>();
            var accountsPayableType = new List<AccountNameList>();
        
            foreach (var row in data)
            {
                switch (row.AccountType.ToUpper())
                {
                    case "BANK":
                        var rec = new AccountNameList()
                        {
                            Name = row.Name,
                            Id=row.Id,
                            
                        };
                        bankType.Add(rec);

                            break;
                    case "CREDIT CARD":
                        var rec1 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        creditCardType.Add(rec1);
                        break;
                    case "ACCOUNT RECEIVABLE":
                        var rec2 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        accountReceivableType.Add(rec2);
                        break;
                    case "OTHER CURRENT ASSET":
                        var rec3 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        otherCurrentAssetType.Add(rec3);
                        break;
                    case "FIXED ASSET":
                        var rec4 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        fixedAssestType.Add(rec4);
                        break;
                    case "OTHER ASSET":
                        var rec5 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        otherAssetType.Add(rec5);
                        break;
                    case "OTHER CURRENT LIABILLITY":
                        var rec6 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        otherCurrentLiabillityType.Add(rec6);
                        break;
                    case "LONG TERM LIABILLITY":
                        var rec7 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        longTermLiablilityType.Add(rec7);
                        break;
                    case "EQUITY":
                        var rec8 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        equityType.Add(rec8);
                        break;
                       
                    case "INCOME":
                        var rec9 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        incomeType.Add(rec9);
                        break;
                       
                    case "OTHER INCOME":
                        var rec10 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        otherIncomeType.Add(rec10);
                        break;
                    case "EXPENSE":
                        var rec11 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        expenseType.Add(rec11);
                        break;
                    case "COST OF GOODS SOLD":
                        var rec12 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        costOfGoodsSoldType.Add(rec12);
                        break;
                    case "ACCOUNTS PAYABLE":
                        var rec13 = new AccountNameList()
                        {
                            Name = row.Name,
                            Id = row.Id,

                        };
                        accountsPayableType.Add(rec13);
                        break;
                }
              
            }
            var bankGroup = new AccountDropDown() { AccountType = "Bank", Accounts = bankType };
            accountDropDownList.Add(bankGroup);

            var creditCardTypeGroup = new AccountDropDown() { AccountType = "Credit Card", Accounts = creditCardType };
            accountDropDownList.Add(creditCardTypeGroup);

            var accountReceivableGroup = new AccountDropDown() { AccountType = "Account Receivable", Accounts = accountReceivableType };
            accountDropDownList.Add(accountReceivableGroup);

            var otherCurrentAssetGroup = new AccountDropDown() { AccountType = "Other Current Asset", Accounts = otherCurrentAssetType };
            accountDropDownList.Add(otherCurrentAssetGroup);

            var fixedAssestGroup = new AccountDropDown() { AccountType = "Fixed Assest", Accounts = fixedAssestType };
            accountDropDownList.Add(fixedAssestGroup);

            var otherAssetGroup = new AccountDropDown() { AccountType = "Other Asset", Accounts = otherAssetType };
            accountDropDownList.Add(otherAssetGroup);

            var otherCurrentLiabillityGroup = new AccountDropDown() { AccountType = "Other Current Liabillity", Accounts = otherCurrentLiabillityType };
            accountDropDownList.Add(otherCurrentLiabillityGroup);

            var longTermLiablilityGroup = new AccountDropDown() { AccountType = "Long Term Liablility", Accounts = longTermLiablilityType };
            accountDropDownList.Add(longTermLiablilityGroup);

            var equityGroup = new AccountDropDown() { AccountType = "Equity", Accounts = equityType };
            accountDropDownList.Add(equityGroup);

            var incomeGroup = new AccountDropDown() { AccountType = "Income", Accounts = incomeType};
            accountDropDownList.Add(incomeGroup);

            var otherIncomeGroup = new AccountDropDown() { AccountType = "Other Income", Accounts = otherIncomeType };
            accountDropDownList.Add(otherIncomeGroup);

            var expenseGroup = new AccountDropDown() { AccountType = "Expense", Accounts = expenseType };
            accountDropDownList.Add(expenseGroup);

            var costOfGoodsSoldGroup = new AccountDropDown() { AccountType = "Cost Of Goods Sold", Accounts = costOfGoodsSoldType };
            accountDropDownList.Add(costOfGoodsSoldGroup);

            var accountsPayableGroup = new AccountDropDown() { AccountType = "Accounts Payable", Accounts = accountsPayableType };
            accountDropDownList.Add(accountsPayableGroup);

            return accountDropDownList;

        }
    }
}

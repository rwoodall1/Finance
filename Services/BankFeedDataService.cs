using ApiBindingModels;
using BindingModels;
using Core;
using Microsoft.AspNetCore.Http;
using SqlClient;
using System;
using DataService;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class BankFeedDataService : BaseDataService
    {
        public async Task<ApiProcessingResult<List<STMTTRN>>> ReadBankFeed(BankUploadFeedModel model)
        {
          
            var processingResult = new ApiProcessingResult<List<STMTTRN>>();

      

            ImportOfx _importer = new ImportOfx();
            var result =await  _importer.OfxToObject(model.File);
            if(result.IsError){

            }
            var vData=(List<STMTTRN>) result.Data;
         
            if (vData.Count == 0)
            {
                processingResult.IsError = true;
                return processingResult;
            }
          

            processingResult.Data = vData;
            return processingResult;
        }

        public async Task<ApiProcessingResult<List<TransactionBankFeedModel>>> StmttrnToTransActionCrud(List<STMTTRN> Model)
        {

            var processingResult = new ApiProcessingResult<List<TransactionBankFeedModel>>();
            var returnList = new List<TransactionBankFeedModel>();
           var bankIdResult = await new TransActionDataService().GetBankId(Model[0].BankAccountNo);
            if (bankIdResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Bank Account not found:" + bankIdResult.Errors[0].DeveloperMessage, "Bank Account not found", ""));
                return processingResult;
            }

           var _bankId=bankIdResult.Data;

            foreach (var item in Model)
            {
              var ruleResult  = await new TransActionDataService().GetNameAccountRuleData(item.NAME);
                if (ruleResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Rules not found:" + ruleResult.Errors[0].DeveloperMessage, "Rules not found", ""));
                    return processingResult;
                }
                RuleData _nameAccountData=ruleResult.Data;

                var matchResult = await new TransActionDataService().Match(item, _nameAccountData);
                if (matchResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Error matching rule:" + matchResult.Errors[0].DeveloperMessage, "Error matching rule", ""));
                    return processingResult;
                }
            string status= matchResult.Data;

                var crudModel = new TransactionBankFeedModel()
                {
                    AccountId = _bankId,
                    ChildAccountId = _nameAccountData.Name != null ? _nameAccountData.AccountId : 0,
                    Credit = item.TRNAMT < 0 ? null : item.TRNAMT.normalize(),
                    Debit = item.TRNAMT > 0 ? null : item.TRNAMT.normalize(),
                    TransType = item.TRNAMT > 0 ? "DEP" : "CHK",
                    TransDate = item.DTPOSTED,
                    PayeeId = _nameAccountData.Name != null ? _nameAccountData.PayeeId : 0,
                    DownloadName = item.NAME,
                    FITID = item.FITID,
                    Status = status,              //"AUTO" : "REV",Rules
                    Import = status == "REV" ? false : true,

                };
                returnList.Add(crudModel);
            }
            processingResult.Data = returnList;
            return processingResult;
        }

        public async Task<ApiProcessingResult> SetImported(string FITID)
        {
           
            List<string> _ids = new List<string>() { FITID };
           var result=await this.SetImported(_ids);
            if (result.IsError) { 
            //log error
            }
           
            return result;

        }
        public async Task<ApiProcessingResult> SetImported(List<string> FITIDs)
        {
            var processingResult = new ApiProcessingResult();
           
            var result = await new TransActionDataService().SetImported(FITIDs);
            if (result.IsError)
            {

                //log error
                processingResult.IsError = true;
                processingResult.Errors = result.Errors;
                return processingResult;
            }
            return processingResult;

        }
        public static List<TransActionCrudModel>MatchData(TransActionCrudModel _parentData, List<TransActionCrudModel> _childData)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            // will always have at least one row or more

            if (_childData.Count == 1)
            {
                if (_parentData.ChildAccountId == 0)
                {
                    _parentData.ChildAccountId = _childData[0].AccountId;
                    _childData[0].ChildAccountId = _parentData.AccountId;
                }
                else
                {
                    _childData[0].AccountId = _parentData.ChildAccountId;
                    _parentData.ChildAccountId = _childData[0].AccountId;


                }
                //----------------------------------------------------
                if (_parentData.Credit != null)
                {
                    _childData[0].Debit = _parentData.Credit;
                }
                else if (_parentData.Debit != null)
                {
                    _childData[0].Credit = _parentData.Debit;
                }
                else if (_childData[0].Credit != null)
                {
                    _parentData.Debit = _childData[0].Credit;
                }
                else if (_childData[0].Debit != null)
                {
                    _parentData.Credit = _childData[0].Debit;
                }
                //-----------------------------------------------               
            }
            else
            {
                _parentData.ChildAccountId = 1;//split account Id Transaction id ties them together
            }
            var parentData = SetTransActionType(_parentData, _childData);
            var returnData = new List<TransActionCrudModel>();



            if (parentData != null)
            {
                foreach (var item in _childData)
                {
                    item.TransDate = parentData.TransDate;
                    item.PayeeId = parentData.PayeeId;
                    item.TransType = parentData.TransType;
                    item.RefNumber = parentData.RefNumber;
                    item.ChildAccountId = parentData.AccountId;

                }
                returnData.Clear();
                returnData.Add(parentData);
                returnData.AddRange(_childData);

            }


            return returnData;
        }
        public static TransActionCrudModel SetTransActionType(TransActionCrudModel parentData, List<TransActionCrudModel> childData)
        {
            var accountResult = new SysDataService().GetSysAccounts();
            if (accountResult.Result.IsError)
            {
                //log error
                return null;
            }
            List<AccountModel> Accounts = accountResult.Result.Data;
            var parentAccount = Accounts.Find(x => x.Id == parentData.AccountId);

            var parentAccountType = parentAccount.AccountTypeId;
            int childAccountType = 0;
            if (childData.Count == 1)
            {
                var childAccount = Accounts.Find(x => x.Id == childData[0].AccountId);
                childAccountType = childAccount.AccountTypeId;
            }
            switch (parentAccountType)
            {
                case (int)AccountType.Bank:

                case (int)AccountType.CreditCard:
                    if (childData.Count > 1)
                    {
                        if (parentData.Debit > 0)
                        {
                            parentData.TransType = "CHK";
                        }
                        else if (parentData.Credit > 0)
                        {
                            parentData.TransType = "DEP";
                        }
                    }
                    else
                    {

                        if (childAccountType == (int)AccountType.Bank || childAccountType == (int)AccountType.CreditCard || childAccountType == (int)AccountType.OtherCurrentAsset
                          || childAccountType == (int)AccountType.OtherAsset || childAccountType == (int)AccountType.LongTermLiablility
                          || childAccountType == (int)AccountType.OtherCurrentLiablility || childAccountType == (int)AccountType.FixedAsset)
                        {
                            parentData.TransType = "TRANSF";
                        }
                        else if (parentData.Debit > 0)
                        {
                            parentData.TransType = "CHK";
                        }
                        else if (parentData.Credit > 0)
                        {
                            parentData.TransType = "DEP";
                        }

                    }
                    break;

                case (int)AccountType.AccountReceivable:

                    break;
                case (int)AccountType.OtherAsset:

                case (int)AccountType.OtherCurrentAsset:

                case (int)AccountType.FixedAsset:
                    parentData.TransType = "TRANSF";
                    break;
                case (int)AccountType.OtherCurrentLiablility:

                case (int)AccountType.LongTermLiablility:
                    parentData.TransType = "TRANSF";
                    break;
                case (int)AccountType.Equity:

                    break;

            }
            return parentData;
        }
    }
   
}

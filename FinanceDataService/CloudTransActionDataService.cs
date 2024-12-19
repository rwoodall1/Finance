using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using BindingModels;
using RESTModule;
using System.Text.Json;
using NLog;
using Equin.ApplicationFramework;
using System.Data;
using Microsoft.Data.SqlClient;
namespace DataService
{
    public class CloudTransActionDataService: ITransActionDataService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChildTransactions(int transActionId, int parentId)
        {

            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();

            var result= await new RESTService().MakeRESTCall("GET","data","URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            } 
            var dataResult = JsonSerializer.Deserialize<List<TransActionCrudModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChecks(int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<TransActionCrudModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<BindingListView<TransActionCrudModel>>>? GetChildCheckTransactions(int ChildAccountId)
        {
            var processingResult = new ApiProcessingResult<BindingListView<TransActionCrudModel>>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<BindingListView<TransActionCrudModel>>(result.Data.APIResult);
            
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetTransactions(int accountId, UnReconciled unReconciled = UnReconciled.No)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<TransActionCrudModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>>? Insert(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<bool>>? Update(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;



        }
        public async Task<ApiProcessingResult<EditAccountModel>>? GetAccount(int CurrentId)
        {
            var processingResult = new ApiProcessingResult<EditAccountModel>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<EditAccountModel>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<List<AccountTypeModel>>>? GetLookUpAccountTypes()
        {
            var processingResult = new ApiProcessingResult<List<AccountTypeModel>>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<AccountTypeModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;
        }

        public async Task<ApiProcessingResult<int>> AddAccount(EditAccountModel data)
        {
            var processingResult = new ApiProcessingResult<int>() { };

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<string>(result.Data.APIResult);
            //processingResult.Data = dataResult;
            return processingResult;

        }

        public async Task<ApiProcessingResult<bool>> EditAccount(EditAccountModel data)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;



        }
        public async Task<ApiProcessingResult<string>> GetAccountBalance(int accountId)
        {
            var processingResult = new ApiProcessingResult<string>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<string>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<NameModel>>? GetName(int id)
        {

            var processingResult = new ApiProcessingResult<NameModel>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<NameModel>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<NameListModel>>>? GetNameList(string nameType)
        {
            var processingResult = new ApiProcessingResult<List<NameListModel>>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<NameListModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>> EditName(NameModel data)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<string>> AddName(NameModel data)
        {
            var processingResult = new ApiProcessingResult<string>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            string dataResult = JsonSerializer.Deserialize<string>(result.Data.APIResult);
            processingResult.Data =dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<RuleData>> GetNameAccountRuleData(string Name)
        {

            var processingResult = new ApiProcessingResult<RuleData>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<RuleData>(result.Data.APIResult);
            processingResult.Data = dataResult;
            return processingResult;

        }
        public async Task<ApiProcessingResult<bool>> SetImported(List<string> fTTIDs)
        {
            var processingResult = new ApiProcessingResult<bool>();
            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error setting transaction as imported:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<RuleData>(result.Data.APIResult);
           

            return processingResult;
        }
        public async Task<ApiProcessingResult<List<STMTTRN>>> GetImportedData(string bankNo)
        {
            var processingResult = new ApiProcessingResult<List<STMTTRN>>();
            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting imported transactions :" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<STMTTRN>(result.Data.APIResult);
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>> InsertImportedParentData(List<STMTTRN> _importedData)
        {
            var processingResult = new ApiProcessingResult<bool>();
            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error importing transactions:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);

            return processingResult;
        }
        public async Task<ApiProcessingResult<int>> GetBankId(string bankAccountNo)
        {
            var processingResult = new ApiProcessingResult<int>();
            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting BankId:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            return processingResult;

        }
        public async Task<ApiProcessingResult<bool>> SetImported(string fTTID)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error setting data as imported.:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>> AddNewRule(TransactionBankFeedModel data)
        {
            var processingResult = new ApiProcessingResult<bool>();

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error setting data as imported.:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            return processingResult;
        }
        public async Task<ApiProcessingResult<string>> Match(STMTTRN importData, RuleData ruleResult)
        {
            var processingResult = new ApiProcessingResult<string>();

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error setting data as imported.:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);
            return processingResult;


        }
        public async Task<ApiProcessingResult<Balances>> GetLastStatementBalance(int accountId)
        {
            var processingResult = new ApiProcessingResult<Balances>();

            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error setting data as imported.:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<Balances>(result.Data.APIResult);
               processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>>? SetReconciled(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data=true};

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("There was an error marking record as cleard:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = false;
                return  processingResult;
            }
            var dataResult = JsonSerializer.Deserialize<Balances>(result.Data.APIResult);
         
            return processingResult;

        }
        public async Task<ApiProcessingResult<bool>>? SetCleared(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("There was an error marking record as cleard:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = false;
                return processingResult;
            }
            var dataResult = JsonSerializer.Deserialize<Balances>(result.Data.APIResult);

            return processingResult;

        }
        public async Task<ApiProcessingResult<int>>? InsertUpdateReconciliation(ReconciledData model)
        {
            var processingResult = new ApiProcessingResult<int>();

            var result = await new RESTService().MakeRESTCall("POST", "data", "URL");

            if (result.IsError)
            {
                Log.Error("There was an error marking record as cleard:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
               
                return processingResult;
            }
            var dataResult = JsonSerializer.Deserialize<Balances>(result.Data.APIResult);

            return processingResult;

        }

        public async Task<ApiProcessingResult<bool>>? SetRegisterColor(int accountId, int color)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };

            var result = await new RESTService().MakeRESTCall("Get", "data", "URL");

            if (result.IsError)
            {
                Log.Error( result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = false;
                return processingResult;
            }
            var dataResult = JsonSerializer.Deserialize<bool>(result.Data.APIResult);

            return processingResult;


        }
        }
}

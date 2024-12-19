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
    public class CloudsysDataService : ISysData
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        public async Task<ApiProcessingResult<List<NameLkpModel>>>? GetNames()
        {
            var processingResult = new ApiProcessingResult<List<NameLkpModel>>();
            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<NameLkpModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;

            return processingResult;
        }
        public async Task<ApiProcessingResult<List<AccountModel>>> GetSysAccounts()
        {

            var processingResult = new ApiProcessingResult<List<AccountModel>>();
            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<AccountModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;


            return processingResult;
        }
        public async Task<ApiProcessingResult<List<AccountLkpModel>>> GetSysLkpAccounts(AccountLkpType type, int typeId = 0, string typename = "")
        {
            var processingResult = new ApiProcessingResult<List<AccountLkpModel>>();
            var result = await new RESTService().MakeRESTCall("GET", "data", "URL");

            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);

                return null;
            }
            var dataResult = JsonSerializer.Deserialize<List<AccountLkpModel>>(result.Data.APIResult);
            processingResult.Data = dataResult;

            return processingResult;
        }

    }
}

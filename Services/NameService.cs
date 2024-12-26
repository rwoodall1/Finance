using BindingModels;
using Core;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NameService : BaseDataService
    {
        public async Task<ApiProcessingResult<List<CompleteNameModel>>>? GetOtherNames()
        {
            var processingResult = new ApiProcessingResult<List<CompleteNameModel>>();
            var sqlQuery = new SQLCustomClient();
            sqlQuery.ClearParameters();
            sqlQuery.CommandText("Select Id,FullName,NameType,EmailAddress,PhoneNumber,MobilePhone,InActive From Names Where NameType=@NameType order By FullName");
            sqlQuery.AddParameter("@NameType", "OTHER NAME");
            var result = sqlQuery.SelectMany<CompleteNameModel>();
            if (result.IsError)
            {
                log.Error("Failed to retrieve names:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = new List<CompleteNameModel>();
                return processingResult;
            }
            

            processingResult.Data =(List<CompleteNameModel>) result.Data;
            return processingResult;

        }
        public async Task<ApiProcessingResult<CompleteNameModel>>? GetOtherName(int id)
        {
            var processingResult = new ApiProcessingResult<CompleteNameModel>();
            var sqlQuery = new SQLCustomClient();
            sqlQuery.ClearParameters();
            sqlQuery.CommandText("Select Id,FullName,NameType,FirstName,LastName,EmailAddress,PhoneNumber,MobilePhone,InActive From Names Where Id=@Id");
            sqlQuery.AddParameter("@Id", id);
            var result = sqlQuery.Select<CompleteNameModel>();
            if (result.IsError)
            {
                log.Error("Failed to retrieve names:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve names:" + result.Errors[0].DeveloperMessage, "Failed to retrieve names:" + result.Errors[0].DeveloperMessage,""));
                processingResult.Data = new CompleteNameModel();
                return processingResult;
            }

            processingResult.Data = (CompleteNameModel)result.Data;
            return processingResult;

        }
    }
}

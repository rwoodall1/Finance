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
        public async Task<ApiProcessingResult> DeleteOtherName(int nameId)
        {
            var processingResult = new ApiProcessingResult();
            var sqlClient = new SQLCustomClient().CommandText("Delete from Names where Id=@Id");
            sqlClient.AddParameter("@Id", nameId);
            var result=sqlClient.Delete();
            if (result.IsError)
            {
                log.Error("Failed to delete record:" + nameId.ToString());
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to delete record", "Failed to delete record",""));
                return processingResult;
            }
            return processingResult;

        }
        public async Task<ApiProcessingResult>SaveOtherName(CompleteNameModel model)
        {
            var processingResult=new ApiProcessingResult();
            var sqlClient = new SQLCustomClient();
            string cmd = "";
            if (model.Id > 0)
            {
                cmd = @"Update Names Set FullName=@FullName,FirstName=@FirstName,LastName=@LastName
                    ,PhoneNumber=@PhoneNumber,EmailAddress=@EmailAddress,MobilePhone=@MobilePhone,InActive=@InActive where Id=@Id";
                sqlClient.CommandText(cmd);
                sqlClient.AddParameter("@FullName",model.FullName);
                sqlClient.AddParameter("@FirstName",model.FirstName);
                sqlClient.AddParameter("@LastName",model.LastName);
                sqlClient.AddParameter("@PhoneNumber",model.PhoneNumber);
                sqlClient.AddParameter("@EmailAddress",model.EmailAddress);
                sqlClient.AddParameter("@MobilePhone",model.MobilePhone);
                sqlClient.AddParameter("@Id", model.Id);
                sqlClient.AddParameter("@InActive",model.InActive);
                var updateResult = sqlClient.Update();
                if (updateResult.IsError) {
                    log.Error(updateResult.Errors[0].DeveloperMessage);
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to save name:"+ updateResult.Errors[0].DeveloperMessage, "Failed to save name:" + updateResult.Errors[0].DeveloperMessage,""));
                return processingResult;
                }
            }
            else
            {
               cmd= @"Insert Into Names (FullName,FirstName,LastName,PhoneNumber
                    ,EmailAddress,MobilePhone,InActive,NameType) Values(@FullName,@FirstName,@LastName
                    ,@PhoneNumber,@EmailAddress,@MobilePhone,@InActive,@NameType)";
                sqlClient.CommandText(cmd);
                sqlClient.AddParameter("@FullName", model.FullName);
                sqlClient.AddParameter("@FirstName", model.FirstName);
                sqlClient.AddParameter("@LastName", model.LastName);
                sqlClient.AddParameter("@PhoneNumber", model.PhoneNumber);
                sqlClient.AddParameter("@EmailAddress", model.EmailAddress);
                sqlClient.AddParameter("@MobilePhone", model.MobilePhone);
                 sqlClient.AddParameter("@InActive", model.InActive);
                sqlClient.AddParameter("@NameType", "OTHER NAME");
                var insertResult= sqlClient.Insert();
                if (insertResult.IsError) {
                    log.Error(insertResult.Errors[0].DeveloperMessage);
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to save name:" + insertResult.Errors[0].DeveloperMessage, "Failed to save name:" + insertResult.Errors[0].DeveloperMessage,""));
                }
            }

            return processingResult;
        }
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

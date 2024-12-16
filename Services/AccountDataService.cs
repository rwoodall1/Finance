
using BindingModels;
using Core;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class AccountDataService : BaseDataService
    {
        public async Task<ApiProcessingResult> SaveAccount(AccountModel model) {

            var processingResult = new ApiProcessingResult();
            var sqlClient = new SQLCustomClient();
            if (model.Id>0) {
                string updatecmd = @"UPDATE [dbo].[Accounts]
            SET [Name] = @Name
            ,[AccountType] = @AccountType      
            ,[Description] = @Description
            ,[AccountNo] = @AccountNo
            ,[AccountTypeId] =@AccountTypeId
            ,[ParentAccountId] =@ParentAccountId 
            ,IsSubAccount=@IsSubAccount
            WHERE Id=@Id";
                sqlClient.AddParameter("@Name", model.Name);
                sqlClient.AddParameter("@AccountType", model.AccountType);
                sqlClient.AddParameter("@Description", model.Description);
                sqlClient.AddParameter("@AccountNo", model.AccountNo);
                sqlClient.AddParameter("@AccountTypeId", model.AccountTypeId);
                sqlClient.AddParameter("@ParentAccountId", model.ParentAccountId);
                sqlClient.AddParameter("@IsSubAccount", model.IsSubAccount);
                sqlClient.AddParameter("@Id", model.Id);
                sqlClient.CommandText(updatecmd);
                var result = sqlClient.Update();
                if (result.IsError)
                {
                    log.Error("Error Updating Account:" + result.Errors[0].DeveloperMessage);
                    processingResult.Errors.Add(new ApiProcessingError("Error Updating Account", "Error Updating Account", ""));
                    processingResult.IsError = true;
                    return processingResult;
                }

                return processingResult;

            }
            else
            {
                string insertcmd = @"Insert INTO Accounts (Name,AccountType,Description,AccountNo,AccountTypeId,ParentAccountId,IsSubAccount)
                Values(@Name,@AccountType,@Description,@AccountNo,@AccountTypeId,@ParentAccountId,@IsSubAccount)";
                sqlClient.AddParameter("@Name", model.Name);
                sqlClient.AddParameter("@AccountType", model.AccountType);
                sqlClient.AddParameter("@Description", model.Description);
                sqlClient.AddParameter("@AccountNo", model.AccountNo);
                sqlClient.AddParameter("@AccountTypeId", model.AccountTypeId);
                sqlClient.AddParameter("@ParentAccountId", model.ParentAccountId);
                sqlClient.AddParameter("@IsSubAccount", model.IsSubAccount);

                sqlClient.CommandText(insertcmd);
                var result = sqlClient.Insert();
                if (result.IsError)
                {
                    log.Error("Error Insertintg Account:" + result.Errors[0].DeveloperMessage);
                    processingResult.Errors.Add(new ApiProcessingError("Error Inserting Account", "Error Inserting Account", ""));
                    processingResult.IsError = true;
                    return processingResult;
                }

                return processingResult;

            }


            

           
            return processingResult;

        }
        public async Task<ApiProcessingResult<List<SubAccount>>>GetSubAccounts(string type)
        {
            var processingResult = new ApiProcessingResult<List<SubAccount>>();
            var sqlClient = new SQLCustomClient();
            string cmd = @"Select Id ,Name ,AccountType  FROM Accounts Where AccountType=@Type  Order By Name";
            sqlClient.AddParameter("@Type", type);                                          
            sqlClient.CommandText(cmd);
            var result = sqlClient.SelectMany<SubAccount>();
            if (result.IsError)
            {
                log.Error("Error Retieving Sub Accounts:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;
            }

            processingResult.Data = (List<SubAccount>) result.Data;
            return processingResult;


        }
        public async Task<ApiProcessingResult<List<AccountNode>>> GetAllAccounts()
        {
            var processingResult = new ApiProcessingResult<List<AccountNode>>();
            var sqlClient = new SQLCustomClient();
          
            //check if user email exist--Check if OpyProduct exists
            string cmd = @"Select Id
                                        ,ParentAccountId
                                        ,Name
                                        ,Description
                                        ,AccountNo
                                        ,IsActive
                                        ,AccountType
                                        ,AccountTypeId
                                        ,AccountBalance 
                                        FROM Accounts Where Id!=1  Order By AccountTypeId,Name";
            sqlClient.CommandText(cmd);

            var result = sqlClient.SelectMany<AccountModel>();
            if (result.IsError)
            {
                // Log.Error("Error Retieving Accounts:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;
            }

            var rawDataList = (List<AccountModel>)result.Data;
            var builtChartOfAccounts = this.BuildAccounts(rawDataList);
            processingResult.Data = builtChartOfAccounts;
            return processingResult;


        }

        public async Task<ApiProcessingResult<List<AccountTypeModel>>> GetAccountTypes()
        {
            var processingResult = new ApiProcessingResult<List<AccountTypeModel>>();
            var sqlClient = new SQLCustomClient();


            string cmd = @"SELECT  [Name]
                              ,[GroupId]
                              ,[Id]
                              ,Abbrev
                          FROM [AccountTypes] Order By GroupId";


            sqlClient.CommandText(cmd);

            var result = sqlClient.SelectMany<AccountTypeModel>();
            if (result.IsError)
            {
                log.Error("Error Retieving Account Types:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;
            }

            var data = (List<AccountTypeModel>)result.Data;

            processingResult.Data = data;
            return processingResult;

        }

        public async Task<ApiProcessingResult<AccountModel>> GetAccount(string id)
        {
            var processingResult = new ApiProcessingResult<AccountModel>();
            var sqlClient = new SQLCustomClient();

            //check if user email exist--Check if OpyProduct exists
            string cmd = @"SELECT  [Name]
                              ,[AccountType]
                              ,[AccountBalance]
                              ,[Description]
                              ,[AccountNo]
                              ,[AccountTypeId]
                              ,[ParentAccountId]
                              ,[IsActive]
                              ,[RegColor]
                              ,[Id]
                          FROM [Accounts] Where Id=@Id ";
            sqlClient.AddParameter("@Id", id);
            sqlClient.CommandText(cmd);

            var result = sqlClient.Select<AccountModel>();
            if (result.IsError)
            {
                log.Error("Error Retieving Account:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;
            }

            var data = (AccountModel)result.Data;

            processingResult.Data = data;
            return processingResult;

        }
        public async Task<ApiProcessingResult<List<AccountModel>>> GetBankAccounts()
        {
            var processingResult = new ApiProcessingResult<List<AccountModel>>();
            var sqlClient = new SQLCustomClient();

            //check if user email exist--Check if OpyProduct exists
            string cmd = @"SELECT  [Name]
                              ,[AccountType]
                              ,[AccountBalance]
                              ,[Description]
                              ,[AccountNo]
                              ,[AccountTypeId]
                              ,[ParentAccountId]
                              ,[IsActive]
                              ,[RegColor]
                              ,[Id]
                          FROM [Accounts] Where UPPER(AccountType)=@AccountType And IsActive=1 ";
            sqlClient.AddParameter("@AccountType", "BANK");
            sqlClient.CommandText(cmd);

            var result = sqlClient.SelectMany<AccountModel>();
            if (result.IsError)
            {
                log.Error("Error Retieving BankAccounts:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Error Retieving BankAccounts", "Error Retieving BankAccounts", ""));
                return processingResult;

            }

            processingResult.Data=(List<AccountModel>)result.Data;
            return processingResult;

        }

        private List<AccountNode> BuildAccounts(List<AccountModel> rawDataList)
        {
            var accountNodes = new List<AccountNode>();

            foreach (var datarow in rawDataList)
            {
                AccountNode accountNode= new AccountNode();
                
                //TopAccount
                if (datarow.ParentAccountId == null || datarow.ParentAccountId == 0)
                {
                    accountNode.Data = datarow;
                    var ChildrenAccounts = GetChildren(rawDataList.FindAll(x => x.ParentAccountId == datarow.Id), rawDataList, datarow.Id);
                   accountNode.Children= ChildrenAccounts;

                    accountNodes.Add(accountNode);
                }
               
            }
     
            return accountNodes;
        }
        private List<AccountNode> GetChildren(List<AccountModel> dataList, List<AccountModel> rawDataList, int parentId)
        {
            List<AccountNode> returnList = new List<AccountNode>();
            foreach (var row in dataList)
            {
                AccountNode accountNode= new AccountNode();
                accountNode.Data=row; 
                var TopChildrenAccounts = GetChildren(rawDataList.FindAll(x => x.ParentAccountId == row.Id), rawDataList, row.Id);

                accountNode.Children = TopChildrenAccounts;


                returnList.Add(accountNode);

            }
            if (returnList.Count > 0) 
            {
                var a = 1;
                    }

            return returnList;
        }
    }
}

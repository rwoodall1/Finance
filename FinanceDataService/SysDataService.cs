using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BindingModels;
using SqlClient;
using NLog;
using Core;
using Services;
namespace DataService
{
    public class SysDataService : ISysData
    {
        static Logger Log { get; set; } = LogManager.GetCurrentClassLogger();

        public async Task<ApiProcessingResult<List<NameLkpModel>>>? GetNames()
        {
            var processingResult = new ApiProcessingResult<List<NameLkpModel>>();
            var sqlQuery = new SQLCustomClient();
            sqlQuery.ClearParameters();
            sqlQuery.CommandText("Select Id,FullName,NameType From Names order By NameType desc,FullName");

            var result = sqlQuery.SelectMany<NameLkpModel>();
            if (result.IsError)
            {
                Log.Error("Failed to retrieve names:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = new List<NameLkpModel>();
                return processingResult;
            }
            var dataList = new List<NameLkpModel>();
            dataList.Add(new NameLkpModel() { FullName = "", Id = 0, NameType = "" });
            dataList.Add(new NameLkpModel() { FullName = "<Add New>", Id = -1, NameType = "" });
            dataList.AddRange((List<NameLkpModel>)result.Data);
            processingResult.Data = dataList;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<AccountModel>>> GetSysAccounts()
        {

            var processingResult = new ApiProcessingResult<List<AccountModel>>();
            var sqlQuery = new SQLCustomClient();
            sqlQuery.ClearParameters();
            sqlQuery.AddParameter("@Id", 1);//split account we don't want.
            sqlQuery.CommandText(@"Select Id
                                        ,ParentAccountId
                                        ,Name
                                        ,Description
                                        ,AccountNo
                                        ,IsActive
                                        ,AccountType
                                        ,AccountTypeId
                                        ,AccountBalance 
                                        FROM Accounts where Id !=@Id Order By AccountTypeId,Name");
            var result = sqlQuery.SelectMany<AccountModel>();
            if (result.IsError)
            {
                Log.Error("Error Retieving Accounts:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = new List<AccountModel>();
                // CMBox.Error("Error Retieving Accounts");
                return processingResult;
            }

            var rawDataList = (List<AccountModel>)result.Data;
            processingResult.Data = rawDataList;
            return processingResult;
        }

        public async Task<ApiProcessingResult<List<AccountNode>>> GetWebChartOfAccounts()
        {

            var processingResult = new ApiProcessingResult<List<AccountNode>>();
            var allAccountResult = await this.GetSysAccounts();
            if (allAccountResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = allAccountResult.Errors;
                //return processingResult;
            }

            var rawDataList = (List<AccountModel>)allAccountResult.Data;
            var builtChartOfAccounts = this.BuildAccountsWeb(rawDataList);
            processingResult.Data = builtChartOfAccounts;
            return processingResult;
        }

        public async Task<ApiProcessingResult<List<AccountLkpModel>>> GetSysLkpAccounts(AccountLkpType type, int typeId = 0, string typename = "")
        {
            var processingResult = new ApiProcessingResult<List<AccountLkpModel>>();
            var sqlQuery = new SQLCustomClient();
            sqlQuery.ClearParameters();
            string cmd = @"Select Id
                                    ,ParentAccountId
                                    ,Name
                                        ,Description
                                        ,AccountNo
                                        ,IsActive
                                        ,AccountType
                                        ,AccountTypeId
                                        ,AccountBalance 
                                        ,RegColor
                                        FROM Accounts ";
            string where = "";
            switch (type)
            {
                case AccountLkpType.All:
                    where = @" WHERE IsActive=1";
                    break;
                case AccountLkpType.TypeId:
                    where = @" WHERE IsActive =1 AND AccountTypeId=@AccountTypeId";
                    sqlQuery.AddParameter("@AccountTypeId", typeId);
                    break;

                case AccountLkpType.TypeIdLessThan:
                    where = @" WHERE IsActive=1 AND AccountTypeId<=@AccountTypeId";
                    sqlQuery.AddParameter("@AccountTypeId", typeId);
                    break;

                case AccountLkpType.TypeName:
                    where = @" WHERE IsActive=1 AND AccountType=@AccountType";
                    sqlQuery.AddParameter("@AccountType", typename);
                    break;
            }
            string orderBy = " Order By AccountTypeId,Name";
            cmd = cmd + where + orderBy;
            sqlQuery.CommandText(cmd);
            var result = sqlQuery.SelectMany<AccountModel>();
            if (result.IsError)
            {
                Log.Error("Error Retieving Accounts:" + result.Errors[0].DeveloperMessage);
                // CMBox.Error(result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Data = new List<AccountLkpModel>();
                return processingResult;
            }

            var rawDataList = (List<AccountModel>)result.Data;
            var finalList = new List<AccountModel>();

            foreach (var datarow in rawDataList)
            {
                //TopAccount
                if (datarow.ParentAccountId == 0)
                {
                    var ChildrenAccounts = GetChildren(rawDataList.FindAll(x => x.ParentAccountId == datarow.Id), rawDataList, datarow.Id);
                    datarow.SubAccounts = ChildrenAccounts.Result.Data;
                    finalList.Add(datarow);
                }

            }

            List<AccountLkpModel> AccountList = new List<AccountLkpModel>();
            var vnewChildAcc = new AccountLkpModel()
            {
                Name = "",
                AccountType = "",
                IsActive = true,
                RegColor=0,
                Id = 0

            };
            AccountList.Add(vnewChildAcc);
            var vnewChildAcc1 = new AccountLkpModel()
            {
                Name = "<Add New>",
                AccountType = "",
                IsActive = true,
                RegColor=0,
                Id = -1

            };
            AccountList.Add(vnewChildAcc1);
            foreach (var row in finalList)
            {
                if (row.IsActive == true)
                {
                    var rec = new AccountLkpModel()
                    {
                        Name = row.Name,
                        AccountType = row.AccountType,
                        IsActive = row.IsActive,
                        RegColor=row.RegColor,
                        Id = row.Id

                    };
                    AccountList.Add(rec);
                    if (row.SubAccounts.Count > 0)
                    {
                        AccountList = LoadChildLookUpDataChildAcc(row.SubAccounts, 1, AccountList).Result.Data;


                    }
                }

            }
            processingResult.Data = AccountList;

            return processingResult;
        }
     
        public async Task<ApiProcessingResult<List<AccountModel>>> GetChildren(List<AccountModel> dataList, List<AccountModel> rawDataList, int parentId)
        {
            var processingResult = new ApiProcessingResult<List<AccountModel>>();
            List<AccountModel> returnList = new List<AccountModel>();
            foreach (var row in dataList)
            {

                var TopChildrenAccounts = GetChildren(rawDataList.FindAll(x => x.ParentAccountId == row.Id), rawDataList, row.Id);

                row.SubAccounts = TopChildrenAccounts.Result.Data;


                returnList.Add(row);

            }
            processingResult.Data = returnList;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<AccountLkpModel>>> LoadChildLookUpDataChildAcc(List<AccountModel> childData, int level, List<AccountLkpModel> AccountList)
        {
            var processingResult = new ApiProcessingResult<List<AccountLkpModel>>();
            foreach (var childrow in childData)
            {
                string indent = "";
                switch (level)
                {
                    case 1:
                        indent = "    ";
                        break;
                    case 2:
                        indent = "        ";
                        break;
                    case 3:
                        indent = "            ";
                        break;
                    case 4:
                        indent = "                ";
                        break;
                    case 5:
                        indent = "                    ";
                        break;
                    case 6:
                        indent = "                        ";
                        break;
                }

                if (childrow.IsActive == true)
                {
                    var rec1 = new AccountLkpModel()
                    {
                        Name = indent + childrow.Name,
                        AccountType = childrow.AccountType,
                        IsActive = childrow.IsActive,
                        Id = childrow.Id

                    };
                    AccountList.Add(rec1);
                    if (childrow.SubAccounts != null && childrow.SubAccounts.Count > 0)
                    {
                        AccountList = LoadChildLookUpDataChildAcc(childrow.SubAccounts, level + 1, AccountList).Result.Data;
                    }
                }
            }
            processingResult.Data = AccountList;
            return processingResult;
        }
        //private List<AccountModel> BuildAccounts(List<AccountModel> rawDataList)
        //{
        //    var accountNodes = new List<AccountModel>();

        //    foreach (var datarow in rawDataList)
        //    {
        //        AccountNode accountNode = new AccountNode();

        //        //TopAccount
        //        if (datarow.ParentAccountId == null || datarow.ParentAccountId == 0)
        //        {
        //            accountNode.Data = datarow;
        //            var ChildrenAccounts = GetChildren(rawDataList.FindAll(x => x.ParentAccountId == datarow.Id), rawDataList, datarow.Id);
        //            accountNode.Children = ChildrenAccounts;

        //            accountNodes.Add(accountNode);
        //        }

        //    }

        //    return accountNodes;
        //}
        private List<AccountNode> BuildAccountsWeb(List<AccountModel> rawDataList)
        {
            var accountNodes = new List<AccountNode>();

            foreach (var datarow in rawDataList)
            {
                AccountNode accountNode = new AccountNode();

                //TopAccount
                if (datarow.ParentAccountId == null || datarow.ParentAccountId == 0)
                {
                    accountNode.Data = datarow;
                    var ChildrenAccounts = GetChildrenWeb(rawDataList.FindAll(x => x.ParentAccountId == datarow.Id), rawDataList, datarow.Id);
                    accountNode.Children = ChildrenAccounts;

                    accountNodes.Add(accountNode);
                }

            }

            return accountNodes;
        }
        private List<AccountNode> GetChildrenWeb(List<AccountModel> dataList, List<AccountModel> rawDataList, int parentId)
        {
            List<AccountNode> returnList = new List<AccountNode>();
            foreach (var row in dataList)
            {
                AccountNode accountNode = new AccountNode();
                accountNode.Data = row;
                var TopChildrenAccounts = GetChildrenWeb(rawDataList.FindAll(x => x.ParentAccountId == row.Id), rawDataList, row.Id);

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

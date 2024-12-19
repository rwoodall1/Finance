using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using BindingModels;
using Equin.ApplicationFramework;
namespace DataService
{
   public interface ITransActionDataService
    {
       //transactions
        public  Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChildTransactions(int transActionId, int parentId);
        public Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChecks(int accountId);
        public Task<ApiProcessingResult<Balances>> GetLastStatementBalance(int accountId);
        public Task<ApiProcessingResult<BindingListView<TransActionCrudModel>>>? GetChildCheckTransactions(int ChildAccountId);
        public Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetTransactions(int accountId, UnReconciled unReconciled = UnReconciled.No);
        public Task<ApiProcessingResult<bool>>? Insert(List<TransActionCrudModel> data);
        public Task<ApiProcessingResult<bool>>? Update(List<TransActionCrudModel> data);
        public Task<ApiProcessingResult<bool>>? SetReconciled(List<TransActionCrudModel> data);
        public Task<ApiProcessingResult<bool>>? SetCleared(List<TransActionCrudModel> data);
        public Task<ApiProcessingResult<int>>? InsertUpdateReconciliation(ReconciledData model);


        //Accounts
        public Task<ApiProcessingResult<bool>>? SetRegisterColor(int accountId, int color);
        public Task<ApiProcessingResult<EditAccountModel>>? GetAccount(int CurrentId);
        public Task<ApiProcessingResult<List<AccountTypeModel>>>? GetLookUpAccountTypes();
        public Task<ApiProcessingResult<int>> AddAccount(EditAccountModel data);
        public Task<ApiProcessingResult<bool>> EditAccount(EditAccountModel data);
        public Task<ApiProcessingResult<string>> GetAccountBalance(int accountId);
        public Task<ApiProcessingResult<string>> Match(STMTTRN importData, RuleData ruleResult);


        //Names
        public Task<ApiProcessingResult<NameModel>>? GetName(int id);
        public Task<ApiProcessingResult<List<NameListModel>>>? GetNameList(string nameType);
        public Task<ApiProcessingResult<bool>> EditName(NameModel data);
        public Task<ApiProcessingResult<string>> AddName(NameModel data);
        public Task<ApiProcessingResult<RuleData>> GetNameAccountRuleData(string Name);
        public Task<ApiProcessingResult<bool>> SetImported(List<string> fTTIDs);
        public Task<ApiProcessingResult<List<STMTTRN>>> GetImportedData(string bankNo);
        public Task<ApiProcessingResult<bool>> InsertImportedParentData(List<STMTTRN> _importedData);
        public Task<ApiProcessingResult<int>> GetBankId(string bankAccountNo);
        public Task<ApiProcessingResult<bool>> AddNewRule(TransactionBankFeedModel data);
       
    }
}

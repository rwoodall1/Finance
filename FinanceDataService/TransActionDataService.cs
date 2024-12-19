using ApiBindingModels;
using BindingModels;
using Core;
using Equin.ApplicationFramework;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using NLog;
using SqlClient;
using Utilities;
using Logger = NLog.Logger;

namespace DataService
{
    public class TransActionDataService:ITransActionDataService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static SQLCustomClient _client { get; set; } = new SQLCustomClient();
       //transactions
        public async  Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChildTransactions(int transActionId, int parentId)
        {

            var processingResult=new ApiProcessingResult<List<TransActionCrudModel>>();

            _client.ClearParameters();
            string cmd = @"Select Id,TransActionId,PayeeId,AccountId,ChildAccountId,TransType,refNumber,Memo,Reconciled,TransDate,Debit,Credit
                  From TransActions where TransActionId=@TransactId and AccountId !=@AccountId";
            _client.CommandText(cmd);
            _client.AddParameter("@TransactId", transActionId);
            _client.AddParameter("@AccountId", parentId);
            var result = _client.SelectMany<TransActionCrudModel>();
            if (result.IsError)
            {
                Log.Error("Error getting child transactrion:" + result.Errors[0].DeveloperMessage);
               processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Error getting child transactrion:" + result.Errors[0].DeveloperMessage, "Error getting child transactrion:" + result.Errors[0].DeveloperMessage,""));
                return null;
            }
            var dataResult = (List<TransActionCrudModel>)result.Data;
            processingResult.Data = dataResult;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetChecks(int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            _client.ClearParameters();
            _client.ClearParameters();
            string cmd = @"Select 
        Id,PTA.PayeeId,PTA.AccountId,PTA.TransActionId,PTA.ChildAccountId,PTA.TransType,PTA.RefNumber,PTA.Memo,PTA.TransDate,PTA.Credit,PTA.Debit
        From TransActions PTA WHERE PTA.AccountId=@AccountId and TransType='CHK' Order By TransDate,RefNumber";
            _client.CommandText(cmd);
            _client.AddParameter("@AccountId", accountId);
            var transactionResult = _client.SelectMany<TransActionCrudModel>();
            if (transactionResult.IsError)
            {
                Log.Error("Failed to retrieve check transactions");
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve check transactions:" + transactionResult.Errors[0].DeveloperMessage, "Failed to retrieve check transactions:" + transactionResult.Errors[0].DeveloperMessage, ""));
               
                return processingResult;
            }
            var data = (List<TransActionCrudModel>)transactionResult.Data;
            processingResult.Data = data;
            return processingResult;

        }
        public async Task<ApiProcessingResult<BindingListView<TransActionCrudModel>>>? GetChildCheckTransactions(int ChildAccountId)
        {
            var processingResult = new ApiProcessingResult<BindingListView<TransActionCrudModel>>();
            _client.ClearParameters();
            string cmd = @"Select A.Name As AccountName, CTA.Id,CTA.PayeeId,CTA.AccountId,CTA.TransActionId,CTA.ChildAccountId,CTA.TransType,CTA.RefNumber,CTA.Memo,CTA.TransDate,CTA.Credit,CTA.Debit
                            From TransActions CTA
                    Inner Join Accounts A On CTA.AccountId=A.Id
                        Where CTA.ChildAccountId=@ChildAccountId AND CTA.TransType='CHK' Order By TransActionId";
            _client.CommandText(cmd);
            _client.ReturnListView(true);
            _client.AddParameter("@ChildAccountId", ChildAccountId);

            var childResult = _client.SelectMany<TransActionCrudModel>();
            _client.ReturnListView(false);
            if (childResult.IsError)
            {
                Log.Error("Failed to get child Check records:" + childResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to get child Check records:" + childResult.Errors[0].DeveloperMessage, "Failed to get child Check records:" + childResult.Errors[0].DeveloperMessage,""));
                return processingResult;
            }
            var dataResult = (BindingListView<TransActionCrudModel>)childResult.Data;
            processingResult.Data = dataResult;
            return processingResult;


        }
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetTransactions(int accountId,UnReconciled reconciled)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            _client.ClearParameters();

            string cmd = @"SELECT  TA.Id
                                  ,TA.TransActionId
                                  ,TA.PayeeId
                                   ,N.FullName AS PayeeName
                                  ,TA.AccountId
                                   ,A.Name as ChildAccountName
                                   ,A2.Name as AccountName
                                  ,TA.ChildAccountId
                                  ,TA.TransType
                                  ,TA.RefNumber
                                  ,TA.Memo
                                  ,TA.Reconciled
                                  ,TA.TransDate
                                  ,TA.Debit
                                  ,TA.Credit
                                  ,TA.ReconciledId
                                  ,TA.Clr
                              FROM TransActions TA
                            Left Join Accounts A On TA.ChildAccountId=A.Id
                            Left Join Accounts A2 On TA.AccountId=A2.Id
                           Left Join Names N on TA.PayeeId=N.Id
                        Where TA.AccountId=@AccountId AND Reconciled= 0  Order By TransDate ";

            
            _client.CommandText(cmd);
            _client.AddParameter("@AccountId", accountId);
            var transactionResult = _client.SelectMany<TransActionCrudModel>();
            if (transactionResult.IsError)
            {
                Log.Error("Failed to retrieve transactions");
                processingResult.IsError = true;
                return processingResult;
            }

            var data = transactionResult.Data == null ? new List<TransActionCrudModel>(): (List<TransActionCrudModel>)transactionResult.Data;
            processingResult.Data = data;
            return processingResult;
        }
        public async Task<ApiProcessingResult<List<TransActionCrudModel>>>? GetTransactions(int accountId)
        {
            var processingResult = new ApiProcessingResult<List<TransActionCrudModel>>();
            _client.ClearParameters();
           
            string cmd = @"SELECT  TA.Id
                                  ,TA.TransActionId
                                  ,TA.PayeeId
                                   ,N.FullName AS PayeeName
                                  ,TA.AccountId
                                   ,A.Name as ChildAccountName
                                   ,A2.Name as AccountName
                                  ,TA.ChildAccountId
                                  ,TA.TransType
                                  ,TA.RefNumber
                                  ,TA.Memo
                                  ,TA.Reconciled
                                  ,TA.TransDate
                                  ,TA.Debit
                                  ,TA.Credit
                                  ,TA.ReconciledId
                                  ,TA.Clr
                              FROM TransActions TA
                            Left Join Accounts A On TA.ChildAccountId=A.Id
                            Left Join Accounts A2 On TA.AccountId=A2.Id
                           Left Join Names N on TA.PayeeId=N.Id
                        Where TA.AccountId=@AccountId Order By TransDate ";
            _client.CommandText(cmd);
            _client.AddParameter("@AccountId", accountId);
            var transactionResult = _client.SelectMany<TransActionCrudModel>();
            if (transactionResult.IsError)
            {
                Log.Error("Failed to retrieve transactions");
                processingResult.IsError = true;
                      return processingResult;
            }
            var data = (List<TransActionCrudModel>)transactionResult.Data;
            processingResult.Data = data;
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>>? Insert(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };
            string strConnString = ApplicationConfig.DefaultConnectionString;
            SqlTransaction objTrans = null;
            bool Errors = false;
            using (SqlConnection objConn = new SqlConnection(strConnString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();
                string cmd = @"Insert INTO TransActions (PayeeId,AccountId,TransActionId,ChildAccountId,TransType,RefNumber,Memo,TransDate,Credit,Debit) 
                                           Values(@PayeeId,@AccountId,@TransActionId,@ChildAccountId,@TransType,@RefNumber,@Memo,@TransDate,@Credit,@Debit)";
                SqlCommand objCmd1 = new SqlCommand(cmd, objConn);
                objCmd1.Transaction = objTrans;
                int? vTansactionId = NewTransActionId();
                if (vTansactionId==0 || vTansactionId==null)
                {
                    processingResult.Data = false;
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to get transaction id", "Failed to get transaction id", ""));
                    return processingResult;

                }
               
                foreach (var row in data)
                {
                    {
                        objCmd1.Parameters.Clear();
                        objCmd1.Parameters.Add(new SqlParameter("@TransActionId", vTansactionId));
                        objCmd1.Parameters.Add(new SqlParameter("@PayeeId", row.PayeeId));
                        objCmd1.Parameters.Add(new SqlParameter("@AccountId", row.AccountId));
                        objCmd1.Parameters.Add(new SqlParameter("@ChildAccountId", row.ChildAccountId));
                        objCmd1.Parameters.Add(new SqlParameter("@TransType", row.TransType == null ? DBNull.Value : row.TransType));
                        objCmd1.Parameters.Add(new SqlParameter("@RefNumber", row.RefNumber == null ? DBNull.Value : row.RefNumber));
                        objCmd1.Parameters.Add(new SqlParameter("@Memo", row.Memo == null ? DBNull.Value : row.Memo));
                        objCmd1.Parameters.Add(new SqlParameter("@TransDate", row.TransDate));
                        objCmd1.Parameters.Add(new SqlParameter("@Credit", row.Credit == null ? DBNull.Value : row.Credit));
                        objCmd1.Parameters.Add(new SqlParameter("@Debit", row.Debit == null ? DBNull.Value : row.Debit));
                        try
                        {
                            objCmd1.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Transaction Failed:" + ex.Message);
                            processingResult.Errors.Add(new ApiProcessingError("There was an error inserting the data", "There was an error inserting the data", ""));
                            objTrans.Rollback();
                            Errors = true;
                            break;
                        }

                    }


                }
                if (!Errors)
                {
                    objTrans.Commit();
                    objConn.Close();

                }
                else
                {
                    objConn.Close();

                }

            }
            if (!Errors)
            {

                return processingResult;
            }
            else
            {
                processingResult.Data = false;
                return processingResult;
            }

          
          
        }
        public async Task<ApiProcessingResult<bool>>? Update(List<TransActionCrudModel> data)
        {
            //need to account for rows taken away in a split transaction
            var processingResult = new ApiProcessingResult<bool>() { Data = true };
            if (string.IsNullOrEmpty(data[0].TransActionId.ToString()) || data[0].TransActionId==0)
            {
                Log.Error("Missing TransActionId for update (id):" + data[0].Id.ToString());
                processingResult.Errors.Add(new ApiProcessingError("There was an error updating the data", "There was an error updating the data", ""));
                return processingResult;
            }
            _client.ClearParameters();
            _client.CommandText(@"Select Id from TransActions Where TransActionId=@TransActionId");
            _client.AddParameter("@TransActionId", data[0].TransActionId);
            var idResult = _client.SelectMany<IID>();
            if (idResult.IsError)
            {
                Log.Error("Failed to get Id list:" + idResult.Errors[0].DeveloperMessage);
                processingResult.Errors.Add(new ApiProcessingError("There was an error updating the data", "There was an error updating the data", ""));
                return processingResult;
            }
            var idList =(List<IID>) idResult.Data;
            foreach(var rec in idList)
            {
                var recid=rec.Id;
                var findResult = data.Where(a=>a.Id==recid).ToList();
                if (findResult.Count == 0)
                {
                    //remove record rec.id in data this was removed by user
                    _client.ClearParameters();
                    _client.CommandText(@"Delete from transactions where Id=@Id");
                    _client.AddParameter("@Id",recid);
                    _client.Delete();
                }
                
            }
            _client.ClearParameters();


            string strConnString = ApplicationConfig.DefaultConnectionString;
            SqlTransaction objTrans = null;
            bool Errors = false;
            using (SqlConnection objConn = new SqlConnection(strConnString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();
                string cmd = @"If Exists (Select Id From Transactions Where Id=@Id)
                                Begin
                                UPDATE TransActions SET PayeeId=@PayeeId,AccountId=@AccountId,TransActionId=@TransActionId,
                                ChildAccountId=@ChildAccountId,TransType=@TransType,RefNumber=@RefNumber,Reconciled=@Reconciled
                                ,Memo=@Memo,TransDate=@TransDate,Credit=@Credit,Debit=@Debit WHERE Id=@Id
                                End
                                Else
                                Begin
                                 Insert INTO TransActions(PayeeId,Accountid,TransActionId,ChildAccountId,TransType,RefNumber,Reconciled,Memo,TransDate,Credit,Debit)
                                Values(@PayeeId,@Accountid,@TransActionId,@ChildAccountId,@TransType,@RefNumber,@Reconciled,@Memo,@TransDate,@Credit,@Debit)
                                End
                                ";

                SqlCommand objCmd1 = new SqlCommand(cmd, objConn);
                objCmd1.Transaction = objTrans;

                foreach (var row in data)
                {
                    {
                        objCmd1.Parameters.Clear();
                        objCmd1.Parameters.Add(new SqlParameter("@Id", row.Id));
                        objCmd1.Parameters.Add(new SqlParameter("@Reconciled", row.Reconciled));
                        objCmd1.Parameters.Add(new SqlParameter("@TransActionId", row.TransActionId));
                        objCmd1.Parameters.Add(new SqlParameter("@PayeeId", row.PayeeId));
                        objCmd1.Parameters.Add(new SqlParameter("@AccountId", row.AccountId));
                        objCmd1.Parameters.Add(new SqlParameter("@ChildAccountId", row.ChildAccountId));
                        objCmd1.Parameters.Add(new SqlParameter("@TransType", row.TransType));
                        objCmd1.Parameters.Add(new SqlParameter("@RefNumber", row.RefNumber));
                        objCmd1.Parameters.Add(new SqlParameter("@Memo", row.Memo == null ? "": row.Memo));
                        objCmd1.Parameters.Add(new SqlParameter("@TransDate", row.TransDate));
                        objCmd1.Parameters.Add(new SqlParameter("@Credit", row.Credit == null ? DBNull.Value : row.Credit));
                        objCmd1.Parameters.Add(new SqlParameter("@Debit", row.Debit == null ? DBNull.Value : row.Debit));
                        try
                        {
                            objCmd1.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Transaction Failed:" + ex.Message);
                            processingResult.Errors.Add(new ApiProcessingError("There was an error updating the data", "There was an error updating the data", ""));
    
                                objTrans.Rollback();
                            Errors = true;
                            break;
                        }

                    }

                }
                if (!Errors)
                {
                    objTrans.Commit();
                    objConn.Close();

                }
                else
                {
                    objConn.Close();

                }

            }
            if (!Errors)
            {

                return processingResult;
            }
            else
            {
                processingResult.Data = false;
                return processingResult;
            }

           

        }
        public async Task<ApiProcessingResult>? DeleteTransAction(int transactionId)
        {
            var processingResult = new ApiProcessingResult();
            _client.ClearParameters();
           
            string cmd = @"Delete From TransActions where TransActionId=@TransActionId";
            _client.CommandText(cmd);
            _client.AddParameter("@TransActionId", transactionId);
            var deleteResult =_client.Delete();
            if (deleteResult.IsError)
            {
                Log.Error("Failed to delete transactions");
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to delete transactions:" + deleteResult.Errors[0].DeveloperMessage, "Failed to delete transactions:" + deleteResult.Errors[0].DeveloperMessage, ""));

                return processingResult;
            }
           
            return processingResult;




        }
        public async Task<ApiProcessingResult<int>>? InsertUpdateReconciliation(ReconciledData model)
        {
            var processingResult = new ApiProcessingResult<int>() ;
          
         
            _client.ClearParameters();
            string insertText= @" Insert Into Reconciledrecords (AccountId,StatementDate,EndingBalance,ReconciledBalance,Reconciled) 
                          Values(@AccountId,@StatementDate,@EndingBalance,@ReconciledBalance,@Reconciled)";
            string updateText = @"Update Reconciledrecords Set StatementDate=@StatementDate,EndingBalance=@EndingBalance,Reconciled=@Reconciled Where Id=@Id";
            if (model.Id==0)
            {
                _client.CommandText(insertText);
                _client.AddParameter("@AccountId", model.AccountId);
                _client.AddParameter("@StatementDate", model.StatementDate);
                _client.AddParameter("@EndingBalance", model.EndingBalance);
                _client.AddParameter("@ReconciledBalance", model.ReconciledBalance);
                _client.AddParameter("@Reconciled", model.Reconciled);
                var dataResult = _client.Insert();
                if (dataResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to create reconciliation:" + dataResult.Errors[0].DeveloperMessage, "Failed to create reconciliation:" + dataResult.Errors[0].DeveloperMessage, ""));
                    return processingResult;
                }
                processingResult.Data = int.Parse(dataResult.Data);

            }
            else
            {
                _client.CommandText(updateText);
                _client.AddParameter("@AccountId", model.AccountId);
                _client.AddParameter("@StatementDate", model.StatementDate);
                _client.AddParameter("@EndingBalance", model.EndingBalance);
                _client.AddParameter("@ReconciledBalance", model.ReconciledBalance);
                _client.AddParameter("@Reconciled", model.Reconciled);
                _client.AddParameter("@Id", model.Id);
                var dataResult = _client.Update();
                if (dataResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to Update reconciliation:" + dataResult.Errors[0].DeveloperMessage, "Failed to Update reconciliation:" + dataResult.Errors[0].DeveloperMessage, ""));
                    return processingResult;
                }
                processingResult.Data = model.Id;

            }
         
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>>? SetReconciled(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };


            string strConnString = ApplicationConfig.DefaultConnectionString;
            SqlTransaction objTrans = null;
            bool Errors = false;
            using (SqlConnection objConn = new SqlConnection(strConnString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();
                string cmd = @"UPDATE TransActions SET Reconciled=1,Clr=1,ReconciledId=@ReconciledId WHERE TransActionId=@TransActionId";

                SqlCommand objCmd1 = new SqlCommand(cmd, objConn);
                objCmd1.Transaction = objTrans;

                foreach (var row in data)
                {
                    {
                        objCmd1.Parameters.Clear();
                        objCmd1.Parameters.Add(new SqlParameter("@TransActionId", row.TransActionId));
                        objCmd1.Parameters.Add(new SqlParameter("@ReconciledId", row.ReconciledId));

                        try
                        {
                            objCmd1.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Transaction Failed:" + ex.Message);
                            processingResult.Errors.Add(new ApiProcessingError("There was an error marking record as cleard", "There was an error marking record as cleard", ""));

                            objTrans.Rollback();
                            Errors = true;
                            break;
                        }

                    }

                }
                if (!Errors)
                {
                    objTrans.Commit();
                    objConn.Close();

                }
                else
                {
                    objConn.Close();

                }

            }
            if (!Errors)
            {

                return processingResult;
            }
            else
            {
                processingResult.Data = false;
                return processingResult;
            }


        }
        public async Task<ApiProcessingResult<bool>>? SetCleared(List<TransActionCrudModel> data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };

            string strConnString = ApplicationConfig.DefaultConnectionString;
            SqlTransaction objTrans = null;
            bool Errors = false;
            using (SqlConnection objConn = new SqlConnection(strConnString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();
                string cmd = @"UPDATE TransActions SET Clr=@Clr WHERE TransActionId=@TransActionId";

                SqlCommand objCmd1 = new SqlCommand(cmd, objConn);
                objCmd1.Transaction = objTrans;

                foreach (var row in data)
                {
                    {
                        objCmd1.Parameters.Clear();
                        objCmd1.Parameters.Add(new SqlParameter("@Clr", row.Clr));
                        objCmd1.Parameters.Add(new SqlParameter("@TransActionId", row.TransActionId));
                        

                        try
                        {
                            objCmd1.ExecuteNonQuery();
                         

                        }
                        catch (Exception ex)
                        {
                            Log.Error("Transaction Failed:" + ex.Message);
                            processingResult.Errors.Add(new ApiProcessingError("There was an error marking record as cleared", "There was an error marking record as cleared", ""));

                            objTrans.Rollback();
                            Errors = true;
                            break;
                        }

                    }

                }
                if (!Errors)
                {
                    objTrans.Commit();
                    objConn.Close();

                }
                else
                {
                    objConn.Close();

                }

            }
            if (!Errors)
            {

                return processingResult;
            }
            else
            {
                processingResult.Data = false;
                return processingResult;
            }


        }

        //Account
        public async Task<ApiProcessingResult<bool>>? SetRegisterColor(int accountId,int color)
        {
            var processingResult = new ApiProcessingResult<bool>();
            _client.ClearParameters();
            string cmd = @"Update Accounts Set RegColor=@RegColor Where Id=@Id";
            _client.AddParameter("@Id", accountId);
            _client.AddParameter("@RegColor", color);
            _client.CommandText(cmd);
            var result = _client.Update();
            if (result.IsError)
            {

                Log.Error("Failed to update Register Color:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to change register color:" + result.Errors[0].DeveloperMessage, "Failed to change register color:" + result.Errors[0].DeveloperMessage, ""));
                return processingResult;
            }
           
            return processingResult;

        }
        public async Task<ApiProcessingResult<EditAccountModel>>? GetAccount(int CurrentId)
        {
            var processingResult=new ApiProcessingResult<EditAccountModel>();
            _client.ClearParameters();
            string cmd = @"Select A.Id,A.ParentAccountId,A.Name,A.Description,A.AccountTypeId,A.AccountNo,A.IsActive,A.AccountType From Accounts A Where A.Id=@Id";
            _client.AddParameter("@Id", CurrentId);
            _client.CommandText(cmd);
            var result = _client.Select<EditAccountModel>();
            if (result.IsError)
            {

                Log.Error("Failed to get account:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to get account:" + result.Errors[0].DeveloperMessage, "Failed to get account:" + result.Errors[0].DeveloperMessage,""));
                return processingResult;
            }
            var data = (EditAccountModel)result.Data;
            processingResult.Data = data;
            return processingResult;

        }
        public async Task<ApiProcessingResult<List<AccountTypeModel>>>? GetLookUpAccountTypes()
        {
            var processingResult = new ApiProcessingResult<List<AccountTypeModel>>();
            //load Accounttypes
            _client.ClearParameters();
            _client.CommandText("Select Name,Id,GroupId From AccountTypes order by GroupId,Name");
            var typeResult = _client.SelectMany<AccountTypeModel>();
            if (typeResult.IsError)
            {
                Log.Error("Failed to retrieve AccountTypes:" + typeResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve AccountTypes:" + typeResult.Errors[0].DeveloperMessage, "Failed to retrieve AccountTypes:" + typeResult.Errors[0].DeveloperMessage, ""));
                return processingResult;
            }
            var data=(List <AccountTypeModel>)typeResult.Data;
            processingResult.Data=data; 
            return processingResult;
        }
        public async Task<ApiProcessingResult<int>> AddAccount(EditAccountModel data)
        {
            var processingResult = new ApiProcessingResult<int>() {  };

            _client.ClearParameters();

            string cmd = @"Insert Into Accounts (ParentAccountId,Name,Description,AccountNo,IsActive,AccountTypeId)
                                                     Values(@ParentAccountId,@Name,@Description,@AccountNo,@IsActive,@AccountTypeId)";
            _client.CommandText(cmd);
            _client.AddParameter("@ParentAccountId", data.ParentAccountId);
            _client.AddParameter("@Name", data.Name);
            _client.AddParameter("@Description", data.Description);
            _client.AddParameter("@AccountNo", data.AccountNo);
            _client.AddParameter("@IsActive", data.IsActive);
            _client.AddParameter("@AccountTypeId", data.AccountTypeId);
            var insertResult = _client.Insert();
            if (insertResult.IsError)
            {
                processingResult.IsError = true;
                Log.Error("Failed to insert new account:" + insertResult.Errors[0].DeveloperMessage );
                
                processingResult.Errors.Add(new ApiProcessingError("Failed to insert new account:" + insertResult.Errors[0].DeveloperMessage,"Failed to insert new account:" + insertResult.Errors[0].DeveloperMessage,""));        
                return processingResult;
            }

            processingResult.Data =int.Parse(insertResult.Data);


            return processingResult;

        }
        public async Task<ApiProcessingResult<Balances>> GetLastStatementBalance(int accountId)
        {
            var processingResult = new ApiProcessingResult<Balances>() ;
            _client.ClearParameters();

            string cmd = @"
            SELECT         
            ReconciledBalance
            ,EndingBalance  
            ,StatementDate,
            (Select StatementDate FROM ReconciledRecords where AccountId=@AccountId And Id=(Select Max(Id) FROM ReconciledRecords where AccountId=@AccountId AND Reconciled=1))AS PreviousStatementDate
            ,AccountId
            ,Reconciled
             ,Id          
            FROM ReconciledRecords Where AccountId=@AccountId And Id=(Select Max(Id) FROM ReconciledRecords where AccountId=@AccountId)";
            _client.CommandText(cmd);
            ;
            _client.AddParameter("@AccountId", accountId);
            var balanceResult = _client.Select<Balances>();
            if (balanceResult.IsError)
            {

                Log.Error("Failed to get reconciled balance:"+ balanceResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to get reconciled balance:" + balanceResult.Errors[0].DeveloperMessage, "Failed to get reconciled balance:" + balanceResult.Errors[0].DeveloperMessage, ""));
                return processingResult;
            }
           
            processingResult.Data =(Balances)balanceResult.Data;
            return processingResult;


        }
        public async Task<ApiProcessingResult<bool>> EditAccount(EditAccountModel data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };
            _client.ClearParameters();

            string cmd = @"Update Accounts Set ParentAccountId=@ParentAccountId,Name=@Name,Description=@Description,
                               AccountNo=@AccountNo,IsActive=@IsActive,AccountTypeId=@AccountTypeId Where Id=@Id";
            _client.CommandText(cmd);
            _client.AddParameter("@ParentAccountId", data.ParentAccountId);
            _client.AddParameter("@Name", data.Name);
            _client.AddParameter("@Description", data.Description);
            _client.AddParameter("@AccountNo", data.AccountNo);
            _client.AddParameter("@IsActive", data.IsActive);
            _client.AddParameter("@AccountTypeId", data.AccountTypeId);
            _client.AddParameter("@Id", data.Id);
            var updateResult = _client.Update();
            if (updateResult.IsError)
            {
            
                Log.Error("Failed to upate AccountId:" + data.Id.ToString() +"||"+updateResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to upate AccountId:" + data.Id.ToString(), "Failed to upate AccountId:" + data.Id.ToString(),""));
                return processingResult;
            }
            return processingResult;



        }
        public async Task<ApiProcessingResult<string>> GetAccountBalance(int accountId)
        {
            var processingResult = new ApiProcessingResult<string>() ;
            _client.ClearParameters();

            string cmd = @"Select SUM(Credit)-Sum(Debit) AS Balance From TransActions Where AccountId = @AccountId";
            _client.CommandText(cmd);
            _client.AddParameter("@AccountId", accountId);
            string retval;
            var result = _client.SelectSingleColumn();
            if (result.IsError)
            {
                Log.Error("Error getting Balance:" + result.Errors[0].DeveloperMessage);
               processingResult.IsError=true;
                processingResult.Errors.Add(new ApiProcessingError("Error getting Balance:" + result.Errors[0].DeveloperMessage, "Error getting Balance:" + result.Errors[0].DeveloperMessage,""));
                return processingResult;
            }
            retval = String.Format("${0:C}", result.Data);
            processingResult.Data = retval;
            return processingResult;

        }
        //Names
        public async Task<ApiProcessingResult<NameModel>>? GetName(int id)
        {
            
            var processingResult = new ApiProcessingResult<NameModel>();
            _client.ClearParameters();
            _client.ClearParameters();
            _client.CommandText(@"SELECT Id
                                  ,FullName
                                  ,FirstName
                                  ,LastName
                                  ,Address
                                  ,Address2
                                  ,City
                                  ,State
                                  ,PostalCode
                                  ,PhoneNumber
                                  ,EmailAddress
                                  ,AccountNo
                                  ,Notes
                                  ,NameType
                                  ,SSN
                                  ,MobilePhone
                                  ,TaxId
                              FROM Names WHERE Id=@Id");
            _client.AddParameter("@Id", id);
            var selectResult = _client.Select<NameModel>();
            if (selectResult.IsError)
            {
                Log.Error("Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage, "Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage,""));
                return processingResult;
            }
            var data = (NameModel)selectResult.Data;
            processingResult.Data = data;

            return processingResult;
        }
        public async Task<ApiProcessingResult<List<NameListModel>>>? GetNameList(string nameType)
        {
            var processingResult=new ApiProcessingResult<List<NameListModel>>();
            _client.ClearParameters();
            _client.CommandText(@"Select Id,FullName,Notes From Names where NameType=@NameType order By FullName");
            _client.AddParameter("@NameType", nameType);
            var selectResult = _client.SelectMany<NameListModel>();
            if (selectResult.IsError)
            {
                Log.Error("Failed to retrieve names:" + selectResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage, "Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage,""));
                return processingResult; ;
            }
            var data = (List<NameListModel>)selectResult.Data;
            processingResult.Data = data;
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>> EditName(NameModel data)
        {
            var processingResult = new ApiProcessingResult<bool>() { Data = true };
            _client.ClearParameters();
            string cmd = @"UPDATE Names
                               SET FullName = @FullName 
                                  ,FirstName = @FirstName
                                  ,LastName = @LastName 
                                  ,Address = @Address 
                                  ,Address2 = @Address2 
                                  ,City = @City 
                                  ,State = @State 
                                  ,PostalCode = @PostalCode 
                                  ,PhoneNumber = @PhoneNumber 
                                  ,EmailAddress = @EmailAddress 
                                  ,AccountNo = @AccountNo 
                                  ,Notes = @Notes 
                                  ,NameType = @NameType 
                                  ,SSN = @SSN 
                                  ,MobilePhone = @MobilePhone
                                  ,TaxId = @TaxId
                            Where Id = @Id";
            _client.CommandText(cmd);
            _client.AddParameter("@Id", data.Id);
            _client.AddParameter("@FullName", data.FullName);
            _client.AddParameter("@FirstName", data.FirstName);
            _client.AddParameter("@LastName", data.LastName);
            _client.AddParameter("@Address", data.Address);
            _client.AddParameter("@Address2", data.Address2);
            _client.AddParameter("@City", data.City);
            _client.AddParameter("@State", data.State);
            _client.AddParameter("@PostalCode", data.PostalCode);
            _client.AddParameter("@PhoneNumber", data.PhoneNumber);
            _client.AddParameter("@MobilePhone", data.MobilePhone);
            _client.AddParameter("@EmailAddress", data.EmailAddress);
            _client.AddParameter("@AccountNo", data.AccountNo);
            _client.AddParameter("@Notes", data.Notes);
            _client.AddParameter("@NameType", data.NameType);
            _client.AddParameter("@SSN", data.SSN);
            _client.AddParameter("@TaxId", data.TaxId);
            var updateResult = _client.Update();
            if (updateResult.IsError)
            {
                Log.Error("Failed to update name:" + updateResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to update name:" + updateResult.Errors[0].DeveloperMessage, "Failed to update name:" + updateResult.Errors[0].DeveloperMessage,""));
               
            }
         

            return processingResult;

        }
        public async Task<ApiProcessingResult<string>> AddName(NameModel data)
        {
            var processingResult = new ApiProcessingResult<string>() ;
            _client.ClearParameters();
            string cmd = @"INSERT INTO Names
                                   (FullName,FirstName,LastName,Address,Address2,City,State
                                   ,PostalCode,PhoneNumber,EmailAddress,AccountNo,Notes,NameType
                                   ,SSN,MobilePhone,TaxId) 
                                Values(@FullName,@FirstName,@LastName,@Address,@Address2,@City
                                   ,@State,@PostalCode,@PhoneNumber,@EmailAddress,@AccountNo
                                   ,@Notes,@NameType,@SSN,@MobilePhone,@TaxId)";
            _client.CommandText(cmd);
      
            _client.AddParameter("@FullName", data.FullName);
            _client.AddParameter("@FirstName", data.FirstName);
            _client.AddParameter("@LastName", data.LastName);
            _client.AddParameter("@Address", data.Address);
            _client.AddParameter("@Address2", data.Address2);
            _client.AddParameter("@City", data.City);
            _client.AddParameter("@State", data.State);
            _client.AddParameter("@PostalCode", data.PostalCode);
            _client.AddParameter("@PhoneNumber", data.PhoneNumber);
            _client.AddParameter("@MobilePhone", data.MobilePhone);
            _client.AddParameter("@EmailAddress", data.EmailAddress);
            _client.AddParameter("@AccountNo", data.AccountNo);
            _client.AddParameter("@Notes", data.Notes);
            _client.AddParameter("@NameType", data.NameType);
            _client.AddParameter("@SSN", data.SSN);
            _client.AddParameter("@TaxId", data.TaxId);
            var insertResult = _client.Insert();
            if (insertResult.IsError)
            {
                Log.Error("Failed to add name:" + insertResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to add name:" + insertResult.Errors[0].DeveloperMessage, "Failed to update name:" + insertResult.Errors[0].DeveloperMessage,""));
               
            }
            processingResult.Data = insertResult.Data;
            return processingResult;

        }
        public async Task<ApiProcessingResult<RuleData>> GetNameAccountRuleData(string Name)
        {
            var processingResult = new ApiProcessingResult<RuleData>();
            _client.ClearParameters();
            _client.CommandText(@"SELECT PayeeId,AccountId,Name,Id
                              FROM ImportRules WHERE Name=@Name");
            _client.AddParameter("@Name", Name);
            var selectResult = _client.Select<RuleData>();
            if (selectResult.IsError)
            {
                Log.Error("Failed to retrieve name:" + selectResult.Errors[0].DeveloperMessage);
              processingResult.IsError= true;
                processingResult.Data= new RuleData() { AccountId = 0, PayeeId = 0 };
                return processingResult;
            }
            RuleData data = (RuleData)selectResult.Data;
            if (data == null)
            {
                processingResult.Data = new RuleData() { AccountId = 0, PayeeId = 0 };
            }
            else
            {
                processingResult.Data=data;

            }
           

            return processingResult;
        }
        public async Task<ApiProcessingResult<string>> Match(STMTTRN importData, RuleData ruleResult)
        {
            var processingResult = new ApiProcessingResult<string>();
            if (ruleResult.PayeeId == 0)
            {
                processingResult.Data = "REV";
                return processingResult;
            }
            _client.ClearParameters();
            string cmd = @"SELECT PayeeId,Debit,Credit,Id
                              FROM TransActions WHERE PayeeId=@PayeeId And ";
             string debitAmount = "Debit=@Amount";
            string creditAmount = "Credit=@Amount";
            if (importData.TRNAMT<0)
            {
                cmd += debitAmount;
               
            }
            else if (importData.TRNAMT > 0)
            {
                cmd+=creditAmount;
            }
            _client.CommandText( cmd);
            _client.AddParameter("@Amount", importData.TRNAMT.normalize());
            _client.AddParameter("@PayeeId", ruleResult.PayeeId);
            var selectResult = _client.Select<TransactionBankFeedModel>();
            if (selectResult.IsError)
            {
                Log.Error("SQL error retrieving transaction:" + selectResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
             
                return processingResult;
            }
             if (selectResult.Data != null )
            {
                processingResult.Data = "AUTO";
            } else if (selectResult.Data == null && ruleResult !=null)
            {
                processingResult.Data = "RULES";
            }
            return processingResult;

        }

        //Import Data
        public async Task<ApiProcessingResult<List<STMTTRN>>> GetImportedData(string bankNo)
        {
            var processingResult = new ApiProcessingResult<List<STMTTRN>>();
            _client.ClearParameters();
            _client.CommandText(@"  Select Id, BankAccountNo ,TRNAMT,TRNTYPE,CHECKNUM,DTPOSTED ,NAME,MEMO,FITID
                                       FROM ImportData
                                   WHERE Imported=0 AND RIGHT(BankAccountNo,4)=@BankAccountNo
                                    Order by DTPOSTED");

            _client.AddParameter("@BankAccountNo", bankNo.Substring(bankNo.Length-4));
            var dataResult = _client.SelectMany<STMTTRN>();
            if (dataResult.IsError)
            {
               Log.Error("Failed to retrieve ImportData:"+dataResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve ImportData:" + dataResult.Errors[0].DeveloperMessage, "Failed to retrieve ImportData:" + dataResult.Errors[0].DeveloperMessage,""));
                return processingResult;
            }
            var data =(List<STMTTRN>) dataResult.Data;
            processingResult.Data = data;
            return processingResult;
        }

        public async Task<ApiProcessingResult<bool>> AddNewRule(TransactionBankFeedModel data)
        {
            var processingResult = new ApiProcessingResult<bool>();
            //Getdata
            _client.ClearParameters();
            _client.CommandText(@"Select FITID,Name,MEMO From ImportData Where FITID=@FITID");
            _client.AddParameter("@FITID", data.FITID);
            var dataResult = _client.Select<ImportToRuleModel>();
            if (dataResult.IsError)
            {
                Log.Error("Failed to retrieve Import Data for rule insertion:" + dataResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;
            }
            var vImportData =(ImportToRuleModel) dataResult.Data;
            //Insert
            _client.ClearParameters();
            _client.CommandText(@"

                                               INSERT INTO ImportRules
                                                (PayeeId,AccountId,Name)
                                                VALUES
                                                (@PayeeId,@AccountId,@Name)");
            _client.AddParameter("@PayeeId", data.PayeeId);
            _client.AddParameter("@AccountId", data.ChildAccountId);
            _client.AddParameter("@Name",vImportData.Name);
            var insertResult = _client.Insert();
            if (insertResult.IsError)
            {
                Log.Error("Failed to update Import Data:" + insertResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                return processingResult;    
            }
            return processingResult;

        }
        public async Task<ApiProcessingResult<bool>> SetImported(List<string> fTTIDs)
        {
            var processingResult = new ApiProcessingResult<bool>();
            _client.CommandText(@"UPDATE ImportData Set Imported=1 WHERE FITID=@FITID ");
            foreach (var id in fTTIDs)
            {
                _client.ClearParameters();
                _client.AddParameter("@FITID", id);
                var selectResult = _client.Update();
                if (selectResult.IsError)
                {
                    Log.Error("Failed to update Import Data:" + selectResult.Errors[0].DeveloperMessage);
                    processingResult.IsError = true;
                 
                }

            }
                     
            return processingResult;
        }
        public async Task<ApiProcessingResult<bool>> InsertImportedParentData(List<STMTTRN> _importedData)
        {
            var processingResult = new ApiProcessingResult<bool>();
            _client.ClearParameters();
            _client.CommandText(@"
                            BEGIN
                                     IF NOT EXISTS ( SELECT FITID FROM ImportData 
                                       WHERE FITID =@FITID )
                            BEGIN
                                    INSERT INTO ImportData
                                       (BankAccountNo
                                       ,TRNAMT
                                       ,TRNTYPE
                                       ,CHECKNUM
                                       ,DTPOSTED
                                        ,NAME
                                       ,MEMO
                                       ,FITID
                                        ,DateImported)
                            Values (@BankAcountNo
                                       ,@TRNAMT
                                       ,@TRNTYPE
                                       ,@CHECKNUM
                                       ,@DTPOSTED
                                        ,@NAME
                                       ,@MEMO
                                       ,@FITID
                                        ,GETDATE())
                            END END");
            foreach (var l in _importedData)
            {
                
                //if (l.DTPOSTED < DateTime.Now.AddDays(-90))
                //{
                    //break;
                //}
                _client.ClearParameters();
             
                _client.AddParameter("@BankAcountNo", l.BankAccountNo);
                _client.AddParameter("@TRNAMT", l.TRNAMT);
                _client.AddParameter("@TRNTYPE", l.TRNTYPE);
                _client.AddParameter("@CHECKNUM", l.CHECKNUM);
                _client.AddParameter("@DTPOSTED", l.DTPOSTED);
                _client.AddParameter("@NAME", l.NAME);
                _client.AddParameter("@MEMO", l.MEMO);
                _client.AddParameter("@FITID", l.FITID);
                var result = _client.Insert();
                if (result.IsError)
                {
                    Log.Error("Error Inserting Imported Data:"+result.Errors[0].DeveloperMessage);
                    //CMBox.Error("Failed to import data. Check logs for reason.");
                    return processingResult;
                }
            }
            return processingResult;
        }
        public async Task<ApiProcessingResult<int>> GetBankId(string bankAccountNo)
        {
            var processingResult = new ApiProcessingResult<int>();
            _client.ClearParameters();
            _client.CommandText(@"  Select Id
                                       FROM Accounts
                                   WHERE RIGHT(AccountNo,4)=@BankAccountNo");

            _client.AddParameter("@BankAccountNo", bankAccountNo.Substring(bankAccountNo.Length-4));
            var dataResult = _client.SelectSingleColumn();
            if (dataResult.IsError  )
            {
               Log.Error("Failed to retrieve ImportData:" + dataResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve ImportData:" + dataResult.Errors[0].DeveloperMessage, "Failed to retrieve ImportData:" + dataResult.Errors[0].DeveloperMessage, ""));
                return processingResult;
            }else if (dataResult.Data == ""|| dataResult.Data == "0")
            {
                Log.Error("Failed to locate bank account");
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to locate bank account" , "Failed to locate bank account" , ""));
                return processingResult;

            }
            processingResult.Data = int.Parse(dataResult.Data);
            return processingResult;

        }
        public int? NewTransActionId()
        {
            int retTransactionId = 0;
            int vTransactionId = 0;
            _client.ClearParameters();
            _client.CommandText("Select Max(TransActionId) as TransActionId From TransActions");
            var result = _client.SelectSingleColumn();
            if (result.IsError)
            {
                Log.Error("Failed to get new transaction Id:" + result.Errors[0].DeveloperMessage);
           
                return null;
            }
            int.TryParse(result.Data, out retTransactionId);
   
          if(retTransactionId==0)
            {
                vTransactionId = 1000;
            }
            else
            {
                 vTransactionId =  retTransactionId + 1;
            }

            return vTransactionId;

        }
       

    }
}
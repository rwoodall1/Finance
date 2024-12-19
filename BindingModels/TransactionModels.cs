namespace BindingModels
{
    public class ReconcileModel
    {
        public int AccountId { get; set; }
        public DateTime StatementDate { get; set; }
        public decimal ReconciledBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public int Id { get; set; }
    }
    public class Balances
    {
        public decimal ReconciledBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime PreviousStatementDate { get; set; }
        public bool Reconciled { get; set; }
        public int AccountId { get; set; }
        public int Id { get; set; }
    }
    public class ReconciledData {
        public DateTime StatementDate{get;set;}
        public decimal SeviceCharge { get; set; }
        public decimal InterestCharge { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal ReconciledBalance{get;set;}
        public int AccountId { get; set; }
        public bool Reconciled { get; set; }
        public int Id { get; set; }


    }
    public class ImportToRuleModel
    {
        public string FITID {get; set;}
        public string Name {get; set;}
        public string MEMO {get; set;}
    }
   public class RuleData
    {   public string Name { get;set; }
        public int PayeeId { get; set; }
        public int AccountId { get; set; }
        public int Id { get; set; } 
    }
    public class TransactionBankFeedModel
    {
        public int Id { get; set; }
     
        public string AccountName { get; set; }
        public string DownloadName { get; set; }
        public int TransActionId { get; set; }
        public int PayeeId { get; set; }   
       
        public int AccountId { get; set; }

        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int ChildAccountId { get; set; }

        public string TransType { get; set; }
        public string RefNumber { get; set; }

        public string Memo { get; set; }

        public bool? Reconciled { get; set; }

        public DateTime TransDate { get; set; }


       

        public decimal? Balance { get; set; }
        public string Status { get; set; }
        public bool Import { get; set; }
        public string FITID { get; set; }//import data id

    }
    //public class TransactionModel
    //{
    //    public int Id { get; set; }
    //    public int PayeeId { get; set; }
    //    public string Payee { get; set; }
    //    public int AccountId { get; set; }
    //    public string RefNumber { get; set; }
    //    public string Memo { get; set; }
    //    public bool Reconciled { get; set; }
    //    public DateTime TransDate { get; set; }
    //    public decimal? Credit { get; set; }
    //    public decimal? Balance { get; set; }

    //}
    public class BankRegisterModel
    {
        public int Id { get; set; }
        public int TransActionId { get; set; }
        public int PayeeId { get; set; }
        public int AccountId { get; set; }
        public int ChildAccountId { get; set; }
        public string TransType { get; set; }
        public string RefNumber { get; set; }
        public string Memo { get; set; }
        public bool Clr { get; set; }
        public DateTime TransDate { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int AccountTypeId { get; set; }


    }
    public class UnReconciledTransActions
    {
        public int ChildAccountId { get; set; }
        public int TransActionId { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string TransType { get; set; }
        public string RefNumber { get; set; }
        public bool? Reconciled { get; set; }
        public DateTime TransDate { get; set; }
    }
    public class TransActionCrudModel : IDirty
    {
        public TransActionCrudModel()
        {

        }
        private decimal? _debit;
        private bool dirty_;
        private int _payeeId;
        private int _accountId;
        private int _childAccountId;
        private string _refNumber;
        private string _memo;
        private bool? _reconciled;
        private DateTime _transDate;
        private decimal? _credit;
        private bool? _clr;
        public int Id { get; set; }
        
        public string AccountName { get; set; }
        public string ChildAccountName { get; set; }
        public int TransActionId { get; set; }
        public int ReconciledId { get; set; }
        public int PayeeId
        {
            get { return _payeeId; }
            set { SetDirty(_payeeId); _payeeId = value; }
        }
        public string PayeeName { get; set; }
       
        public int AccountId
        {
            get { return _accountId; }

            set { SetDirty(_accountId); _accountId = value; }
            
        }
        public decimal? Debit
        {
            get { return _debit; }
            set { SetDirty(_debit); _debit = value; }
        }
        public int ChildAccountId
        {
            get { return _childAccountId; }
            set
            {
                
                SetDirty(_childAccountId); 

                _childAccountId = value;
            }
           
        }
        public string TransType { get; set; }
        public string RefNumber
        {
            get { return _refNumber; }
            set { SetDirty(_refNumber); _refNumber = value; }
        }
        public string Memo
        {
            get { return _memo; }
            set { SetDirty(_memo); _memo = value; }
        }
        public bool? Reconciled
        {
            get { return _reconciled; }
            set { SetDirty(_reconciled); _reconciled = value; }
        }
        public bool? Clr
        {
            get { return _clr; }
            set { SetDirty(_clr); _clr = value; }

        }
        public DateTime TransDate
        {
            get { return _transDate; }
            set { SetDirty(_transDate); _transDate = value; }
        }

        public decimal? Credit
        {
            get { return _credit; }
            set { SetDirty(_credit); _credit = value; }
        }
        public decimal? Balance { get; set; }
        public bool IsDirty { 
            get { return dirty_; }
            set { dirty_ = value; }
        }
        private void SetDirty(object val)
        {
            if (val != null)
            {
                dirty_ = true;
            }
        }
        private void SetDirty(DateTime val)
        {
            if (val.Year > 1800)
            {
                dirty_ = true;
            }
        }
        private void SetDirty(decimal? val)
        {
            if (val != null)
            {
                dirty_ = true;
            }
        }
        private void SetDirty(int val)
        {
            if (val != 0)
            {
                dirty_ = true;
            }
        }
        private void SetDirty(bool? val)
        {
            if (val != null)
            {
                dirty_ = true;
            }
        }
       

    }
    public class BankImportData
    {
        public TransActionCrudModel ParentData { get; set; }
        public TransActionCrudModel ChildData { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public class AccountDropDown
    {
        public string? AccountType { get; set; }
        public List<AccountNameList>Accounts{ get; set; }
       

      
    }
    public class AccountNameList
    { 
        public string Name { get; set; }
         public int Id { get; set; }
    }
    public class SubAccount
    {

    }
    public class AccountNode
    {
        public AccountModel Data { get; set; }
        public List<AccountNode> Children { get; set; }
    }
    public class AccountTypeModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Abbrev { get; set; }


    }
    public class AccountModel
    {
        public int Id { get; set; }
        public int? ParentAccountId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? AccountNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubAccount { get; set; }
        public string? AccountType { get; set; }
        public int AccountTypeId { get; set; }
        public decimal? AccountBalance { get; set; }
        public int RegColor { get;set; }
        public List<AccountModel>? SubAccounts { get; set; }
    }
    public class EditAccountModel
    {
        public int Id { get; set; }
        public int? ParentAccountId { get; set; }
        public string Name { get; set; }
            public string? Description { get; set; }
        public string? AccountNo { get; set; }
        public bool IsActive { get; set; }
        public string? AccountType { get; set; }
        public int AccountTypeId { get; set; }
        public decimal? AccountBalance { get; set; }

    }
    public class AccountLkpModel
    {
        public string Name { get; set; }
        public int ParentAccountId { get; set; }
        public string? AccountType { get; set; }
        public int RegColor { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }


    }

}

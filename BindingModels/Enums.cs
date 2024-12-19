using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public enum UnReconciled
    {
      No,
      Yes
    }
    public enum DataAction
    {
        Modify,
        Add
    }
    public enum AccountLkpType
    {
        All,
        TypeId,
        TypeIdLessThan,
        TypeName

    }
    public enum SaveType
    {
        Add,
        Modify
    }
    public enum TransActionType
    {
        Debit,
        Credit,
        UnKnown
    }
   public enum AccountType
    {
        Bank,
        CreditCard,
        AccountReceivable,
        OtherCurrentAsset,
        FixedAsset,
        OtherAsset,
        OtherCurrentLiablility,
        LongTermLiablility,
        Equity,
        Income,
        OtherIncome,
        Expense,
        CostOfGoodsSold

    }
}

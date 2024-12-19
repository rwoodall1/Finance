using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BindingModels;
namespace Utilities
{
   public static class ExtensionMethods
    {
        public static decimal normalize(this decimal val)
        {
           
                val =Math.Abs(val);
            
            return val;
        }
        public static string ToStringCurrency(this decimal? val)
        {
           
            if(val == null) { val = 0; }

           decimal val1=(decimal)val;
          string  retval =val1.ToString("C2");
          
           return retval;
        }
        public static string ToStringCurrency(this decimal val)
        {

         

            decimal val1 = (decimal)val;
            string retval = val1.ToString("C2");

            return retval;
        }
        public static List<string> toImportIds(this List<TransactionBankFeedModel> data)
        {
            //only return rows marked for import
            var returnData=new List<string>();
            foreach (var item in data)
            {
                if (item.Import==true)
                {
                    returnData.Add(item.FITID);
                }
            }
            return returnData;
        }
        public static List<TransActionCrudModel> toTransActionCrudModel(this List<TransactionBankFeedModel> model)
        {
            List<TransActionCrudModel> _list = new List<TransActionCrudModel>();

            foreach (var item in model)
            {
                var newItem = new TransActionCrudModel()
                {
                    Id = item.Id,
                    AccountName = item.AccountName,
                    TransActionId = item.TransActionId,
                    PayeeId = item.PayeeId,
                    AccountId = item.AccountId,
                    Debit = item.Debit,
                    Credit = item.Credit,
                    ChildAccountId = item.ChildAccountId,
                    TransType = item.TransType,
                    RefNumber = item.RefNumber,
                    Memo = item.Memo,
                    Reconciled = item.Reconciled,
                    TransDate = item.TransDate,


                };
               
                _list.Add(newItem);
            }
            return _list;
        }
        public static TransActionCrudModel toTransActionCrudModel(this TransactionBankFeedModel model)
        {

            var newItem = new TransActionCrudModel()
            {
                Id = model.Id,
                AccountName = model.AccountName,
                TransActionId = model.TransActionId,
                PayeeId = model.PayeeId,
                AccountId = model.AccountId,
                Debit = model.Debit,
                Credit = model.Credit,
                ChildAccountId = model.ChildAccountId,
                TransType = model.TransType,
                RefNumber = model.RefNumber,
                Memo = model.Memo,
                Reconciled = model.Reconciled,
                TransDate = model.TransDate,

            };

            return newItem;
        }
        public static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                    ? value
                    : value.Substring(0, maxLength)
                    );
        }
        public static string OnlyDigits(this string str)
        {
            return new string(str?.Where(c => char.IsDigit(c)).ToArray());
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using BindingModels;
namespace DataService
{
    public interface ISysData
    {
        public Task<ApiProcessingResult<List<NameLkpModel>>>? GetNames();
        public Task<ApiProcessingResult<List<AccountModel>>>? GetSysAccounts();

       public Task<ApiProcessingResult<List<AccountLkpModel>>>? GetSysLkpAccounts(AccountLkpType type, int typeId = 0, string typename = "");
       
    }
}

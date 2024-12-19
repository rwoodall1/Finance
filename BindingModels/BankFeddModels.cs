using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public class BankUploadFeedModel
    {
        public IFormFile File { get; set; }
        public string AccountId { get; set; }
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ApiProcessingError {
        public string DeveloperMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }

        public ApiProcessingError(string developerMessage, string errorMessage, string errorCode) {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            DeveloperMessage = developerMessage;
        }
    }
}

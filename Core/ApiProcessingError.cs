namespace Core
{
   
        public class ApiProcessingError
        {
            public string DeveloperMessage { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorCode { get; set; }
        public string ErrorCod1e { get; set; }
        public ApiProcessingError(string developerMessage, string errorMessage, string errorCode)
            {
                ErrorCode = errorCode;
                ErrorMessage = errorMessage;
                DeveloperMessage = developerMessage;
            }
        }
    }


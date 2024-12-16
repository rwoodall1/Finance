using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ApiProcessingResult
    {
        private bool _IsError = false;
        private List<ApiProcessingError> _Errors = new List<ApiProcessingError>();
    
        public bool IsError {
            get {
                IsSuccessful = !_IsError;
                return _IsError;
            }
            set {
                _IsError = value;
                IsSuccessful = !_IsError;
            }
        }

        public List<ApiProcessingError> Errors {
            get { 
                try {
                    if (_Errors.Count > 0) {
                        Error = new ProcessingError(_Errors[0].ErrorMessage, _Errors[0].ErrorMessage, false, false);
                    }
                   
                } catch {
                }
                
                return _Errors;
            }
            set {
                _Errors = value;
                Error = new ProcessingError(_Errors[0].ErrorMessage, _Errors[0].ErrorMessage, false, false);
            }
        }

        public bool IsSuccessful { get; set; }
        public ProcessingError Error { get; set; }
    }

    public class ApiProcessingResult<TReturnedData> : ApiProcessingResult
    {
        public TReturnedData Data { get; set; }
    }
}

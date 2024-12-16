using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public ValidationResult()
        {
            Errors = new List<string>();
        }

        public virtual ApiProcessingError ToProcessingError(string userMessage)
        {
            var errorsBuilder = new StringBuilder();
            Errors.ForEach(error => errorsBuilder.AppendLine(error));

            return new ApiProcessingError(userMessage, errorsBuilder.ToString(), "");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DataAccessResult
    {
        public bool IsSuccessful { get; set; }

        public ApiProcessingError Error { get; set; }

        public virtual ApiProcessingResult ToServiceProcessingResult(ApiProcessingError processingError)
        {
            var processingResult = new ApiProcessingResult {
                IsError = !IsSuccessful
            };
            if (!IsSuccessful)
            {
                processingResult.Errors.Add(processingError);
            }
            return processingResult;
        }
    }

    public class DataAccessResult<TReturnedData> : DataAccessResult
    {
        public TReturnedData Data { get; set; }

        public ApiProcessingResult<TReturnedData> ToServiceProcessingResult(ApiProcessingError processingError)
        {
            var processingResult = new ApiProcessingResult<TReturnedData>
            {
                IsError = !IsSuccessful,
                Data = Data
            };
            if (!IsSuccessful)
            {
                processingResult.Errors.Add(processingError);
            }

            return processingResult;
        }
    }

    public class PagedDataAccessResult<TReturnedData> : DataAccessResult<TReturnedData>
    {
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int Count { get; set; }

        public int CurrentPage { get; set; }
    }
}

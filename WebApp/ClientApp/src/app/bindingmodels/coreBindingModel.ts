  export class ApiProcessingError {
    errorMessage: string;
    developerMessage: string;
    errorCode: string;
    constructor(_errorMessage: string, _developerMessage: string, _errorCode: string) {
        this.errorMessage = _errorMessage;
      this.developerMessage = _developerMessage;
      this.errorCode = _errorCode;

    }
    
  }


  export class ApiProcessingResult<T> {
    [x: string]: any;
    data: T;
    isError: boolean = false;
    errors?: ApiProcessingError[] = [];
  }


using System;
using System.Collections.Generic;
using System.Text;

namespace CarParkRateCommon
{
    public class Response
    {
        public bool IsValidOrSuccess { get; set; }
        public string Message { get; set; }
        public bool DisplayMessage { get; set; }

        public Response(bool isValidOrSuccess, string message, bool displayMessage = false)
        {
            IsValidOrSuccess = isValidOrSuccess;
            Message = message;
            if (!isValidOrSuccess || displayMessage)
            {
                DisplayMessage = true;
            }

        }
    }

    public class ResponseWithData<T> : Response
    {
        public T Data { get; set; }

        public ResponseWithData(bool isValidOrSuccess, string message, T data) : base(isValidOrSuccess, message)
        {
            Data = data;
        }
    }
}

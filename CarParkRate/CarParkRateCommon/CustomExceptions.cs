using System;
using System.Collections.Generic;
using System.Text;

namespace CarParkRateCommon
{
    public class InvalidDate : Exception
    {
        public InvalidDate() : base(ErrorMessages.INVALID_ENTRY)
        {
        }
    }
}

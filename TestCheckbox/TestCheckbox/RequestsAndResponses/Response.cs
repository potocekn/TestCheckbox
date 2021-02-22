using System;
using System.Collections.Generic;
using System.Text;

namespace TestCheckbox.RequestsAndResponses
{
    public abstract class Response
    {
        public ResponseStatus Status { get; protected set; }
        public RequestType Type { get; protected set; }
    }
}

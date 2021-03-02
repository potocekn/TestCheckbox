using System;
using System.Collections.Generic;
using System.Text;

namespace AppBaseNamespace.RequestsAndResponses
{
    class ResponseAllAvailableLanguages : Response
    {
        public List<string> Languages { get; set; }


        public ResponseAllAvailableLanguages(ResponseStatus status, RequestType type, List<string> languages)
        {
            Status = status;
            Type = RequestType.ALL_AVAILABLE_LANGUAGES;
            Languages = languages;
        }
    }
}

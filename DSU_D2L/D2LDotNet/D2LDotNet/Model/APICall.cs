using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2LDotNet.Model
{
    public class APICall
    {
        public APICall(HttpMethod method, string path)
        {
            this.method = Utilities.ConvertHttpMethod(method);
            this.path = path;
        }

        public string method;
        public string path;
    }
}

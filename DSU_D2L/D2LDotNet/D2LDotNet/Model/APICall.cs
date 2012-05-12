using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2LDotNet.Model
{
    public class APICall
    {
        public APICall(string method, string path)
        {
            this.method = method.ToString();
            this.path = path;
        }

        public string method;
        public string path;
    }
}

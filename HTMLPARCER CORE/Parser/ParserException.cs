using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;
namespace HTMLPARCER_CORE.Parse
{

    [Serializable]
    public class ParserException : System.Net.WebException
    {
        public ParserException() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, Exception inner) : base(message, inner) { }
        protected ParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
}


using System;

namespace ObjectsLibrary.Parser
{
    [Serializable]
    public class ParserException : System.Net.WebException
    {
        public string WebSite { get; }
        public override string Message { get; }
        public ParserException()
        {
            //
        }

        public ParserException(string message) : base()
        {
            Message = message;
        }
        public ParserException(string message, string webSite) : base(message)
        {
            WebSite = webSite;
        }

        public ParserException(string message, Exception inner) : base(message, inner)
        {
            //
        }

        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            //
        }
    }
}
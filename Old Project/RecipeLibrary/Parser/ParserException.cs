using System;

namespace ObjectsLibrary.Parser
{
    [Serializable]
    public class ParserException : System.Net.WebException
    {
        public ParserException()
        {
            //
        }
        
        public ParserException(string message) : base(message)
        {
        }

        public ParserException(string message, Exception inner) : base(message, inner)
        {
            //
        }

        protected ParserException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            //
        }
    }
}
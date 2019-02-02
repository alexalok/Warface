using System;

namespace Warface.XMPP
{
    public class IqAwaitingException : Exception
    {
        public string IQ { get; }

        public IqAwaitingException(string iq, Exception innerException) : base("Awaiting operation failed for IQ with ID: " + iq, innerException)
        {
            IQ             = iq;
        }
    }
}
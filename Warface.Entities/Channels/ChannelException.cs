using System;

namespace Warface.Entities.Channels
{
    public class ChannelException : Exception
    {
        public ChannelException(string message) : base(message)
        {
        }
    }
}
namespace Warface.XMPP
{
    public struct IQError
    {
        public int? Code       { get; }
        public int? CustomCode { get; }

        public IQError(int? code, int? customCode)
        {
            Code       = code;
            CustomCode = customCode;
        }
    }
}
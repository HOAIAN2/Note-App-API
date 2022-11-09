namespace TokenModel
{
    public class Token
    {
        public string accessToken { get; private set; }
        public string refreshToken { get; private set; }
        public Token(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
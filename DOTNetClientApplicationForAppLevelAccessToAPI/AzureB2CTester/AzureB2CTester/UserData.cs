namespace AzureB2CTester
{
    public class UserData
    {
        public String username { get; set; } = "siddhant602@gmail.com";
        public String password { get; set; } = "Siddhant@602";
        public String grant_type { get; set; } = "password";
        public String scope { get; set; } = "https://siddhantazb2c.onmicrosoft.com/secureid/api/secureid.read";
        public String client_id { get; set; } = "cd25429d-94b2-4fa5-8c6e-9c4aa8df40f8";
        public String response_type { get; set; } = "token";

    }

    public class TokenDetails
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public String expires_in { get; set; }

    }
}

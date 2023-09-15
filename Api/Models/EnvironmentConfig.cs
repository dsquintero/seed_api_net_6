namespace Api.Models
{
    public class EnvironmentConfig
    {
        public string MSSQL_USER { get; set; }
        public string MSSQL_PASS { get; set; }
        public string MSSQL_DB { get; set; }
        public string MSSQL_HOST { get; set; }
        public string MSSQL_CONNECTION_STRING { get; set; }
        public string JWT_AccessTokenSecret { get; set; }
        public double JWT_AccessTokenExpirationMinutes { get; set; }
        public string JWT_RefreshTokenSecret { get; set; }
        public double JWT_RefreshTokenExpirationMinutes { get; set; }
        public string JWT_Issuer { get; set; }
        public string JWT_Audience { get; set; }
        public string X_Api_Key { get; set; }
    }
}

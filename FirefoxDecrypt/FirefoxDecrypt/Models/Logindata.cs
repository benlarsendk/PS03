namespace FirefoxDecrypt.Models
{

    public class FfLoginDataRoot
    {
        public int nextId { get; set; }
        public FFLogin[] logins { get; set; }
        public object[] disabledHosts { get; set; }
        public int version { get; set; }
    }

    public class FFLogin
    {
        public int id { get; set; }
        public string hostname { get; set; }
        public string httpRealm { get; set; }
        public string formSubmitURL { get; set; }
        public string usernameField { get; set; }
        public string passwordField { get; set; }
        public string encryptedUsername { get; set; }
        public string encryptedPassword { get; set; }
        public string guid { get; set; }
        public int encType { get; set; }
        public long timeCreated { get; set; }
        public long timeLastUsed { get; set; }
        public long timePasswordChanged { get; set; }
        public int timesUsed { get; set; }
    }

    public class FFData
    {
        public string Host;
        public string Username;
        public string Password;
    }


}
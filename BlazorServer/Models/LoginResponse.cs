namespace BlazorServer.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public PersonalDataLogin PersonalDataLogin { get; set; }
    }

    public class PersonalDataLogin
    {
        public string Userid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string StatoReg { get; set; }
    }
}

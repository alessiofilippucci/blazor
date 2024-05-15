namespace BlazorServer.Models
{
    public partial class UserStatus
    {
        public PersonalData PersonalData { get; set; }
        public Subscriptions Subscriptions { get; set; }
    }

    public partial class PersonalData
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        public string Userid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Ragsoc { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Factory { get; set; }
        public string AddressFlag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Cap { get; set; }
        public string Province { get; set; }
        public string Nation { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string CodFisc { get; set; }
        public string Title { get; set; }
        public string Profession { get; set; }
        public string Sector { get; set; }
        public string BirthDate { get; set; }
        public string Sex { get; set; }
        public string BirthPlace { get; set; }
        public Flag FlagPriv { get; set; }
        public Flag FlagDiscl { get; set; }
        public FlagGdpr FlagGdpr { get; set; }
        public Flag FlagNp0 { get; set; }
        public Flag FlagNp1 { get; set; }
        public Flag FlagNp2 { get; set; }
        public string UltimaLogin { get; set; }
        public Flag FlagNp3 { get; set; }
        public Flag FlagNp4 { get; set; }
        public string Group { get; set; }
        public string CodEnte { get; set; }
        public string Stato { get; set; }
        public string StatoReg { get; set; }
        public string SitoReg { get; set; }
        public string FlagEmail { get; set; }
        public string Registrazione { get; set; }
        public DateTime? RegistrazioneCompleta { get; set; }
        public string Update { get; set; }
        public string FlagCp { get; set; }
        public Flag FlagPrivateInvestor { get; set; }
        public string ExpiryDateSubMf { get; set; }
    }

    public partial class Flag
    {
        public DateTime Date { get; set; }
        public string Value { get; set; }
    }

    public partial class FlagGdpr
    {
        public DateTime? Date { get; set; }
        public string Value { get; set; }
    }

    public partial class Subscriptions
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        public List<Subscription> Subscription { get; set; }
    }

    public partial class Subscription
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}

namespace BlazorServer.Models
{
    public class News
    {
        public int Pos { get; set; }
        public string IdNews { get; set; } = null!;
        public string Titolo { get; set; } = null!;
        public string DataPubblicazione { get; set; } = null!;
        public string TipiCod { get; set; } = null!;
        public string Autore { get; set; } = null!;
        public string IconaCategoria { get; set; } = null!;
        public bool LinkTargetBlank { get; set; }
        public string Login { get; set; } = null!;
        public string Pagamento { get; set; } = null!;
        public string PathDettaglio { get; set; } = null!;
        public string PathDettaglioRelative { get; set; } = null!;
        public string PathImg { get; set; } = null!;
        public string PathImgPortrait { get; set; } = null!;
        public string PathVideo { get; set; } = null!;
        public string Sommario { get; set; } = null!;
        public string SommarioHtml { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public string TipoDesc { get; set; } = null!;
        public string TipoPath { get; set; } = null!;
        public string TitoloHp { get; set; } = null!;
        public string TitoloHtml { get; set; } = null!;
    }
}

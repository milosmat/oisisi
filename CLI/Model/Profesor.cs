/*Profesor (#profesor)
Prezime
Ime
Datum rođenja
Adresa stanovanja
Kontakt telefon
E-mail adresa
Broj lične karte
Zvanje
Godine staža
Spisak predmeta na kojima je profesor
*/

public class Profesor
{
    public string Prezime {get; set;}
    public string Ime {get; set;}
    public DateTime DatumRodjenja {get; set;}
    public string AdresaStanovanja {get; set;}
    public string KontaktTelefon {get; set;}
    public string EmailAdresa {get; set;}
    public string BrojLicneKarte {get; set;}
    public string Zvanje {get; set;}
    public int GodineStaza {get; set;}
    public List<string> SpisakPredmeta {get; set;}

    public Profesor(string prezime, string ime, DateTime datum, string adresa, string telefon, string email, string licna, string zvanje, int staz)
    {
        Prezime = prezime;
        Ime = ime;
        DatumRodjenja = datum;
        AdresaStanovanja = adresa;
        KontaktTelefon = telefon;
        EmailAdresa = email;
        BrojLicneKarte = licna;
        Zvanje = zvanje;
        GodineStaza = staz;
        SpisakPredmeta = new List<string>();
    }
}

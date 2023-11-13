
public enum StatusEnum {Budzet, Samofinansiranje} 
public class Student
{
    public string Prezime {get; set;}
    public string Ime {get; set;}
    public DateTime DatumRodjenja {get; set;}
    public string AdresaStanovanja {get; set;}
    public string KontaktTelefon {get; set;}
    public string EmailAdresa {get; set;}
    public string BrojIndeksa{get; set;}
    public int TrenutnaGodinaStudija {get; set;}
    public StatusEnum Status {get; set;}
    public double ProsecnaOcena {get; set;}
    public List<int> SpisakPolozenihIspita {get; set;}
    public List<string> SpisakNepolozenihPredmeta {get; set;}

    public Student(string prezime, string ime, DateTime datum, string adresa, string telefon, string email, string brIndeksa, int trGodina, StatusEnum status, double prosek)
    {
        Prezime = prezime;
        Ime = ime;
        DatumRodjenja = datum;
        AdresaStanovanja = adresa;
        KontaktTelefon = telefon;
        EmailAdresa = email;
        BrojIndeksa = brIndeksa;
        TrenutnaGodinaStudija = trGodina;
        Status = status;
        ProsecnaOcena = prosek;
        SpisakPolozenihIspita = new List<int>();
        SpisakNepolozenihPredmeta = new List<string>();
    }
}
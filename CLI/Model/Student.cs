using CLI.DAO;
using StudentskaSluzba.Serialization;
namespace StudentskaSluzba.Model;

public enum StatusEnum {Budzet, Samofinansiranje} 
public class Student : ISerializable
{
    public int Id { get; set; } 
    public string Prezime {get; set;}
    public string Ime {get; set;}
    public DateTime DatumRodjenja {get; set;}
    public Adresa AdresaStanovanja {get; set;}
    public string KontaktTelefon {get; set;}
    public string EmailAdresa {get; set;}
    public string BrojIndeksa{get; set;}
    public int TrenutnaGodinaStudija {get; set;}
    public StatusEnum Status {get; set;}
    public double ProsecnaOcena {get; set;}
    public List<int> SpisakPolozenihIspita {get; set;}
    public List<string> SpisakNepolozenihPredmeta {get; set;}

    public Student()
    {
    }
    public Student(int id, string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, string brIndeksa, int trGodina, StatusEnum status, double prosek)
    {
        Id = id;
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

    public Student(string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, string brIndeksa, int trGodina, StatusEnum status, double prosek)
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

    public override string ToString()
    {
        return $"ID: {Id,5} | Prezime: {Prezime,-20} | Ime: {Ime,-20} | Godina: {TrenutnaGodinaStudija,3} | Status: {Status} | Prosek: {ProsecnaOcena,5:F2} |";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Prezime,
            Ime,
            DatumRodjenja.ToString("yyyy-MM-dd"),
            AdresaStanovanja.ToString(),
            KontaktTelefon,
            EmailAdresa,
            BrojIndeksa,
            TrenutnaGodinaStudija.ToString(),
            Status.ToString(),
            ProsecnaOcena.ToString("F2"),
            string.Join(",", SpisakPolozenihIspita),
            string.Join(",", SpisakNepolozenihPredmeta)
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Prezime = values[1];
        Ime = values[2];
        DatumRodjenja = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);
        AdresaStanovanja = new Adresa
        {
            Ulica = values[4],
            Broj = values[5],
            Grad = values[6],
            Drzava = values[7]
        };
        KontaktTelefon = values[8];
        EmailAdresa = values[9];
        BrojIndeksa = values[10];
        TrenutnaGodinaStudija = int.Parse(values[11]);
        Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), values[12]);
        ProsecnaOcena = double.Parse(values[13], null);

        SpisakPolozenihIspita = values[14].Split(',').Select(int.Parse).ToList();
        SpisakNepolozenihPredmeta = values[15].Split(',').ToList();
    }
}
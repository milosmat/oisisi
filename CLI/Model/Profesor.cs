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

namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
using System.Text;

public class Profesor : ISerializable
{
    public int Id { get; set; }
    public string Prezime { get; set; }
    public string Ime { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public Adresa AdresaStanovanja { get; set; }
    public string KontaktTelefon { get; set; }
    public string EmailAdresa { get; set; }
    public string BrojLicneKarte { get; set; }
    public string Zvanje { get; set; }
    public int GodineStaza { get; set; }
    public List<Predmet> SpisakPredmeta { get; set; }
    public Profesor()
    {
        SpisakPredmeta = new List<Predmet>();
    }
    public Profesor(int id, string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, string licna, string zvanje, int staz)
    {
        Id = id;
        Prezime = prezime;
        Ime = ime;
        DatumRodjenja = datum;
        AdresaStanovanja = adresa;
        KontaktTelefon = telefon;
        EmailAdresa = email;
        BrojLicneKarte = licna;
        Zvanje = zvanje;
        GodineStaza = staz;
        SpisakPredmeta = new List<Predmet>();
    }

    public Profesor(string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, string licna, string zvanje, int staz)
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
        SpisakPredmeta = new List<Predmet>();
    }

    public override string ToString()
    {
        string predmetiInfo = "";

        if (SpisakPredmeta.Count == 0)
        {
            predmetiInfo = "Nema predmeta";
        }
        else
        {
            predmetiInfo = string.Join(", ", SpisakPredmeta.Select(predmet => predmet.NazivPredmeta));
        }

        return $"ID: {Id} | Prezime: {Prezime} | Ime: {Ime} | Datum rodjenja: {DatumRodjenja} | Adresa stanovanja: {AdresaStanovanja} | Kontakt telefon {KontaktTelefon} | Email adresa {EmailAdresa} | Broj licne karte: {BrojLicneKarte} | Zvanje: {Zvanje} | Godine staza: {GodineStaza} | Spisak predmeta: {predmetiInfo} |";
    }

    public string[] ToCSV()
    {
        string predmetiInfo = "";

        foreach (var predmet in SpisakPredmeta)
        {
            if (!string.IsNullOrEmpty(predmetiInfo))
            {
                predmetiInfo += ";";
            }
            predmetiInfo += predmet.SifraPredmeta;
        }

        string[] csvValues =
        {
        Id.ToString(),
        Prezime,
        Ime,
        DatumRodjenja.ToString("yyyy-MM-dd"),
        AdresaStanovanja.Ulica,
        AdresaStanovanja.Broj.ToString(),
        AdresaStanovanja.Grad,
        AdresaStanovanja.Drzava,
        KontaktTelefon,
        EmailAdresa,
        BrojLicneKarte,
        Zvanje,
        GodineStaza.ToString(),
        predmetiInfo
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
            Broj = int.Parse(values[5]),
            Grad = values[6],
            Drzava = values[7]
        };
        KontaktTelefon = values[8];
        EmailAdresa = values[9];
        BrojLicneKarte = values[10];
        Zvanje = values[11];
        GodineStaza = int.Parse(values[12]);
        if (values[13].Split(';').Length < 1) return;
        SpisakPredmeta = values[13].Split(';').Select(sifra => new Predmet() { SifraPredmeta = sifra }).ToList();

    }

    public void UnesiPredmete()
    {
        System.Console.WriteLine("Unesite predmete koje profesor predaje (završite unos enterom):");
        string unosPredmeta;
        while (!string.IsNullOrWhiteSpace(unosPredmeta = System.Console.ReadLine()))
        {
            // Ovde možete dodati proveru da li predmet već postoji negde
            if (!SpisakPredmeta.Any(predmet => predmet.SifraPredmeta == unosPredmeta))
            {
                // Kreirajte novi Predmet na osnovu šifre
                Predmet noviPredmet = new Predmet { SifraPredmeta = unosPredmeta };

                // Dodajte novi predmet u listu
                SpisakPredmeta.Add(noviPredmet);

                // Dodajte novi predmet i profesora u bazu ili na drugo odgovarajuće mesto

                //DodajNoviPredmet();
                System.Console.WriteLine($"Predmet '{unosPredmeta}' dodat profesoru.");
            }
            else
            {
                System.Console.WriteLine($"Predmet '{unosPredmeta}' već postoji u spisku profesora.");
            }
        }
    }


    public void DodajNoviPredmet()
    {
        System.Console.WriteLine("Unesite šifru predmeta: ");
        string sifra = System.Console.ReadLine();

        // Provera da li predmet već postoji
        if (SpisakPredmeta.Any(p => p.SifraPredmeta == sifra))
        {
            System.Console.WriteLine($"Predmet sa šifrom '{sifra}' već postoji u spisku profesora.");
            return;
        }

        System.Console.WriteLine("Unesite naziv predmeta: ");
        string naziv = System.Console.ReadLine();

        System.Console.WriteLine("Unesite semestar (Letnji/Zimski): ");
        SemestarEnum semestar;
        if (Enum.TryParse(System.Console.ReadLine(), true, out semestar))
        {
            System.Console.WriteLine("Unesite godinu studija: ");
            int godinaStudija = Convert.ToInt32(System.Console.ReadLine());

            System.Console.WriteLine("Unesite broj ESPB bodova: ");
            int bodovi = Convert.ToInt32(System.Console.ReadLine());

            // Ovde možete dodati proveru da li predmet već postoji negde
            //Predmet noviPredmet = new Predmet(sifra, naziv, semestar, godinaStudija, this, bodovi);
            //SpisakPredmeta.Add(noviPredmet);
            System.Console.WriteLine($"Novi predmet '{naziv}' dodat profesoru.");
        }
        else
        {
            System.Console.WriteLine("Neispravan unos semestra. Molimo vas unesite 'Letnji' ili 'Zimski'.");
        }
    }

    public string imePrezimeToString()
    {
        return $"{Ime} {Prezime}";
    }
}

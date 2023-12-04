/*Predmet (#predmet)
Šifra predmeta
Naziv predmeta
Semestar (letnji ili zimski)
Godina studija u kojoj se predmet izvodi
Predmetni profesor
Broj ESPB bodova
Spisak studenata koji su položili predmet
Spisak studenata koji nisu položili predmet
*/
namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
public enum SemestarEnum {Letnji, Zimski}
public class Predmet : ISerializable
{
    public string SifraPredmeta {get; set;}
    public string NazivPredmeta {get; set;}
    public SemestarEnum Semestar {get; set;}
    public int GodinaStudija {get; set;}
    public Profesor PredmetniProfesor {get; set;}
    public int BrojESPB {get; set;}
    public List<string> SpisakStudenataPolozili {get; set;}
    public List<string> SpisakStudenataNisuPolozili {get; set;}

    public Predmet()
    { 
    }
    public Predmet(string sifra, string naziv, SemestarEnum semestar, int godina, Profesor prof, int bodovi)
    {
        SifraPredmeta = sifra;
        NazivPredmeta = naziv;
        Semestar = semestar;
        GodinaStudija = godina;
        PredmetniProfesor = prof;
        BrojESPB = bodovi;
        SpisakStudenataPolozili = new List<string>();
        SpisakStudenataNisuPolozili = new List<string>();
    }

    public override string ToString()
    {
        string profesorImePrezime = (PredmetniProfesor != null) ? $"{PredmetniProfesor.Ime} {PredmetniProfesor.Prezime}" : "N/A";

        return $"Sifra: {SifraPredmeta} | Naziv: {NazivPredmeta} | Semestar: {Semestar} | Godina studija: {GodinaStudija} | Predmetni profesor: {profesorImePrezime} | Broj ESPB: {BrojESPB}";
    }

    public string[] ToCSV()
    {
        string profesorId = (PredmetniProfesor != null) ? PredmetniProfesor.Id.ToString() : "N/A";

        string[] csvValues =
        {
            SifraPredmeta,
            NazivPredmeta,
            Semestar.ToString(),
            GodinaStudija.ToString(),
            profesorId,
            BrojESPB.ToString(),
            string.Join(",", SpisakStudenataPolozili),
            string.Join(",", SpisakStudenataNisuPolozili)
        };

        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        SifraPredmeta = values[0];
        NazivPredmeta = values[1];
        Semestar = Enum.Parse<SemestarEnum>(values[2]);
        GodinaStudija = int.Parse(values[3]);

        if (int.TryParse(values[4], out int profesorId) && profesorId != -1)
        {
            // Ako postoji ID za predmetnog profesora, pronađi profesora sa datim ID-jem
            PredmetniProfesor = new Profesor() { Id = profesorId }; // Trebalo bi postaviti samo ID, a ne kompletnog profesora
        }
        else
        {
            // Inače, nema predmetnog profesora
            PredmetniProfesor = null;
        }

        BrojESPB = int.Parse(values[5]);
        SpisakStudenataPolozili = values[6].Split(',').ToList();
        SpisakStudenataNisuPolozili = values[7].Split(',').ToList();
    }



}
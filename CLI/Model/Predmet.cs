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
public enum SemestarEnum { Letnji, Zimski }
public class Predmet : ISerializable
{
    public string SifraPredmeta { get; set; }
    public string NazivPredmeta { get; set; }
    public SemestarEnum Semestar { get; set; }
    public int GodinaStudija { get; set; }
    public Profesor? PredmetniProfesor { get; set; }
    public int BrojESPB { get; set; }
    public List<Student> SpisakStudenataPolozili { get; set; }
    public List<Student> SpisakStudenataNisuPolozili { get; set; }
    public int? BrojcanaVrednostOcene { get; set; }
    public DateTime? DatumPolaganjaIspita { get; set; }

    public Predmet()
    {
        SpisakStudenataPolozili = new List<Student>();
        SpisakStudenataNisuPolozili = new List<Student>();
    }
    public Predmet(string sifra, string naziv, SemestarEnum semestar, int godina, Profesor prof, int bodovi,int ocena, DateTime datum)
    {
        SifraPredmeta = sifra;
        NazivPredmeta = naziv;
        Semestar = semestar;
        GodinaStudija = godina;
        PredmetniProfesor = prof;
        BrojESPB = bodovi;
        SpisakStudenataPolozili = new List<Student>();
        SpisakStudenataNisuPolozili = new List<Student>();
        BrojcanaVrednostOcene = ocena;
        DatumPolaganjaIspita = datum;
    }

    public override string ToString()
    {
        string profesorImePrezime = (PredmetniProfesor != null) ? PredmetniProfesor.imePrezimeToString() : "N/A";
        var prof = (PredmetniProfesor != null) ? PredmetniProfesor.Id : -1;
        return $"{SifraPredmeta}|{NazivPredmeta}|{Semestar}|{GodinaStudija}|{prof}|{BrojESPB}";
    }

    public string[] ToCSV()
    {
        string profesorInfo = (PredmetniProfesor != null) ? PredmetniProfesor.Id.ToString() : "-1";

        string[] csvValues =
        {
        SifraPredmeta,
        NazivPredmeta,
        Semestar.ToString(),
        GodinaStudija.ToString(),
        profesorInfo,
        BrojESPB.ToString(),
        string.Join(";", SpisakStudenataPolozili),
        string.Join(";", SpisakStudenataNisuPolozili)
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
            PredmetniProfesor = new Profesor() { Id = profesorId };
        }
        else
        {
            PredmetniProfesor = null;
        }
        BrojESPB = int.Parse(values[5]);
        if (values[6].Equals(String.Empty)) return;

        List<Student> tmp = new List<Student>();
        foreach (var se in values[6].Split(";"))
        {
            var tmpPred = se.Split('|');
            if (tmpPred.Length < 6) continue;
            tmp.Add(new Student()
            {
                Id = int.Parse(tmpPred[0].Trim()),
                Prezime = tmpPred[1].Trim(),
                Ime = tmpPred[2].Trim(),
                TrenutnaGodinaStudija = int.Parse(tmpPred[3].Trim()),
                Status = Enum.Parse<StatusEnum>(tmpPred[4]),
                ProsecnaOcena = double.Parse(tmpPred[5].Trim())
            });
        }

        SpisakStudenataPolozili = tmp;
        tmp = new List<Student>();
        foreach (var se in values[6].Split(";"))
        {
            var tmpPred = se.Split('|');
            if (tmpPred.Length < 6) continue;
            tmp.Add(new Student()
            {
                Id = int.Parse(tmpPred[0].Trim()),
                Prezime = tmpPred[1].Trim(),
                Ime = tmpPred[2].Trim(),
                TrenutnaGodinaStudija = int.Parse(tmpPred[3].Trim()),
                Status = Enum.Parse<StatusEnum>(tmpPred[4]),
                ProsecnaOcena = double.Parse(tmpPred[5].Trim())
            });
        }
        SpisakStudenataNisuPolozili = tmp;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is not Predmet) return false;
        var tmp = obj as Predmet;
        if (tmp.SifraPredmeta == this.SifraPredmeta) return true;
        return false;
    }
}
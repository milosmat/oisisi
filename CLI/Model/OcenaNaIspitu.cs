/*Ocena na ispitu (#ocena)
Student koji je položio ispit
Predmet
Brojčana vrednost ocene (u intervalu od 6 do 10)
Datum polaganja ispita
*/
namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
using CLI.DAO;

public class OcenaNaIspitu : ISerializable
{
    public Student StudentKojiJePolozio {get; set;}
    public Predmet Predmet {get; set;}
    public int BrojcanaVrednostOcene {get; set;}
    public DateTime DatumPolaganjaIspita {get; set;}

    public OcenaNaIspitu()
    { 
    }
    public OcenaNaIspitu(Student student, Predmet predmet, int ocena, DateTime datum)
    {
        StudentKojiJePolozio = student;
        Predmet = predmet;
        BrojcanaVrednostOcene = ocena;
        DatumPolaganjaIspita = datum;
    }

    public override string ToString()
    {
        return $"Student: {StudentKojiJePolozio.ToString()} | Predmet: {Predmet.ToString()} | Ocena: {BrojcanaVrednostOcene} | Datum polaganja: {DatumPolaganjaIspita.ToString("yyyy-MM-dd")}";
    }
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            StudentKojiJePolozio.Id.ToString(),
            Predmet.SifraPredmeta,
            BrojcanaVrednostOcene.ToString(),
            DatumPolaganjaIspita.ToString("yyyy-MM-dd")
        };
        return csvValues;
    }
    public void FromCSV(string[] values)
    {
        // Očekujemo da CSV vrednosti imaju odgovarajući redosled
        int studentId = int.Parse(values[0]);
        string predmetSifra = values[1];
        BrojcanaVrednostOcene = int.Parse(values[2]);
        DatumPolaganjaIspita = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);

        // Instanciramo objekte StudentDAO i PredmetDAO
        var studentDAO = new StudentDAO();
        var predmetDAO = new PredmetDAO();

        // Koristimo instancirane objekte
        StudentKojiJePolozio = studentDAO.UzmiStudentaPoID(studentId);
        Predmet = predmetDAO.UzmiPredmetPoSifri(predmetSifra);
    }

}
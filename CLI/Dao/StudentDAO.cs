
using StudentskaSluzba.Storage;
using StudentskaSluzba.Model;
namespace CLI.DAO;
class StudentDAO
{
    private readonly List<Student> studenti;
    private readonly Storage<Student> skladiste;

    public StudentDAO()
    {
        skladiste = new Storage<Student>("studenti.txt");
        studenti = skladiste.Load();
    }

    private int GenerateId()
    {
        if (studenti.Count == 0) return 0;
        return studenti[^1].Id + 1;
    }

    public Student DodajStudenta(Student student)
    {
        student.Id = GenerateId();
        studenti.Add(student);
        skladiste.Save(studenti);
        return student;
    }

    public Student AzurirajStudenta(Student student)
    {
        Student stariStudent = UzmiStudentaPoID(student.Id);
        if (stariStudent is null) return null;

        stariStudent.Prezime = student.Prezime;
        stariStudent.Ime = student.Ime;
        stariStudent.DatumRodjenja = student.DatumRodjenja;
        stariStudent.AdresaStanovanja = student.AdresaStanovanja;
        stariStudent.KontaktTelefon = student.KontaktTelefon;
        stariStudent.EmailAdresa = student.EmailAdresa;
        stariStudent.BrojIndeksa = student.BrojIndeksa;
        stariStudent.TrenutnaGodinaStudija = student.TrenutnaGodinaStudija;
        stariStudent.Status = student.Status;
        stariStudent.ProsecnaOcena = student.ProsecnaOcena;
        stariStudent.SpisakPolozenihIspita = student.SpisakPolozenihIspita;
        stariStudent.SpisakNepolozenihPredmeta = student.SpisakNepolozenihPredmeta;

        skladiste.Save(studenti);
        return stariStudent;
    }

    public Student? IzbrisiStudenta(int id)
    {
        Student? student = UzmiStudentaPoID(id);
        if (student == null) return null;

        studenti.Remove(student);
        skladiste.Save(studenti);
        return student;
    }

    public Student? UzmiStudentaPoID(int id)
    {
        return studenti.Find(s => s.Id == id);
    }

    public List<Student> UzmiSveStudente()
    {
        return studenti;
    }
}

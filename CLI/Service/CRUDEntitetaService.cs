using CLI.DAO;
using CLI.Service;
using StudentskaSluzba.Model;

namespace StudentskaSluzba.Service;

public class CRUDEntitetaService
{
    private static readonly StudentDAO StudentDao = new ();
    private static readonly ProfesorDAO ProfesorDao = new ();
    private static readonly PredmetDAO PredmetDao = new ();
    private static readonly AdresaDAO AdresaDao = new ();
    private static readonly IndeksDAO IndeksDao = new ();
    private static readonly OcenaNaIspituDAO Ocena = new();
    public static bool DodajStudenta(Student student)
    {
        Student s = StudentDao.DodajStudenta(student);
        return s.Id > -1;
    }

    public static Adresa DodajAdresu(Adresa a)
    {
        return AdresaDao.dodajAdresu(a);
    }

    public static Indeks DodajIndeks(Indeks i)
    {
        return IndeksDao.dodajIndeks(i);
    }

    public static bool DodajProfesora(Profesor profesor)
    {
        Profesor p = ProfesorDao.DodajProfesora(profesor);
        return p.Id > -1;
    }

    public static bool DodajPredmet(Predmet predmet)
    {
        Predmet p = PredmetDao.DodajPredmet(predmet);
        return p.SifraPredmeta != null;
    }

    public static bool IzmeniStudenta(Student student)
    {
        try
        {
            StudentDao.AzurirajStudenta(student);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IzmeniProfestora(Profesor profesor)
    {
        try
        {
            ProfesorDao.AzurirajProfesora(profesor);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IzmeniPredmet(Predmet predmet)
    {
        try
        {
            PredmetDao.AzurirajPredmet(predmet);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void PonistiOcenu(String predmet, int studentId)
    {
        try
        {
            OcenaNaIspitu tmpOcena = Ocena.UzmiSveOceneNaIspitu().Find(o => o.Predmet.SifraPredmeta == predmet);
            tmpOcena.BrojcanaVrednostOcene = 0;
            Ocena.AzurirajOcenuNaIspitu(tmpOcena);
            Student tmp = StudentDao.UzmiStudentaPoID(studentId);
            tmp.SpisakPolozenihIspita.Remove(tmp.SpisakPolozenihIspita.Find(p => p.SifraPredmeta == predmet));
            Predmet tmpP = PredmetService.GetByid(predmet);
            tmp.SpisakNepolozenihPredmeta.Add(tmpP);
            tmpP.SpisakStudenataPolozili.Remove(tmpP.SpisakStudenataPolozili.Find(s => s.Id==tmp.Id));
            tmpP.SpisakStudenataNisuPolozili.Add(tmp);
            PredmetDao.AzurirajPredmet(tmpP);
            StudentDao.AzurirajStudenta(tmp);
        }
        catch (Exception e)
        {   
            
            System.Console.WriteLine("Greška prilikom brisanja ocene!\n" + e.Message);
        }
    }

    public static bool ObrisiOcenu(String predmet, int studentId)
    {
        try
        {
            Ocena.IzbrisiOcenuNaIspitu(studentId, predmet);
            Student tmp = StudentDao.UzmiStudentaPoID(studentId);
            if (!tmp.SpisakPolozenihIspita.Remove(PredmetService.GetByid(predmet)))
                tmp.SpisakNepolozenihPredmeta.Remove(PredmetService.GetByid(predmet));
            StudentDao.AzurirajStudenta(tmp);
            Predmet tmpP = PredmetService.GetByid(predmet);
            if (!tmpP.SpisakStudenataPolozili.Remove(tmp))
                tmpP.SpisakStudenataNisuPolozili.Remove(tmp);
            PredmetDao.AzurirajPredmet(tmpP);
            return true;
        }
        catch
        {
            System.Console.Error.WriteLine("Greška prilikom brisanja ocene");
            return false;
        }
    }

    public static bool DodajPredmetStudentu(Predmet predmet, Student student, int ocena, DateTime datum)
    {
        try
        {
            if (ocena < 5)
                Ocena.DodajOcenuNaIspitu(new OcenaNaIspitu()
                {
                    StudentKojiJePolozio = student,
                    Predmet = predmet,
                    BrojcanaVrednostOcene = ocena,
                    DatumPolaganjaIspita = datum
                });
            else
            {
                OcenaNaIspitu tmpOcena = Ocena.UzmiSveOceneNaIspitu()
                    .Find(o => o.Predmet.Equals(predmet) && o.StudentKojiJePolozio.Equals(student));
                tmpOcena.BrojcanaVrednostOcene = ocena;
                tmpOcena.DatumPolaganjaIspita = datum;
                Ocena.AzurirajOcenuNaIspitu(tmpOcena);
            }
            if (ocena > 5)
            {
                student.SpisakPolozenihIspita.Add(predmet);
                student.SpisakNepolozenihPredmeta.Remove(predmet);
                predmet.SpisakStudenataPolozili.Add(student);                
            }
            else
            {
                student.SpisakNepolozenihPredmeta.Add(predmet);
                predmet.SpisakStudenataNisuPolozili.Add(student);
            }
            StudentDao.AzurirajStudenta(student);
            PredmetDao.AzurirajPredmet(predmet);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static bool DodajPredmetProfesoru(Profesor profesor, Predmet predmet)
    {
        try
        {
            profesor.SpisakPredmeta.Add(predmet);
            predmet.PredmetniProfesor = profesor;
            PredmetDao.AzurirajPredmet(predmet);
            ProfesorDao.AzurirajProfesora(profesor);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static bool ObrisiPredmetProfesoru(Profesor editProfesor, Predmet selectedPredmet)
    {
        try
        {
            selectedPredmet.PredmetniProfesor = null;
            editProfesor.SpisakPredmeta.Remove(selectedPredmet);
            PredmetDao.AzurirajPredmet(selectedPredmet);
            ProfesorDao.AzurirajProfesora(editProfesor);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
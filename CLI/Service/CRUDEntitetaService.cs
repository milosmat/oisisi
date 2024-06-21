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

    public static bool IzmeniProfesora(Profesor profesor)
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
            Ocena.IzbrisiOcenuNaIspitu(studentId, predmet);
            Student tmp = StudentDao.UzmiStudentaPoID(studentId);
            if (!tmp.SpisakPolozenihIspita.Remove(PredmetService.GetByid(predmet)))
                tmp.SpisakNepolozenihPredmeta.Remove(PredmetService.GetByid(predmet));
            StudentDao.AzurirajStudenta(tmp);
            Predmet tmpP = PredmetService.GetByid(predmet);
            if (!tmpP.SpisakStudenataPolozili.Remove(tmp))
                tmpP.SpisakStudenataNisuPolozili.Remove(tmp);
            PredmetDao.AzurirajPredmet(tmpP);
        }
        catch (Exception e)
        {   
            System.Console.WriteLine("Greška prilikom brisanja ocene!");
        }
    }

    public static bool DodajPredmetStudentu(Predmet predmet, Student student, int ocena, DateTime datum)
    {
        try
        {
            System.Console.WriteLine("Adding subject to student: " + student.Id);

            Ocena.DodajOcenuNaIspitu(new OcenaNaIspitu()
            {
                StudentKojiJePolozio = student,
                Predmet = predmet,
                BrojcanaVrednostOcene = ocena,
                DatumPolaganjaIspita = datum
            });

            if (ocena > 5)
            {
                student.SpisakPolozenihIspita.Add(predmet);
                predmet.SpisakStudenataPolozili.Add(student);
            }
            else
            {
                student.SpisakNepolozenihPredmeta.Add(predmet);
                predmet.SpisakStudenataNisuPolozili.Add(student);
            }

            StudentDao.AzurirajStudenta(student);
            PredmetDao.AzurirajPredmet(predmet);

            System.Console.WriteLine("Subject added successfully to student: " + student.Id);

            return true;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Error adding subject to student: " + e.Message);
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
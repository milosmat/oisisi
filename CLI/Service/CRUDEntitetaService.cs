using CLI.DAO;
using StudentskaSluzba.Model;

namespace StudentskaSluzba.Service;

public class CRUDEntitetaService
{
    private static readonly StudentDAO _studentDao = new StudentDAO();
    private static readonly ProfesorDAO _profesorDao = new ProfesorDAO();
    private static readonly PredmetDAO _predmetDao = new PredmetDAO();
    private static readonly AdresaDAO _adresaDao = new AdresaDAO();
    private static readonly IndeksDAO _indeksDao = new IndeksDAO();
    public static bool DodajStudenta(Student student)
    {
        Student s = _studentDao.DodajStudenta(student);
        return s.Id != null;
    }

    public static Adresa DodajAdresu(Adresa a)
    {
        return _adresaDao.dodajAdresu(a);
    }

    public static Indeks DodajIndeks(Indeks i)
    {
        return _indeksDao.dodajIndeks(i);
    }

    public static bool DodajProfesora(Profesor profesor)
    {
        Profesor p = _profesorDao.DodajProfesora(profesor);
        return p.Id != null;
    }

    public static bool DodajPredmet(Predmet predmet)
    {
        Predmet p = _predmetDao.DodajPredmet(predmet);
        return p.SifraPredmeta != null;
    }

    public static bool IzmeniStudenta(Student student)
    {
        try
        {
            _studentDao.AzurirajStudenta(student);
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
            _profesorDao.AzurirajProfesora(profesor);
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
            _predmetDao.AzurirajPredmet(predmet);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
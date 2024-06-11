using CLI.DAO;
using StudentskaSluzba.Model;

namespace StudentskaSluzba.Service;

public class DodavanjeEntitetaService
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
}
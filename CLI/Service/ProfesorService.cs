using CLI.DAO;
using StudentskaSluzba.Model;

namespace CLI.Service;

public class ProfesorService
{
    private static readonly ProfesorDAO _profesorDao = new ProfesorDAO();

    public static List<Profesor> GetProfesors()
    {
        return _profesorDao.UzmiSveProfesore();
    }

    public static Profesor GetById(int id)
    {
        return _profesorDao.UzmiProfesoraPoID(id);
    }

    public static bool DeleteProfesor(int profesorId)
    {
        var profesor = _profesorDao.UzmiProfesoraPoID(profesorId);
        if (profesor == null) return false;
        return _profesorDao.IzbrisiProfesora(profesorId) != null;
    }
}
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
}
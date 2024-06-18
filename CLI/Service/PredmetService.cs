using CLI.DAO;
using StudentskaSluzba.Model;

namespace CLI.Service;

public class PredmetService
{
    private static readonly PredmetDAO _predmetDao = new PredmetDAO();

    public static List<Predmet> GetPredmets()
    {
        return _predmetDao.UzmiSvePredmete();
    }

    public static Predmet GetByid(string sifra)
    {
        return _predmetDao.UzmiPredmetPoSifri(sifra);
    }
}
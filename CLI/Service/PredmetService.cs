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
    public static bool DeletePredmet(string predmetSifra)
    {
        var predmet = _predmetDao.UzmiPredmetPoSifri(predmetSifra);
        if (predmet == null) return false;
        return _predmetDao.IzbrisiPredmet(predmetSifra) != null;
    }
}
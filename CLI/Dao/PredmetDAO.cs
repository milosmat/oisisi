using StudentskaSluzba.Model;
using StudentskaSluzba.Storage;
namespace CLI.DAO;
class PredmetDAO
{
    private List<Predmet> predmeti;
    private readonly Storage<Predmet> skladiste;

    public PredmetDAO()
    {
        skladiste = new Storage<Predmet>("predmet.txt");
    }

    public Predmet DodajPredmet(Predmet predmet)
    {
        predmeti = skladiste.Load();
        predmeti.Add(predmet);
        skladiste.Save(predmeti);
        return predmet;
    }

    public Predmet AzurirajPredmet(Predmet predmet)
    {
        predmeti = skladiste.Load();
        Predmet stariPredmet = UzmiPredmetPoSifri(predmet.SifraPredmeta);
        if (stariPredmet is null) return null;

        stariPredmet.NazivPredmeta = predmet.NazivPredmeta;
        stariPredmet.Semestar = predmet.Semestar;
        stariPredmet.GodinaStudija = predmet.GodinaStudija;
        stariPredmet.PredmetniProfesor = predmet.PredmetniProfesor;
        stariPredmet.BrojESPB = predmet.BrojESPB;
        stariPredmet.SpisakStudenataPolozili = predmet.SpisakStudenataPolozili;
        stariPredmet.SpisakStudenataNisuPolozili = predmet.SpisakStudenataNisuPolozili;

        skladiste.Save(predmeti);
        return stariPredmet;
    }

    public Predmet? IzbrisiPredmet(string sifra)
    {
        predmeti = skladiste.Load();
        Predmet? predmet = UzmiPredmetPoSifri(sifra);
        if (predmet == null) return null;

        predmeti.Remove(predmet);
        skladiste.Save(predmeti);
        return predmet;
    }

    public Predmet? UzmiPredmetPoSifri(string sifra)
    {
        predmeti = skladiste.Load();
        return predmeti.Find(p => p.SifraPredmeta == sifra);
    }

    public List<Predmet> UzmiSvePredmete()
    {
        predmeti = skladiste.Load();
        return predmeti;
    }
}

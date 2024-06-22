using CLI.Service;
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
        var predmetToRemove = predmeti.FirstOrDefault(p => p.SifraPredmeta == stariPredmet.SifraPredmeta);
        if (predmetToRemove != null)
        {
            predmeti.Remove(predmetToRemove);
        }

        predmeti.Add(stariPredmet); // Dodajte ažurirani predmet
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

    public Predmet UzmiPredmetPoSifri(string sifraPredmeta)
    {
        var predmeti = PredmetService.GetPredmets(); // Pretpostavljam da ovde učitavaš predmete
        var predmet = predmeti.FirstOrDefault(p => p.SifraPredmeta.Equals(sifraPredmeta, StringComparison.OrdinalIgnoreCase));

        if (predmet == null)
        {
            Console.WriteLine($"Predmet sa šifrom {sifraPredmeta} nije pronađen.");
            throw new NullReferenceException($"Predmet sa šifrom {sifraPredmeta} nije pronađen.");
        }

        return predmet;
    }


    public List<Predmet> UzmiSvePredmete()
    {
        predmeti = skladiste.Load();
        return predmeti;
    }
}

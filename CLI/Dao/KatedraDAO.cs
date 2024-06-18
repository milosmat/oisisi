using StudentskaSluzba.Model;
using StudentskaSluzba.Storage;
namespace CLI.DAO;
class KatedraDAO
{
    private List<Katedra> katedre;
    private readonly Storage<Katedra> skladiste;

    public KatedraDAO()
    {
        skladiste = new Storage<Katedra>("katedra.txt");
    }

    public Katedra DodajKatedru(Katedra katedra)
    {
        katedre = skladiste.Load();
        katedre.Add(katedra);
        skladiste.Save(katedre);
        return katedra;
    }

    public Katedra AzurirajKatedru(Katedra katedra)
    {
        katedre = skladiste.Load();
        Katedra staraKatedra = UzmiKatedruPoSifri(katedra.SifraKatedre);
        if (staraKatedra is null) return null;

        staraKatedra.NazivKatedre = katedra.NazivKatedre;
        staraKatedra.SefKatedre = katedra.SefKatedre;
        staraKatedra.SpisakProfesora = katedra.SpisakProfesora;

        skladiste.Save(katedre);
        return staraKatedra;
    }

    public Katedra? IzbrisiKatedru(string sifra)
    {
        katedre = skladiste.Load();

        Katedra? katedra = UzmiKatedruPoSifri(sifra);
        if (katedra == null) return null;

        katedre.Remove(katedra);
        skladiste.Save(katedre);
        return katedra;
    }

    private Katedra? UzmiKatedruPoSifri(string sifra)
    {
        katedre = skladiste.Load();

        return katedre.Find(k => k.SifraKatedre == sifra);
    }

    public List<Katedra> UzmiSveKatedre()
    {
        katedre = skladiste.Load();

        return katedre;
    }
}

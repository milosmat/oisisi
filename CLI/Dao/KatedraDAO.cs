
using System.Collections.Generic;
using StudentskaSluzba.Storage;
using StudentskaSluzba.Model;
namespace CLI.DAO;
class KatedraDAO
{
    private readonly List<Katedra> katedre;
    private readonly Storage<Katedra> skladiste;

    public KatedraDAO()
    {
        skladiste = new Storage<Katedra>("katedra.txt");
        katedre = skladiste.Load();
    }

    public Katedra DodajKatedru(Katedra katedra)
    {
        katedre.Add(katedra);
        skladiste.Save(katedre);
        return katedra;
    }

    public Katedra AzurirajKatedru(Katedra katedra)
    {
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
        Katedra? katedra = UzmiKatedruPoSifri(sifra);
        if (katedra == null) return null;

        katedre.Remove(katedra);
        skladiste.Save(katedre);
        return katedra;
    }

    private Katedra? UzmiKatedruPoSifri(string sifra)
    {
        return katedre.Find(k => k.SifraKatedre == sifra);
    }

    public List<Katedra> UzmiSveKatedre()
    {
        return katedre;
    }
}

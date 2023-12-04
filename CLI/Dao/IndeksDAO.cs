using StudentskaSluzba.Storage;
using StudentskaSluzba.Model;
namespace CLI.DAO;
class IndeksDAO
{
    private readonly List<Indeks> indeksi;
    private readonly Storage<Indeks> skladiste;

    public IndeksDAO()
    {
        skladiste = new Storage<Indeks>("indeks.txt");
        indeksi = skladiste.Load();
    }

    private int GenerateId()
    {
        if (indeksi.Count == 0) return 0;
        return indeksi[^1].Id + 1;
    }

    public Indeks dodajIndeks(Indeks indeks)
    {
        indeks.Id = GenerateId();
        indeksi.Add(indeks);
        skladiste.Save(indeksi);
        return indeks;
    }
    /*
    public Adresa azurirajAdresu(Adresa adresa)
    {
        Indeks stariIndeks = UzmiIndeksPoID(indeks.Id);
        if (staraAdresa is null) return null;

        staraAdresa.Ulica = adresa.Ulica;
        staraAdresa.Broj = adresa.Broj;
        staraAdresa.Grad = adresa.Grad;
        staraAdresa.Drzava = adresa.Drzava;

        skladiste.Save(adrese);
        return staraAdresa;
    }
    */
    public Indeks? IzbrisiIndeks(int id)
    {
        Indeks? indeks = UzmiIndeksPoID(id);
        if (indeks == null) return null;

        indeksi.Remove(indeks);
        skladiste.Save(indeksi);
        return indeks;
    }

    private Indeks? UzmiIndeksPoID(int id)
    {
        return indeksi.Find(i => i.Id == id);
    }

    public List<Indeks> UzmiSveIndekse()
    {
        return indeksi;
    }
}
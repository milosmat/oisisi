
using StudentskaSluzba.Storage;
using StudentskaSluzba.Model;

namespace CLI.DAO;
class AdresaDAO
{
    private readonly List<Adresa> adrese;
    private readonly Storage<Adresa> skladiste;


    public AdresaDAO()
    {
        skladiste = new Storage<Adresa>("adresa.txt");
        adrese = skladiste.Load();
    }

    private int GenerateId()
    {
        if (adrese.Count == 0) return 0;
        return adrese[^1].Id + 1;
    }

    public Adresa dodajAdresu(Adresa adresa)
    {
        adresa.Id = GenerateId();
        adrese.Add(adresa);
        skladiste.Save(adrese);
        return adresa;
    }

    public Adresa azurirajAdresu(Adresa adresa)
    {
        Adresa staraAdresa = UzmiAdresuPoID(adresa.Id);
        if (staraAdresa is null) return null;

        staraAdresa.Ulica = adresa.Ulica;
        staraAdresa.Broj = adresa.Broj;
        staraAdresa.Grad = adresa.Grad;
        staraAdresa.Drzava = adresa.Drzava;

        skladiste.Save(adrese);
        return staraAdresa;
    }

    public Adresa? IzbrisiAdresu(int id)
    {
        Adresa? adresa = UzmiAdresuPoID(id);
        if (adresa == null) return null;

        adrese.Remove(adresa);
        skladiste.Save(adrese);
        return adresa;
    }

    private Adresa? UzmiAdresuPoID(int id)
    {
        return adrese.Find(a => a.Id == id);
    }

    public List<Adresa> UzmiSveAdrese()
    {
        return adrese;
    }

}
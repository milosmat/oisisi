using StudentskaSluzba.Model;
using StudentskaSluzba.Storage;

namespace CLI.DAO;
public class AdresaDAO
{
    private List<Adresa> adrese;
    private readonly Storage<Adresa> skladiste;


    public AdresaDAO()
    {
        skladiste = new Storage<Adresa>("adresa.txt");
    }

    private int GenerateId()
    {
        if (adrese.Count == 0) return 0;
        return adrese[^1].Id + 1;
    }

    public Adresa dodajAdresu(Adresa adresa)
    {
        adrese = skladiste.Load();

        adresa.Id = GenerateId();
        adrese.Add(adresa);
        skladiste.Save(adrese);
        return adresa;
    }

    public Adresa azurirajAdresu(Adresa adresa)
    {
        adrese = skladiste.Load();

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
        adrese = skladiste.Load();

        Adresa? adresa = UzmiAdresuPoID(id);
        if (adresa == null) return null;

        adrese.Remove(adresa);
        skladiste.Save(adrese);
        return adresa;
    }

    private Adresa? UzmiAdresuPoID(int id)
    {
        adrese = skladiste.Load();

        return adrese.Find(a => a.Id == id);
    }

    public List<Adresa> UzmiSveAdrese()
    {
        adrese = skladiste.Load();

        return adrese;
    }

}
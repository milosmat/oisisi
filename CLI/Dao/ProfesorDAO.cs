using System.Collections.Generic;
using StudentskaSluzba.Model;
using StudentskaSluzba.Storage;
namespace CLI.DAO;
class ProfesorDAO
{
    private readonly List<Profesor> profesori;
    private readonly Storage<Profesor> skladiste;

    public ProfesorDAO()
    {
        skladiste = new Storage<Profesor>("profesori.txt");
        profesori = skladiste.Load();
    }

    private int GenerateId()
    {
        if (profesori.Count == 0) return 0;
        return profesori[^1].Id + 1;
    }

    public Profesor DodajProfesora(Profesor profesor)
    {
        profesor.Id = GenerateId();
        profesori.Add(profesor);
        skladiste.Save(profesori);
        return profesor;
    }

    public Profesor AzurirajProfesora(Profesor profesor)
    {
        Profesor stariProfesor = UzmiProfesoraPoID(profesor.Id);
        if (stariProfesor is null) return null;

        stariProfesor.Prezime = profesor.Prezime;
        stariProfesor.Ime = profesor.Ime;
        stariProfesor.DatumRodjenja = profesor.DatumRodjenja;
        stariProfesor.AdresaStanovanja = profesor.AdresaStanovanja;
        stariProfesor.KontaktTelefon = profesor.KontaktTelefon;
        stariProfesor.EmailAdresa = profesor.EmailAdresa;
        stariProfesor.BrojLicneKarte = profesor.BrojLicneKarte;
        stariProfesor.Zvanje = profesor.Zvanje;
        stariProfesor.GodineStaza = profesor.GodineStaza;
        stariProfesor.SpisakPredmeta = profesor.SpisakPredmeta;

        skladiste.Save(profesori);
        return stariProfesor;
    }

    public Profesor? IzbrisiProfesora(int id)
    {
        Profesor? profesor = UzmiProfesoraPoID(id);
        if (profesor == null) return null;

        profesori.Remove(profesor);
        skladiste.Save(profesori);
        return profesor;
    }

    public Profesor? UzmiProfesoraPoID(int id)
    {
        return profesori.Find(p => p.Id == id);
    }

    public List<Profesor> UzmiSveProfesore()
    {
        return profesori;
    }


}

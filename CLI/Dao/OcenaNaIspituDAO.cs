
using System.Collections.Generic;
using StudentskaSluzba.Model;
using StudentskaSluzba.Storage;

namespace CLI.DAO;
public class OcenaNaIspituDAO
    {
    private readonly List<OcenaNaIspitu> oceneNaIspitu;
    private readonly Storage<OcenaNaIspitu> skladiste;

    public OcenaNaIspituDAO()
    {
        skladiste = new Storage<OcenaNaIspitu>("ocene_na_ispitu.txt");
        oceneNaIspitu = skladiste.Load();
    }

    public OcenaNaIspitu DodajOcenuNaIspitu(OcenaNaIspitu ocena)
    {
        oceneNaIspitu.Add(ocena);
        skladiste.Save(oceneNaIspitu);
        return ocena;
    }

    public OcenaNaIspitu AzurirajOcenuNaIspitu(OcenaNaIspitu ocena)
    {
        OcenaNaIspitu staraOcena = UzmiOcenuNaIspitu(ocena.StudentKojiJePolozio.Id, ocena.Predmet.SifraPredmeta);
        if (staraOcena is null) return null;

        staraOcena.BrojcanaVrednostOcene = ocena.BrojcanaVrednostOcene;
        staraOcena.DatumPolaganjaIspita = ocena.DatumPolaganjaIspita;

        skladiste.Save(oceneNaIspitu);
        return staraOcena;
    }

    public OcenaNaIspitu? IzbrisiOcenuNaIspitu(int studentId, string predmetSifra)
    {
        OcenaNaIspitu? ocena = UzmiOcenuNaIspitu(studentId, predmetSifra);
        if (ocena == null) return null;

        oceneNaIspitu.Remove(ocena);
        skladiste.Save(oceneNaIspitu);
        return ocena;
    }

    private OcenaNaIspitu? UzmiOcenuNaIspitu(int studentId, string predmetSifra)
    {
        return oceneNaIspitu.Find(o => o.StudentKojiJePolozio.Id == studentId && o.Predmet.SifraPredmeta == predmetSifra);
    }

    public List<OcenaNaIspitu> UzmiSveOceneNaIspitu()
    {
        return oceneNaIspitu;
    }
    
}

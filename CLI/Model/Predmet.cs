/*Predmet (#predmet)
Šifra predmeta
Naziv predmeta
Semestar (letnji ili zimski)
Godina studija u kojoj se predmet izvodi
Predmetni profesor
Broj ESPB bodova
Spisak studenata koji su položili predmet
Spisak studenata koji nisu položili predmet
*/

using System;
using System.Collections.Generic;
using System.Linq;
using CLI.Service;

namespace StudentskaSluzba.Model;

using CLI.DAO;
using StudentskaSluzba.Serialization;
public enum SemestarEnum {Letnji, Zimski}
public class Predmet : ISerializable
{
    public string SifraPredmeta {get; set;}
    public string NazivPredmeta {get; set;}
    public SemestarEnum Semestar {get; set;}
    public int GodinaStudija {get; set;}
    public Profesor? PredmetniProfesor {get; set;}
    public int BrojESPB {get; set;}
    public List<Student> SpisakStudenataPolozili {get; set;}
    public List<Student> SpisakStudenataNisuPolozili {get; set;}

    public Predmet()
    {
        SpisakStudenataPolozili = new List<Student>();
        SpisakStudenataNisuPolozili = new List<Student>();
    }
    public Predmet(string sifra, string naziv, SemestarEnum semestar, int godina, Profesor prof, int bodovi)
    {
        SifraPredmeta = sifra;
        NazivPredmeta = naziv;
        Semestar = semestar;
        GodinaStudija = godina;
        PredmetniProfesor = prof;
        BrojESPB = bodovi;
        SpisakStudenataPolozili = new List<Student>();
        SpisakStudenataNisuPolozili = new List<Student>();
    }

    public override string ToString()
    {
        string profesorImePrezime = (PredmetniProfesor != null) ? PredmetniProfesor.imePrezimeToString() : "N/A";

        return $"{SifraPredmeta}|{NazivPredmeta}|{Semestar}|{GodinaStudija}|{profesorImePrezime}|{BrojESPB}";
    }

    public string[] ToCSV()
    {
        string profesorInfo = (PredmetniProfesor != null) ? PredmetniProfesor.Id.ToString() : "-1";

        string[] csvValues =
        {
        SifraPredmeta,
        NazivPredmeta,
        Semestar.ToString(),
        GodinaStudija.ToString(),
        profesorInfo,
        BrojESPB.ToString(),
        string.Join(";", SpisakStudenataPolozili),
        string.Join(";", SpisakStudenataNisuPolozili)
    };

        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        SifraPredmeta = values[0];
        NazivPredmeta = values[1];
        Semestar = Enum.Parse<SemestarEnum>(values[2]);
        GodinaStudija = int.Parse(values[3]);

        if (int.TryParse(values[4], out int profesorId) && profesorId != -1)
        {
            PredmetniProfesor =
                ProfesorService.GetById(profesorId);
        }
        else
        {
            PredmetniProfesor = null;
        }
        if (values[6].Equals(String.Empty)) return;
        
        BrojESPB = int.Parse(values[5]);
        List<Student> tmp = new List<Student>();
        foreach (var se in values[6].Split(";"))
        {
            var tmpPred = se.Split('|');
            tmp.Add(new Student()
            {
                Id = int.Parse(tmpPred[0].Trim()),
                Prezime = tmpPred[1].Trim(),
                Ime = tmpPred[2].Trim(),
                TrenutnaGodinaStudija = int.Parse(tmpPred[3].Trim()),
                Status = Enum.Parse<StatusEnum>(tmpPred[4]),
                ProsecnaOcena = double.Parse(tmpPred[5].Trim())
            });
        }

        SpisakStudenataPolozili = tmp;
        tmp = new List<Student>();
        foreach (var se in values[6].Split(";"))
        {
            var tmpPred = se.Split('|');
            tmp.Add(new Student()
            {
                Id = int.Parse(tmpPred[0].Trim()),
                Prezime = tmpPred[1].Trim(),
                Ime = tmpPred[2].Trim(),
                TrenutnaGodinaStudija = int.Parse(tmpPred[3].Trim()),
                Status = Enum.Parse<StatusEnum>(tmpPred[4]),
                ProsecnaOcena = double.Parse(tmpPred[5].Trim())
            });
        }
        SpisakStudenataNisuPolozili = tmp;
    }

}
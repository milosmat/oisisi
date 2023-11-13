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
public enum SemestarEnum {Letnji, Zimski}
public class Predmet
{
    public string SifraPredmeta {get; set;}
    public string NazivPredmeta {get; set;}
    public SemestarEnum Semestar {get; set;}
    public int GodinaStudija {get; set;}
    public Profesor PredmetniProfesor {get; set;}
    public int BrojESPB {get; set;}
    public List<string> SpisakStudenataPolozili {get; set;}
    public List<string> SpisakStudenataNisuPolozili {get; set;}

    public Predmet(string sifra, string naziv, SemestarEnum semestar, int godina, Profesor prof, int bodovi)
    {
        SifraPredmeta = sifra;
        NazivPredmeta = naziv;
        Semestar = semestar;
        GodinaStudija = godina;
        PredmetniProfesor = prof;
        BrojESPB = bodovi;
        SpisakStudenataPolozili = new List<string>();
        SpisakStudenataNisuPolozili = new List<string>();
    }
}
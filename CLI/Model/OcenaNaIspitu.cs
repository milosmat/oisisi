/*Ocena na ispitu (#ocena)
Student koji je položio ispit
Predmet
Brojčana vrednost ocene (u intervalu od 6 do 10)
Datum polaganja ispita
*/

public class OcenaNaIspitu
{
    public Student StudentKojiJePolozio {get; set;}
    public Predmet Predmet {get; set;}
    public int BrojcanaVrednostOcene {get; set;}
    public DateTime DatumPolaganjaIspita {get; set;}
    public OcenaNaIspitu(Student student, Predmet predmet, int ocena, DateTime datum)
    {
        StudentKojiJePolozio = student;
        Predmet = predmet;
        BrojcanaVrednostOcene = ocena;
        DatumPolaganjaIspita = datum;
    }
}
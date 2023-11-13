/*Indeks (#indeks)
Oznaka smera
Broj upisa
Godina upisa*/

public class Indeks
{
    public string OznakaSmera {get; set;}
    public int BrojUpisa {get; set;}
    public int GodinaUpisa {get; set;}

    public Indeks(string oznaka, int brojUpisa, int godina)
    {
        OznakaSmera = oznaka;
        BrojUpisa = brojUpisa;
        GodinaUpisa = godina;
    }
}

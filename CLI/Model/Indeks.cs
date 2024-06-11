/*Indeks (#indeks)
Oznaka smera
Broj upisa
Godina upisa*/


namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
public class Indeks : ISerializable
{
    public string OznakaSmera {get; set;}
    public int BrojUpisa {get; set;}
    public int GodinaUpisa {get; set;}

    public Indeks()
    { 
    }
    public Indeks(string oznaka, int brojUpisa, int godina)
    {
        OznakaSmera = oznaka;
        BrojUpisa = brojUpisa;
        GodinaUpisa = godina;
    }

    public override string ToString()
    {
        return $"{OznakaSmera} {BrojUpisa}/{GodinaUpisa}";
    }
    public string[] ToCSV()
    {
        string[] csvValues =
       {
            OznakaSmera,
            BrojUpisa.ToString(),
            GodinaUpisa.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        OznakaSmera = values[0];
        BrojUpisa = int.Parse(values[1]);
        GodinaUpisa = int.Parse(values[2]);
    }

    public Indeks unesiIndeks()
    {
        System.Console.WriteLine("Unesite oznaku smera: ");
        string oznakaSmera = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite broj upisa: ");
        int brojUpisa;
        while (!int.TryParse(System.Console.ReadLine(), out brojUpisa))
        {
            System.Console.WriteLine("Neispravan unos. Molimo unesite broj upisa ponovo: ");
        }

        System.Console.WriteLine("Unesite godinu upisa: ");
        int godinaUpisa;
        while (!int.TryParse(System.Console.ReadLine(), out godinaUpisa))
        {
            System.Console.WriteLine("Neispravan unos. Molimo unesite godinu upisa ponovo: ");
        }

        return new Indeks(oznakaSmera, brojUpisa, godinaUpisa);
    }

}

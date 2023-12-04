/*Indeks (#indeks)
Oznaka smera
Broj upisa
Godina upisa*/


namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
public class Indeks : ISerializable
{
    public int Id { get; set; }
    public string OznakaSmera {get; set;}
    public int BrojUpisa {get; set;}
    public int GodinaUpisa {get; set;}

    public Indeks()
    { 
    }
    public Indeks(int id, string oznaka, int brojUpisa, int godina)
    {
        Id = id;
        OznakaSmera = oznaka;
        BrojUpisa = brojUpisa;
        GodinaUpisa = godina;
    }
    public Indeks(string oznaka, int brojUpisa, int godina)
    {
        OznakaSmera = oznaka;
        BrojUpisa = brojUpisa;
        GodinaUpisa = godina;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Oznaka smera: {OznakaSmera} | Broj upisa: {BrojUpisa} | Godina upisa: {GodinaUpisa}";
    }
    public string[] ToCSV()
    {
        string[] csvValues =
       {
            Id.ToString(),
            OznakaSmera,
            BrojUpisa.ToString(),
            GodinaUpisa.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        OznakaSmera = values[1];
        BrojUpisa = int.Parse(values[2]);
        GodinaUpisa = int.Parse(values[3]);
    }

}

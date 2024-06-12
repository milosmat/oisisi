/*Katedra (#katedra)
Šifra katedre
Naziv katedre
Šef katedre
Spisak profesora koji su na katedri
*/

using System.Collections.Generic;
using System.Linq;

namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
public class Katedra : ISerializable
{
    public string SifraKatedre { get; set; }
    public string NazivKatedre { get; set; }
    public Profesor SefKatedre { get; set; }
    public List<Profesor> SpisakProfesora { get; set; }

    public Katedra()
    {
    }
    public Katedra(string sifra, string naziv, Profesor sef)
    {
        SifraKatedre = sifra;
        NazivKatedre = naziv;
        SefKatedre = sef;
        SpisakProfesora = new List<Profesor>();
    }

    public override string ToString()
    {
        string sefImePrezime = $"{SefKatedre.Ime} {SefKatedre.Prezime}";

        return $"Sifra: {SifraKatedre} | Naziv: {NazivKatedre} | Sef katedre: {sefImePrezime}";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            SifraKatedre,
            NazivKatedre,
            SefKatedre.Id.ToString()
        };

        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        SifraKatedre = values[0];
        NazivKatedre = values[1];
        SefKatedre = SpisakProfesora.FirstOrDefault(p => p.Id == int.Parse(values[2]));
    }
}
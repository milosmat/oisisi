
using System.Collections.Generic;
using System.Linq;

namespace StudentskaSluzba.Model;

using CLI.Service;
using StudentskaSluzba.Serialization;
public class Katedra : ISerializable
{
    public string SifraKatedre { get; set; }
    public string NazivKatedre { get; set; }
    public Profesor SefKatedre { get; set; }
    public List<Profesor> SpisakProfesora { get; set; }

    public Katedra()
    {
        SpisakProfesora = new List<Profesor>();
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
        string sefImePrezime = SefKatedre != null ? $"{SefKatedre.Ime} {SefKatedre.Prezime}" : "Nema šefa";
        return $"Sifra: {SifraKatedre} | Naziv: {NazivKatedre} | Sef katedre: {sefImePrezime}";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            SifraKatedre,
            NazivKatedre,
            SefKatedre != null ? SefKatedre.Id.ToString() : string.Empty,
            string.Join(";", SpisakProfesora.Select(p => p.Id.ToString()))
        };

        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        List<Profesor> sviProfesori = ProfesorService.GetProfesors();
        SifraKatedre = values[0];
        NazivKatedre = values[1];
        int sefId;
        if (int.TryParse(values[2], out sefId))
        {
            SefKatedre = sviProfesori.FirstOrDefault(p => p.Id == sefId);
        }
        SpisakProfesora = values[3].Split(';').Select(id => sviProfesori.FirstOrDefault(p => p.Id == int.Parse(id))).Where(p => p != null).ToList();
    }
}
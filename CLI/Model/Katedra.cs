/*Katedra (#katedra)
Šifra katedre
Naziv katedre
Šef katedre
Spisak profesora koji su na katedri
*/

public class Katedra
{
    public string SifraKatedre {get; set;}
    public string NazivKatedre {get; set;}
    public Profesor SefKatedre {get; set;}
    public List<Profesor> SpisakProfesora {get; set;}

    public Katedra(string sifra, string naziv, Profesor sef)
    {
        SifraKatedre = sifra;
        NazivKatedre = naziv;
        SefKatedre = sef;
        SpisakProfesora = new List<Profesor>();
    }
}
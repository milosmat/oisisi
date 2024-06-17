using System;
using System.Collections.Generic;
using System.Linq;
using StudentskaSluzba.Serialization;
namespace StudentskaSluzba.Model;

public enum StatusEnum { Budzet, Samofinansiranje }
public class Student : ISerializable
{
    public int Id { get; set; }
    public string Prezime { get; set; }
    public string Ime { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public Adresa AdresaStanovanja { get; set; }
    public string KontaktTelefon { get; set; }
    public string EmailAdresa { get; set; }
    public Indeks BrojIndeksa { get; set; }
    public int TrenutnaGodinaStudija { get; set; }
    public StatusEnum Status { get; set; }
    public double ProsecnaOcena { get; set; }
    public List<Predmet> SpisakPolozenihIspita { get; set; }
    public List<Predmet> SpisakNepolozenihPredmeta { get; set; }

    public Student()
    {
        SpisakPolozenihIspita = new List<Predmet>();
        SpisakNepolozenihPredmeta = new List<Predmet>();
    }
    public Student(int id, string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, Indeks brIndeksa, int trGodina, StatusEnum status, double prosek)
    {
        Id = id;
        Prezime = prezime;
        Ime = ime;
        DatumRodjenja = datum;
        AdresaStanovanja = adresa;
        KontaktTelefon = telefon;
        EmailAdresa = email;
        BrojIndeksa = brIndeksa;
        TrenutnaGodinaStudija = trGodina;
        Status = status;
        ProsecnaOcena = prosek;
        SpisakPolozenihIspita = new List<Predmet>();
        SpisakNepolozenihPredmeta = new List<Predmet>();
    }

    public Student(string prezime, string ime, DateTime datum, Adresa adresa, string telefon, string email, Indeks brIndeksa, int trGodina, StatusEnum status, double prosek)
    {
        Prezime = prezime;
        Ime = ime;
        DatumRodjenja = datum;
        AdresaStanovanja = adresa;
        KontaktTelefon = telefon;
        EmailAdresa = email;
        BrojIndeksa = brIndeksa;
        TrenutnaGodinaStudija = trGodina;
        Status = status;
        ProsecnaOcena = prosek;
        SpisakPolozenihIspita = new List<Predmet>();
        SpisakNepolozenihPredmeta = new List<Predmet>();
    }

    public override string ToString()
    {
        return $"ID: {Id,5} | Prezime: {Prezime,-20} | Ime: {Ime,-20} | Godina: {TrenutnaGodinaStudija,3} | Status: {Status} | Prosek: {ProsecnaOcena,5:F2} |";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Prezime,
            Ime,
            DatumRodjenja.ToString("yyyy-MM-dd"),
            AdresaStanovanja.Id.ToString(),
            AdresaStanovanja.Ulica,
            AdresaStanovanja.Broj.ToString(),
            AdresaStanovanja.Grad,
            AdresaStanovanja.Drzava,
            KontaktTelefon,
            EmailAdresa,
            BrojIndeksa.OznakaSmera,
            BrojIndeksa.BrojUpisa.ToString(),
            BrojIndeksa.GodinaUpisa.ToString(),
            TrenutnaGodinaStudija.ToString(),
            Status.ToString(),
            ProsecnaOcena.ToString("F2"),
            string.Join(";", SpisakPolozenihIspita),
            string.Join(";", SpisakNepolozenihPredmeta)
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Prezime = values[1];
        Ime = values[2];
        DatumRodjenja = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);
        AdresaStanovanja = new Adresa
        {
            Id = int.Parse(values[4]),
            Ulica = values[5],
            Broj = int.Parse(values[6]),
            Grad = values[7],
            Drzava = values[8]
        };
        KontaktTelefon = values[9];
        EmailAdresa = values[10];
        BrojIndeksa = new Indeks
        {
            OznakaSmera = values[11],
            BrojUpisa = int.Parse(values[12]),
            GodinaUpisa = int.Parse(values[13])
        };
        TrenutnaGodinaStudija = int.Parse(values[14]);
        Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), values[15]);
        ProsecnaOcena = double.Parse(values[16], null);

        //SpisakPolozenihIspita = values[17].Split(';').Select(sifra => new Predmet { SifraPredmeta = sifra }).ToList();
        //SpisakNepolozenihPredmeta = values[18].Split(';').Select(sifra => new Predmet { SifraPredmeta = sifra }).ToList();
    }
}
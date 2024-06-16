/*
Adresa (#adresa)
Ulica
Broj
Grad
Država
*/

using System;

namespace StudentskaSluzba.Model;
using StudentskaSluzba.Serialization;
using System.Xml.Linq;

public class Adresa : ISerializable, IComparable<Adresa>
{ 
    public int Id { get; set; }
    public string Ulica {get; set;}
    public int Broj {get; set;}
    public string Grad {get; set;}
    public string Drzava {get; set;}

    public Adresa()
    {
    }
    public Adresa(int id, string ulica, int broj, string grad, string drzava)
    {
        Id = id;
        Ulica = ulica;
        Broj = broj;
        Grad = grad;
        Drzava = drzava;
    }

    public Adresa(string ulica, int broj, string grad, string drzava)
    {
        Ulica = ulica;
        Broj = broj;
        Grad = grad;
        Drzava = drzava;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Ulica: {Ulica} | Broj: {Broj} | Grad: {Grad} | Drzava: {Drzava} |";
    }
    public string[] ToCSV()
    {
        string[] csvValues =
       {
            Id.ToString(),
            Ulica,
            Broj.ToString(),
            Grad,
            Drzava
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Ulica = values[1];
        Broj = int.Parse(values[2]);
        Grad = values[3];
        Drzava = values[4]; 
    }

    public Adresa UnesiAdresu()
    {
        Adresa adresa = new Adresa();

        System.Console.WriteLine("Unesite ulicu: ");
        adresa.Ulica = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite broj: ");
        adresa.Broj = Convert.ToInt32(System.Console.ReadLine() ?? string.Empty);

        System.Console.WriteLine("Unesite grad: ");
        adresa.Grad = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite državu: ");
        adresa.Drzava = System.Console.ReadLine() ?? string.Empty;

        return adresa;
    }

    public int CompareTo(Adresa? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var ulicaComparison = string.Compare(Ulica, other.Ulica, StringComparison.Ordinal);
        if (ulicaComparison != 0) return ulicaComparison;
        var brojComparison = Broj.CompareTo(other.Broj);
        if (brojComparison != 0) return brojComparison;
        var gradComparison = string.Compare(Grad, other.Grad, StringComparison.Ordinal);
        if (gradComparison != 0) return gradComparison;
        return string.Compare(Drzava, other.Drzava, StringComparison.Ordinal);
    }
}
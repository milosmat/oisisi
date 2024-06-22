using StudentskaSluzba.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StudentskaSluzba.Model
{
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
            return $"{Id}|{Prezime}|{Ime}|{TrenutnaGodinaStudija}|{Status}|{ProsecnaOcena}";
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
                ProsecnaOcena.ToString(),
                string.Join(";", SpisakPolozenihIspita),
                string.Join(";", SpisakNepolozenihPredmeta)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length < 19)
            {
                Debug.WriteLine("CSV row does not have enough elements.");
                return;
            }
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
            Debug.WriteLine($"{values[16]}");
            SpisakPolozenihIspita = new List<Predmet>();
            if (!string.IsNullOrWhiteSpace(values[17]))
            {
                Debug.WriteLine($"{values[17]}");
                foreach (var se in values[17].Split(";"))
                {
                    Debug.WriteLine($"{ se}");
                    var tmpPred = se.Split('|');
                    if (tmpPred.Length == 6)
                    {
                        SpisakPolozenihIspita.Add(new Predmet()
                        {
                            SifraPredmeta = tmpPred[0],
                            NazivPredmeta = tmpPred[1],
                            Semestar = Enum.Parse<SemestarEnum>(tmpPred[2]),
                            GodinaStudija = int.Parse(tmpPred[3]),
                            PredmetniProfesor = int.Parse(tmpPred[4]) != -1 ? new() { Id = int.Parse(tmpPred[4]) } : null,
                            BrojESPB = int.Parse(tmpPred[5])
                        });
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid format for passed subject: {se}");
                    }
                }
            }

            SpisakNepolozenihPredmeta = new List<Predmet>();
            if (!string.IsNullOrWhiteSpace(values[18]))
            {
                Debug.WriteLine($"{values[18]}");
                foreach (var se in values[18].Split(";"))
                {
                    var tmpPred = se.Split('|');
                    if (tmpPred.Length == 6)
                    {
                        SpisakNepolozenihPredmeta.Add(new Predmet()
                        {
                            SifraPredmeta = tmpPred[0],
                            NazivPredmeta = tmpPred[1],
                            Semestar = Enum.Parse<SemestarEnum>(tmpPred[2]),
                            GodinaStudija = int.Parse(tmpPred[3]),
                            PredmetniProfesor = int.Parse(tmpPred[4]) != -1 ? new() { Id = int.Parse(tmpPred[4]) } : null,
                            BrojESPB = int.Parse(tmpPred[5])
                        });
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid format for failed subject: {se}");
                    }
                }
            }

            // Provera učitanih podataka
            Debug.WriteLine($"Učitan student: {Ime} {Prezime}");
            Debug.WriteLine("Položeni ispiti:");
            foreach (var p in SpisakPolozenihIspita)
            {
                Debug.WriteLine($" - {p.SifraPredmeta}: {p.NazivPredmeta}");
            }
            Debug.WriteLine("Nepoloženi predmeti:");
            foreach (var p in SpisakNepolozenihPredmeta)
            {
                Debug.WriteLine($" - {p.SifraPredmeta}: {p.NazivPredmeta}");
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Student) return false;
            var tmp = obj as Student;
            if (tmp.Id == this.Id) return true;
            return false;
        }
    }
}

using CLI.DAO;
using CLI.Service;
using StudentskaSluzba.Model;

namespace StudentskaSluzba.Service;

public class CRUDEntitetaService
{
    private static readonly StudentDAO StudentDao = new ();
    private static readonly ProfesorDAO ProfesorDao = new ();
    private static readonly PredmetDAO PredmetDao = new ();
    private static readonly AdresaDAO AdresaDao = new ();
    private static readonly IndeksDAO IndeksDao = new ();
    private static readonly OcenaNaIspituDAO Ocena = new();
    public static bool DodajStudenta(Student student)
    {
        Student s = StudentDao.DodajStudenta(student);
        return s.Id > -1;
    }

    public static Adresa DodajAdresu(Adresa a)
    {
        return AdresaDao.dodajAdresu(a);
    }

    public static Indeks DodajIndeks(Indeks i)
    {
        return IndeksDao.dodajIndeks(i);
    }

    public static bool DodajProfesora(Profesor profesor)
    {
        Profesor p = ProfesorDao.DodajProfesora(profesor);
        return p.Id > -1;
    }

    public static bool DodajPredmet(Predmet predmet)
    {
        Predmet p = PredmetDao.DodajPredmet(predmet);
        return p.SifraPredmeta != null;
    }

    public static bool IzmeniStudenta(Student student)
    {
        try
        {
            StudentDao.AzurirajStudenta(student);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IzmeniProfesora(Profesor profesor)
    {
        try
        {
            ProfesorDao.AzurirajProfesora(profesor);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IzmeniPredmet(Predmet predmet)
    {
        try
        {
            
            PredmetDao.AzurirajPredmet(predmet);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void PonistiOcenu(string predmet, int studentId)
    {
        try
        {
            // Pronađi ocenu koju treba poništiti
            OcenaNaIspitu tmpOcena = Ocena.UzmiSveOceneNaIspitu().Find(o => o.Predmet.SifraPredmeta == predmet && o.StudentKojiJePolozio.Id == studentId);

            if (tmpOcena != null)
            {
                // Izbriši ocenu iz baze podataka
                Ocena.IzbrisiOcenuNaIspitu(studentId, predmet);

                // Pronađi studenta i ažuriraj njegove liste položenih i nepoloženih predmeta
                Student tmp = StudentDao.UzmiStudentaPoID(studentId);
                if (tmp != null)
                {
                    // Ukloni predmet iz liste položenih ispita
                    Predmet polozeniPredmet = tmp.SpisakPolozenihIspita.Find(p => p.SifraPredmeta == predmet);
                    if (polozeniPredmet != null)
                    {
                        tmp.SpisakPolozenihIspita.Remove(polozeniPredmet);
                        tmp.SpisakNepolozenihPredmeta.Add(polozeniPredmet);
                    }

                    // Ažuriraj predmete na osnovu promene statusa
                    Predmet tmpP = PredmetService.GetByid(predmet);
                    if (tmpP != null)
                    {
                        tmpP.SpisakStudenataPolozili.RemoveAll(s => s.Id == tmp.Id);
                        tmpP.SpisakStudenataNisuPolozili.Add(tmp);

                        PredmetDao.AzurirajPredmet(tmpP);
                    }

                    // Ažuriraj studenta u bazi podataka
                    StudentDao.AzurirajStudenta(tmp);
                }
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Greška prilikom brisanja ocene!\n" + e.Message);
        }
    }


    public static bool ObrisiOcenu(String predmet, int studentId)
    {
        try
        {
            Ocena.IzbrisiOcenuNaIspitu(studentId, predmet);
            Student tmp = StudentDao.UzmiStudentaPoID(studentId);
            if (!tmp.SpisakPolozenihIspita.Remove(PredmetService.GetByid(predmet)))
                tmp.SpisakNepolozenihPredmeta.Remove(PredmetService.GetByid(predmet));
            StudentDao.AzurirajStudenta(tmp);
            Predmet tmpP = PredmetService.GetByid(predmet);
            if (!tmpP.SpisakStudenataPolozili.Remove(tmp))
                tmpP.SpisakStudenataNisuPolozili.Remove(tmp);
            PredmetDao.AzurirajPredmet(tmpP);
            return true;
        }
        catch
        {
            System.Console.Error.WriteLine("Greška prilikom brisanja ocene");
            return false;
        }
    }
    public static bool DodajOcenuZaPredmet(Predmet predmet, Student student, int ocena, DateTime datum)
    {
        try
        {
            var ocenaNaIspitu = new OcenaNaIspitu()
            {
                StudentKojiJePolozio = student,
                Predmet = predmet,
                BrojcanaVrednostOcene = ocena,
                DatumPolaganjaIspita = datum
            };

            student.SpisakPolozenihIspita.Add(predmet);
            student.SpisakNepolozenihPredmeta.Remove(predmet);
            predmet.SpisakStudenataPolozili.Add(student);
            predmet.SpisakStudenataNisuPolozili.Remove(student);

            var ocenaDao = new OcenaNaIspituDAO();
            ocenaDao.DodajOcenuNaIspitu(ocenaNaIspitu);
            StudentDao.AzurirajStudenta(student);
            PredmetDao.AzurirajPredmet(predmet);

            System.Console.WriteLine("Ocena uspešno dodata za predmet: " + predmet.SifraPredmeta);
            return true;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Greška prilikom dodavanja ocene za predmet: " + e.Message);
            return false;
        }
    }
    public static bool DodajPredmetStudentu(Predmet predmet, Student student)
    {
        try
        {
            // Provera da li predmet već postoji u listi nepoloženih predmeta
            if (!student.SpisakNepolozenihPredmeta.Any(p => p.SifraPredmeta == predmet.SifraPredmeta))
            {
                student.SpisakNepolozenihPredmeta.Add(predmet);
                predmet.SpisakStudenataNisuPolozili.Add(student);

                StudentDao.AzurirajStudenta(student);
                PredmetDao.AzurirajPredmet(predmet);

                System.Console.WriteLine("Predmet uspešno dodat studentu: " + student.Id);
                return true;
            }
            else
            {
                System.Console.WriteLine("Predmet je već dodat studentu.");
                return false;
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Greška prilikom dodavanja predmeta studentu: " + e.Message);
            return false;
        }
    }



    public static bool DodajPredmetProfesoru(Profesor profesor, Predmet predmet)
    {
        try
        {
            profesor.SpisakPredmeta.Add(predmet);
            predmet.PredmetniProfesor = profesor;
            PredmetDao.AzurirajPredmet(predmet);
            ProfesorDao.AzurirajProfesora(profesor);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static bool ObrisiPredmetProfesoru(Profesor editProfesor, Predmet selectedPredmet)
    {
        try
        {
            selectedPredmet.PredmetniProfesor = null;
            editProfesor.SpisakPredmeta.Remove(selectedPredmet);
            PredmetDao.AzurirajPredmet(selectedPredmet);
            ProfesorDao.AzurirajProfesora(editProfesor);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
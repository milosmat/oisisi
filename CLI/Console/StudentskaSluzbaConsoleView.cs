/*using System;
using System.Collections.Generic;
using System.Linq;
using CLI.DAO;
using StudentskaSluzba.Model;

namespace StudentskaSluzba.Console;


 * Klasa koju koristimo za interakciju sa korisnikom.
 * Ova klasa sadrži glavnu petlju programa, i učitava
 * i ispisuje podatke korisniku.
 * Ova klasa NE TREBA da sadrži poslovnu logiku!
 * Sva poslovna logika se nalazu u DAO klasi i ova
 * klasa delegira DAO klasi svu poslovnu logiku i
 * prosleđuje joj sve potrebne podatke.
 
class StudentskaSluzbaConsoleView
{
    private readonly AdresaDAO _adresaDao;
    private readonly IndeksDAO _indeksDao;
    private readonly KatedraDAO _katedraDao;
    private readonly OcenaNaIspituDAO _ocenaNaIspituDao;
    private readonly PredmetDAO _predmetDao;
    private readonly ProfesorDAO _profesorDao;
    private readonly StudentDAO _studentDao;

    public StudentskaSluzbaConsoleView(AdresaDAO adresaDao, IndeksDAO indeksDao, KatedraDAO katedraDao, OcenaNaIspituDAO ocenaNaIspituDao, PredmetDAO predmetDao, ProfesorDAO profesorDao, StudentDAO studentDao)
    {
        _adresaDao = adresaDao;
        _indeksDao = indeksDao;
        _katedraDao = katedraDao;
        _ocenaNaIspituDao = ocenaNaIspituDao;
        _predmetDao = predmetDao;
        _profesorDao = profesorDao;
        _studentDao = studentDao;
    }

    public void RunMainMenu()
    {
        while (true)
        {
            ShowMainMenu();
            string userInput = System.Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandleMainMenuInput(userInput);
        }
    }

    private void ShowMainMenu()
    {
        System.Console.WriteLine("\nIzaberite opciju: ");
        System.Console.WriteLine("1: Profesori");
        System.Console.WriteLine("2: Studenti");
        System.Console.WriteLine("3: Predmeti");
        System.Console.WriteLine("4: Katedra");
        System.Console.WriteLine("0: Zavrsi");
        /*
        System.Console.WriteLine("4: Remove vehicle");
        System.Console.WriteLine("5: Show and sort vehicles");
        System.Console.WriteLine("0: Close");
    }

    private void HandleMainMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                RunProfMenu();
                break;
            case "2":
                RunStudMenu();
                break;
            case "3":
                RunPredMenu();
                break;
            case "4":
                RunKatMenu();
                break;
        }
    }
    public void RunProfMenu()
    {
        while (true)
        {
            ShowProfMenu();
            string userInput = System.Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandleProfMenuInput(userInput);
        }
    }
    private void ShowProfMenu()
    {
        System.Console.WriteLine("\nIzaberiteOpciju: ");
        System.Console.WriteLine("1: Prikazi sve profesore");
        System.Console.WriteLine("2: Dodaj profesora");
        System.Console.WriteLine("3: Azuriraj profesora");
        System.Console.WriteLine("4: Izbrisi profesora");
        System.Console.WriteLine("0: Zavrsi");
    }
    private void HandleProfMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrikaziSveProf();
                break;
            case "2":
                DodajProf();
                break;
            case "3":
                AzurirajProf();
                break;
            case "4":
                IzbrisiProf();
                break;
        }
    }

    private void PrikaziSveProf()
    {
        PrintProf(_profesorDao.UzmiSveProfesore());
    }
    private void PrintProf(List<Profesor> profesori)
    {
        System.Console.WriteLine("Profesori: ");
        string header = $"ID {""} | Prezime: {""} | Ime: {""} | Datum rodjenja: {""} | Adresa stanovanja: {""} | Kontakt telefon: {""} | Email: {""} | Broj licne karte: {""} | Zvanje: {""} | Godine staza: {""} | Spisak predmeta: {""} |";
        System.Console.WriteLine(header);
        foreach (Profesor p in profesori)
        {
            System.Console.WriteLine(p);
        }
    }

    private void DodajProf()
    {
        Profesor profesor = InputProf();
        _profesorDao.DodajProfesora(profesor);
        System.Console.WriteLine("Profesor dodat");
    }

    private Profesor InputProf()
    {

        System.Console.WriteLine("Unesite ime profesora: ");
        string ime = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite prezime profesora: ");
        string prezime = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite datum rodjenja profesora (format: yyyy-MM-dd): ");
        DateTime datumRodjenja = DateTime.ParseExact(System.Console.ReadLine(), "yyyy-MM-dd", null);

        System.Console.WriteLine("Unesite adresu stanovanja profesora: ");
        Adresa adr = new Adresa();
        adr.UnesiAdresu(); // Trebate implementirati unos adrese prema vašim potrebama

        System.Console.WriteLine("Unesite kontakt telefon profesora: ");
        string telefon = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite email adresu profesora: ");
        string email = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite broj lične karte profesora: ");
        string licna = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite zvanje profesora: ");
        string zvanje = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite godine staža profesora: ");
        int staz = Convert.ToInt32(System.Console.ReadLine());

        // Kreirajte objekat profesora koristeći konstruktor
        Profesor noviProfesor = new Profesor(ime, prezime, datumRodjenja, adr, telefon, email, licna, zvanje, staz);

        System.Console.WriteLine("Unesite predmete na kojima profesor predaje: ");
        noviProfesor.UnesiPredmete();

        return noviProfesor;
    }


    private void AzurirajProf()
    {
        int IDProf = inputIdProf();
        Profesor prof = InputProf();
        prof.Id = IDProf;
        Profesor azuriranProf = _profesorDao.AzurirajProfesora(prof);
        if (azuriranProf == null)
        {
            System.Console.WriteLine("Profesor nije pronadjen!");
            return;
        }
        System.Console.WriteLine("Profesor azuriran!");
    }
    private void IzbrisiProf()
    {
        int idProf = inputIdProf();
        Profesor? obrisanProf = _profesorDao.IzbrisiProfesora(idProf);
        if (obrisanProf == null)
        {
            System.Console.WriteLine("Profesor nije pronadjen!");
            return;
        }
        System.Console.WriteLine("Profesor izbrisan!");
    }

    private int inputIdProf()
    {
        System.Console.WriteLine("Unesite id profesora: ");
        PrikaziSveProf();
        return ConsoleViewUtils.SafeInputInt();
    }

    public void RunStudMenu()
    {
        while (true)
        {
            ShowStudMenu();
            string userInput = System.Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandleStudMenuInput(userInput);
        }
    }

    private void ShowStudMenu()
    {
        System.Console.WriteLine("\nIzaberiteOpciju: ");
        System.Console.WriteLine("1: Prikazi sve studente");
        System.Console.WriteLine("2: Dodaj studenta");
        System.Console.WriteLine("3: Azuriraj studenta");
        System.Console.WriteLine("4: Izbrisi studenta");
        System.Console.WriteLine("0: Zavrsi");
    }

    private void HandleStudMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrikaziSveStud();
                break;
            case "2":
                DodajStud();
                break;
            case "3":
                AzurirajStud();
                break;
            case "4":
                IzbrisiStud();
                break;
        }
    }

    private void PrikaziSveStud()
    {
        PrintStudent(_studentDao.UzmiSveStudente());
    }

    private void PrintStudent(List<Student> studenti)
    {
        System.Console.WriteLine("Studenti: ");
        string header = $"ID {""} | Prezime: {""} | Ime: {""} | Datum rodjenja: {""} | Adresa stanovanja: {""} | Kontakt telefon: {""} | Email: {""} | Broj indeksa: {""} | Godina studija: {""} | Status: {""} | Prosecna ocena: {""} | Polozeni ispiti: {""} | Nepolozeni predmeti: {""} |";
        System.Console.WriteLine(header);
        foreach (Student s in studenti)
        {
            System.Console.WriteLine(s);
        }
    }

    private void DodajStud()
    {
        Student student = InputStud();
        _studentDao.DodajStudenta(student);
        System.Console.WriteLine("Student dodat");
    }

    private Student InputStud()
    {
        System.Console.WriteLine("Unesite ime studenta: ");
        string ime = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite prezime studenta: ");
        string prezime = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite datum rodjenja studenta (format: yyyy-MM-dd): ");
        DateTime datumRodjenja = DateTime.ParseExact(System.Console.ReadLine(), "yyyy-MM-dd", null);

        System.Console.WriteLine("Unesite adresu stanovanja studenta: ");
        Adresa adr = new Adresa();
        adr.UnesiAdresu();

        System.Console.WriteLine("Unesite kontakt telefon studenta: ");
        string telefon = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite email adresu studenta: ");
        string email = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite broj indeksa studenta: ");
        Indeks brojIndeksa = new Indeks();
        brojIndeksa.unesiIndeks();

        System.Console.WriteLine("Unesite trenutnu godinu studija studenta: ");
        int trenutnaGodina = Convert.ToInt32(System.Console.ReadLine());

        System.Console.WriteLine("Unesite status studenta (Budzet/Samofinansiranje): ");
        StatusEnum status;
        Enum.TryParse(System.Console.ReadLine(), true, out status);

        System.Console.WriteLine("Unesite prosecnu ocenu studenta: ");
        double prosecnaOcena = Convert.ToDouble(System.Console.ReadLine());

        // Kreirajte objekat studenta koristeći konstruktor
        Student noviStudent = new Student(ime, prezime, datumRodjenja, adr, telefon, email, brojIndeksa, trenutnaGodina, status, prosecnaOcena);
        //GRESKE SA UNOSOM
        System.Console.WriteLine("Unesite položene ispite studenta (odvojene tačkom-zarez, završite unos enterom): ");

        //System.Console.WriteLine("Unesite nepoložene predmete studenta (odvojene tačkom-zarez, završite unos enterom): ");
        string unosNepolozenih = System.Console.ReadLine();
        //noviStudent.SpisakNepolozenihPredmeta.AddRange(unosNepolozenih.Split(';'));


        return noviStudent;
    }

    private void AzurirajStud()
    {
        int IDStudenta = InputIdStud();
        Student student = InputStud();
        student.Id = IDStudenta;
        Student azuriranStudent = _studentDao.AzurirajStudenta(student);
        if (azuriranStudent == null)
        {
            System.Console.WriteLine("Student nije pronadjen!");
            return;
        }
        System.Console.WriteLine("Student azuriran!");
    }

    private void IzbrisiStud()
    {
        int idStudenta = InputIdStud();
        Student? obrisanStudent = _studentDao.IzbrisiStudenta(idStudenta);
        if (obrisanStudent == null)
        {
            System.Console.WriteLine("Student nije pronadjen!");
            return;
        }
        System.Console.WriteLine("Student izbrisan!");
    }

    private int InputIdStud()
    {
        System.Console.WriteLine("Unesite ID studenta: ");
        PrikaziSveStud();
        return ConsoleViewUtils.SafeInputInt();
    }

    public void RunPredMenu()
    {
        while (true)
        {
            ShowPredMenu();
            string userInput = System.Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandlePredMenuInput(userInput);
        }
    }

    private void ShowPredMenu()
    {
        System.Console.WriteLine("\nIzaberiteOpciju: ");
        System.Console.WriteLine("1: Prikazi sve predmete");
        System.Console.WriteLine("2: Dodaj predmet");
        System.Console.WriteLine("3: Azuriraj predmet");
        System.Console.WriteLine("4: Izbrisi predmet");
        System.Console.WriteLine("0: Zavrsi");
    }

    private void HandlePredMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrikaziSvePred();
                break;
            case "2":
                DodajPred();
                break;
            case "3":
                AzurirajPred();
                break;
            case "4":
                IzbrisiPred();
                break;
        }
    }

    private void PrikaziSvePred()
    {
        PrintPred(_predmetDao.UzmiSvePredmete());
    }

    private void PrintPred(List<Predmet> predmeti)
    {
        System.Console.WriteLine("Predmeti: ");
        string header = $"Sifra predmeta {""} | Naziv predmeta: {""} | Semestar: {""} | Godina studija: {""} | Predmetni profesor: {""} | Broj ESPB: {""} | Polozeni studenti: {""} | Nepolozeni studenti: {""} |";
        System.Console.WriteLine(header);
        foreach (Predmet p in predmeti)
        {
            System.Console.WriteLine(p);
        }
    }

    private void DodajPred()
    {
        Predmet predmet = InputPred();
        _predmetDao.DodajPredmet(predmet);
        System.Console.WriteLine("Predmet dodat");
    }
    private Predmet InputPred()
    {
        System.Console.WriteLine("Unesite šifru predmeta: ");
        string sifra = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite naziv predmeta: ");
        string naziv = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite semestar (Letnji/Zimski): ");
        SemestarEnum semestar;
        Enum.TryParse(System.Console.ReadLine(), true, out semestar);

        System.Console.WriteLine("Unesite godinu studija u kojoj se predmet izvodi: ");
        int godinaStudija = Convert.ToInt32(System.Console.ReadLine());

        System.Console.WriteLine("Unesite profesora koji predaje predmet (ID): ");
        PrikaziSveProf();
        int idProfesora = Convert.ToInt32(System.Console.ReadLine());
        // Trebalo bi proveriti da li profesor sa unetim ID-jem postoji u sistemu
        Profesor predmetniProfesor = _profesorDao.UzmiProfesoraPoID(idProfesora);

        if (predmetniProfesor == null)
        {
            System.Console.WriteLine("Profesor sa unetim ID-jem nije pronađen u sistemu. Pokušajte ponovo.");
            return null; // Ako profesor nije pronađen, možete vratiti null ili neki drugi signal o grešci.
        }

        System.Console.WriteLine($"Izabrani profesor: {predmetniProfesor.Ime} {predmetniProfesor.Prezime}");

        System.Console.WriteLine("Unesite broj ESPB bodova za predmet: ");
        int brojESPB = Convert.ToInt32(System.Console.ReadLine());

        // Kreirajte objekat predmeta koristeći konstruktor
        //Predmet noviPredmet = new Predmet(sifra, naziv, semestar, godinaStudija, predmetniProfesor, brojESPB);

        PrikaziSveStud();
        System.Console.WriteLine("Unesite studente koji su položili predmet (odvojene tačkom-zarez, završite unos enterom): ");

        string unosPolozenih = System.Console.ReadLine();
        //noviPredmet.SpisakStudenataPolozili.AddRange(unosPolozenih.Split(';'));

        System.Console.WriteLine("Unesite studente koji nisu položili predmet (odvojene tačkom-zarez, završite unos enterom): ");
        string unosNepolozenih = System.Console.ReadLine();
        //noviPredmet.SpisakStudenataNisuPolozili.AddRange(unosNepolozenih.Split(';'));

        return noviPredmet;
    }

    private string GetImePrezimeProfesora(int idProfesora)
    {
        Profesor profesor = _profesorDao.UzmiProfesoraPoID(idProfesora);
        if (profesor != null)
        {
            return $"{profesor.Ime} {profesor.Prezime}";
        }
        return "Nepoznato";
    }

    private void AzurirajPred()
    {
        string sifraPredmeta = InputSifraPred();
        Predmet predmet = InputPred();

        // Azuriranje predmeta
        predmet.SifraPredmeta = sifraPredmeta;
        Predmet azuriranPredmet = _predmetDao.AzurirajPredmet(predmet);

        if (azuriranPredmet == null)
        {
            System.Console.WriteLine("Predmet nije pronadjen!");
            return;
        }

        System.Console.WriteLine("Predmet azuriran!");
    }
    private void IzbrisiPred()
    {
        string sifraPredmeta = InputSifraPred();
        Predmet obrisanPredmet = _predmetDao.IzbrisiPredmet(sifraPredmeta);

        if (obrisanPredmet == null)
        {
            System.Console.WriteLine("Predmet nije pronadjen!");
            return;
        }

        System.Console.WriteLine("Predmet izbrisan!");
    }

    private string InputSifraPred()
    {
        System.Console.WriteLine("Unesite šifru predmeta: ");
        PrikaziSvePred(); // PrikaziSvePredmete bi trebalo da bude metoda koja prikazuje sve predmete
        return System.Console.ReadLine();
    }

    public void RunKatMenu()
    {
        while (true)
        {
            ShowKatMenu();
            string userInput = System.Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandleKatMenuInput(userInput);
        }
    }
    private void ShowKatMenu()
    {
        System.Console.WriteLine("\nIzaberiteOpciju: ");
        System.Console.WriteLine("1: Prikazi sve katedre");
        System.Console.WriteLine("2: Dodaj katedru");
        System.Console.WriteLine("3: Azuriraj katedru");
        System.Console.WriteLine("4: Izbrisi katedru");
        System.Console.WriteLine("0: Zavrsi");
    }
    private void HandleKatMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrikaziSveKat();
                break;
            case "2":
                DodajKat();
                break;
            case "3":
                AzurirajKat();
                break;
            case "4":
                IzbrisiKat();
                break;
        }
    }
    private void PrikaziSveKat()
    {
        PrintKat(_katedraDao.UzmiSveKatedre());
    }

    private void PrintKat(List<Katedra> katedre)
    {
        System.Console.WriteLine("Katedre: ");
        string header = $"Sifra katedre {""} | Naziv katedre: {""} | Sef katedre: {""} | Spisak profesora: {""} |";
        System.Console.WriteLine(header);
        foreach (Katedra k in katedre)
        {
            System.Console.WriteLine(k);
        }
    }

    private void DodajKat()
    {
        Katedra katedra = InputKat();
        _katedraDao.DodajKatedru(katedra);
        System.Console.WriteLine("Katedra dodata");
    }

    private Katedra InputKat()
    {
        System.Console.WriteLine("Unesite šifru katedre: ");
        string sifraKatedre = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite naziv katedre: ");
        string nazivKatedre = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Unesite šefa katedre (ID): ");
        int idSefaKatedre = Convert.ToInt32(System.Console.ReadLine());
        // Trebalo bi proveriti da li profesor sa unetim ID-jem postoji u sistemu
        PrikaziSveProf();
        Profesor sefKatedre = _profesorDao.UzmiProfesoraPoID(idSefaKatedre);

        if (sefKatedre == null)
        {
            System.Console.WriteLine("Profesor sa unetim ID-jem nije pronađen u sistemu. Pokušajte ponovo.");
            return null; // Ako profesor nije pronađen, možete vratiti null ili neki drugi signal o grešci.
        }

        System.Console.WriteLine("Unesite profesore koji su članovi katedre (ID-jevi, odvojeni tačkom-zarez): ");
        string unosProfesora = System.Console.ReadLine();
        List<Profesor> spisakProfesora = unosProfesora.Split(';').Select(id => _profesorDao.UzmiProfesoraPoID(Convert.ToInt32(id))).ToList();

        // Kreirajte objekat katedre koristeći konstruktor
        Katedra novaKatedra = new Katedra(sifraKatedre, nazivKatedre, sefKatedre);
        novaKatedra.SpisakProfesora = spisakProfesora;

        return novaKatedra;
    }

    private void AzurirajKat()
    {
        string sifraKatedre = InputSifraKat();
        Katedra katedra = InputKat();

        // Azuriranje katedre
        katedra.SifraKatedre = sifraKatedre;
        Katedra azuriranaKatedra = _katedraDao.AzurirajKatedru(katedra);

        if (azuriranaKatedra == null)
        {
            System.Console.WriteLine("Katedra nije pronadjena!");
            return;
        }

        System.Console.WriteLine("Katedra azurirana!");
    }

    private void IzbrisiKat()
    {
        string sifraKatedre = InputSifraKat();
        Katedra obrisanaKatedra = _katedraDao.IzbrisiKatedru(sifraKatedre);

        if (obrisanaKatedra == null)
        {
            System.Console.WriteLine("Katedra nije pronadjena!");
            return;
        }

        System.Console.WriteLine("Katedra izbrisana!");
    }

    private string InputSifraKat()
    {
        System.Console.WriteLine("Unesite šifru katedre: ");
        PrikaziSveKat(); // PrikaziSveKatedre bi trebalo da bude metoda koja prikazuje sve katedre
        return System.Console.ReadLine();
    }

}
*/
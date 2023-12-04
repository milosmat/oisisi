using CLI.DAO;
using StudentskaSluzba.Model;
using System.Collections.Generic;

namespace StudentskaSluzba.Console;

/*
 * Klasa koju koristimo za interakciju sa korisnikom.
 * Ova klasa sadrži glavnu petlju programa, i učitava
 * i ispisuje podatke korisniku.
 * Ova klasa NE TREBA da sadrži poslovnu logiku!
 * Sva poslovna logika se nalazu u DAO klasi i ova
 * klasa delegira DAO klasi svu poslovnu logiku i
 * prosleđuje joj sve potrebne podatke.
 */
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
        System.Console.WriteLine("0: Close");*/
    }

    private void HandleMainMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                RunProfMenu();
                break;
            case "2":
                //ShowStudMenu();
                break;
            case "3":
                //ShowPredMenu();
                break;
            case "4":
                //ShowKatMenu();
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
        Adresa adresa = new Adresa(); // Trebate implementirati unos adrese prema vašim potrebama

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
        Profesor noviProfesor = new Profesor(ime, prezime, datumRodjenja, adresa, telefon, email, licna, zvanje, staz);

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
        if(azuriranProf == null) 
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
        if(obrisanProf == null)
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
}

/*private void UpdateVehicle()
    {
        int id = InputId();
        Vehicle vehicle = InputVehicle();
        vehicle.Id = id;
        Vehicle? updatedVehicle = _vehiclesDao.UpdateVehicle(vehicle);
        if (updatedVehicle == null)
        {
            System.Console.WriteLine("Vehicle not found");
            return;
        }

        System.Console.WriteLine("Vehicle updated");
    }

    private int InputId()
    {
        System.Console.WriteLine("Enter vehicle id: ");
        return ConsoleViewUtils.SafeInputInt();
    }*/

/* private void RemoveVehicle()
   {
       int id = InputId();
       Vehicle? removedVehicle = _vehiclesDao.RemoveVehicle(id);
       if (removedVehicle is null)
       {
           System.Console.WriteLine("Vehicle not found");
           return;
       }

       System.Console.WriteLine("Vehicle removed");
   }*/
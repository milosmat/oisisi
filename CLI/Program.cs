// See https://aka.ms/new-console-template for more information

using CLI.DAO;
using StudentskaSluzba.Console;

namespace StudentskaSluzba;

class Program
{
    static void Main()
    {
        ProfesorDAO profesor = new ProfesorDAO();
        AdresaDAO adresa = new AdresaDAO();
        IndeksDAO indeks = new IndeksDAO();
        KatedraDAO katedra = new KatedraDAO();
        OcenaNaIspituDAO ocenaNaIspitu = new OcenaNaIspituDAO();
        PredmetDAO predmet = new PredmetDAO();
        StudentDAO student = new StudentDAO();
        StudentskaSluzbaConsoleView view = new StudentskaSluzbaConsoleView(adresa, indeks, katedra, ocenaNaIspitu, predmet, profesor, student);
        view.RunMainMenu();
        /*VehiclesDAO vehicles = new VehiclesDAO();
        VehicleConsoleView view = new VehicleConsoleView(vehicles);
        view.RunMenu();*/
    }
}
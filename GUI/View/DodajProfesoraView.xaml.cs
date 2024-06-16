using System;
using System.Globalization;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class DodajProfesoraView : Window
{
    public event EventHandler? OnFinish;
    public DodajProfesoraView()
    {
        InitializeComponent();
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        var adr = TxtAdresaStanovanja.Text.Split(" ");
        var p = new Profesor()
        {
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.ParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture),
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojLicneKarte = TxtLicnaKarta.Text,
            GodineStaza = int.Parse(TxtGodineStaza.Text),
            EmailAdresa = TxtEmailAdresa.Text,
            AdresaStanovanja = new Adresa()
            {
                Drzava = adr[3],
                Grad = adr[2],
                Ulica = adr[0],
                Broj = int.Parse(adr[1])
            },
            Zvanje = TxtZvanje.Text
        };
        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = FindResource("AddFail") as string;
        var status = CRUDEntitetaService.DodajProfesora(p);
        MessageBox.Show(status ? success : fail,
            title);
        DialogResult = status;
        Close();
    }
    
    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
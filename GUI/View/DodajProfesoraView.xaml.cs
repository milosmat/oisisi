using System;
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
            DatumRodjenja = DateTime.Parse(TxtDatumRodjenja.Text),
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojLicneKarte = TxtLicnaKarta.Text,
            GodineStaza = int.Parse(TxtGodineStaza.Text),
            EmailAdresa = TxtEmailAdresa.Text,
            AdresaStanovanja = new Adresa()
            {
                Drzava = adr[2],
                Ulica = adr[0],
                Broj = int.Parse(adr[1])
            }
        };
        string? title = (string)Application.Current.FindResource("AddTitle");
        string? success = (string)Application.Current.FindResource("AddSuccess");
        string? fail = (string)Application.Current.FindResource("AddFail");
        
        MessageBox.Show(CRUDEntitetaService.DodajProfesora(p) ? success : fail,
            title);
        OnFinish?.Invoke(sender, e);
        Close();
    }
    
    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
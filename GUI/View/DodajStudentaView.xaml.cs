using System;
using System.Windows;
using StudentskaSluzba.Model;
using CLI.DAO;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class DodajStudentaView : Window
{
    public DodajStudentaView()
    {
        InitializeComponent();
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        var adr = TxtAdresaStanovanja.Text.Split(" ");
        Student s = new()
        {
            AdresaStanovanja = DodavanjeEntitetaService.DodajAdresu(new Adresa
            {
                Drzava = adr[2],
                Ulica = adr[0],
                Broj = int.Parse(adr[1])
            }),
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.Parse(TxtDatumRodjenja.Text),
            EmailAdresa = TxtEmailAdresa.Text,
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojIndeksa = DodavanjeEntitetaService.DodajIndeks(new Indeks
            {
                GodinaUpisa = int.Parse(TxtGodinaUpisa.Text),
                OznakaSmera = TxtBrojIndeksa.Text
            }),
            TrenutnaGodinaStudija = int.Parse("" + CmbTrenutnaGodinaStudija.SelectedItem),
            Status = CmbNacinFinansiranja.SelectedItem is StatusEnum ? (StatusEnum)CmbNacinFinansiranja.SelectedItem : StatusEnum.Budzet 
        };
        MessageBox.Show(DodavanjeEntitetaService.DodajStudenta(s) ? "Entitet uspešno dodat!" : "Entitet nije dodat!",
            "Dodavanje entiteta");
    }

    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
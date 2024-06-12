using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class DodajPredmetView : Window
{
    public event EventHandler? OnFinish;

    private List<Profesor> _profesors = ProfesorService.GetProfesors();

    public List<Profesor> Profesors
    {
        get => _profesors;
        set
        {
            _profesors = value;
            OnPropertyChanged();
        }
    }

    private Profesor _profesor = ProfesorService.GetProfesors().FirstOrDefault() ?? throw new InvalidOperationException();

    public Profesor SelectedProfesor
    {
        get => _profesor;
        set
        {
            _profesor = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public DodajPredmetView()
    {
        InitializeComponent();
    }
    
    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        Predmet p = new Predmet()
        {
            SifraPredmeta = TxtSifra.Text,
            NazivPredmeta = TxtNaziv.Text,
            PredmetniProfesor = SelectedProfesor,
            Semestar = ((ComboBoxItem)CmbSemestar.SelectedItem).Tag is SemestarEnum
                ? (SemestarEnum)((ComboBoxItem)CmbSemestar.SelectedItem).Tag
                : SemestarEnum.Letnji,
            GodinaStudija = int.Parse(TxtGodinaStudija.Text),
            BrojESPB = int.Parse(TxtEspb.Text)
        };
        string? title = (string)Application.Current.FindResource("AddTitle");
        string? success = (string)Application.Current.FindResource("AddSuccess");
        string? fail = (string)Application.Current.FindResource("AddFail");
        
        MessageBox.Show(DodavanjeEntitetaService.DodajPredmet(p) ? success : fail,
            title);
        OnFinish?.Invoke(sender, e);
        Close();
    }
    
    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
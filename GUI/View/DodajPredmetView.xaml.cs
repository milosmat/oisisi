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
        _profesors = ProfesorService.GetProfesors();
        Profesors = _profesors;
        InitializeComponent();
        DataContext = this;
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
        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = this.FindResource("AddFail") as string;
        
        MessageBox.Show(CRUDEntitetaService.DodajPredmet(p) ? success : fail,
            title);
        OnFinish?.Invoke(sender, e);
        Close();
    }
    
    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
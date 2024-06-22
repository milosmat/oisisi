using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class IzaberiPredmetDialog : Window
{
    private Predmet? _predmet;

    public Predmet? SelectedPredmet
    {
        get => _predmet;
        set
        {
            _predmet = value;
            OnPropertyChanged();
        }
    }
    
    private List<Predmet> _predmets;
    public List<Predmet> Predmets
    {
        get => _predmets;
        set
        {
            _predmets = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Student? _student;
    private Profesor? _profesor;
    public IzaberiPredmetDialog()
    {
        InitializeComponent();
        Predmets = PredmetService.GetPredmets();
        DataContext = this;
        if (Predmets == null || Predmets.Count == 0)
        {
            MessageBox.Show("Nema dostupnih predmeta.");
        }
    }

    public IzaberiPredmetDialog(Student student)
    {
        InitializeComponent();
        _student = student;
        _profesor = null;
        Predmets = PredmetService.GetPredmets();
        DataContext = this;
    }

    public IzaberiPredmetDialog(Profesor profesor)
    {
        InitializeComponent();
        _profesor = profesor;
        _student = null;
        Predmets = PredmetService.GetPredmets();
        DataContext = this;
    }


    private void SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedPredmet == null)
        {
            MessageBox.Show("Niste odabrali predmet.");
            return;
        }
        MessageBoxResult dlgRes = MessageBox.Show("Da li želite da dodelite ovaj predmet?", "Obaveštenje",
            MessageBoxButton.YesNo);

        if (dlgRes.Equals(MessageBoxResult.Yes) && SelectedPredmet != null)
        {
            if (_profesor == null)
                DialogResult = CRUDEntitetaService.DodajPredmetStudentu(SelectedPredmet, _student);
            else
                DialogResult = CRUDEntitetaService.DodajPredmetProfesoru(_profesor, SelectedPredmet);
            Close();
        }
    }
}
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class AddGradeView : Window
{
    private int[] _grades = { 5, 6, 7, 8, 9, 10 };

    public int[] Grades
    {
        get => _grades;
        set
        {
            _grades = value;
            OnPropertyChanged();
        }
    }

    private int? _grade;

    public int? SelectedGrade
    {
        get => _grade;
        set
        {
            _grade = value;
            OnPropertyChanged();
        }
    }

    private string _datum;

    public string Datum
    {
        get => _datum;
        set
        {
            _datum = value;
            OnPropertyChanged();
        }
    }

    private Predmet? _predmet;

    public Predmet? Predmet
    {
        get => _predmet;
        set
        {
            _predmet = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Student _student;

    public AddGradeView()
    {
        InitializeComponent();
        DataContext = this;
    }

    public AddGradeView(Predmet predmet, Student student)
    {
        InitializeComponent();
        Predmet = predmet;
        _student = student;
        DataContext = this;
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult =
            CRUDEntitetaService.DodajPredmetStudentu(Predmet!, _student, SelectedGrade ?? 0, DateTime.ParseExact(Datum, "yyyy-MM-dd", null));
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }
}
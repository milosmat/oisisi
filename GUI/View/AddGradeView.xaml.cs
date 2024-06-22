using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GUI.View;

public partial class AddGradeView : Window, INotifyPropertyChanged
{
    private int[] _grades = { 6, 7, 8, 9, 10 };

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

    private Student _student;
    private EditStudentView _editStudentView;

    public AddGradeView(EditStudentView editStudentView, Predmet predmet, Student student)
    {
        InitializeComponent();
        Predmet = predmet;
        _student = student;
        _editStudentView = editStudentView;
        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedGrade.HasValue && !string.IsNullOrEmpty(Datum))
        {
            var datumPolaganja = DateTime.ParseExact(Datum, "dd/MM/yyyy", null);
            this.DialogResult = CRUDEntitetaService.DodajOcenuZaPredmet(Predmet!, _student, SelectedGrade.Value, datumPolaganja);
            _editStudentView.UpdatePassedAndFailedSubjects();
            Close();
        }
        else
        {
            MessageBox.Show("Molimo unesite ocenu i datum.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }
}

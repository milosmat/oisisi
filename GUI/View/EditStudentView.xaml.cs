using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditStudentView : Window
{
    private Student _student;
    public Student EditStudent
    {
        get => _student;
        set
        {
            _student = value;
            OnPropertyChanged();
        }
    }
    
    private List<Predmet> _predmets;
    public List<Predmet> PredmetsNePolozeni
    {
        get => _predmets;
        set
        {
            EditStudent.SpisakNepolozenihPredmeta = value;
            _predmets = value;
            OnPropertyChanged();
        }
    }
    
    private List<Predmet> _predmetsPolozeni;
    public List<Predmet> PredmetsPolozeni
    {
        get => _predmetsPolozeni;
        set
        {
            EditStudent.SpisakPolozenihIspita = value;
            _predmetsPolozeni = value;
            OnPropertyChanged();
        }
    }

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
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EditStudentView()
    {
        InitializeComponent();
        DataContext = this;
    }
    
    public EditStudentView(Student selectedStud)
    {
        InitializeComponent();
        _student = selectedStud;
        EditStudent = _student;
        DataContext = this;
        CmbTrenutnaGodinaStudija.SelectedIndex = EditStudent.TrenutnaGodinaStudija - 1;
        CmbNacinFinansiranja.SelectedIndex = EditStudent.Status.Equals(StatusEnum.Budzet) ? 0 : 1;
        PredmetsNePolozeni = EditStudent.SpisakNepolozenihPredmeta;
        PredmetsPolozeni = EditStudent.SpisakPolozenihIspita;
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniStudenta(EditStudent);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }

    private void AddNewSubject_Click(object sender, RoutedEventArgs e)
    {
        IzaberiPredmetDialog izaberiPredmetDialog = new IzaberiPredmetDialog();
        if (izaberiPredmetDialog.ShowDialog() == true)
        {
            MessageBox.Show("Predmet uspešno dodat!");
        }
    }

    private void UndoPassedExam_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        MessageBoxResult res =  MessageBox.Show("Da li ste sigurni da hoćete da poništite ocenu?", "Upozorenje", MessageBoxButton.OKCancel);
        if (res.Equals(MessageBoxResult.OK))
        {
            CRUDEntitetaService.PonistiOcenu(SelectedPredmet.SifraPredmeta, EditStudent.Id);
        }
    }

    private void AddGrade_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        AddGradeView gradeView = new AddGradeView(SelectedPredmet, _student);
        if (gradeView.ShowDialog() == true)
        {
            MessageBox.Show("Ocena uspešno upisana");
        }
    }

    private void TabChangedEvent(object sender, SelectionChangedEventArgs e)
    {
        SelectedPredmet = null;
    }

    private void RemoveSubject_Click(object sender, RoutedEventArgs e)
    {
        if(SelectedPredmet == null) return;
        MessageBoxResult res =  MessageBox.Show("Da li ste sigurni da hoćete da poništite ocenu?", "Upozorenje", MessageBoxButton.OKCancel);
        if (res.Equals(MessageBoxResult.OK))
        {
            CRUDEntitetaService.PonistiOcenu(SelectedPredmet.SifraPredmeta, EditStudent.Id);
        }
    }
}
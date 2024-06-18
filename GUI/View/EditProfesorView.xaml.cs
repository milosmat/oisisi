using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditProfesorView : Window
{
    private Profesor _profesor;

    public Profesor EditProfesor
    {
        get => _profesor;
        set
        {
            _profesor = value;
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
    public EditProfesorView()
    {
        InitializeComponent();
        DataContext = this;
    }
    public EditProfesorView(Profesor selectedProf)
    {
        InitializeComponent();
        _profesor = selectedProf;
        EditProfesor = _profesor;
        DataContext = this;
    }
    
    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniProfestora(EditProfesor);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }

    private void AddSubject_Click(object sender, RoutedEventArgs e)
    {
        var predmetDialog = new IzaberiPredmetDialog(EditProfesor);
        if (predmetDialog.ShowDialog() == true)
            MessageBox.Show("Uspšeno dodat predmet!");
    }

    private void RemoveSubject_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        var dialogRes = MessageBox.Show("Da li ste sigurni?", "Ukloni predmet", MessageBoxButton.OKCancel);
        if(!dialogRes.Equals(MessageBoxResult.OK)) return;
        var res = CRUDEntitetaService.ObrisiPredmetProfesoru(EditProfesor, SelectedPredmet);
        MessageBox.Show(res ? "Uspešno skinut profesor sa predmeta" : "Došlo je do greške prilikom birsanja!");
    }
}
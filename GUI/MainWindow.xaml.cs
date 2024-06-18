using CLI.Service;
using GUI.View;
using StudentskaSluzba.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Student> FilteredStudents { get; set; }
        public ObservableCollection<Profesor> FilteredProfesors { get; set; }
        public ObservableCollection<Predmet> FilteredPredmets { get; set; }

        private ObservableCollection<Student> students;
        public ObservableCollection<Student> Students
        {
            get => students;
            set
            {
                students = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Predmet> predmets;
        public ObservableCollection<Predmet> Predmets
        {
            get => predmets;
            set
            {
                predmets = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Profesor> profesors;
        public ObservableCollection<Profesor> Profesors
        {
            get => profesors;
            set
            {
                profesors = value;
                OnPropertyChanged();
            }
        }

        private TabItem? selected;

        public TabItem? Selected
        {
            get => selected;
            set
            {
                selected = value;
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

        private Profesor? _profesor;

        public Profesor? SelectedProfesor
        {
            get => _profesor;
            set
            {
                _profesor = value;
                OnPropertyChanged();
            }
        }

        private Student? _student;

        public Student? SelectedStudent
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            students = new ObservableCollection<Student>(StudentService.GetStudents());
            profesors = new ObservableCollection<Profesor>(ProfesorService.GetProfesors());
            predmets = new ObservableCollection<Predmet>(PredmetService.GetPredmets());
            Students = students;
            Profesors = profesors;
            Predmets = predmets;
            FilteredStudents = new ObservableCollection<Student>(Students);
            FilteredProfesors = new ObservableCollection<Profesor>(Profesors);
            FilteredPredmets = new ObservableCollection<Predmet>(Predmets);
            DataContext = this;
            SetMenuIcons();
            selected = Tabs1.SelectedItem as TabItem;
            StatusBarText.Content += " - " + selected?.Header;
            StatusDateText.Content = DateTime.Now.ToString("hh:mm dd:MM:yyyy");
            GridStudents.ItemsSource = FilteredStudents;
            //TxtSearch.Margin = TxtSearch.Margin with { Left = TTray.Width - 270 };
        }

        private void SetMenuIcons()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var imagesFolder = Path.Combine(baseDirectory, "..", "..", "..", "Resources", "images");

            SetMenuItemIcon(NewMenuItem, Path.Combine(imagesFolder, "add.png"));
            SetMenuItemIcon(SaveMenuItem, Path.Combine(imagesFolder, "diskette.png"));
            SetMenuItemIcon(OpenMenuItem, Path.Combine(imagesFolder, "folder.png"));
            SetMenuItemIcon(CloseMenuItem, Path.Combine(imagesFolder, "close.png"));
            SetMenuItemIcon(EditMenuItem, Path.Combine(imagesFolder, "edit.png"));
            SetMenuItemIcon(DeleteMenuItem, Path.Combine(imagesFolder, "delete.png"));
            SetMenuItemIcon(AboutMenuItem, Path.Combine(imagesFolder, "info.png"));
            SetButtonIcon(BtnNew, Path.Combine(imagesFolder, "add.png"));
            SetButtonIcon(BtnEdit, Path.Combine(imagesFolder, "edit.png"));
            SetButtonIcon(BtnDelete, Path.Combine(imagesFolder, "delete.png"));
            SetButtonIcon(BtnSearch, Path.Combine(imagesFolder, "info.png"));
        }

        private void SetMenuItemIcon(MenuItem menuItem, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                var uri = new Uri(imagePath, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                var image = new Image { Source = bitmap };
                menuItem.Icon = image;
            }
            else
            {
                MessageBox.Show($"Image not found: {imagePath}");
            }
        }

        private void SetButtonIcon(Button button, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                var uri = new Uri(imagePath, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                var image = new Image { Source = bitmap };
                button.Content = image;
            }
            else
            {
                MessageBox.Show($"Image not found: {imagePath}");
            }
        }

        private void SwitchLanguage(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var dict = new ResourceDictionary();
            var resourcesPath = Path.Combine("Resources", "l10n", $"StringResources.{cultureCode}.xaml");
            dict.Source = new Uri(resourcesPath, UriKind.Relative);

            this.Resources.MergedDictionaries[0] = dict;
            MessageBoxManager.OK = (dict["OkButton"] as string)!;
            MessageBoxManager.Cancel = (dict["CancelButton"] as string)!;
            MessageBoxManager.Register();
        }

        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var selectedLanguage = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            if (selectedLanguage != null) SwitchLanguage(selectedLanguage);
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Tabs1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = Tabs1.SelectedItem as TabItem;
            var text = StatusBarText.Content as String;
            text = text?[..text.LastIndexOf('-')];
            StatusBarText.Content = text + "- " + selected?.Header;
        }

        private void RefreshData(object? sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(GridStudents.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(GridProfessors.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(GridSubjects.ItemsSource).Refresh();
            students = new ObservableCollection<Student>(StudentService.GetStudents());
            profesors = new ObservableCollection<Profesor>(ProfesorService.GetProfesors());
            predmets = new ObservableCollection<Predmet>(PredmetService.GetPredmets());
            Students = students;
            Profesors = profesors;
            Predmets = predmets;
        }

        private void NewEntityBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshData(sender, e);
            switch (Selected?.Tag)
            {
                case "Studenti":
                    DodajStudentaView dsv = new DodajStudentaView();
                    if (dsv.ShowDialog() == true)
                    {
                        RefreshData(sender, e);
                    }
                    break;
                case "Profesori":
                    DodajProfesoraView dpv = new DodajProfesoraView();
                    if (dpv.ShowDialog() == true)
                    {
                        RefreshData(sender, e);
                    }
                    break;
                case "Predmeti":
                    DodajPredmetView dprv = new DodajPredmetView();
                    dprv.OnFinish += RefreshData;
                    dprv.Show();
                    break;
                default:
                    MessageBox.Show("Greška", "Greška");
                    break;
            }
        }

        private void SaveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CloseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void EditBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Tabs1.SelectedItem is not TabItem selectedTab) return;
            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    if (SelectedStudent == null)
                    {
                        MessageBox.Show("Nijedan student nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        EditStudentView esv = new EditStudentView(SelectedStudent);
                        MessageBox.Show(esv.ShowDialog() == true ? "Uspesno izmenjen student!" : "Izmene nisu sacuvane!", "Izmena studenta");
                    }
                    break;

                case "Profesori":
                    if (SelectedProfesor == null)
                    {
                        MessageBox.Show("Nijedan profesor nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        EditProfesorView epv = new EditProfesorView(SelectedProfesor);
                        MessageBox.Show(epv.ShowDialog() == true ? "Uspesno izmenjen student!" : "Izmene nisu sacuvane!", "Izmena studenta");
                    }
                    break;

                case "Predmeti":
                    if (SelectedPredmet == null)
                    {
                        MessageBox.Show("Nijedan predmet nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        EditPredmetView epv = new EditPredmetView(SelectedPredmet);
                        MessageBox.Show(epv.ShowDialog() == true ? "Uspesno izmenjen student!" : "Izmene nisu sacuvane!", "Izmena studenta");
                    }
                    break;

                default:
                    MessageBox.Show("Nepoznata opcija.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

        }

        private void DeleteBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HelpBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Verzija: \t0.1\nAutor: \tMiloš Matunović\nStudent FTN-a E2 smer.");
        }

        private void SearchBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Tabs1.SelectedItem is not TabItem selectedTab) return;
            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    string query = TxtSearch.Text;
                    FilteredStudents.Clear();

                    if (string.IsNullOrWhiteSpace(query))
                    {
                        foreach (var student in Students)
                        {
                            FilteredStudents.Add(student);
                        }

                        return;
                    }

                    var parts = query.Split(',');

                    if (parts.Length == 1)
                    {
                        string lastNamePart = parts[0].Trim().ToLower();
                        foreach (var student in Students)
                        {
                            if (student.Prezime.ToLower().Contains(lastNamePart))
                            {
                                FilteredStudents.Add(student);
                            }
                        }
                    }
                    else if (parts.Length == 2)
                    {
                        string lastNamePart = parts[0].Trim().ToLower();
                        string firstNamePart = parts[1].Trim().ToLower();
                        foreach (var student in Students)
                        {
                            if (student.Prezime.ToLower().Contains(lastNamePart) &&
                                student.Ime.ToLower().Contains(firstNamePart))
                            {
                                FilteredStudents.Add(student);
                            }
                        }
                    }
                    else if (parts.Length == 3)
                    {
                        string indexPart = parts[0].Trim().ToLower();
                        string firstNamePart = parts[1].Trim().ToLower();
                        string lastNamePart = parts[2].Trim().ToLower();
                        foreach (var student in Students)
                        {
                            if (student.BrojIndeksa.ToString().ToLower().Contains(indexPart) &&
                                student.Ime.ToLower().Contains(firstNamePart) &&
                                student.Prezime.ToLower().Contains(lastNamePart))
                            {
                                FilteredStudents.Add(student);
                            }
                        }
                    }
                    break;
                case "Profesori":

                    break;

                case "Predmeti":

                    break;

                default:
                    MessageBox.Show("Nepoznata opcija.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void OnDataGridSort_Executed(object sender, DataGridSortingEventArgs e)
        {
            var dg = sender as DataGrid;
            var lcv = CollectionViewSource.GetDefaultView(dg.ItemsSource) as ListCollectionView;
        }

        private void ShowKatedraInfo(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
    }
}

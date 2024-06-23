using CLI.Service;
using GUI.View;
using StudentskaSluzba.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using CLI.DAO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
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
        public ObservableCollection<Katedra> Katedre { get; set; }
        public Katedra? SelectedKatedra { get; set; }

        public static readonly RoutedUICommand PrikazKatedriCommand = new RoutedUICommand(
            "PrikazKatedri", "PrikazKatedri", typeof(MainWindow));
        private DispatcherTimer timer;
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
        public static readonly RoutedUICommand BrisanjeCommand = new RoutedUICommand(
                "Brisanje", "Brisanje", typeof(MainWindow));
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
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateStatusBar();
            GridStudents.ItemsSource = FilteredStudents;

            // Ovo će osigurati da se komande odmah osveže
            CommandManager.InvalidateRequerySuggested();

            // Dodajte SelectionChanged događaje za DataGrid
            GridStudents.SelectionChanged += GridStudents_SelectionChanged;
            GridProfessors.SelectionChanged += GridProfessors_SelectionChanged;
            GridSubjects.SelectionChanged += GridSubjects_SelectionChanged;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
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
            CommandManager.InvalidateRequerySuggested();
            UpdateStatusBar();
        }
        private void UpdateStatusBar()
        {
            if (selected != null)
            {
                TabNameTextBlock.Text = selected.Header.ToString();
            }
            else
            {
                TabNameTextBlock.Text = string.Empty;
            }
        }
        private void RefreshData(object? sender, EventArgs e)
        {
            Students = new ObservableCollection<Student>(StudentService.GetStudents());
            Profesors = new ObservableCollection<Profesor>(ProfesorService.GetProfesors());
            Predmets = new ObservableCollection<Predmet>(PredmetService.GetPredmets());
            FilteredStudents = new ObservableCollection<Student>(Students);
            FilteredProfesors = new ObservableCollection<Profesor>(Profesors);
            FilteredPredmets = new ObservableCollection<Predmet>(Predmets);
            GridStudents.ItemsSource = FilteredStudents;
            GridProfessors.ItemsSource = FilteredProfesors;
            GridSubjects.ItemsSource = FilteredPredmets;
            CollectionViewSource.GetDefaultView(GridStudents.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(GridProfessors.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(GridSubjects.ItemsSource).Refresh();
        }
        private void GridStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStudent = GridStudents.SelectedItem as Student;
            CommandManager.InvalidateRequerySuggested();
        }

        private void GridProfessors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProfesor = GridProfessors.SelectedItem as Profesor;
            CommandManager.InvalidateRequerySuggested();
        }

        private void GridSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPredmet = GridSubjects.SelectedItem as Predmet;
            CommandManager.InvalidateRequerySuggested();
        }
        private void NewEntityBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Tabs1.SelectedItem is not TabItem selectedTab) return;

            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    DodajStudentaView dsv = new DodajStudentaView();
                    dsv.OnFinish += RefreshData; // Subscribe to the OnFinish event
                    if (dsv.ShowDialog() == true)
                    {
                        RefreshData(sender, e);
                    }
                    break;
                case "Profesori":
                    DodajProfesoraView dpv = new DodajProfesoraView();
                    dpv.OnFinish += RefreshData; // Subscribe to the OnFinish event
                    if (dpv.ShowDialog() == true)
                    {
                        RefreshData(sender, e);
                    }
                    break;
                case "Predmeti":
                    DodajPredmetView dprv = new DodajPredmetView();
                    dprv.OnFinish += RefreshData; // Subscribe to the OnFinish event
                    dprv.ShowDialog(); // Use ShowDialog to ensure blocking behavior
                    break;
                default:
                    MessageBox.Show("Greška", "Greška");
                    break;
            }
        }


        private void SaveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void OpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectTabByTag("Studenti"); // Ovo možete promeniti u zavisnosti od selektovanog entiteta
        }

        private void CloseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void DeleteBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (Tabs1.SelectedItem is not TabItem selectedTab) return;

            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    if (SelectedStudent == null)
                    {
                        MessageBox.Show("Nijedan student nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var studentResult = MessageBox.Show("Da li ste sigurni da želite da obrišete selektovanog studenta?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (studentResult == MessageBoxResult.Yes)
                    {
                        if (StudentService.DeleteStudent(SelectedStudent.Id))
                        {
                            Students.Remove(SelectedStudent);
                            FilteredStudents.Remove(SelectedStudent);
                            MessageBox.Show("Student je uspešno obrisan.", "Brisanje studenta", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Greška prilikom brisanja studenta.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    break;

                case "Profesori":
                    if (SelectedProfesor == null)
                    {
                        MessageBox.Show("Nijedan profesor nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var profesorResult = MessageBox.Show("Da li ste sigurni da želite da obrišete selektovanog profesora?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (profesorResult == MessageBoxResult.Yes)
                    {
                        if (ProfesorService.DeleteProfesor(SelectedProfesor.Id))
                        {
                            Profesors.Remove(SelectedProfesor);
                            FilteredProfesors.Remove(SelectedProfesor);
                            MessageBox.Show("Profesor je uspešno obrisan.", "Brisanje profesora", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Greška prilikom brisanja profesora.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    break;

                case "Predmeti":
                    if (SelectedPredmet == null)
                    {
                        MessageBox.Show("Nijedan predmet nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var predmetResult = MessageBox.Show("Da li ste sigurni da želite da obrišete selektovani predmet?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (predmetResult == MessageBoxResult.Yes)
                    {
                        if (PredmetService.DeletePredmet(SelectedPredmet.SifraPredmeta))
                        {
                            var dao = new ProfesorDAO();
                            var stariProfesor = dao.UzmiSveProfesore();
                            foreach (var prof in stariProfesor)
                            {
                                if (prof.SpisakPredmeta.Any(p => p.SifraPredmeta == SelectedPredmet.SifraPredmeta))
                                {
                                    prof.SpisakPredmeta.Remove(SelectedPredmet);
                                    dao.AzurirajProfesora(prof);
                                }
                            }
                            Predmets.Remove(SelectedPredmet);
                            FilteredPredmets.Remove(SelectedPredmet);
                            MessageBox.Show("Predmet je uspešno obrisan.", "Brisanje predmeta", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Greška prilikom brisanja predmeta.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    break;

                default:
                    MessageBox.Show("Nepoznata opcija.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
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
                        esv.OnFinish += RefreshData; // Subscribe to the OnFinish event
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
                        epv.OnFinish += RefreshData; // Subscribe to the OnFinish event
                        MessageBox.Show(epv.ShowDialog() == true ? "Uspesno izmenjen profesor!" : "Izmene nisu sacuvane!", "Izmena profesora");
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
                        epv.OnFinish += RefreshData; // Subscribe to the OnFinish event
                        MessageBox.Show(epv.ShowDialog() == true ? "Uspesno izmenjen predmet!" : "Izmene nisu sacuvane!", "Izmena predmeta");
                    }
                    break;

                default:
                    MessageBox.Show("Nepoznata opcija.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

        }
        private void MenuItem_Studenti_Click(object sender, RoutedEventArgs e)
        {
            SelectTabByTag("Studenti");
        }

        private void MenuItem_Predmeti_Click(object sender, RoutedEventArgs e)
        {
            SelectTabByTag("Predmeti");
        }

        private void MenuItem_Profesori_Click(object sender, RoutedEventArgs e)
        {
            SelectTabByTag("Profesori");
        }

        private void SelectTabByTag(string tag)
        {
            foreach (TabItem tabItem in Tabs1.Items)
            {
                if (tabItem.Tag != null && tabItem.Tag.ToString() == tag)
                {
                    Tabs1.SelectedItem = tabItem;
                    break;
                }
            }
        }

        private void HelpBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Verzija: \t0.1\nAutor: \tMiloš Matunović\nStudent FTN-a E2 smer.");
        }

        private void SearchBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Tabs1.SelectedItem is not TabItem selectedTab) return;
            string query = TxtSearch.Text;
            if (string.IsNullOrWhiteSpace(query))
            {
                RefreshData(sender, e);
                return;
            }

            var parts = query.Split(',');

            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    FilteredStudents.Clear();
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
                    FilteredProfesors.Clear();
                    if (parts.Length == 1)
                    {
                        string part = parts[0].Trim().ToLower();
                        foreach (var profesor in Profesors)
                        {
                            if (profesor.Prezime.ToLower().Contains(part) ||
                                profesor.Ime.ToLower().Contains(part))
                            {
                                FilteredProfesors.Add(profesor);
                            }
                        }
                    }
                    break;

                case "Predmeti":
                    FilteredPredmets.Clear();
                    if (parts.Length == 1)
                    {
                        string part = parts[0].Trim().ToLower();
                        foreach (var predmet in Predmets)
                        {
                            if (predmet.SifraPredmeta.ToLower().Contains(part) ||
                                predmet.NazivPredmeta.ToLower().Contains(part))
                            {
                                FilteredPredmets.Add(predmet);
                            }
                        }
                    }
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
        private void FilterStudents_Click(object sender, RoutedEventArgs e)
        {
            var filterDialog = new PredmetFilterDialog();
            if (filterDialog.ShowDialog() == true)
            {
                var firstSubject = filterDialog.FirstSubject;
                var secondSubject = filterDialog.SecondSubject;
                var filterType = filterDialog.FilterType;

                var filteredStudents = new ObservableCollection<Student>();

                if (filterType == "AttendingBoth")
                {
                    foreach (var student in StudentService.GetStudents())
                    {
                        // Prikazujemo listu nepoloženih predmeta za svakog studenta
                        var nepolozeniPredmeti = string.Join(", ", student.SpisakNepolozenihPredmeta.Select(p => p.SifraPredmeta));

                        if (student.SpisakNepolozenihPredmeta.Any(p => p.SifraPredmeta == firstSubject.SifraPredmeta) &&
                            student.SpisakNepolozenihPredmeta.Any(p => p.SifraPredmeta == secondSubject.SifraPredmeta))
                        {
                            filteredStudents.Add(student);
                        }
                    }
                }
                else if (filterType == "PassedFirstNotSecond")
                {
                    foreach (var student in StudentService.GetStudents())
                    {
                        var polozniPredmeti = string.Join(", ", student.SpisakPolozenihIspita.Select(p => p.SifraPredmeta));

                        if (student.SpisakPolozenihIspita.Any(p => p.SifraPredmeta == firstSubject.SifraPredmeta) &&
                            !student.SpisakPolozenihIspita.Any(p => p.SifraPredmeta == secondSubject.SifraPredmeta))
                        {
                            filteredStudents.Add(student);
                        }
                    }
                }

                GridStudents.ItemsSource = filteredStudents;
            }
        }

        private void ShowKatedraInfo(object sender, ExecutedRoutedEventArgs e)
        {
            var katedraDialog = new KatedraDialog();
            if (katedraDialog.ShowDialog() == true)
            {
                SelectedKatedra = katedraDialog.SelectedKatedra;
                SelectedProfesor = katedraDialog.SelectedProfesor;

                // Postavite šefa katedre ako su ispunjeni uslovi
                if (SelectedProfesor != null && SelectedKatedra != null && katedraDialog.IsEligibleForHead())
                {
                    SelectedKatedra.SefKatedre = SelectedProfesor;
                    MessageBox.Show($"{SelectedProfesor.Ime} {SelectedProfesor.Prezime} je postavljen za šefa katedre {SelectedKatedra.NazivKatedre}.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Odabrani profesor ne zadovoljava uslove za šefa katedre.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchLanguage(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var dict = new ResourceDictionary();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var resourcesPath = Path.Combine(baseDirectory, "Resources", $"StringResources.{cultureCode}.xaml");
            dict.Source = new Uri(resourcesPath, UriKind.Absolute);

            this.Resources.MergedDictionaries[0] = dict;
        }
        
        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var selectedLanguage = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            SwitchLanguage(selectedLanguage);
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}

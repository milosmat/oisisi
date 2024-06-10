using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            var dict = new ResourceDictionary
            {
                Source = cultureCode switch
                {
                    "sr" => new Uri("Resources/StringResources.srb.xaml", UriKind.Relative),
                    _ => new Uri("Resources/StringResources.en.xaml", UriKind.Relative)
                }
            };

            this.Resources.MergedDictionaries[0] = dict;
        }
        
        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var selectedLanguage = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            SwitchLanguage(selectedLanguage);
        }
    }
}

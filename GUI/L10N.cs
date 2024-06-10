using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace GUI;

public class LocalizedResourceDictionary : MarkupExtension
{
    //public string Source { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var cultureCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var resourcePath = Path.Combine(baseDirectory, "..", "..", "..", "Resources", "l10n", $"StringResources.{cultureCode}.xaml");

        var dictionary = new ResourceDictionary
        {
            Source = new Uri(resourcePath, UriKind.Absolute)
        };
        return dictionary;
    }
}
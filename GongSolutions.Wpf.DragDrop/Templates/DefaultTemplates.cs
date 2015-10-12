using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace GongSolutions.Wpf.DragDrop.Templates
{
  static class DefaultTemplates
  {
    static DefaultTemplates()
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GongSolutions.Wpf.DragDrop.Templates.DefaultEffectAdorner.xaml");
      EffectAdorner = (DataTemplate)XamlReader.Load(stream);      
    }

    public static DataTemplate EffectAdorner;
  }
}

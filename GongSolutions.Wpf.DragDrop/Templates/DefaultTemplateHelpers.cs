using GongSolutions.Wpf.DragDrop.Icons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GongSolutions.Wpf.DragDrop.Templates
{
  /// <summary>
  /// Interaction logic for DefaultDragAdorner.xaml
  /// </summary>
 
    public class IconConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        switch ((DragDropEffects)value) {
          case DragDropEffects.Move: return IconFactory.EffectMove;
          case DragDropEffects.Copy: return IconFactory.EffectCopy;
          case DragDropEffects.Link: return IconFactory.EffectLink;
          case DragDropEffects.None: return IconFactory.EffectNone;
        }

        return null;
      }
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    public class TextConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        switch ((DragDropEffects)value) {
          case DragDropEffects.Move: return "Move to";
          case DragDropEffects.Copy: return "Copy to";
          case DragDropEffects.Link: return "Link to";
        }

        return String.Empty;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}

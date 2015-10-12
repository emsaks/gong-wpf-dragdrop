using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GongSolutions.Wpf.DragDrop
{
  class DragTransaction
  {
    public IDragInfo Source;
    public IDropInfo Target;
    public DragDropEffects Effects
    {
      get { return Source.Effects & Target.Effects;  }      
    }
    
    public DragDropEffects ActiveEffect { get; set; }       
  }
}

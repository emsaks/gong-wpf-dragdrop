using System.Linq;
using System.Windows;
using GongSolutions.Wpf.DragDrop.Utilities;

namespace GongSolutions.Wpf.DragDrop
{
  public class DefaultDragHandler : IDragSource
  {
    public virtual DragDropEffects Dragged(IDragInfo dragInfo)
    {
      var itemCount = dragInfo.SourceItems.Cast<object>().Count();

      if (itemCount == 1) {
        dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
      } else if (itemCount > 1) {
        dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
      }

      return (dragInfo.Data != null) ?
                           DragDropEffects.Copy | DragDropEffects.Move :
                           DragDropEffects.None;
    }        

    public void Detach(IDragInfo dragInfo)
    {
      var src = dragInfo.SourceCollection.TryGetList();
      if (src == null) { return; }      
      foreach (var i in dragInfo.SourceItems) { src.Remove(i); }
    }

    public virtual void Dropped(IDropInfo dropInfo)
    {
    }

    public virtual void DragCancelled()
    {
    }
  }
}
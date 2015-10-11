using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace GongSolutions.Wpf.DragDrop.Utilities
{
  public static class VisualTreeExtensions
  {
    /// <summary>
    /// Gets the next ancestor element which is a drop target.
    /// </summary>
    /// <param name="element">The start element.</param>
    /// <returns>The first element which is a drop target.</returns>
    public static UIElement TryGetNextAncestorDropTargetElement(this UIElement element)
    {
      if (element == null)
      {
        return null;
      }
      var ancestor = element.GetVisualAncestor<UIElement>();
      while (ancestor != null)
      {
        if (ancestor.IsDropTarget())
        {
          return ancestor;
        }
        ancestor = ancestor.GetVisualAncestor<UIElement>();
      }
      return null;
    }

    internal static DependencyObject FindVisualTreeRoot(this DependencyObject d)
    {
      var current = d;
      var result = d;

      while (current != null) {
        result = current;
        if (current is Visual || current is Visual3D) {
          break;
        } else {
          // If we're in Logical Land then we must walk 
          // up the logical tree until we find a 
          // Visual/Visual3D to get us back to Visual Land.
          current = LogicalTreeHelper.GetParent(current);
        }
      }

      return result;
    }

    public static T GetVisualAncestor<T>(this DependencyObject d) where T : class
    {
      var item = VisualTreeHelper.GetParent(d.FindVisualTreeRoot());

      while (item != null) {
        var itemAsT = item as T;
        if (itemAsT != null) {
          return itemAsT;
        }
        item = VisualTreeHelper.GetParent(item);
      }

      return null;
    }

    public static DependencyObject GetVisualAncestor(this DependencyObject d, Type type)
    {
      var item = VisualTreeHelper.GetParent(d.FindVisualTreeRoot());

      while (item != null && type != null) {
        if (item.GetType() == type || item.GetType().IsSubclassOf(type)) {
          return item;
        }
        item = VisualTreeHelper.GetParent(item);
      }

      return null;
    }

    /// <summary>
    /// find the visual ancestor by type and go through the visual tree until the given itemsControl will be found
    /// </summary>
    public static DependencyObject GetVisualAncestor(this DependencyObject d, Type type, ItemsControl itemsControl)
    {
      var item = VisualTreeHelper.GetParent(d.FindVisualTreeRoot());
      DependencyObject lastFoundItemByType = null;

      while (item != null && type != null) {
        if (item == itemsControl) {
          return lastFoundItemByType;
        }
        if ((item.GetType() == type || item.GetType().IsSubclassOf(type))
            && (itemsControl == null || itemsControl.ItemContainerGenerator.IndexFromContainer(item) != -1)) {
          lastFoundItemByType = item;
        }
        item = VisualTreeHelper.GetParent(item);
      }

      return lastFoundItemByType;
    }

    public static T GetVisualDescendent<T>(this DependencyObject d) where T : DependencyObject
    {
      return d.GetVisualDescendents<T>().FirstOrDefault();
    }

    public static IEnumerable<T> GetVisualDescendents<T>(this DependencyObject d) where T : DependencyObject
    {
      var childCount = VisualTreeHelper.GetChildrenCount(d);

      for (var n = 0; n < childCount; n++) {
        var child = VisualTreeHelper.GetChild(d, n);

        if (child is T) {
          yield return (T)child;
        }

        foreach (var match in GetVisualDescendents<T>(child)) {
          yield return match;
        }
      }

      yield break;
    }

    public static DependencyObject FindDescendent(this DependencyObject d, Func<DependencyObject, bool> where, Func<DependencyObject, bool> skip)
    {      
      var childCount = VisualTreeHelper.GetChildrenCount(d);

      for (var n = 0; n < childCount; n++) {
        var child = VisualTreeHelper.GetChild(d, n);

        if (where(child)) { return child; }
        if (!skip(child)) {
          var grandchild = FindDescendent(child, where, skip);
          if (grandchild != null) { return grandchild;  } 
        }
      }

      return null;      
    }

    public static DependencyObject FindAncestor(this DependencyObject d, Func<DependencyObject, bool> where)
    {
      return d.FindAncestor(where, p => p == null);
    }

    public static DependencyObject FindAncestor(this DependencyObject d, Func<DependencyObject, bool> where, Func<DependencyObject, bool> stop)
    {
      DependencyObject current = VisualTreeHelper.GetParent(FindVisualTreeRoot(d));
      while (current != null && !where(current)) {
        if (stop(current)) { return null; }
        current = VisualTreeHelper.GetParent(current);        
      }

      return current;
    }

    public static DependencyObject ContainerOrDefault(this DependencyObject d, ItemsControl root)
    {
      DependencyObject current = d;
      DependencyObject container = null;

      while (current != null) {
        DependencyObject parent = VisualTreeHelper.GetParent(current);
        if (((parent as Panel)?.IsItemsHost ?? false) && container == null) {
          var owner = FindAncestor(current, a => a is ItemsControl) as ItemsControl;
          if (owner == null) { return null; }
          if (owner.ItemContainerGenerator.IndexFromContainer(current) >= 0) { container = current; }
          parent = owner;          
        } else if(current is ItemsControl && (parent as Panel)?.IsItemsHost != true) {
          container = null;
        }

        if (parent == root) { return container; }                       
        current = parent;        
      }

      return null;
    }
  }  
}

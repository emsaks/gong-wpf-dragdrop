using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop.Utilities;

namespace GongSolutions.Wpf.DragDrop
{
  /// <summary>
  /// Holds information about a the source of a drag drop operation.
  /// </summary>
  /// 
  /// <remarks>
  /// The <see cref="DragInfo"/> class holds all of the framework's information about the source
  /// of a drag. It is used by <see cref="IDragSource.Dragged"/> to determine whether a drag 
  /// can start, and what the dragged data should be.
  /// </remarks>
  public class DragInfo : IDragInfo
  {
    /// <summary>
    /// Initializes a new instance of the DragInfo class.
    /// </summary>
    /// 
    /// <param name="e">
    /// The mouse event that initiated the drag.
    /// </param> 
    public DragInfo(ItemsControl root, MouseButtonEventArgs e)
    {
      int index = -1;
      ItemsControl owner = null;
      this.SourceContainer = (e.OriginalSource as DependencyObject)?.ContainerOrDefault(root, ref owner, ref index);
      if (this.SourceContainer == null) { return;  }
      this.SourceItem = owner.ItemContainerGenerator.ItemFromContainer(this.SourceContainer);

      if (this.SourceItem is CollectionViewGroup) {
        this.SourceGroup = (CollectionViewGroup)this.SourceItem;
        // this is probably never necessary - ListView removes empty groups.
        if ((this.SourceItem as CollectionViewGroup).Items.Count() == 0) {
          this.SourceItem = this.SourceContainer =  null;
          return;
        }
        
        // fudge all the origin details:

        this.SourceItems = (this.SourceItem as CollectionViewGroup).Items.Cast<object>().ToArray();
        this.SourceItem = this.SourceItems.Cast<object>().FirstOrDefault();
        // find SourceItem's container (assume the first descendant with a matching DataContext is the container)
        this.SourceContainer = VisualTreeExtensions.FindDescendent(this.SourceContainer, d => ((d as FrameworkElement)?.DataContext ?? (d as FrameworkContentElement)?.DataContext) == this.SourceItem);
        this.SourceCollection = ItemsControl.ItemsControlFromItemContainer(this.SourceContainer).ItemsSource;
      } else {
        this.SourceGroup = root.FindGroup(this.DragStartPosition);
        this.SourceCollection = owner.ItemsSource ?? owner.Items;

        this.SourceItems = root.ReflectSelectedItems().Cast<object>().ToArray();

        // Some controls (I'm looking at you TreeView!) haven't updated their
        // SelectedItem by this point. Check to see if there 1 or less item in 
        // the SourceItems collection, and if so, override the control's 
        // SelectedItems with the clicked item.
        // edit: just check if the SelectedItems contains the sourceitem.
        if (this.SourceItems == null || !this.SourceItems.Cast<object>().Contains(this.SourceItem)) {
          this.SourceItems = new object[] { this.SourceItem };
        }
                
      }

      this.DragStartPosition = e.GetPosition((IInputElement)this.SourceControl);
      this.Effects = DragDropEffects.None;
      this.MouseButton = e.ChangedButton;
      this.SourceControl = root as UIElement;
      this.DragDropCopyKeyState = DragDrop.GetDragDropCopyKeyState(this.SourceControl);

      // Remember the relative position of the item being dragged
      this.PositionInDraggedItem = e.GetPosition((IInputElement)this.SourceContainer);
      this.SourceControlFlowDirection = root.GetItemsPanelFlowDirection();      
      
    }

    /// <summary>
    /// Gets or sets the drag data.
    /// </summary>
    /// 
    /// <remarks>
    /// This must be set by a drag handler in order for a drag to start.
    /// </remarks>
    public object Data { get; set; }

    /// <summary>
    /// Gets the position of the click that initiated the drag, relative to <see cref="SourceControl"/>.
    /// </summary>
    public Point DragStartPosition { get; private set; }

    /// <summary>
    /// Gets the point where the cursor was relative to the item being dragged when the drag was started.
    /// </summary>
    public Point PositionInDraggedItem { get; private set; }

    /// <summary>
    /// Gets or sets the allowed effects for the drag.
    /// </summary>
    /// 
    /// <remarks>
    /// This must be set to a value other than <see cref="DragDropEffects.None"/> by a drag handler in order 
    /// for a drag to start.
    /// </remarks>
    public DragDropEffects Effects { get; set; }

    /// <summary>
    /// Gets the mouse button that initiated the drag.
    /// </summary>
    public MouseButton MouseButton { get; private set; }

    /// <summary>
    /// Gets the collection that the source ItemsControl is bound to.
    /// </summary>
    /// 
    /// <remarks>
    /// If the control that initated the drag is unbound or not an ItemsControl, this will be null.
    /// </remarks>
    public IEnumerable SourceCollection { get; private set; }

    /// <summary>
    /// Gets the position from where the item was dragged.
    /// </summary>
    /// <value>The index of the source.</value>
    public int SourceIndex { get; private set; }

    /// <summary>
    /// Gets the object that a dragged item is bound to.
    /// </summary>
    /// 
    /// <remarks>
    /// If the control that initated the drag is unbound or not an ItemsControl, this will be null.
    /// </remarks>
    public object SourceItem { get; private set; }

    /// <summary>
    /// Gets a collection of objects that the selected items in an ItemsControl are bound to.
    /// </summary>
    /// 
    /// <remarks>
    /// If the control that initated the drag is unbound or not an ItemsControl, this will be empty.
    /// </remarks>
    public IEnumerable SourceItems { get; private set; }

    /// <summary>
    /// Gets the group from a dragged item if the drag is currently from an ItemsControl with groups.
    /// </summary>
    public CollectionViewGroup SourceGroup { get; private set; }

    /// <summary>
    /// Gets the control that initiated the drag.
    /// </summary>
    public UIElement SourceControl { get; private set; }

    /// <summary>
    /// Gets the item in an ItemsControl that started the drag.
    /// </summary>
    /// 
    /// <remarks>
    /// If the control that initiated the drag is an ItemsControl, this property will hold the item
    /// container of the clicked item. For example, if <see cref="SourceControl"/> is a ListBox this
    /// will hold a ListBoxItem.
    /// </remarks>
    public DependencyObject SourceContainer { get; private set; }

    /// <summary>
    /// Gets the FlowDirection of the current drag source.
    /// </summary>
    public FlowDirection SourceControlFlowDirection { get; private set; }

    /// <summary>
    /// Gets the <see cref="IDataObject"/> which is used by the drag and drop operation. Set it to
    /// a custom instance if custom drag and drop behavior is needed.
    /// </summary>
    public IDataObject DataObject { get; set; }

    /// <summary>
    /// Gets the drag drop copy key state indicating the effect of the drag drop operation.
    /// </summary>
    public DragDropKeyStates DragDropCopyKeyState { get; private set; }
  }
}
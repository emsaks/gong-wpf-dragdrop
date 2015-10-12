using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop.Icons;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Reflection;

namespace GongSolutions.Wpf.DragDrop
{
  public static class DragDrop
  {
    public static readonly DataFormat DataFormat = DataFormats.GetDataFormat("GongSolutions.Wpf.DragDrop");

    #region "dp"
    /// <summary>
    /// The drag drop copy key state property (default None).
    /// So the drag drop action is
    /// - Move, within the same control or from one to another, if the drag drop key state is None
    /// - Copy, from one to another control with the given drag drop copy key state
    /// </summary>
    public static readonly DependencyProperty DragDropCopyKeyStateProperty =
      DependencyProperty.RegisterAttached("DragDropCopyKeyState", typeof(DragDropKeyStates), typeof(DragDrop), new PropertyMetadata(DragDropKeyStates.ControlKey));

    /// <summary>
    /// Gets the drag drop copy key state indicating the effect of the drag drop operation.
    /// </summary>
    public static DragDropKeyStates GetDragDropCopyKeyState(UIElement target)
    {
      return (DragDropKeyStates)target.GetValue(DragDropCopyKeyStateProperty);
    }

    /// <summary>
    /// Sets the drag drop copy key state indicating the effect of the drag drop operation.
    /// </summary>
    public static void SetDragDropCopyKeyState(UIElement target, DragDropKeyStates value)
    {
      target.SetValue(DragDropCopyKeyStateProperty, value);
    }

    public static readonly DependencyProperty DragAdornerTemplateProperty =
      DependencyProperty.RegisterAttached("DragAdornerTemplate", typeof(DataTemplate), typeof(DragDrop));
    public static DataTemplate GetDragAdornerTemplate(UIElement target)
    {
      return (DataTemplate)target.GetValue(DragAdornerTemplateProperty);
    }
    public static void SetDragAdornerTemplate(UIElement target, DataTemplate value)
    {
      target.SetValue(DragAdornerTemplateProperty, value);
    }

    public static readonly DependencyProperty DragAdornerTemplateSelectorProperty = DependencyProperty.RegisterAttached(
      "DragAdornerTemplateSelector", typeof (DataTemplateSelector), typeof (DragDrop), new PropertyMetadata(default(DataTemplateSelector)));
    public static void SetDragAdornerTemplateSelector(DependencyObject element, DataTemplateSelector value) 
    {
      element.SetValue(DragAdornerTemplateSelectorProperty, value);
    }
    public static DataTemplateSelector GetDragAdornerTemplateSelector(DependencyObject element) 
    {
      return (DataTemplateSelector) element.GetValue(DragAdornerTemplateSelectorProperty);
    }

    public static readonly DependencyProperty UseDefaultDragAdornerProperty =
      DependencyProperty.RegisterAttached("UseDefaultDragAdorner", typeof(bool), typeof(DragDrop), new PropertyMetadata(false));
    public static bool GetUseDefaultDragAdorner(UIElement target)
    {
      return (bool)target.GetValue(UseDefaultDragAdornerProperty);
    }
    public static void SetUseDefaultDragAdorner(UIElement target, bool value)
    {
      target.SetValue(UseDefaultDragAdornerProperty, value);
    }

    public static readonly DependencyProperty DefaultDragAdornerOpacityProperty =
      DependencyProperty.RegisterAttached("DefaultDragAdornerOpacity", typeof(double), typeof(DragDrop), new PropertyMetadata(0.8));
    public static double GetDefaultDragAdornerOpacity(UIElement target)
    {
      return (double)target.GetValue(DefaultDragAdornerOpacityProperty);
    }
    public static void SetDefaultDragAdornerOpacity(UIElement target, double value)
    {
      target.SetValue(DefaultDragAdornerOpacityProperty, value);
    }
  
    public static DataTemplate GetEffectsTemplate(DependencyObject obj)
    {
      return (DataTemplate)obj.GetValue(EffectsTemplateProperty);
    }
    public static void SetEffectsTemplate(DependencyObject obj, DataTemplate value)
    {
      obj.SetValue(EffectsTemplateProperty, value);
    }    
    public static readonly DependencyProperty EffectsTemplateProperty =
        DependencyProperty.RegisterAttached("EffectsTemplate", typeof(DataTemplate), typeof(DragDrop), new PropertyMetadata(null));

    public static readonly DependencyProperty UseDefaultEffectsTemplateProperty =
      DependencyProperty.RegisterAttached("UseDefaultEffectsTemplate", typeof(bool), typeof(DragDrop), new PropertyMetadata(false));
    public static bool GetUseDefaultEffectsTemplate(UIElement target)
    {
      return (bool)target.GetValue(UseDefaultEffectsTemplateProperty);
    }
    public static void SetUseDefaultEffectsTemplate(UIElement target, bool value)
    {
      target.SetValue(UseDefaultEffectsTemplateProperty, value);
    }

 
    public static readonly DependencyProperty IsDragSourceProperty =
      DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragDrop), new UIPropertyMetadata(false, IsDragSourceChanged));
    public static bool GetIsDragSource(UIElement target)
    {
      return (bool)target.GetValue(IsDragSourceProperty);
    }
    public static void SetIsDragSource(UIElement target, bool value)
    {
      target.SetValue(IsDragSourceProperty, value);
    }

    public static readonly DependencyProperty IsDropTargetProperty =
      DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDrop), new UIPropertyMetadata(false, IsDropTargetChanged));
    public static bool GetIsDropTarget(UIElement target)
    {
      return (bool)target.GetValue(IsDropTargetProperty);
    }
    public static void SetIsDropTarget(UIElement target, bool value)
    {
      target.SetValue(IsDropTargetProperty, value);
    }

    public static readonly DependencyProperty DragDropContextProperty = 
        DependencyProperty.RegisterAttached("DragDropContext", typeof(string), typeof(DragDrop), new UIPropertyMetadata(string.Empty));
    public static string GetDragDropContext(UIElement target)
    {
        return (string)target.GetValue(DragDropContextProperty);
    }
    public static void SetDragDropContext(UIElement target, string value)
    {
        target.SetValue(DragDropContextProperty, value);
    }

    public static readonly DependencyProperty DragHandlerProperty =
      DependencyProperty.RegisterAttached("DragHandler", typeof(IDragSource), typeof(DragDrop));
    public static IDragSource GetDragHandler(UIElement target)
    {
      return (IDragSource)target.GetValue(DragHandlerProperty);
    }
    public static void SetDragHandler(UIElement target, IDragSource value)
    {
      target.SetValue(DragHandlerProperty, value);
    }

    public static readonly DependencyProperty DropHandlerProperty =
      DependencyProperty.RegisterAttached("DropHandler", typeof(IDropTarget), typeof(DragDrop));
    public static IDropTarget GetDropHandler(UIElement target)
    {
      return (IDropTarget)target.GetValue(DropHandlerProperty);
    }
    public static void SetDropHandler(UIElement target, IDropTarget value)
    {
      target.SetValue(DropHandlerProperty, value);
    }

    public static readonly DependencyProperty DragSourceIgnoreProperty =
      DependencyProperty.RegisterAttached("DragSourceIgnore", typeof(bool), typeof(DragDrop), new PropertyMetadata(false));
    public static bool GetDragSourceIgnore(UIElement source)
    {
      return (bool)source.GetValue(DragSourceIgnoreProperty);
    }
    public static void SetDragSourceIgnore(UIElement source, bool value)
    {
      source.SetValue(DragSourceIgnoreProperty, value);
    }

    public static readonly DependencyProperty DragDirectlySelectedOnlyProperty =
      DependencyProperty.RegisterAttached("DragDirectlySelectedOnly", typeof(bool), typeof(DragDrop), new PropertyMetadata(false));      
    public static bool GetDragDirectlySelectedOnly(DependencyObject obj)
    {
      return (bool)obj.GetValue(DragDirectlySelectedOnlyProperty);
    }
    public static void SetDragDirectlySelectedOnly(DependencyObject obj, bool value)
    {
      obj.SetValue(DragDirectlySelectedOnlyProperty, value);
    }

    /// <summary>
    /// DragMouseAnchorPoint defines the horizontal and vertical proportion at which the pointer will anchor on the DragAdorner.
    /// </summary>
    public static readonly DependencyProperty DragMouseAnchorPointProperty =
      DependencyProperty.RegisterAttached("DragMouseAnchorPoint", typeof(Point), typeof(DragDrop), new PropertyMetadata(new Point(0, 1)));
    public static Point GetDragMouseAnchorPoint(UIElement target)
    {
      return (Point)target.GetValue(DragMouseAnchorPointProperty);
    }
    public static void SetDragMouseAnchorPoint(UIElement target, Point value)
    {
      target.SetValue(DragMouseAnchorPointProperty, value);
    }
    #endregion

    private static void IsDragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var uiElement = (UIElement)d;

      if ((bool)e.NewValue == true) {
        uiElement.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
        uiElement.PreviewMouseLeftButtonUp += DragSource_PreviewMouseLeftButtonUp;
        uiElement.PreviewMouseMove += DragSource_PreviewMouseMove;
        uiElement.QueryContinueDrag += DragSource_QueryContinueDrag;
      } else {
        uiElement.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
        uiElement.PreviewMouseLeftButtonUp -= DragSource_PreviewMouseLeftButtonUp;
        uiElement.PreviewMouseMove -= DragSource_PreviewMouseMove;
        uiElement.QueryContinueDrag -= DragSource_QueryContinueDrag;
      }

      // todo: watch itemspresenter instead of the entire control
      // var itemsPresenter = (sender as DependencyObject).FindDescendent((d) => (d as Panel)?.IsItemsHost ?? false, (d) => d is ItemsControl);  
    }
    private static void IsDropTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var uiElement = (UIElement)d;

      if ((bool)e.NewValue == true) {
        uiElement.AllowDrop = true;

        if (uiElement is ItemsControl) {
          // use normal events for ItemsControls
          uiElement.DragEnter += DropTarget_PreviewDragEnter;
          uiElement.DragLeave += DropTarget_PreviewDragLeave;
          uiElement.DragOver += DropTarget_PreviewDragOver;
          uiElement.Drop += DropTarget_PreviewDrop;
          uiElement.GiveFeedback += DropTarget_GiveFeedback;
        } else {
          // issue #85: try using preview events for all other elements than ItemsControls
          uiElement.PreviewDragEnter += DropTarget_PreviewDragEnter;
          uiElement.PreviewDragLeave += DropTarget_PreviewDragLeave;
          uiElement.PreviewDragOver += DropTarget_PreviewDragOver;
          uiElement.PreviewDrop += DropTarget_PreviewDrop;
          uiElement.PreviewGiveFeedback += DropTarget_GiveFeedback;
        }
      } else {
        uiElement.AllowDrop = false;

        if (uiElement is ItemsControl) {
          uiElement.DragEnter -= DropTarget_PreviewDragEnter;
          uiElement.DragLeave -= DropTarget_PreviewDragLeave;
          uiElement.DragOver -= DropTarget_PreviewDragOver;
          uiElement.Drop -= DropTarget_PreviewDrop;
          uiElement.GiveFeedback -= DropTarget_GiveFeedback;
        } else {
          uiElement.PreviewDragEnter -= DropTarget_PreviewDragEnter;
          uiElement.PreviewDragLeave -= DropTarget_PreviewDragLeave;
          uiElement.PreviewDragOver -= DropTarget_PreviewDragOver;
          uiElement.PreviewDrop -= DropTarget_PreviewDrop;
          uiElement.PreviewGiveFeedback -= DropTarget_GiveFeedback;
        }

        Mouse.OverrideCursor = null;
      }
    }

    public static IDragSource DefaultDragHandler
    {
      get
      {
        if (m_DefaultDragHandler == null) {
          m_DefaultDragHandler = new DefaultDragHandler();
        }

        return m_DefaultDragHandler;
      }
      set { m_DefaultDragHandler = value; }
    }
    public static IDropTarget DefaultDropHandler
    {
      get
      {
        if (m_DefaultDropHandler == null) {
          m_DefaultDropHandler = new DefaultDropHandler();
        }

        return m_DefaultDropHandler;
      }
      set { m_DefaultDropHandler = value; }
    }


    // todo:
    // finish DefaultDragAdorner.xaml
    // fix bindings to <Transaction>
    // trace where Effects is coming from
    // bind template to SourceContainer(s)! <will need to propagate owner (really could get it later) or actually build list early on...
    private static void CreateDragAdorner()
    {
      var template = GetDragAdornerTemplate(m_DragInfo.SourceControl);
      var templateSelector = GetDragAdornerTemplateSelector(m_DragInfo.SourceControl);

      UIElement adornment = null;

      var useDefaultDragAdorner = GetUseDefaultDragAdorner(m_DragInfo.SourceControl);

      if (template == null && templateSelector == null && useDefaultDragAdorner) {        
        if (m_DragInfo.SourceContainer != null) {
          var cont = m_DragInfo.SourceContainer as FrameworkElement;                    
          var factory = new FrameworkElementFactory(typeof(Canvas));          
          factory.SetValue(Panel.WidthProperty, cont.RenderSize.Width);
          factory.SetValue(Panel.HeightProperty, cont.RenderSize.Height);          
          factory.SetValue(Panel.BackgroundProperty, new VisualBrush { Visual = (Visual)m_DragInfo.SourceContainer, Stretch = Stretch.None });          
          
          template = new DataTemplate { VisualTree = factory };
        }
      }

      if (template != null || templateSelector != null) {
        if (m_DragInfo.Data is IEnumerable && !(m_DragInfo.Data is string)) {
          if (!useDefaultDragAdorner && ((IEnumerable)m_DragInfo.Data).Cast<object>().Count() <= 10) {
            var itemsControl = new ItemsControl();
            itemsControl.ItemsSource = (IEnumerable)m_DragInfo.Data;
            itemsControl.ItemTemplate = template;
            itemsControl.ItemTemplateSelector = templateSelector;

            // The ItemsControl doesn't display unless we create a border to contain it.
            // Not quite sure why this is...
            var border = new Border();
            border.Child = itemsControl;
            adornment = border;
          }
        } else {
          var contentPresenter = new ContentPresenter();
          contentPresenter.Content = m_DragInfo.Data;
          contentPresenter.ContentTemplate = template;
          contentPresenter.ContentTemplateSelector = templateSelector;
          adornment = contentPresenter;
        }
      }

      if (adornment != null) {
        if (useDefaultDragAdorner) {
          adornment.Opacity = GetDefaultDragAdornerOpacity(m_DragInfo.SourceControl);
        }

        var parentWindow = m_DragInfo.SourceControl.GetVisualAncestor<Window>();
        var rootElement = parentWindow != null ? parentWindow.Content as UIElement : null;
        if (rootElement == null && Application.Current != null && Application.Current.MainWindow != null) {
          rootElement = (UIElement)Application.Current.MainWindow.Content;
        }
        //                i don't want the fu... windows forms reference
        //                if (rootElement == null) {
        //                    var elementHost = m_DragInfo.VisualSource.GetVisualAncestor<ElementHost>();
        //                    rootElement = elementHost != null ? elementHost.Child : null;
        //                }
        if (rootElement == null) {
          rootElement = m_DragInfo.SourceControl.GetVisualAncestor<UserControl>();
        }

        DragAdorner = new DragAdorner(rootElement, adornment);
      }
    }

    private static void CreateEffectAdorner(DropInfo dropInfo)
    {
      var template = GetEffectAdornerTemplate(m_DragInfo.SourceControl, dropInfo.Effects, dropInfo.DestinationText);

      if (template != null) {
        var rootElement = (UIElement)Application.Current.MainWindow.Content;
        UIElement adornment = null;

        var contentPresenter = new ContentPresenter();
        contentPresenter.Content = new { ActiveEffect = /*dropInfo.Effects*/ DragDropEffects.Move, dropInfo.DestinationText }; //m_DragInfo.Data;        
        contentPresenter.ContentTemplate = template;        
        adornment = contentPresenter;

        EffectAdorner = new DragAdorner(rootElement, adornment, dropInfo.Effects);
      }
    }

    private static DataTemplate GetEffectAdornerTemplate(UIElement target, DragDropEffects effect, string destinationText)
    {
      switch (effect) {
        case DragDropEffects.All:
          return null;
        case DragDropEffects.Copy:
        case DragDropEffects.Move: 
        case DragDropEffects.Link:
          return GetEffectsTemplate(target) ?? (GetUseDefaultEffectsTemplate(target) ? Templates.DefaultTemplates.EffectAdorner : null);
        case DragDropEffects.None:
          return null;//GetEffectNoneAdornerTemplate(target);
        case DragDropEffects.Scroll:
          return null;
        default:
          return null;
      }
    }

    private static void Scroll(DependencyObject visualTarget, DragEventArgs e)
    {
      ScrollViewer scrollViewer = null;

      if (visualTarget is TabControl)
      {
        var tabPanel = visualTarget.GetVisualDescendent<TabPanel>();
        if (tabPanel != null)
        {
          scrollViewer = tabPanel.GetVisualAncestor<ScrollViewer>();
          if (scrollViewer == null)
          {
            return;
          }
        }
      }

      scrollViewer = scrollViewer ?? visualTarget.GetVisualDescendent<ScrollViewer>();
      
      if (scrollViewer != null) {
        var position = e.GetPosition(scrollViewer);
        var scrollMargin = Math.Min(scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

        if (position.X >= scrollViewer.ActualWidth - scrollMargin &&
            scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth) {
          scrollViewer.LineRight();
        } else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0) {
          scrollViewer.LineLeft();
        } else if (position.Y >= scrollViewer.ActualHeight - scrollMargin &&
                   scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) {
          scrollViewer.LineDown();
        } else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0) {
          scrollViewer.LineUp();
        }
      }
    }

    /// <summary>
    /// Gets the drag handler from the drag info or from the sender, if the drag info is null
    /// </summary>
    /// <param name="dragInfo">the drag info object</param>
    /// <param name="sender">the sender from an event, e.g. mouse down, mouse move</param>
    /// <returns></returns>
    private static IDragSource TryGetDragHandler(DragInfo dragInfo, UIElement sender)
    {
      IDragSource dragHandler = null;
      if (dragInfo != null && dragInfo.SourceControl != null)
      {
        dragHandler = GetDragHandler(dragInfo.SourceControl);
      }
      if (dragHandler == null && sender != null)
      {
        dragHandler = GetDragHandler(sender);
      }
      return dragHandler ?? DefaultDragHandler;
    }

    /// <summary>
    /// Gets the drop handler from the drop info or from the sender, if the drop info is null
    /// </summary>
    /// <param name="dropInfo">the drop info object</param>
    /// <param name="sender">the sender from an event, e.g. drag over</param>
    /// <returns></returns>
    private static IDropTarget TryGetDropHandler(DropInfo dropInfo, UIElement sender)
    {
      IDropTarget dropHandler = null;
      if (dropInfo != null && dropInfo.VisualTarget != null)
      {
        dropHandler = GetDropHandler(dropInfo.VisualTarget);
      }
      if (dropHandler == null && sender != null)
      {
        dropHandler = GetDropHandler(sender);
      }
      return dropHandler ?? DefaultDropHandler;
    }

    // this may be raised multiple times, and the final DragInfo will represent the deepest dragable child in the tree...
    private static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {                     
      // Ignore the click on doubleClick, no valid source or handler, or handler doesn't allow drag...
      // perhaps check if there are any intervening DragSource/DragObjects? is this necessary?
      // (after I implement DragObject, it might have a different handler. but so what? it will replace m_DragInfo anyway?...
      var elementPosition = e.GetPosition((IInputElement)sender);
      if (e.ClickCount != 1
          || (bool)((e.OriginalSource as DependencyObject).GetValue(DragSourceIgnoreProperty))
          || (e.OriginalSource as DependencyObject).FindAncestor(p => (bool)p.GetValue(DragSourceIgnoreProperty), r => r == sender) != null
          || (m_DragInfo = new DragInfo((ItemsControl)sender, e)).SourceItem == null
          || (TryGetDragHandler(m_DragInfo, sender as UIElement)?.Dragged(m_DragInfo) ??  DragDropEffects.None) == DragDropEffects.None
      ) { m_DragInfo = null; return; }

      // If the sender is a list box that allows multiple selections, ensure that clicking on an 
      // already selected item does not change the selection, otherwise dragging multiple items 
      // is made impossible.    

      // todo: if we're *not* selected, we want the selection to change...

      // reuse the event args. can't modify till after we are finished routing.
      // should really make new and copy members
      // todo: see if we can raise Preview events instead?
      if ((m_DragInfo.SourceControl as ItemsControl).ReflectSelectedItems().Cast<object>().Contains(m_DragInfo.SourceItem)) {
        m_clickSupressArgs = e;
        e.Handled = true;
      }
    }

    private static void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (sender != m_DragInfo?.SourceControl) { return; }

      // If we prevented the control's default selection handling in DragSource_PreviewMouseLeftButtonDown
      // by setting 'e.Handled = true' and a drag was not initiated, forward the event      
      if (m_clickSupressArgs != null) {
        m_clickSupressArgs.Handled = false;
        m_clickSupressArgs.Source = m_DragInfo.SourceContainer;
        m_clickSupressArgs.RoutedEvent = UIElement.MouseLeftButtonDownEvent;
        (m_DragInfo.SourceContainer as IInputElement).RaiseEvent(m_clickSupressArgs);
        m_clickSupressArgs = null; // could we have a race on this vs. the RaiseEvent?
      }

      m_DragInfo = null;      
    }

    private static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (m_DragInfo != null && !m_DragInProgress) {
        
        // do nothing if mouse left button is released or the pointer is captured
        if (e.LeftButton == MouseButtonState.Released || ((IInputElement)e.OriginalSource).IsMouseCaptured) 
        {
          m_DragInfo = null;
          return;
        } 
        
        var delta = e.GetPosition((IInputElement)sender) - m_DragInfo.DragStartPosition;      
        if (Math.Abs(delta.X) >= SystemParameters.MinimumHorizontalDragDistance ||
            Math.Abs(delta.Y) >= SystemParameters.MinimumVerticalDragDistance) {

          var dragHandler = TryGetDragHandler(m_DragInfo, sender as UIElement);

          m_DragInfo.Effects = dragHandler.Dragged(m_DragInfo);
          if (m_DragInfo.Effects != DragDropEffects.None && m_DragInfo.Data != null) {                                    
              var data = m_DragInfo.DataObject;

              if (data == null) {
                data = new DataObject(DataFormat.Name, m_DragInfo.Data);
              } else {
                data.SetData(DataFormat.Name, m_DragInfo.Data);
              }

              try {
                m_DragInProgress = true;
                var result = System.Windows.DragDrop.DoDragDrop(m_DragInfo.SourceControl, data, m_DragInfo.Effects);
                if (result == DragDropEffects.None)
                  dragHandler.DragCancelled();
              }
              finally {
                m_DragInProgress = false;
              }                          
          }

          m_DragInProgress = false;
          m_DragInfo = null;
        }
      }
    }

    private static void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {      
      if (e.Action == DragAction.Cancel || e.EscapePressed) {
        DragAdorner = null;
        EffectAdorner = null;
        DropTargetAdorner = null;
      } else {
        // check for keystate changes here
        
      }
    }

    private static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
    {
      DropTarget_PreviewDragOver(sender, e);

      Mouse.OverrideCursor = Cursors.Arrow;
    }

    private static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
    {
      DragAdorner = null;
      EffectAdorner = null;
      DropTargetAdorner = null;

      Mouse.OverrideCursor = null;
    }

    private static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
    {
      var elementPosition = e.GetPosition((IInputElement)sender);
      if (HitTestUtilities.HitTest4Type<ScrollBar>(sender, elementPosition)
          || HitTestUtilities.HitTest4GridViewColumnHeader(sender, elementPosition)
          || HitTestUtilities.HitTest4DataGridTypesOnDragOver(sender, elementPosition)) {
        e.Effects = DragDropEffects.None;
        e.Handled = true;
        return;
      }

      var dropInfo = new DropInfo(sender, e, m_DragInfo);
      var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
      var itemsControl = dropInfo.VisualTarget;

      dropHandler.DragOver(dropInfo);

      if (DragAdorner == null && m_DragInfo != null) {
        CreateDragAdorner();
      }

      if (DragAdorner != null) {
        var tempAdornerPos = e.GetPosition(DragAdorner.AdornedElement);

        if (tempAdornerPos.X >= 0 && tempAdornerPos.Y >= 0) {
          _adornerPos = tempAdornerPos;
        }

        // Fixed the flickering adorner - Size changes to zero 'randomly'...?
        if (DragAdorner.RenderSize.Width > 0 && DragAdorner.RenderSize.Height > 0) {
          _adornerSize = DragAdorner.RenderSize;
        }

        if (m_DragInfo != null) {
          // move the adorner
          var offsetX = _adornerSize.Width * -GetDragMouseAnchorPoint(m_DragInfo.SourceControl).X;
          var offsetY = _adornerSize.Height * -GetDragMouseAnchorPoint(m_DragInfo.SourceControl).Y;
          _adornerPos.Offset(offsetX, offsetY);
          var maxAdornerPosX = DragAdorner.AdornedElement.RenderSize.Width;
          var adornerPosRightX = (_adornerPos.X + _adornerSize.Width);
          if (adornerPosRightX > maxAdornerPosX) {
            _adornerPos.Offset(-adornerPosRightX + maxAdornerPosX, 0);
          }
          if (_adornerPos.Y < 0) {
            _adornerPos.Y = 0;
          }
        }

        DragAdorner.MousePosition = _adornerPos;
        DragAdorner.InvalidateVisual();
      }

      // If the target is an ItemsControl then update the drop target adorner.
      if (itemsControl != null) {
        // Display the adorner in the control's ItemsPresenter. If there is no 
        // ItemsPresenter provided by the style, try getting hold of a
        // ScrollContentPresenter and using that.
        var adornedElement =
          itemsControl is TabControl
            ? itemsControl.GetVisualDescendent<TabPanel>()
            : (itemsControl.GetVisualDescendent<ItemsPresenter>() ?? itemsControl.GetVisualDescendent<ScrollContentPresenter>() as UIElement ?? itemsControl);

        if (adornedElement != null) {
          if (dropInfo.DropTargetAdorner == null) {
            DropTargetAdorner = null;
          } else if (!dropInfo.DropTargetAdorner.IsInstanceOfType(DropTargetAdorner)) {
            DropTargetAdorner = DropTargetAdorner.Create(dropInfo.DropTargetAdorner, adornedElement);
          }

          if (DropTargetAdorner != null) {
            DropTargetAdorner.DropInfo = dropInfo;
            DropTargetAdorner.InvalidateVisual();
          }
        }
      }

      // Set the drag effect adorner if there is one
      if (m_DragInfo != null && (EffectAdorner == null || EffectAdorner.Effects != dropInfo.Effects)) {
        CreateEffectAdorner(dropInfo);
      }

      if (EffectAdorner != null) {
        var adornerPos = e.GetPosition(EffectAdorner.AdornedElement);
        adornerPos.Offset(20, 20);
        EffectAdorner.MousePosition = adornerPos;
        EffectAdorner.InvalidateVisual();
      }

      e.Effects = dropInfo.Effects;
      e.Handled = !dropInfo.NotHandled;

      if (!dropInfo.IsSameDragDropContextAsSource)
      {
          e.Effects = DragDropEffects.None;
      }

      Scroll(dropInfo.VisualTarget, e);
    }

    private static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
    {
      var dropInfo = new DropInfo(sender, e, m_DragInfo);
      var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
      var dragHandler = TryGetDragHandler(m_DragInfo, sender as UIElement);

      DragAdorner = null;
      EffectAdorner = null;
      DropTargetAdorner = null;
            
      if (dropInfo.DragInfo.SourceControl == dropInfo.VisualTarget
          || !e.KeyStates.HasFlag(m_DragInfo.DragDropCopyKeyState)
      ) {
        dragHandler.Detach(m_DragInfo);
      }      

      dropHandler.DragOver(dropInfo);
      dropHandler.Drop(dropInfo);
      dragHandler.Dropped(dropInfo);

      Mouse.OverrideCursor = null;
      e.Handled = !dropInfo.NotHandled;
    }

    private static void DropTarget_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      if (EffectAdorner != null) {
        e.UseDefaultCursors = false;
        e.Handled = true;
      } else {
        e.UseDefaultCursors = true;        
        e.Handled = true;
      }
    }

    private static DragAdorner DragAdorner
    {
      get { return m_DragAdorner; }
      set
      {
        if (m_DragAdorner != null) {
          m_DragAdorner.Detach();
        }

        m_DragAdorner = value;
      }
    }
    private static DragAdorner EffectAdorner
    {
      get { return m_EffectAdorner; }
      set
      {
        if (m_EffectAdorner != null) {
          m_EffectAdorner.Detach();
        }

        m_EffectAdorner = value;
      }
    }
    private static DropTargetAdorner DropTargetAdorner
    {
      get { return m_DropTargetAdorner; }
      set
      {
        if (m_DropTargetAdorner != null) {
          m_DropTargetAdorner.Detatch();
        }

        m_DropTargetAdorner = value;
      }
    }

    private static IDragSource m_DefaultDragHandler;
    private static IDropTarget m_DefaultDropHandler;
    private static DragAdorner m_DragAdorner;
    private static DragAdorner m_EffectAdorner;
    private static DragInfo m_DragInfo;
    private static bool m_DragInProgress;
    private static DropTargetAdorner m_DropTargetAdorner;    
    private static MouseButtonEventArgs m_clickSupressArgs;    
    private static Point _adornerPos;
    private static Size _adornerSize;
  }
}

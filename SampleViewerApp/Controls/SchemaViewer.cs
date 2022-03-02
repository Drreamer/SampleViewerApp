using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using WpfApp1;

namespace WpfApp1.Controls {
   /// <summary>
   /// A viewer control used to display Elements  
   /// </summary>
   public class SchemaViewer : Control {
      static SchemaViewer() {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SchemaViewer), new FrameworkPropertyMetadata(typeof(SchemaViewer)));
      }

      Panel canvas;
      double zoom, left, top;

      public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(
          "Elements", typeof(IEnumerable<Model.Element>), typeof(SchemaViewer),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));
      public IEnumerable<Model.Element> Elements {
         get => (IEnumerable<Model.Element>)GetValue(ElementsProperty);
         set => SetValue(ElementsProperty, value);
      }

      public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
          "Zoom", typeof(double), typeof(SchemaViewer),
          new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange));
      public double Zoom {
         get => (double)GetValue(ZoomProperty);
         set => SetValue(ZoomProperty, value);
      }

      public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
           "Center", typeof(Point), typeof(SchemaViewer),
            new FrameworkPropertyMetadata(new Point(double.NaN, double.NaN), FrameworkPropertyMetadataOptions.AffectsArrange));
      public Point Center {
         get => (Point)GetValue(CenterProperty);
         set => SetValue(CenterProperty, value);
      }
            

      public override void OnApplyTemplate() {
         base.OnApplyTemplate();
         canvas = (Canvas)GetTemplateChild("canvas");
      }

      protected override Size ArrangeOverride(Size arrangeBounds) {
         canvas.Children.Clear();
         if (Elements?.Any() == true) {
            double newZoom = Zoom;
            Point newCenter = Center;
            if (double.IsNaN(newZoom) || double.IsNaN(newCenter.X) || double.IsNaN(newCenter.Y)) {
               double left = double.MaxValue;
               double top = double.MaxValue;
               double right = double.MinValue;
               double bottom = double.MinValue;
               foreach (Model.Element el in Elements) {
                  left = Math.Min(left, el.LeftTop.X);
                  top = Math.Min(top, el.LeftTop.Y);
                  right = Math.Max(right, el.BottomRight.X);
                  bottom = Math.Max(bottom, el.BottomRight.Y);
               }
               if (double.IsNaN(newZoom)) newZoom = Math.Min(arrangeBounds.Width / (right - left), arrangeBounds.Height / (bottom - top));
               if (double.IsNaN(newCenter.X) || double.IsNaN(newCenter.Y)) newCenter = new Point((right + left) / 2, (bottom + top) / 2);
               this.zoom = newZoom;
               this.left = newCenter.X - arrangeBounds.Width / zoom / 2;
               this.top = newCenter.Y - arrangeBounds.Height / zoom / 2;

            }
            foreach (Model.Element el in Elements) {
               canvas.Children.Add(CreateUIElement(el));
            }
         }
         else {
         }


         return base.ArrangeOverride(arrangeBounds);
      }

      const double StrokeWidth = 1;
      private FrameworkElement CreateUIElement(Model.Element el) {
         Path path = new Path();
         Brush brush = BrushFromColor(el.Color);
         Model.FilledElement fe = el as Model.FilledElement;
         if (fe != null) {
            path.Fill = brush;
            path.StrokeThickness = 0;
         }
         else {
            path.StrokeThickness = StrokeWidth * zoom;
            path.Stroke = brush;
         }
         Debug.WriteLine(el.Type);

         switch (el.Type) {
            case Model.ElementType.Circle:
               Model.Circle c = (Model.Circle)el;
               path.Data = new EllipseGeometry(GetUIPoint(el, c.Center), c.Radius * zoom, c.Radius * zoom);
               break;
            case Model.ElementType.Line:
               Model.Line l = (Model.Line)el;
               path.Data = new LineGeometry(GetUIPoint(el, l.A), GetUIPoint(el, l.B));
               break;
            case Model.ElementType.Triangle:
               Model.Triangle t = (Model.Triangle)el;
               PathFigure pf = new PathFigure(GetUIPoint(el, t.A),
                   new LineSegment[] {
                      new LineSegment(GetUIPoint(el, t.B), true),
                      new LineSegment(GetUIPoint(el, t.C), true)
                   }, true);
               path.Data = new PathGeometry(new PathFigure[] { pf });
               break;
            default:
               throw new Exception();
         }
         Viewbox viewBox = new Viewbox() {
            Stretch = Stretch.UniformToFill,
            Width = (el.BottomRight.X - el.LeftTop.X) * zoom,
            Height = (el.BottomRight.Y - el.LeftTop.Y) * zoom,
            Child = path
         };
         Canvas.SetLeft(viewBox, (el.LeftTop.X - left) * zoom);
         Canvas.SetTop(viewBox, (el.LeftTop.Y - top) * zoom);

         return viewBox;
      }

      private Point GetUIPoint(Model.Element el, Model.Point p) {
         return new Point((p.X - el.LeftTop.X) * zoom, (p.Y - el.LeftTop.Y) * zoom);
      }

      private Brush BrushFromColor(Model.Color c) {
         return new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
      }
   }
}

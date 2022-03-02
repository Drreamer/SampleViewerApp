using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfApp1.Model {
   public abstract class Element {
      
      [JsonConverter(typeof(StringEnumConverter))]
      public ElementType Type { get; set; }
      public Color Color { get; set; }
      public abstract Point LeftTop { get; }
      public abstract Point BottomRight { get; }
   }

   public abstract class FilledElement : Element {
      public bool Filled { get; set; }
   } 


   public class Line : Element {
      public Point A { get; set; }
      public Point B { get; set; }

      public override Point LeftTop => new Point(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y));
      public override Point BottomRight => new Point(Math.Max(A.X, B.X), Math.Max(A.Y, B.Y));
   }

   public class Circle : FilledElement {
      public Point Center { get; set; }
      public double Radius { get; set; }
      public override Point LeftTop => new Point(Center.X - Radius, Center.Y - Radius);
      public override Point BottomRight => new Point(Center.X + Radius, Center.Y + Radius);

   }

   public class Triangle : FilledElement {
      public Point A { get; set; }
      public Point B { get; set; }
      public Point C { get; set; }
      public override Point LeftTop => new Point(Math.Min(Math.Min(A.X, B.X), C.X), Math.Min(Math.Min(A.Y, B.Y),C.Y));
      public override Point BottomRight => new Point(Math.Max(Math.Max(A.X, B.X), C.X), Math.Max(Math.Max(A.Y, B.Y), C.Y));
   }

   public enum ElementType {
      Line,
      Circle,
      Triangle
   }



}

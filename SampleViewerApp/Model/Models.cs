using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model {

   public struct Point {
      public Point(double x, double y) {
         X = x;
         Y = y;
      }
      public double X { get; set; }
      public double Y { get; set; }
      public override string ToString() {
         return $"{{{X},{Y}}}";
      }
   }
   public struct Color {
      public Color(byte a, byte r, byte g, byte b) {
         A = a;
         R = r;
         G = g;
         B = b;
      }

      public byte A { get; set; }
      public byte R { get; set; }
      public byte G { get; set; }
      public byte B { get; set; }
   }
}

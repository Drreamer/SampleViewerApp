using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Model {


   public abstract class BaseJsonConverter<t> : JsonConverter {
      public override bool CanConvert(Type objectType) {
         return objectType == typeof(t);
      }

      public override bool CanWrite {
         get { return false; }
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
         throw new NotImplementedException(); // won't be called because CanWrite returns false
      }
   }


   public class ElementJsonConverter : BaseJsonConverter<Element> {

      public override bool CanConvert(Type objectType) {
         return objectType.IsAssignableFrom(typeof(Element));
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
         JObject jo = JObject.Load(reader);
         switch (jo["type"].ToObject<ElementType>()) {
            case ElementType.Circle:
               return serializer.Deserialize<Circle>(jo.CreateReader());
            case ElementType.Line:
               return serializer.Deserialize<Line>(jo.CreateReader());
            case ElementType.Triangle:
               return serializer.Deserialize<Triangle>(jo.CreateReader());
            default:
               throw new Exception();
         }
         throw new NotImplementedException();
      }

   }

   public class PointJsonConverter : BaseJsonConverter<Point> {

      static IFormatProvider formatProvider = new CultureInfo("de");
      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
         string[] parts = ((string)reader.Value).Split(';');
         return new Point(double.Parse(parts[0], formatProvider), double.Parse(parts[1], formatProvider));
      }
     
   }

   public class ColorJsonConverter : BaseJsonConverter<Color> {

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
         string[] parts = ((string)reader.Value).Split(';');
         return new Color(byte.Parse(parts[0]), byte.Parse(parts[1]), byte.Parse(parts[2]), byte.Parse(parts[3]));
      }

   }
}

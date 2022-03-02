using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.ViewModels {
   class GraphViewModel : ViewModelBase {

      string json;
      Element[] elements;
      JsonSerializerSettings jsonSettings;
      public GraphViewModel() {
         jsonSettings = new JsonSerializerSettings();
         jsonSettings.Converters.Add(new ElementJsonConverter());
         jsonSettings.Converters.Add(new PointJsonConverter());
         jsonSettings.Converters.Add(new ColorJsonConverter());
         

         Json = File.ReadAllText("SampleData.json");
      }

      public string Json {
         get { return json; }
         set {
            if (value != json) {
               json = value;
               OnPropertyChanged();
               elements = JsonConvert.DeserializeObject<Element[]>(json, jsonSettings);
               OnPropertyChanged(nameof(Elements));
            }
         }
      }

      public IEnumerable<Element> Elements {
         get {
            return elements;
         }
      }


   }
}





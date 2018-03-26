using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace UXC.Plugins.SessionsAPI.Controllers
{
    public class LocalDateTimeJsonConverterAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters
                              .OfType<JsonMediaTypeFormatter>()
                              .ForEach(f =>
                              {
                                  f.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                                  f.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff";
                              });
        }
    }
}

using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics
{
    public class PluginConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool Anonymized { get; set; } = false;
        public string ApiEndpoint { get; set; } = "https://exiledplugins.kingsplayground.fun/api";
    }
}

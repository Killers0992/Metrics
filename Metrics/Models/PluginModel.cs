using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Models
{
    public class PluginModel
    {
        public string PluginName { get; set; }
        public string PluginAuthor { get; set; }
        public string PluginVersion { get; set; }
        public string FileHash { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Models
{
    public class MetricsModel
    {
        public int ServerPort { get; set; }
        public bool Anonymized { get; set; }
        public List<PluginModel> Plugins { get; set; }
    }
}

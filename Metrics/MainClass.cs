using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using Metrics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Metrics
{
    public class MainClass : Plugin<PluginConfig>
    {
        public override string Author { get; } = "Killers0992";
        public override string Name { get; } = "Metrics";
        public override string Prefix { get; } = "metrics";
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);
        public override Version Version { get; } = new Version(1, 0, 0);
        public override PluginPriority Priority => PluginPriority.Last;

        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public override void OnEnabled()
        {
            List<PluginModel> plugins = new List<PluginModel>();
            foreach(var plugin in Loader.Plugins)
            {
                if (plugin == this)
                    continue;
                plugins.Add(new PluginModel()
                {
                    PluginName = plugin.Name,
                    PluginAuthor = plugin.Author,
                    PluginVersion = plugin.Version.ToString(),
                    FileHash = CalculateMD5(plugin.GetPath())
                });
            }
            Task.Factory.StartNew(async () =>
            {
                await IssueMetrics(Config.ApiEndpoint, Config.Debug, new MetricsModel()
                {
                    Anonymized = Config.Anonymized,
                    ServerPort = Server.Port,
                    Plugins = plugins
                });
            });
            base.OnEnabled();
        }

        public static async Task<bool> IssueMetrics(string endpoint, bool debug, MetricsModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpContent = new StringContent(Encoding.UTF8.GetString(Utf8Json.JsonSerializer.Serialize(model)), Encoding.UTF8, "application/json");

                var webRequest = await client.PostAsync($"{endpoint}/sendmetrics", httpContent);

                if (!webRequest.IsSuccessStatusCode)
                {
                    Log.Error($"[SendMetrics] Web API connection error. " + webRequest.StatusCode + " - " + await webRequest.Content.ReadAsStringAsync());
                    return false;
                }

                string apiResponse = await webRequest.Content.ReadAsStringAsync();

                Log.Debug($"[SendMetrics] API Returned: {apiResponse}", debug);
                return true;
            }
        }

    }
}

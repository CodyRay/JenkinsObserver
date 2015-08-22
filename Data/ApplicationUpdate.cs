using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using Contracts;

namespace Data
{
    public static class ApplicationUpdate
    {
        public class GitHubRelease
        {
            [JsonProperty(PropertyName = "html_url")]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "tag_name")]
            public string Tag { get; set; }

            [JsonProperty(PropertyName = "target_commitish")]
            public string Target { get; set; }

            [JsonProperty(PropertyName = "published_at")]
            public DateTime Publish { get; set; }

            public string Name { get; set; }
            public string Body { get; set; }
            public bool Draft { get; set; }
            public bool Prerelease { get; set; }
            public bool ShouldUpgrade => !Prerelease && !Draft && Target == "master";
        }

        public static bool IsAvailable(Uri gitHubRelease, string currentVersion, out Uri uri)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "JenkinsObserver");
                    var releases = JsonConvert.DeserializeObject<IEnumerable<GitHubRelease>>(http.GetAsync(gitHubRelease).Result.Content.ReadAsStringAsync().Result);
                    var newestRelease = releases.Where(r => r.ShouldUpgrade).OrderByDescending(r => r.Publish).First();

                    var localVersion = currentVersion.Split(' ', '.')
                        .Take(2).Select(int.Parse).ToArray();
                    var gitHubTag = newestRelease.Tag.Trim().Replace("v", "").Split('.', '-', ' ')
                        .Take(2).Select(int.Parse).ToArray();

                    if (gitHubTag[0] > localVersion[0] || gitHubTag[1] > localVersion[1])
                    {
                        uri = new Uri(newestRelease.Url);
                        return true;
                    }
                }
            }
            catch (Exception ex)//If there is a horific error here we will just let them run this version forever
            {
                var str = ex.Message;
            }
            uri = null;
            return false;
        }
    }
}
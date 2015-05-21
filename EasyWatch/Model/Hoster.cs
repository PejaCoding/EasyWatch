using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace EasyWatch.Model
{
    internal class Hoster
    {
        public static async Task<string> StreamCloud(string streamcloudUrl)
        {


            using (HttpClient client = new HttpClient())
            {

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("op", "download2"),
                    new KeyValuePair<string, string>("usr", "login"),
                    new KeyValuePair<string, string>("id", streamcloudUrl.Split('/')[3]),
                    new KeyValuePair<string, string>("fname", streamcloudUrl.Split('/')[4]),
                    new KeyValuePair<string, string>("iamhuman", "Weiter zum Video")
                });


                var response = await client.PostAsync(streamcloudUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var regex = new Regex("file: \"(.*?)\",");
                Match match = regex.Match(responseString);

                return match.Groups[1].Value;
            }
        }

        public static async Task<string> Vivo(string vivoUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                string vivoSource = await client.GetStringAsync(vivoUrl);
                var regexHash = new Regex("\"hash\" value=\"(.*?)\"");
                var regexExpires = new Regex("\"expires\" value=\"(.*?)\"");
                var regexTimestamp = new Regex("\"timestamp\" value=\"(.*?)\"");
                Match matchHash = regexHash.Match(vivoSource); //matchVivo.Groups[1].Value)
                Match matchExpires = regexExpires.Match(vivoSource); //matchExpires.Groups[1].Value)
                Match matchTimestamp = regexTimestamp.Match(vivoSource); //matchTimestamp.Groups[1].Value)
                string hash = matchHash.Groups[1].Value;
                string expires = matchExpires.Groups[1].Value;
                string timestamp = matchTimestamp.Groups[1].Value;


                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("hash", hash),
                    new KeyValuePair<string, string>("expires", expires),
                    new KeyValuePair<string, string>("timestamp", timestamp),
                });

                var response = await client.PostAsync(vivoUrl, formContent);
                var stringContent = await response.Content.ReadAsStringAsync();
                var regex = new Regex("data-url=\"(.*?)\"");
                Match match = regex.Match(stringContent);
                return match.Groups[1].Value;

            }



        }
    }
}


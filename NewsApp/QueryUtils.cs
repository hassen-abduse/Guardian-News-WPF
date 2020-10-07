using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewsApp
{
    class QueryUtils
    {
        private QueryUtils() { }
        public static List<Item> FetchNewsData(String requestUrl)
        {
            Uri url = CreateUrl(requestUrl);
            String jsonResponse = null;

            try {
                jsonResponse = MakeHttpRequest(url);
                } catch (IOException e)
            {
                Console.WriteLine("Problem making HTTP request.", e);
            }
            
            List<Item> news = ExtractFeatureFromJson(jsonResponse);
            return news;

        }
        private static Uri CreateUrl(string stringUrl)
        {
            Uri url = null;
            try
            {
                url = new Uri(stringUrl);
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("problem building the url", e);
            }
            return url;

        }
        private static string MakeHttpRequest(Uri url)
        {
            string jsonResponse = "";
            if (url == null)
            {
                return jsonResponse;
            }

            HttpWebRequest urlConnection = null;
            Stream inputStream = null;
            try
            {
                urlConnection = (HttpWebRequest)WebRequest.Create(url);
                //urlConnection.Timeout = 10000;
                urlConnection.Method = "GET";
                urlConnection.KeepAlive = false;

                HttpWebResponse response = (HttpWebResponse)urlConnection.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    inputStream = response.GetResponseStream();
                    jsonResponse = ReadFromStream(inputStream);
                }
                else
                {
                    Console.WriteLine("Error response code", response.StatusCode);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Problem retrieving the news JSON results", e);
            }
            catch (WebException e)
            {
                Console.WriteLine("Cannot connect to the url");
            }
            finally
            {
                if (urlConnection != null)
                {
                    urlConnection.Abort();

                }
                if (inputStream != null)
                {
                    inputStream.Close();
                }
            }
            return jsonResponse;

        }


        private static string ReadFromStream(Stream inputStream)
        {
            StringBuilder output = new StringBuilder();

            if (inputStream != null)
            {
                StreamReader inputStreamReader = new StreamReader(inputStream);
                string line = inputStreamReader.ReadLine();
                while (line != null)
                {
                    output.Append(line);
                    line = inputStreamReader.ReadLine();
                }

            }
            return output.ToString();
        }

        private static List<Item> ExtractFeatureFromJson(string newsJSON)
        {
            if (string.IsNullOrEmpty(newsJSON))
            {
                return null;
            }

            List<Item> news = new List<Item>();

            try
            {
                JObject baseJsonResponse = JObject.Parse(newsJSON);
                JObject response = (JObject)baseJsonResponse["response"];
                JArray results = (JArray)response["results"];
                
                for (int i = 0; i < results.ToArray().Length; i++)
                {
                    JObject properties = (JObject)results[i];
                    string title = properties["webTitle"].ToString();
                    string pubDate = properties["webPublicationDate"].ToString();
                    string url = properties["webUrl"].ToString();
                    string sectionName = properties["sectionName"].ToString();

                    JObject fld = null;
                    String imgUrl;
                    try
                    {
                        fld = (JObject)properties["fields"];

                         imgUrl = fld["thumbnail"].ToString();
                    }
                    catch (NullReferenceException)
                    {
                        return null;
                    }
                    Item item = new Item(title, sectionName, url, pubDate, imgUrl);
                    news.Add(item);

                    
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine("Problem parsing the news JSON results", e);
            }
            return news;
        }
    }
}



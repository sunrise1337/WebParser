using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class Parser : IParser<Data>
    {
        const string http = "http://";
        const string https = "https://";

        Data data;
        HtmlDocument htmlDoc;
        string _url;

        Data IParser<Data>.Parse(string url)
        {
            if (url.Length <= 0)
            {
                MessageBox.Show("Enter URL please");
                return null;
            }

            if (!(url.StartsWith(http) || url.StartsWith(https)))
                url = url.Insert(0, http);

            _url = url;


            GetInitialInfo();
            GetTitle();
            GetDescription();
            GetHeaders();
            GetImages();
            GetLinks();

            return data;
        }

        public void GetInitialInfo()
        {
            data = new Data();
            data.H1Headers = new List<string>();
            data.Images = new List<string>();
            data.Links = new List<string>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                timer.Stop();
                data.ResponseTime = timer.Elapsed;

                if (request.HaveResponse)
                {
                    data.ServerResponse = response.StatusCode.ToString();

                    string stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(stream);
                }

                response.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void GetTitle()
        {
            try
            {
                var nodeTitle = htmlDoc.DocumentNode.SelectNodes("//title");
                data.Title = nodeTitle["title"].InnerText;
            }

            catch (Exception)
            {
                data.Title = "Some error occurred :(";
            }
        }

        void GetDescription()
        {
            try
            {
                var nodeMeta = htmlDoc.DocumentNode.SelectNodes("//meta");
                if (nodeMeta != null)
                {
                    foreach (var tag in nodeMeta)
                    {
                        if (tag.Attributes["name"] != null && tag.Attributes["name"].Value == "description")
                        {
                            data.Description = tag.Attributes["content"].Value;
                        }
                    }
                }
            }
            catch (Exception)
            {
                data.Description = "Some error occurred :(";
            }
        }

        void GetHeaders()
        {
            try
            {
                var nodesH1 = htmlDoc.DocumentNode.SelectNodes("//h1");
                if (nodesH1 != null)
                {
                    foreach (var tag in nodesH1)
                    {
                        data.H1Headers.Add(tag.InnerText);
                    }
                }
            }

            catch (Exception)
            {
                data.H1Headers.Add("Some error occurred :(");
            }
        }

        void GetImages()
        {
            try
            {
                foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                {
                    if (node.Attributes["src"] != null)
                    {
                        data.Images.Add(node.Attributes["src"].Value);
                    }
                }
            }

            catch (Exception)
            {
                data.Images.Add("Some error occurred or no img found :(");
            }
        }

        void GetLinks()
        {
            try
            {
                var nodesA = htmlDoc.DocumentNode.SelectNodes("//a");
                if (nodesA != null)
                {
                    foreach (var tag in nodesA)
                    {
                        if (tag.Attributes["href"] != null)
                        {
                            data.Links.Add(tag.Attributes["href"].Value);
                        }
                    }
                }
            }

            catch (Exception)
            {
                data.Images.Add("Some error occurred or no links found :(");
            }
        }
    }
}

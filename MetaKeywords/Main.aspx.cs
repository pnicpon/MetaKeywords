using HtmlAgilityPack;
using MSHTML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaKeywords
{
    public partial class Main : System.Web.UI.Page
    {
        protected void btnClick_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == false)
            {
                pnlMessage.Visible = true;
                ltrMessage.Text = "Wprowadź adres url";

                return;
            }

            try
            {
                string url = txtUrl.Text.Trim();

                if (url.StartsWith("http://") == false && url.StartsWith("https://") == false)
                    url = "http://" + url;

                pnlPreview.Visible = pnlMessage.Visible = pnlResult.Visible = pnlResult2.Visible = false;

                CheckByMSHTML(url);
                CheckByHtmlAgilityPack(url);
                
                if (cbShowPage.Checked)
                {
                    frame.Src = url;
                    pnlPreview.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                pnlResult.Visible = pnlResult2.Visible = pnlPreview.Visible = false;

                ltrMessage.Text = "Nie udało się pobrać zawartości strony.";
            }
        }
        
        #region Private Methods

        private void CheckByMSHTML(string url)
        {
            // Get page from url
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            
            string responseFromServer = reader.ReadToEnd();
            
            reader.Close();
            dataStream.Close();
            response.Close();
            
            IHTMLDocument2 doc = new HTMLDocumentClass();
            doc.write(new object[] { responseFromServer });
            doc.close();

            // Get keywords list
            List<string> keywordsList = new List<string>();
            foreach (IHTMLElement el in (IHTMLElementCollection)doc.all)
            {
                if (el.tagName == "META")
                {
                    HTMLMetaElement meta = (HTMLMetaElement)el;
                    if (string.IsNullOrEmpty(meta.name) || meta.name.ToLower() != "keywords" || string.IsNullOrEmpty(meta.content))
                        continue;

                    keywordsList.AddRange(meta.content.Split(',').ToList());
                }
            }
            
            string innerText = doc.body.innerText.ToLower();
            string result = "<b>Użycie MSHTML</b><br/>";

            if (innerText != null && keywordsList.Count != 0)
            {
                foreach (string key in keywordsList)
                {
                    int count = 0;
                    int i = 0;

                    while ((i = innerText.IndexOf(key.ToLower(), i)) != -1)
                    {
                        i += key.Length;
                        count++;
                    }

                    //foreach (Match match in Regex.Matches(bodyHtml, key, RegexOptions.IgnoreCase))
                    //{
                    //    count++;
                    //}

                    result += "<br/>" + key + ": " + count;
                }
            }
            else
                result += "<br/> - nie zwróciło wyników";

            pnlResult.Visible = true;
            ltrInfo.Text = result;
        }

        private void CheckByHtmlAgilityPack(string url)
        {
            // HtmlAgilityPack
            List<string> keywordsList = new List<string>();

            var webGet = new HtmlWeb();
            var document = webGet.Load(url);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    if (tagName != null && tagContent != null && tagName.Value.ToLower() == "keywords")
                    {
                        keywordsList.AddRange(tagContent.Value.Split(',').ToList());
                    }
                }
            }

            string innerText = document.DocumentNode.SelectSingleNode("//body").InnerText.ToLower();
            string result = "<b>Użycie HtmlAgilityPack</b><br/>";

            if (string.IsNullOrWhiteSpace(innerText) == false && keywordsList.Count != 0)
            {
                foreach (string key in keywordsList)
                {
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    int count = 0;
                    foreach (Match match in Regex.Matches(innerText, key.Trim(), RegexOptions.IgnoreCase))
                    {
                        count++;
                    }

                    result += "<br/>" + key.Trim() + ": " + count;
                }
            }
            else
                result += "<br/> - nie zwróciło wyników";

            pnlResult2.Visible = true;
            ltrInfo2.Text = result;
        }

        #endregion Private Methods
    }
}
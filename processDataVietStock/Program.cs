using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace processDataVietStock
{
    class Program
    {
        static void Main(string[] args)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            List<data> listData = db.datas.ToList();

            for (int j = 0; j < listData.Count; j++)
            {
                data data = listData[j];
                string fullHtml = data.full_html;
                fullHtml = fullHtml.Replace(@"u003c", "<");
                fullHtml = fullHtml.Replace(@"\", "");
                fullHtml = fullHtml.Replace(@"u003e", ">");
                fullHtml = fullHtml.Replace(@"u0027", "'");

                var doc = new HtmlDocument();
                doc.LoadHtml(fullHtml);

                var arrayLinkOfNews = doc.DocumentNode.SelectNodes("//div[contains(@class,'FFull_News_Title')]//a");
                var arrayDateOfNews = doc.DocumentNode.SelectNodes("//label[contains(@class,'FFull_News_DateTime')]");

                List<detail> listDetail = new List<detail>();
                detail detail;
                for (int i = 0; i < arrayLinkOfNews.Count; i++)
                {
                    detail = new detail();
                    detail.macp = data.ma_doanh_nghiep;
                    detail.url = arrayLinkOfNews[i].GetAttributeValue("href", "unknown");
                    detail.title = arrayLinkOfNews[i].InnerText;
                    detail.date = arrayDateOfNews[i].InnerText;
                    listDetail.Add(detail);
                }

                db.details.InsertAllOnSubmit(listDetail);
                db.SubmitChanges();
                Console.WriteLine("Xong record thu: " + j);
                //Console.ReadLine();
            }

          
            Console.WriteLine("Xong het");
            
            Console.ReadLine();
        }
    }
}

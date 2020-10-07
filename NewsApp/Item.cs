using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp
{
    public class Item
    {

        private string title;
        private string sectionName;
        private string webUrl;
        private string publicationDate;
        private string imgUrl;

        // constructor
        public Item(string title, string sectionName, string webUrl, string publicationDate, string imgUrl)
        {
            this.Title = title;
            this.SectionName = sectionName;
            this.WebUrl = webUrl;
            this.PublicationDate = publicationDate;
            this.ImageUrl = imgUrl;

        }
        // constructor
        public Item(string title, string sectionName, string webUrl, string publicationDate)
        {
            this.Title = title;
            this.SectionName = sectionName;
            this.WebUrl = webUrl;
            this.PublicationDate = publicationDate;

        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string SectionName
        {
            get { return sectionName; }
            set { sectionName = value; }
        }

        public string WebUrl
        {
            get { return webUrl; }
            set { webUrl = value; }
        }
        public string PublicationDate
        {
            get { return publicationDate; }
            set { publicationDate = value; }
        }
        public String ImageUrl
        {
            get { return imgUrl; }
            set { imgUrl = value; }
        }
    }
}

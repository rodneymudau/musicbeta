using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MusicBeta1.Models
{
    public class UploadTooBigViewModel
    {
        public UploadTooBigViewModel()
        {
            MaxRequestLength = GetMaxRequestLength();
        }
        [Key]
        public int itemID { get; set; }

        public string MaxRequestLength { get; private set; }

        private string GetMaxRequestLength()
        {
            const int DEFAULT_VALUE = 214748364;

            int byteQty = DEFAULT_VALUE;

            var fileSpec = HttpContext.Current.Server.MapPath("~/web.config");

            if (System.IO.File.Exists(fileSpec))
            {
                var contents = System.IO.File.ReadAllText(fileSpec);
                var doc = new System.Xml.XmlDocument();

                /* We'll assume the web.config file is a valid XML document. We wouldn't have made it 
                    * this far otherwise. */
                doc.LoadXml(contents);

                var xPath = "//configuration/system.web/httpRuntime/@maxRequestLength";
                var node = doc.SelectSingleNode(xPath);
                byteQty = node == null ? DEFAULT_VALUE : int.Parse(node.Value);
            }

            var result = ReduceBytes(byteQty * 1024);
            return result;
        }
        private string ReduceBytes(int byteQty)
        {
            float byteResult = byteQty;
            const string PREFIX = " KMGTPEZY";
            var index = 0;

            while (byteResult > 1024)
            {
                byteResult /= 1024F;
                index++;
            }

            var result = byteResult.ToString("#,##0.0") + " " + PREFIX.Substring(index, 1) + "B";
            return result;
        }
    }
}
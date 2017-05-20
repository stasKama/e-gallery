using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Web_gellary.Models
{
    public class ImageSave
    {
        public static void Save(string Data, string Path)
        {
            var dataIndex = Data.IndexOf("base64", StringComparison.Ordinal) + 7;
            var cleareData = Data.Substring(dataIndex);
            var fileInformation = Convert.FromBase64String(cleareData);
            var bytes = fileInformation.ToArray();
            var fileStream = File.Create(Path);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

    
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret2020.WebUI.AppClasses
{
    public class Context
    {
        private static Model1 baglanti;
        public static Model1 Baglanti
        {
            get
            {
                if (baglanti==null)
                {
                    baglanti = new Model1();
                } return baglanti; }
            set { baglanti = value;}
        }
    }
}
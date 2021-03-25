using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace FirmadorDeXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string archivoFirma = "Firma RM 2020.pfx";
            X509Certificate2 cert = new X509Certificate2(archivoFirma, "agpsa2020", X509KeyStorageFlags.Exportable);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load("documento.xml");
            MetodosFirmadores metodos = new MetodosFirmadores();
            metodos.SignXml(xmlDoc, cert);
            xmlDoc.Save(@"documentoFirmado.xml");
        }
    }
}

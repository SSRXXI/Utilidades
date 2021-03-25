using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace FirmadorDeXml
{
	/// 
	///	Firma un archivo XML con un certificado
	/// 
	public class MetodosFirmadores
	{
        public void Firmar()
        {
            string archivoFirma = "Firma RM 2020.pfx";
            X509Certificate2 cert = new X509Certificate2(archivoFirma, "agpsa2020", X509KeyStorageFlags.Exportable);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load("documento.xml");
            SignXml(xmlDoc, cert);
            xmlDoc.Save(@"documentoFirmado.xml");
        }

        public void SignXml(XmlDocument xmlDoc, X509Certificate2 cert)
        {
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (cert == null)
                throw new ArgumentException("Cert");

            var exportedKeyMaterial = cert.PrivateKey.ToXmlString( /* includePrivateParameters = */ true);
            var key = new RSACryptoServiceProvider(new CspParameters(24 /* PROV_RSA_AES */));
            key.PersistKeyInCsp = false;
            key.FromXmlString(exportedKeyMaterial);

            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = key;
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            Reference reference = new Reference();
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            reference.Uri = "";
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

        }
    }
}

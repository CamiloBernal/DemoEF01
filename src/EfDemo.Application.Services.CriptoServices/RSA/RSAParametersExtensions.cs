using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace EfDemo.Application.Services.CriptoServices.RSA
{
    public static class RsaParametersExtensions
    {
        public static string ToXmlString(this RSAParameters rsaParameters)
        {
            var xmlSerializer = new XmlSerializer(rsaParameters.GetType());
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, rsaParameters);
                return textWriter.ToString();
            }
        }
    }
}
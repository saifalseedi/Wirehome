﻿using System.Linq;
using System.Xml.Linq;
using System.IO;
using Wirehome.Extensions.Devices;

namespace Wirehome.Extensions.Messaging.DenonMessages
{
    public class DenonStatusMessage : HttpMessage
    {
        public DenonStatusMessage()
        {
            RequestType = "GET";
        }

        public override string MessageAddress()
        {
            return $"http://{Address}/goform/formMainZone_MainZoneXmlStatus.xml";
        }

        public override object ParseResult(string responseData)
        {
            using (var reader = new StringReader(responseData))
            {
                var xml = XDocument.Parse(responseData);

                var renamed = xml.Descendants("InputFuncList").Descendants("value").Select(x => x.Value.Trim()).ToList();
                var input = xml.Descendants("RenameSource").Descendants("value").Descendants("value").Select(x => x.Value.Trim()).ToList();
                
                return new DenonDeviceInfo
                {
                    InputSources = input.Zip(renamed, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v),
                    Surround = xml.Descendants("SurrMode").FirstOrDefault()?.Value?.Trim(),
                    Model = xml.Descendants("Model").FirstOrDefault()?.Value?.Trim()
                };
            }
        } 
    }
}
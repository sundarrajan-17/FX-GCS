using System.Xml.Serialization;

namespace XagSurveillanceGCS.Utilities.CoT
{
    public class contact
    {
        [XmlAttribute] public string callsign;
        [XmlAttribute] public string endpoint;
    }
}
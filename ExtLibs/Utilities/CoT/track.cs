using System.Xml.Serialization;

namespace XagSurveillanceGCS.Utilities.CoT
{
    public class track
    {
        [XmlAttribute] public string course;
        [XmlAttribute] public string speed;
    }
}
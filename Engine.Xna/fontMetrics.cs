using System.Xml.Serialization;

namespace Engine.Xna
{
    public partial class fontMetrics
    {
        /// <remarks />
        [XmlElement("character")]
        public FontMetricsCharacter[] character { get; set; }

        /// <remarks />
        [XmlAttribute()]
        public string file { get; set; }
    }
}
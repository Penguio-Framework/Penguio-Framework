using System.Xml.Serialization;

namespace Engine.Xna
{
    /// <remarks />
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FontMetricsCharacter
    {
        /// <remarks />
        public int x { get; set; }

        /// <remarks />
        public int y { get; set; }

        /// <remarks />
        public int width { get; set; }

        /// <remarks />
        public int height { get; set; }

        /// <remarks />
        [XmlAttribute()]
        public int character { get; set; }
    }
}
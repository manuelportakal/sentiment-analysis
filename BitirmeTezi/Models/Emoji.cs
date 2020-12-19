using System;
using System.Collections.Generic;

#nullable disable

namespace BitirmeTezi.Models
{
    public partial class Emoji
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string TemelGrup { get; set; }
        public string Grup { get; set; }
        public string SadnessPoint { get; set; }
        public string AngerPoint { get; set; }
        public string LovePoint { get; set; }
        public string SurprisePoint { get; set; }
        public string FearPoint { get; set; }
        public string JoyPoint { get; set; }
        public string Codepoints { get; set; }
        public string Emo { get; set; }
    }
}

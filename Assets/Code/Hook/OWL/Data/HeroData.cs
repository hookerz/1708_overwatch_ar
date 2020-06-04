using System;

namespace Hook.OWL
{
    [Serializable]
    public class HeroData
    {
        #region Properties

        public string Hero;
        public int UsageCount;
        public int Eliminations;
        public int FinalBlows;
        public int Deaths;
        public int Assists;
        public string HeroImageUrl;
        public double Percent;

        #endregion
    }
}
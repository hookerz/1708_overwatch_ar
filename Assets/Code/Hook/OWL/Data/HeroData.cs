using System;

namespace Hook.OWL
{
    [Serializable]
    public class MatchData
    {
        public string TimePlayed;
    }
    
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
        public double HeroDamage;
        public double BarrierDamage;
        public double DamageTaken;
        public double HealingReceived;
        public double DamageBlocked;
        public int UltimatesUsed;
        public int SoloKills;
        public string HeroImageUrl;
        public double Percent;

        #endregion
    }
}
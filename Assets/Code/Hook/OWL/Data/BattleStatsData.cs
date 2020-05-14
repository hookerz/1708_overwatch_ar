using System;

namespace Hook.OWL
{
    [Serializable]
    public class BattleStatsData
    {
        #region Properties
        
        public string Map;
        public string Team;
        public string Player;
        public string Hero;
        public int Elims;
        public int FinalBlows;
        public int Deaths;
        public int Assists;
        public int HeroDamage;
        public int Healing;
        public int BarrierDamage;
        public int DamageTaken;
        public int HealingReceived;
        public int DamageBlocked;
        public int DamageAmp;
        public int UltimatesUsed;
        public double UltimatesEarned;
        public int OffensiveAssists;
        public int DefensiveAssists;
        public int SoloKills;
        public int EnvKills;
        public int CriticalHitKills;
        public DateTime TimePlayed;
        
        #endregion
        
        #region Class Methods

        public override string ToString()
        {
            return LitJson.JsonMapper.ToJson(this);
        }

        #endregion
    }
}
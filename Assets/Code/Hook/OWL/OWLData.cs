using System;
using Hook.HXF;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hook.OWL
{
    public enum OverwatchCharacter
    {
        
    }
    public class OWLData
    {
        #region Properties

        private DatabaseManager _db;
    
        #endregion
    
        #region Constructor

        public OWLData(string databaseName)
        {
            _db = new DatabaseManager(databaseName);
        }

        ~OWLData()
        {
        }
    
        #endregion
    
        #region Class Methods

        public List<TeamData> GetTeams()
        {
            string query = String.Format("SELECT * FROM Teams");
            var results = _db.RunQuery<List<TeamData>>(query);
            
            // getting team roster
            results.ForEach(team => { team.Roster = GetTeamRoster(team.TeamName); });

            return results;
        }
        
        public TeamRosterData GetTeamRoster(string teamName)
        {
            string query = String.Format("SELECT PlayerName, OverwatchName, ProfileImageUrl FROM Players P INNER JOIN Teams T ON P.TeamId = T.TeamId WHERE lower(T.TeamName) = '{0}'", teamName.ToLower());

            var roster = new TeamRosterData(_db.RunQuery<List<PlayerProfileData>>(query));

            return roster;
        }
    
        public List<HeroData> GetAllHeroes(string playerName)
        {
            string query = String.Format(@"
                SELECT  Hero, 
                        COUNT(Hero) AS UsageCount, 
                        sum(B.Elims) AS Eliminations, 
                        sum(B.FinalBlows) AS FinalBlows, 
                        sum(B.Deaths) AS Deaths, 
                        sum(B.Assists) AS Assists,
                        sum(B.HeroDamage) AS HeroDamage,
                        sum(B.BarrierDamage) AS BarrierDamage,
                        sum(B.DamageTaken) AS DamageTaken,
                        sum(B.HealingReceived) AS HealingReceived,
                        sum(B.DamageBlocked) AS DamageBlocked,
                        sum(B.UltimatesUsed) AS UltimatesUsed,
                        sum(B.SoloKills) AS SoloKills,
                        H.ImageAsset AS HeroImageUrl
                FROM BattleStats B 
                INNER JOIN Heroes H
                WHERE lower(B.Player)='{0}' AND Hero == H.Name
                GROUP BY B.Hero"
                , playerName.ToLower());
            
            var data = _db.RunQuery<List<HeroData>>(query);

            // calculating percent used for each hero
            double total = data.Sum(hero => hero.UsageCount);
            data.ForEach(hero => hero.Percent = hero.UsageCount / total);
            
            // calculting totals for all hero stats
            var heroTotals = new HeroData();
            heroTotals.Hero = "Total";
            heroTotals.UsageCount = data.Sum(hero => hero.UsageCount);
            heroTotals.Eliminations = data.Sum(hero => hero.Eliminations);
            heroTotals.FinalBlows = data.Sum(hero => hero.FinalBlows);
            heroTotals.Deaths = data.Sum(hero => hero.Deaths);
            heroTotals.Assists = data.Sum(hero => hero.Assists);
            heroTotals.HeroDamage = data.Sum(hero => hero.HeroDamage);
            heroTotals.BarrierDamage = data.Sum(hero => hero.BarrierDamage);
            heroTotals.DamageTaken = data.Sum(hero => hero.DamageTaken);
            heroTotals.HealingReceived = data.Sum(hero => hero.HealingReceived);
            heroTotals.DamageBlocked = data.Sum(hero => hero.DamageBlocked);
            heroTotals.UltimatesUsed = data.Sum(hero => hero.UltimatesUsed);
            heroTotals.SoloKills = data.Sum(hero => hero.SoloKills);
            
            // adding total to hero list
            data.Add(heroTotals);
            
            return data;
        }

        public List<BattleStatsData> GetBattleStats(string playerName)
        {
            string query = String.Format("SELECT * FROM BattleStats B WHERE lower(B.Player) = '{0}'", playerName.ToLower());
        
            var data = _db.RunQuery<List<BattleStatsData>>(query);
            
            Debug.Log("Results: [ data: " + data + ", count: " + data.Count + " ]");
            
            return data;
        }

        public List<BattleStatsData> GetBattleStats(string playerName, string hero)
        {
            string query = String.Format("SELECT * FROM BattleStats B WHERE lower(B.Player) = '{0}' AND lower(B.Hero) = '{1}'", playerName.ToLower(), hero.ToLower());
        
            var data = _db.RunQuery<List<BattleStatsData>>(query);

            return data;
        }
        
        public List<BattleStatsData> GetBattleStats(string playerName, string hero, string map)
        {
            string query = String.Format("SELECT * FROM BattleStats B WHERE lower(B.Player) = '{0}' AND lower(B.Hero) = '{1}' AND lower(B.Map) = '{2}'", playerName.ToLower(), hero.ToLower(), map.ToLower());
            Debug.Log(query);
        
            var data = _db.RunQuery<List<BattleStatsData>>(query);

            return data;
        }
        
        #endregion
    }
}
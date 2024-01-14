using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AwardDetailsDTORepository : IAwardDetailsDTORepository
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public AwardDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public AwardDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }


        public AwardSummaryDTO GetAwardSummaryDTO(int userid)
        {
            String[] awardLevel = new String[] { "Noob", "FreshMan", "Rookie", "Master" };
            AwardSummaryDTO awdSummary = new AwardSummaryDTO
            {
                Level = awardLevel[random.Next(awardLevel.Length)],
                Score = random.Next(1, 1000),
                TotalAchivement = random.Next(1, 100)
            };
            return awdSummary;
        }


        public IQueryable<AchievementDTO> GetAchievementDTO(int userid, string achievementType)
        {

            List<AchievementDTO> achievementlist = new List<AchievementDTO>();
            String[] achievementName = new String[] { "Big Big Mac", "Wire To Wire", "Shrewed BusinessMan", "College Grad" };
            for (int i = 0; i < 4; i++)
                achievementlist.Add(new AchievementDTO
                    {
                        AchievementName = achievementName[random.Next(achievementName.Length)],
                        AchievedPercent = Math.Round(Convert.ToDecimal(random.NextDouble() * 100), 2),
                        Score = random.Next(10, 50000)
                    });
            return achievementlist.AsQueryable();
        }


        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}

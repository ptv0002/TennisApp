using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ScoreCalculation
    {
        private readonly DbContext _context;
        public ScoreCalculation(DbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get point deposit from each pair in the level
        /// Use parameters from level to determine
        /// Round Id for this "round" is 9
        /// </summary>
        /// <param name="idTrinh">Level id</param>
        /// <returns>List of score deposit before starting Table round</returns>
        public void Point_Deposit(int idTrinh)
        {
            var pointList = new List<DS_Diem>();
            var level = _context.Set<DS_Trinh>().Find(idTrinh);
            var pairs = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.ID_Trinh == idTrinh);
            // Total deposit points from all pairs in level: B_DT
            int T_DT = 0;
            foreach (var pair in pairs)
            {
                // Total point of pair before starting : C_D1
                var C_D1 = pair.VDV1.Diem + pair.VDV2.Diem;
                // Deposite point taken off from each pair : CD_DT
                int CD_DT = level.Diem_Tru * 2;
                if (C_D1 < level.Trinh - level.Diem_Tru * 2) CD_DT -= Math.Min(level.Diem_Tru * 2, level.Trinh - level.Diem_Tru * 2 - C_D1);
                else if (C_D1 > level.Trinh + level.Diem_Tru * 2) CD_DT += Math.Max(0, C_D1 - level.Trinh - level.Diem_Tru * 3);
                
                // Find if pair id is in the DB for this "round"
                // If yes, update
                if (_context.Set<DS_Diem>().Any(m => m.ID_Cap == pair.Id && m.ID_Vong == 9))
                {
                    var temp = _context.Set<DS_Diem>().FirstOrDefault(m => m.ID_Cap == pair.Id && m.ID_Vong == 9);
                    temp.Diem = CD_DT;
                    _context.Update(temp);
                }
                // If not, add new instance
                else
                {
                    _context.Add(new DS_Diem
                    {
                        Diem = CD_DT,
                        ID_Cap = pair.Id,
                        ID_Vong = 10
                    });
                }

                // Update điểm phân bổ for level
                T_DT += CD_DT;
            }
            level.Tong_Diem = T_DT;
            _context.Update(level);
            _context.SaveChanges();
        }
        /// <summary>
        /// Calculate points for pairs at Table rounds
        /// </summary>
        /// <param name="idTrinh">Level id</param>
        /// <param name="table">Table's name</param>
        /// <returns>List of pair Id and corresponding point</returns>
        public List<DS_Diem> TableAndPositive_Point(int idTrinh, char table)
        {
            var matches = _context.Set<DS_Tran>().Where(m => m.ID_Trinh == idTrinh && m.Ma_Tran[7] == table).ToList();
            var pairs = _context.Set<DS_Cap>().Include(m => m.DS_Bang).Where(m => m.DS_Bang.Ten == table).ToList();
            var level = _context.Set<DS_Trinh>().Find(idTrinh);
            // Total point deposit of the Table
            var B_DT = _context.Set<DS_Diem>().Where(m => m.ID_Vong == 9 && pairs.Select(m => m.Id).Contains(m.ID_Cap)).Sum(m => m.Diem);
            // Max award points for this Table
            var B_max = B_DT * level.TL_Bang;
            // Max winning ratio for this Table
            var C_HSMax = matches.Count * 9;
            // Return list
            var pointList = new List<DS_Diem>();
            var pRatioList = new List<DS_Diem>();
            foreach (var pair in pairs)
            {
                var C_HS = pair.Tran_Thang * 3 - (matches.Count - pair.Tran_Thang) * 3 + pair.Hieu_so;

                // Calculation for positive ratio point distribution
                if (C_HS > 0) pRatioList.Add(new DS_Diem {
                    ID_Cap = pair.Id,
                    Diem = C_HS,
                    ID_Vong = 9 // Hệ số dương
                });
                // Add score for Table point distribution
                pointList.Add(new DS_Diem { 
                    ID_Cap = pair.Id, 
                    Diem = C_HS * B_max / C_HSMax,
                    ID_Vong = 8 // Bảng
                });
            }
            var C_HSdSum = pRatioList.Sum(m => m.Diem);
            pRatioList.ForEach(m => m.Diem = m.Diem * B_DT / C_HSdSum);
            // Add pRatioList to point list and return
            pointList.AddRange(pRatioList);
            return pointList;
        }
        private decimal Head2Head_Winning(DS_Tran match)
        {
            var p1 = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).FirstOrDefault(m => m.Id == match.ID_Cap1);
            var p2 = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).FirstOrDefault(m => m.Id == match.ID_Cap2);
            var level = _context.Set<DS_Trinh>().Find(match.ID_Trinh);
            var C_D1_Thua = match.Kq_1 > match.Kq_2 ? p1.VDV1.Diem + p1.VDV2.Diem : p2.VDV1.Diem + p2.VDV2.Diem;
            var C_D1_Thang = match.Kq_1 < match.Kq_2 ? p2.VDV1.Diem + p2.VDV2.Diem : p1.VDV1.Diem + p1.VDV2.Diem;
            var ratio = match.Kq_1 > match.Kq_2 ? match.Kq_1 - match.Kq_2 : match.Kq_2 - match.Kq_1;
            return (C_D1_Thua - level.Diem_PB * 2) * (level.Diem_Tru / 6) * (ratio / 6) / (C_D1_Thang - level.Diem_PB * 2);
        }
        /// <summary>
        /// Calculate points for pairs at Playoff or V1 to V3
        /// </summary>
        /// <param name="match">Current match</param>
        /// <returns>List of pair Id and corresponding point</returns>
        public List<DS_Diem> Head2Head_Point(DS_Tran match)
        {
            var winPoint = Head2Head_Winning(match);
            // Return list
            return new List<DS_Diem>
            {
                new DS_Diem
                {
                    ID_Cap = (int)match.ID_Cap1,
                    Diem = match.Kq_1 > match.Kq_2 ? winPoint : -winPoint,
                    ID_Vong = match.Ma_Vong
                },
                new DS_Diem
                {
                    ID_Cap = (int)match.ID_Cap2,
                    Diem = match.Kq_1 < match.Kq_2 ? winPoint : -winPoint,
                    ID_Vong = match.Ma_Vong
                }
            };
        }
        /// <summary>
        /// Calculate points for pairs at special rounds
        /// </summary>
        /// <param name="match">Current match</param>
        /// <returns>List of pair Id and corresponding point</returns>
        public List<DS_Diem> Special_Point(DS_Tran match)
        {
            var level = _context.Set<DS_Trinh>().Find(match.ID_Trinh);
            decimal parameter = 0;
            switch (match.Ma_Vong)
            {
                case 3: // Quarterfinals
                    parameter = level.TL_TuKet;
                    break;
                case 2: // Semifinals
                    parameter = level.TL_BanKet;
                    break;
                case 1: // Final
                    parameter = level.TL_ChungKet;
                    break;
            }
            var winPoint = Head2Head_Winning(match);
            // Return list
            var pointList = new List<DS_Diem>
            {
                new DS_Diem
                {
                    ID_Cap = (int)match.ID_Cap1,
                    Diem = level.Tong_Diem * parameter + match.Kq_1 > match.Kq_2 ? winPoint : -winPoint,
                    ID_Vong = match.Ma_Vong
                },
                new DS_Diem
                {
                    ID_Cap = (int)match.ID_Cap2,
                    Diem = level.Tong_Diem * parameter + match.Kq_1 < match.Kq_2 ? winPoint : -winPoint,
                    ID_Vong = match.Ma_Vong
                }
            };
            // If it's final, add score for winning pair as well
            if (match.Ma_Vong == 1)
            {
                pointList.Add(new DS_Diem
                {
                    ID_Cap = (int)(match.Kq_1 > match.Kq_2 ? match.ID_Cap1 : match.ID_Cap2),
                    Diem = level.Tong_Diem * level.TL_VoDich,
                    ID_Vong = 0
                });
            }
            return pointList;
        }
        /// <summary>
        /// Distribute points to player of each pair
        /// </summary>
        /// <param name="idTrinh">Level id</param>
        /// <returns>List of updated score for players</returns>
        public List<DS_VDV> Player_PointDistribution(int idTrinh)
        {
            var level = _context.Set<DS_Trinh>().Find(idTrinh);
            var pairs = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.ID_Trinh == idTrinh);
            var updatedScore = new List<DS_VDV>();
            foreach (var pair in pairs)
            {
                // Sum of all points achieve from new tournament
                var C_PS = _context.Set<DS_Diem>().Where(m => m.ID_Cap == pair.Id).Sum(m => m.Diem);

                var total = pair.VDV1.Diem + pair.VDV2.Diem - level.Diem_PB * 2;
                updatedScore.Add(new DS_VDV
                {
                    Id = pair.VDV1.Id,
                    Diem_Cu = pair.VDV1.Diem,
                    Diem = (int)Math.Ceiling(pair.VDV1.Diem + (pair.VDV1.Diem - level.Diem_PB) * C_PS / total)
                });
                updatedScore.Add(new DS_VDV
                {
                    Id = pair.VDV2.Id,
                    Diem_Cu = pair.VDV2.Diem,
                    Diem = (int)Math.Ceiling(pair.VDV2.Diem + (pair.VDV2.Diem - level.Diem_PB) * C_PS / total)
                });
            }
            return updatedScore;
        }
    }
}

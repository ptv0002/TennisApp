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
        /// - Trích điểm các cặp VDV --> Cập nhật điểm cho cặp
        /// - Cập nhật tổng điểm vào tham số trình
        /// - Cập nhật tổng điểm theo bảng
        /// </summary>
        /// <param name="idTrinh">Level id</param>
        /// <returns>List of score deposit before starting Table round</returns>
        public void Point_Deposit(int idTrinh)
        {
            var pointList = new List<DS_Diem>();
            var level = _context.Set<DS_Trinh>().Find(idTrinh);
            var pairs = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.ID_Trinh == idTrinh).OrderBy(m => m.ID_Bang).ToList() ;
            var tables = _context.Set<DS_Bang>().Where(m => m.ID_Trinh == idTrinh).ToList();
            // Total deposit points from all pairs in level: B_DT
            int T_DT = 0;   // Tổng điểm trích toàn Trình
            int CD_DT = 0;  // Tổng điểm trích cặp VĐV
            int B_DT = 0;   // Tổng điểm trích từng bảng
            int i = 0;
            int? mBang = 0;
            int C_D1 = 0;   // Điểm của các cặp VĐV

            var mTrinh = _context.Set<DS_Trinh>().First(m => m.Id == idTrinh);
            var mGiai = _context.Set<DS_Giai>().First(m => m.Id == mTrinh.ID_Giai);
            var mDate = new DateTime(2022, 03, 30);
            int compare = DateTime.Compare(mGiai.Ngay, mDate);

            while (i<pairs.Count)
            {
                mBang = pairs[i].ID_Bang;
                B_DT = 0;
                while (i < pairs.Count && pairs[i].ID_Bang == mBang)
                {
                    // Total point of pair before starting : C_D1
                    C_D1 = pairs[i].VDV1.Diem + pairs[i].VDV2.Diem;
                    pairs[i].Diem = C_D1; // Cập nhật điểm tổng theo các cặp
                    // Deposite point taken off from each pair : CD_DT
                    CD_DT = level.Diem_Tru * 2;
                    if (compare < 0)
                    { // Công thức cũ
                        if (C_D1 < level.Trinh - level.Diem_Tru * 2) CD_DT -= Math.Min(level.Diem_Tru * 2, level.Trinh - C_D1 - 10);
                        if (C_D1 > level.Trinh + level.Diem_Tru * 2) CD_DT += Math.Max(0, C_D1 - level.Trinh - 10);
                    }
                    else // Công thức mới
                    {
                        if (C_D1 < level.Trinh - level.Diem_Tru * 2) CD_DT -= Math.Min(level.Diem_Tru * 2, level.Trinh - level.Diem_Tru * 2 - C_D1);
                        if (C_D1 > level.Trinh + level.Diem_Tru * 2) CD_DT += Math.Max(0, C_D1 - level.Trinh - level.Diem_Tru * 5);
                    }

                    // Find if pair id is in the DB for this "round"
                    // If yes, update
                    if (_context.Set<DS_Diem>().Any(m => m.ID_Cap == pairs[i].Id && m.ID_Vong == 10))
                    {
                        var temp = _context.Set<DS_Diem>().FirstOrDefault(m => m.ID_Cap == pairs[i].Id && m.ID_Vong == 10);
                        temp.Diem = -CD_DT;
                        _context.Update(temp);
                    }
                    // If not, add new instance
                    else
                    {
                        _context.Add(new DS_Diem
                        {
                            Diem = -CD_DT,
                            ID_Cap = pairs[i].Id,
                            ID_Vong = 10
                        });
                    }
                    i++;
                    // Update điểm phân bổ for level
                    T_DT += CD_DT;
                    B_DT += CD_DT;
                }
                // Kết thúc Bảng --> Cập nhật bảng
                tables.FirstOrDefault(m => m.Id == mBang).Diem = B_DT;
            }
            // Kết thúc trình
            level.Tong_Diem = T_DT;
            _context.Update(level);        // Cập nhật điểm tổng theo trình
            _context.UpdateRange(tables);  // Cập nhật điểm tổng theo bảng
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
            var matches = _context.Set<DS_Tran>().ToList().Where(m => m.ID_Trinh == idTrinh && m.Ma_Tran[7] == table).ToList();
            var pairs = _context.Set<DS_Cap>().Include(m => m.DS_Bang).Where(m => m.DS_Bang.Ten == table && m.ID_Trinh==idTrinh).ToList();
            var level = _context.Set<DS_Trinh>().Find(idTrinh);
            var tables = _context.Set<DS_Bang>().Where(m => m.ID_Trinh == idTrinh).ToList();
            // Total point deposit of the Table
            var mGiai = _context.Set<DS_Giai>().First(m => m.Id == level.ID_Giai);
            var mDate = new DateTime(2022, 03, 30);
            int compare = DateTime.Compare(mGiai.Ngay, mDate);

            decimal B_DT = 0;
            if (compare<0) 
            {
                B_DT = level.Tong_Diem/tables.Count;
            }
            else
            {
                B_DT = _context.Set<DS_Diem>().Where(m => m.ID_Vong == 10 && pairs.Select(m => m.Id).Contains(m.ID_Cap)).Sum(m => m.Diem); // Lấy điểm trích theo bảng
            }

            // Max award points for this Table
            //var B_max = B_DT * level.TL_Bang;
            var B_max = level.Tong_Diem * level.TL_Bang / tables.Count/100;
            // Max winning ratio for this Table
            var C_HSMax = (pairs.Count-1) * 9;
            // Return list
            var pointList = new List<DS_Diem>();
            var pRatioList = new List<DS_Diem>();
            decimal mTileDuong = (100 - level.TL_VoDich - level.TL_ChungKet*2 - level.TL_BanKet*4 - level.TL_TuKet*8) / 100;

            foreach (var pair in pairs)
            {
                var C_HS = (pair.Tran_Thang*2 - pairs.Count + 1) * 3 + pair.Hieu_so;
                // Calculation for positive ratio point distribution
                if (mTileDuong>0 && C_HS>0) pRatioList.Add(new DS_Diem {
                    ID_Cap = pair.Id,
                    Diem = C_HS *mTileDuong,
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
            // Những trận đấu cũ không tính đối đầu trước ngày 30/03/2022.
            var mTrinh = _context.Set<DS_Trinh>().First(m => m.Id == match.ID_Trinh);
            var mGiai = _context.Set<DS_Giai>().First(m => m.Id == mTrinh.ID_Giai);
            var mDate = new DateTime(2022, 03, 30);
            int compare = DateTime.Compare(mGiai.Ngay, mDate);
            if (compare < 0) {return 0;}
            var p1 = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).FirstOrDefault(m => m.Id == match.ID_Cap1);
            var p2 = _context.Set<DS_Cap>().Include(m => m.VDV1).Include(m => m.VDV2).FirstOrDefault(m => m.Id == match.ID_Cap2);
            var level = _context.Set<DS_Trinh>().Find(match.ID_Trinh);
            int C_D1_Thua = 0;
            int C_D1_Thang = 0;
            var ratio = match.Kq_1 - match.Kq_2;
            if (ratio>0) 
            {
                C_D1_Thang  = p1.Diem;
                C_D1_Thua   = p2.Diem;
            }
            else 
            {
                C_D1_Thang  = p2.Diem;
                C_D1_Thua   = p1.Diem;
            }
            decimal diem = (C_D1_Thua - level.Diem_PB * 2) * ((decimal)level.Diem_Tru / 6) * ((decimal)ratio / 6) / (C_D1_Thang - level.Diem_PB * 2);
            return Math.Abs(diem);
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
                    Diem = (decimal) (level.Tong_Diem*parameter)/100 + (match.Kq_1 > match.Kq_2 ? winPoint : -winPoint),
                    ID_Vong = match.Ma_Vong
                },
                new DS_Diem
                {
                    ID_Cap = (int)match.ID_Cap2,
                    Diem = (decimal) (level.Tong_Diem * parameter)/100 + (match.Kq_1 < match.Kq_2 ? winPoint : -winPoint),
                    ID_Vong = match.Ma_Vong
                }
            };
            // If it's final, add score for winning pair as well
            if (match.Ma_Vong == 1)
            {
                pointList.Add(new DS_Diem
                {
                    ID_Cap = (int)(match.Kq_1 > match.Kq_2 ? match.ID_Cap1 : match.ID_Cap2),
                    Diem = (decimal)(level.Tong_Diem * level.TL_VoDich)/100,
                    ID_Vong = 0
                });
            }
            return pointList;
        }
        /// <summary>
        /// Update/Phục hồi lại toàn bộ điểm của VĐV từ DSVDVDiem --> DS_VDV
        /// </summary>
        public void Player_Update()
        {
            var DSDiem= _context.Set<DS_VDVDiem>().OrderBy(m=>m.ID_Vdv).OrderBy(m => m.Ngay).ToList();
            int i = 0;
            int mIDVDV = 0;
            int mDiem = 0;
            int mDiemCu = 0;
            while (i < DSDiem.Count) 
            {
                mIDVDV = DSDiem[i].ID_Vdv;
                mDiem = 0;
                mDiemCu = 0;
                while ((i < DSDiem.Count) && (mIDVDV == DSDiem[i].ID_Vdv))
                {
                    mDiemCu = mDiem;
                    mDiem += DSDiem[i].Diem;
                    i++;
                }
                var mVDV = _context.Set<DS_VDV>().Find(mIDVDV);
                mVDV.Diem = mDiem;
                mVDV.Diem_Cu= mDiem;
                _context.Set<DS_VDV>().Update(mVDV);
            }
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

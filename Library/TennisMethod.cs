using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class TennisMethod
    {
        private readonly DbContext _context;
        public TennisMethod(DbContext context)
        {
            _context = context;
        }
        public Dictionary<DS_Cap, int?> TableRanking(List<DS_Cap> list)
        {
            // Get pair Ids
            var idCap = list.Select(m => m.Id).ToList();
            var returnList = new Dictionary<DS_Cap, int?>(); // T1: Pair, T2: Rank

            bool first = true;
            while (true)
            {
                var pointsList = new Dictionary<DS_Cap, int>(); // T1: Pair, T2: Score (win rounds)
                foreach (var pair in list)
                {
                    // Get matches that the pair participate
                    var matchesPair1 = _context.Set<DS_Tran>().Where(m => m.ID_Cap1 == pair.Id).GroupBy(m => m.Kq_1 > m.Kq_2);
                    var matchesPair2 = _context.Set<DS_Tran>().Where(m => m.ID_Cap2 == pair.Id).GroupBy(m => m.Kq_1 < m.Kq_2);
                    // Get matches won
                    int count = matchesPair1.Count(m => m.Key) + matchesPair2.Count(m => m.Key);
                    pointsList.Add(pair, count);
                }
                // Rank point descending
                pointsList = pointsList.OrderByDescending(m => m.Value).ToDictionary(x => x.Key, y => y.Value);

                if (pointsList.All(m => m.Value == pointsList.ElementAt(0).Value))
                {
                    // If return list if null, transfer all obj from pointsList to return list
                    if (returnList == null)
                    {
                        for (int i = 0; i < pointsList.Count; i++)
                        {
                            returnList.Add(pointsList.ElementAt(i).Key, null);
                        }
                        return returnList;
                    }
                    goto exit;
                }
                for (int i = 0; i < pointsList.Count; i++)
                {
                    // If first loop, add all obj to return list
                    if (first)
                    {
                        int? rank = null;
                        // If points achieved isn't repeated, add rank to return list and remove obj from list of Pairs to consider
                        if (pointsList.Where(m => m.Value == pointsList.ElementAt(i).Value).Count() == 1)
                        {
                            rank = i + 1; // Rank start from 1
                            list.Remove(pointsList.ElementAt(i).Key);
                        }
                        returnList.Add(pointsList.ElementAt(i).Key, rank);
                        first = false;
                    }
                    // From the second loop, update existing obj
                    else
                    {
                        if (pointsList.Where(m => m.Value == pointsList.ElementAt(i).Value).Count() == 1)
                        {
                            var obj = returnList.FirstOrDefault(m => m.Key.Id == pointsList.ElementAt(i).Key.Id);
                            returnList[obj.Key] = NewId(returnList);
                            list.Remove(pointsList.ElementAt(i).Key);
                        }
                    }
                }
                // If only one pair remain in the list, add the ranking.
                if (list.Count == 1)
                {
                    var obj = returnList.FirstOrDefault(m => m.Key.Id == list[0].Id);
                    returnList[obj.Key] = NewId(returnList);
                    break;
                }
            }
            exit:
            return returnList.OrderBy(m => m.Value).ToDictionary(x => x.Key, y => y.Value);
        }
        /// <summary>
        /// Xếp hạng theo điểm, đầu vào danh sách cặp, ds trận đấu để tính điểm
        /// Khi nào thi đấu xong mới gọi hàm xếp hạng này.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<DS_Cap> Rank_Full(int idTrinh, string bang)
        {
            // Lấy danh sách cặp đấu và ds trận đấu của bảng
            var returnList  = _context.Set<DS_Cap>().Where(m => m.ID_Trinh == idTrinh).Where(m => m.Ma_Cap.Substring(0,1)==bang).ToList();
            var ds_tran     = _context.Set<DS_Tran>().Where(m => m.ID_Trinh==idTrinh)
                                .Where(m => m.Ma_Tran.Substring(5,1)=="8")
                                .Where(m => m.Ma_Tran.Substring(7,1)==bang)
                                .ToList();

            // ============= Xếp hạng theo điểm
            returnList = Rank_Point(returnList, ds_tran); // 
            //=============== Kiểm tra nếu xếp hạng bằng nhau --> Xếp hạng theo điểm nội bộ
            List<Rank_Data> data = Rank_Select(returnList, ds_tran);
            for (int i=0; i< data.Count; i++)
            {
                var mini_dscap = Rank_Point(data[i].DS_Cap , data[i].DS_Tran);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số nội bộ
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                var mini_dscap = Rank_Ratio(data[i].DS_Cap, data[i].DS_Tran);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số toàn cục
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                var mini_dscap = Rank_Ratio(data[i].DS_Cap, ds_tran);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
            }
            //=============== Xếp hạng theo bốc thăm (ở ngoài --> dùng nút xếp hạng sau cùng)
            return (List<DS_Cap>)returnList.OrderBy(m => m.Xep_Hang);
        }

        public static List<DS_Cap> Rank_Point(List<DS_Cap> lcap, List<DS_Tran> ltran)
        {
            // Tính điểm các cặp dựa trên danh sách trận
            int id_Cap = 0;
            for (int i=0; i< ltran.Count; i++ ) 
            { 
                if ((ltran[i].Kq_1- ltran[i].Kq_2)>0 ) 
                { id_Cap = lcap.First(m => m.Id == ltran[i].ID_Cap1).Id; } 
                else 
                { id_Cap = lcap.First(m => m.Id == ltran[i].ID_Cap2).Id; }
                lcap[lcap.FindIndex(m => m.Id == id_Cap)].Tran_Thang++;
            }
            lcap = (List<DS_Cap>) lcap.OrderByDescending(m => m.Tran_Thang); // Xếp thứ tự sau khi tính điểm
            // Bắt đầu Xếp hạng sau khi tính điểm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lcap[0].Xep_Hang>0) { cur = lcap[0].Xep_Hang; }  // Khi xếp hạng theo điểm nhóm con
            int next = 0;
            int diem = lcap[0].Tran_Thang;
            lcap[0].Xep_Hang = cur;
            for (int i = 1; i < lcap.Count; i++)
            {
                if (lcap[i].Tran_Thang == diem) 
                    {
                        next++;
                    }
                else
                    {
                    cur += next ;
                    next = 0;
                    diem = lcap[i].Tran_Thang;
                    }
                lcap[0].Xep_Hang = cur;
            }

            return (List<DS_Cap>) lcap.OrderBy(m => m.Xep_Hang);
        }
        public static List<DS_Cap> Rank_Ratio(List<DS_Cap> lCap, List<DS_Tran> lTran)
        {
            return lCap;
        }
        public static List<DS_Cap> Rank_Replace(List<DS_Cap> lFull, List<DS_Cap> lMini)
        {
            for (int i=0; i< lMini.Count; i++) 
            {
                lFull[lFull.FindIndex(m => m.Id == lMini[i].Id)].Xep_Hang  = lMini[i].Xep_Hang;
            }
            return lFull;
        }
        public static List<Rank_Data> Rank_Select(List<DS_Cap> lCap, List<DS_Tran> lTran)
        {
            List<Rank_Data> lData = new();
            int xephang = 0;
            int i = 0; bool first = true;
            while (i < lCap.Count)
            {
                if (first)
                {
                    xephang = lCap[i].Xep_Hang;
                    i++;
                    first = false;
                }
                else
                {
                    if (xephang == lCap[i].Xep_Hang)  // Lấy danh sách các cặp đồng điểm
                    {
                        List<DS_Tran> mini_dstran = new();
                        List<DS_Cap> mini_dscap = new();
                        mini_dscap.Add(lCap[i - 1]);
                        while (xephang == lCap[i].Xep_Hang)
                        {
                            mini_dscap.Add(lCap[i]);
                            i++;
                        }
                        first = true;
                        // Lấy danh sách các trận có các cặp đồng điểm (Loại bỏ các trận không có ID cặp trong danh sách)
                        for (int j = 0; j < lTran.Count; j++)
                        {
                            if (mini_dscap.Any(m => m.Id == lTran[j].ID_Cap1) && mini_dscap.Any(m => m.Id == lTran[j].ID_Cap2))
                            {
                                mini_dstran.Add(lTran[j]);
                            }
                        }
                        lData.Add(new Rank_Data {DS_Tran = mini_dstran, DS_Cap = mini_dscap});
                    }
                    else
                    {
                        xephang = lCap[i].Xep_Hang;
                        i++;
                        first = true;
                    }
                }
            }
            return lData;
        }
        public class Rank_Data
        {
            public List<DS_Tran> DS_Tran { get; set; }
            public List<DS_Cap> DS_Cap { get; set; }
        }
        private static int NewId(Dictionary<DS_Cap, int?> list)
        {
            // Get not null Value obj and order ascending
            list = list.Where(m => m.Value != null).OrderBy(m => m.Value).ToDictionary(x => x.Key, y => y.Value);
            for (int i = 1; i < list.Count; i++)
            {
                // If list[i] != list[i-1] + 1, return list[i-1] + 1 since list[i] is not consecutive
                if (list.ElementAt(i).Value != (list.ElementAt(i-1).Value + 1))
                {
                    return (int)(list.ElementAt(i - 1).Value + 1);
                }
            }
            // If the list contains all consecutive ranking, return new id next to last
            return (int)(list.Last().Value + 1);
        }
    }
}

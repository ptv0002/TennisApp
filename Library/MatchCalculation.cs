﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class MatchCalculation
    {
        private readonly DbContext _context;
        public MatchCalculation(DbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Rank by winning match, each match's score difference and draws if needed 
        /// </summary>
        /// <param name="idTrinh">Level's id</param>
        /// <param name="bang">Table's name</param>
        /// <returns>List of ranked pairs, repeated order if all the test pass and need result from draws</returns>
        public List<DS_Cap> Rank_Full(int idTrinh, char bang)
        {
            // Lấy danh sách cặp đấu và ds trận đấu của bảng
            var returnList = _context.Set<DS_Cap>().ToList().Where(m => m.ID_Trinh == idTrinh && m.Ma_Cap[0] == bang).ToList();
            var ds_tran = _context.Set<DS_Tran>().ToList().Where(m => m.ID_Trinh == idTrinh && m.Ma_Tran[5] == '8'&& m.Ma_Tran[7] == bang).ToList();
            for (int i=0; i< returnList.Count;i++ ) { returnList[i].Xep_Hang = 0; returnList[i].Tran_Thang = 0; returnList[i].Hieu_so = 0; }
            // ============= Xếp hạng theo điểm
            returnList = Rank_Point(returnList, ds_tran, true); 
            //=============== Kiểm tra nếu xếp hạng bằng nhau --> Xếp hạng theo điểm nội bộ
            List<Rank_Data> data = Rank_Select(returnList, ds_tran); // Các nhóm cặp đồng hạng
            List<DS_Cap> mini_dscap = new();
            for (int i=0; i< data.Count; i++)
            {
                mini_dscap = Rank_Point(data[i].DS_Cap, data[i].DS_Tran, false);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                returnList = returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số nội bộ
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Ratio(data[i].DS_Cap, data[i].DS_Tran);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                returnList = returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số toàn cục
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Ratio(data[i].DS_Cap, ds_tran);   //Xếp hạng các cặp đồng điểm bằng điểm và hiệu số nội bộ 
                returnList= returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Xếp hạng theo bốc thăm (ở ngoài --> dùng nút xếp hạng sau cùng)
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Lottery(data[i].DS_Cap);   //Xếp hạng các cặp đồng điểm bằng bốc thăm
            }
            return returnList.OrderBy(m => m.Xep_Hang).ToList();
        }
        // Rank by winning match
        public static List<DS_Cap> Rank_Point(List<DS_Cap> lCap, List<DS_Tran> lTran, bool PointCal)
        {
            // Tính điểm các cặp dựa trên danh sách trận
            for (int i = 0; i < lCap.Count; i++) lCap[i].Tinh_toan = 0;
            int cap_1 = 0; int cap_2 = 0; int cap_0 = 0;
            for (int i=0; i< lTran.Count; i++ ) 
            {
                cap_1 = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap1);
                cap_2 = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap2);
                if (lTran[i].Kq_1 + lTran[i].Kq_2 > 0)
                {
                    if ((lTran[i].Kq_1 - lTran[i].Kq_2) > 0) cap_0 = cap_1;
                    else cap_0 = cap_2;
                    if (PointCal)
                    {
                        lCap[cap_1].Hieu_so += lTran[i].Kq_1 - lTran[i].Kq_2;
                        lCap[cap_2].Hieu_so += lTran[i].Kq_2 - lTran[i].Kq_1;
                        lCap[cap_0].Tran_Thang++;
                    }
                    lCap[cap_0].Tinh_toan++;
                }
            }
            lCap = lCap.OrderByDescending(m => m.Tinh_toan).ToList(); // Xếp thứ tự sau khi tính điểm
            // Bắt đầu Xếp hạng sau khi tính điểm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang>0) cur = lCap[0].Xep_Hang;  // Khi xếp hạng theo điểm nhóm con
            int next = 1;
            int diem = lCap[0].Tinh_toan;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Tinh_toan == diem) next++; 
                else
                {
                    cur += next;
                    next = 1;
                    diem = lCap[i].Tinh_toan;
                }
                lCap[i].Xep_Hang = cur;
            }
            return lCap.OrderBy(m => m.Xep_Hang).ToList();
        }
        // Rank by match's score difference
        public static List<DS_Cap> Rank_Ratio(List<DS_Cap> lCap, List<DS_Tran> lTran)
        {
            // Tính hiệu số thắng thua các cặp dựa trên danh sách trận
            for (int i = 0; i < lCap.Count; i++) lCap[i].Tinh_toan = 0;
            int j = 0;
            for (int i = 0; i < lTran.Count; i++)
            {
                j = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap1);
                if (j>=0) lCap[j].Tinh_toan += lTran[i].Kq_1 - lTran[i].Kq_2;

                j = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap2);
                if (j >= 0) lCap[j].Tinh_toan += lTran[i].Kq_2 - lTran[i].Kq_1;
            }
            lCap = lCap.OrderByDescending(m => m.Tinh_toan).ToList(); // Xếp thứ tự sau khi tính hiệu số thắng thua
            // Bắt đầu Xếp hạng sau khi tính điểm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang > 0) cur = lCap[0].Xep_Hang;  // Khi xếp hạng theo điểm nhóm con
            int next = 1;
            int diem = lCap[0].Tinh_toan;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Tinh_toan == diem) next++;
                else
                {
                    cur += next;
                    next = 1;
                    diem = lCap[i].Tinh_toan;
                }
                lCap[i].Xep_Hang = cur;
            }
            return lCap.OrderBy(m => m.Xep_Hang).ToList();
        }
        // Rank by draws
        public static List<DS_Cap> Rank_Lottery(List<DS_Cap> lCap)
        {
            lCap = lCap.OrderBy(m => m.Xep_Hang*10+ m.Boc_Tham).ToList(); // Xếp thứ tự sau khi tính hiệu số thắng thua
            // Bắt đầu Xếp hạng sau khi bốc thăm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang > 0) cur = lCap[0].Xep_Hang;  // Khi xếp hạng theo điểm nhóm con
            int next = 1;
            int diem = lCap[0].Boc_Tham;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Boc_Tham == diem) next++;
                else
                {
                    cur += next;
                    next = 1;
                    diem = lCap[i].Boc_Tham;
                }
                lCap[i].Xep_Hang = cur;
            }
            return lCap.OrderBy(m => m.Xep_Hang).ToList();
        }
        /// <summary>
        /// Tách ra các list : Mỗi list gồm các cặp xếp hạng bằng nhau, và danh sách các trận có liên quan
        /// </summary>
        /// <param name="lCap"></param>
        /// <param name="lTran"></param>
        /// <returns></returns>
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
                        while (i < lCap.Count && xephang == lCap[i].Xep_Hang)
                        {
                            mini_dscap.Add(lCap[i]);
                            i++;
                        }
                        first = true;
                        // Lấy danh sách các trận có các cặp đồng điểm (Loại bỏ các trận không có ID cặp trong danh sách)
                        for (int j = 0; j < lTran.Count; j++)
                        {
                            if (mini_dscap.Any(m => m.Id == lTran[j].ID_Cap1) && mini_dscap.Any(m => m.Id == lTran[j].ID_Cap2))
                            mini_dstran.Add(lTran[j]);
                        }
                        lData.Add(new Rank_Data {DS_Tran = mini_dstran, DS_Cap = mini_dscap});
                    }
                    else
                    first = true;
                }
            }
            return lData;
        }
        public class Rank_Data
        {
            public List<DS_Tran> DS_Tran { get; set; }
            public List<DS_Cap> DS_Cap { get; set; }
        }

        /// <summary>
        /// Update next round based on previous matches results
        /// </summary>
        /// <param name="lTran"></param> List of all matches
        /// <param name="vong"></param> Id of current match
        public List<DS_Tran> Special_Update(List<DS_Tran> lTran, int vong) 
        { 
            if (vong <2 || vong>6) { return lTran; }  // Không nằm trong vòng đặc biệt
            var kq_vong = lTran.Where(m => m.Ma_Vong == vong).ToList();
            foreach (var kq in kq_vong) 
            {
                lTran = Special_Select(lTran, kq);
            }
            return lTran;
        }

        /// <summary>
        /// Pick the pairs for the next round, based on previous results
        /// </summary>
        /// <param name="lTran"></param> : List of all matches
        /// <param name="tran"></param>  : Current match 
        public List<DS_Tran> Special_Select(List<DS_Tran> lTran, DS_Tran tran)
        {
            //Trình(4)*Vòng(1)*Bang(1)*TT Vòng (2)* TT Trình (3)  - 0-3*5*7*9-10*12-14
            if (tran.Ma_Vong > 6 || tran.Ma_Vong<2 || (tran.Kq_1 + tran.Kq_2) == 0)
            {
                return lTran; // Trận chung kết --> Không chọn đội vào vòng trong
            }

            // Tìm trận tiếp theo
            int stt =   Convert.ToInt32(tran.Ma_Tran[9..11]);           // Số thứ tự trận trong vòng hiện thời
            int n2 = (stt-1)/2 ;                                        // Hiện là trận thứ mấy trong nhóm 2 trận
            n2 = stt - n2 * 2;

            string nxt = "00"+((stt-1)/ 2+1);                            // số tt trận tiếp theo của vòng trong
            nxt = nxt[^2..];                                              
            bool cap = (n2 - 1) % 2 == 0;                               // vào vị trí cặp 1 hay cặp 2 của trận tiếp theo

            string ma_tran = tran.Ma_Tran[0..5] + (tran.Ma_Vong-1) + "*0*" + nxt + "*"; // Không cần lấy số thứ tự trận trong trình này
            var trantiep = lTran.First(m => m.Ma_Tran[0..12] == ma_tran);  /*.Substring(0, 12)*/
            if (tran.Kq_1 > tran.Kq_2) 
            { 
                if (cap) trantiep.ID_Cap1 = tran.ID_Cap1; 
                else trantiep.ID_Cap2 = tran.ID_Cap1;
            } 
            else
            {
                if (cap) trantiep.ID_Cap1 = tran.ID_Cap2;
                else trantiep.ID_Cap2 = tran.ID_Cap2;
            }
            return lTran;
        }
        public static int? Champion_Select(DS_Tran tran)
        {
            if (tran.Ma_Vong > 1 || (tran.Kq_1 + tran.Kq_2) == 0) { return null;}
            if (tran.Kq_1 > tran.Kq_2) return tran.ID_Cap1;  
            else return tran.ID_Cap2; 
        }
    }
}

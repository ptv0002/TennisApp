﻿using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Xếp hạng theo điểm, đầu vào danh sách cặp, ds trận đấu để tính điểm
        /// Khi nào thi đấu xong mới gọi hàm xếp hạng này.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<DS_Cap> Rank_Full(int idTrinh, char bang)
        {
            // Lấy danh sách cặp đấu và ds trận đấu của bảng
            var returnList = _context.Set<DS_Cap>().ToList().Where(m => m.ID_Trinh == idTrinh && m.Ma_Cap[0] == bang).ToList();
            var ds_tran = _context.Set<DS_Tran>().ToList().Where(m => m.ID_Trinh == idTrinh && m.Ma_Tran[5] == '8'&& m.Ma_Tran[7] == bang).ToList();
            for (int i=0; i< returnList.Count;i++ ) { returnList[i].Xep_Hang = 0; returnList[i].Tran_Thang = 0; returnList[i].Hieu_so = 0; }
            // ============= Xếp hạng theo điểm
            returnList = Rank_Point(returnList, ds_tran, true); 
            //=============== Kiểm tra nếu xếp hạng bằng nhau --> Xếp hạng theo điểm nội bộ
            List<Rank_Data> data = Rank_Select(returnList, ds_tran);
            List<DS_Cap> mini_dscap = new();
            for (int i=0; i< data.Count; i++)
            {
                mini_dscap = Rank_Point(data[i].DS_Cap, data[i].DS_Tran, false);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                //returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
                returnList = returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số nội bộ
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Ratio(data[i].DS_Cap, data[i].DS_Tran);   //Xếp hạng các cặp đồng điểm bằng điểm nội bộ
                //returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
                returnList = returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Nếu vẫn còn xếp hạng theo hiệu số toàn cục
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Ratio(data[i].DS_Cap, ds_tran);   //Xếp hạng các cặp đồng điểm bằng điểm và hiệu số nội bộ 
                //returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
                returnList= returnList.OrderBy(m => m.Xep_Hang).ToList();
            }
            //=============== Xếp hạng theo bốc thăm (ở ngoài --> dùng nút xếp hạng sau cùng)
            data = Rank_Select(returnList, ds_tran);
            for (int i = 0; i < data.Count; i++)
            {
                mini_dscap = Rank_Lottery(data[i].DS_Cap);   //Xếp hạng các cặp đồng điểm bằng bốc thăm
                //returnList = Rank_Replace(returnList, mini_dscap);  // Gán lại vào returnList
            }
            return returnList.OrderBy(m => m.Xep_Hang).ToList();
        }

        public static List<DS_Cap> Rank_Point(List<DS_Cap> lCap, List<DS_Tran> lTran, bool PointCal)
        {
            // Tính điểm các cặp dựa trên danh sách trận
            for (int i = 0; i < lCap.Count; i++) { lCap[i].Tinh_toan = 0; }
            int cap_1 = 0; int cap_2 = 0; int cap_0 = 0;
            for (int i=0; i< lTran.Count; i++ ) 
            {
                //if ((lTran[i].Kq_1- lTran[i].Kq_2)>0 ) 
                //{ id_Cap = lCap.First(m => m.Id == lTran[i].ID_Cap1).Id; } 
                //else 
                //{ id_Cap = lCap.First(m => m.Id == lTran[i].ID_Cap2).Id; }
                cap_1 = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap1);
                cap_2 = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap2);
                if ((lTran[i].Kq_1 - lTran[i].Kq_2) > 0) 
                {
                    cap_0 = cap_1;
                }
                else
                {
                    cap_0 = cap_2;
                }
                if (PointCal) 
                {
                    lCap[cap_1].Hieu_so += (lTran[i].Kq_1 - lTran[i].Kq_2);
                    lCap[cap_2].Hieu_so += (lTran[i].Kq_2 - lTran[i].Kq_1);
                    lCap[cap_0].Tran_Thang++;
                }
                lCap[cap_0].Tinh_toan++;
            }
            lCap = lCap.OrderByDescending(m => m.Tinh_toan).ToList(); // Xếp thứ tự sau khi tính điểm
            // Bắt đầu Xếp hạng sau khi tính điểm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang>0) { cur = lCap[0].Xep_Hang; }  // Khi xếp hạng theo điểm nhóm con
            int next = 1;
            int diem = lCap[0].Tinh_toan;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Tinh_toan == diem) 
                    {
                        next++;
                    }
                else
                    {
                    cur += next ;
                    next = 1;
                    diem = lCap[i].Tinh_toan;
                    }
                lCap[i].Xep_Hang = cur;
            }
            return lCap.OrderBy(m => m.Xep_Hang).ToList();
        }
        public static List<DS_Cap> Rank_Ratio(List<DS_Cap> lCap, List<DS_Tran> lTran)
        {
            // Tính hiệu số thắng thua các cặp dựa trên danh sách trận
            for (int i = 0; i < lCap.Count; i++) { lCap[i].Tinh_toan=0; }
            int j = 0;
            for (int i = 0; i < lTran.Count; i++)
            {
                j = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap1);
                if (j>=0) { lCap[j].Tinh_toan += lTran[i].Kq_1 - lTran[i].Kq_2; }

                j = lCap.FindIndex(m => m.Id == lTran[i].ID_Cap2);
                if (j >= 0) { lCap[j].Tinh_toan += lTran[i].Kq_2 - lTran[i].Kq_1; }
            }
            lCap = lCap.OrderByDescending(m => m.Tinh_toan).ToList(); // Xếp thứ tự sau khi tính hiệu số thắng thua
            // Bắt đầu Xếp hạng sau khi tính điểm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang > 0) { cur = lCap[0].Xep_Hang; }  // Khi xếp hạng theo điểm nhóm con
            int next = 1;
            int diem = lCap[0].Tinh_toan;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Tinh_toan == diem)
                {
                    next++;
                }
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
        public static List<DS_Cap> Rank_Lottery(List<DS_Cap> lCap)
        {
            lCap = lCap.OrderBy(m => m.Xep_Hang*10+ m.Boc_Tham).ToList(); // Xếp thứ tự sau khi tính hiệu số thắng thua
            // Bắt đầu Xếp hạng sau khi bốc thăm
            int cur = 1;                                         // Lần đầu xếp hạng   
            if (lCap[0].Xep_Hang > 0) { cur = lCap[0].Xep_Hang; }  // Khi xếp hạng theo điểm nhóm con
            int next = 0;
            int diem = lCap[0].Boc_Tham;
            lCap[0].Xep_Hang = cur;
            for (int i = 1; i < lCap.Count; i++)
            {
                if (lCap[i].Boc_Tham == diem)
                {
                    next++;
                }
                else
                {
                    cur += next;
                    next = 0;
                    diem = lCap[i].Boc_Tham;
                }
                lCap[i].Xep_Hang = cur;
            }
            return lCap.OrderBy(m => m.Xep_Hang).ToList();
        }
        public static List<DS_Cap> Rank_Replace(List<DS_Cap> lFull, List<DS_Cap> lMini)
        {
            for (int i=0; i< lMini.Count; i++) 
            {
                lFull[lFull.FindIndex(m => m.Id == lMini[i].Id)].Xep_Hang  = lMini[i].Xep_Hang;
            }
            return lFull.OrderBy(m => m.Xep_Hang).ToList();
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
                            {
                                mini_dstran.Add(lTran[j]);
                            }
                        }
                        lData.Add(new Rank_Data {DS_Tran = mini_dstran, DS_Cap = mini_dscap});
                    }
                    else
                    {
                        //xephang = lCap[i].Xep_Hang;
                        //i++;
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
    }
}

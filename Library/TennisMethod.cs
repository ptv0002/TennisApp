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
        //public List<Tuple<DS_Cap, int, bool>> TableRanking(List<DS_Cap> list)
        //{
        //    // Get pair Ids
        //    var idCap = list.Select(m => m.Id).ToList();
        //    var returnList = new List<Tuple<DS_Cap, int, bool>>(); // T1: Pair, T2: Rank, T3: Draw (Bốc thăm)
        //    var rankedList = new List<Tuple<DS_Cap, int, int>>(); // T1: Pair, T2: Score, T3: Difference
            
        //    bool cont = true;
        //    while (cont)
        //    {
        //        foreach (var pair in list)
        //        { 
        //            int count = 0, indif = 0, dif = 0;

        //            // Get matches that the pair participate
        //            var matches = _context.Set<DS_Tran>().Where(m => m.ID_Cap1 == pair.Id || m.ID_Cap2 == pair.Id);
        //            foreach (var match in matches)
        //            {
        //                if (match.ID_Cap1 == pair.Id) indif = match.Kq_1 - match.Kq_2;
        //                else indif = match.Kq_2 - match.Kq_1;
        //                if (indif > 0)
        //                {
        //                    // If the pair wins, increment win count 
        //                    count++;
        //                }
        //                // Calculate the point difference
        //                dif += indif;
        //            }
        //            var tuple = new Tuple<DS_Cap, int, int>(pair, count, dif);
        //            rankedList.Add(tuple);
        //        }
        //    }
        //    foreach (var pair in list)
        //    {
                
        //    }
        //    // Order by the win score, then the point difference
        //    return rankedList.OrderByDescending(m => m.Item2).ThenByDescending(m => m.Item3).ToList();
        //}
    }
}

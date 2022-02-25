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

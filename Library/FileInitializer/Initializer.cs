using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.FileInitializer
{
    public class Initializer
    {
        private readonly IWebHostEnvironment _webHost;
        public Initializer(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public static Dictionary<int, string> RoundList()
        {
            return new Dictionary<int, string>
                        {
                            { 0, "Vô Địch" },
                            { 1, "Chung Kết"},
                            { 2, "Bán Kết" },
                            { 3, "Tứ Kết"},
                            { 4, "Vòng 1/16"},
                            { 5, "Vòng 1/32" },
                            { 6, "Vòng 1/64" },
                            { 7, "Playoff"},
                            { 8, "Vòng Bảng" },
                            { 9, "Hệ Số Dương" },
                            { 10,"Trích Điểm" }
                        };
        }
        public static Dictionary<int, string> ActivityMenu()
        {
            // Display in descending order
            return new Dictionary<int, string>
                        {
                            { 0, "Khác" },
                            { 1, "Tennis"},
                            { 2, "Từ thiện" }
                        };
        }
        public async void Special1stRoundGenerator()
        {
            // Get path for the Json file
            string path = _webHost.WebRootPath + "/uploads/Json/Special1stRound.json";
            var param = new List<Special1stRound>
            {
                new Special1stRound
                {
                    TableNum = 1,
                    PairNum = 2, // Chung Kết, 2 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "2A"}
                    }
                },
                new Special1stRound
                {
                    TableNum = 2,
                    PairNum = 4, // Bán Kết, 4 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "2B"},
                        { "1B", "2A" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 3,
                    PairNum = 8, // Tứ Kết, 8 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "3CB"},
                        { "1B", "2C" },
                        { "1C", "3AB"},
                        { "2A", "2B" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 4,
                    PairNum = 8, // Tứ Kết, 8 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "2D"},
                        { "1B", "2C" },
                        { "1C", "2B"},
                        { "1D", "2A" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 5,
                    PairNum = 8, // Tứ Kết, 8 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "1E"},
                        { "1B", "2DCEA" },
                        { "1C", "2AEBD"},
                        { "1D", "2EABC" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 6,
                    PairNum = 16, // Vòng 3, 16 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "3FEDBC"},
                        { "1B", "3FEDBC" },
                        { "1C", "2D"},
                        { "2F", "2B" },
                        { "2A", "2E"},
                        { "1D", "2C" },
                        { "1E", "3BACFD"},
                        { "1F", "3ACBED" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 7,
                    PairNum = 16, // Vòng 3, 16 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "3GFEDCB"},
                        { "1B", "2G" },
                        { "1C", "2F"},
                        { "1D", "2E" },
                        { "2A", "2B"},
                        { "1E", "2D" },
                        { "1F", "2C"},
                        { "1G", "3BCDEFG" }
                    }
                },
                new Special1stRound
                {
                    TableNum = 8,
                    PairNum = 16, // Vòng 3, 16 pairs
                    P1_P2_Pair = new Dictionary<string, string>
                    {
                        { "1A", "2H"},
                        { "1B", "2G" },
                        { "1C", "2F"},
                        { "1D", "2E" },
                        { "1E", "2D"},
                        { "1F", "2C" },
                        { "1G", "2B"},
                        { "1H", "2A" }
                    }
                }
            };
            FileStream inStream;
            // Delete file if Json file is already exist
            if (File.Exists(path)) File.Delete(path);

            inStream = File.Create(path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(inStream, param, options);
            inStream.Dispose();
        }
    }
}

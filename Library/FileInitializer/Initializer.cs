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
        public async void RoundGeneratorAsync()
        {
            // Get path for the Json file
            string path = _webHost.WebRootPath + "/Files/Json/RoundInfo.json";
            var listRound = new List<Round>
                        {
                            new Round { Ten = "Vô Địch", Ma_Vong = 0 },
                            new Round { Ten = "Chung Kết", Ma_Vong = 1 },
                            new Round { Ten = "Bán Kết", Ma_Vong = 2 },
                            new Round { Ten = "Tứ Kết", Ma_Vong = 3 },
                            new Round { Ten = "Vòng 3", Ma_Vong = 4 },
                            new Round { Ten = "Vòng 2", Ma_Vong = 5 },
                            new Round { Ten = "Vòng 1", Ma_Vong = 6 },
                            new Round { Ten = "Playoff", Ma_Vong = 7 },
                            new Round { Ten = "Vòng Bảng", Ma_Vong = 8 },
                            new Round { Ten = "Trích Điểm", Ma_Vong = 9 }
                        };
            FileStream fileStream;
            // Delete file if Json file is already exist
            if (File.Exists(path)) File.Delete(path);

            fileStream = File.Create(path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(fileStream, listRound, options);
            fileStream.Dispose();
        }
        public async void Special1stRoundGenerator()
        {
            // Get path for the Json file
            string path = _webHost.WebRootPath + "/Files/Json/Special1stRound.json";
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
                        { "1A", "3BC"},
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
                        { "1A", "3FEDCB"},
                        { "1B", "3EFDCA" },
                        { "1C", "2D"},
                        { "2F", "2B" },
                        { "2A", "2E"},
                        { "1D", "2C" },
                        { "1E", "3BACDF"},
                        { "1F", "3ABCDE" }
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

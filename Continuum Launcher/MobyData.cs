using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum_Launcher
{
    /// <summary>
    /// Class for handling information from the MobyGames database
    /// </summary>
    public class GameInfo
    {
        public class VariantHolder
        {
            public GameVariant[]? Data { get; set; }
        }
        public class GameVariant
        {
            public string? GameName { get; set; }
            public string? TitleID { get; set; }
            public string? Serial { get; set; }
            public string? Region { get; set; }
            public string? XEXCRC { get; set; }
            public string? MediaID { get; set; }
            public string? Wave { get; set; }
        }
        // --- ORIGINAL DATA ---
        public string[]? Developers { get; set; }
        public string[]? Genres { get; set; }
        public int? ID { get; set; }
        public double? Moby_Score { get; set; }
        public string? Moby_URL { get; set; }
        public string[]? Platforms { get; set; }
        public string[]? Publishers { get; set; }
        public string? Release_Date { get; set; }
        public string? Title { get; set; }
        // --- NEW DATA ---
        public string? Alternate_Name { get; set; }
        public GameVariant[]? Variants { get; set; }
        // --- CONTINUUM DATA ---
        public bool? Incorrect_Date { get; set; }
    }
    public class MobyData
    {
        public GameInfo[]? Data { get; set; }
    }
}

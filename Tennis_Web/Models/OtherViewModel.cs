using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class MatchGeneratorErrorViewModel
    {
        public IEnumerable<DS_Cap> NoCodePairs { get; set; }
        public IEnumerable<DS_VDV> NoPairPlayers { get; set; }
    }
    public class EntityListViewModel
    {
        public bool IsSelected { get; set; }
        public string EntityName { get; set; }
    }
    public class GroupStatusViewModel<T>
    {
        public StatusViewModel StatusViewModel { get; set; }
        public IEnumerable<T> ClassModel { get; set; }
    }
    public class StatusViewModel
    {
        public string SelectedValue { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
    }
}

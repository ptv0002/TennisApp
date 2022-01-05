using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class FileViewModel
    {
    }
    public class EntityListViewModel
    {
        public bool IsSelected { get; set; }
        public string EntityName { get; set; }
    }
}

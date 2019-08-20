using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class ChartList
    {
        public int ChartID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
        public int? ListOrder { get; set; }
    }
}

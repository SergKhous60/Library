using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class PerformanceDetailsData
    {
        public Performance Performance { get; set; }
        public int TotalSecs { get; set; }
        public ICollection<ChartViewModel> Charts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    public class Cart
    {
        private List<Chart> chartCollection = new List<Chart>();

        public virtual void AddItem(Chart chart)
        {
            var check = chartCollection.FirstOrDefault(c => c.ID == chart.ID);
            if (check == null)
            {
                chartCollection.Add(chart);
            }
        }
        public virtual void RemoveChart(int id)
        {
            Chart chart = chartCollection.FirstOrDefault(c => c.ID == id);
            chartCollection.Remove(chart);
        }

        public virtual void Clear() => chartCollection.Clear();

        public virtual IEnumerable<Chart> ChartsPool => chartCollection;
        public virtual int TotalChartsSecs => ChartsPool.Sum(i => i.Minutes * 60 + i.Seconds);
    }
}

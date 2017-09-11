using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    class ChartData
    {
        public string AxName { get; set; }
        public int ColumValue { get; set; }
        public ChartData(string name, int value)
        {
            this.AxName = name;
            this.ColumValue = value;
        }
        public ChartData() { }
        public List<ChartData> ordenar(List<ChartData> lista)
        {
            IEnumerable<ChartData> order = lista.OrderBy(x => x.ColumValue).Reverse();
            List<ChartData> finalList = new List<ChartData>();
            foreach (var item in order)
            {
                finalList.Add(item);
            }
            return finalList;
        }
    }
}

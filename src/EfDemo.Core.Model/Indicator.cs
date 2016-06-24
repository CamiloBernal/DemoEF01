using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class Indicator
    {
        public Category IndicatorCategory { get; set; }
        public int IndicatorId { get; set; }
        public int IndicatorMaxValue { get; set; }
        public int IndicatorMinValue { get; set; }
        public string IndicatorName { get; set; }
        public ICollection<string> IndicatorTags { get; set; } = new List<string>();
    }
}
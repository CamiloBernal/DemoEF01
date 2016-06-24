using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class Goal
    {
        public int GoalId { get; set; }
        public string GoalDescription { get; set; }
        public string GoalNotes { get; set; }
        public string GoalColor { get; set; }
        public Category GoalCategory { get; set; }
        public EntityStatus GoalStatus { get; set; }
        public ICollection<string> GoalTags { get; set; } = new List<string>();
        public ICollection<Indicator> Indicators { get; set; } = new List<Indicator>();
    }
}
using System;
using System.Collections.Generic;

#nullable disable

namespace Fit_Center_Model
{
    public partial class Class
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; } = null!;

        public string? ClassDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Location { get; set; } = null!;

        public virtual ICollection<Assignment> Assignments { get; } = new List<Assignment>();
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace Fit_Center_Model
{
    public partial class Assignment
    {
        public int AssignmentId { get; set; }

        public int UserId { get; set; }

        public int ClassId { get; set; }

        public DateTime AssignmentDate { get; set; }

        public string Status { get; set; } = null!;

        public int? AssignmentGrade { get; set; }

        public virtual Class Class { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}

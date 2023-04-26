using System;
using System.Collections.Generic;

namespace API.Models;

public partial class AssignmentPost
{
    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public DateTime AssignmentDate { get; set; }

    public string Status { get; set; } = null!;

    public int? AssignmentGrade { get; set; }

}

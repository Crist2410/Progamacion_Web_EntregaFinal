using System;
using System.Collections.Generic;

namespace API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Assignment> Assignments { get; } = new List<Assignment>();
}

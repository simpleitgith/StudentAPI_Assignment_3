using System;
using System.Collections.Generic;

namespace TallyAssignment3.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Class { get; set; }
}

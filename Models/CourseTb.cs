using System;
using System.Collections.Generic;

namespace StudentApi.Models;

public partial class CourseTb
{
    public int CoursePkid { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseName { get; set; }

    public string? Description { get; set; }

    public int? Credits { get; set; }
}

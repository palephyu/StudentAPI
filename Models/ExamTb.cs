using System;
using System.Collections.Generic;

namespace StudentApi.Models;

public partial class ExamTb
{
    public int ExamPkid { get; set; }

    public int? CoursePkid { get; set; }

    public string? ExamTitle { get; set; }

    public DateOnly? ExamDate { get; set; }

    public int? MaxMarks { get; set; }
}

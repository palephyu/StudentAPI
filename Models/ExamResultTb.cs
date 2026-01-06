using System;
using System.Collections.Generic;

namespace StudentApi.Models;

public partial class ExamResultTb
{
    public int ResultPkid { get; set; }

    public int? ExamPkid { get; set; }

    public int? StudentPkid { get; set; }

    public int? MarksObtained { get; set; }

    public string? Grade { get; set; }
}

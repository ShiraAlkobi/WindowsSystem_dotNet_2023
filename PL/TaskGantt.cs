using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;
/// <summary>
/// task for the gantt chart
/// </summary>
public class TaskGantt
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public int Duration { get; set; }
    /// <summary>
    /// time from the start date of the project the the scheduled date
    /// </summary>
    public int TimeFromStart { get; set; }
    /// <summary>
    /// time from the forecast date of the task the the end date of the project
    /// </summary>
    public int TimeToEnd { get; set; }
    public BO.Status Status { get; set; }

    public List<BO.TaskInEngineer>? Dependencies { get; set; }

}

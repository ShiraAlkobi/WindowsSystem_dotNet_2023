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
    ///the task's id
    public int Id { get; init; }

    ///the task's name
    public string? Name { get; set; }

    ///the task's required effort time
    public int Duration { get; set; }

    /// time from the start date of the project the the scheduled date
    public int TimeFromStart { get; set; }

    /// time from the forecast date of the task the the end date of the project
    public int TimeToEnd { get; set; }

    ///the task's status
    public BO.Status Status { get; set; }

    ///the task's list of dependencies
    public List<BO.TaskInEngineer>? Dependencies { get; set; }

}

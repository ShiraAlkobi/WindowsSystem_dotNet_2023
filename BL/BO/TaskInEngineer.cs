using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// in this file we define the class TaskInEngineer - an object to represent the task assigned to engineer
/// </summary>
public class TaskInEngineer
{
    public int Id {  get; init; }
    public string? Alias { get; set; }
    public override string ToString() => Tools.ToStringProperty(this);
}

using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

public class TaskGantt
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public int Duration { get; set; }
    public int TimeFromStart { get; set; }
    public int TimeToEnd { get; set; }
    public BO.Status Status { get; set; }
    
}

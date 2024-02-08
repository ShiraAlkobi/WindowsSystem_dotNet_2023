using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask Task =>  new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void setStatus()
    {
        _dal.setStatus();

    }
    public void changeStatus()
    {
        _dal.changeStatus();

    }

    public void setStartAndEndDates(DateTime start)
    {
        DateTime? end=start;
        DateTime? help=null;
        foreach (var item in _dal.Task.ReadAll())
        {
            if (item.ScheduledDate is null)
            {
               Task.UpdateScedualedDate(item.Id, start);
            }
            
        }
        foreach (var item in _dal.Task.ReadAll())
        {
            help = item.ScheduledDate + item.RequiredEffortTime;
            end = end < help ? help : end;
        }
        _dal.setStartAndEndDates(start, end);
        changeStatus();

    }

    public ProjectStatus getProjectStatus()
    {
       return (BO.ProjectStatus)_dal.getProjectStatus();
    }

    public DateTime? getStartDate()
    {
        return _dal.getStartDate();
    }

    public DateTime? getEndDate()
    {
        return _dal.getEndDate();
    }
}

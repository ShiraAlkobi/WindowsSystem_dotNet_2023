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

    public void setStartAndEndDates(DateTime start, DateTime end)
    {
        _dal.setStartAndEndDates(start, end);
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

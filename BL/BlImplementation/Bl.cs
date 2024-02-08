using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

/// <summary>
/// in this file we create the class Bl which descends from the IBL interface 
/// this class creates fields of implementions objects of all of the BL main entities 
/// also, we define the methods that manage the project's date and status, gotten in the BL's program
/// </summary>

internal class Bl : IBl
{
    public ITask Task =>  new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// an instance of the IDAL interface in order to get data from the DAL data source
    /// </summary>
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// sets the plan stage status in the data source
    /// </summary>
    public void setStatus()
    {
        _dal.setStatus();
    }

    /// <summary>
    /// change the project's status to execution stage in the data source
    /// </summary>
    public void changeStatus()
    {
        _dal.changeStatus();
    }

    /// <summary>
    /// sets the given date as the project's start date in the data source 
    /// also, initialize the scheduled dates for each task 
    /// finally, calculates the end date and sets it in the data source
    /// </summary>
    /// <param name="start"></param>
    public void setStartAndEndDates(DateTime start)
    {
        DateTime? end = start;
        DateTime? help = null;

        ///for each task, update its scheduled date according to the function in the task implementation
        foreach (var item in _dal.Task.ReadAll())
        {
            ///if the date wasn't initialized, then set it
            if (item.ScheduledDate is null)
            {
               Task.UpdateScedualedDate(item.Id, start);
            }            
        }

        ///in order to calculate the end date, find the maximum out of all the tasks' complete dates
        foreach (var item in _dal.Task.ReadAll())
        {
            ///the task's complete date
            help = item.ScheduledDate + item.RequiredEffortTime;
            ///change for the max value if needed
            end = end < help ? help : end;
        }
        ///set the dates in the data source
        _dal.setStartAndEndDates(start, end);

        ///change the project's status from plan to execution stage
        changeStatus();
    }

    /// <summary>
    /// returns the project's status from the data source
    /// </summary>
    /// <returns></returns>
    public ProjectStatus getProjectStatus()
    {
       return (BO.ProjectStatus)_dal.getProjectStatus();
    }

    /// <summary>
    /// returns the project's start date from the data source
    /// </summary>
    /// <returns></returns>
    public DateTime? getStartDate()
    {
        return _dal.getStartDate();
    }

    /// <summary>
    /// returns the project's end date from the data source
    /// </summary>
    /// <returns></returns>
    public DateTime? getEndDate()
    {
        return _dal.getEndDate();
    }
}

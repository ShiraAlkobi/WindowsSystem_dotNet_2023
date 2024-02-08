using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
/// <summary>
/// this class implements the Ibl interface
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// getting an object of task implementation
    /// </summary>
    public ITask Task =>  new TaskImplementation();
    /// <summary>
    /// getting an object of enginner implementation
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();
    /// <summary>
    /// creating an appearance that connects the bl layer to dal layer
    /// </summary>
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// calling set status from dl level
    /// </summary>
    public void setStatus()
    {
        _dal.setStatus();

    }
    /// <summary>
    /// calling change status from dl level
    /// </summary>
    public void changeStatus()
    {
        _dal.changeStatus();

    }
    /// <summary>
    /// this function sets schedual dates to all tasks, and saving the start and end(after calculation) 
    /// dates in the data base and changing the status of the project to executing stage 
    /// ///this is a wrap function for recursive UpdateSchedualedDate in task implementation
    /// </summary>
    /// <param name="start">the start date of the project- if a task does not depend on any task- 
    /// it`s scheduale date will be the strat+3-see recursive function</param>
    public void setStartAndEndDates(DateTime start)
    {
        DateTime? end=start;
        DateTime? help=null;
        foreach (var item in _dal.Task.ReadAll())//going through list of tasks
        {
            if (item.ScheduledDate is null)//if there isnt already a schedual date
            {
               Task.UpdateScedualedDate(item.Id, start);//call the function from task implementation to set date
            }
            
        }
        foreach (var item in _dal.Task.ReadAll())//going through task list after setting all dates to find the end date for the project
        {
            help = item.ScheduledDate + item.RequiredEffortTime;//getting the forecast date-when the task is supposed to be completed
            end = end < help ? help : end;//finding the max of all tasks
        }
        _dal.setStartAndEndDates(start, end);//set the dates for the project!(not task!) in database
        changeStatus();//changing status to from plan to execution

    }
    /// <summary>
    /// calling the function from dal level
    /// </summary>
    /// <returns></returns>
    public ProjectStatus getProjectStatus()
    {
       return (BO.ProjectStatus)_dal.getProjectStatus();
    }
    /// <summary>
    /// calling the function from dal level
    /// </summary>
    /// <returns></returns>
    public DateTime? getStartDate()
    {
        return _dal.getStartDate();
    }
    /// <summary>
    /// calling the function from dal level
    /// </summary>
    /// <returns></returns>
    public DateTime? getEndDate()
    {
        return _dal.getEndDate();
    }
}

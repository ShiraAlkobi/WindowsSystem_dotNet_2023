
namespace Dal;
using DalApi;
using DO;
using System;

/// <summary>
/// this class gather the implementations of the entities by inheriting 
/// from IDal interface. for each entity(property in IDal) we call the implementations function
/// and initialize interface type accordingly.
/// also, there're properties for the project's start and end dates and status
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    /// <summary>
    /// properties for the project's start and end dates and status
    /// </summary>
    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public ProjectStatus ProjectStatus { get; set; }

    /// <summary>
    /// sets the given dates in the list DataSource config file
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void setStartAndEndDates(DateTime start, DateTime? end)
    {
        DataSource.Config.projectStartDate = start;
        DataSource.Config.projectEndDate = end;
    }

    /// <summary>
    /// set the project's status in the DataSource to plan stage
    /// </summary>
    public void setStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.PlanStage;
    }

    /// <summary>
    /// change the project's status in the datasource from plan to execution
    /// </summary>
    public void changeStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.ExecutionStage;
    }

    /// <summary>
    /// return the project's start date
    /// </summary>
    /// <returns></returns>
    public DateTime? getStartDate()
    {
        return DataSource.Config.projectStartDate;
    }

    /// <summary>
    /// return the project's end date
    /// </summary>
    /// <returns></returns>
    public DateTime? getEndDate()
    {
        return DataSource.Config.projectEndDate;
    }

    /// <summary>
    /// return the project's status
    /// </summary>
    /// <returns></returns>
    public ProjectStatus getProjectStatus()
    {
        return DataSource.Config.projectStatus;
    }
}

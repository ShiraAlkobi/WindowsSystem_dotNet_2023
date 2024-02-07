
namespace Dal;
using DalApi;
using DO;
using System;

/// <summary>
/// this class gather the implementations of the entities by inheriting 
/// from IDal interface. for each entity(property in IDal) we call the implementations function
/// and initialize interface type accordingly.
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();


    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public ProjectStatus ProjectStatus { get; set; }

    public void setStartAndEndDates(DateTime start, DateTime end)
    {
        DataSource.Config.projectStartDate=start;
        DataSource.Config.projectEndDate=end;
    }
    public void setStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.PlanStage;
    }
    public void changeStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.ExecutionStage;
    }

    public DateTime? getStartDate()
    {
        return DataSource.Config.projectStartDate;
    }

    public DateTime? getEndDate()
    {
        return DataSource.Config.projectEndDate;
    }

    public ProjectStatus getProjectStatus()
    {
        return DataSource.Config.projectStatus;
    }
}

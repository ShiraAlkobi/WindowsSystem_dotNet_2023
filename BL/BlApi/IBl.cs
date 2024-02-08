namespace BlApi;
/// <summary>
/// this interface wrapps all of the BL head entities` interfaces
/// using this one we can access the entities' interfaces 
/// </summary>
public interface IBl
{
    public ITask Task { get;} 
    public IEngineer Engineer { get; }

    /// <summary>
    /// sets the plan stage status in the data source
    /// </summary>
    public void setStatus();

    /// <summary>
    /// change the project's status to execution stage in the data source
    /// </summary>
    public void changeStatus();

    /// <summary>
    ///gets the project's start date of the project' sets it  in the data source
    ///then, sets all of the tasks' scheduled dates
    ///also, calculates the project's end date and sets it in the data source 
    /// </summary>
    /// <param name="start"></param>
    public void setStartAndEndDates(DateTime start);

    /// <summary>
    /// returns the project's start date from the data source
    /// </summary>
    /// <returns></returns>
    public DateTime? getStartDate();

    /// <summary>
    /// returns the project's end date from the data source
    /// </summary>
    /// <returns></returns>
    public DateTime? getEndDate();

    /// <summary>
    /// returns the project's status from the data source
    /// </summary>
    /// <returns></returns>
    public BO.ProjectStatus getProjectStatus();
}

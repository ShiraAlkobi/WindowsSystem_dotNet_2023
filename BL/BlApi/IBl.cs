namespace BlApi;
/// <summary>
/// this interface wrapps all of the BL head entities` interfaces
/// using this one we can access the entities' interfaces 
/// </summary>
public interface IBl
{
    public ITask Task { get;} 
    public IEngineer Engineer { get; }
    public IUser User { get; }

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
    /// <summary>
    /// initialize the database
    /// </summary>
    public void InitializeDB() => DalTest.Initialization.Do();
    /// <summary>
    /// reset database
    /// </summary>
    public void ResetDB() => DalTest.Initialization.emptyData();

    public void setManager() => DalTest.Initialization.setManager();

    #region Clock Defining
    /// <summary>
    /// the property represent the whole program clock
    /// </summary>
    public DateTime getClock();
    /// <summary>
    /// adds one year to the clock
    /// </summary>
    public void AddYear();
    /// <summary>
    /// adds one month to the clock
    /// </summary>
    public void AddMonth();
    /// <summary>
    /// adds one day to the clock
    /// </summary>
    public void AddDay();
    /// <summary>
    /// adds one hour to the clock
    /// </summary>
    public void AddHour();

    /// <summary>
    /// resets the clock to the current time
    /// </summary>
    public DateTime ResetClock();


    #endregion
}

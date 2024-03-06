namespace DalApi;
/// <summary>
/// this interface wrapps all of the entities` interfaces
/// to represent the dal data level
/// </summary>
public interface IDal
{
    ITask Task { get; }///property for the sub interface ITask
    IEngineer Engineer { get; }///property for the sub interface IEngineer
    IDependency Dependency { get; }///property for the sub interface IDependency
    IUser User { get; }///property for the sun interface IUser

    #region Project Dates and Status
    DateTime Clock { get; set; }
    DateTime? ProjectStartDate { get; set; }///property for the project's start date
    DateTime? ProjectEndDate { get; set; }///property for the project's end date
    DO.ProjectStatus ProjectStatus { get; set; }///property for the project's status
    public void setClock(DateTime clock);
    public DateTime getClock();
    /// <summary>
    ///sets the given dates in the XML config file
    /// </summary>
    public void setStartAndEndDates(DateTime start, DateTime? end);

    /// <summary>
    /// change the project's status in the data base (list/XML) from plan to execution
    /// </summary>
    public void changeStatus();

    /// <summary>
    /// set the project's status in the data base (list/XML) to plan stage
    /// </summary>
    /// <returns></returns>
    public void setStatus();

    /// <summary>
    /// return the project's start date
    /// </summary>
    /// <returns></returns>
    public DateTime? getStartDate();

    /// <summary>
    /// return the project's end date
    /// </summary>
    /// <returns></returns>
    public DateTime? getEndDate();

    /// <summary>
    /// return the project's status
    /// </summary>
    /// <returns></returns>
    public DO.ProjectStatus getProjectStatus();
    /// <summary>
    /// function to call resetId of chosen implementation of the factory
    /// </summary>
    public void ResetId();
    #endregion
}

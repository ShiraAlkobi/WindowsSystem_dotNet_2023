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
    DateTime? ProjectStartDate { get; set; }
    DateTime? ProjectEndDate { get; set; }
    DO.ProjectStatus ProjectStatus { get; set; }

    public void setStartAndEndDates(DateTime start, DateTime end);
    public void changeStatus();
    public void setStatus();
    public DateTime? getStartDate();
    public DateTime? getEndDate();
    public DO.ProjectStatus getProjectStatus();
}

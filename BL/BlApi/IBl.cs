namespace BlApi;
/// <summary>
/// this interface wrapps all of the BL head entities` interfaces
/// using this one we can access the entities' interfaces 
/// </summary>
public interface IBl
{
    public ITask Task { get;} 
    public IEngineer Engineer { get; }
    

    public void setStatus();

    public void changeStatus();

    public void setStartAndEndDates(DateTime start, DateTime end);
    public DateTime? getStartDate();
    public DateTime? getEndDate();
    public BO.ProjectStatus getProjectStatus();
}

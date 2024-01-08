namespace DO;
/// <summary>
/// This class represents the dependency between tasks
/// </summary>
/// <param name="Id">The identifing number of the task</param>
/// <param name="DependentTask">Can't be executed without task a completed</param>
/// <param name="DependsOnTask">Task A</param>
public record Dependency
(
     int Id,
     int DependentTask,
     int DependsOnTask

)
{
    public Dependency() : this(0,0,0) { }///empty ctor
}

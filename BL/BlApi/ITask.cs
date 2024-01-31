namespace BlApi;
/// <summary>
///interface defines the methods of the bo level task entity
/// </summary>
public interface ITask
{
    /// <summary>
    /// add new task
    /// </summary>
    /// <param name="t">task to add</param>
    public int Create(BO.Task t);

    /// <summary>
    /// update task
    /// </summary>
    /// <param name="t">task to update</param>
    public void Update(Task t); 

    /// <summary>
    /// return collection of tasks- if filter is not null- return sub list according
    /// to filter. else- returns full list.
    /// </summary>
    /// <param name="filter">filter to apply</param>
    /// <returns></returns>
    public IEnumerable<Task> ReadAll(Func<Task,bool>? filter=null);

    /// <summary>
    /// return task with param id
    /// </summary>
    /// <param name="id">id of task to return</param>
    /// <returns></returns>
    public Task? Read(int id);
    /// <summary>
    /// delete task with param id
    /// </summary>
    /// <param name="id">id of task to delete</param>
    public void Delete(int id);  
    /// <summary>
    /// update start date of task
    /// </summary>
    /// <param name="id">is of task to update</param>
    /// <param name="date">new start date</param>
    public void UpdateScedualedDate(int id, DateTime date);

}

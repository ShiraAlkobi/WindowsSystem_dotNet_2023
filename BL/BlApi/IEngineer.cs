namespace BlApi;
/// <summary>
///interface defines the methods of the bo level engineer entity
/// </summary>
public interface IEngineer
{
    /// <summary>
    /// add new engineer
    /// </summary>
    /// <param name="t">engineer to add</param>
    public int Create(BO.Engineer t);

    /// <summary>
    /// update engineer
    /// </summary>
    /// <param name="t">engineer to update</param>
    public void Update(BO.Engineer t);

    /// <summary>
    /// return collection of engineers- if filter is not null- return sub list according
    /// to filter. else- returns full list.
    /// </summary>
    /// <param name="filter">filter to apply</param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// return collection of engineerInTask instances - if filter is not null- return sub list according
    /// to filter. else - returns full list.
    /// goes through all of the engineers, check which ones apply to the filter
    /// then converts the chosen ones to engineerInTask - a class with only the engineer's Id and name
    /// </summary>
    /// <param name="filter">filter to apply</param>
    /// <returns></returns>
    public IEnumerable<BO.EngineerInTask>? ReadAllToTask(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// return engineer with param id
    /// </summary>
    /// <param name="id">id of engineer to return</param>
    /// <returns></returns>
    public BO.Engineer Read(int id);

    /// <summary>
    /// delete engineer with param id
    /// </summary>
    /// <param name="id">id of engineer to delete</param>
    public void Delete(int id);

    /// <summary>
    /// sorts the engineers by Alphabetical order of their names
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> SortByName();

    /// <summary>
    /// returns whether a task can be assigned to engineer
    /// </summary>
    /// <param name="t_id"></param>
    /// <returns></returns>
    public bool checkAssignedTask(int t_id);
}

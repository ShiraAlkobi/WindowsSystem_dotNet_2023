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
    /// return engineer with param id
    /// </summary>
    /// <param name="id">id of engineer to return</param>
    /// <returns></returns>
    public BO.Engineer? Read(int id);

    /// <summary>
    /// delete engineer with param id
    /// </summary>
    /// <param name="id">id of engineer to delete</param>
    public void Delete(int id);
}

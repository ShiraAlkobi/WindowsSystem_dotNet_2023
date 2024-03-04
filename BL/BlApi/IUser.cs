namespace BlApi;
/// <summary>
///interface defines the methods of the bo level user entity
/// </summary>
public interface IUser
{
    /// <summary>
    /// add new user
    /// </summary>
    /// <param name="t">engineer to add</param>
    public int Create(BO.User t);

    /// <summary>
    /// update user
    /// </summary>
    /// <param name="t">engineer to update</param>
    public void Update(BO.User t);

    /// <summary>
    /// return collection of users- if filter is not null- return sub list according
    /// to filter. else- returns full list.
    /// </summary>
    /// <param name="filter">filter to apply</param>
    /// <returns></returns>
    public IEnumerable<BO.User>? ReadAll(Func<BO.User, bool>? filter = null);

    /// <summary>
    /// return user with param id
    /// </summary>
    /// <param name="id">id of user to return</param>
    /// <returns></returns>
    public BO.User Read(int id);

    /// <summary>
    /// delete user with param id
    /// </summary>
    /// <param name="id">id of user to delete</param>
    public void Delete(int id);
}

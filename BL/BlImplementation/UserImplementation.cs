namespace BlImplementation;

using System.Net.NetworkInformation;
using System;
using BlApi;
using BO;
using System.Diagnostics;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//creating an appearance to connect bl to dal

    /// <summary>
    /// function to create a new user
    /// </summary>
    /// <param name="t"> user to add</param>
    /// <returns>id of user</returns>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.User t)
    {
        _dal.User.Create(new DO.User(t.Id, t.UserName, t.Password, (DO.Position)t.Position));
        return t.Id;
    }

    /// <summary>
    /// deleting a user
    /// </summary>
    /// <param name="id">id of user to delete</param>
    /// <exception cref="BO.BlCanNotDelete"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        try
        {
            BO.User t_user = Read(id);///get the user to be deleted
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException)
        { throw new BO.BlDoesNotExistException($"User with ID={id} doesn`t exist"); }

    }


    /// <summary>
    /// returns the right user object from the data source accorrding to the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.User? Read(int id)
    {
        try
        {
            DO.User t_user = _dal.User.Read(id);
            return new BO.User()
            {
                Id = t_user.Id,
                UserName = t_user.UserName,
                Password = t_user.Password,
                Position = (BO.Position)t_user.Position
            };
        }
        catch (DO.DalDoesNotExistException)
        { throw new BO.BlDoesNotExistException($"User with ID={id} doesn`t exist"); }
    }
    

    /// <summary>
    /// returns a collection of users that answer to the filter sent
    /// if filter is null,return all of the users
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool> filter = null)
    {
        ///ReadAll function return an IEnumreable of DO objects
        ///so convertion is needed in order to use filter on the object
        return _dal.User.ReadAll()
            .Select(
            user => new BO.User()
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Position = (BO.Position)user.Position
            })
                .Where(user => filter is null ? true : filter(user))
                    .OrderBy(user => user.Id); // Add this line for ordering by Id
    }

    /// <summary>
    /// updates the user
    /// </summary>
    /// <param name="t">user to update</param>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.User t)
    {
        try
        {
            ///create new task of DO type  
            DO.User t_user = new DO.User(t.Id, t.UserName, t.Password, (DO.Position)t.Position);
            _dal.User.Update(t_user);//call dal function to save in database

        }
        catch (DO.DalDoesNotExistException)//if the user doesnt exist, update will throw an exception
        { throw new BO.BlDoesNotExistException($"User with ID={t.Id} does Not exist"); }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;

public class UserImplementation : IUser
{
    ///getting a user's object and adding it to the DataSounce
    public int Create(User item)
    {
        DataSource.Users.Add(item);
        return item.Id;
    }

    ///getting one user's ID and deleting its object from the DataSource
    public void Delete(int id)
    {
        //check if it exists in the DataSource
        if (!DataSource.Users.Exists(p => p.Id == id))
            throw new DalDoesNotExistException($"User with ID={id} does Not exist");
        else
        {
            User temp = DataSource.Users.Find(p => p.Id == id)!;//find the user according to the ID
            DataSource.Users.Remove(temp);
        }
    }

    ///getting one user's ID and returning its user (as object)
    public User? Read(int id)
    {
        ///check if it exists in the DataSource using the linq function and gotten id
        ///if found the right user will return it, else returns null
        return DataSource.Users.FirstOrDefault(p => p.Id == id);
    }

    ///Gets a parameter and returns the first object to meet filter (delegate)
    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter!);
    }

    ///returning a reduced list - only the items that are true in the filter function
    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null) //in this case, return the whole list/collection
            return DataSource.Users.Select(item => item);
        //otherwise, return the collection of the users which are true for the filter function
        else return DataSource.Users.Where(filter!);
    }

    ///Gets a user and updates it in the DataSource (finds it according to similar ID)
    public void Update(User item)
    {
        ///check if it exists in the DataSource
        if (!DataSource.Users.Exists(p => p!.Id == item.Id))
            throw new DalDoesNotExistException($"User with ID={item.Id} does Not exist");
        else
        {
            User temp = DataSource.Users.Find(p => p!.Id == item.Id)!;
            DataSource.Users.Remove(temp);///remove the old object from the DataSource
            DataSource.Users.Add(item);///add the new object - the updated one
        }
    }
}

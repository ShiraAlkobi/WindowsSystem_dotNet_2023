namespace Dal;

using System.Linq;
using System.Collections.Generic;
using DalApi;
using DO;
//CRUD functions for the engineer entity
internal class EngineerImplementation : IEngineer //derived from this interface
{
    ///getting an engineer's object and adding it to the DataSounce
    public int Create(Engineer item)
    {
        //check if it exists in the DataSource - if so, no need to add another one
        if (DataSource.Engineers.Exists(p=>p.Id==item.Id)) 
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    ///getting one engineer's ID and deleting its object from the DataSource 
    public void Delete(int id)
    {
        //check if it exists in the DataSource
        if (!DataSource.Engineers.Exists(p => p.Id == id))
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        else
        {
            Engineer temp = DataSource.Engineers.Find(p => p.Id == id);// find the engineer according to the ID
            DataSource.Engineers.Remove(temp);
        }
    }

    ///getting one engineer's ID and returning its engineer (as object)
    public Engineer? Read(int id)
    {
        //check if it exists in the DataSource using the linq function and the gotten id
        //if found the right engineer will return it, else rturns null
        return DataSource.Engineers.FirstOrDefault(p => p.Id == id);
    }

    ///returning a reduced list - only the items that are true in the filter function
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter)
    {
        if (filter == null) //in this case, return the whole list/collection
            return DataSource.Engineers.Select(item => item);
        //otherwise, return the collection of the engineers which are true for the filter function
        else return DataSource.Engineers.Where(filter!);
    }

    ///Gets an engineer and updates it in the DataSource (finds it according to similar ID)
    public void Update(Engineer item)
    {
        //check if it exists in the DataSource
        if (!DataSource.Engineers.Exists(p => p.Id == item.Id))
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        else
        {
            Engineer temp = DataSource.Engineers.Find(p => p.Id == item.Id);
            DataSource.Engineers.Remove(temp);//remove the old object from the DataSource
            DataSource.Engineers.Add(item);//add the new object - the updated
        }
    }

    ///Gets a parameter and returns the first object to meet filter (delegate)
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter!);
    }
}

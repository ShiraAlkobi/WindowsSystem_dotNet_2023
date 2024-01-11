namespace Dal;

using System.Collections;
using System.Collections.Generic;
using DalApi;
using DO;
//CRUD functions for the dependency entity
internal class DependencyImplementation : IDependency //derived from this interface
{
    ///getting a dependency's object and adding it to the DataSounce
    public int Create(Dependency item) 
    {
        int newID = DataSource.Config.NextDependencyId; //identifing
        Dependency copy = item with { Id = newID };
        DataSource.Dependencys.Add(copy);
        return newID;
    }

    ///getting one dependency's ID and deleting its object from the DataSource 
    public void Delete(int id)
    {
        //check if it exists in the DataSource
        if (!DataSource.Dependencys.Exists(p => p.Id == id))
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
        else
        {
            Dependency temp = DataSource.Dependencys.Find(p => p.Id == id); //find the dependency of this ID
            DataSource.Dependencys.Remove(temp);
        }
    }

    ///getting one dependency's ID and returning its dependency (as object)
    public Dependency? Read(int id)
    {
        //check if it exists in the DataSource
        if (DataSource.Dependencys.Exists(p => p.Id == id))  
            return DataSource.Dependencys.Find(p => p.Id == id);
        else return null; //if doesn't exist 
    }

    ///returning a new list which is a copy of the Dependencys DataSource
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    ///Gets a dependency object and updates the object in
    public void Update(Dependency item)
    {
        //check if it exists in the DataSource
        if (!DataSource.Dependencys.Exists(p => p.Id == item.Id))
            throw new DalDoesNotExistException($"Dependency with ID ={ item.Id } does Not exist");
        else
        {
            Dependency temp = DataSource.Dependencys.Find(p => p.Id == item.Id);
            DataSource.Dependencys.Remove(temp); //remove the old object from the DataSource
            DataSource.Dependencys.Add(item); //add the new object - the updated
        }
    }
}

namespace Dal;

using System.Collections.Generic;
using System.Security.Cryptography;
using DalApi;
using DO;
///CRUD functions for the engineer entity
internal class TaskImplementation : ITask ///derived from this interface
{
    ///getting an task's object and adding it to the DataSounce
    public int Create(Task item)
    { 
        int newID = DataSource.Config.NextTaskId; ///identifing
        Task copy = item with { Id = newID };
        DataSource.Tasks.Add(copy);
        return newID;
    }

    ///getting one task's ID and deleting its object from the DataSource
    public void Delete(int id)
    {
        //check if it exists in the DataSource
        if (!DataSource.Tasks.Exists(p => p.Id == id))
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p.Id == id)!;//find the task according to the ID
            DataSource.Tasks.Remove(temp);
        }
    }

    ///getting one task's ID and returning its task (as object)
    public Task? Read(int id)
    {
        //check if it exists in the DataSource using the linq function and gotten id
        //if found the right task will return it, else rturns null
        return DataSource.Tasks.FirstOrDefault(p => p.Id == id);
    }

    ///returning a reduced list - only the items that are true in the filter function
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter)
    {
        if (filter == null) //in this case, return the whole list/collection
            return DataSource.Tasks.Select(item => item);
        //otherwise, return the collection of the tasks which are true for the filter function
        else return DataSource.Tasks.Where(filter!); 
    }

    ///Gets a task and updates it in the DataSource (finds it according to similar ID)
    public void Update(Task item)
    {
        //check if it exists in the DataSource
        if (!DataSource.Tasks.Exists(p=>p!.Id==item.Id))
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p!.Id == item.Id)!;
            DataSource.Tasks.Remove(temp);//remove the old object from the DataSource
            DataSource.Tasks.Add(item);//add the new object - the updated
        }
    }

    ///Gets a parameter and returns the first object to meet filter (delegate)
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter!);
    }
}

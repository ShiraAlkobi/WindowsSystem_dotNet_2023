namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;
//CRUD functions for the engineer entity
internal class TaskImplementation : ITask //derived from this interface
{
    ///getting an task's object and adding it to the DataSounce
    public int Create(Task item)
    { 
        int newID = DataSource.Config.NextTaskId; //identifing
        Task copy = item with { Id = newID };
        DataSource.Tasks.Add(copy);
        return newID;
    }

    ///getting one task's ID and deleting its object from the DataSource
    public void Delete(int id)
    {
        //check if it exists in the DataSource
        if (!DataSource.Tasks.Exists(p => p.Id == id))
            throw new Exception($"Task with ID={id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p.Id == id)!;//find the task according to the ID
            DataSource.Tasks.Remove(temp);
        }
    }

    ///getting one task's ID and returning its task (as object)
    public Task? Read(int id)
    {
        //check if it exists in the DataSource
        if (DataSource.Tasks.Exists(p => p!.Id == id))
            return DataSource.Tasks.Find(p => p!.Id == id);
        else return null; //if doesn't exist
    }

    ///returning a new list which is a copy of the Tasks DataSource
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks!);
    }

    ///Gets a task and updates it in the DataSource (finds it according to similar ID)
    public void Update(Task item)
    {
        //check if it exists in the DataSource
        if (!DataSource.Tasks.Exists(p=>p!.Id==item.Id))
            throw new Exception($"Task with ID={item.Id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p!.Id == item.Id)!;
            DataSource.Tasks.Remove(temp);//remove the old object from the DataSource
            DataSource.Tasks.Add(item);//add the new object - the updated
        }
    }
}

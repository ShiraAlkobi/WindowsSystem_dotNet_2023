

namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;
public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newID = DataSource.Config.NextTaskId;
        Task copy = item with { Id = newID };
        DataSource.Tasks.Add(copy);
        return newID;
    }

    public void Delete(int id)
    {
        ///check if entity can be deleted
        if (!DataSource.Tasks.Exists(p => p.Id == id))
            throw new Exception($"Task with ID={id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p.Id == id);
            DataSource.Tasks.Remove(temp);
        }
    }

    public Task? Read(int id)
    {
        if (DataSource.Tasks.Exists(p => p.Id == id))
            return DataSource.Tasks.Find(p => p.Id == id);
        else return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (!DataSource.Tasks.Exists(p=>p.Id==item.Id))
            throw new Exception($"Task with ID={item.Id} does Not exist");
        else
        {
            Task temp = DataSource.Tasks.Find(p => p.Id == item.Id)!;
            DataSource.Tasks.Remove(temp);
            DataSource.Tasks.Add(item);
        }
    }
}

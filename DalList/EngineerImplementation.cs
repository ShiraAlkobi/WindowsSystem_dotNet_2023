
namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;
public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Exists(p=>p.Id==item.Id))
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        ///check if entity can be deleted
        if(!DataSource.Engineers.Exists(p => p.Id == id))
            throw new Exception($"Engineer with ID={id} does Not exist");
        else
        {
            Engineer temp = DataSource.Engineers.Find(p => p.Id == id);
            DataSource.Engineers.Remove(temp);
        }
    }

    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Exists(p => p.Id == id))
            return DataSource.Engineers.Find(p => p.Id == id);
        else return null;
    }

    public List<Engineer> ReadAll()
    {
        return new List <Engineer>(DataSource.Engineers);   
    }

    public void Update(Engineer item)
    {
        if (!DataSource.Engineers.Exists(p => p.Id == item.Id))
            throw new Exception($"Engineer with ID={item.Id} does Not exist");
        else
        {
            Engineer temp = DataSource.Engineers.Find(p => p.Id == item.Id);
            DataSource.Engineers.Remove(temp);
            DataSource.Engineers.Add(item);
        }
    }
}

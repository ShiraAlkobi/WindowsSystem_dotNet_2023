

namespace Dal;

using System.Collections;
using System.Collections.Generic;
using DalApi;
using DO;
public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newID = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = newID };
        DataSource.Dependencys.Add(copy);
        return newID;
    }

    public void Delete(int id)
    {
        ///check if entity can be deleted
        if (!DataSource.Dependencys.Exists(p => p.Id == id))
            throw new Exception($"Dependency with ID={id} does Not exist");
        else
        {
            Dependency temp = DataSource.Dependencys.Find(p => p.Id == id);
            DataSource.Dependencys.Remove(temp);
        }
    }

    public Dependency? Read(int id)
    {
        if (DataSource.Dependencys.Exists(p => p.Id == id))
            return DataSource.Dependencys.Find(p => p.Id == id);
        else return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    public void Update(Dependency item)
    {
        if (!DataSource.Dependencys.Exists(p => p.Id == item.Id))
            throw new Exception($"Dependency with ID ={ item.Id } does Not exist");
        else
        {
            Dependency temp = DataSource.Dependencys.Find(p => p.Id == item.Id);
            DataSource.Dependencys.Remove(temp);
            DataSource.Dependencys.Add(item);
        }
    }
}

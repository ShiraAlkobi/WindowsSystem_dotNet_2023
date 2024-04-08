using DalApi;
using DO;

namespace Dal;
/// <summary>
/// ICRUD functions' implementation for the dependency entity using the data saved in the right XML file 
/// in this file, we implement using the XSerializer class
/// </summary>
internal class DependencyImplementation : IDependency
{
    readonly string s_dependency_xml = "dependencys";//name of xml file
    /// <summary>
    /// creating new object and adding to xml file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        ///Loading the list of Dependencys from the file
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        //next auto id from the config class
        int nextId = Config.NextDependencyId;
        Dependency Copy = item with { Id = nextId };//creating new object with updated id
        Dependencys.Add(Copy);//adding to list
        XMLTools.SaveListToXMLSerializer<Dependency>(Dependencys, s_dependency_xml);//save list to xml file
        return nextId;
    }
    /// <summary>
    /// deleting a Dependency from the xml file
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);//loading the list
        if (!Dependencys.Exists(p => p.Id == id))//if the Dependency doesnt exist
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");//throw exception
        else
        {
            Dependency temp = Dependencys.Find(p => p.Id == id)!;//find the Dependency according to the ID
            Dependencys.Remove(temp);//remove Dependency from list
            XMLTools.SaveListToXMLSerializer<Dependency>(Dependencys, s_dependency_xml);//save updated list to xml file
        }

    }
    /// <summary>
    /// read Dependency by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);//loading the list
        return Dependencys.FirstOrDefault(p => p.Id == id);//return the right Dependency

    }
    /// <summary>
    /// read Dependency by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);//loading list
        return Dependencys.FirstOrDefault(filter);//return the the first Dependency to answer filter
    }
    /// <summary>
    /// read all Dependencys that answer to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);//loading list
        if (filter == null)
            return Dependencys;//no filter to answer,return full list
        else
            return Dependencys.Where(filter);//return sub-list where all Dependencys answer to filter

    }
    /// <summary>
    /// updating Dependency in the list from xml file
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);//loading list

        //check if it exists in the list
        if (!Dependencys.Exists(p => p!.Id == item.Id))//if Dependency doesnt exist
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        else
        {
            Dependency temp = Dependencys.Find(p => p!.Id == item.Id)!;//finding the correct Dependency
            Dependencys.Remove(temp);//remove the old object from the list
            Dependencys.Add(item);//add the new object - the updated
            XMLTools.SaveListToXMLSerializer<Dependency>(Dependencys, s_dependency_xml);//save updated list to xml file
        }
    }
}

using DalApi;
using DO;

namespace Dal;
/// <summary>
/// ICRUD functions' implementation for the task entity using the data saved in the right XML file 
/// in this file, we implement using the XSerializer class
/// </summary>
internal class TaskImplementation:ITask
{
    readonly string s_task_xml = "tasks";//name of xml file
    /// <summary>
    /// creating new object and adding to xml file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        ///Loading the list of tasks from the file
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);
        //next auto id from the config class
        int nextId = Config.NextTaskId;
        DO.Task Copy = item with { Id = nextId };//creating new object with updated id
        Tasks.Add(Copy);//adding to list
        XMLTools.SaveListToXMLSerializer<DO.Task>(Tasks, s_task_xml);//save list to xml file
        return nextId;
    }
    /// <summary>
    /// deleting a task from the xml file
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);//loading the list
        if (!Tasks.Exists(p => p.Id == id))//if the task doesnt exist
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");//throw exception
        else
        {
            DO.Task temp =Tasks.Find(p => p.Id == id)!;//find the task according to the ID
            Tasks.Remove(temp);//remove task from list
            XMLTools.SaveListToXMLSerializer<DO.Task>(Tasks, s_task_xml);//save updated list to xml file
        }

    }
    /// <summary>
    /// read task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);//loading the list
        return Tasks.FirstOrDefault(p => p.Id == id);//return the right task

    }
    /// <summary>
    /// read task by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);//loading list
        return Tasks.FirstOrDefault(filter);//return the the first task to answer filter
    }
    /// <summary>
    /// read all tasks that answer to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);//loading list
        if (filter == null)
             return Tasks;//no filter to answer,return full list
        else
            return Tasks.Where(filter);//return sub-list where all tasks answer to filter

    }
    /// <summary>
    /// updating task in the list from xml file
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task_xml);//loading list

        //check if it exists in the list
        if (!Tasks.Exists(p => p!.Id == item.Id))//if task doesnt exist
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        else
        {
            DO.Task temp = Tasks.Find(p => p!.Id == item.Id)!;//finding the correct task
            Tasks.Remove(temp);//remove the old object from the list
            Tasks.Add(item);//add the new object - the updated
            XMLTools.SaveListToXMLSerializer<DO.Task>(Tasks, s_task_xml);//save updated list to xml file

        }
    }
}

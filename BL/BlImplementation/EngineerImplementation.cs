namespace BlImplementation;
//using BlApi;
using BO;
using System;
using System.Collections.Generic;
//using DalApi;

/// <summary>
/// in this file we implement all of the BL head entity Engineer's functions 
/// we defined them in the interface IEngineer 
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    /// <summary>
    /// this field connects us to the data source. 
    /// the Factoy.Get function returns the current form of data (list or XML)
    /// it is being kept in a IDal object
    /// </summary>
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// this function gets a BO engineer oblect and adds it to the data source (converting from BO object to DO one)
    /// the function returns the id of the engineer added
    /// </summary>
    /// <param name="t"> the gotten engineer object</param>
    /// <returns></returns>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Engineer t)
    {
        ///Check for correct input
        if (t.Id < 0 || t.Name == "" || t.Cost < 0 || t.Email == "" || !t.Email!.Contains('@'))
        {
            throw new BO.BlInputCheckException("wrong input\n");
        }
        ///creates the DO engineer using the right values from the gotten object
        DO.Engineer t_engineer = new DO.Engineer(t.Id, t.Email, t.Cost, t.Name, (DO.EngineerExperience)(int)t.Level);
        try///the create function can throw an exeption
        {
            ///calling for create to add the engineer to the data source
            int t_id = _dal.Engineer.Create(t_engineer); 
            return t_id;
        }///catching the exception that can be thrown, then throwing it to the PL level to handle
        catch (DO.DalAlreadyExistsException e) { throw new BO.BlAlreadyExistsException($"Engineer with ID={t.Id} already exists");/*TODO: להוסיף חריגה מתאימה*/}
    }

    /// <summary>
    /// deletes an engineer from the data source by id
    /// </summary>
    /// <param name="id">the engineer's id to delete</param>
    /// <exception cref="BO.BlCanNotDelete"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        try 
        {
            ///search for the engineer, if found - check if there's no task assigned to it, then it can be deleted
            if (Read(id)!.Task == null) 
                _dal.Engineer.Delete(id);
            else /// if there's a task assigned to the engineer - can't delete, throw an exception
                throw new BO.BlCanNotDelete($"can`t delete engineer with ID={id}");
        } ///catch any exception that can be thrown from the delete function
        catch(DO.DalDoesNotExistException e) { throw new BO.BlDoesNotExistException($"Engineer with ID={id} doesn`t exist"); }
    }

    public Engineer? Read(int id)
    {
        DO.Engineer? t_engineer = _dal.Engineer.Read(id);
        if (t_engineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        DO.Task? taskOfEngineer=(from task in _dal.Task.ReadAll(task=>task.EngineerId == id)
                                where task.StartDate!=null&&task.CompleteDate==null
                                select task).FirstOrDefault();
        TaskInEngineer? taskInEngineer = null;
        if (taskOfEngineer != null)
            taskInEngineer = new() { Id=taskOfEngineer.Id,Alias = taskOfEngineer.Alias };
        return new BO.Engineer()
        {
            Id = id,
            Name = t_engineer.Name,
            Cost = t_engineer.Cost,
            Email = t_engineer.Email,
            Level = (BO.EngineerExperience)(int)t_engineer.Level,
            Task = taskInEngineer
        };

    }

    public IEnumerable<BO.Engineer>? ReadAll(Func<Engineer, bool>? filter)
    {

        return _dal.Engineer.ReadAll(filter).Select( item => Read(item.Id));
    }

    public void Update(Engineer t)
    {
        if (t.Id < 0 || t.Name == "" || t.Cost < 0 || t.Email == "" || !t.Email!.Contains('@'))
        {
            throw new BO.BlInputCheckException("wrong input\n");
        }
        try
        {
            if ((int)t.Level < (int)_dal.Engineer.Read(t.Id).Level)
                throw new BO.BlCanNotUpdate($"can`t update engineer");
            DO.Engineer t_engineer = new DO.Engineer(t.Id, t.Email, t.Cost, t.Name, (DO.EngineerExperience)(int)t.Level);
            _dal.Engineer.Update(t_engineer);
            DO.Task? t_task = null;
            try
            {
               t_task = _dal.Task.Read(t.Task.Id);
            } catch (DO.DalDoesNotExistException e)
            { throw new BO.BlDoesNotExistException($"Task with ID={t.Id} doesn`t exist"); }
            DO.Task updatedTask= t_task with { EngineerId = t.Id };
            _dal.Task.Update(updatedTask);
        }
        catch (DO.DalDoesNotExistException e) 
        { throw new BO.BlDoesNotExistException($"Engineer with ID={t.Id} doesn`t exist"); }
    }
}

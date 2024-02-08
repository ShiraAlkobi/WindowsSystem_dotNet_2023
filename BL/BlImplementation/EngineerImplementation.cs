namespace BlImplementation;
//using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Cryptography;

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
        if ( t.Id < 0) 
        {
            throw new BO.BlInputCheckException("Id can't be negative\n");
        }
        if (t.Name == "")
        {
            throw new BO.BlInputCheckException("must insert name\n");
        }
        if (t.Cost < 0)
        {
            throw new BO.BlInputCheckException("cost can't be negative\n");
        }
        if (!(new EmailAddressAttribute().IsValid(t.Email)))
        {
            throw new BO.BlInputCheckException("email is not valid\n");
        }
        ///creates the DO engineer using the right values from the gotten object
        DO.Engineer t_engineer = new DO.Engineer(t.Id, t.Email, t.Cost, t.Name, (DO.EngineerExperience)(int)t.Level);
        try///the create function can throw an exeption
        {
            ///calling for create to add the engineer to the data source
            int t_id = _dal.Engineer.Create(t_engineer); 
            return t_id;
        }///catching the exception that can be thrown, then throwing it to the PL level to handle
        catch (DO.DalAlreadyExistsException e) { throw new BO.BlAlreadyExistsException($"Engineer with ID={t.Id} already exists");}
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
                throw new BO.BlCanNotDelete($"can`t delete engineer with ID={id} because they`re assigned to a task\n");
        } ///catch any exception that can be thrown from the delete function
        catch(DO.DalDoesNotExistException e) { throw new BO.BlDoesNotExistException($"Engineer with ID={id} doesn`t exist"); }
    }

    /// <summary>
    /// returns the right engineer object from the data source accorrding to the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public Engineer Read(int id) => 
        ///if the DO read function returns a non nullable object type - then convert it from DO to BO type
        ///else, throw an exception for not founding the object
             _dal.Engineer.Read(id) is DO.Engineer doEngineer ? doToBoEngineer(doEngineer) : 
        throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

    /// <summary>
    /// help function - converts a DO engineer to BO one
    /// </summary>
    /// <param name="doEngineer"></param>
    /// <returns></returns>
    private Engineer doToBoEngineer(DO.Engineer doEngineer)
    {
        ///find the active task that is assigned to the engineer
        DO.Task? taskOfEngineer = _dal.Task.Read(task => task.EngineerId == doEngineer.Id &&
            task.StartDate != null && task.CompleteDate == null);

        ///create the BO engineer with the right params
        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Cost = doEngineer.Cost,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            ///if taskOfEngineer is null - there isn't a task assigned for the engineer
            ///else, create the object with the right params
            Task = taskOfEngineer is null ? null : new TaskInEngineer() { Id = taskOfEngineer.Id, Alias = taskOfEngineer.Alias }
        };
    }

    /// <summary>
    /// returns a collection of engineers that answer to the filter sent
    /// if filter is null,return all of the engineers
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<Engineer, bool>? filter = null)=>
        ///ReadAll function return an IEnumreable of DO objects
        ///so convertion is needed in order to use filter on the object
         _dal.Engineer.ReadAll().Select(engineer => doToBoEngineer(engineer))
            .Where(engineer => filter is null ? true : filter(engineer));
 
    /// <summary>
    /// this function updates details of engineer
    /// </summary>
    /// <param name="t">engineer to update</param>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlCanNotUpdate"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(Engineer t)
    {
        
        ///input check:
        if (t.Name == "")
        {
            throw new BO.BlInputCheckException("must insert name\n");
        }
        if (t.Cost < 0)
        {
            throw new BO.BlInputCheckException("cost can't be negative\n");
        }
        if (!(new EmailAddressAttribute().IsValid(t.Email)))
        {
            throw new BO.BlInputCheckException("email is not valid\n");
        }

        try
        {
            
            if ((int)t.Level < (int)_dal.Engineer.Read(t.Id).Level)//can only upgrade level of engineer
                throw new BO.BlCanNotUpdate($"can only upgrade level of engineer\n");
            if (t.Task is not null)//if the user assigned a task to the engineer
            {
                DO.Task? t_task = _dal.Task.Read(t.Task.Id);//read the task to be assigned
                if ((int)t.Level <(int)t_task.Complexity)//can only assign engineer with enough experience for the comlexity of the task
                    throw new BO.BlCanNotUpdate($"can`t assign engineer with not enough experience");
           
                if (t_task.EngineerId > 0)//if there is already an engineer assigned to the task
                    throw new BO.BlCanNotUpdate($"can`t update engineer because there is already an engineer assigned to the task");
                // going through the dependencies and checking if there is a previous task  that is not yet completed
                DO.Task? p = (from item in _dal.Dependency.ReadAll()
                              where item.DependentTask == t_task.Id
                              let temp = _dal.Task.Read(item.DependsOnTask)
                              where (temp.CompleteDate is null)
                              select temp).FirstOrDefault();

                if (p is not null)
                    throw new BO.BlCanNotUpdate("the previous tasks are still in progress");


            }
            DO.Engineer t_engineer = new DO.Engineer(t.Id, t.Email, t.Cost, t.Name, (DO.EngineerExperience)t.Level);//creating new object with updated fields
            _dal.Engineer.Update(t_engineer);//calling dal function to update the data base
            
        }
        catch (DO.DalDoesNotExistException e)//for if the read function throws an exception
        { throw new BO.BlDoesNotExistException($"Engineer with ID={t.Id} doesn`t exist"); }
    }
    /// <summary>
    /// function that sorts the engineer bt name
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> SortByName()
    {
        return from item in ReadAll()
               orderby item.Name
               select item;
    }

}

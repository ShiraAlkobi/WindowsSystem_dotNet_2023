namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer t)
    {
        if (t.Id < 0 || t.Name == "" || t.Cost < 0 || t.Email == "" || !t.Email!.Contains('@'))
        {
            throw new BO.BlInputCheckException("wrong input\n");
        }
        DO.Engineer t_engineer = new DO.Engineer(t.Id, t.Email, t.Cost, t.Name, (DO.EngineerExperience)(int)t.Level);
        try
        {
            int t_id = _dal.Engineer.Create(t_engineer);
            return t_id;
        }
        catch (DO.DalAlreadyExistsException e) { throw new BO.BlAlreadyExistsException($"Engineer with ID={t.Id} already exists");/*TODO: להוסיף חריגה מתאימה*/}
    }

    public void Delete(int id)
    {
        try
        {
            if (Read(id)!.Task == null)
                _dal.Engineer.Delete(id);
            else
                throw new BO.BlCanNotDelete($"can`t delete engineer with ID={id}");
        }
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

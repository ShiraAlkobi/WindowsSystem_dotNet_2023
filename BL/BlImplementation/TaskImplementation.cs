namespace BlImplementation;

using System.Net.NetworkInformation;
using System;
using BlApi;
using BO;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task t)
    {
        if (t.Id < 0 || string.IsNullOrEmpty(t.Alias))
            throw new BO.BlInputCheckException("wrong input\n");

        try
        {
            t.Dependencies?
                .Select(item => new DO.Dependency(0, t.Id, item.Id))
                .ToList()
                .ForEach(dependency => _dal.Dependency.Create(dependency));

            DO.Task t_task = new(0, t.Alias, t.Description, false, t.CreatedAtDate, t.ScheduledDate, t.StartDate,
                                 t.RequiredEffortTime, t.DeadlineDate, t.CompleteDate, t.Deliverables, t.Remarks, 0, (DO.EngineerExperience)t.Complexity);

            return _dal.Task.Create(t_task);
        }
        catch (DO.DalAlreadyExistsException)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={t.Id} already exists");
        }
    }


    public void Delete(int id)
    {
        try
        {
            BO.Task t_task = Read(id);
            if (_dal.Dependency.ReadAll().Any(item => item.DependsOnTask == id))
            {
                throw new BO.BlCanNotDelete($"can't delete task with ID={id}");
            }
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException)
        { throw new BO.BlDoesNotExistException($"Task with ID={id} doesn`t exist"); }

    }


    /// <summary>
    /// returns the right task object from the data source accorrding to the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task? Read(int id) =>
    ///if the DO read function returns a non nullable object type - then convert it from DO to BO type
    ///else, throw an exception for not founding the object
    _dal.Task.Read(id) is DO.Task doTask ? doToBoTask(doTask) :
    throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");


    /// <summary>
    /// help function - converts a DO task to BO one
    /// </summary>
    /// <param name="doEngineer"></param>
    /// <returns></returns>
    private BO.Task doToBoTask(DO.Task t)
    {
        ///find the values for the missing fields, using the help functions
        List<BO.TaskInList>? t_dependecies = getDependencies(t.Id);
        BO.Status t_status = getStatus(t);
        DateTime? t_forecast = getForecastDate(t);
        BO.EngineerInTask? t_engineer = getEngineer(t);

        ///create the BO task with the right params
        return new BO.Task()
        {
            Id = t.Id,
            Alias = t.Alias,
            Description = t.Description,
            Status = t_status,
            CreatedAtDate = t.CreatedAtDate,
            ScheduledDate = t.ScheduledDate,
            StartDate = t.StartDate,
            ForecastDate = t_forecast,
            RequiredEffortTime = t.RequiredEffortTime,
            DeadlineDate = t.DeadlineDate,
            CompleteDate = t.CompleteDate,
            Deliverables = t.Deliverables,
            Dependencies = t_dependecies,
            Remarks = t.Remarks,
            Engineer = t_engineer,
            Complexity = (BO.EngineerExperience)t.Complexity
        };
    }

    /// <summary>
    /// returns a collection of tasks that answer to the filter sent
    /// if filter is null,return all of the tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool> filter = null)
    {
        ///ReadAll function return an IEnumreable of DO objects
        ///so convertion is needed in order to use filter on the object
        return _dal.Task.ReadAll().Select(task => doToBoTask(task))
           .Where(task => filter is null ? true : filter(task))
           .Select(item => new BO.TaskInList()
           {
               Id = item.Id,
               Alias = item.Alias,
               Description = item.Description,
               Status = item.Status
           });

    }


    public void Update(BO.Task t)
    {
        if (t.Id < 0 || t.Alias == "" || t.Alias == null)
            throw new BO.BlInputCheckException("wrong input\n");

        try
        {
           
            DO.Task t_task = new()
            {
                Id = t.Id,
                Alias = t.Alias,
                Description = t.Description,
                CreatedAtDate = t.CreatedAtDate,
                ScheduledDate = t.ScheduledDate,
                StartDate = t.StartDate,
                RequiredEffortTime = t.RequiredEffortTime,
                DeadlineDate = t.DeadlineDate,
                CompleteDate = t.CompleteDate,
                Deliverables = t.Deliverables,
                Remarks = t.Remarks,
                EngineerId =( t.Engineer is null)?0:t.Engineer.Id,
                Complexity = (DO.EngineerExperience)t.Complexity
            };

            _dal.Task.Update(t_task);
            
        }
        catch (DO.DalDoesNotExistException)
        { throw new BO.BlDoesNotExistException($"Task with ID={t.Id} does Not exist"); }
        
    }

    public void UpdateScedualedDate(int id, DateTime date)
    {
        DateTime? t_date;
        BO.Task t = Read(id);
        if (t.ScheduledDate is not null) return;
        else
        {
            if (t.Dependencies.Count==0)
                t_date = date.AddDays(3);
            else
            {
                List<BO.TaskInList?> temp = (from item in t.Dependencies
                                             where item.Status < BO.Status.Scheduled
                                             select item).ToList();


                if (temp.Count>0)
                    foreach (TaskInList item in temp)
                    {
                        UpdateScedualedDate(item.Id, date);
                    }
                DateTime? help;
                t_date = getForecastDate(_dal.Task.Read(t.Dependencies.First().Id));
                foreach (var item in t.Dependencies)
                {
                    help = getForecastDate(_dal.Task.Read(item.Id));
                    if (t_date < help)
                        t_date = help;

                }
                t_date = t_date?.AddDays(1);
            }
            try
            {
                DO.Task t_task = new()
                {
                    Id = t.Id,
                    Alias = t.Alias,
                    Description = t.Description,
                    CreatedAtDate = t.CreatedAtDate,
                    ScheduledDate = t_date,
                    StartDate = t.StartDate,
                    RequiredEffortTime = t.RequiredEffortTime,
                    DeadlineDate = t.DeadlineDate,
                    CompleteDate = t.CompleteDate,
                    Deliverables = t.Deliverables,
                    Remarks = t.Remarks,
                    EngineerId = t.Engineer is not null ? t.Engineer.Id : 0,
                    Complexity = (DO.EngineerExperience)t.Complexity
                };

                _dal.Task.Update(t_task);
            }
            catch (DO.DalDoesNotExistException)
            { throw new BO.BlDoesNotExistException($"Task with ID={t.Id} does Not exist"); }
        }
    }


    private List<BO.TaskInList> getDependencies(int id)
    {

        return (from item in _dal.Dependency.ReadAll()
                where item.DependentTask == id
                let t_task = _dal.Task.Read(item.DependsOnTask)
                let t_description = t_task.Description
                let t_Alias = t_task.Alias
                let t_status = getStatus(t_task)
                select new BO.TaskInList() { Id = item.DependsOnTask, Description = t_description, Alias = t_Alias, Status = t_status })
                .ToList();
    }

    private BO.Status getStatus(DO.Task t)
    {
        if (t.ScheduledDate == null)
            return BO.Status.Unscheduled;
        if (t.StartDate == null)
            return BO.Status.Scheduled;
        if (t.CompleteDate == null)
            return BO.Status.OnTrack;
        else return BO.Status.Done;

        //TODO:add jeopardy condition
    }

    public DateTime? getForecastDate(DO.Task t)
    {
        return t.ScheduledDate + t.RequiredEffortTime;
                
    }

    private BO.EngineerInTask? getEngineer(DO.Task t)
    {
        if (t.EngineerId == 0)
            return null;
        return new BO.EngineerInTask() { Id = t.EngineerId, Name = _dal.Engineer.Read(t.EngineerId)!.Name };
    }
    public IEnumerable<IGrouping<BO.Status, BO.TaskInList>> GroupByStatus()
    {
        return from item in ReadAll()
               group item by item.Status into groups
               select groups;
    }
}
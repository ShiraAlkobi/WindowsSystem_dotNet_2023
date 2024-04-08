namespace BlImplementation;

using System.Net.NetworkInformation;
using System;
using BlApi;
using BO;
using System.Diagnostics;

/// <summary>
/// this class implement the task interface
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//creating an appearance to connect bl to dal
    /// <summary>
    /// function to create a new task
    /// </summary>
    /// <param name="t"> task to add</param>
    /// <returns>id of task</returns>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task t)
    {
        if (t.Id < 0 )
            throw new BO.BlInputCheckException("Id can't be negative\n");
        if(string.IsNullOrEmpty(t.Alias))
            throw new BO.BlInputCheckException("must insert alias\n");
        if (t.RequiredEffortTime == null)
            throw new BO.BlInputCheckException("must insert RequiredEffortTime\n");
        if (t.Complexity == BO.EngineerExperience.All)
            throw new BO.BlInputCheckException("please select complexity\n");

        try
        {
            //going through the list of dependencies of the task t and creating new dependecien in the database
            t.Dependencies?
                .Select(item => new DO.Dependency(0, t.Id, item.Id))
                .ToList()
                .ForEach(dependency => _dal.Dependency.Create(dependency));

            //create the task
            DO.Task t_task = new(0, t.Alias, t.Description, t.CreatedAtDate, t.ScheduledDate, t.StartDate,
                                t.RequiredEffortTime, t.CompleteDate, t.Deliverables, t.Remarks, 0, (DO.EngineerExperience)t.Complexity);
            
            //call dal fuction to save in data base
            return _dal.Task.Create(t_task);
        }
        catch (DO.DalAlreadyExistsException)//if the task id already exists
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={t.Id} already exists");
        }
    }

    /// <summary>
    /// deleting a task
    /// </summary>
    /// <param name="id">id of task to delete</param>
    /// <exception cref="BO.BlCanNotDelete"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        try
        {
            BO.Task t_task = Read(id);//get the task to be deleted
            if (_dal.Dependency.ReadAll().Any(item => item.DependsOnTask == id))//if theres any task that depends on this task
            {
                throw new BO.BlCanNotDelete($"can't delete task with ID={id}");//can't delete it
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
            CompleteDate = t.CompleteDate,
            Deliverables = t.Deliverables,
            Dependencies = t_dependecies,
            Remarks = t.Remarks,
            Engineer = t_engineer,
            Complexity = (BO.EngineerExperience)t.Complexity
        };
    }

    /// <summary>
    /// returns a collection of taskinlist that answer to the filter sent
    /// if filter is null,return all of the tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool> filter = null)
    {
        ///ReadAll function return an IEnumreable of DO objects
        ///so convertion is needed in order to use filter on the object
        return _dal.Task.ReadAll()
      .Select(task => doToBoTask(task))
      .Where(task => filter is null ? true : filter(task))
      .OrderBy(task => task.Id) // Add this line for ordering by Id
      .Select(item => new BO.TaskInList()
      {
          Id = item.Id,
          Alias = item.Alias,
          Description = item.Description,
          Status = item.Status
      });

    }

    /// <summary>
    /// updates the task
    /// </summary>
    /// <param name="t">task to update</param>
    /// <exception cref="BO.BlInputCheckException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task t)
    {
        //input check
        if (t.Id < 0)
            throw new BO.BlInputCheckException("Id can't be negative\n");
        if (string.IsNullOrEmpty(t.Alias))
            throw new BO.BlInputCheckException("must insert alias\n");
        if (t.RequiredEffortTime == null)
            throw new BO.BlInputCheckException("must insert RequiredEffortTime\n");
        if (t.Complexity == BO.EngineerExperience.All)
            throw new BO.BlInputCheckException("please select complexity\n");
        try
        {
         ///create new task of DO type  
            DO.Task t_task = new()
            {
                Id = t.Id,
                Alias = t.Alias,
                Description = t.Description,
                CreatedAtDate = t.CreatedAtDate,
                ScheduledDate = t.ScheduledDate,
                StartDate = t.StartDate,
                RequiredEffortTime = t.RequiredEffortTime,
                CompleteDate = t.CompleteDate,
                Deliverables = t.Deliverables,
                Remarks = t.Remarks,
                EngineerId =( t.Engineer is null)?0:t.Engineer.Id,
                Complexity = (DO.EngineerExperience)t.Complexity
            };

            _dal.Task.Update(t_task);//call dal function to save in database
            
        }
        catch (DO.DalDoesNotExistException)//if the task doesnt exist, update will throw an exception
        { throw new BO.BlDoesNotExistException($"Task with ID={t.Id} does Not exist"); }
        
    }
    /// <summary>
    /// recursive function for updating schedual dates for the tasks
    /// an automatic initialization- finding the schedual date based on the dependencies list of each task
    /// </summary>
    /// <param name="id">id of task to update</param>
    /// <param name="date">start date of project, if a task doesnt have any previous tasks,its schedual date 
    /// will be start+3 days</param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void UpdateScedualedDate(int id, DateTime date)
    {
        DateTime? t_date;
        BO.Task t = Read(id);//get the task to update(no try needed because the task is from the list of tasks so it exists)
        if (t.ScheduledDate is not null) return;//if the task already has a schedual date,return
        else
        {
            /// if a task doesnt have any previous tasks, its scheduale date
            /// will be start+3
            if (t.Dependencies.Count==0)
                t_date = date.AddDays(3);
            else
            {
                List<BO.TaskInList?> temp = (from item in t.Dependencies//finding all the tasks that this task depends on
                                             where item.Status < BO.Status.Scheduled
                                             select item).ToList();

                //if the task is dependent on any task,call the function on these tasks because they need to be updated first
                if (temp.Count>0)
                    foreach (TaskInList item in temp)
                    {
                        UpdateScedualedDate(item.Id, date);
                    }
                
                //this happens after all tasks in dependencies list of the curreent task has been initialized
                DateTime? help;
                t_date = getForecastDate(_dal.Task.Read(t.Dependencies.First().Id));//the forcast date of first dependency
                foreach (var item in t.Dependencies)//go through list of dependencies and finding the max of forecast date
                {
                    help = getForecastDate(_dal.Task.Read(item.Id));
                    if (t_date < help)
                        t_date = help;

                }
                t_date = t_date?.AddDays(1);//the scheduale date of current task will be the max of forecast dates of tasks in dependency list+1
            }
            try//creating new task
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
                    CompleteDate = t.CompleteDate,
                    Deliverables = t.Deliverables,
                    Remarks = t.Remarks,
                    EngineerId = t.Engineer is not null ? t.Engineer.Id : 0,
                    Complexity = (DO.EngineerExperience)t.Complexity
                };

                _dal.Task.Update(t_task);//call dal function to update in database
            }
            catch (DO.DalDoesNotExistException)
            { throw new BO.BlDoesNotExistException($"Task with ID={t.Id} does Not exist"); }
        }
    }

    /// <summary>
    /// help function to get the dependencies of each task-used by read function
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BO.TaskInList> getDependencies(int id)
    {

        return (from item in _dal.Dependency.ReadAll()//going through dependency list and finding all dependencies
                                                      //that their dependenttask field is the wanted task's id
                where item.DependentTask == id
                let t_task = _dal.Task.Read(item.DependsOnTask)//getting the task that the current depends on
                let t_description = t_task.Description
                let t_Alias = t_task.Alias
                let t_status = getStatus(t_task)
                //creating new taskinlist object for dependencies field of the task
                select new BO.TaskInList() { Id = item.DependsOnTask, Description = t_description, Alias = t_Alias, Status = t_status })
                .ToList();
    }

    /// <summary>
    /// help function to find at what stage the task is-used by read function
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private BO.Status getStatus(DO.Task t)
    {
        if (t.ScheduledDate == null)//if there isnt a schedual date
            return BO.Status.Unscheduled;
        if (t.StartDate == null)//if there is a schedual date but not start
            return BO.Status.Scheduled;

        if (t.CompleteDate == null && _dal.getClock() > (t.ScheduledDate + t.RequiredEffortTime))
            return BO.Status.Delayed;
        if (t.CompleteDate == null)//if there is a start date but not complete
            return BO.Status.OnTrack;
        else return BO.Status.Done;//if there is a complete but not done

    }
    /// <summary>
    /// help function to get forecast date to finish task-used by read function
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public DateTime? getForecastDate(DO.Task t)
    {
        return t.ScheduledDate + t.RequiredEffortTime;
                
    }
    /// <summary>
    /// help function to get engineer for engineer field in task
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private BO.EngineerInTask? getEngineer(DO.Task t)
    {
        if (t.EngineerId == 0)//there is no engineer
            return null;
        return new BO.EngineerInTask() { Id = t.EngineerId, Name = _dal.Engineer.Read(t.EngineerId)!.Name };//create the engineer in task by using read function
    }
    /// <summary>
    /// function to group task by their status
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<BO.Status, BO.TaskInList>> GroupByStatus()
    {
        return from item in ReadAll()
               group item by item.Status into groups
               select groups;
    }

    /// <summary>
    /// Creates a new dependency according to the given objects
    /// </summary>
    /// <param name="dependsOn"></param>
    /// <param name="dependent"></param>
    public void UpdateDependencies(int dependsOn, int dependent)
    {
        DO.Dependency d = new DO.Dependency() { DependentTask = dependent, DependsOnTask = dependsOn };
        _dal.Dependency.Create(d);
    }

    /// <summary>
    /// Deletes a dependency according to the given id
    /// </summary>
    /// <param name="dependsOn"></param>
    /// <param name="dependent"></param>
    public void DeleteDependencies(int dependsOn, int dependent)
    {
        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll();
        DO.Dependency? t_dependency=(from item in dependencies
                                     where item.DependsOnTask == dependsOn && item.DependentTask == dependent
                                     select item).FirstOrDefault();
        if (t_dependency != null)
        {
            _dal.Dependency.Delete(t_dependency.Id);
        }
    }

    /// <summary>
    /// checks if taskToCheck is dependent on t - in all of its dependencies and their dependencies
    /// </summary>
    /// <param name="taskToCheck"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool CircularDependency(int taskToCheck, int t)
    {
        bool flag = true;
        ///check if t is in the taskToCheck dependencies
        BO.TaskInList? temp = (from item in getDependencies(taskToCheck)
                               where item.Id == t
                               select item).FirstOrDefault();
        if (temp != null) { return false; }
        else
        {
            //if we didn't find t in taskToCheck dependencies, we want to check this also in taskToCheck dependencies
            //so there won't be circular dependency
            foreach (var item in getDependencies(taskToCheck))
            {
                flag = CircularDependency(item.Id, t);
                if (!flag) { return flag; }
            }
        }
        return true;
    }
}
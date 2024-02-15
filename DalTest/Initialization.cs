namespace DalTest;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;
/// <summary>
/// this file will initialize the entities with our project`s(house renovation) details- 
/// 20 tasks to do, 6 workers and at least 40 dependencies
/// </summary>
public static class Initialization
{
    /// <summary>
    /// creating interface type object of IDal so we can use the entities` methods
    /// </summary>
    private static IDal? s_dal;
    private static readonly Random s_rand = new();//random type object for when needed
    /// <summary>
    /// this function create the tasks that need to be done for the renovating
    /// </summary>
    private static void createTasks()
    {
        string[] taskDescriptions = {//array with 20 tasks
                "Define Renovation Goals and Budget",
                "Hire Architect and Interior Designer",
                "Obtain Necessary Permits",
                "Demolition and Site Preparation",
                "Structural Changes (e.g., Walls, Ceilings)",
                "Plumbing and Electrical Work",
                "Flooring Installation",
                "Kitchen Remodeling",
                "Bathroom Remodeling",
                "Painting and Wall Finishing",
                "Window and Door Replacement",
                "Roof Repair or Replacement",
                "Install HVAC Systems",
                "Lighting Installation",
                "Insulation Installation",
                "Cabinetry Installation",
                "Fixture and Appliance Installation",
                "Landscaping and Outdoor Improvements",
                "Final Inspection",
                "Cleanup and Interior Decorating"
            };
        ///loop that will initialize the id, create time and description fields of task
        ///and push it to the list using create() of interface
        for (int i = 0; i < 20; i++)
        {
            TimeSpan t_requiredEffortTime = TimeSpan.FromDays(s_rand.Next(30, 50));
            string t_Alias = $"TaskName{i + 1}";//id
            string t_Description = taskDescriptions[i];//what is the task (using the array)
            DateTime t_CreatedAtDate = DateTime.Today;//initialize the creation date of the task to today
            //creating new task wuth the correct fields
            Task t_Task = new Task(0, t_Alias, t_Description, false, t_CreatedAtDate, default, default, t_requiredEffortTime, default, default, null, null, 0, 0);
            s_dal!.Task.Create(t_Task);//push it to list
        }
    }

    /// <summary>
    /// this function initialize 6 'engineers'-workers with random ids and push them to the engineer list
    /// </summary>
    private static void createEngineers()
    {
        //6 names for the engineers
        string[] names =
        { "ProjectManager","Architect","InteriorDesigner","ConstructionCrew","Electrician","Plumber" };
        Engineer t_Engineer = new Engineer();//create new engineer
        for (int i = 0; i < 6; i++)
        {
            int t_Id;
            do
                t_Id = s_rand.Next(100000000, 999999999);//random id
            while (s_dal!.Engineer.Read(t_Id) != null);//if the random id already exists, draw another one
            string t_Email = $"{names[i]}@jct.com"; //create an email according to the engineer's name
            double t_Cost = s_rand.Next(50,400); //create a random hourly wage
            DO.EngineerExperience t_Level = (DO.EngineerExperience)s_rand.Next(1, 5);
            t_Engineer = new Engineer(t_Id, t_Email, t_Cost, names[i],t_Level);//initialize the engineer with the id
            s_dal!.Engineer.Create(t_Engineer);//push to list
        }
    }
    /// <summary>
    /// this function initialize dependencies- cant start task without other done
    /// </summary>
    private static void CreateDependencies()
    {
        var TaskList = s_dal!.Task.ReadAll().ToList<Task>();//import new list (IEnumerable type)
        if (TaskList != null)//if the list is not empty
        {
            Dependency d;
            Task t1;
            //loop that will create the dependencies between the
            //first task-Define Renovation Goals and Budget to the others
            //cant start any of the other tasks without completing the first one
            for (int i = 1; i <19; i++)
            {
                t1 = TaskList[i];
                d = new Dependency(0, t1.Id, TaskList[0].Id);
                s_dal!.Dependency.Create(d);
            }
            //loop that will create the dependencies between the
            //last task-Cleanup and Interior Decorating to the others
            //cant start the last task without completing all the others
            for (int i = 0; i < 18; i++)
            {
                t1 = TaskList[i+1];
                d = new Dependency(0, TaskList[19].Id, t1.Id);
                s_dal!.Dependency.Create(d);
            }
            //loop that will create the dependencies between the
            //second to last task-Final Inspection to the others
            //cant start this task without completing the tasks before it
            for (int i = 0; i < 17; i++)
            {
                t1 = TaskList[i+1];
                d = new Dependency(0, TaskList[18].Id, t1.Id);
                s_dal!.Dependency.Create(d);
            }
        }
    }
    /// <summary>
    /// this function actually sending the interface objects to the create functions 
    /// we defined and do the initialization 
    /// </summary>
    /// <param name="dalTask"></param>
    /// <param name="dalEngineer"></param>
    /// <param name="dalDependency"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = Factory.Get;
        createTasks();
        createEngineers();
        CreateDependencies();
    }

    public static void emptyData()
    {
        s_dal = Factory.Get;
        ///deleting the entities' data from every file
        List<Task> tasks = s_dal!.Task.ReadAll()!.ToList<Task>();
        if (tasks.Count > 0)
        {
            foreach (Task task in tasks)
            {
                s_dal.Task.Delete(task.Id);
            }
        }

        List<Engineer> engineers = s_dal.Engineer.ReadAll()!.ToList<Engineer>();
        if (engineers.Count > 0)
        {
            foreach (Engineer engineer in engineers)
            {
                s_dal.Engineer.Delete(engineer.Id);
            }
        }

        List<Dependency> dependencys = s_dal.Dependency.ReadAll()!.ToList<Dependency>();
        if (dependencys.Count > 0)
        {
            foreach (Dependency dependency in dependencys)
            {
                s_dal.Dependency.Delete(dependency.Id);
            }
        }
    }



}

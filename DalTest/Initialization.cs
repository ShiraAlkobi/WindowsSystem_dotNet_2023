namespace DalTest;
using DalApi;
using DO;
using System;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
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
        string[] taskNames = {//array with 20 tasks
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
        string[] taskDescriptions =
            {
                "Work with stakeholders to set clear renovation goals and establish a budget.",
                "Engage professionals to design and plan the renovation project.",
                "Acquire all required permits from local authorities.",
                "Clear the space by demolishing existing structures and preparing the site.",
                "Implement structural modifications as needed.",
                "Install or modify plumbing and electrical systems.",
                "Lay down new flooring materials.",
                "Renovate and upgrade the kitchen area.",
                "Renovate and upgrade the bathroom area.",
                "Apply paint and finish to walls.",
                "Install new windows and doors.",
                "Repair or replace the roof as necessary.",
                "Install heating, ventilation, and air conditioning systems.",
                "Install new lighting fixtures.",
                "Add insulation to walls and ceilings.",
                "Install cabinets and storage units.",
                "Install fixtures and appliances.",
                "Enhance the outdoor spaces.",
                "Conduct a thorough inspection to ensure everything meets standards.",
                "Clean the renovated space and add final interior touches."
         };
        ///loop that will initialize the id, create time and description fields of task
        ///and push it to the list using create() of interface
        for (int i = 0; i < 20; i++)
        {
            DO.EngineerExperience t_Complexity = (DO.EngineerExperience)s_rand.Next(1, 6);
            TimeSpan t_requiredEffortTime = TimeSpan.FromDays(s_rand.Next(7, 20));
            string t_Alias = taskNames[i];//id
            string t_Description = taskDescriptions[i];//what is the task (using the array)
            DateTime t_CreatedAtDate = DateTime.Now;//initialize the creation date of the task to today
            //creating new task wuth the correct fields
            Task t_Task = new Task(0, t_Alias, t_Description, false, t_CreatedAtDate, default, default, t_requiredEffortTime, default, default, null, null, 0, t_Complexity);
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
        {

            "Amit Cohen",
            "Maya Levi",
            "Odeya Lapian",
            "Yael Shoham",
            "Itay Avraham",
            "Noa Moshe" 
        };
        Engineer t_Engineer = new Engineer();//create new engineer
        for (int i = 0; i < 6; i++)
        {
            int t_Id;
            do
                t_Id = s_rand.Next(100000000, 999999999);//random id
            while (s_dal!.Engineer.Read(t_Id) != null);//if the random id already exists, draw another one
            string t_Email = $"{names[i].Split(' ')[0]}@reno.com"; //create an email according to the engineer's name
            double t_Cost = s_rand.Next(50,400); //create a random hourly wage
            DO.EngineerExperience t_Level = (DO.EngineerExperience)s_rand.Next(1, 6);
            t_Engineer = new Engineer(t_Id, t_Email, t_Cost, names[i],t_Level);//initialize the engineer with the id
            s_dal!.Engineer.Create(t_Engineer);//push to list
        }
        s_dal!.Engineer.Create(new Engineer(87654321, "Engineer@reno.com", 50, "Engineer", EngineerExperience.Expert));
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
            d = new Dependency(0, 2, 1);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 3, 2);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 4, 3);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 5, 4);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 6, 4);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 7, 4);
            s_dal!.Dependency.Create(d);
            for (int i=5;i<8;i++)
            {
                d = new Dependency(0, 8, i);
                s_dal!.Dependency.Create(d);
            }
            for (int i = 5; i < 8; i++)
            {
                d = new Dependency(0, 9, i);
                s_dal!.Dependency.Create(d);
            }
            for (int i = 10; i < 16; i++)
            {
                d = new Dependency(0, i, 5);
                s_dal!.Dependency.Create(d);
                d = new Dependency(0, i, 6);
                s_dal!.Dependency.Create(d);
            }
            d = new Dependency(0, 11, 10);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 14, 10);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 16, 8);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 17, 8);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 17, 9);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 18, 12);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 18, 13);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 8);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 9);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 11);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 14);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 15);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 19, 17);
            s_dal!.Dependency.Create(d);
            d = new Dependency(0, 20, 19);
            s_dal!.Dependency.Create(d);

         
        }
    }

    /// <summary>
    /// this function initialize users- for each engineer created, a user is created
    /// </summary>
    private static void CreateUsers()
    {
        ///add the manager to the data base
        s_dal!.User.Create(new User(12345678, "12345678", "Manager123", DO.Position.Manager));
        ///get all the engineers
        IEnumerable<DO.Engineer?> engineers = s_dal!.Engineer.ReadAll();
        string t_password;
        ///for each created engineer, create a user
        ///the user's name is the engineer's id
        ///the user's password is the engineer's name (without spaces) and the last 4 digits of his id
        foreach (var engineer in engineers) 
        {
            t_password = engineer.Name.Replace(" ", "");
            t_password += LastFourDigits(engineer.Id);
            s_dal.User.Create(new User(engineer.Id, engineer.Id.ToString(), t_password, DO.Position.Engineer));
        }
    }
    /// <summary>
    /// help function, gets a number and returns a string of its 4 last digits
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string LastFourDigits(int number)
    {
        string numberString = number.ToString();
        return numberString.Substring(numberString.Length - 4);
       
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
        CreateUsers();  
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

        List<User> users = s_dal.User.ReadAll()!.ToList<User>();
        if (users.Count > 0)
        {
            foreach (User user in users)
            {
                s_dal.User.Delete(user.Id);
            }
        }

        s_dal.ResetId();
        s_dal.setStatus();
        s_dal.setClock(DateTime.Now.Date);
    }



}

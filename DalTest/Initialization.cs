namespace DalTest;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;

public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new();
    private static void createTasks()
    {
        string[] taskDescriptions = {
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

        for (int i = 0; i < taskDescriptions.Length; i++)
        {
            string t_Alias = $"TaskName{i + 1}";
            string t_Description = taskDescriptions[i];
            DateTime t_CreatedAtDate = DateTime.Today;

            Task t_Task = new Task(0, t_Alias, t_Description, false, t_CreatedAtDate, default, default, default, default, default, null, null, 0, 0);
            s_dalTask!.Create(t_Task);
        }
    }


    private static void createEngineers()
    {
        Engineer t_Engineer = new Engineer();
        for (int i = 1; i <= 6; i++)
        {
            int t_Id;
            do
                t_Id = s_rand.Next(100000000, 999999999);
            while (s_dalEngineer!.Read(t_Id) != null);
            t_Engineer = new Engineer(t_Id, null, null, null, null);
            s_dalEngineer!.Create(t_Engineer);
        }
    }
    private static void CreateDependencies()
    {
        List<Task> TaskList = s_dalTask!.ReadAll();
        if (TaskList != null)
        {
            Dependency d;
            Task t1;
            for (int i = 0; i < 19; i++)
            {
                t1 = TaskList[i + 1];
                d = new Dependency(0, t1.Id, TaskList[0].Id);
                s_dalDependency!.Create(d);
            }
            for (int i = 1; i < 19; i++)
            {
                t1 = TaskList[i];
                d = new Dependency(0, TaskList[19].Id, t1.Id);
                s_dalDependency!.Create(d);
            }
            for (int i = 1; i < 18; i++)
            {
                t1 = TaskList[i];
                d = new Dependency(0, TaskList[18].Id, t1.Id);
                s_dalDependency!.Create(d);
            }
        }
    }
    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        createTasks();
        createEngineers();
        CreateDependencies();
    }





}

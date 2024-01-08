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
        for (int i = 1; i <= 20; i++)
        {
            string? t_Alias = ($"TaskName{i}");
            string? t_Description = ($"This task is {i} out of 50");
            DateTime? t_CreatedAtDate = DateTime.Today;


            Task t_Task = new(0, t_Alias, t_Description, false, t_CreatedAtDate, default(DateTime), default(DateTime), default(TimeSpan), default(DateTime), default(DateTime), null, null, 0, 0);
            s_dalTask!.Create(t_Task);
        }

    }
    private static void createEngineers()
    {
        for (int i = 1; i <= 6; i++)
        {
            int t_Id;
            do
                t_Id = s_rand.Next(100000000, 999999999);
            while (s_dalEngineer!.Read(t_Id) != null);
            string? t_Email = ($"engineer{i}@minip.com");
            double? t_Cost = (double?)s_rand.Next(1000, 10000);
            string? t_Name = ($"engineer{i}");
            EngineerExperience[] enumValues = (EngineerExperience[])Enum.GetValues(typeof(EngineerExperience));
            DO.EngineerExperience t_Level = enumValues[s_rand.Next(enumValues.Length)];
            Engineer t_Engineer = new(t_Id, t_Email, t_Cost, t_Name, t_Level);
            s_dalEngineer!.Create(t_Engineer);
        }
    }
    private static void CreateDependencies()
    {
        List<Task> TaskList = s_dalTask!.ReadAll();
        for(int i=0;i<40;i++)
        {
            Dependency d;    
            if (TaskList!=null)
            { 
                Task t;
                t = TaskList[s_rand.Next(1, 20)];///choose a random task from the list
                foreach (Task task in TaskList)
                { 
                    if (t.ScheduledDate>task.DeadlineDate)
                    {
                        d=new(0,t.Id,task.Id);
                        s_dalDependency!.Create(d);
                        break;
                    }
                }
                
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

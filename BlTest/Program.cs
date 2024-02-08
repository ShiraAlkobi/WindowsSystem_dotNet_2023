using BO;
using DalApi;
using DalTest;
using DO;
using System.Diagnostics.Contracts;
using System.Linq;


internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();///getting implementation of bl level so we can use the functions 
   

    private static void Main(string[] args)
    {
        
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new BO.BlInputCheckException("try again\n");
        if (ans == "Y")
        {
            s_bl.setStatus();//set status to planStage
            DalTest.Initialization.emptyData();//empty the data
            DalTest.Initialization.Do();//initialization
        }
        try ///to catch any exceptions
        {

            int choice;
            string? s_choice;
            Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Set dates to start and end the project\n");
            s_choice = Console.ReadLine(); ///getting the user's choice
            int.TryParse(s_choice, out choice);//converting string to int
            do
            {
                ///move to the wanted entity's menu - in each one you can use the CRUD funcs
                switch (choice)
                {
                    case 1:
                        TaskMenu();
                        break;
                    case 2:
                        EngineerMenu();
                        break;
                    case 3:
                        try
                        {
                            setStartAndEndDates();
                        }
                        catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                        break;
                    case 0:
                        break;
                    default: ///wrong choice
                        Console.WriteLine("try again\n");
                        break;
                }
                ///because we are in do-while loop the first iteration will always happen, even if the choice=0 (exit)
                ///so we want to prevent another input if the first choice is 0
                if (choice != 0)
                {
                    Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Set date to start project\n");
                    s_choice = Console.ReadLine();
                    int.TryParse(s_choice, out choice);
                }
            } while (choice != 0); //until the user wants to stop
        }
        catch (Exception e) {
            if (e.InnerException != null)
                Console.WriteLine("Dal Exception:");

            Console.WriteLine(e.GetType());
            //Console.WriteLine(messege + "\n");
            Console.WriteLine(e.Message);
        }//cant know which exception is thrown 

    }

    
    /// <summary>
    /// this function will get a start date for the project from the user,
    /// then will set schedual dated for all tasks and calculate an end date,
    /// then write the start and end dates of the project in list\xml
    /// and change status to execution stage
    /// </summary>
    private static void setStartAndEndDates()
    {
        string temp;
        Console.WriteLine("enter start date for the project:\n");
        temp= Console.ReadLine();
        DateTime.TryParse(temp, out var start);
        Console.WriteLine("setting schedualed dates for each task...\n");
        s_bl.setStartAndEndDates(start);
        Console.WriteLine("end date for the project:" + s_bl.getEndDate());
    }
    /// <summary>
    /// task menu - functions for task entity
    /// </summary>
    private static void TaskMenu()
    {
        int choice;
        string? s_choice;
        Console.WriteLine
            ("enter your choice:\n" +
            "1-exit\n2-create\n3-read\n4-read all\n" +
            "5-update\n6-delete\n" +
            "8-show tasks by status order\n");
        s_choice = Console.ReadLine();
        int.TryParse(s_choice, out choice);
        do
        {

            ///according to the user's choice - call the right function
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    try
                    {
                        if (s_bl.getProjectStatus()==BO.ProjectStatus.PlanStage)//can only add task in plan stage of project
                        addTask();
                        else Console.WriteLine("Can`t create new task in execution stage of project.\n");
                    } catch (BlAlreadyExistsException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlInputCheckException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }//for dependencies
                    break;
                case 3:
                    try
                    {
                        readTask();
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 4:
                    readAllTasks();
                    break;
                case 5:
                    ///update - getting the ID, then finding its object and then sending it to the update func
                    Console.WriteLine("enter task Id to update: \n");
                    string? temp = Console.ReadLine();
                    int t_id = int.Parse(temp!);
                    try
                    {
                        updateTask(t_id);
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlCanNotUpdate e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlInputCheckException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 6:
                    try
                    {
                        deleteTask();
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlCanNotDelete e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 8:
                    {
                        groupTasks();
                    }
                    break;
                default:
                    break;
            }

            if (choice != 1) ///handling the first iteration - if the first choice is 1, no need to get another choice
            {
                Console.WriteLine("enter your choice:\n" +
                    "1-exit\n2-create\n3-read\n4-read all\n" +
                    "5-update\n6-delete\n7-update schedualed date\n" +
                    "8-show tasks by status order\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
            }

        } while (choice != 1);
    }

    /// <summary>
    /// engineer menu-functions of engineer entity
    /// </summary>
    private static void EngineerMenu()
    {
        int choice;
        string? s_choice;
        Console.WriteLine("enter your choice:\n" +
            "1-exit\n2-create\n3-read\n4-read all\n" +
            "5-update\n6-delete\n7-sort by name\n");
        s_choice = Console.ReadLine();
        int.TryParse(s_choice, out choice);
        do
        {

            ///according to the user's choice - call the right function
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    try
                    {
                        if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)//can only add engineers in plan stage of project
                            addEngineer();
                        else Console.WriteLine("Can`t add new engineer in execution stage of project.\n");
                    }
                    catch (BlAlreadyExistsException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        readEngineer();
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    };
                    break;
                case 4:
                    readAllEngineers();
                    break;
                case 5:
                    ///update - getting the ID, then finding its object and then sending it to the update func 
                    Console.WriteLine("enter engineer Id to update: \n");
                    string? temp = Console.ReadLine();
                    int t_id;
                    int.TryParse(temp!, out t_id);
                    try
                    {
                        updateEngineer(t_id);
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlInputCheckException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlCanNotUpdate e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 6:
                    try
                    {
                        deleteEngineer();
                    }
                    catch (BlDoesNotExistException e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    catch (BlCanNotDelete e) {
                        if (e.InnerException != null)
                            Console.WriteLine("Dal Exception:");

                        Console.WriteLine(e.GetType());
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 7:
                    sortEngineers();
                    break;
                default:
                    break;
            }


            if (choice != 1)///handling the first iteration - if the first choice is 1, no need to get another choice
            {
                Console.WriteLine("enter your choice:\n" +
                    "1-exit\n2-create\n3-read\n4-read all\n" +
                    "5-update\n6-delete\n7-sort by name\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
            }
        } while (choice != 1);
    }


    #region 'task menu implementations
    /// <summary>
    /// add new task
    /// note: cant set schedual date in create in any part of the project
    /// </summary>
    private static void addTask()
    {
        string? temp; ///help variable using the ReadLine func

                      ///getting the task's values from the user and inserting them into temporary variables
        Console.WriteLine("enter the task details:\n Alias: ");
        string? t_Alias = Console.ReadLine();

        Console.WriteLine("\nDescription: ");
        string? t_Description = Console.ReadLine();
        BO.Task? dependsOnTask = null;

        int t;
        List<BO.TaskInList>? t_Dependencies=new();//getting dependencies from user
        do
        {
            Console.WriteLine($"\nEnter Id of the task that {t_Alias} depends on:\n");
            temp = Console.ReadLine();
            int.TryParse(temp, out t);
            dependsOnTask = s_bl.Task.Read(t);
            BO.TaskInList taskInList = new() { Id = t, Description = dependsOnTask!.Description, Alias = dependsOnTask.Alias, Status = dependsOnTask.Status };
            t_Dependencies.Add(taskInList);
            Console.WriteLine("would you like to add another one?\n");
            temp = Console.ReadLine();
        } while (temp == "Y");

        Console.WriteLine("\nRequired effort time: ");
        temp = Console.ReadLine();
        TimeSpan t_RequiredEffortTime = TimeSpan.Parse(temp!);

        Console.WriteLine("\nDeliverables: ");
        string t_Deliverables = Console.ReadLine()!;

        Console.WriteLine("\nRemarks: ");
        string t_Remarks = Console.ReadLine()!;


        BO.Status t_status = BO.Status.Unschedualed;

        Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Complexity = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), temp);
        //creating the task to add, with the gotten values
        BO.Task task = new()
        {
            Id = 0,
            Alias = t_Alias,
            Description = t_Description,
            Dependencies = t_Dependencies,
            Status = t_status,
            CreatedAtDate = DateTime.Today,
            ScheduledDate = null,
            StartDate = null,
            ForecastDate = null,
            RequiredEffortTime = t_RequiredEffortTime,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = t_Deliverables!,
            Remarks = t_Remarks!,
            Engineer = null,
            Complexity = t_Complexity
        };
        int newID = s_bl.Task.Create(task); //calling the create func, which returns the new task's ID (identifing running number)
        Console.WriteLine("the new task is: " + s_bl!.Task.Read(newID)); //printing for the user
    }
    /// <summary>
    /// read task and print
    /// </summary>
    private static void readTask()
    {
        Console.WriteLine("Enter the ID of the task: ");
        string? temp = Console.ReadLine();///read the id
        int t_Id = int.Parse(temp!);///convert from string to int
        BO.Task t = s_bl!.Task.Read(t_Id)!;
        ///use read function of task interface to import the right task
        Console.WriteLine(t);
    }
    /// <summary>
    /// prints list of taskinlist entity
    /// according to filter- in this stage the filter will be null
    /// </summary>
    private static void readAllTasks()
    {
        List<BO.TaskInList> tasks = s_bl.Task.ReadAll().ToList<BO.TaskInList>();///import list of tasks
        foreach (BO.TaskInList t in tasks)///for each task in the list
        {
            Console.WriteLine(t);///print tasks details
        }
    }
    /// <summary>
    /// updates a task
    /// note: some fields can only be updated in plan stage and some in execution stage of the project
    /// </summary>
    /// <param name="t_id"></param>
    private static void updateTask(int t_id)
    {
        BO.Task? t = s_bl!.Task.Read(t_id);//get the task to update from database

        Console.WriteLine(t);
        string? temp, t_Alias; ///temp values for inserting in the task to update
        DateTime?  t_StartDate = null, t_DeadlineDate = null, t_CompleteDate = null;
        TimeSpan? t_RequiredEffortTime = null;

        ///getting the values from the user, then checking if they're null - no need to update, the temp variable will be the same as the one from the original object
        ///else - insert in the temp variable
        Console.WriteLine("enter the task values:\n Alias: ");
        t_Alias = Console.ReadLine();
        if (t_Alias == "" || t_Alias == null)
            t_Alias = t.Alias;

        Console.WriteLine("\n Description: ");
        string? t_Description = Console.ReadLine();
        if (t_Description == "" || t_Description == null)
            t_Description = t.Description;
        

        if (s_bl.getProjectStatus() == BO.ProjectStatus.ExecutionStage)//can only update start date in execution stage of project
        {
            Console.WriteLine("\n Start Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_StartDate = t.StartDate;
            else
                if (DateTime.TryParse(temp!, out var result))
            {
                t_StartDate = result;
            }
        }
        else
        {
            t_StartDate = t.StartDate;
        }

        if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)//can only update required effort time in plan stage of project
        {
            Console.WriteLine("\n Required effort time: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_RequiredEffortTime = t.RequiredEffortTime;
            else
                if (TimeSpan.TryParse(temp!, out var result))
            {
                t_RequiredEffortTime = result;
            }
        }
        else
        {
            t_RequiredEffortTime = t.RequiredEffortTime;
        }
        Console.WriteLine("\n Deadline Date: ");
        temp = Console.ReadLine();
        if (temp == "" || temp == null)
            t_DeadlineDate = t.DeadlineDate;
        else
            if (DateTime.TryParse(temp!, out var result))
        {
            t_DeadlineDate = result;
        }
        if (s_bl.getProjectStatus() == BO.ProjectStatus.ExecutionStage)//can only update complete date in execution stage of project
        {
            Console.WriteLine("\n Complete Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_CompleteDate = t.CompleteDate;
            else
                if (DateTime.TryParse(temp!, out var result))
            {
                t_CompleteDate = result;
            }
        }
        else { t_CompleteDate = t.CompleteDate; }

        Console.WriteLine("\n Deliverables: ");
        string? t_Deliverables = Console.ReadLine()!;
        if (t_Deliverables == "" || t_Deliverables == null)
            t_Deliverables = t.Deliverables;

        Console.WriteLine("\n Remarks: ");
        string? t_Remarks = Console.ReadLine()!;
        if (t_Remarks == "" || t_Remarks == null)
            t_Remarks = t.Remarks;
        BO.EngineerInTask t_Engineer;
        if (s_bl.getProjectStatus() == BO.ProjectStatus.ExecutionStage)
        {
            Console.WriteLine("\n Engineer ID: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_Engineer = t.Engineer;
            else
            {
                int t_engineerId = int.Parse(temp!);
                Console.WriteLine("\n Engineer name: ");
                string? t_engineerName = Console.ReadLine()!;
                t_Engineer = new() { Id = t_engineerId, Name = t_engineerName };
            }
           
        }
        else t_Engineer = t.Engineer;

        Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Complexity;
        if (temp == "" || temp == null)
            t_Complexity = t.Complexity;
        else t_Complexity = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), temp);
        
        
        ///create the task with the updated values we got
        BO.Task task = new()
        {
            Id = t.Id,
            Alias = t_Alias,
            Description = t_Description,
            Dependencies = null,
            Status = BO.Status.Unschedualed,
            CreatedAtDate = DateTime.Today,
            ScheduledDate = t.ScheduledDate,
            StartDate = t_StartDate,
            ForecastDate = null,
            RequiredEffortTime = t_RequiredEffortTime,
            DeadlineDate = t_DeadlineDate,
            CompleteDate = t_CompleteDate,
            Deliverables = t_Deliverables!,
            Remarks = t_Remarks!,
            Engineer = t_Engineer,
            Complexity = t_Complexity
        };

        s_bl!.Task.Update(task); ///call for the update func
        Console.WriteLine(s_bl!.Task.Read(task.Id)); ///print the task after updating    }
    }
    /// <summary>
    /// deleteing a task from data base
    /// </summary>
    private static void deleteTask()
    {
        Console.WriteLine("Enter the ID of the task to delete: ");
        string? temp = Console.ReadLine();
        int t_Id = int.Parse(temp!);///converting string to int
        s_bl!.Task.Delete(t_Id);///calling delete function from interface 
    }
    /// <summary>
    /// divide the tasks to groups by their status
    /// </summary>
    private static void groupTasks()
    {
        IEnumerable<IGrouping<BO.Status, BO.TaskInList>> grouped = s_bl.Task.GroupByStatus();
        foreach (var item in grouped)//printing in groups
        {
            switch (item.Key)
            {
                case BO.Status.Unschedualed:
                    Console.WriteLine("Unschedualed:\n");
                    foreach (var n in item)
                        Console.WriteLine(n);
                    Console.WriteLine("\n");
                    break;
                case BO.Status.Schedualed:
                    Console.WriteLine("Schedualed:\n");
                    foreach (var n in item)
                        Console.WriteLine(n);
                    Console.WriteLine("\n");
                    break;
                case BO.Status.OnTrack:
                    Console.WriteLine("On track:\n");
                    foreach (var n in item)
                        Console.WriteLine(n);
                    Console.WriteLine("\n");
                    break;
                case BO.Status.Done:
                    Console.WriteLine("Done:\n");
                    foreach (var n in item)
                        Console.WriteLine(n);
                    Console.WriteLine("\n");
                    break;

            }

        }
    }
    #endregion


    #region engineer menu implementations
    /// <summary>
    /// adding new engineer
    /// </summary>
    private static void addEngineer()
    {
        string? temp; ///help variable using the ReadLine func

                      ///getting the engineer's values from the user and inserting them into temporary variables
        Console.WriteLine("enter the engineer details:\n Engineer ID: ");
        temp = Console.ReadLine();
        int t_ID = int.Parse(temp!);

        Console.WriteLine("\n Email: ");
        string? t_Email = Console.ReadLine();

        Console.WriteLine("\n Cost: ");
        temp = Console.ReadLine();
        double t_Cost = double.Parse(temp!);

        Console.WriteLine("\n Name: ");
        string? t_Name = Console.ReadLine();

        Console.WriteLine("\n Level:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), temp);

       
        ///creating the engineer to add, with the gotten values
        BO.Engineer e = new() { Id = t_ID, Email = t_Email, Cost = t_Cost, Name = t_Name, Level = t_Level };
        s_bl!.Engineer.Create(e);///calling the create func    
    }
    /// <summary>
    /// getting engineer's details and printing
    /// </summary>
    private static void readEngineer()
    {
        Console.WriteLine("Enter the ID of the engineer: ");
        string? temp = Console.ReadLine();///reading to nullable string
        int t_Id = int.Parse(temp!);///converting to int
        BO.Engineer e = s_bl!.Engineer.Read(t_Id)!;
        ///using read function of interface to import the right engineer
        Console.WriteLine(e);
    }
    /// <summary>
    /// printing list of engineers accordong to filter
    /// in this stage the filter will be null
    /// </summary>
    private static void readAllEngineers()
    {
        
        var engineers = s_bl!.Engineer.ReadAll();///import new list (IEnumerable type)
        foreach (BO.Engineer e in engineers)///for each engineer int the list
        {
            Console.WriteLine(e);///print
        }
    }
    /// <summary>
    /// udatign engineer details
    /// </summary>
    /// <param name="t_id"></param>
    private static void updateEngineer(int t_id)
    {
        BO.Engineer? t = s_bl!.Engineer.Read(t_id);
        Console.WriteLine(t);
        string? temp, t_Email, t_Name;///temp values for inserting in the engineer to update
        double t_Cost = 0;
        int t_Id;
        t_Id = t.Id;

        ///getting the values from the user, then checking if they're null - no need to update, the temp variable will be the same as the one from the original object
        ///else - insert in the temp variable
        Console.WriteLine("\n Email: ");
        t_Email = Console.ReadLine();
        if (t_Email == "" || t_Email == null)
            t_Email = t.Email;

        Console.WriteLine("\n Cost: ");
        temp = Console.ReadLine();
        if (double.TryParse(temp!, out var help))
            t_Cost = help;
        if (temp == "" || temp == null)
            t_Cost = t.Cost;

        Console.WriteLine("\n Name: ");
        t_Name = Console.ReadLine();
        if (t_Name == "" || t_Name == null)
            t_Name = t.Name;


        Console.WriteLine("Level:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Level = 0;

        if (Enum.TryParse(temp, out BO.EngineerExperience help2))
            t_Level = help2;
        if (temp == "" || temp == null)
            t_Level = (BO.EngineerExperience)t.Level;
        int i;
        BO.Task? task = null;
        BO.TaskInEngineer? taskInEngineer = null;
        if (s_bl.getProjectStatus() == BO.ProjectStatus.ExecutionStage)//can only assign task to engineer in execution stage of project
        {
            Console.WriteLine("\nassigned task id:");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                taskInEngineer = t.Task;
            else
            {
                int.TryParse(temp, out i);
                task = s_bl.Task.Read(i);//finding the task for the alias
                taskInEngineer = new()
                {
                    Id = i,
                    Alias = task!.Alias
                };
            }
        }
        ///create the engineer with the updated values we got
        BO.Engineer p = new() { Id = t_Id, Email = t_Email, Cost = t_Cost, Name = t_Name, Level = t_Level,Task=taskInEngineer };
        s_bl!.Engineer.Update(p);///call for the update func
        Console.WriteLine(p);///print the engineer after updating  
    }
    /// <summary>
    /// deleting an engineer from the database
    /// </summary>
    private static void deleteEngineer()
    {
        Console.WriteLine("Enter the ID of the engineer to delete: ");
        string? temp = Console.ReadLine();
        int t_Id = int.Parse(temp!);///convert to int
        s_bl!.Engineer.Delete(t_Id);///delete using interface function
    }
    /// <summary>
    /// print engineers in order of the names
    /// </summary>
    private static void sortEngineers()
    {
        foreach(var item in s_bl.Engineer.SortByName())
        {
            Console.WriteLine(item);
        }
    }
    #endregion

    
   


}
using BO;
using DalApi;
using DalTest;
using DO;
using System.Linq;


internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
        {
            DalTest.Initialization.emptyData();
            DalTest.Initialization.Do();
        }
        try ///to catch any exceptions
        {

            int choice;
            string? s_choice;
            Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n");
            s_choice = Console.ReadLine(); ///getting the user's choice
            int.TryParse(s_choice, out choice);
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
                    Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n");
                    s_choice = Console.ReadLine();
                    int.TryParse(s_choice, out choice);
                }
            } while (choice != 0); //until the user wants to stop
        }
        catch (Exception e) { Console.WriteLine(e); }//cant know which exception is thrown 

    }

    private static void TaskMenu()
    {
        int choice;
        string? s_choice;
        Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n7-update scedualed date\n");
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
                        addTask();
                    } catch (BlAlreadyExistsException e) { Console.WriteLine(e); }
                    catch (BlInputCheckException e) { Console.WriteLine(e); }
                    break;
                case 3:
                    try
                    {
                        readTask();
                    }
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
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
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                    catch (BlCanNotUpdate e) { Console.WriteLine(e); }
                    catch (BlInputCheckException e) { Console.WriteLine(e); }
                    break;
                case 6:
                    try
                    {
                        deleteTask();
                    }
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                    catch (BlCanNotDelete e) { Console.WriteLine(e); }
                    break;
                case 7:
                    try
                    {
                        updateScedualedDateTask();
                    }
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                    catch (BlCanNotUpdate e) { Console.WriteLine(e); }
                    break;
                default:
                    break;
            }

            if (choice != 1) ///handling the first iteration - if the first choice is 1, no need to get another choice
            {
                Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
            }

        } while (choice != 1);
    }

    private static void EngineerMenu()
    {
        int choice;
        string? s_choice;
        Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
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
                        addEngineer();
                    }
                    catch (BlAlreadyExistsException e) { Console.WriteLine(e); }
                    break;
                case 3:
                    try
                    {
                        readEngineer();
                    }catch(BlDoesNotExistException e) { Console.WriteLine(e); };
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
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                    catch (BlInputCheckException e) { Console.WriteLine(e); }
                    catch (BlCanNotUpdate e) { Console.WriteLine(e); }
                    break;
                case 6:
                    try
                    {
                        deleteEngineer();
                    }
                    catch (BlDoesNotExistException e) { Console.WriteLine(e); }
                    catch (BlCanNotDelete e) { Console.WriteLine(e); }
                    break;
                default:
                    break;
            }


            if (choice != 1)///handling the first iteration - if the first choice is 1, no need to get another choice
            {
                Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
            }
        } while (choice != 1);
    }
    #region engineer menu implementations
    private static void deleteEngineer()
    {
        Console.WriteLine("Enter the ID of the engineer to delete: ");
        string? temp = Console.ReadLine();
        int t_Id = int.Parse(temp!);///convert to int
        s_bl!.Engineer.Delete(t_Id);///delete using interface function
    }

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

        ///create the engineer with the updated values we got
        BO.Engineer p = new() { Id = t_Id, Email = t_Email, Cost = t_Cost, Name = t_Name, Level = t_Level };
        s_bl!.Engineer.Update(p);///call for the update func
        Console.WriteLine(p);///print the engineer after updating  
    }

    private static void readAllEngineers()
    {
        //TODO: filter?
        var engineers = s_bl!.Engineer.ReadAll();///import new list (IEnumerable type)
        foreach (BO.Engineer e in engineers)///for each engineer int the list
        {
            Console.WriteLine(e);///print
        }
    }

    private static void readEngineer()
    {
        Console.WriteLine("Enter the ID of the engineer: ");
        string? temp = Console.ReadLine();///reading to nullable string
        int t_Id = int.Parse(temp!);///converting to int
        BO.Engineer e = s_bl!.Engineer.Read(t_Id)!;///using read function of interface to import the right engineer
        
    }

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
        BO.Engineer e = new() { Id = t_ID, Email=t_Email,Cost= t_Cost,Name= t_Name,Level=t_Level };
        s_bl!.Engineer.Create(e);///calling the create func    
    }
    #endregion


    #region 'task menu implementations
    private static void addTask()
    {
        string? temp; ///help variable using the ReadLine func

                      ///getting the task's values from the user and inserting them into temporary variables
        Console.WriteLine("enter the task details:\n Alias: ");
        string? t_Alias = Console.ReadLine();

        Console.WriteLine("\n Description: ");
        string? t_Description = Console.ReadLine();
        //TODO: לבדוק אם צריך לקלוט פה
        //Console.WriteLine("\n Description: ");
        //List<BO.TaskInList>? t_Dependencies = Console.ReadLine();

        Console.WriteLine("\n Scheduled Date: ");
        temp = Console.ReadLine();
        DateTime t_ScheduledDate = DateTime.Parse(temp!);

        Console.WriteLine("\n Start Date: ");
        temp = Console.ReadLine();
        DateTime t_StartDate = DateTime.Parse(temp!);

        Console.WriteLine("\n Required effort time: ");
        temp = Console.ReadLine();
        TimeSpan t_RequiredEffortTime = TimeSpan.Parse(temp!);

        Console.WriteLine("\n Deadline Date: ");
        temp = Console.ReadLine();
        DateTime t_DeadlineDate = DateTime.Parse(temp!);

        Console.WriteLine("\n Complete Date: ");
        temp = Console.ReadLine();
        DateTime t_CompleteDate = DateTime.Parse(temp!);

        Console.WriteLine("\n Deliverables: ");
        string t_Deliverables = Console.ReadLine()!;

        Console.WriteLine("\n Remarks: ");
        string t_Remarks = Console.ReadLine()!;

        Console.WriteLine("\n Engineer ID: ");
        temp = Console.ReadLine();
        int t_engineerId = int.Parse(temp!);
        Console.WriteLine("\n Engineer name: ");
        string? t_engineerName = Console.ReadLine()!;
        BO.EngineerInTask t_Engineer = new() { Id = t_engineerId, Name = t_engineerName };

        BO.Status t_status = BO.Status.Unscheduled;

        //TODO: מהנדס במשימה? איזה שלב מוסיפים?
        Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Complexity = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), temp);
        //creating the task to add, with the gotten values
        BO.Task t = new() { Id = 0, Alias = t_Alias, Description = t_Description, Dependencies = null, Status = t_status, CreatedAtDate = DateTime.Today, ScheduledDate = t_ScheduledDate, StartDate = t_StartDate, ForecastDate = null, RequiredEffortTime = t_RequiredEffortTime,
            DeadlineDate = t_DeadlineDate, CompleteDate = t_CompleteDate, Deliverables = t_Deliverables!, Remarks = t_Remarks!, Engineer = t_Engineer, Complexity = t_Complexity };
        int newID = s_bl.Task.Create(t); //calling the create func, which returns the new task's ID (identifing running number)
        Console.WriteLine("the new task is: " + s_bl!.Task.Read(newID)); //printing for the user
    }

    private static void readTask()
    {
        Console.WriteLine("Enter the ID of the task: ");
        string? temp = Console.ReadLine();///read the id
        int t_Id = int.Parse(temp!);///convert from string to int
        BO.Task t = s_bl!.Task.Read(t_Id)!;///use read function of task interface to import the right task
    }

    private static void readAllTasks()
    {
        List<BO.TaskInList> tasks = s_bl.Task.ReadAll().ToList<BO.TaskInList>();///import list of tasks
        foreach (BO.TaskInList t in tasks)///for each task in the list
        {
            Console.WriteLine(t);///print tasks details
        }
    }

    private static void updateTask(int t_id)
    {
        BO.Task? t = s_bl!.Task.Read(t_id);

        Console.WriteLine(t);
        string? temp, t_Alias; ///temp values for inserting in the task to update
        DateTime? t_ScheduledDate = null, t_StartDate = null, t_DeadlineDate = null, t_CompleteDate = null;
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

        Console.WriteLine("\n Scheduled Date: ");
        temp = Console.ReadLine();
        if (temp == "" || temp == null)
            t_ScheduledDate = t.ScheduledDate;
        else
            if (DateTime.TryParse(temp!, out var result))
        {
            t_ScheduledDate = result;
        }

        Console.WriteLine("\n Start Date: ");
        temp = Console.ReadLine();
        if (temp == "" || temp == null)
            t_StartDate = t.StartDate;
        else
            if (DateTime.TryParse(temp!, out var result))
        {
            t_StartDate = result;
        }


        Console.WriteLine("\n Required effort time: ");
        temp = Console.ReadLine();
        if (temp == "" || temp == null)
            t_RequiredEffortTime = t.RequiredEffortTime;
        else
            if (TimeSpan.TryParse(temp!, out var result))
        {
            t_RequiredEffortTime = result;
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

        Console.WriteLine("\n Complete Date: ");
        temp = Console.ReadLine();
        if (temp == "" || temp == null)
            t_CompleteDate = t.CompleteDate;
        else
            if (DateTime.TryParse(temp!, out var result))
        {
            t_CompleteDate = result;
        }

        Console.WriteLine("\n Deliverables: ");
        string? t_Deliverables = Console.ReadLine()!;
        if (t_Deliverables == "" || t_Deliverables == null)
            t_Deliverables = t.Deliverables;

        Console.WriteLine("\n Remarks: ");
        string? t_Remarks = Console.ReadLine()!;
        if (t_Remarks == "" || t_Remarks == null)
            t_Remarks = t.Remarks;

        Console.WriteLine("\n Engineer ID: ");
        temp = Console.ReadLine();
        int t_engineerId = int.Parse(temp!);
        Console.WriteLine("\n Engineer name: ");
        string? t_engineerName = Console.ReadLine()!;
        BO.EngineerInTask t_Engineer = new() { Id = t_engineerId, Name = t_engineerName };

        Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
        temp = Console.ReadLine();
        BO.EngineerExperience t_Complexity = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), temp);
        if (temp == "" || temp == null)
            t_Complexity = t.Complexity;
        ///create the task with the updated values we got
        BO.Task task = new()
        {
            Id = 0,
            Alias = t_Alias,
            Description = t_Description,
            Dependencies = null,
            Status = BO.Status.Unscheduled,//TODO:מה ההגיון לעדכן את זה אם גם ככה לא שומרים?
            CreatedAtDate = DateTime.Today,
            ScheduledDate = t_ScheduledDate,
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

    private static void deleteTask()
    {
        Console.WriteLine("Enter the ID of the task to delete: ");
        string? temp = Console.ReadLine();
        int t_Id = int.Parse(temp!);///converting string to int
        s_bl!.Task.Delete(t_Id);///calling delete function from interface 
    }

    private static void updateScedualedDateTask()
    {
        string? temp;
        Console.WriteLine("enter task Id to update:\n");
        temp = Console.ReadLine();
        int.TryParse(temp!, out var t_id);
        Console.WriteLine("enter the new date to update:\n");
        temp = Console.ReadLine();
        DateTime.TryParse(temp, out var t_date);
        s_bl.Task.UpdateScedualedDate(t_id, t_date);
        Console.WriteLine(s_bl.Task.Read(t_id));
    }
    #endregion


}
namespace DalTest  
{
    using System.Threading.Channels;
    using Dal;
    using DalApi;
    using DO;
    /// <summary>
    /// in this file we create the 'main' which manages all of the entities data, performs all of the CRUD funcs on each entity's datasource
    /// </summary>
    internal class Program
    {
        //So we can use the CRUD funcs
        private static ITask? s_dalTask = new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            try //to catch any exceptions
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency); //initializing each entity's data source
                int choice;
                string? s_choice;
                Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Dependency\n");
                s_choice = Console.ReadLine(); //getting the user's choice
                int.TryParse(s_choice, out choice); 
                do
                {
                    //move to the wanted entity's menu - in each one you can use the CRUD funcs
                    switch (choice)
                    {
                        case 1:
                            TaskMenu(); 
                            break;
                        case 2:
                            EngineerMenu();
                            break;
                        case 3:
                            DependencyMenu();
                            break;
                        case 0:
                            break;
                        default: //wrong choice
                            Console.WriteLine("try again\n");
                            break;
                    }
                    //because we are in do-while loop the first iteration will always happen, even if the choice=0 (exit)
                    //so we want to prevent another input if the first choice is 0
                    if (choice != 0) 
                    {
                        Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Dependency\n");
                        s_choice = Console.ReadLine();
                        int.TryParse(s_choice, out choice);
                    }
                } while (choice != 0); //until the user wants to stop
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        /// <summary>
        /// The 3 entities menues 
        /// for performing the CRUD funcs according to the user's choice
        /// </summary>
        private static void TaskMenu()
        {
            int choice;
            string? s_choice;
            Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
            s_choice = Console.ReadLine();
            int.TryParse(s_choice, out choice);
            do
            {
                try
                {
                    //according to the user's choice - call the right function
                    switch (choice)
                    {
                        case 1:
                            break;
                        case 2:
                            addTask();
                            break;
                        case 3:
                            readTask();
                            break;
                        case 4:
                            readAllTasks();
                            break;
                        case 5: 
                            //update - getting the ID, then finding its object and then sending it to the update func
                            Console.WriteLine("enter task Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id = int.Parse(temp!);
                            try
                            {
                                Task? t = s_dalTask!.Read(t_id);
                                if (t != null)//preventing dealing with null in the update
                                {
                                    Console.WriteLine(t); //print before updating
                                    updateTask(t);
                                }
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                            break;
                        case 6:
                            deleteTask();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (choice != 1) //handling the first iteration - if the first choice is 1, no need to get another choice
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
                try
                {
                    //according to the user's choice - call the right function
                    switch (choice)
                    {
                        case 1:
                            break;
                        case 2:
                            addEngineer();
                            break;
                        case 3:
                            readEngineer();
                            break;
                        case 4:
                            readAllEngineers();
                            break;
                        case 5:
                            //update - getting the ID, then finding its object and then sending it to the update func 
                            Console.WriteLine("enter engineer Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id;
                            int.TryParse(temp!,out t_id);
                            try
                            {
                                Engineer? t = s_dalEngineer!.Read(t_id);
                                if (t != null) //preventing dealing with null in the update
                                {
                                    Console.WriteLine(t); //print before updating
                                    updateEngineer(t);
                                }
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                            break;
                        case 6:
                            deleteEngineer();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
                if (choice != 1)//handling the first iteration - if the first choice is 1, no need to get another choice
                {
                    Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
                    s_choice = Console.ReadLine();
                    int.TryParse(s_choice, out choice);
                }
            } while (choice != 1);
        }
        private static void DependencyMenu()
        {
            int choice;
            string? s_choice;
            Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
            s_choice = Console.ReadLine();
            int.TryParse(s_choice, out choice);
            do
            {
                try
                {
                    switch (choice)
                    {
                        //according to the user's choice - call the right function
                        case 1:
                            break;
                        case 2:
                            addDependency();
                            break;
                        case 3:
                            readDependency();
                            break;
                        case 4:
                            readAllDependencies();
                            break;
                        case 5:
                            //update - getting the ID, then finding its object and then sending it to the update func
                            Console.WriteLine("enter task Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id = int.Parse(temp!);
                            try
                            {
                                Dependency? t = s_dalDependency!.Read(t_id);
                                if (t != null)//preventing dealing with null in the update
                                {
                                    Console.WriteLine(t); //print before updating
                                    updateDependency(t);
                                }
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                            break;
                        case 6:
                            deleteDependency();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (choice != 1)//handling the first iteration - if the first choice is 1, no need to get another choice
                {
                    Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
                    s_choice = Console.ReadLine();
                    int.TryParse(s_choice, out choice);
                }
            } while (choice != 1);
        }

        /// <summary>
        /// The functions which manage the entities datasource using CRUD funcs 
        /// </summary>
        
        ///</summary>
        ///adds a task
        ///</summary>
        private static void addTask()
        {
            string? temp; //help variable using the ReadLine func

            //getting the task's values from the user and inserting them into temporary variables
            Console.WriteLine("enter the task details:\n Alias: ");
            string? t_Alias = Console.ReadLine();

            Console.WriteLine("\n Description: ");
            string? t_Description = Console.ReadLine();

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
            int t_EngineerID = int.Parse(temp!);

            Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
            temp = Console.ReadLine();
            DO.EngineerExperience t_Complexity = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
            //creating the task to add, with the gotten values
            Task t = new(0, t_Alias, t_Description, false, DateTime.Today, t_ScheduledDate, t_StartDate, t_RequiredEffortTime,
               t_DeadlineDate, t_CompleteDate, t_Deliverables!, t_Remarks!, t_EngineerID, t_Complexity);
            int newID = s_dalTask!.Create(t); //calling the create func, which returns the new task's ID (identifing running number)
            Console.WriteLine("the new task is: " + s_dalTask.Read(newID)); //printing for the user

        }
        /// <summary>
        /// prints the task with the input id
        /// </summary>
        private static void readTask()
        {
            Console.WriteLine("Enter the ID of the task: ");
            string? temp = Console.ReadLine();//read the id
            int t_Id = int.Parse(temp!);//convert from string to int
            Task t = s_dalTask!.Read(t_Id)!;//use read function of task interface to import the right task
            if (t != null)//if the task exists
            {
                Console.WriteLine(t);//print
            }
            else { Console.WriteLine("ID was not found"); }
        }
        /// <summary>
        /// prints all tasks
        /// </summary>
        private static void readAllTasks()
        {
            List<Task> tasks = s_dalTask!.ReadAll();//import list of tasks
            foreach (Task t in tasks)//for each task in the list
            {
                Console.WriteLine(t);//print tasks details
            }
        }
        /// <summary>
        /// update a task (gotten as a parameter
        /// </summary>
        /// <param name="t"></param>
        private static void updateTask(Task t)
        {
            string? temp, t_alias; //temp values for inserting in the task to update
            DateTime t_ScheduledDate, t_StartDate, t_DeadlineDate, t_CompleteDate;
            TimeSpan t_RequiredEffortTime;
            
            //getting the values from the user, then checking if they're null - no need to update, the temp variable will be the same as the one from the original object
            //else - insert in the temp variable
            Console.WriteLine("enter the task values:\n Alias: ");
            t_alias = Console.ReadLine();
            if (t_alias == "" || t_alias == null)
                t_alias = t.Alias;
            
            Console.WriteLine("\n Description: ");
            string? t_Description = Console.ReadLine();
            if (t_Description == "" || t_Description == null)
                t_Description = t.Description;

            Console.WriteLine("\n Scheduled Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_ScheduledDate = t.ScheduledDate;
            else
                DateTime.TryParse(temp!, out t_ScheduledDate);

            Console.WriteLine("\n Start Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_StartDate = t.StartDate;
            else
                DateTime.TryParse(temp!, out t_StartDate);

            Console.WriteLine("\n Required effort time: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_RequiredEffortTime = t.RequiredEffortTime;
            else
                TimeSpan.TryParse(temp!, out t_RequiredEffortTime);

            Console.WriteLine("\n Deadline Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_DeadlineDate = t.DeadlineDate;
            else
                DateTime.TryParse(temp!, out t_DeadlineDate);

            Console.WriteLine("\n Complete Date: ");
            temp = Console.ReadLine();
            if (temp == "" || temp == null)
                t_CompleteDate = t.CompleteDate;
            else
                DateTime.TryParse(temp!, out t_CompleteDate);

            Console.WriteLine("\n Deliverables: ");
            string t_Deliverables = Console.ReadLine()!;
            if (t_Deliverables == "" || t_Deliverables == null)
                t_Deliverables = t.Deliverables;

            Console.WriteLine("\n Remarks: ");
            string t_Remarks = Console.ReadLine()!;
            if (t_Remarks == "" || t_Remarks == null)
                t_Remarks = t.Remarks;
            int t_EngineerID;
            Console.WriteLine("\n Engineer ID: ");
            temp = Console.ReadLine();
            int.TryParse(temp!, out t_EngineerID);
            if (temp == "" || temp == null)
                t_EngineerID = t.EngineerId;

            Console.WriteLine("Complexity:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
            temp = Console.ReadLine();
            DO.EngineerExperience t_Complexity = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
            if (temp == "" || temp == null)
                t_Complexity = t.Complexity;
            //create the task with the updated values we got
            Task p = new(t.Id, t_alias, t_Description, false, DateTime.Today, t_ScheduledDate, t_StartDate, t_RequiredEffortTime,
                t_DeadlineDate, t_CompleteDate, t_Deliverables!, t_Remarks!, t_EngineerID, t_Complexity);
            s_dalTask!.Update(p); //call for the update func
            Console.WriteLine(p); //print the task after updating
        }
        /// <summary>
        /// deleting task from the list
        /// </summary>
        private static void deleteTask()
        {
            Console.WriteLine("Enter the ID of the task to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);//converting string to int
            try
            {
                s_dalTask!.Delete(t_Id);//calling delete function from interface
            }
            catch (Exception ex) { Console.WriteLine(ex); }//throe exception
        }

        /// <summary>
        /// adds an engineer
        /// </summary>
        private static void addEngineer()
        {
            string? temp; //help variable using the ReadLine func

            //getting the engineer's values from the user and inserting them into temporary variables
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
            DO.EngineerExperience t_Level = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
            //creating the engineer to add, with the gotten values
            Engineer e = new(t_ID, t_Email, t_Cost, t_Name, t_Level);
            s_dalEngineer!.Create(e);//calling the create func
        }
        /// <summary>
        /// prints the engineer with the input id
        /// </summary>
        private static void readEngineer()
        {
            Console.WriteLine("Enter the ID of the engineer: ");
            string? temp = Console.ReadLine();//reading to nullable string
            int t_Id = int.Parse(temp!);//converting to int
            Engineer e = s_dalEngineer!.Read(t_Id)!;//using read function of interface to import the right engineer
            if (e != null)
            {
                Console.WriteLine(e);//print details
            }
            else { Console.WriteLine("ID was not found"); }
        }
        /// <summary>
        /// prints all engineers
        /// </summary>
        private static void readAllEngineers()
        {
            List<Engineer> engineers = s_dalEngineer!.ReadAll();//import list
            foreach (Engineer e in engineers)//for each engineer int the list
            {
                Console.WriteLine(e);//print
            }
        }
        /// <summary>
        /// updates an engineer (gotten as parameter)
        /// </summary>
        /// <param name="t"></param>
        private static void updateEngineer(Engineer t)
        {
            string? temp, t_Email, t_Name;//temp values for inserting in the engineer to update
            double? t_Cost = 0;
            int t_Id;
            t_Id = t.Id;

            //getting the values from the user, then checking if they're null - no need to update, the temp variable will be the same as the one from the original object
            //else - insert in the temp variable
            Console.WriteLine("\n Email: ");
            t_Email = Console.ReadLine();
            if (t_Email == "" || t_Email == null)
                t_Email = t.Email;

            Console.WriteLine("\n Cost: ");
            temp = Console.ReadLine();
            if(double.TryParse(temp!,out var help))
                t_Cost = help;
            if (temp == "" || temp == null)
                t_Cost = t.Cost;

            Console.WriteLine("\n Name: ");
            t_Name = Console.ReadLine();
            if (t_Name == "" || t_Name == null)
                t_Name = t.Name;


            Console.WriteLine("Level:\n0 - Beginner\n1 - AdvancedBeginner\n2 - Intermediate\n3 - Advanced\n4 - Expert\n");
            temp = Console.ReadLine();
            DO.EngineerExperience? t_Level = null;
            if(Enum.TryParse(temp,out EngineerExperience help2))
                t_Level= help2;
            if (temp == "" || temp == null)
                t_Level = t.Level;
            //create the engineer with the updated values we got
            Engineer p = new(t_Id, t_Email, t_Cost, t_Name, t_Level);
            s_dalEngineer!.Update(p);//call for the update func
            Console.WriteLine(p);//print the engineer after updating
        }
        /// <summary>
        /// delete engineer from list
        /// </summary>
        private static void deleteEngineer()
        {
            Console.WriteLine("Enter the ID of the engineer to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);//convert to int
            try
            {
                s_dalEngineer!.Delete(t_Id);//delete using interface function
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }


        /// <summary>
        /// adds a dependency
        /// </summary>
        private static void addDependency()
        {
            string? temp;//help variable using the ReadLine func

            //getting the dependency's values from the user and inserting them into temporary variables
            Console.WriteLine("enter the dependency details:\n The ID of dependent task: ");
            temp = Console.ReadLine();
            int t_Dependent = int.Parse(temp!);

            Console.WriteLine("\n Depends on (ID): ");
            temp = Console.ReadLine();
            int t_DependsOn = int.Parse(temp!);
            //creating the dependency to add, with the gotten values
            Dependency d = new(0, t_Dependent, t_DependsOn);
            s_dalDependency!.Create(d);//calling the create func, which returns the new dependency's ID (identifing running number)
            int newID = s_dalDependency!.Create(d);//calling the create func, which returns the new task's ID (identifing runnung number)
            Console.WriteLine("the new dependency is: " + s_dalDependency.Read(newID));
        }
        /// <summary>
        /// print dependency with input id
        /// </summary>
        private static void readDependency()
        {
            Console.WriteLine("Enter the ID of the dependency: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);//converting to int
            Dependency d = s_dalDependency!.Read(t_Id)!;//importing the right dependency
            if (d != null)
            {
                Task dependent = s_dalTask!.Read(d.DependentTask)!;//find the dependent task
                Task dependsOn = s_dalTask!.Read(d.DependsOnTask)!;//find the task the is depended on
                //print id of dependency,descriptions of dependent an depends ov task
                Console.WriteLine("Dependency { Id = "+d.Id+", DependentTask = "+dependent.Description+", DependsOnTask = "+dependsOn.Description +"}");
            }
            else { Console.WriteLine("ID was not found"); }
        }
        /// <summary>
        /// print all of the dependencies
        /// </summary>
        private static void readAllDependencies()
        {
            List<Dependency> dependencies = s_dalDependency!.ReadAll();//import list
            foreach (Dependency d in dependencies)
            {
                Task dependent = s_dalTask!.Read(d.DependentTask)!;//find the dependent task
                Task dependsOn = s_dalTask!.Read(d.DependsOnTask)!;//find the task the is depended on
                //print id of dependency,descriptions of dependent an depends ov task
                Console.WriteLine("Dependency { Id = " + d.Id + ", DependentTask = " + dependent.Description + ", DependsOnTask = " + dependsOn.Description + "}");
            }
        }

        /// <summary>
        /// updates a engineer (gotten as parameter)
        /// </summary>
        /// <param name="t"></param>
        private static void updateDependency(Dependency t)
        {
            string? temp;//temp values for inserting in the dependency to update
            int t_Dependent=0, t_DependsOn=0;

            //getting the values from the user, then checking if they're null - no need to update, the temp variable will be the same as the one from the original object
            //else - insert in the temp variable
            Console.WriteLine("enter the engineer values:\n The dependent task ID: ");
            temp = Console.ReadLine();
            if(int.TryParse(temp!,out var help))
                t_Dependent = help;
            if (temp == "" || temp == null)
                t_Dependent = t.DependentTask;

            Console.WriteLine("\n depends on task ID: ");
            temp = Console.ReadLine();
            if(int.TryParse(temp!,out var help2))
                t_DependsOn = help2;          
            if (temp == "" || temp == null)
                t_DependsOn = t.DependsOnTask;

            //create the dependency with the updated values we got
            Dependency p = new(t.Id, t_Dependent, t_DependsOn);
            s_dalDependency!.Update(p);//call for the update func
            //print the dependency after updating
            Task dependent = s_dalTask!.Read(p.DependentTask)!;
            Task dependsOn = s_dalTask!.Read(p.DependsOnTask)!;
            Console.WriteLine("Dependency { Id = " + p.Id + ", DependentTask = " + dependent.Description + ", DependsOnTask = " + dependsOn.Description + "}");
        }
        /// <summary>
        /// delete a dependency by id
        /// </summary>
        private static void deleteDependency()
        {
            Console.WriteLine("Enter the ID of the dependency to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);//convert from string to int
            try ///delete can throw exceptions
            {
                s_dalDependency!.Delete(t_Id);//delete using interface function
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}
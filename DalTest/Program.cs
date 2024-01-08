namespace DalTest
{
    using System.Threading.Channels;
    using Dal;
    using DalApi;
    using DO;
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        private static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
                int choice;
                string? s_choice;
                Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Dependency\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
                do
                {

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
                        default:
                            Console.WriteLine("try again\n");
                            break;
                    }
                    if (choice != 0)
                    {
                        Console.WriteLine("enter your entity choice:\n0-exit\n1-Task\n2-Engineer\n3-Dependency\n");
                        s_choice = Console.ReadLine();
                        int.TryParse(s_choice, out choice);
                    }
                } while (choice != 0);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
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
                            Console.WriteLine("enter task Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id = int.Parse(temp!);
                            try
                            {
                                Task? t = s_dalTask!.Read(t_id);
                                if (t != null)
                                {
                                    Console.WriteLine(t);
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
                if (choice != 1) ///handling the first iteration
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
                            Console.WriteLine("enter engineer Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id;
                            int.TryParse(temp!,out t_id);
                            try
                            {
                                Engineer? t = s_dalEngineer!.Read(t_id);
                                if (t != null)
                                {
                                    Console.WriteLine(t);
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
                if (choice != 1)
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
                            Console.WriteLine("enter task Id to update: \n");
                            string? temp = Console.ReadLine();
                            int t_id = int.Parse(temp!);
                            try
                            {
                                Dependency? t = s_dalDependency!.Read(t_id);
                                if (t != null)
                                {
                                    Console.WriteLine(t);
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
                Console.WriteLine("enter your choice:\n1-exit\n2-create\n3-read\n4-read all\n5-update\n6-delete\n");
                s_choice = Console.ReadLine();
                int.TryParse(s_choice, out choice);
            } while (choice != 1);
        }
        private static void addTask()
        {
            string? temp;

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
            Task t = new(0, t_Alias, t_Description, false, DateTime.Today, t_ScheduledDate, t_StartDate, t_RequiredEffortTime,
               t_DeadlineDate, t_CompleteDate, t_Deliverables!, t_Remarks!, t_EngineerID, t_Complexity);
            int newID = s_dalTask!.Create(t);
            Console.WriteLine("the new task is: " + s_dalTask.Read(newID));

        }
        private static void readTask()
        {
            Console.WriteLine("Enter the ID of the task: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);
            Task t = s_dalTask!.Read(t_Id)!;
            if (t != null)
            {
                Console.WriteLine(t);
            }
            else { Console.WriteLine("ID was not found"); }
        }
        private static void readAllTasks()
        {
            List<Task> tasks = s_dalTask!.ReadAll();
            foreach (Task t in tasks)
            {
                Console.WriteLine(t);
            }
        }
        /// <summary>
        /// updating function
        /// </summary>
        /// <param name="t"></param>
        private static void updateTask(Task t)
        {
            string? temp, t_alias;
            DateTime t_ScheduledDate, t_StartDate, t_DeadlineDate, t_CompleteDate;
            TimeSpan t_RequiredEffortTime;
            ///enter alias and check if needs to be changed
            Console.WriteLine("enter the task values:\n Alias: ");
            t_alias = Console.ReadLine();
            if (t_alias == "" || t_alias == null)
                t_alias = t.Alias;
            ///enter description etc...
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
            Task p = new(t.Id, t_alias, t_Description, false, DateTime.Today, t_ScheduledDate, t_StartDate, t_RequiredEffortTime,
                t_DeadlineDate, t_CompleteDate, t_Deliverables!, t_Remarks!, t_EngineerID, t_Complexity);
            s_dalTask!.Update(p);
            Console.WriteLine(p);
        }
        private static void deleteTask()
        {
            Console.WriteLine("Enter the ID of the task to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);
            try
            {
                s_dalTask!.Delete(t_Id);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }




        private static void addEngineer()
        {
            string? temp;
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
            Engineer e = new(t_ID, t_Email, t_Cost, t_Name, t_Level);
            s_dalEngineer!.Create(e);
        }
        private static void readEngineer()
        {
            Console.WriteLine("Enter the ID of the engineer: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);
            Engineer e = s_dalEngineer!.Read(t_Id)!;
            if (e != null)
            {
                Console.WriteLine(e);
            }
            else { Console.WriteLine("ID was not found"); }
        }
        private static void readAllEngineers()
        {
            List<Engineer> engineers = s_dalEngineer!.ReadAll();
            foreach (Engineer e in engineers)
            {
                Console.WriteLine(e);
            }
        }

        private static void updateEngineer(Engineer t)
        {
            string? temp, t_Email, t_Name;
            double? t_Cost = 0;
            int t_Id;
            t_Id = t.Id;

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
            DO.EngineerExperience? t_Level=null;
            if(Enum.TryParse(temp,out EngineerExperience help2))
                t_Level= help2;
            if (temp == "" || temp == null)
                t_Level = t.Level;
            Engineer p = new(t_Id, t_Email, t_Cost, t_Name, t_Level);
            s_dalEngineer!.Update(p);
            Console.WriteLine(p);
        }
        private static void deleteEngineer()
        {
            Console.WriteLine("Enter the ID of the engineer to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);
            try
            {
                s_dalEngineer!.Delete(t_Id);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }



        private static void addDependency()
        {
            string? temp;
            Console.WriteLine("enter the dependency details:\n The ID of dependent task: ");
            temp = Console.ReadLine();
            int t_Dependent = int.Parse(temp!);

            Console.WriteLine("\n Depends on (ID): ");
            temp = Console.ReadLine();
            int t_DependsOn = int.Parse(temp!);

            Dependency d = new(0, t_Dependent, t_DependsOn);
            s_dalDependency!.Create(d);
            int newID = s_dalDependency!.Create(d);
            Console.WriteLine("the new dependency is: " + s_dalDependency.Read(newID));
        }

        private static void readDependency()
        {
            Console.WriteLine("Enter the ID of the dependency: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);
            Dependency d = s_dalDependency!.Read(t_Id)!;
            if (d != null)
            {
                Task dependent = s_dalTask!.Read(d.DependentTask)!;
                Task dependsOn = s_dalTask!.Read(d.DependsOnTask)!;

                Console.WriteLine("Dependency { Id = "+d.Id+", DependentTask = "+dependent.Description+", DependsOnTask = "+dependsOn.Description +"}");
            }
            else { Console.WriteLine("ID was not found"); }
        }
        //print all of the dependencies
        private static void readAllDependencies()
        {
            List<Dependency> dependencies = s_dalDependency!.ReadAll();
            foreach (Dependency d in dependencies)
            {
                Task dependent = s_dalTask!.Read(d.DependentTask)!;
                Task dependsOn = s_dalTask!.Read(d.DependsOnTask)!;

                Console.WriteLine("Dependency { Id = " + d.Id + ", DependentTask = " + dependent.Description + ", DependsOnTask = " + dependsOn.Description + "}");
            }
        }
        
        //update the t object 
        private static void updateDependency(Dependency t)
        {
            string? temp;
            int t_Dependent=0, t_DependsOn=0;
            //in each field' checks if the user typed blank- doesn't want the field to change
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

            Dependency p = new(t.Id, t_Dependent, t_DependsOn);
            s_dalDependency!.Update(p);
            Console.WriteLine(p);
        }
        /// <summary>
        /// delete a dependency by id
        /// </summary>
        private static void deleteDependency()
        {
            Console.WriteLine("Enter the ID of the dependency to delete: ");
            string? temp = Console.ReadLine();
            int t_Id = int.Parse(temp!);//convert from string
            try ///delete can throw exceptions
            {
                s_dalDependency!.Delete(t_Id);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}
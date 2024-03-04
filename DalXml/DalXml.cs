using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    /// <summary>
    /// this class connects the IDal interface to the XML implementations of the entities 
    /// </summary>
    sealed internal class DalXml : IDal
    {
        public static IDal Instance { get; } = new DalXml();
        private DalXml() { }
        
        public ITask Task => new TaskImplementation();
        public IEngineer Engineer => new EngineerImplementation();
        public IDependency Dependency => new DependencyImplementation();
        public IUser User => new UserImplementation();


        /// <summary>
        /// these properties represents the project start and end dates
        /// also, there's a property for the project's current status 
        /// the status is an enum type
        /// all of these properties will be saved in the config xml file
        /// </summary>
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        /// <summary>
        /// sets the given dates in the XML config file
        /// </summary>
        /// <param name="start"> project's start date </param>
        /// <param name="end"> project's end date </param>
        public void setStartAndEndDates(DateTime start, DateTime? end)
        {
            ///load the data from the config file
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///if the start date isn't written in the file, insert it
            if(root.Element("ProjectStartDate") is null)
            {
                XElement t_pStart = new XElement("ProjectStartDate", start);
                root.Add(t_pStart);
            }
            else///change the value of the start date to the given one
                root.Element("ProjectStartDate")?.SetValue((start).ToString());

            ///if the end date isn't written in the file, insert it
            if (root.Element("ProjectEndDate") is null)
            {
                XElement t_pEnd = new XElement("ProjectEndDate", end);
                root.Add(t_pEnd);
            }
            else///change the value of the end date to the given one
                root.Element("ProjectEndDate")?.SetValue((end).ToString());

            ///save the changes to the file
            XMLTools.SaveListToXMLElement(root, "data-config");
        }

        /// <summary>
        /// change the project's status in the file from plan to execution
        /// </summary>
        public void changeStatus()
        {
            ///load the data from the config file
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///change the status the execution stage
            root.Element("ProjectStatus")?.SetValue((DO.ProjectStatus.ExecutionStage).ToString());

            ///save the changes to the file
            XMLTools.SaveListToXMLElement(root, "data-config");
        }

        /// <summary>
        /// set the project's status in the file to plan stage
        /// </summary>
        public void setStatus()
        {
            ///load the data from the config file
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///if the status isn't written in the file, insert it
            if (root.Element("ProjectStatus") is null)
            {
                XElement t_pStatus = new XElement("ProjectStatus", DO.ProjectStatus.PlanStage);
                root.Add(t_pStatus);
            }
            else///change the value of the project's status to the plan stage
                root.Element("ProjectStatus")?.SetValue((DO.ProjectStatus.PlanStage).ToString());

            ///save the changes to the file
            XMLTools.SaveListToXMLElement(root, "data-config");
        }

        /// <summary>
        /// return the project's start date
        /// </summary>
        /// <returns></returns>
        public DateTime? getStartDate()
        {
            ///load data
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///gets the date from the file
            XElement? e = root.Element("ProjectStartDate");

            ///if the element was already inserted
            if (e is not null)
            {
                ///convert the value to string, then convert it to date time object to return
                string temp = e.Value;
                DateTime s;
                DateTime.TryParse(temp, out s);
                return s;
            }
            else
                return null;
        }

        /// <summary>
        /// return the project's end date
        /// </summary>
        /// <returns></returns>
        public DateTime? getEndDate()
        {
            ///load data
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///gets the date from the file
            XElement? e = root.Element("ProjectEndDate");

            ///if the element was already inserted
            if (e is not null)
            {
                ///convert the value to string, then convert it to date time object to return
                string temp = e.Value;
                DateTime s;
                DateTime.TryParse(temp, out s);
                return s;
            }
            else
                return null;
        }

        /// <summary>
        /// return the project's status 
        /// </summary>
        /// <returns></returns>
        public ProjectStatus getProjectStatus()
        {
            ///load data
            XElement root = XMLTools.LoadListFromXMLElement("data-config");

            ///gets the date from the file
            XElement? e = root.Element("ProjectStatus");

            ///convert the value to string, then convert it to enum to return
            string temp = e.Value;
            DO.ProjectStatus s;
            Enum.TryParse(temp, out s);
            return s;           
        }
        /// <summary>
        /// calling help function in XML.Tools
        /// set id's to 1
        /// </summary>
        public void ResetId()
        {
            XMLTools.ResetId("data-config", "NextDependencyId");
            XMLTools.ResetId("data-config", "NextTaskId");
        }
    }
}

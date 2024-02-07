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


        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        public void setStartAndEndDates(DateTime start, DateTime end)
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            if(root.Element("ProjectStartDate") is null)
            {
                XElement t_pStart = new XElement("ProjectStartDate", start);
                root.Add(t_pStart);
            }
            else
                root.Element("ProjectStartDate")?.SetValue((start).ToString());
            if (root.Element("ProjectEndDate") is null)
            {
                XElement t_pEnd = new XElement("ProjectEndDate", end);
                root.Add(t_pEnd);
            }
            else
                root.Element("ProjectEndDate")?.SetValue((end).ToString());
            XMLTools.SaveListToXMLElement(root, "data-config");
        }

        public void changeStatus()
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            root.Element("ProjectStatus")?.SetValue((DO.ProjectStatus.ExecutionStage).ToString());
            XMLTools.SaveListToXMLElement(root, "data-config");
        }
        public void setStatus()
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            if (root.Element("ProjectStatus") is null)
            {
                XElement t_pStatus = new XElement("ProjectStatus", DO.ProjectStatus.PlanStage);
                root.Add(t_pStatus);
            }
            XMLTools.SaveListToXMLElement(root, "data-config");
        }

        public DateTime? getStartDate()
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            XElement? e = root.Element("ProjectStartDate");
            if (e is not null)
            {
                string temp = e.Value;
                DateTime s;
                DateTime.TryParse(temp, out s);
                return s;
            }
            else
                return null;
        }

        public DateTime? getEndDate()
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            XElement? e = root.Element("ProjectEndDate");
            if (e is not null)
            {
                string temp = e.Value;
                DateTime s;
                DateTime.TryParse(temp, out s);
                return s;
            }
            else
                return null;
        }

        public ProjectStatus getProjectStatus()
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            XElement? e = root.Element("ProjectStatus");
            string temp = e.Value;
            DO.ProjectStatus s;
            Enum.TryParse(temp, out s);
            return s;
            
        }
    }
}

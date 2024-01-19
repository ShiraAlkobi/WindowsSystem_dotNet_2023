using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal;
/// <summary>
/// ICRUD functions' implementation for the engineer entity using the data saved in the right XML file 
/// in this file, we implement using the XElement class
/// </summary>
internal class EngineerImplementation:IEngineer
{
    readonly string s_engineer_xml = "engineers"; ///name of XML file

    ///help function - gets an XElement object and converting it to engineer
    static Engineer getEngineer(XElement element)
    {
        return new Engineer()
        {
            Id = element.ToIntNullable("Id") ?? throw new FormatException("Can't convert Id"),
            Email = (string?)element.Element("Email") ?? "",
            Cost = element.ToDoubleNullable("Cost") ?? throw new FormatException("Can't convert Cost"),
            Name = (string?)element.Element("Name") ?? "",
            Level = element.ToEnumNullable<EngineerExperience>("Level") ?? EngineerExperience.Beginner
        };
    }

    /// <summary>
    /// creating new object and adding it to the XML file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        ///Loading the collection of engineers from the file
        IEnumerable<XElement> engineers = XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements(); 
        ///search for the right enginner by id
        XElement? engineer_Elem = engineers.FirstOrDefault(p => (int?)p.Element("Id") == item.Id);
        if (engineer_Elem == null) ///if not found, add it to the file
        {
            ///convert engineer to XElement
            XElement t_Id = new XElement("Id", item.Id);
            XElement t_Email = new XElement("Email", item.Email);
            XElement t_Cost = new XElement("Cost", item.Cost);
            XElement t_Name = new XElement("Name", item.Name);
            XElement t_Level = new XElement("Level", item.Level);

            XElement t_Engineer = new XElement("Engineer", t_Id,t_Email,t_Cost,t_Name,t_Level);

            ///load data from the file to XElement object, add the new engineer and save all in the file
            XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineer_xml);
            engineerRoot.Add(t_Engineer);
            XMLTools.SaveListToXMLElement(engineerRoot, s_engineer_xml);
            return item.Id;
        }///if the engineer already exists in the file, no need to add a new one
        throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
    }

    /// <summary>
    /// Removes an object from the file by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        ///Loading the collection of engineers from the file
        IEnumerable<XElement> engineers = XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements();
        ///search for the right enginner by id
        XElement? engineer_Elem = engineers.FirstOrDefault(p => (int?)p.Element("Id") == id);
        if (engineer_Elem != null)///if found
        {
            ///remove and update the file
            engineer_Elem.Remove();
            XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineer_xml);
            XMLTools.SaveListToXMLElement(engineerRoot, s_engineer_xml);
            return;
        }///if the engineer doesn't exist in the file, can't delete it
        throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
    }

    /// <summary>
    /// Returns the right engineer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        ///Load and search
        IEnumerable<XElement> engineers = XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements();
        XElement? engineer_Elem = engineers.FirstOrDefault(p => (int?)p.Element("Id") == id);

        if (engineer_Elem != null) ///if found
            return getEngineer(engineer_Elem); ///calling the help function for coverting 
        return null; ///if not found
    }

    /// <summary>
    /// Returns the right engineer by condition - the filter param
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements().Select(s => getEngineer(s)).FirstOrDefault(filter);
    }

    /// <summary>
    /// Returns a collection of the engineers that answer to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        /// if the filter is null, return all engineers
        if (filter == null) { return XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements().Select(s => getEngineer(s)); }
        return XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements().Select(s => getEngineer(s)).Where(filter);
    }

    /// <summary>
    /// Update an existing engineer in the file
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        ///load and search
        IEnumerable<XElement> engineers = XMLTools.LoadListFromXMLElement(s_engineer_xml).Elements();
        XElement? engineer_Elem = engineers.FirstOrDefault(p => (int?)p.Element("Id") == item.Id);
        if (engineer_Elem != null) /// if found
        {
            ///updating the values in the object
            engineer_Elem.Element("Email").Value = item.Email;
            engineer_Elem.Element("Cost").Value = item.Cost.ToString();
            engineer_Elem.Element("Name").Value = item.Name;
            engineer_Elem.Element("Level").Value = item.Level.ToString();

            ///update the file
            XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineer_xml);
            XMLTools.SaveListToXMLElement(engineerRoot, s_engineer_xml);
            return;
        }///if the engineer doesn't exist in the file, can't update it
        throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
    }
}

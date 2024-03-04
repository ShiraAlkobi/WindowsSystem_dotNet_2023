using System.Linq;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;
/// <summary>
/// ICRUD functions' implementation for the user entity using the data saved in the right XML file 
/// in this file, we implement using the XSerializer class
/// </summary>
public class UserImplementation : IUser
{
    readonly string s_user_xml = "users";///name of xml file

    /// <summary>
    /// creating new object and adding to xml file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(User item)
    {
        ///Loading the list of users from the file
        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);
        Users.Add(item);///adding to list
        XMLTools.SaveListToXMLSerializer<DO.User>(Users, s_user_xml);///save list to xml file
        return item.Id;
    }

    /// <summary>
    /// deleting a user from the xml file
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);///loading the list
        if (!Users.Exists(p => p.Id == id))///if the user doesn't exist
            throw new DalDoesNotExistException($"User with ID={id} does Not exist");///throw exception
        else
        {
            DO.User temp = Users.Find(p => p.Id == id)!;///find the user according to the ID
            Users.Remove(temp);///remove user from list
            XMLTools.SaveListToXMLSerializer<DO.User>(Users, s_user_xml);///save updated list to xml file
        }
    }

    /// <summary>
    /// read user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public User? Read(int id)
    {
        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);///loading the list
        return Users.FirstOrDefault(p => p.Id == id);///return the right user
    }

    /// <summary>
    /// read user by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public User? Read(Func<User, bool> filter)
    {
        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);///loading list
        return Users.FirstOrDefault(filter);///return the the first user to answer filter
    }

    /// <summary>
    /// read all users that answer to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);///loading list
        if (filter == null)
            return Users;///no filter to answer,return full list
        else
            return Users.Where(filter);///return sub-list where all users answer to filter
    }

    /// <summary>
    /// updating user in the list from xml file
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(User item)
    {

        List<DO.User> Users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);///loading list

        ///check if it exists in the list
        if (!Users.Exists(p => p!.Id == item.Id))///if user doesnt exist
            throw new DalDoesNotExistException($"User with ID={item.Id} does Not exist");
        else
        {
            DO.User temp = Users.Find(p => p!.Id == item.Id)!;///finding the correct user
            Users.Remove(temp);///remove the old object from the list
            Users.Add(item);///add the new object - the updated
            XMLTools.SaveListToXMLSerializer<DO.User>(Users, s_user_xml);///save updated list to xml file

        }
    }
}

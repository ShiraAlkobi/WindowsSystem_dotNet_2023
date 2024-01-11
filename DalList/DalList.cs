
namespace Dal;
using DalApi;
/// <summary>
/// this class gather the implementations of the entities by inheriting 
/// from IDal interface. for each entity(property in IDal) we call the implementations function
/// and initialize interface type accordingly.
/// </summary>
sealed public class DalList : IDal
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();
}

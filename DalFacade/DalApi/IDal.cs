namespace DalApi;
/// <summary>
/// this interface wrapps all of the entities` interfaces
/// to represent the dal data level
/// </summary>
public interface IDal
{
    ITask Task { get; }///property for the sub interface ITask
    IEngineer Engineer { get; }///property for the sub interface IEngineer
    IDependency Dependency { get; }///property for the sub interface IDependency
}

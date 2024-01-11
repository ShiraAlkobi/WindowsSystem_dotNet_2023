using DO;
using System;

namespace DalApi;
/// <summary>
/// parent interface of the entities` interfaces(they inherit from ICrud)
/// the crud functions will be used by each entity. T will be the entity type.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
    List<T> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}

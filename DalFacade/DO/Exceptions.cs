///this file gathers the exceptions` classes
///they inherit from exception class
namespace DO;
[Serializable]
public class DalDoesNotExistException : Exception
{
    /// <summary>
    /// in case of trying to update or delete an object but it does not exist
    /// </summary>
    /// <param name="message"></param>
    public DalDoesNotExistException(string? message) : base(message) { }
}
[Serializable]
public class DalAlreadyExistsException : Exception
{
    /// <summary>
    /// in case of trying to create an object with a new id, but the id already exists
    /// </summary>
    /// <param name="message"></param>
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalDeletionImpossible : Exception
{
    /// <summary>
    /// in case of trying to delete an object but its forbidden
    /// </summary>
    /// <param name="message"></param>
    public DalDeletionImpossible(string? message) : base(message) { }
}
public class DalXMLFileLoadCreateException : Exception
{
    /// <summary>
    /// in case of trying to delete an object but its forbidden
    /// </summary>
    /// <param name="message"></param>
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
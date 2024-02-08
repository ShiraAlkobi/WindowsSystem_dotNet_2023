namespace BO;

[Serializable]
public class BlInputCheckException : Exception
{
    /// <summary>
    /// wrong input exception
    /// </summary>
    /// <param name="message"></param>
    public BlInputCheckException(string? message) : base(message) { }
}
  [Serializable]
public class BlDoesNotExistException : Exception
{
    /// <summary>
    /// exception for when trying to operate on an object but it doesnt exsist
    /// </summary>
    /// <param name="message"></param>
	public BlDoesNotExistException(string? message) : base(message) { }
	public BlDoesNotExistException(string message, Exception innerException)
        		: base(message, innerException) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    /// <summary>
    /// in case of trying to create an object with a new id, but the id already exists
    /// </summary>
    /// <param name="message"></param>
    public BlAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class BlDeletionImpossible : Exception
{
    /// <summary>
    /// in case of trying to delete an object but its forbidden
    /// </summary>
    /// <param name="message"></param>
    public BlDeletionImpossible(string? message) : base(message) { }
}
[Serializable]
public class BlXMLFileLoadCreateException : Exception
{
    /// <summary>
    /// failed to load data from xml
    /// </summary>
    /// <param name="message"></param>
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
}

[Serializable]
public class BlCanNotDelete : Exception
{
    /// <summary>
    /// in case of trying to delete an object but its forbidden
    /// </summary>
    /// <param name="message"></param>
    public BlCanNotDelete(string? message) : base(message) { }
}
[Serializable]
public class BlCanNotUpdate : Exception
{
    /// <summary>
    /// in case of trying to update an object but its forbidden
    /// </summary>
    /// <param name="message"></param>
    public BlCanNotUpdate(string? message) : base(message) { }
}
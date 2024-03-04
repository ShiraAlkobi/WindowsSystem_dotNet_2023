namespace DO;

/// <summary>
/// This class represents a user in the project
/// </summary>
/// <param name="Id">the id of the user</param>
/// <param name="UserName">the user's name</param>
/// <param name="Password">the user's password</param>
/// <param name="Position">the position of the user, Manager or Engineer</param>

public record User
(
    int Id,
    string UserName,
    string Password,
    DO.Position Position
)
{
    public User() :this (0,string.Empty,string.Empty,default)
    { }
}

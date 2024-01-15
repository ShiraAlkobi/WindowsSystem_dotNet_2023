namespace DO;
/// <summary>
/// This class represents an engineer in the project
/// </summary>
/// <param name="Id">The personal unique id of the engineer</param>
/// <param name="Email">Email of the engineer</param>
/// <param name="Cost">Hourly pay</param>
/// <param name="Name">The name of the engineer</param>
/// <param name="Level">The engineer's expertism</param>
public record Engineer
(
    int Id,
    string? Email,
    double Cost,
    string? Name,
    DO.EngineerExperience Level
)
{
    public Engineer() : this(0,null,0,null,0) { } ///empty ctor
}

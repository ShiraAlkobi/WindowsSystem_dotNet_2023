namespace BO;

/// <summary>
/// in this file we define the class EngineerInTask - an object to represent the engineer which is assigned to the task
/// </summary>
public class EngineerInTask
{
    public int Id {  get; init; }
    public string? Name { get; set; }
    public override string ToString() => Tools.ToStringProperty(this);
}

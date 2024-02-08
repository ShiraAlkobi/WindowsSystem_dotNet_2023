namespace BO;
/// <summary>
/// in this file we define the Engineer class - a main entity of the BL level 
/// </summary>
public class Engineer
{ 
    public int Id { get; init; }
    public string? Email { get; set; }
    public double Cost { get; set; }
    public string? Name { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task {  get; set; }///the task assigned to engineer

    public override string ToString() => Tools.ToStringProperty(this);
}

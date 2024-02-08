namespace BO;
/// <summary>
/// Engineer class for Engineer entity in the BL level
/// 
/// </summary>
public class Engineer
{ 
    public int Id { get; init; }
    public string? Email { get; set; }
    public double Cost { get; set; }
    public string? Name { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task {  get; set; }

    public override string ToString() => Tools.ToStringProperty(this);
}

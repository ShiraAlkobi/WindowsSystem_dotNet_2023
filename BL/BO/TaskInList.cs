namespace BO;

public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public BO.Status Status { get; set; }

    public override string ToString() => Tools.ToStringProperty(this);
}

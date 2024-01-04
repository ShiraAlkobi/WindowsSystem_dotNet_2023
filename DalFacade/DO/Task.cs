namespace DO;
/// <summary>
/// This class represents a task in the project
/// </summary>
/// <param name="Id">The unique identifing number of the task </param>
/// <param name="Alias">General name</param>
/// <param name="Description">What the task is about</param>
/// <param name="IsMilestone">      </param>
/// <param name="CreatedAtDate">The creation date of the task</param>
/// <param name="ScheduledDate">The planned date for starting working on the task </param>
/// <param name="StartDate">The actual date of starting the work on the task</param>
/// <param name="RequiredEffortTime">The amount of time required for completing the task</param>
/// <param name="DeadlineDate">The last possible date to finish the task</param>
/// <param name="CompleteDate">The actual date when the task was completed</param>
/// <param name="Deliverables">The complete task to deliver </param>
/// <param name="Remarks">Any remarks on the task</param>
/// <param name="EngineerId">The id of the engineer who worked on the task</param>
/// <param name="Complexity">How hard was the task</param>
public record Task
(
    int Id,
    string? Alias,  
    string? Description,
    bool? IsMilestone,
    DateTime? CreatedAtDate,
    DateTime ScheduledDate,
    DateTime StartDate,
    TimeSpan RequiredEffortTime,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId,
    DO.EngineerExperience Complexity

)
{
    public Task() : this (0,null,null,null,null,default(DateTime), default(DateTime), default(TimeSpan), default(DateTime), default(DateTime),"","",0, 0) {}
    ///empty ctor
}


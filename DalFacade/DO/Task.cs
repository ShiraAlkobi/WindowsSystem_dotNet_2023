namespace DO;

public record Task
(
    int? Id,
    string? Alias,
    string? Description,
    bool? IsMilestone,
    DateTime? CreatedAtDate,
    DateTime? ScheduledDate,
    DateTime? StartDate,
    TimeSpan? RequiredEffortTime,
    DateTime? DeadlineDate,
    DateTime? CompleteDate,
    string? Deliverables,
    string? Remarks,
    int? EngineerId,
    DO.EngineerExperience Complexity

)
{
    public Task() : this (0){}
}


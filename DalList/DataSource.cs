namespace Dal;
/// <summary>
/// The class is the base of the data layer
/// Each of the entities of the DO folder has a list of nullable references 
/// </summary>
internal static class DataSource
{
    internal static List<DO.Dependency?> Dependencys { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();

    /// <summary>
    /// Unique identifing number for each task and dependency, project's start and end dates and status
    /// </summary>
    internal static class Config
    {   
        internal const int startTaskId = 0;///the lowest possible value for a task's id 
        private static int nextTaskId = startTaskId;///will get the last value
        internal static int NextTaskId { get => nextTaskId++; } ///will update the value


        internal const int startDependencyId = 0;///the lowest possible value for a dependency's id 
        private static int nextDependencyId = startDependencyId;///will get the last value
        internal static int NextDependencyId { get => nextDependencyId++; }///will update the value

        ///fields to the project's dates and status
        internal static DateTime? projectStartDate = null;
        internal static DateTime? projectEndDate = null;
        internal static DO.ProjectStatus projectStatus = DO.ProjectStatus.PlanStage;        
    }
}

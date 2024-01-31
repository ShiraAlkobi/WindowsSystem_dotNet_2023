namespace BlImplementation;
using BlApi;


internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task t)
    {
        if (t.Id < 0 || t.Alias == "" || t.Alias == null)
            throw new BO.BlInputCheckException("wrong input\n");
        try
        {
            int t_id = _dal.Task.Create(t_engineer);
            return t_id;
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Task t)
    {
        throw new NotImplementedException();
    }

    public void UpdateScedualedDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }
}

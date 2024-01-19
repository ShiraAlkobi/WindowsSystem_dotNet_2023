using DalApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalXml : IDal
    {
        public ITask Task => new TaskImplementation();
        public IEngineer Engineer => new EngineerImplementation();
        public IDependency Dependency => new DependencyImplementation();
    }
}

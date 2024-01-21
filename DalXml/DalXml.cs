using DalApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    /// <summary>
    /// this class connects the IDal interface to the XML implementations of the entities 
    /// </summary>
    public class DalXml : IDal
    {
        public ITask Task => new TaskImplementation();
        public IEngineer Engineer => new EngineerImplementation();
        public IDependency Dependency => new DependencyImplementation();
    }
}

using DalApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    /// <summary>
    /// this class connects the IDal interface to the XML implementations of the entities 
    /// </summary>
    sealed internal class DalXml : IDal
    {
        public static IDal Instance { get; } = new DalXml();
        private DalXml() { }
        public ITask Task => new TaskImplementation();
        public IEngineer Engineer => new EngineerImplementation();
        public IDependency Dependency => new DependencyImplementation();
    }
}

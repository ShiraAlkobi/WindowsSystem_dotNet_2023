using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

/// <summary>
/// in this class we create a 'Factory for creating objects'
/// by using this pattern we can ensure that only one instance of the BL interface will be created and not several 
/// this factory is for the BL level
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}

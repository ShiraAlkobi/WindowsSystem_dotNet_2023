﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PL
{
    /// <summary>
    /// converts the enum EngineerExperience values into a collection - IEnumerable
    /// </summary>
    /// 

    internal class EngineerExperienceCollection : IEnumerable
    {
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
                (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class StatusCollection : IEnumerable
    {
        static readonly IEnumerable<BO.Status> s_enums =
                (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
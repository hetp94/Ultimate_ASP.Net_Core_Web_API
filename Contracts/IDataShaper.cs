﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataShaper<T> where T : class
    {
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString);
        ExpandoObject ShapeData(T entity, string fieldString);
    }
}

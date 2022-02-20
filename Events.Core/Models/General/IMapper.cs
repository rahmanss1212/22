using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public interface IMapper<T,V> 
    {
        V Convert(T entity);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Base;

public class Entity : IEntity
{
    /// <summary>
    /// Creates a copy from this entity.
    /// </summary>
    /// <returns>The clone.</returns>
    public IEntity Clone()
    {
        return (IEntity)this.MemberwiseClone();
    }

    /// <summary>
    /// Creates a copy from this entity.
    /// </summary>
    /// <returns>The clone.</returns>
    public T Clone<T>()
        where T : class, IEntity
    {
        return (T)this.Clone();
    }
}

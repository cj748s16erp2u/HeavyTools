using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

public interface IEntity
{
    /// <summary>
    /// Creates a copy from this entity.
    /// </summary>
    /// <returns>The clone.</returns>
    IEntity Clone();

    /// <summary>
    /// Creates a copy from this entity.
    /// </summary>
    /// <returns>The clone.</returns>
    T Clone<T>() where T : class, IEntity;
}

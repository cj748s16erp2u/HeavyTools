using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

public static class EntityExtensions
{
    public static void ClearNavigationProperties<TEntity>(this TEntity entity)
        where TEntity : Base.Entity
    {
        if (entity is null)
        {
            return;
        }

        var props = typeof(TEntity).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty)
            .Where(p => p.GetCustomAttributes(typeof(InversePropertyAttribute), true)?.Any() == true);

        foreach (var prop in props)
        {
            var propType = prop.PropertyType;
            if (propType.IsGenericType)
            {
                propType = propType.GetGenericTypeDefinition();
            }

            if (propType == typeof(ICollection<>))
            {
                var propValue = prop.GetValue(entity);
                if (propValue is not null)
                {
                    var method = prop.PropertyType.GetMethod(nameof(ICollection<object>.Clear), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod);
                    method?.Invoke(propValue, null);
                }
            }
            else
            {
                prop.SetValue(entity, null);
            }
        }
    }
}

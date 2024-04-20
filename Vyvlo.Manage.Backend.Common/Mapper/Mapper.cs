using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper;

public static class Mapper
{
    public static TDestination Map<TDestination>(object source) where TDestination : class
    {
        TDestination instance = Activator.CreateInstance(typeof(TDestination)) as TDestination ?? throw new InvalidCastException("Could not create an instance of the destination object");
        ICollection<PropertyInfo> instanceProperties = instance.GetType().GetProperties();
        foreach (PropertyInfo item in source.GetType().GetProperties())
        {
            if (instanceProperties.Where(x => x.Name == item.Name).Any())
            {
                PropertyInfo prop = instance.GetType().GetProperty(item.Name) ?? throw new NullReferenceException("Invalid property");
                prop.SetValue(instance, item.GetValue(source), null);
            }
        }
        return instance;
    }
}
using System.Reflection;

namespace Application.Services;

public class Mapper
{
    public TTarget Map<TSource, TTarget>(TSource source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        
        var target = Activator.CreateInstance<TTarget>();
        Map(source, target);
        return target;
    }

    public void Map<TSource, TTarget>(TSource source, TTarget target)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        
        var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public 
                                                  | BindingFlags.Instance).Where(p => p.CanRead);
        var targetProps = typeof(TTarget).GetProperties(BindingFlags.Public 
                                                     | BindingFlags.Instance).Where(p => p.CanRead);

        foreach (var sourceProp in sourceProps)
        {
            var targetProp = targetProps.FirstOrDefault(p => 
                p.Name == sourceProp.Name && 
                p.PropertyType == sourceProp.PropertyType);

            if (targetProp != null && sourceProp.CanWrite)
            {
                targetProp.SetValue(target, sourceProp.GetValue(source));
            }
        }
    }
}
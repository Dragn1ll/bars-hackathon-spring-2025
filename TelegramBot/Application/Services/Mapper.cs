using System.Reflection;

namespace Application.Services;

public class Mapper
{
    public TTarget Map<TSource, TTarget>(TSource source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        
        if (IsPositionalRecord(typeof(TTarget)))
        {
            return MapToPositionalRecord<TSource, TTarget>(source);
        }
        
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
                                                     | BindingFlags.Instance).Where(p => p.CanWrite);

        foreach (var sourceProp in sourceProps)
        {
            var targetProp = targetProps.FirstOrDefault(p => 
                p.Name == sourceProp.Name && 
                p.PropertyType == sourceProp.PropertyType);

            if (targetProp != null)
            {
                targetProp.SetValue(target, sourceProp.GetValue(source));
            }
        }
    }

    private TTarget MapToPositionalRecord<TSource, TTarget>(TSource source)
    {
        var sourceProps = typeof(TSource)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToDictionary(p => p.Name, p => p.GetValue(source));

        var recordType = typeof(TTarget);
        var constructor = recordType.GetConstructors().First();
        var parameters = constructor.GetParameters();

        var args = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];
            if (sourceProps.TryGetValue(param.Name!, out var value))
            {
                args[i] = value;
            }
            else
            {
                args[i] = GetDefaultValue(param.ParameterType);
            }
        }

        return (TTarget)constructor.Invoke(args);
    }

    private bool IsPositionalRecord(Type type)
    {
        if (!type.IsClass)
            return false;

        return type.GetMethods().Any(m => m.Name == "<Clone>$") &&
               type.GetConstructors().Length > 0;
    }

    private object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
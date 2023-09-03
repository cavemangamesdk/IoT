using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MotionController.Data.Models;

public abstract class DatabaseModel
{
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> DatabaseGeneratedProperties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> Properties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> NotMappedProperties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ModelEqualityProperties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, object?> DefaultValues = new();
    private int? _requestedHashCode;

    private static IEnumerable<PropertyInfo> GetDatabaseGeneratedProperties(Type type)
    {
        return DatabaseGeneratedProperties.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes<DatabaseGeneratedAttribute>(true).Any(a => a.DatabaseGeneratedOption != DatabaseGeneratedOption.None)).ToArray();
        });
    }

    private static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return Properties.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            return type.GetProperties().ToArray();
        });
    }

    private static IEnumerable<PropertyInfo> GetKeyProperties(Type type)
    {
        return KeyProperties.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes<KeyAttribute>(true).Any()).ToArray();
        });
    }

    private static IEnumerable<PropertyInfo> GetNotMappedProperties(Type type)
    {
        return NotMappedProperties.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes<NotMappedAttribute>(true).Any()).ToArray();
        });
    }

    private static IEnumerable<PropertyInfo> GetModelEqualityProperties(Type type)
    {
        return ModelEqualityProperties.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes<PropertyModelEqualityBehaviorAttribute>(true).Any()).ToArray();
        });
    }

    private static object? GetDefaultValue(Type type)
    {
        return DefaultValues.GetOrAdd(type.TypeHandle, typeHandle =>
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        });
    }

    private static bool IsDefaultValue(object? value)
    {
        if (value == null)
        {
            return true;
        }
        return value.Equals(GetDefaultValue(value.GetType()));
    }

    public virtual bool IsTransient()
    {
        var generatedProperties = GetDatabaseGeneratedProperties(GetType());

        if (!generatedProperties.Any())
        {
            return false;
        }

        if (generatedProperties.Any(p => p.GetValue(this) == null))
        {
            return true;
        }

        return generatedProperties.Any(p => IsDefaultValue(p.GetValue(this)));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not DatabaseModel model)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        if (model.IsTransient() || IsTransient())
        {
            return false;
        }
        else
        {
            var keyProperties = GetKeyProperties(GetType());
            if (keyProperties.Any(p => p.GetValue(this) == null))
            {
                return false;
            }

            var modelEqualityProperties = GetModelEqualityProperties(GetType());
            var notMappedProperties = GetNotMappedProperties(GetType());
            var properties = GetProperties(GetType());
            foreach (var property in properties.Except(notMappedProperties.Except(modelEqualityProperties)))
            {
                var thisValue = property.GetValue(this);
                var modelValue = property.GetValue(model);
                if (thisValue == null)
                {
                    if (modelValue != null)
                    {
                        return false;

                    }
                    else
                    {
                        continue;
                    }
                }

                if (!thisValue.Equals(property.GetValue(model)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
            {
                var keyProperties = GetKeyProperties(GetType());
                if (keyProperties.Any(p => p.GetValue(this) == null))
                {
                    return base.GetHashCode();
                }

                var properties = GetProperties(GetType());
                foreach (var property in properties)
                {
                    var thisValue = property.GetValue(this);
                    if (thisValue == null)
                    {
                        return base.GetHashCode();
                    }

                    var hashCode = thisValue.GetHashCode() ^ 31;

                    if (_requestedHashCode.HasValue)
                    {
                        _requestedHashCode ^= hashCode;
                    }
                    else
                    {
                        _requestedHashCode = hashCode;
                    }
                }
            }
            return _requestedHashCode ?? base.GetHashCode();
        }
        else
        {
            return base.GetHashCode();
        }
    }

    public static bool operator ==(DatabaseModel left, DatabaseModel right)
    {
        if (Equals(left, null))
        {
            return Equals(right, null);
        }
        else
        {
            return left.Equals(right);
        }
    }

    public static bool operator !=(DatabaseModel left, DatabaseModel right)
    {
        return !(left == right);
    }
}

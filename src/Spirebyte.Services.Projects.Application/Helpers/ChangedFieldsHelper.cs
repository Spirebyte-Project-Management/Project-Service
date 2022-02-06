using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Projects.Application.Helpers.Objects;

namespace Spirebyte.Services.Projects.Application.Helpers;

public static class ChangedFieldsHelper
{
    public static Change[] GetChanges(object oldObject, object newObject)
    {
        var newProperties = newObject.GetType().GetProperties();
        var oldProperties = oldObject.GetType().GetProperties();
        var differentProperties = oldProperties.Where(x =>
        {
            var matchingProperty =
                newProperties.FirstOrDefault(n => n.Name == x.Name && n.PropertyType == x.PropertyType);
            if (matchingProperty is null) return false;

            if (matchingProperty.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) is null) return false;

            var oldValue = x.GetValue(oldObject);
            var newValue = matchingProperty.GetValue(newObject);

            return oldValue != null && !oldValue.Equals(newValue);
        });
        return differentProperties.Select(x => new Change(x.Name, x.GetValue(oldObject)?.ToString(),
            newProperties.First(n => n.Name == x.Name).GetValue(newObject)?.ToString())).ToArray();
    }
}
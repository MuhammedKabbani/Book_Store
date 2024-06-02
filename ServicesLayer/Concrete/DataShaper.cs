using ServicesLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Concrete
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {


        public PropertyInfo[] EntityProperties { get; set; }
        public DataShaper()
        {
            EntityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entites, string fieldsString)
        {
            var requiredProps = GetRequiredProperties(fieldsString);
            var result = FetchDataForEntity(entites, requiredProps);
            return result;
        }

        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var requiredProps = GetRequiredProperties(fieldsString);
            var result = FetchDataForEntity(entity, requiredProps);
            return result;
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            if (string.IsNullOrWhiteSpace(fieldsString))
                return EntityProperties;

            var requiredProps = new List<PropertyInfo>();
            
            foreach(var field in fieldsString.Split(',',StringSplitOptions.RemoveEmptyEntries))
            {
                var prop = EntityProperties.FirstOrDefault(x => x.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                if (prop is null)
                    continue;
                requiredProps.Add(prop);
            }

            return requiredProps;
        }
        
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requirdePropertis)
        {
            var shapedObject = new ExpandoObject();

            foreach (var prop in requirdePropertis)
            {
                var objectPropValue = prop.GetValue(entity);
                shapedObject.TryAdd(prop.Name, objectPropValue);
            }

            return shapedObject;
        }

        private IEnumerable<ExpandoObject> FetchDataForEntity(IEnumerable<T> entities, IEnumerable<PropertyInfo> requirdePropertis)
        {
            var shapedData = new List<ExpandoObject>();

            foreach(var entity in entities)
            {
                shapedData.Add(FetchDataForEntity(entity, requirdePropertis));
            }

            return shapedData;
        }
    }
}

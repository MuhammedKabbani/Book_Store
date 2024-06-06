using EntityLayer.Models;
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
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entites, string fieldsString)
        {
            var requiredProps = GetRequiredProperties(fieldsString);
            var result = FetchDataForEntity(entites, requiredProps);
            return result;
        }

        public ShapedEntity ShapeData(T entity, string fieldsString)
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
        
        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requirdePropertis)
        {
            var shapedObject = new ShapedEntity();

            foreach (var prop in requirdePropertis)
            {
                var objectPropValue = prop.GetValue(entity);
                shapedObject.Entity.TryAdd(prop.Name, objectPropValue);
            }
            var objecProp = entity.GetType().GetProperty("Id");
            shapedObject.Id = (int)objecProp.GetValue(entity);

            return shapedObject;
        }

        private IEnumerable<ShapedEntity> FetchDataForEntity(IEnumerable<T> entities, IEnumerable<PropertyInfo> requirdePropertis)
        {
            var shapedData = new List<ShapedEntity>();

            foreach(var entity in entities)
            {
                shapedData.Add(FetchDataForEntity(entity, requirdePropertis));
            }

            return shapedData;
        }
    }
}

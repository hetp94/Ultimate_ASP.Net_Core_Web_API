using Contracts;
using System.Dynamic;
using System.Reflection;

namespace Service.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
        {
            var requiredProperties = GetRequiredProperties(fieldString);

            return FetchData(entities, requiredProperties);
        }

        public ExpandoObject ShapeData(T entity, string fieldString)
        {
            var requiredProperties = GetRequiredProperties(fieldString);

            return FetchDataForEntity(entity, requiredProperties);
        }

        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ExpandoObject();

            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectPropertyValue);
            }
            return shapedObject;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            List<ExpandoObject> shapedData = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                ExpandoObject shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
        {
            var requiredProperties = new List<PropertyInfo>();
            if (!string.IsNullOrEmpty(fieldString))
            {
                var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(x => x.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (property == null)
                    {
                        continue;
                    }
                    requiredProperties.Add(property);
                }
            }
            else
            {
                requiredProperties = Properties.ToList();
            }

            return requiredProperties;
        }
    }
}

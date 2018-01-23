using ProtoBuf.Meta;
using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Data.Api
{
    public class TypeModelHelper
    {
        private static RuntimeTypeModel instance;

        public static RuntimeTypeModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = TypeModel.Create();
                    AddTypeModels(instance);
                }

                return instance;
            }
        }

        public static void AddTypeModels(RuntimeTypeModel model)
        {
            int i = 100;
            var domainTypes = new Type[] { typeof(Order), typeof(Customer), typeof(OrderLine), typeof(Person) };
            foreach (var domainType in domainTypes)
            {
                var regularProperties = domainType.GetProperties()
                    .Where(p => !p.GetGetMethod().IsVirtual)
                    .Select(p => p.Name).OrderBy(name => name);
                var metaTypeModel = model.Add(domainType, true).Add(regularProperties.ToArray());
                var virtualProperties = domainType.GetProperties().Where(p => p.GetGetMethod().IsVirtual).Select(p => p.Name).OrderBy(name => name);
                foreach (var virtualProperty in virtualProperties)
                {
                    var field = metaTypeModel.AddField(i++, virtualProperty);
                    field.AsReference = true;
                }

                metaTypeModel.AsReferenceDefault = true;
            }
        }
    }
}

// http://www.codeproject.com/Articles/24385/Have-a-Great-DesignTime-Experience-with-a-Powerful

using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.DesignSurfaceExt
{

    internal class DesignerSerializationServiceImpl : IDesignerSerializationService
    {

        private IServiceProvider _serviceProvider;

        public DesignerSerializationServiceImpl(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public ICollection Deserialize(object serializationData)
        {
            SerializationStore serializationStore = serializationData as SerializationStore;
            if (serializationStore != null)
            {
                ComponentSerializationService componentSerializationService = _serviceProvider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
                ICollection collection = componentSerializationService.Deserialize(serializationStore);
                return collection;
            }
            return new object[] { };
        }

        public object Serialize(ICollection objects)
        {
            ComponentSerializationService componentSerializationService = _serviceProvider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
            SerializationStore returnObject = null;
            using (SerializationStore serializationStore = componentSerializationService.CreateStore())
            {
                foreach (object obj in objects)
                {
                    if (obj is Control)
                        componentSerializationService.Serialize(serializationStore, obj);
                }
                returnObject = serializationStore;
            }
            return returnObject;
        }

    }//end_class

}//end_namespace
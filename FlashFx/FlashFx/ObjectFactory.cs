using flash.utils;
using FlashFx.AMF3;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlashFx
{
    public class ObjectFactory
    {
        private static ObjectFactory _instance;

        private Dictionary<string, Type> ClassMappings = new Dictionary<string, Type>();

        private ObjectFactory()
        {
            ClassMappings.Add("flex.messaging.io.ArrayCollection", typeof(ArrayCollection));
        }

        private static ObjectFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObjectFactory();
                }

                return _instance;
            }
        }

        private object InternalCreateInstance(string typeName)
        {
            Type type = null;

            if (ClassMappings.ContainsKey(typeName))
            {
                type = ClassMappings[typeName];
            }

            if (type != null)
            {
                if (type.IsAbstract && type.IsSealed)
                {
                    return null;
                }

                return Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, null, null);
            }

            return null;
        }

        public static object CreateInstance(string type)
        {
            return Instance.InternalCreateInstance(type);
        }
    }
}

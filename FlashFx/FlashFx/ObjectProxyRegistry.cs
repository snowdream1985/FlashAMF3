using flash.utils;
using FlashFx.AMF3;
using System;
using System.Collections.Generic;

namespace FlashFx.IO
{
    public class ObjectProxyRegistry
    {
        private static ObjectProxyRegistry _instance;

        private Dictionary<Type, IObjectProxy> registeredProxies;

        private static IObjectProxy defaultObjectProxy;

        public static ObjectProxyRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObjectProxyRegistry();

                    defaultObjectProxy = new ObjectProxy();
                }

                return _instance;
            }
        }

        public ObjectProxyRegistry()
        {
            registeredProxies = new Dictionary<Type, IObjectProxy>();

            registeredProxies.Add(typeof(ASObject), new ASObjectProxy());

            registeredProxies.Add(typeof(IExternalizable), new ExternalizableProxy());
        }

        public IObjectProxy GetObjectProxy(Type type)
        {
            if (type.GetInterface(typeof(IExternalizable).FullName, true) != null)
            {
                return registeredProxies[typeof(IExternalizable)] as IObjectProxy;
            }

            if (registeredProxies.ContainsKey(type))
            {
                return registeredProxies[type] as IObjectProxy;
            }

            foreach (var entry in registeredProxies)
            {
                if (type.IsSubclassOf(entry.Key))
                {
                    return entry.Value;
                }
            }

            return defaultObjectProxy;
        }
    }
}

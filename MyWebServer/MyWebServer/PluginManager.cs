using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        public PluginManager()
        {
            Add(new TestPlugin());
            Add(new StaticFilePlugin());
            Add(new LowerPlugin());
            Add(new TemperaturePlugin());
            Add(new NavigationPlugin());

            LoadPlugins();
        }
        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        private void LoadPlugins()
        {
            string pluginPath = AppContext.Current.PluginDirectory;
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
                return;
            }

            foreach (string file in Directory.GetFiles(pluginPath))
            {
                if (!file.EndsWith(".dll"))
                {
                    continue;
                }

                Assembly pluginDLL = Assembly.LoadFrom(Path.Combine(AppContext.Current.PluginDirectory, file));
                Type[] pluginTypes = pluginDLL.GetTypes();
                foreach (Type pluginType in pluginTypes)
                {
                    if (pluginType.GetInterface("BIF.SWE1.Interfaces.IPlugin") == null)
                    {
                        continue;
                    }

                    Add(Activator.CreateInstance(pluginType) as IPlugin);
                }
            }
        }
        public void Add(IPlugin plugin)
        {
            ((List<IPlugin>)Plugins).Add(plugin);
        }

        public void Add(string plugin)
        {
            if (string.IsNullOrEmpty(plugin))
            {
                throw new ArgumentNullException("Plugin is null");
            }
            Type t = Type.GetType(plugin);
            IPlugin res = (IPlugin)Activator.CreateInstance(t);
            Add(res);
        }

        public void Clear()
        {
            ((List<IPlugin>)Plugins).Clear();
        }
    }
}

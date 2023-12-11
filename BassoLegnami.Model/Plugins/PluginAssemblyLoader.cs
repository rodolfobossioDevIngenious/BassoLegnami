using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Plugins
{
	public static class PluginAssemblyLoader
	{
		public static List<Assembly> LoadPlugins(string pluginsPath)
		{
			return Directory.GetFiles(pluginsPath, "*.dll")
				.Select(LoadPlugin)
				.ToList();
		}

		public static Assembly LoadPlugin(string pluginLocation)
		{
			PluginLoadContext loadContext = new(pluginLocation);
			return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
		}

		public static IEnumerable<PluginBase> CreateCommands(Assembly assembly)
		{
			int count = 0;

			foreach (Type type in assembly.GetTypes())
			{
				if (typeof(PluginBase).IsAssignableFrom(type) && Activator.CreateInstance(type) is PluginBase result)
				{
					count++;
					yield return result;
				}
			}

			if (count == 0)
			{
				string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
				throw new ApplicationException(
					$"Can't find any type which implements InDocPluginBase in {assembly} from {assembly.Location}.\n" +
					$"Available types: {availableTypes}");
			}
		}

		public static void Seed(Assembly assembly, Data.IUnitOfWork unitOfWork)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (typeof(IPluginSeed).IsAssignableFrom(type) && Activator.CreateInstance(type) is IPluginSeed result)
				{
					result.Seed(unitOfWork);
				}
			}
		}
	}
}

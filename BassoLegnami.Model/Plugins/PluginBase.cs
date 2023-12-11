using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Plugins
{
	public abstract class PluginBase
	{
		protected Data.IUnitOfWork UnitOfWork { get; }

		public abstract string Name { get; }
		public abstract string Description { get; }

		public abstract bool Execute();

		public PluginBase(Data.IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}
	}
}

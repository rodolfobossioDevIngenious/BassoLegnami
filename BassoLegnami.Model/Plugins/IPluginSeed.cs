using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Plugins
{
	public interface IPluginSeed
	{
		void Seed(Data.IUnitOfWork unitOfWork);
	}
}

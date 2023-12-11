using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models
{
	public class AttachedFiles
	{
		public int ReferenceID { get; set; }
		public string FileFolderKey { get; set; }
		public ICollection<Support.File> Files { get; set; }

		public static AttachedFiles CreateNewInstance(int referenceID, string fileFolderKey, ICollection<Support.File> files)
		{
			return new AttachedFiles()
			{
				ReferenceID = referenceID,
				FileFolderKey = fileFolderKey,
				Files = files,
			};
		}

		public AttachedFiles()
		{
			Files = new HashSet<Support.File>();
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public interface IFile
	{
	}

	public class File : In.Core.Models.Auditable, IFile
	{
		public int FileID { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string MimeType { get; set; }
        public DateTime? DatetimeView { get; set; }
        public string UserView { get; set; }
        public int FileFolderID { get; set; }

        [NotMapped]
		public byte[] Content { get; set; }

		public virtual FileFolder FileFolder { get; set; }

		public string GetFilenameForDownload()
		{
			return Name + Path.GetExtension(FileName);
		}

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		private void _Setup(int fileFolderID, string name, string extension, byte[] content)
		{
			if (!extension.StartsWith("."))
			{
				extension = "." + extension;
			}

			FileFolderID = fileFolderID;
			Name = name;
			FileName = Guid.NewGuid().ToString() + extension;
			MimeType = "application/octet-stream";
			Content = content;
		}

		public File(int fileFolderID, string name, string extension, byte[] content)
			: this()
		{
			_Setup(fileFolderID, name, extension, content);
		}

		public File(int fileFolderID, string fileName, Stream inputStream)
			: this()
		{
			using (MemoryStream target = new())
			{
				inputStream.CopyTo(target);
				_Setup(fileFolderID, Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName), target.ToArray());
			}
		}

		public File()
		{
		}
	}
}

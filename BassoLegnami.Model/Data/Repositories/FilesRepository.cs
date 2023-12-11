using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Data.Repositories
{
	public interface IFilesRepository : In.Core.Data.IGenericRepository<Models.Support.File>
	{
		void SaveFile(Models.Support.File file);
		void DeleteFile(Models.Support.File file);
		byte[] GetFile(Models.Support.File file);
		byte[] GetFile(Models.Support.File file, int? height, int? width);
		string GetFileFullName(Models.Support.File file);
		string GetTempFileName();
	}

	public class FilesRepository : In.Core.Data.GenericRepository<Models.Support.File>, IFilesRepository
	{
		public const string FILESWHITELIST_WILDCARD = "*";

		private string _filesPath;
		private string _thumbsPath;

		private string FilesPath => _filesPath ??= _context.Set<Models.Support.Setting>().AsNoTracking().First(r => r.Key == "System.FilesPath").Value;
		private string ThumbsPath => _thumbsPath ??= _context.Set<Models.Support.Setting>().AsNoTracking().First(r => r.Key == "System.ThumbsPath").Value;

		public string GetFileFullName(Models.Support.File file)
		{
			Models.Support.FileFolder fileFolder = _context.Set<Models.Support.FileFolder>().Find(file.FileFolderID);
			string directory = Path.Combine(FilesPath, fileFolder.Path);
			return Path.Combine(directory, file.FileName);
		}

		public void SaveFile(Models.Support.File file)
		{
			Models.Support.FileFolder fileFolder = _context.Set<Models.Support.FileFolder>().Find(file.FileFolderID);
			string directory = Path.Combine(FilesPath, fileFolder.Path);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			string fullFileName = GetFileFullName(file);
			FileStream writer = new(fullFileName, FileMode.Create);
			writer.Write(file.Content, 0, file.Content.Length);
			writer.Close();
		}

		public void DeleteFile(Models.Support.File file)
		{
			string fullFileName = GetFileFullName(file);
			if (File.Exists(fullFileName))
			{
				File.Delete(fullFileName);
			}
		}

		public byte[] GetFile(Models.Support.File file)
		{
			FileStream fileStream = new(GetFileFullName(file), FileMode.Open);
			byte[] output = new byte[fileStream.Length];
			fileStream.Read(output, 0, (int)fileStream.Length);
			fileStream.Close();
			return output;
		}

		public byte[] GetFile(Models.Support.File file, int? height, int? width)
		{
			if (!File.Exists(GetFileFullName(file)))
			{
				return null;
			}

			if (height.HasValue && width.HasValue)
			{
				if (!Directory.Exists(ThumbsPath))
				{
					Directory.CreateDirectory(ThumbsPath);
				}
				string fileName = Path.Combine(ThumbsPath, $"{file.FileID}_{height.Value}_{width.Value}{Path.GetExtension(file.GetFilenameForDownload())}");
				FileStream fileStream = new(fileName, FileMode.Open);
				byte[] output = new byte[fileStream.Length];
				fileStream.Read(output, 0, (int)fileStream.Length);
				fileStream.Close();

				return output;
			}
			else
			{
				return GetFile(file);
			}
		}

		public string GetTempFileName()
		{
			string tempFolder = _context.Set<Models.Support.Setting>().AsNoTracking().First(r => r.Key == "System.TempFolder").Value;
			if (!Directory.Exists(tempFolder))
			{
				Directory.CreateDirectory(tempFolder);
			}

			return Path.Combine(tempFolder, Guid.NewGuid().ToString());
		}

		public FilesRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}
	}
}

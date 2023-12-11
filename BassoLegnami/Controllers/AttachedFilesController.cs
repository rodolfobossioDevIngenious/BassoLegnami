using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BassoLegnami.Model.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BassoLegnami.Controllers
{
	[Route("[controller]/[action]")]
	public class AttachedFilesController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;

		public AttachedFilesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}

		[HttpPost]
		public JsonResult Insert(int referenceID, string fileFolderKey, IFormFile file)
		{
			int? fileFolderID = _unitOfWork.FileFoldersRepository.FirstOrDefault(r => r.Key.Equals(fileFolderKey)).FileFolderID;
			string filesWhiteList = _unitOfWork.SettingsRepository.GetByKey("System.FilesWhiteList").Value;
			if (filesWhiteList != Model.Data.Repositories.FilesRepository.FILESWHITELIST_WILDCARD && (!fileFolderID.HasValue || !$";{filesWhiteList};".Contains(Path.GetExtension(file.FileName), StringComparison.InvariantCultureIgnoreCase)))
			{
				return Json(new { result = false });
			}

			Model.Models.Support.File newFile = new(fileFolderID.Value, file.FileName, file.OpenReadStream());
			switch (fileFolderKey)
			{
				//TODO: migrazione
				//case _unitOfWork.FILEFOLDER_EMAILATTACHMENTS:
				//    //newFile.EmailMessageID = referenceID;
				//    break;
				default:
					return Json(new { result = false });
			}
			_unitOfWork.FilesRepository.Add(newFile);
			_unitOfWork.Save();
			return Json(new
			{
				result = true,
				fileID = newFile.FileID,
				fileName = newFile.GetFilenameForDownload(),
				createdBy = _userManager.FindByIdAsync(newFile.CreatedBy).Result.UserName,
				createdOn = newFile.CreatedOn.ToString(),
				updatedBy = _userManager.FindByIdAsync(newFile.UpdatedBy).Result.UserName,
				updatedOn = newFile.UpdatedOn.ToString(),
			});
		}

		[HttpPost]
		public JsonResult Delete(int fileID)
		{
			_unitOfWork.FilesRepository.Delete(_unitOfWork.FilesRepository.Get(fileID));
			_unitOfWork.Save();
			return Json(new { result = true });
		}

		public FileContentResult Details(int id, int? height, int? width)
		{
			Model.Models.Support.File file = _unitOfWork.FilesRepository.Get(id);
			if (file == null)
			{
				return null;
			}

			byte[] fileStream = _unitOfWork.FilesRepository.GetFile(file, height, width);
			if (fileStream == null)
			{
				return null;
			}

			return File(fileStream, "application/octet-stream", file.GetFilenameForDownload());
		}
	}
}
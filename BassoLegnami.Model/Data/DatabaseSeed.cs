using Elsa.Services;
using In.Core.Models;
using BassoLegnami.Model.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetBox.Extensions;

namespace BassoLegnami.Model.Data
{
    public static class DatabaseSeed
    {
        public static void Seed(IWebHostEnvironment env, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IWorkflowPublisher workflowPublisher, Elsa.Serialization.IContentSerializer contentSerializer)
        {
            if (!unitOfWork.RecordFilterRuleTypesRepository.Any())
            {

            }

            if (!unitOfWork.FileFoldersRepository.Any())
            {
                unitOfWork.FileFoldersRepository.AddRange(new List<Models.Support.FileFolder>()
                {
                    new Models.Support.FileFolder() { Name = "Allegati email", Key = UnitOfWork.FILEFOLDER_EMAILATTACHMENTS, Path = UnitOfWork.FILEFOLDER_EMAILATTACHMENTS },
                });
                unitOfWork.Save();
            }

            if (!unitOfWork.SettingsRepository.Any())
            {
                unitOfWork.SettingsRepository.AddRange(new List<Models.Support.Setting>()
                {
                    new Models.Support.Setting() { Key = "System.FilesPath", Name = "Files path", Value = @"E:\BassoLegnami\Files" },
                    new Models.Support.Setting() { Key = "System.FilesWhiteList", Name = "Files white list", Value = Repositories.FilesRepository.FILESWHITELIST_WILDCARD },
                    new Models.Support.Setting() { Key = "System.ThumbsPath", Name = "Thumbs path", Value = @"E:\BassoLegnami\Files\Thumbs" },
                    new Models.Support.Setting() { Key = "System.GoogleAPIKey", Name = "Google API Key", Value = "" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.TestMode", Name = "EmailTx Test mode", Value = "1" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.Server", Name = "EmailTx Server", Value = "smtp-relay.sendinblue.com" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.ServerPort", Name = "EmailTx Server Port", Value = "587" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.Username", Name = "EmailTx Username", Value = "rory.gennaro@icrimpianti.com" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.Password", Name = "EmailTx Password", Value = "hm9Zkn34MdGA72Is" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.UseSSL", Name = "EmailTx Use SSL", Value = "1" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.SenderName", Name = "EmailTx Sender Name", Value = "ICRI - BassoLegnami" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.SenderEmail", Name = "EmailTx Sender Email", Value = "no-reply@icrimpianti.com" },
                    new Models.Support.Setting() { Key = "TxRx.Tx.EmailTx.TestEmail", Name = "EmailTx Test Email", Value = "alberto.armida@ingenioussrl.it" },
                    new Models.Support.Setting() { Key = "System.TempFolder", Name = "Temporary working folder", Value = @"E:\BassoLegnami\Temp" },
                });
                unitOfWork.Save();
            }

            List<System.Reflection.Assembly> plugins = PluginAssemblyLoader.LoadPlugins(UnitOfWork.PLUGINS_FOLDER);
            plugins.Add(System.Reflection.Assembly.GetExecutingAssembly());

            // load workflows
            foreach (System.Reflection.Assembly plugin in plugins)
            {
                string folderName = $"{plugin.GetName().Name}.Resources.Workflows";
                plugin.GetManifestResourceNames()
                    .Where(r => r.StartsWith(folderName) && r.EndsWith(".json"))
                    .Select(r => new System.IO.StreamReader(plugin.GetManifestResourceStream(r)).ReadToEnd())
                    .ToList()
                    .ForEach(r =>
                    {
                        Elsa.Models.WorkflowDefinition workflowModel = contentSerializer.Deserialize<Elsa.Models.WorkflowDefinition>(r);
                        if (workflowPublisher.GetDraftAsync(workflowModel.DefinitionId).Result == null)
                        {
                            Elsa.Models.WorkflowDefinition output = workflowPublisher.PublishAsync(workflowModel).Result;
                        }
                    });
            }

            if (!unitOfWork.FestivitiesRepository.Any())
            {
                unitOfWork.FestivitiesRepository.AddRange(new List<Models.Support.Festivity>()
                {
                    new Models.Support.Festivity() { Name = "Capodanno", Day = 1, Month = 1, Year = null },
                    new Models.Support.Festivity() { Name = "Epifania", Day = 6, Month = 1, Year = null },
                    new Models.Support.Festivity() { Name = "Anniversario della Liberazione", Day = 25, Month = 4, Year = null },
                    new Models.Support.Festivity() { Name = "Festa del Lavoro", Day = 1, Month = 5, Year = null },
                    new Models.Support.Festivity() { Name = "Festa della Repubblica", Day = 2, Month = 6, Year = null },
                    new Models.Support.Festivity() { Name = "Assunzione", Day = 15, Month = 8, Year = null },
                    new Models.Support.Festivity() { Name = "Ognissanti", Day = 1, Month = 11, Year = null },
                    new Models.Support.Festivity() { Name = "Immacolata Concezione", Day = 8, Month = 12, Year = null },
                    new Models.Support.Festivity() { Name = "Natale", Day = 25, Month = 12, Year = null },
                    new Models.Support.Festivity() { Name = "Santo Stefano", Day = 26, Month = 12, Year = null },
                    new Models.Support.Festivity() { Name = "Pasqua 2022", Day = 17, Month = 4, Year = 2022 },
                    new Models.Support.Festivity() { Name = "Lunedì dell'Angelo 2022", Day = 18, Month = 4, Year = 2022 },
                    new Models.Support.Festivity() { Name = "Pasqua 2023", Day = 9, Month = 4, Year = 2023 },
                    new Models.Support.Festivity() { Name = "Lunedì dell'Angelo 2022", Day = 10, Month = 4, Year = 2023 },
                    new Models.Support.Festivity() { Name = "Pasqua 2024", Day = 31, Month = 3, Year = 2024 },
                    new Models.Support.Festivity() { Name = "Lunedì dell'Angelo 2024", Day = 1, Month = 4, Year = 2024 },
                    new Models.Support.Festivity() { Name = "Pasqua 2025", Day = 20, Month = 4, Year = 2025 },
                    new Models.Support.Festivity() { Name = "Lunedì dell'Angelo 2025", Day = 21, Month = 4, Year = 2025 },
                });
                unitOfWork.Save();
            }
        }
    }
}

using In.Core.Data;
using BassoLegnami.Model.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using In.Core.Configuration;
using BassoLegnami.Model.Data.Repositories;
using BassoLegnami.Model.Models.Log;
using Microsoft.IdentityModel.Protocols;

namespace BassoLegnami.Model.Data
{
	public interface IUnitOfWork : In.Core.Data.IUnitOfWork
	{
		Guid User { get; }
		void SetDetectChanges(bool value);
		void SetUser(Guid user);

		IGenericRepository<Log> LogsRepository { get; }
		IGenericRepository<Error> ErrorsRepository { get; }

		IRecordFilterRulesRepository RecordFilterRulesRepository { get; }
		IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleType> RecordFilterRuleTypesRepository { get; }
		IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleValue> RecordFilterRuleValuesRepository { get; }
		IAuthorizationsRepository AuthorizationsRepository { get; }

		IGenericRepository<FileFolder> FileFoldersRepository { get; }
		IFilesRepository FilesRepository { get; }
		IGenericRepository<Models.EmailMessage> EmailMessagesRepository { get; }
		IGenericRepository<ExternalSystem> ExternalSystemsRepository { get; }
		IGenericRepository<ExternalSystemValue> ExternalSystemValuesRepository { get; }
		ISettingsRepository SettingsRepository { get; }
		IFestivitiesRepository FestivitiesRepository { get; }

		public ICityRepository CityRepository { get; }
		public IGenericRepository<Models.GeographicSupport.Province> ProvinceRepository { get; }
		public IGenericRepository<Models.GeographicSupport.Region> RegionRepository { get; }
		public IGenericRepository<Models.GeographicSupport.RegionalZone> RegionalZoneRepository { get; }
		public IGenericRepository<UnitOfMeasurement> UnitOfMeasurementRepository { get; }
        public IAgentiGiacenzeRepository AgentiGiacenzeRepository { get; }
        IGenericRepository<Clienti> ClientiRepository { get; }
        IGenericRepository<Tabelle> TabelleRepository { get; }
    }
}

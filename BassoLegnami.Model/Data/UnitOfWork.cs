using In.Core.Models;
using BassoLegnami.Model.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BassoLegnami.Model.Models.Log;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Localization;
using Elsa.Services;
using Elsa.Activities.Signaling.Services;
using In.Core.Configuration;
using BassoLegnami.Model.Models.Support;
using In.Core.Data;
using Microsoft.Extensions.Configuration;

namespace BassoLegnami.Model.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string ITALIAN_COUNTRY = "IT";

        public const string ROLE_ADMINISTRATORS = "Administrators";

        public const string PLUGINS_FOLDER = "Plugins";

        public const string FILEFOLDER_EMAILATTACHMENTS = "EmailAttachments";

        private readonly SqlServerApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<ApplicationConfigurations> _applicationConfigurations;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly IHtmlLocalizer<Models.SharedResource> _sharedLocalizer;

        private readonly ISignaler _signaler;

        private IGenericRepository<Log> _logsRepository;
        private IGenericRepository<Error> _errorsRepository;

        private IRecordFilterRulesRepository _recordFilterRulesRepository;
        private IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleType> _recordFilterRuleTypesRepository;
        private IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleValue> _recordFilterRuleValuesRepository;
        private IAuthorizationsRepository _authorizationsRepository;

        private IGenericRepository<FileFolder> _fileFoldersRepository;
        private IFilesRepository _filesRepository;
        private IGenericRepository<Models.EmailMessage> _emailMessagesRepository;
        private IGenericRepository<ExternalSystem> _externalSystemsRepository;
        private IGenericRepository<ExternalSystemValue> _externalSystemValuesRepository;
        private ISettingsRepository _settingsRepository;
        private IFestivitiesRepository _festivitiesRepository;
        private IAgentiGiacenzeRepository _agentiGiacenzeRepository;
        //private IClientiRepository _clientiRepository;
        private IGenericRepository<Clienti> _clientiRepository;

        private ICityRepository _cityRepository;
        private GenericRepository<Models.GeographicSupport.Province> _provinceRepository;
        private GenericRepository<Models.GeographicSupport.Region> _regionRepository;
        private GenericRepository<Models.GeographicSupport.RegionalZone> _regionalZoneRepository;

        private GenericRepository<UnitOfMeasurement> _unitOfMeasurementRepository;

        private IGenericRepository<Tabelle> _tabelleRepository;

        public UnitOfWork(IServiceProvider serviceProvider, IWebHostEnvironment env, IHttpContextAccessor httpContext, IOptions<ApplicationConfigurations> applicationConfigurations, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHtmlLocalizer<Models.SharedResource> sharedLocalizer, ISignaler signaler, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<SqlServerApplicationDbContext>();
            _applicationConfigurations = applicationConfigurations;
            _env = env;
            _httpContext = httpContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _sharedLocalizer = sharedLocalizer;
            _signaler = signaler;
            _configuration = configuration;
        }

        public Guid User => _context.User;

        public void SetUser(Guid user)
        {
            _context.SetUser(user);
        }

        public void SetDetectChanges(bool value)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = value;
        }

        public IGenericRepository<Log> LogsRepository => _logsRepository ??= new GenericRepository<Log>(_httpContext, _context, User);
        public IGenericRepository<Error> ErrorsRepository => _errorsRepository ??= new GenericRepository<Error>(_httpContext, _context, User);

        public IRecordFilterRulesRepository RecordFilterRulesRepository => _recordFilterRulesRepository ??= new RecordFilterRulesRepository(_httpContext, _context, User);
        public IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleType> RecordFilterRuleTypesRepository => _recordFilterRuleTypesRepository ??= new GenericRepository<In.Core.Models.Authorization.RecordFilterRuleType>(_httpContext, _context, User);
        public IGenericRepository<In.Core.Models.Authorization.RecordFilterRuleValue> RecordFilterRuleValuesRepository => _recordFilterRuleValuesRepository ??= new GenericRepository<In.Core.Models.Authorization.RecordFilterRuleValue>(_httpContext, _context, User);
        public IAuthorizationsRepository AuthorizationsRepository => _authorizationsRepository ??= new AuthorizationsRepository(_httpContext, _context, User);

        public IGenericRepository<FileFolder> FileFoldersRepository => _fileFoldersRepository ??= new GenericRepository<FileFolder>(_httpContext, _context, User);
        public IFilesRepository FilesRepository => _filesRepository ??= new FilesRepository(_httpContext, _context, User);
        public IGenericRepository<Models.EmailMessage> EmailMessagesRepository => _emailMessagesRepository ??= new GenericRepository<Models.EmailMessage>(_httpContext, _context, User);
        public IGenericRepository<ExternalSystem> ExternalSystemsRepository => _externalSystemsRepository ??= new GenericRepository<ExternalSystem>(_httpContext, _context, User);
        public IGenericRepository<ExternalSystemValue> ExternalSystemValuesRepository => _externalSystemValuesRepository ??= new GenericRepository<ExternalSystemValue>(_httpContext, _context, User);
        public ISettingsRepository SettingsRepository => _settingsRepository ??= new SettingsRepository(_httpContext, _context, User);
        public IFestivitiesRepository FestivitiesRepository => _festivitiesRepository ??= new FestivitiesRepository(_httpContext, _context, User);
        public ICityRepository CityRepository => _cityRepository ??= new CityRepository(_httpContext, _context, User);
        public IGenericRepository<Models.GeographicSupport.Province> ProvinceRepository => _provinceRepository ??= new GenericRepository<Models.GeographicSupport.Province>(_httpContext, _context, User);
        public IGenericRepository<Models.GeographicSupport.Region> RegionRepository => _regionRepository ??= new GenericRepository<Models.GeographicSupport.Region>(_httpContext, _context, User);
        public IGenericRepository<Models.GeographicSupport.RegionalZone> RegionalZoneRepository => _regionalZoneRepository ??= new GenericRepository<Models.GeographicSupport.RegionalZone>(_httpContext, _context, User);
        public IGenericRepository<UnitOfMeasurement> UnitOfMeasurementRepository => _unitOfMeasurementRepository ??= new GenericRepository<UnitOfMeasurement>(_httpContext, _context, User);
        public IAgentiGiacenzeRepository AgentiGiacenzeRepository => _agentiGiacenzeRepository ??= new AgentiGiacenzeRepository(_httpContext, _context, User);
        public IGenericRepository<Clienti> ClientiRepository => _clientiRepository ??= new GenericRepository<Clienti>(_httpContext, _context, User);
        public IGenericRepository<Tabelle> TabelleRepository => _tabelleRepository ??= new GenericRepository<Tabelle>(_httpContext, _context, User);

        private void _ManageFiles()
        {
            _context.ChangeTracker.Entries().Where(p => p.Entity is File && (p.State == Microsoft.EntityFrameworkCore.EntityState.Added || p.State == Microsoft.EntityFrameworkCore.EntityState.Modified)).ToList()
                .ForEach(f => FilesRepository.SaveFile((File)f.Entity));
            _context.ChangeTracker.Entries().Where(p => p.Entity is File && p.State == Microsoft.EntityFrameworkCore.EntityState.Deleted).ToList()
                .ForEach(f => FilesRepository.DeleteFile((File)f.Entity));
        }

        public int Save()
        {
            _ManageFiles();
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            _ManageFiles();
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using In.Core.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BassoLegnami.Model.Data.Repositories;
using BassoLegnami.Model.Models.GeographicSupport;

namespace BassoLegnami.Model.Data.Repositories
{
	public interface ICityRepository : IGenericRepository<City>
	{
		City Search(string cityName, string cap);
		(double Latitude, double Longitude) GetCityLocation(int cityID);

	}

	public class CityRepository : GenericRepository<City>, ICityRepository
	{
		public CityRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user) : base(httpContext, context, user)
		{
		}

		public City Search(string cityName, string cap)
		{
			City output = null;

			cap = cap.Trim();
			if (cap.Length < 5)
			{
				cap = cap.PadLeft(5, '0');
			}

			IEnumerable<City> cities = FindBy(c => c.CAP == cap).Include(r => r.Province).ToList();
			if (cities.Count() == 1)                    //city found!
			{
				output = cities.First();
			}
			else if (cities.Count() > 1)                //search by city name
			{
				cities = cities.Where(c => c.Name.ToUpper() == cityName.ToUpper());
				if (cities.Count() == 1)
				{
					output = cities.First();
				}
			}
			else
			{
				cap = cap.Substring(0, 3) + "00";       //search by generic cap
				cities = FindBy(c => c.CAP == cap);
				if (cities.Count() == 1)
				{
					output = cities.First();
				}
				else
				{
					cities = FindBy(c => c.Name.ToUpper() == cityName.ToUpper());
					if (cities.Count() == 1)
					{
						output = cities.First();
					}
				}
			}

			return output;
		}

		public (double Latitude, double Longitude) GetCityLocation(int cityID)
		{
			SettingsRepository settingsRepository = new(_httpContext, _context, User);
			GoogleMapsAPI.NET.API.Client.MapsAPIClient client = new(settingsRepository.GetByKey("System.GoogleAPIKey").Value);
			GoogleMapsAPI.NET.API.Geocoding.GeocodingAPI geocoding = new(client);
			GoogleMapsAPI.NET.API.Geocoding.Responses.GeocodeResponse response = null;

			City city = Find(r => r.CityID == cityID);

			if (city.Latitude.HasValue && city.Longitude.HasValue)
			{
				return (city.Latitude.Value, city.Longitude.Value);
			}

			int testCount = 1;
			do
			{
				response = geocoding.Geocode(Find(r => r.CityID == cityID).Name, language: "it");
				if (response.HasErrorMessage)
				{
					testCount++;
					System.Threading.Thread.Sleep(2000);
				}
			} while (testCount <= 5 && response.HasErrorMessage);
			if (response.IsValid)
			{
				if (response.Results.Count() > 0)
				{
					GoogleMapsAPI.NET.API.Geocoding.Results.GeocodeResult result = response.Results.First();
					city.Latitude = result.Geometry.Location.Latitude;
					city.Longitude = result.Geometry.Location.Longitude;
					Update(city, city.CityID);
				}
				else
				{
					throw new ArgumentException();
				}
			}
			else
			{
				throw new ArgumentException();
			}

			return (city.Latitude.Value, city.Longitude.Value);
		}
	}
}

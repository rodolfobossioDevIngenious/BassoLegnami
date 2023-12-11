using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BassoLegnami.Model.Models
{
	public abstract partial class LocalizableObject : In.Core.Models.Auditable
	{
		private bool _changed;
		private string _address;
		private string _cap;
		private string _cityName;
		private string _province;
		private string _country = "IT";

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "Address")]
		[StringLength(100)]
		public string Address
		{
			get { return _address; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (_address != value)
					{
						_changed = true;
					}

					_address = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLower());
				}
			}
		}

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "CAP")]
		[StringLength(5)]
		[RegularExpression("^[0-9]{5}?", ErrorMessageResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), ErrorMessageResourceName = "CAPError")]
		public string CAP
		{
			get { return _cap; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (_cap != value)
					{
						_changed = true;
					}

					_cap = value.Trim().PadLeft(5, '0');
				}
			}
		}

		[NotMapped]
		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "ProvinceName")]
		[StringLength(2)]
		public string ProvinceName
		{
			get
			{
				if (string.IsNullOrEmpty(_province) && CityID != 0)
				{
					_province = City.Province.Abbreviation;
				}
				return _province;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (_province != value)
					{
						_changed = true;
					}

					_province = value.Trim().ToUpper();
				}
			}
		}

		[NotMapped]
		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "CityName")]
		[StringLength(100)]
		public string CityName
		{
			get
			{
				if (string.IsNullOrEmpty(_cityName) && CityID != 0)
				{
					_cityName = City.Name;
				}
				return _cityName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (_cityName != value)
					{
						_changed = true;
					}

					_cityName = value.Trim();
				}
			}
		}

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "Country")]
		[StringLength(2)]
		public string Country
		{
			get { return _country; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (_country != value)
					{
						_changed = true;
					}

					_country = value.Trim().ToUpper();
				}
			}
		}

		public string AddressStreetType { get; set; }
		public string AddressStreet { get; set; }
		public string AddressStreetNumber { get; set; }

		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "CityName")]
		public int CityID { get; set; }

		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "Latitude")]
		public double? Latitude { get; set; }

		[Display(ResourceType = typeof(Resources.Models.LocalizableObject.LocalizableObject), Name = "Longitude")]
		public double? Longitude { get; set; }

		public virtual GeographicSupport.City City { get; set; }

		[NotMapped]
		public string PlainAddress => $"{Address} - {CAP} {CityName} ({ProvinceName} - {Country})";

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> output = new();
			if (string.IsNullOrEmpty(Address))
			{
				output.Add(new ValidationResult(Resources.Models.SharedResource.FieldRequired, new string[] { nameof(Address) }));
			}

			if (string.IsNullOrEmpty(CAP))
			{
				output.Add(new ValidationResult(Resources.Models.SharedResource.FieldRequired, new string[] { nameof(CAP) }));
			}

			if (string.IsNullOrEmpty(ProvinceName))
			{
				output.Add(new ValidationResult(Resources.Models.SharedResource.FieldRequired, new string[] { nameof(ProvinceName) }));
			}

			if (string.IsNullOrEmpty(CityName))
			{
				output.Add(new ValidationResult(Resources.Models.SharedResource.FieldRequired, new string[] { nameof(CityName) }));
			}

			if (string.IsNullOrEmpty(Country))
			{
				output.Add(new ValidationResult(Resources.Models.SharedResource.FieldRequired, new string[] { nameof(Country) }));
			}

			if (_changed && (((Latitude ?? 0) == 0 && (Longitude ?? 0) == 0) || (CityID == 0)))
			{
				Latitude = 0;
				Longitude = 0;

				Data.IUnitOfWork unitOfWork = validationContext.GetService(typeof(Data.IUnitOfWork)) as Data.IUnitOfWork;
				GoogleMapsAPI.NET.API.Client.MapsAPIClient client = new(unitOfWork.SettingsRepository.GetByKey("System.GoogleAPIKey").Value);
				GoogleMapsAPI.NET.API.Geocoding.GeocodingAPI geocoding = new(client);
				GoogleMapsAPI.NET.API.Geocoding.Responses.GeocodeResponse response = null;

				int testCount = 1;
				do
				{
					response = geocoding.Geocode(Country + " " + CityName + " " + CAP + " " + Address, language: "it");
					if (response.HasErrorMessage)
					{
						testCount++;
						System.Threading.Thread.Sleep(2000);
					} //IF
				} while (testCount <= 5 && response.HasErrorMessage);
				if (response.IsValid)
				{
					if (response.Results.Count > 0)
					{
						GoogleMapsAPI.NET.API.Geocoding.Results.GeocodeResult result = response.Results.ToList().First();
						Latitude = result.Geometry.Location.Latitude;
						Longitude = result.Geometry.Location.Longitude;

						GoogleMapsAPI.NET.API.Geocoding.Components.Address postalCode = result.AddressComponents.Find(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.PostalCode));
						GoogleMapsAPI.NET.API.Geocoding.Components.Address locality = result.AddressComponents.Find(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.AdministrativeAreaLevel3));
						GoogleMapsAPI.NET.API.Geocoding.Components.Address province = result.AddressComponents.Find(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.AdministrativeAreaLevel2));
						GoogleMapsAPI.NET.API.Geocoding.Components.Address country = result.AddressComponents.Find(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.Country));

						GeographicSupport.City curCity = unitOfWork.CityRepository.Search(locality?.LongName ?? CityName, postalCode?.ShortName ?? CAP);
						if (curCity == null)
						{
							output.Add(new ValidationResult(Resources.Models.LocalizableObject.LocalizableObject.AddressError, new string[] { nameof(Address), nameof(CAP), nameof(CityName), nameof(ProvinceName) }));
						}
						else
						{
							CityName = curCity.Name;
							CityID = curCity.CityID;
						}

						GoogleMapsAPI.NET.API.Geocoding.Components.Address outputAddressComponent = result.AddressComponents.Find(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.Route));
						try
						{
							AddressStreetNumber = result.AddressComponents.FirstOrDefault(r => r.Types.Contains(GoogleMapsAPI.NET.API.Places.Enums.PlaceResultTypeEnum.StreetNumber))?.LongName ?? "1";
							AddressStreetType = outputAddressComponent.LongName.Split(' ').FirstOrDefault();
							AddressStreet = outputAddressComponent.LongName.Substring(AddressStreetType.Length + 1);
						}
						catch (Exception)
						{
							AddressStreetType = "Via";
							AddressStreet = Address;
						}
						Address = string.Format("{0} {1}, {2}", AddressStreetType, AddressStreet, AddressStreetNumber);
						CAP = (postalCode?.ShortName ?? CAP);
						ProvinceName = province?.ShortName ?? curCity.Province.Abbreviation;
						Country = country?.ShortName.ToUpper() ?? Country.ToUpper();
						_changed = false;
					} //IF
					else
					{
						output.Add(new ValidationResult(Resources.Models.LocalizableObject.LocalizableObject.AddressError, new string[] { nameof(Address), nameof(CAP), nameof(CityName), nameof(ProvinceName) }));
					}
				} //IF
				else
				{
					output.Add(new ValidationResult(Resources.Models.LocalizableObject.LocalizableObject.AddressError, new string[] { nameof(Address), nameof(CAP), nameof(CityName), nameof(ProvinceName) }));
				}
			}
			return output;
		}

		public double DistanceBetween(LocalizableObject obj)
		{
			if (Latitude.HasValue && Longitude.HasValue && (obj.Latitude.HasValue && obj.Longitude.HasValue))
			{
				return _GetPointsDistance(Latitude.Value, Longitude.Value, obj.Latitude.Value, obj.Longitude.Value);
			}
			else
			{
				return double.MaxValue;
			}
		}

		private double _GetPointsDistance(double lat1, double lon1, double lat2, double lon2)
		{
			//Costanti di appoggio.
			const int HEARTH_DIAMETER = 6378137;
			const int ROUND_COEFF = 1000;                            //1 per non arrotondare
			const double DEGREES_TO_RADIANS = Math.PI / 180;

			//Trasforma le coordinate in radianti.
			double lat1Rad = lat1 * DEGREES_TO_RADIANS;
			double lon1Rad = lon1 * DEGREES_TO_RADIANS;
			double lat2Rad = lat2 * DEGREES_TO_RADIANS;
			double lon2Rad = lon2 * DEGREES_TO_RADIANS;

			//Calcola il delta delle coordinate.
			double deltaLat = lat2Rad - lat1Rad;
			double deltaLon = lon2Rad - lon1Rad;

			//Calcola l'angolo al centro.
			double a = Math.Pow(Math.Sin(deltaLat / 2.0), 2.0) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(deltaLon / 2.0), 2.0);
			double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

			//Imposta il valore di ritorno.
			return (Math.Round((HEARTH_DIAMETER * c) / ROUND_COEFF) * ROUND_COEFF);
		}

		public LocalizableObject()
		{
			Latitude = null;
			Longitude = null;
			_changed = false;
		}
	}
}

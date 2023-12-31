﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models
{
	public class SharedResource
	{
		public static string FieldRequired => Resources.Models.SharedResource.FieldRequired;
		public static string MaxLength => Resources.Models.SharedResource.MaxLength;
		public static string Note => Resources.Models.SharedResource.Note;
		public static string InvalidValue => Resources.Models.SharedResource.InvalidValue;
		public static string Anomaly => Resources.Models.SharedResource.Anomaly;
		public static string Number => Resources.Models.SharedResource.Number;
		public static string DocumentType => Resources.Models.SharedResource.DocumentType;
		public static string Ladder => Resources.Models.SharedResource.Ladder;
		public static string PresentRegolarMaintenance => Resources.Models.SharedResource.PresentRegolarMaintenance;
		public static string ExternalAccess => Resources.Models.SharedResource.ExternalAccess;
		public static string InternAccess => Resources.Models.SharedResource.InternAccess;
		public static string Emergency => Resources.Models.SharedResource.Emergency;
		public static string SelectValue => Resources.Models.SharedResource.SelectValue;
		public static string Range => Resources.Models.SharedResource.Range;
		public static string From => Resources.Models.SharedResource.From;
		public static string To => Resources.Models.SharedResource.To;
		public static string Presence => Resources.Models.SharedResource.Presence;
		public static string Absence => Resources.Models.SharedResource.Absence;
		public static string Road => Resources.Models.SharedResource.Road;
		public static string Courtyard => Resources.Models.SharedResource.Courtyard;
		public static string OneSwingWithGlass => Resources.Models.SharedResource.OneSwingWithGlass;
		public static string InvalidLoginAttempt => Resources.Models.SharedResource.InvalidLoginAttempt;
		public static string TwoSwingsWithGlass => Resources.Models.SharedResource.TwoSwingsWithGlass;
		public static string OneSwingWithoutGlass => Resources.Models.SharedResource.OneSwingWithoutGlass;
		public static string TwoSwingsWithoutGlass => Resources.Models.SharedResource.TwoSwingsWithoutGlass;
		public static string Safe => Resources.Models.SharedResource.Safe;
		public static string Unsafe => Resources.Models.SharedResource.Unsafe;
		public static string Fixed => Resources.Models.SharedResource.Fixed;
		public static string Nonslip => Resources.Models.SharedResource.Nonslip;
		public static string BuitIn => Resources.Models.SharedResource.BuitIn;
		public static string Yes => Resources.Models.SharedResource.Yes;
		public static string No => Resources.Models.SharedResource.No;
		public static string Shared => Resources.Models.SharedResource.Shared;
		public static string Other => Resources.Models.SharedResource.Other;
		public static string PrivacyCompliant => Resources.Models.SharedResource.PrivacyCompliant;
		public static string NonPrivacyCompliant => Resources.Models.SharedResource.NonPrivacyCompliant;
		public static string Compliant => Resources.Models.SharedResource.Compliant;
		public static string NonCompliant => Resources.Models.SharedResource.NonCompliant;
		public static string InternalSecurity => Resources.Models.SharedResource.InternalSecurity;
		public static string ExternalSecurity => Resources.Models.SharedResource.ExternalSecurity;
		public static string Good => Resources.Models.SharedResource.Good;
		public static string BrokenOrUnsafe => Resources.Models.SharedResource.BrokenOrUnsafe;
		public static string Likely => Resources.Models.SharedResource.Likely;
		public static string Unlikely => Resources.Models.SharedResource.Unlikely;
		public static string Excluded => Resources.Models.SharedResource.Excluded;
		public static string Regular => Resources.Models.SharedResource.Regular;
		public static string NotRegular => Resources.Models.SharedResource.NotRegular;
		public static string GreaterThan120 => Resources.Models.SharedResource.GreaterThan120;
		public static string LessThan120 => Resources.Models.SharedResource.LessThan120;
		public static string GreaterThan100 => Resources.Models.SharedResource.GreaterThan100;
		public static string LessThan100 => Resources.Models.SharedResource.LessThan100;
		public static string RegularParapet => Resources.Models.SharedResource.RegularParapet;
		public static string NotRegularParapet => Resources.Models.SharedResource.NotRegularParapet;
		public static string GreaterThan30 => Resources.Models.SharedResource.GreaterThan30;
		public static string LessThan30 => Resources.Models.SharedResource.LessThan30;
		public static string BadStateBeenStep => Resources.Models.SharedResource.BadStateBeenStep;
		public static string GoodStateBeenStep => Resources.Models.SharedResource.GoodStateBeenStep;
		public static string OneSide => Resources.Models.SharedResource.OneSide;
		public static string TwoSides => Resources.Models.SharedResource.TwoSides;
		public static string SufficientLighting => Resources.Models.SharedResource.SufficientLighting;
		public static string InsufficientLighting => Resources.Models.SharedResource.InsufficientLighting;
		public static string GoodConditionStructure => Resources.Models.SharedResource.GoodCondictionStructure;
		public static string BrokenStructure => Resources.Models.SharedResource.BrokenStructure;
		public static string RampEndNotSigned => Resources.Models.SharedResource.RampEndNotSigned;
		public static string RampEndSigned => Resources.Models.SharedResource.RampEndSigned;
		public static string RampStartNotSigned => Resources.Models.SharedResource.RampStartNotSigned;
		public static string RampStartSigned => Resources.Models.SharedResource.RampStartSigned;
		public static string GoodCondictionStucture => Resources.Models.SharedResource.GoodCondictionStucture;
		public static string Advice => Resources.Models.SharedResource.Advice;
		public static string BadCondictionStucture => Resources.Models.SharedResource.BadCondictionStucture;
		public static string UndamagedBox => Resources.Models.SharedResource.UndamagedBox;
		public static string BrokenBox => Resources.Models.SharedResource.BrokenBox;
		public static string Security => Resources.Models.SharedResource.Security;
		public static string NotSecurity => Resources.Models.SharedResource.NotSecurity;
		public static string Sure => Resources.Models.SharedResource.Sure;
		public static string NotSure => Resources.Models.SharedResource.NotSure;
		public static string Present => Resources.Models.SharedResource.Present;
		public static string NotPresent => Resources.Models.SharedResource.NotPresent;
		public static string Presents => Resources.Models.SharedResource.Presents;
		public static string NotPresents => Resources.Models.SharedResource.NotPresents;
		public static string PavimentFixed => Resources.Models.SharedResource.PavimentFixed;
		public static string AntiSlip => Resources.Models.SharedResource.AntiSlip;
		public static string Incassed => Resources.Models.SharedResource.Incassed;
		public static string AccessAtticDoor => Resources.Models.SharedResource.AccessAtticDoor;
		public static string AccessAtticManhole => Resources.Models.SharedResource.AccessAtticManhole;
		public static string AuthorizedPersons => Resources.Models.SharedResource.AuthorizedPersons;
		public static string AuthorizedToAll => Resources.Models.SharedResource.AuthorizedToAll;
		public static string OpenSecurity => Resources.Models.SharedResource.OpenSecurity;
		public static string CloseSecurity => Resources.Models.SharedResource.CloseSecurity;
		public static string NA => Resources.Models.SharedResource.NA;
		public static string LowQuantity => Resources.Models.SharedResource.LowQuantity;
		public static string HighQuantity => Resources.Models.SharedResource.HighQuantity;
		public static string MediumQuantity => Resources.Models.SharedResource.MediumQuantity;
		public static string Deepen => Resources.Models.SharedResource.Deepen;
		public static string CustomSecurity => Resources.Models.SharedResource.CustomSecurity;
		public static string CustomSlip => Resources.Models.SharedResource.CustomSlip;
		public static string CustomBuitln => Resources.Models.SharedResource.CustomBuitln;
		public static string DoorWithGlass => Resources.Models.SharedResource.DoorWithGlass;
		public static string TwoDoorWithGlass => Resources.Models.SharedResource.TwoDoorWithGlass;
		public static string DoorWithoutGlass => Resources.Models.SharedResource.DoorWithoutGlass;
		public static string TwoDoorWithoutGlass => Resources.Models.SharedResource.TwoDoorWithoutGlass;
		public static string Higher => Resources.Models.SharedResource.Higher;
		public static string Inferior => Resources.Models.SharedResource.Inferior;
		public static string EqualOrGreater30 => Resources.Models.SharedResource.EqualOrGreater30;
		public static string Inferior30 => Resources.Models.SharedResource.Inferior30;
		public static string GoodCondition => Resources.Models.SharedResource.GoodCondition;
		public static string DamageItem => Resources.Models.SharedResource.DamageItem;
		public static string NotSureMaterial => Resources.Models.SharedResource.NotSureMaterial;
		public static string NotSureObstacles => Resources.Models.SharedResource.NotSureObstacles;
		public static string AbsenceItem => Resources.Models.SharedResource.AbsenceItem;
		public static string PresenceItem => Resources.Models.SharedResource.PresenceItem;
		public static string Insecurities => Resources.Models.SharedResource.Insecurities;
		public static string Incomplete => Resources.Models.SharedResource.Incomplete;
		public static string DistrictHeating => Resources.Models.SharedResource.DistrictHeating;
		public static string Accordant => Resources.Models.SharedResource.Accordant;
		public static string NotAccordant => Resources.Models.SharedResource.NotAccordant;
		public static string NotCarriedOut => Resources.Models.SharedResource.NotCarriedOut;
		public static string CarriedOut => Resources.Models.SharedResource.CarriedOut;
		public static string Door2Swing => Resources.Models.SharedResource.Door2Swing;
		public static string DoorWay2Swing => Resources.Models.SharedResource.DoorWay2Swing;
		public static string ManualGate => Resources.Models.SharedResource.ManualGate;
		public static string AutomaticGate => Resources.Models.SharedResource.AutomaticGate;
		public static string ToBeVerified => Resources.Models.SharedResource.ToBeVerified;
		public static string Flat => Resources.Models.SharedResource.Flat;
		public static string Gradients5 => Resources.Models.SharedResource.Gradients5;
		public static string UpperSlopes5 => Resources.Models.SharedResource.UpperSlopes5;
		public static string SignalPresent => Resources.Models.SharedResource.SignalPresent;
		public static string NotSignalPresent => Resources.Models.SharedResource.NotSignalPresent;
		public static string Greater100cm => Resources.Models.SharedResource.Greater100cm;
		public static string Less100cm => Resources.Models.SharedResource.Less100cm;
		public static string Compliance10cm => Resources.Models.SharedResource.Compliance10cm;
		public static string NotCompliance10cm => Resources.Models.SharedResource.NotCompliance10cm;
		public static string Appropriate => Resources.Models.SharedResource.Appropriate;
		public static string NotAppropriate => Resources.Models.SharedResource.NotAppropriate;
		public static string SlopesLess5 => Resources.Models.SharedResource.SlopesLess5;
		public static string SlopesGreater5 => Resources.Models.SharedResource.SlopesGreater5;
		public static string Suitable => Resources.Models.SharedResource.Suitable;
		public static string Dangerous => Resources.Models.SharedResource.Dangerous;
		public static string MaintenancePresent => Resources.Models.SharedResource.MaintenancePresent;
		public static string MaintenanceAbsence => Resources.Models.SharedResource.MaintenanceAbsence;
		public static string Forbidden => Resources.Models.SharedResource.Forbidden;
		public static string Goods => Resources.Models.SharedResource.Goods;
		public static string NotGood => Resources.Models.SharedResource.NotGood;
		public static string Allowed => Resources.Models.SharedResource.Allowed;
		public static string NotSuitable => Resources.Models.SharedResource.NotSuitable;
		public static string FreeObstacles => Resources.Models.SharedResource.FreeObstacles;
		public static string MaterialPresent => Resources.Models.SharedResource.MaterialPresent;
		public static string Obstacle => Resources.Models.SharedResource.Obstacle;
		public static string LedgeWalls => Resources.Models.SharedResource.LedgeWalls;
		public static string UnsuitableWindows => Resources.Models.SharedResource.UnsuitableWindows;
		public static string Integrated => Resources.Models.SharedResource.Integrated;
		public static string LessThan300 => Resources.Models.SharedResource.LessThan300;
		public static string Between300And1000 => Resources.Models.SharedResource.Between300And1000;
		public static string Between1000And3000 => Resources.Models.SharedResource.Between1000And3000;
		public static string Over3000 => Resources.Models.SharedResource.Over3000;
		public static string InsufficientMaintenance => Resources.Models.SharedResource.InsufficientMaintenance;
		public static string AvaibleDocumentation => Resources.Models.SharedResource.AvaibleDocumentation;
		public static string SummayAnomalies => Resources.Models.SharedResource.SummayAnomalies;
		public static string Entrance => Resources.Models.SharedResource.Entrance;
		public static string StairsAndCorridors => Resources.Models.SharedResource.StairsAndCorridors;
		public static string CommonAttic => Resources.Models.SharedResource.CommonAttic;
		public static string Cellars => Resources.Models.SharedResource.Cellars;
		public static string TechnicalRoom => Resources.Models.SharedResource.TechnicalRoom;
		public static string SolarLastric => Resources.Models.SharedResource.SolarLastric;
		public static string ConciergeRoom => Resources.Models.SharedResource.ConciergeRoom;
		public static string Laundry => Resources.Models.SharedResource.Laundry;
		public static string ServiceArea => Resources.Models.SharedResource.ServiceArea;
		public static string CourtyardState => Resources.Models.SharedResource.CourtyardState;
		public static string Parking => Resources.Models.SharedResource.Parking;
		public static string GreenAreas => Resources.Models.SharedResource.GreenAreas;
		public static string GameAreas => Resources.Models.SharedResource.GameAreas;
		public static string Garage => Resources.Models.SharedResource.Garage;
		public static string EchologicalArea => Resources.Models.SharedResource.EchologicalArea;
		public static string BuildingInterior => Resources.Models.SharedResource.BuildingInterior;
		public static string OutsideBuilding => Resources.Models.SharedResource.OutsideBuilding;
		public static string AbsenceSingular => Resources.Models.SharedResource.AbsenceSingular;
		public static string PresenceSingular => Resources.Models.SharedResource.PresenceSingular;
		public static string AbsenceGeneric => Resources.Models.SharedResource.AbsenceGeneric;
		public static string PresenceGeneric => Resources.Models.SharedResource.PresenceGeneric;
		public static string CompliantPlural => Resources.Models.SharedResource.CompliantPlural;
		public static string NonCompliantPlural => Resources.Models.SharedResource.NonCompliantPlural;
		public static string LessThan90 => Resources.Models.SharedResource.LessThan90;
		public static string EqualOrGreater90 => Resources.Models.SharedResource.EqualOrGreater90;
		public static string UndamagedLightingStructure => Resources.Models.SharedResource.UndamagedLightingStructure;
		public static string DamagedLightingStructure => Resources.Models.SharedResource.DamagedLightingStructure;
		public static string NotSecurityLess1m => Resources.Models.SharedResource.NotSecurityLess1m;
		public static string NotIsPresent => Resources.Models.SharedResource.NotIsPresent;
		public static string IsPresent => Resources.Models.SharedResource.IsPresent;
		public static string AbsentPlural => Resources.Models.SharedResource.AbsentPlural;
		public static string PresentPlural => Resources.Models.SharedResource.PresentPlural;
		public static string ExcludedFemale => Resources.Models.SharedResource.ExcludedFemale;
		public static string PresentSingular => Resources.Models.SharedResource.PresentSingular;
		public static string VideoMonitoringSystemInternal => Resources.Models.SharedResource.VideoMonitoringSystemInternal;
		public static string VideoMonitoringSystemExternal => Resources.Models.SharedResource.VideoMonitoringSystemExternal;
		public static string SafeSingular => Resources.Models.SharedResource.SafeSingular;
		public static string UnsafeSingular => Resources.Models.SharedResource.UnsafeSingular;
		public static string BrokenStructureFemale => Resources.Models.SharedResource.BrokenStructureFemale;
		public static string SuitablePlural => Resources.Models.SharedResource.SuitablePlural;
		public static string NotSuitablePlural => Resources.Models.SharedResource.NotSuitablePlural;
		public static string DamageItemFemale => Resources.Models.SharedResource.DamageItemFemale;
		public static string LowQuantityPlural => Resources.Models.SharedResource.LowQuantityPlural;
		public static string MediumQuantityPlural => Resources.Models.SharedResource.MediumQuantityPlural;
		public static string HighQuantityPlural => Resources.Models.SharedResource.HighQuantityPlural;
		public static string VideoMonitoringSystemAccessBuilding => Resources.Models.SharedResource.VideoMonitoringSystemAccessBuilding;
		public static string VideoMonitoringSystemAccessParking => Resources.Models.SharedResource.VideoMonitoringSystemAccessParking;
		public static string AbsentSingular => Resources.Models.SharedResource.AbsentSingular;
		public static string NotApplicable => Resources.Models.SharedResource.NotApplicable;
		public static string Months => Resources.Models.SharedResource.Months;
		public static string Years => Resources.Models.SharedResource.Years;
		public static string Days => Resources.Models.SharedResource.Days;

	}
}

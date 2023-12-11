using System;
using System.Collections.Generic;
using System.Text;

namespace BassoLegnami.Reports
{
    /// <summary>
    /// Scale mode
    /// </summary>
    public enum PDFScaleMode
    {
        Centimeters = 1,
        Millimeters = 2,
        Twips = 3,
    }

	internal class PrintConversionUtils
	{

        /// <summary>
        /// Convert point unit to another scalemode
        /// </summary>
        /// <param name="point">Input point meaure to convert</param>
        /// <param name="scaleMode">Destination scale mode</param>
        /// <returns>Point converted to new scalemode</returns>
        internal static float ConvertFromPointToScaleModeUnit(float point, Reports.PDFScaleMode scaleMode)
        {
            float output = 0;
            switch (scaleMode)
            {
                case Reports.PDFScaleMode.Centimeters:
                    output = point * 2.54F / 72F;
                    break;
                case Reports.PDFScaleMode.Millimeters:
                    output = point * 25.4F / 72F;
                    break;
                case Reports.PDFScaleMode.Twips:
                    output = point * 20;
                    break;
            } //SWITCH

            return output;
        }

        /// <summary>
        /// Convert scalemode unit to point
        /// </summary>
        /// <param name="scaleModeUnit">Scale mode unit meaure to convert</param>
        /// <param name="scaleMode">Scale mode input measure</param>
        /// <returns>Scalemode unit converted to point</returns>
        internal static float ConvertFromScaleModeUnitToPoint(float scaleModeUnit, Reports.PDFScaleMode scaleMode)
        {
            float output = 0;
            switch (scaleMode)
            {
                case Reports.PDFScaleMode.Centimeters:
                    output = scaleModeUnit / 2.54F * 72F;
                    break;
                case Reports.PDFScaleMode.Millimeters:
                    output = scaleModeUnit / 25.4F * 72F;
                    break;
                case Reports.PDFScaleMode.Twips:
                    output = scaleModeUnit / 20;
                break;
            } //SWITCH

            return output;
        }
	}
}

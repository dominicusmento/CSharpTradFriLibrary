using System;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    internal static class ColorExtension
    {
        internal static (int, int) CalculateCIEFromRGB(int r, int g, int b)
        {
            double red = GammaCorrection(r);
            double green = GammaCorrection(g);
            double blue = GammaCorrection(b);

            // Wide RGB D65 conversion
            // math inspiration: http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
            double X = (red * 0.664511) + (green * 0.154324) + (blue * 0.162028);
            double Y = (red * 0.283881) + (green * 0.668433) + (blue * 0.047685);
            double Z = (red * 0.000088) + (green * 0.072310) + (blue * 0.986039);

            // calculate the xy values from XYZ
            double x = (X / (X + Y + Z));
            double y = (Y / (X + Y + Z));

            int xyX = (int)((x * 65535) + 0.5);
            int xyY = (int)((y * 65535) + 0.5);

            return (xyX, xyY);
        }

        private static double GammaCorrection(double colorTone)
        {
            // gamma correction
            return (colorTone > 0.04045) ? Math.Pow((colorTone + 0.055) / (1.0 + 0.055), 2.4) : (colorTone / 12.92);
        }
    }
}

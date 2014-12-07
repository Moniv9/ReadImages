using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;


namespace ReadImages
{
     public class Imaging : IPicture
     {
          private readonly Bitmap _bmp;

          public Imaging( HttpPostedFileBase file )
          {
               _bmp = new Bitmap( file.InputStream );

          }


          public List<Color> GetTopColorsFromImage()
          {
               var topColors = GetImagePixelsColor();
               return RemoveSimilarColors( topColors );
          }


          private List<Color> GetImagePixelsColor()
          {
               int height = _bmp.Height;
               int width = _bmp.Width;

               var colorList = new Dictionary<Color , int>();

               for (int x = 0 ; x < width ; x++)
                    for (int y = 0 ; y < height ; y++) {
                         var color = _bmp.GetPixel( x , y );

                         if (!IsGrayScale( ref color ) && !IsNearGrayScale( ref color ))//removing reflection
                              AddColor( ref colorList , color );

                    }

               return ConvertIntoColorsList( colorList.OrderByDescending( x => x.Value ) );
          }


          private static void AddColor( ref  Dictionary<Color , int> colorList , Color color )
          {
               if (colorList.ContainsKey( color ))
                    colorList[color]++;
               else
                    colorList.Add( color , 1 );
          }


          private List<Color> RemoveSimilarColors( List<Color> colorList )
          {
               int length = colorList.Count;

               for (int i = 0 ; i < length ; i++)
                    for (int j = i + 1 ; j < length ; j++)

                         if (IsSimilarColor( colorList[i] , colorList[j] ))
                              colorList[j] = Color.Empty;

               return colorList;
          }


          private Boolean IsGrayScale( ref Color color )
          {
               return color.G == color.B && color.G == color.R;
          }

          private Boolean IsNearGrayScale( ref Color color )
          {
               int average = (color.R + color.G + color.B) / 3;
               return Math.Abs( color.R - average ) + Math.Abs( color.G - average ) + Math.Abs( color.B - average ) < 9;
          }

          private Boolean IsSimilarColor( Color color1 , Color color2 )
          {
               if (color1.IsEmpty || color2.IsEmpty)
                    return false;

               int threshold = 40; //RGB color difference

               return Math.Abs( color1.R - color2.R ) < threshold && Math.Abs( color1.G - color2.G ) < threshold ||
               Math.Abs( color1.R - color2.R ) < threshold && Math.Abs( color1.B - color2.B ) < threshold ||
               Math.Abs( color1.G - color2.G ) < threshold && Math.Abs( color1.B - color2.B ) < threshold;

          }


          private List<Color> ConvertIntoColorsList( IEnumerable<KeyValuePair<Color , int>> colorList )
          {
               var topColors = new List<Color>();

               foreach (var n in colorList)
                    topColors.Add( n.Key );

               return topColors;
          }

     }
}
using System.Drawing;
using System.Collections.Generic;

namespace ReadImages
{
     public interface IPicture
     {
          List<Color> GetTopColorsFromImage();
     }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Search2.Model.Rectangle;

namespace Search2.Model.BitmapComparer
{
    public interface IBitmapComparer
    {
        Task<IList<RectangleModel>> CheckerAsync(Bitmap first, Bitmap second, IProgress<int> progress, double threshold);
    }
}
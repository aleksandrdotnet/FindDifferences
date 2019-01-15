using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Search2.Model.Rectangle;
using Search2.Static;

namespace Search2.Model.BitmapComparer
{
    public class TemplateBitmapComparer : IBitmapComparer
    {
        public async Task<IList<RectangleModel>> CheckerAsync(Bitmap first, Bitmap second, IProgress<int> progress, double threshold = 0.98D)
        {
            return await Task.Run(() => Checker(first, second, progress, threshold));
        }

        private IList<RectangleModel> Checker(Bitmap first, Bitmap second, IProgress<int> progress, double threshold)
        {
            var frame = WorkScreen.GetMatrix(first);
            var source = second;

            var list = new List<RectangleModel>();

            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    var template = new Bitmap(frame[i, j].Image.StreamSource);

                    var result = TemplateComparsion(source, template, threshold);

                    var procent = (i * frame.GetLength(1) + j) * 101 / (frame.GetLength(0) * frame.GetLength(1));

                    if (result.Item1)
                    {

                    }
                    else
                    {
                        var rect = new RectangleModel(leftTop: frame[i, j].LeftTop, height: template.Height, width: template.Width);
                        list.Add(rect);
                    }

                    progress.Report(procent);
                }
            }

            progress.Report(0);

            return list;
        }

        private Tuple<bool, IList<RectangleModel>> TemplateComparsion(Bitmap source, Bitmap template, double threshold )
        {
            var fst = new Image<Bgr, byte>(source);
            var scd = new Image<Bgr, byte>(template);

            using (var result = fst.MatchTemplate(scd, TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out var _, out var maxValues, out var minLocations, out var maxLocations);

                if (minLocations.Length != maxLocations.Length)
                    throw new ArgumentException();

                var collection = new List<RectangleModel>();
                foreach (Point point in maxLocations)
                {
                    var rect = new RectangleModel(leftTop: point, height: template.Height, width: template.Width);

                    collection.Add(rect);
                }

                return new Tuple<bool, IList<RectangleModel>>(maxValues[0] > threshold, collection);
            }
        }
    }
}
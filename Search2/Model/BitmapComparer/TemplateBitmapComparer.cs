using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Search2.Model.Rectangle;
using Search2.Static;

namespace Search2.Model.BitmapComparer
{
    public class TemplateBitmapComparer : IBitmapComparer
    {
        public async Task<IList<RectangleModel>> CheckerAsync(Bitmap first, Bitmap second, IProgress<int> progress, double threshold = 0.98D, bool equal = false)
        {
            return await Task.Run(() => Checker(first, second, progress, threshold, equal));
        }

        private IList<RectangleModel> Checker(Bitmap first, Bitmap second, IProgress<int> progress, double threshold,
            bool equal = false)
        {
            var matrix = WorkScreen.GetMatrix(first);
            var source = second;

            var list = new List<RectangleModel>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var template = matrix[i, j].Image;

                    var result = IsIncludes(source, template, threshold);

                    var procent = (i * matrix.GetLength(1) + j) * 101 / (matrix.GetLength(0) * matrix.GetLength(1));

                    if (result == equal)
                    {
                        var rect = new RectangleModel<Bitmap>(leftTop: matrix[i,j].LeftTop, height: template.Height, width: template.Width);
                        rect.SetImage(matrix[i, j].Image);
                        list.Add(rect);
                    }

                    progress.Report(procent);
                }
            }

            progress.Report(0);

            return list;
        }

        private Tuple<bool, IList<RectangleModel<Bitmap>>> TemplateComparsion(Bitmap source, Bitmap template, double threshold )
        {
            var src = source.ToImageGrayByte();
            var tmpl = template.ToImageGrayByte();

            using (var result = src.MatchTemplate(tmpl, TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out var _, out var maxValues, out var minLocations, out var maxLocations);

                if (minLocations.Length != maxLocations.Length)
                    throw new ArgumentException();

                var collection = new List<RectangleModel<Bitmap>>();
                foreach (var point in maxLocations)
                {
                    var rect = new RectangleModel<Bitmap>(leftTop: point, height: template.Height, width: template.Width);
                    rect.SetImage(tmpl.Bitmap);
                    collection.Add(rect);
                }

                return new Tuple<bool, IList<RectangleModel<Bitmap>>>(maxValues[0] > threshold, collection);
            }
        }

        private bool IsIncludes(Bitmap source, Bitmap template, double threshold)
        {
            var src = source.ToImageGrayByte();
            var tmpl = template.ToImageGrayByte();

            using (var result = src.MatchTemplate(tmpl, TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out var _, out var maxValues, out var _, out var _);

                return maxValues[0] > threshold;
            }
        }
    }
}
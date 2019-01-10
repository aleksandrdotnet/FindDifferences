using System;
using System.Drawing;
using System.Windows.Input;
using Caliburn.Micro;
using Emgu.CV;
using Emgu.CV.Structure;
using Search2.Hook;
using Search2.Model;
using Search2.Static;
using Application = System.Windows.Application;
using Point = System.Drawing.Point;

namespace Search2.ViewModel
{
    public class MainViewModel : PropertyChangedBase
    {
        private ElementModel _element = new ElementModel();
        public ElementModel Element
        {
            get => _element;
            set
            {
                if (!Equals(_element, value))
                {
                    _element = value;
                    NotifyOfPropertyChange(() => Element);
                }
            }
        }

        public void ImageOne(object sender, EventArgs e)
        {
            //var dialog = new OpenFileDialog { InitialDirectory = Environment.CurrentDirectory };
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    _first = new Image<Bgr, Byte>(new Bitmap(dialog.OpenFile()));

            //    if (dialog.ShowDialog() == DialogResult.OK)
            //    {
            //        _second = new Image<Bgr, Byte>(new Bitmap(dialog.OpenFile()));


            //        double Threshold = 100;

            //        Image<Bgr, byte> difference = _first.AbsDiff(_second);
            //        difference = difference.ThresholdBinary(new Bgr(Threshold, Threshold, Threshold), new Bgr(0, 255, 0));
            //        difference.Add(_second).Bitmap.Save("Difference.png", ImageFormat.Png);


            //        //Image<Gray, Byte> RefImgGray = _first.Convert<Gray, byte>();
            //        //Image<Gray, Byte> CompImgGray = _second.Convert<Gray, byte>();

            //        ////Compare image and build mask
            //        //Image<Gray, Byte> MaskDifferenceHigh = RefImgGray.Cmp(CompImgGray, CmpType.GreaterThan);
            //        //Image<Gray, Byte> MaskDifferenceLow = RefImgGray.Cmp(CompImgGray, CmpType.LessThan);

            //        ////Draw result
            //        //Image<Bgr, byte> result = _first.CopyBlank();
            //        //result.SetValue(new Bgr(Color.Red), MaskDifferenceHigh);
            //        //result.SetValue(new Bgr(Color.Green), MaskDifferenceLow);
            //        //result.Bitmap.Save("Difference.png", ImageFormat.Png);
            //    }
            //}

            var img1 = new Image<Bgr, Byte>(Environment.CurrentDirectory + @"\1.png");
            var img2 = new Image<Bgr, Byte>(Environment.CurrentDirectory + @"\2.png");

            int maxheight = 0, minheight = 0, maxwidth = 0, minwidth = 0;
            //Find Maximum/Minimum width and minimum/height
            if (img1.Width > img2.Width)
            {
                maxwidth = img1.Width;
                minwidth = img2.Width;
            }
            else
            {
                maxwidth = img2.Width;
                minwidth = img1.Width;
            }

            if (img1.Height > img2.Height)
            {
                maxheight = img1.Height;
                minheight = img2.Height;
            }
            else
            {
                maxheight = img2.Height;
                minheight = img1.Height;
            }

            //cvCopy function respect ROI so you could adjust the ROI of img3/img4 so that the images are in the centre if required
            //but the ROIs must match that of the image being copied
            var img3 = new Image<Bgr, byte>(maxwidth, maxheight)
            {
                ROI = new Rectangle(0, 0, img1.Width, img1.Height)
            }; //create the image
               //Set the ROI to match img to be copied
            CvInvoke.cvCopy(img1, img3, IntPtr.Zero); //Copy images over
                                                      //or
            img1.CopyTo(img3);
            img3.ROI = new Rectangle(0, 0, maxwidth, maxheight);      //Reset the ROI
                                                                      // img3.ROI = new Rectangle(0, 0, minwidth, minheight);

            var img4 = new Image<Bgr, byte>(maxwidth, maxheight)
            {
                ROI = new Rectangle(0, 0, img2.Width, img2.Height)
            };
            CvInvoke.cvCopy(img2, img4, IntPtr.Zero);
            img4.ROI = new Rectangle(0, 0, maxwidth, maxheight);
            // img4.ROI = new Rectangle(0, 0, minwidth, minheight);

            //img3.Add(img4).ToBitmap().Save("Different.png", ImageFormat.Png);


            Image<Bgr, byte> difference = img4.AbsDiff(img3);
            difference = difference.ThresholdBinary(new Bgr(90, 90, 90), new Bgr(255, 255, 255));
            //difference.Bitmap.Save("Difference.png", ImageFormat.Png);

            //var difference = img3.MatchTemplate(img4, TemplateMatchingType.Sqdiff);
            //Image<Bgr, byte> imgSource = new Image<Bgr, byte>(img4.Size);
            //float[,,] matches = difference.Data;
            //for (int y = 0; y < matches.GetLength(0); y++)
            //{
            //    for (int x = 0; x < matches.GetLength(1); x++)
            //    {
            //        double matchScore = matches[y, x, 0];
            //        if (matchScore > Threshold)
            //        {
            //            Rectangle rect = new Rectangle(new Point(x, y), new Size(1, 1));
            //            imgSource.Draw(rect, new Bgr(Color.Red), 1);
            //        }

            //    }

            //}

            //Image<Bgr, byte> source = img4; // Image B 
            //Image<Bgr, byte> template = img3; // Image A 
            //Image<Bgr, byte> imageToShow = source.Copy();

            //using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            //{
            //    double[] minValues, maxValues;
            //    //double minValues = 0, maxValues = 0; 
            //    Point[] minLocations, maxLocations;
            //    //Point minLocations = new Point(), maxLocations = new Point(); 
            //    //CvInvoke.Normalize(result, result, 0, 1, Emgu.CV.CvEnum.NormType.MinMax, Emgu.CV.CvEnum.DepthType.Default, new Mat()); 
            //    //CvInvoke.MinMaxLoc(result, ref minValues, ref maxValues, ref minLocations, ref maxLocations, new Mat()); 
            //    result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            //    // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good. 
            //    if (maxValues[0] > Threshold)
            //    {
            //        // This is a match. Do something with it, for example draw a rectangle around it. 
            //        Rectangle match = new Rectangle(maxLocations[0], template.Size);
            //        //Rectangle match = new Rectangle(maxLocations, template.Size); 
            //        imageToShow.Draw(match, new Bgr(Color.Green), 1);
            //    }
            //}
            

            Element = new ElementModel();
            //Element.Children.Add(new System.Windows.Controls.Image
            //{
            //    Source = WorkScreen.ConvertBitmapToBitmapImage(difference.Bitmap),
            //});
        }

       
        public void Close(object sender, EventArgs e)
        {
            Element.Dispose();

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.IO;
using static GoCreate_Badge.MainWindow;


namespace GoCreate_Badge
{
    class Badge
    {
        private Image badgeComplete;
        private string nameOfBadge;
        private ImageSource profileImage;
        private ImageSource badgeType;
        private string badgePath;

        public Badge(string _nameOfBadge, ImageSource _profileImage, ImageSource _badgeType)
        {
            nameOfBadge = _nameOfBadge;
            profileImage = _profileImage;
            badgeType = _badgeType;

            badgeComplete = new Image();

            //Name text for badge
            FormattedText badgeName = new FormattedText(nameOfBadge, 
                new CultureInfo("en-us"),FlowDirection.LeftToRight, 
                new Typeface(new FontFamily("Gotham"), FontStyles.Normal,
                FontWeights.Bold, FontStretches.Normal),
                24, 
                Brushes.Black,
                1.0);

            badgeName.MaxTextWidth = 158;
            badgeName.MaxTextHeight = 62;
            badgeName.TextAlignment = TextAlignment.Center;

            
            //Badges include a date since joining which will continuously change.
            string sinceYear = "Since " + DateTime.Now.Year.ToString();

            //Years since being with GoCreate text.
            FormattedText timeWithText = new FormattedText(sinceYear,
                new CultureInfo("en-us"), FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Gotham"), FontStyles.Normal,
                FontWeights.Light, FontStretches.Normal),
                9,
                Brushes.Black,
                1.0);

            timeWithText.MaxTextWidth = 51;
            timeWithText.MaxTextHeight = 26;
            timeWithText.TextAlignment = TextAlignment.Left;

            //Draws up images and text to bitmap.
            DrawingVisual badgeVisual = new DrawingVisual();
            DrawingContext badgeContext = badgeVisual.RenderOpen();

            badgeContext.DrawImage(badgeType, new Rect(0, 0, 204, 324));
            badgeContext.DrawImage(profileImage, new Rect(18, 97, 77, 98));
            badgeContext.DrawText(badgeName, new Point(23, 32));
            badgeContext.DrawText(timeWithText, new Point(102, 166));
            badgeContext.Close();

            //Renders new bitmap image.
            RenderTargetBitmap badgeBMP = new RenderTargetBitmap(320, 507, 150, 150, PixelFormats.Pbgra32);
            badgeBMP.Render(badgeVisual);

            //Assigns new image.
            badgeComplete.Source = badgeBMP;

            //Saves png file to  file system
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(badgeBMP));

            string badgeDirectory = @"C:\Temp\badges\Complete\";
            badgePath = badgeDirectory + nameOfBadge + ".png";
            using (Stream fs = File.Create(badgePath))
            {
                png.Save(fs);
            }
        }

        public string getBadgePath() => badgePath;
        public string getBadgeName() => nameOfBadge;

        public Image getCompleteImage() => badgeComplete;

    }
}

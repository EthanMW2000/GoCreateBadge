using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Xps.Packaging;
using System.Resources;
using static GoCreate_Badge.Badge;
using System.Drawing.Printing;

namespace GoCreate_Badge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Badge newBadge;
        bool profilePhotoSelection = false;
        string profilePhotoPath = " ";

        public MainWindow()
        {
            InitializeComponent();
        }

        //Will send document to print dialog. Needs to be completed. 
        private void printButton_Click(object sender, RoutedEventArgs e)
        {

            if(!CombineImages(ref newBadge))
            {
                return;
            }


            PrintDialog pd = new PrintDialog();

            if (pd.ShowDialog() == true)
            {
                Image tmpImage = new Image();
                tmpImage.Stretch = Stretch.Uniform;
                tmpImage.Width = pd.PrintableAreaWidth;
                tmpImage.Source = this.newBadge.getCompleteImage().Source;
                tmpImage.Measure(new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight));
                tmpImage.Arrange(new Rect(new Point(0, 0), tmpImage.DesiredSize));

                pd.PrintVisual(tmpImage, newBadge.getBadgeName());
            }

        }

        private void badgeType_Checked(object sender, RoutedEventArgs e)
        {
        }

        //Opens inPhoto Application
        private void takeImageButton_Click(object sender, RoutedEventArgs e)
        {
            profilePhotoSelection = true;
            Process camProcess = new Process();
            camProcess.StartInfo.FileName = @"C:\Program Files (x86)\inPhoto ID Webcam\inPhoto.exe";
            camProcess.Start();
        }

        //Allows user to select a previously taken photo
        private void selectImageButton_Click(object sender, RoutedEventArgs e)
        {
            profilePhotoSelection = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            openFileDialog.InitialDirectory = "C:\\Temp\\badges";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                profilePhotoPath = filename;
            }
        }

        //Readys the images and name for the badge to be put in a new bitmap.
        private bool CombineImages(ref Badge newBadge)
        {
            string badgeName = nameBox.Text;
            ImageSource profileImage;
            

            //If true will use photo recently taken if false use photo selected.
            if (profilePhotoSelection)
            {
                //This will set a directory for the GetFiles function and search by last modified to give the newly take photo.
                var directory = new DirectoryInfo("C:\\Temp\\badges");
                var profilePhotoPath = (from f in directory.GetFiles()
                                        orderby f.LastWriteTime descending
                                        select f).First();
                profileImage = new BitmapImage(new Uri(profilePhotoPath.FullName));
            }
            else
            {
                if (profilePhotoPath == " ")
                {
                    MessageBox.Show("Select or take a photo first!", "Photo Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                profileImage = new BitmapImage(new Uri(profilePhotoPath));
            }
            

            //Badge type image will change depending on which selected choice was made.
            if (memberRadio.IsChecked == true)
            {
                string memberFileName = "member.png";
                string memberPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Badges\", memberFileName);
                ImageSource memberImage = new BitmapImage(new Uri(memberPath));

                newBadge = new Badge(badgeName, profileImage, memberImage);
            }   
            else if (mentorRadio.IsChecked == true)
            {
                string mentorFileName = "mentor.png";
                string mentorPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Badges\", mentorFileName);
                ImageSource mentorImage = new BitmapImage(new Uri(mentorPath));

                newBadge = new Badge(badgeName, profileImage, mentorImage);
            }
            else if (saRadio.IsChecked == true)
            {
                string saFileName = "sa.png";
                string saPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Badges\", saFileName);
                ImageSource saImage = new BitmapImage(new Uri(saPath));

                newBadge = new Badge(badgeName, profileImage, saImage);
            }
            else if (staffRadio.IsChecked == true)
            {
                string staffFileName = "staff.png";
                string staffPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Badges\", staffFileName);
                ImageSource staffImage = new BitmapImage(new Uri(staffPath));

                newBadge = new Badge(badgeName, profileImage, staffImage);
            }
            else
            {
                MessageBox.Show("Select a badge type, try again.", "Badge Type Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }



            return true;
        }

        
    }
}

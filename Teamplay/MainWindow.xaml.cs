using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing.Printing;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspose.Imaging;
using Aspose.Imaging.ImageOptions;
using Aspose.Words;
using Aspose.BarCode;
using Aspose.BarCode.Generation;
using Microsoft.VisualBasic;

namespace Teamplay
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
             
            
        }
        private System.Drawing.Point? _p;
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void MinClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaxClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState.ToString() == "Normal")
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
        public void ToImage(Canvas canvas, string filename)
        {
            try
            {
                imgs = null;
                canvas.LayoutTransform = null;  //обнуляем маштабировние если было


                //качество изображения
                double dpiw = (Convert.ToDouble(width.Text) / 0.26458) * 2;
                double dpih = (Convert.ToDouble(height.Text) / 0.26458) * 2;
                double scalew = dpiw / Convert.ToDouble(touch.Text) + 10;
                double scaleh = dpih / Convert.ToDouble(touch.Text) + 10;

                System.Windows.Size size = canvas.RenderSize;
                RenderTargetBitmap image = new RenderTargetBitmap((int)(size.Width * scalew), (int)(size.Height * scaleh), dpiw, dpih, PixelFormats.Pbgra32);

                canvas.Measure(size);
                canvas.Arrange(new Rect(size)); // This is important      




                image.Render(canvas);
                //FileInfo fileInfo = new FileInfo(filename);
                //fileInfo.Delete();
                //Thread.Sleep(100);

                    PngBitmapEncoder encoder = new PngBitmapEncoder(); 
                encoder.Frames.Add(BitmapFrame.Create(image));
                File.Delete(filename); 
                using (FileStream file = File.Create(filename))
                {
                    encoder.Save(file);
                    file.Position = 0;
                    file.Close();

                }
                //encoder.Preview = image;
                //BitmapSource bit = encoder.Preview;


                /*FileStream stream = new FileStream(filename, FileMode.Create);
                
                encoder.Save(stream);

                stream.Close();*/
                    
                
                Thread.Sleep(1000);
                

            }catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                
            }

        }
        

        private void updateDio(object sender, RoutedEventArgs e)
        {

        }
        private void canvas_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        string ras;
        private void viewbtn(object sender, RoutedEventArgs e)
        {
            imgs.Source = null;
            foreach (var item in canvas.Children)
            {
                
                System.Windows.Controls.Border bor = item as System.Windows.Controls.Border;
                if(bor.BorderBrush == System.Windows.Media.Brushes.Red)
                bor.BorderBrush = null;
            }
            if (width.Text == "" || height.Text == "" || (height.Text == "" && width.Text == ""))
            {
                width.Text = "87";
                height.Text = "90";
            }
            new wind().Show();
            ToImage(canvas, Environment.CurrentDirectory + "\\Etick\\images.png");
            imgs = new System.Windows.Controls.Image();
            Thread.Sleep(1000);
            imgs.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Etick\\images.png"));
            imagepanel.Children.Add(imgs);
        
                
                
                //TextBlock box = (TextBlock)combo.SelectedItem;
            /*double w = Convert.ToDouble(width.Text) / 0.26458;
            double h = Convert.ToDouble(height.Text) / 0.26458;*/
            /*using (FileStream imgStream = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
            {
                var yourImage = resizeImage(System.Drawing.Image.FromStream(imgStream), new System.Drawing.Size(((int)w), ((int)h)));

                yourImage.Save(Environment.CurrentDirectory + $"\\Etick\\images.{box.Text}");
            }*/
            /*switch (box.Text)
                {
                    case "BMP":
                        using (FileStream imgStreambmp = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "bmp";

                            var yourImagebmp = resizeImage(System.Drawing.Image.FromStream(imgStreambmp), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImagebmp.Save(Environment.CurrentDirectory + "\\Etick\\images.bmp", ImageFormat.Bmp);
                        }
                        
                            break;
                    case "PNG":
                        using (FileStream imgStreampng = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "png";
                            var yourImagepng = resizeImage(System.Drawing.Image.FromStream(imgStreampng), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImagepng.Save(Environment.CurrentDirectory + "\\Etick\\images.png");

                        }
                        break;
                    case "JPG":
                        using (FileStream imgStreamjpg = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "jpg";
                            var yourImagejpg = resizeImage(System.Drawing.Image.FromStream(imgStreamjpg), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImagejpg.Save(Environment.CurrentDirectory + "\\Etick\\images.jpg", ImageFormat.Jpeg);

                        }
                        break;
                    case "TIFF":

                        using (FileStream imgStreamtiff = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "tiff";
                            var yourImagetiff = resizeImage(System.Drawing.Image.FromStream(imgStreamtiff), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImagetiff.Save(Environment.CurrentDirectory + "\\Etick\\images.tiff", ImageFormat.Tiff);

                        }
                        break;

                    case "EXIF":
                        using (FileStream imgStreamexif = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "exif";
                            var yourImageexif = resizeImage(System.Drawing.Image.FromStream(imgStreamexif), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImageexif.Save(Environment.CurrentDirectory + "\\Etick\\images.exif", ImageFormat.Exif);
                            imgStreamexif.Position = 5;
                            imgStreamexif.Dispose();
                        }
                        System.Windows.MessageBox.Show(ras);
                        break;

                    case "GIF":
                        using (FileStream imgStreamgif = File.OpenRead(Environment.CurrentDirectory + "\\Etick\\images.png"))
                        {

                            ras = "gif";
                            var yourImagegif = resizeImage(System.Drawing.Image.FromStream(imgStreamgif), new System.Drawing.Size(((int)w), ((int)h)));
                            yourImagegif.Save(Environment.CurrentDirectory + "\\Etick\\images.gif", ImageFormat.Gif);
                            imgStreamgif.Position = 6;
                            imgStreamgif.Dispose();
                        }
                        break;


                }*/
                /*imgs.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Etick\\images.png"));*/







        }
        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new Bitmap(imgToResize, size));
        }
        private void printbtn(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(ras);
            System.Windows.Forms.PrintDialog printDialog1 = new System.Windows.Forms.PrintDialog();
             System.Drawing.Printing.PrintDocument Document = new System.Drawing.Printing.PrintDocument();
             Document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Document_PrintPage);
             DialogResult result = printDialog1.ShowDialog();
             if (result == System.Windows.Forms.DialogResult.OK)
             {
                 Document.Print();
             }  
        }
        void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            e.Graphics.DrawImage(System.Drawing.Image.FromFile(Environment.CurrentDirectory + $"\\Etick\\images1.{ras}"), new System.Drawing.Point(0,0));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int i = 0;
            canvas.ClipToBounds = true;
            foreach(var item in panelbutton.Children)
            {
                var bor = item as System.Windows.Controls.Border;
                var but = bor.Child as System.Windows.Controls.Button;
                but.Uid = $"{i}";
                but.Click += But_Click;
                i++;
            }
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            double sredwidth = canvas.Width / 2;
            double sredheight = canvas.Height / 2;
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            switch (btn.Uid)
            {
                
               
                case "0":
                    System.Windows.Controls.Border stacktext = new System.Windows.Controls.Border();
                    stacktext.BorderThickness = new Thickness(5);
                    stacktext.Background = System.Windows.Media.Brushes.Transparent;
                    stacktext.Padding = new Thickness(5);
                    System.Windows.Controls.TextBox text = new System.Windows.Controls.TextBox() { FontSize = 20};
                    
                    stacktext.MouseEnter += Control_MouseEnter;
                    stacktext.MouseLeave += Control_MouseLeave;
                    stacktext.MouseDown += Control_MouseDown;
                    stacktext.MouseMove += Control_MouseMove;
                    stacktext.MouseUp += Control_MouseUp;
                    stacktext.Child = text;

                    Canvas.SetLeft(stacktext, sredwidth);
                    Canvas.SetTop(stacktext, sredheight);

                    canvas.Children.Add(stacktext);
                    break;

                case "1":
                    System.Windows.Controls.Border stackstak = new System.Windows.Controls.Border();
                    
                    stackstak.BorderThickness = new Thickness(5);
                    stackstak.Background = System.Windows.Media.Brushes.Transparent;
                    stackstak.Padding = new Thickness(5);
                    System.Windows.Controls.Border stak = new System.Windows.Controls.Border() { BorderThickness = new Thickness(1.5),BorderBrush = System.Windows.Media.Brushes.Black,Width=150,Height=150};
                    stak.Background = System.Windows.Media.Brushes.Transparent;
                    stackstak.Child = stak;
                    stackstak.MouseEnter += Control_MouseEnter;
                    stackstak.MouseLeave += Control_MouseLeave;
                    stackstak.MouseDown += Control_MouseDown;
                    stackstak.MouseMove += Control_MouseMove;
                    stackstak.MouseUp += Control_MouseUp;
                    Canvas.SetLeft(stackstak, sredwidth);
                    Canvas.SetTop(stackstak, sredheight);

                    canvas.Children.Add(stackstak);
                    break;

                case "2":
                    System.Windows.Controls.Border stackline = new System.Windows.Controls.Border();
                    stackline.BorderThickness = new Thickness(5);
                    stackline.Background = System.Windows.Media.Brushes.Transparent;
                    stackline.Padding = new Thickness(5);
                    Line line = new Line();
                    line.X1 = 100;
                    line.X2 = -100;
                    line.Stroke = System.Windows.Media.Brushes.Black;
                    stackline.MouseEnter += Control_MouseEnter;
                    stackline.MouseLeave += Control_MouseLeave;
                    stackline.MouseDown += Control_MouseDown;
                    stackline.MouseMove += Control_MouseMove;
                    stackline.MouseUp += Control_MouseUp;
                    stackline.Child = line;
                    Canvas.SetLeft(stackline, sredwidth);
                    Canvas.SetTop(stackline, sredheight);
                    canvas.Children.Add(stackline);
                    break;

                case "3":
                    System.Windows.Controls.Border stackimg = new System.Windows.Controls.Border();
                    stackimg.BorderThickness = new Thickness(5);
                    stackimg.Background = System.Windows.Media.Brushes.Transparent;
                    stackimg.Padding = new Thickness(5);
                    stackimg.Width = 150;
                    stackimg.Height = 150;
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                     // Default file name
                    dialog.DefaultExt = ".png"; // Default file extension
                    dialog.Filter = "Text documents (.png)|*.png"; // Filter files by extension
                    bool? result = dialog.ShowDialog();
                    if (result == true)
                    {
                        System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                        img.Source = new BitmapImage(new Uri(dialog.FileName));
                        img.Width = 150;
                        stackimg.MouseEnter += Control_MouseEnter;
                        stackimg.MouseLeave += Control_MouseLeave;
                        stackimg.MouseDown += Control_MouseDown;
                        stackimg.MouseMove += Control_MouseMove;
                        stackimg.MouseUp += Control_MouseUp;
                        Canvas.SetLeft(stackimg, sredwidth);
                        Canvas.SetTop(stackimg, sredheight);
                        stackimg.Child = img;
                        canvas.Children.Add(stackimg);
                    }
                    break;

                case "4":
                    System.Windows.Controls.Border stackbarimg = new System.Windows.Controls.Border();
                    stackbarimg.BorderThickness = new Thickness(5);
                    stackbarimg.Width = 100;
                    stackbarimg.Height = 100;
                    stackbarimg.Background = System.Windows.Media.Brushes.Transparent;
                    stackbarimg.Padding = new Thickness(5);
                    string a = Interaction.InputBox("Введите кодированное слово", Title: "Cлово");
                    using (var generator = new BarcodeGenerator(EncodeTypes.UPCE, a))
                    {
                        // Set parameters
                        generator.Parameters.Barcode.XDimension.Millimeters *= 2;
                        generator.Parameters.Barcode.CodeTextParameters.Location = CodeLocation.Below;

                        // Generate image
                        Bitmap res = generator.GenerateBarCodeImage();

                        res.Save(Environment.CurrentDirectory + "/barcode.png");
                        

                    }
                    System.Windows.Controls.Image barimg = new System.Windows.Controls.Image();
                    barimg.Width = stackbarimg.Width;
                    
                    barimg.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/barcode.png"));
                    stackbarimg.MouseEnter += Control_MouseEnter;
                    stackbarimg.MouseLeave += Control_MouseLeave;
                    stackbarimg.MouseDown += Control_MouseDown;
                    stackbarimg.MouseMove += Control_MouseMove;
                    stackbarimg.MouseUp += Control_MouseUp;
                    Canvas.SetLeft(stackbarimg, sredwidth);
                    Canvas.SetTop(stackbarimg, sredheight);
                    stackbarimg.Child = barimg;
                    canvas.Children.Add(stackbarimg);
                    break;
            }
        }

        private void Control_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _canMove = false;
            e.MouseDevice.Capture(null);
        }

        private void Control_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            if (_canMove)
            {
                System.Windows.Controls.Border control = sender as System.Windows.Controls.Border;
                if (e.MouseDevice.Captured == control)
                {
                    System.Windows.Point p = e.MouseDevice.GetPosition(this);
                    Canvas.SetLeft(control, p.X - _offsetPoint.X / 2);
                    Canvas.SetTop(control, p.Y - _offsetPoint.Y / 2);
                }
            }
        }

        

        int countZ = 0;
        bool _canMove = false;
        System.Windows.Point _offsetPoint = new System.Windows.Point(0,0);
        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(var item in canvas.Children)
            {
                System.Windows.Controls.Border bar = item as System.Windows.Controls.Border;
                bar.BorderBrush = null;
            }
            System.Windows.Controls.Border control = sender as System.Windows.Controls.Border;
            control.BorderBrush = System.Windows.Media.Brushes.Red;
            _canMove = true;
            countZ++;
            Grid.SetZIndex(control, countZ);
            System.Windows.Point posCursor = e.MouseDevice.GetPosition(this);
            _offsetPoint = new System.Windows.Point(posCursor.X - control.Margin.Left, posCursor.Y -control.Margin.Top);
            e.MouseDevice.Capture(control);
        }

        private void Control_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Border control = sender as System.Windows.Controls.Border;
            if (control.BorderBrush != System.Windows.Media.Brushes.Red)
            {
                control.BorderBrush = null;
            }
        }

        private void Control_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Border control = sender as System.Windows.Controls.Border;
            if (control.BorderBrush != System.Windows.Media.Brushes.Red)
            {
                control.BorderBrush = System.Windows.Media.Brushes.Aqua;
            }
        }

        private void control_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Windows.Controls.Border bord = new System.Windows.Controls.Border();
                foreach (var item in canvas.Children)
                {
                    System.Windows.Controls.Border bor = item as System.Windows.Controls.Border;
                    if (bor.BorderBrush == System.Windows.Media.Brushes.Red)
                    {
                        bord = bor;
                        break;

                    }
                }

                bord.Width = Convert.ToDouble(widthel.Text);
                bord.Height = Convert.ToDouble(heightel.Text);

            }
            catch
            { }
        }
    }
    
}

using System;
using System.Collections;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sph_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ArrayList m_refined_segment_display_list = null;
        public NGraphicsContext m_gc = null;
        public NPointCloud m_point_cloud = new NPointCloud();
        public bool m_cart_display = true;
        public bool m_polar_origin_enter_mode = false;
        public MainWindow()
        {
            InitializeComponent();
            DisplayAxis(m_drawing_canvas);
            m_gc = new NGraphicsContext(m_drawing_canvas, 0, 0, 1, 1);

        }

        public void ClearAll()
        {
            m_drawing_canvas.Children.Clear();
            DisplayAxis(m_drawing_canvas);
              m_cart_display = true;
            m_polar_origin_enter_mode = false;
        }
        private void DisplayAxis(Canvas can)
        {
            return;
            Point x_st = new Point(), y_st = new Point(), x_end = new Point(), y_end = new Point();
            x_st.X = 0.0;
            x_end.X = can.Width;
            x_st.Y = can.Height / 2.0;
            x_end.Y = can.Height / 2.0;
            DisplayLine(can, x_st, x_end, Colors.Black, 3);
            y_st.X = can.Width / 2.0;
            y_end.X = can.Width / 2.0;
            y_st.Y = 0.0;
            y_end.Y = can.Height;
            DisplayLine(can, y_st, y_end, Colors.Blue, 3);

        }
        private void DisplayText(Canvas can, double x, double y, string text, Color color)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(color);
            textBlock.Width = 10;
            Canvas.SetLeft(textBlock, x + 4);
            Canvas.SetTop(textBlock, y + 4);
            can.Children.Add(textBlock);
        }


        void DisplayLine(Canvas can, Point st_point, Point end_point, Color color, double thickness)
        {
            Line ln1 = new Line();
            ln1.X1 = st_point.X;
            ln1.Y1 = st_point.Y;
            ln1.X2 = end_point.X;
            ln1.Y2 = end_point.Y;
            ln1.Stroke = new SolidColorBrush(color);
            ln1.StrokeThickness = thickness;
            m_drawing_canvas.Children.Add(ln1);
        }

        public void WriteFile()
        {
            System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"D:\TestData\Point150.pts");
            string line = "100";
            file.WriteLine(line);
            int y = 0;
            int x = 0;
            int z = 0;
            for(z = 0; z < 50; z ++)
            {
                y = 0;
                x = 0;
                for(x = 0; x < 50; x++)
                {
                    line = string.Format("{0} {1} {2} {3} {4} {5} {6}", (double)x, (double)y, (double)z, 0, 0, 0, 0);
                    file.WriteLine(line);
                }
                for  (y = 0; y < 50; y++)
                {
                    line = string.Format("{0} {1} {2} {3} {4} {5} {6}", (double)x, (double)y, (double)z, 0, 0, 0, 0);
                    file.WriteLine(line);
                }
                for (x =50; x > 0; x--)
                {
                    line = string.Format("{0} {1} {2} {3} {4} {5} {6}", (double)x, (double)y, (double)z, 0, 0, 0, 0);
                    file.WriteLine(line);
                }
                for (y = 50; y > 0; y--)
                {
                    line = string.Format("{0} {1} {2} {3} {4} {5} {6}", (double)x, (double)y, (double)z, 0, 0, 0, 0);
                    file.WriteLine(line);
                }
            }
            z = 0;
            for (z = 0; z < 51; z += 50)
            {
                for (x = 1; x < 50; x++)
                {
                    for (y = 1; y < 50; y++)
                    {
                        line = string.Format("{0} {1} {2} {3} {4} {5} {6}", (double)x, (double)y, (double)z, 0, 0, 0, 0);
                        file.WriteLine(line);
                    }
                }
            }
            file.Close();
        }

        public void PopulatePointCloud(NPointCloud pnt_cloud, string file_name )
        {
            if (file_name != null)
            {
                this.Title = file_name;
                string[] lines = System.IO.File.ReadAllLines(file_name);
                //         MessageBox.Show(@"All lines are read");
                char[] spearator = { ',', ' ' };
                bool first_line = true;
                double x, y, z;
                foreach (string line in lines)
                {
                    if (first_line == true)   // skip first line
                    {
                        first_line = false;
                    }
                    else
                    {
                        string[] strlist = line.Split(spearator,
                                                 StringSplitOptions.RemoveEmptyEntries);
                        x = Convert.ToDouble(strlist[0]);
                        y = Convert.ToDouble(strlist[1]);
                        z = Convert.ToDouble(strlist[2]);
                        NPoint pt = new NPoint(x, y, z);
                        pnt_cloud.AddPoint(pt);
                    }
                }
            }
            else
            {
                MessageBox.Show("Empty OR Wrong file name");
            }

        }

        private void PTS_Open_Click(object sender, RoutedEventArgs e)
        {
            
            m_point_cloud.Clear();
            m_drawing_canvas.Children.Clear();

            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();


            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                PopulatePointCloud(m_point_cloud, openFileDlg.FileName);
                m_point_cloud.DisplayPointsAsPolyLine();
            }

          
        }

        private void m_file_create_Click(object sender, RoutedEventArgs e)
        {
            WriteFile();
        }

        private void RefineBoundary(object sender, RoutedEventArgs e)
        {
            if(m_refined_segment_display_list != null)
            {
                NGraphicsContext gc = NGraphicsUtilities.GetCurrentGC();
                gc.HideChildren(m_refined_segment_display_list);
            }
            bool spike_removal_flag = false;
            spike_removal_flag = NGraphicsUtilities.IsSpikeRemovalRequired();
            double overlap_fraction = NGraphicsUtilities.GetOverlapTolerance();
            bool collinear_removal_flag = NGraphicsUtilities.IsRemoveCollinearPoints();
            m_refined_segment_display_list = m_point_cloud.RefineBoundaryLineAndDisplay(spike_removal_flag, overlap_fraction, collinear_removal_flag);
        }

    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections;
using System.Windows.Media.Imaging;

namespace sph_test
{
    public class NUtilities
    {
        public static ArrayList RemoveDuplicateIndexes(ArrayList index_list)
        {
            ArrayList new_list = new ArrayList();
            int count = index_list.Count;
            int cur_value = -1;
            int new_value = -1;
            for(int i = 0; i < count; i++)
            {
                cur_value = (int)(index_list[i]);
                for(int j = i+1; j < count; j++)
                {
                    new_value = (int)(index_list[j]);
                    if(new_value != cur_value)
                    {
                        i = j - 1;
                        break;
                    }
 
                }
                new_list.Add(cur_value);
            }
            return new_list;
        }
    }
    public class NGraphicsContext
    {
        public Canvas m_canvas;
        public double m_x_offset = 0, m_y_offset = 0, m_x_fac = 1, m_y_fac = 1;
        public NGraphicsContext(Canvas can, double x_offset = 0, double y_offset = 0, double x_fac = 1, double y_fac = 1)
        {
            m_canvas = can;
            m_x_offset = x_offset;
            m_y_offset = y_offset;
            m_x_fac = x_fac;
            m_y_fac = y_fac;
        }

        public Object AddText(string text, NPoint mid_pt)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            textBlock.Width = 50;
            double x = (mid_pt.m_x * m_x_fac) + m_x_offset;
            double y = (mid_pt.m_y * m_y_fac) + m_y_offset;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            m_canvas.Children.Add(textBlock);
            return (Object)textBlock;

        }
        public Object  AddLine(Brush line_brush, double thickness, double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            //        line.Stroke = Brushes.LightSteelBlue;
            line.Stroke = line_brush;
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            line.StrokeThickness = thickness;
            m_canvas.Children.Add(line);

            return (Object)line;

        }
        public void HideChildren(ArrayList list)
        {
            if(list != null)
            {
                int count = list.Count;
                for(int i = 0; i<count; i++)
                {
                    m_canvas.Children.Remove((UIElement)(list[i]));
                }
            }
        }
    }
    public class NGraphicsUtilities
    {
        static public NGraphicsContext GetCurrentGC()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            return win.m_gc;
        }
        static public double GetOverlapTolerance()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            TextBox tb = win.m_overlap_textbox;
            string cur_value = tb.Text;
            double cur_double_value = double.Parse(cur_value);
            return cur_double_value;

        }
        static public double GetTouchingTolerance()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            TextBox tb = win.m_touch_textbox;
            string cur_value = tb.Text;
            double cur_double_value = double.Parse(cur_value);
            return cur_double_value;

        }
        static public bool IsDisplayNumberChecked()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            bool val = (bool)(win.m_number_display.IsChecked);
            return val;
        }
        static public bool IsDisplayBbox()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            bool val = (bool)(win.m_bbox_display.IsChecked);
            return val;
        }

        internal static bool IsSpikeRemovalRequired()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            return (bool)win.m_spike_removal.IsChecked;
        }

        static public bool IsRemoveCollinearPoints()
        {
            MainWindow win = (MainWindow)(Application.Current.MainWindow);
            bool val = (bool)(win.m_remove_collinear_points.IsChecked);
            return val;
        }
    }

    public class NPlane
    {
        public NPoint m_base;
        public NPoint m_normal;
        public NPlane(NPoint b, NPoint n)
        {
            m_base = b;
            m_normal = n;
        }

        public Boolean Intersect(NPlane other_plane, ref NLine xsect_line)
        {
            // find cross product
            NPoint pnt = m_normal.CrossProduct(other_plane.m_normal);
            /*
            double x_val, y_val, z_val;
            x_val = (m_normal.m_y) * (other_plane.m_normal.m_z) - (m_normal.m_z) * (other_plane.m_normal.m_y);
            y_val = (m_normal.m_z) * (other_plane.m_normal.m_x) - (m_normal.m_x) * (other_plane.m_normal.m_z);
            z_val = (m_normal.m_x) * (other_plane.m_normal.m_y) - (m_normal.m_y) * (other_plane.m_normal.m_x);
            */
            NPoint origin = new NPoint(0, 0, 0);  // currently ised. Actual intersection point will be found later
            xsect_line = new NLine(origin, pnt );
            return true;

        }

        internal bool IsXYPlane()
        {
            const double dist_toler = 0.00001;
            if( (Math.Abs(m_normal.m_x) < dist_toler)  && (Math.Abs(m_normal.m_x) < dist_toler) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal double FindAngle(NPlane plane2)
        {

            NPoint vec1 = m_normal;
            NPoint vec2 = plane2.m_normal;
            vec1.Unitize();
            vec2.Unitize();
            double angle = vec1.FindAngle(vec2);
            return angle;
        }
    }

    public class NPrincipalBoundingBox
    {
        public double x_min = 0;
        public double y_min = 0;
        public double z_min = 0;
        public double x_max = 0;
        public double y_max = 0;
        public double z_max = 0;
        public NPrincipalBoundingBox(double x1, double y1, double z1, double x2, double y2, double z2)
        {

            x_min = x1;
            y_min = y1;
            z_min = z1;
            x_max = x2;
            y_max = y2;
            z_max = z2;

        }
        public NPolyLine Get2DPolyLine()
        {
            ArrayList pnt_list = new ArrayList();
            NPoint pnt = new NPoint(x_min, y_min, z_min); pnt_list.Add(pnt);
            pnt = new NPoint(x_max, y_min, z_min); pnt_list.Add(pnt);
            pnt = new NPoint(x_max, y_max, z_min); pnt_list.Add(pnt);
            pnt = new NPoint(x_min, y_max, z_min); pnt_list.Add(pnt);

            NPolyLine n_pline = new NPolyLine(pnt_list, 0, 3);
            return n_pline;

        }

        internal double GetOverlapAllowance(double dist_toler_fraction)
        {
            double max_distance = double.MinValue;
            max_distance = Math.Max(max_distance, (x_max - x_min));
            max_distance = Math.Max(max_distance, (y_max - y_min));
            max_distance = Math.Max(max_distance, (z_max - z_min));

            return (dist_toler_fraction * max_distance);
        }

        public double GetArea()
        {
            double area = (x_max - x_min) * (y_max - y_min);
            return area;
        }
    }


    public class NDistIndexPair
    {
        public double m_distance = 0;
        public int m_index = -1;
        public NDistIndexPair(double dis, int index)
        {
            m_distance = dis;
            m_index = index;
        }
    }
    public class NPolyLine
    {
        public ArrayList m_input_points = null;
        public int m_start_index = -1;
        public int m_end_index = -1;
        public Boolean[] m_collinear_list = null;
        public NPolyLine(ArrayList input_points, int start_index, int end_index)
        {
            m_input_points = input_points;
            m_start_index = start_index;
            m_end_index = end_index;
        }

        public NPolyLine(ArrayList input_points)
        {
            m_input_points = input_points;
            if (input_points != null)
            {
                m_input_points = input_points;
                m_start_index = 0;
                if (input_points.Count > 0)
                {
                    m_end_index = input_points.Count - 1;
                }
                else
                {
                    m_end_index = -1;
                }
            }
            else
            {
                m_input_points = null;
            }
        }
        public void GetHeadTailLines(ArrayList index_list, ref NPolyLine head_line, ref NPolyLine tail_line)
        {

            if(index_list == null)
            {
                head_line = null;
                tail_line = null;
                return;
            }
            int count = index_list.Count;
            if((int)(index_list[0]) > m_start_index)
            {
                head_line = new NPolyLine(m_input_points, m_start_index, (int)(index_list[0]));
            }
            else
            {
                head_line = null;
            }
            if ((int)(index_list[count-1]) < m_end_index)
            {
                tail_line = new NPolyLine(m_input_points, (int)(index_list[count - 1]), m_end_index);
            }
            else
            {
                tail_line = null;
            }
        }




        public NPolyLine()
        {
            m_input_points = null;
        }
        public void Transform(NMatrix mat)
        {
            int count = m_input_points.Count;
            for (int i = 0; i < count; i++)
            {
                NPoint n_pnt = mat * (NPoint)(m_input_points[i]);
                m_input_points[i] = n_pnt;
            }
        }

        public NPolyLine Clone()
        {
            ArrayList pnt_list = null;
            if (m_input_points != null)
            {
                pnt_list = new ArrayList();
                int count = m_input_points.Count;

                for (int i = 0; i < count; i++)
                {
                    NPoint pnt = (NPoint)(m_input_points[i]);
                    pnt_list.Add(new NPoint(pnt.m_x, pnt.m_y, pnt.m_z));
                }
            }
            NPolyLine p_line = new NPolyLine(pnt_list, m_start_index, m_end_index);
            return p_line;
        }

        static void ListAddWithDuplicateCheck(ArrayList list, object obj)
        {
            if (list == null)
            {
                return;
            }
            int cur_index = list.Count - 1;
            if (cur_index >= 0)
            {
                int a = (int)(list[cur_index]);
                int b = (int)(list[0]);
                if ((a != ((int)obj)) && (b != (int)obj))
                {
                    list.Add(obj);
                }
            }
            else
            {
                list.Add(obj);
            }
        }
        public void GetExtremePointsFromBbox(ref NPrincipalBoundingBox p_box, 
                                            ref ArrayList index_list)
        {
            if (p_box != null)
            {
                NPolyLine p_box_line = null;
                p_box_line = p_box.Get2DPolyLine();

                if (p_box_line.m_input_points != null)
                {
                    if (p_box_line.m_input_points.Count > 1)
                    {
                        for (int i = 0; i < p_box_line.m_input_points.Count; i++)
                        {
                            NPoint seg_pnt1 = null;
                            NPoint seg_pnt2 = null;
                            seg_pnt1 = (NPoint)(p_box_line.m_input_points[i]);
                            if (i < (p_box_line.m_input_points.Count - 1))
                            {
                                seg_pnt2 = (NPoint)(p_box_line.m_input_points[i + 1]);
                            }
                            else if (i == (p_box_line.m_input_points.Count - 1) && i > 1)
                            {
                                seg_pnt2 = (NPoint)(p_box_line.m_input_points[0]);
                            }

                            NPoint int_pnt = null;
                            int int_index = -1;
                            ArrayList pair_list = null;
                            if (seg_pnt1 != null && seg_pnt2 != null)
                            {
                                GetSegmentIntersectionPoint(seg_pnt1, seg_pnt2, ref pair_list);
                            }

                            if (pair_list != null)
                            {
                                if (index_list == null)
                                {
                                    index_list = new ArrayList();
                                }
                                int pair_cnt = pair_list.Count;
                                double min_dist = double.MaxValue;
                                NDistIndexPair closest_pair = null;
                                for (int k = 0; k < pair_cnt; k++)
                                {
                                    NDistIndexPair obj = (NDistIndexPair)(pair_list[k]);
                                    if (obj.m_distance < min_dist)
                                    {
                                        min_dist = obj.m_distance;
                                        closest_pair = obj;
                                    }
                                }
                                if (closest_pair != null)
                                {
                                    index_list.Add(closest_pair.m_index);
                                }

                            }
                            else
                            {
                                //                              MessageBox.Show("No intersection found with principal bounding box");
                            }
                        }
                    }
                }

                if (index_list != null)
                {
                    index_list.Sort();
                    
                }
            }

        }

        private void UpdateMinimalValues(double dist, int i, ref ArrayList min_distance_list)
        {
            NDistIndexPair pair = new NDistIndexPair(dist, i);
            min_distance_list.Add(pair);

        }

        public void GetSegmentIntersectionPoint(NPoint seg_start_point, NPoint seg_end_point,
                                               ref ArrayList min_distance_list)
        {
            const double Noverlap_fraction = 0.05;
            double N_Dist_Tolerance = Noverlap_fraction * seg_start_point.Distance(seg_end_point);
            min_distance_list = new ArrayList();
            NPoint pnt = null;
            for (int i = m_start_index; i <= m_end_index; i++)
            {
                NPoint pt = (NPoint)(m_input_points[i]);
                double dist = pt.DistanceFromSegment(seg_start_point, seg_end_point,ref  pnt);
                if (dist < N_Dist_Tolerance)
                {
                    UpdateMinimalValues(dist, i, ref min_distance_list); ;
                }
            }
            return;
        }



        public NPrincipalBoundingBox GetPrincipalBoundingBox()
        {
            const double max_val = double.MaxValue;
            const double min_val = double.MinValue;
            // to do
            NPrincipalBoundingBox bbox = null;

            double x_min = max_val, y_min = max_val, x_max = min_val, y_max = min_val, z_min = max_val, z_max = min_val;

            NPoint pt = null;
            if (m_end_index == -1 || m_start_index == -1)
            {
                return null;
            }

            for (int i = m_start_index; i <= m_end_index; i++)
            {
                pt = (NPoint)m_input_points[i];
                x_min = Math.Min(x_min, pt.m_x);
                y_min = Math.Min(y_min, pt.m_y);
                z_min = Math.Min(z_min, pt.m_z);
                x_max = Math.Max(x_max, pt.m_x);
                y_max = Math.Max(y_max, pt.m_y);
                z_max = Math.Max(z_max, pt.m_z);
            }
            bbox = new NPrincipalBoundingBox(x_min, y_min, z_min, x_max, y_max, z_max);
            return bbox;
        }
        public void RotateAndGetBoundingBoxAtAngle(double angle, ref NPolyLine cloned_line, ref NPrincipalBoundingBox bbox)
        {
            // to do
            bbox = null;

            // clone the polynile
            cloned_line = this.Clone();

            // transform the cloned line
            NMatrix mat = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, angle);
            cloned_line.Transform(mat);

            bbox = cloned_line.GetPrincipalBoundingBox();

            return;
        }
        public Boolean IsBboxOverlapping(NPrincipalBoundingBox bbox, double overlap_allownace)
        {
            Boolean val = false;

            NPolyLine bbox_2d_line = bbox.Get2DPolyLine();

            int st_index = 0;
            int end_index = 0;

            for (int i = bbox_2d_line.m_start_index; i <= bbox_2d_line.m_end_index; i++)
            {
                st_index = i;
                if (i == bbox_2d_line.m_end_index)
                {
                    end_index = bbox_2d_line.m_start_index;
                }
                else
                {
                    end_index = i + 1;
                }
                val = IsSegmentOverlapping((NPoint)(bbox_2d_line.m_input_points[st_index]),
                    (NPoint)(bbox_2d_line.m_input_points[end_index]), overlap_allownace);
                if (val == true)
                {
                    break;
                }

            }


            return val;
        }


        public ArrayList RemoveSpikes(ArrayList index_list, double length_proportion = 0.25, double thinness_proportion = 0.01)
        {
            int cnt = index_list.Count;
            ArrayList output_list = new ArrayList();
            Boolean[] mark_list = new Boolean[cnt];
            Boolean[] l_mark_list = new Boolean[4];
            NPoint[] pnts = new NPoint[4];
            Boolean is_spike = false;
            for (int i = 0; i < cnt; i++)
            {
                mark_list[i] = false;

            }
            for (int i = 0; i < cnt - 3; i++)
            {
                // identify longer spikes
                for (int k = 0; k < 4; k++)
                {
                    l_mark_list[k] = mark_list[i + k];
                    pnts[k] = (NPoint)(m_input_points[(int)(index_list[i+k])]);
                }

                is_spike = IsItSpike(4, pnts, l_mark_list);
                if (is_spike == true)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        mark_list[i + k] = l_mark_list[k];
                    }
                }
            }
            int output_cnt = -1;
            for (int i = 0; i < cnt; i++)
            {
                if( mark_list[i] == false)
                {
                    output_cnt++;
                    output_list.Add(index_list[i]);
                }

            }
            return output_list;
        }

        private bool IsItSpike(int cnt, NPoint[] pnts, Boolean[] l_mark_list)
        {
            if (cnt > 4)
            {
                return false;
            }
            double[] len = new double[4];
            double min_distance = double.MaxValue;
            double max_distance = double.MinValue;
            int st_ind, end_ind;


            for (int i = 0; i < cnt; i++)
            {
                st_ind = i;
                if (st_ind < (cnt - 1))
                {
                    end_ind = i + 1;
                }
                else
                {
                    end_ind = 0;
                }
                len[i] = pnts[st_ind].Distance(pnts[end_ind]);
                min_distance = Math.Min(min_distance, len[i]);
                max_distance = Math.Max(max_distance, len[i]);
            }




            double distance_0_2 = 0.0;
            double distance_2_0 = 0.0;

            NPoint pnt3 = null;
            distance_0_2 = pnts[0].DistanceFromSegment(pnts[1], pnts[2], ref pnt3);
            NPoint vec1 = pnts[1] - pnts[0];
            NPoint vec2 = pnts[2] - pnts[1];
            double angle = Math.Abs(vec1.FindAngle(vec2));

          //  distance_0_2 = pnts[0].Distance(pnts[2]);
            // spike with 1st 2 segments
            bool is_spike = false;
            if (angle > (Math.PI * 0.8))
            {
                if ((distance_0_2 / max_distance) < 0.15)
                {
                    is_spike = true;
                }
                if (is_spike == false)
                {
                    distance_0_2 = pnts[2].DistanceFromSegment(pnts[0], pnts[1], ref pnt3);
                    //distance_0_2 = pnts[2].Distance(pnts[0]);
                    if (distance_0_2 / max_distance < 0.15)
                    {
                        is_spike = true;

                    }
                }
            }

            if (is_spike  == true)
            {
                l_mark_list[0] = false;
                l_mark_list[1] = true;
                l_mark_list[2] = false;
                if ((len[3] / max_distance) < 0.15)
                {
                    l_mark_list[2] = true;
                 //   l_mark_list[3] = false;
                }
                return true;

            }



            // spike with 1st 3 segments

            if (((len[0] / max_distance) > 0.15) &&
                  ((len[1] / max_distance) < 0.15) &&
                  ((len[2] / max_distance) > 0.15) &&
                  ((len[3] / max_distance) < 0.15))
            {
          //      l_mark_list[0] = true;
                l_mark_list[1] = true;
                l_mark_list[2] = true;
         //       l_mark_list[3] = true;

                return true;

            }







            return false;

        }

        public void GetSplittingBoundingBox(ref NPolyLine n_poly_line, ref NPrincipalBoundingBox guess_bb, ref double touching_tolerance)
        {
            Boolean is_overlapping = true;
            double deg_angle = 45;
            guess_bb = null;
             double overlap_allownace = -1.0;

            while (deg_angle < 180)
            {
                double angle = deg_angle * Math.PI / 180.0;
                RotateAndGetBoundingBoxAtAngle(angle, ref n_poly_line, ref guess_bb);
                if (guess_bb != null && touching_tolerance < 0)
                {
                    double touch_toler_fraction = NGraphicsUtilities.GetTouchingTolerance();
                    touching_tolerance = guess_bb.GetOverlapAllowance(touch_toler_fraction);
                }
                is_overlapping = n_poly_line.IsBboxOverlapping(guess_bb, touching_tolerance);
                //              n_poly_line.Display(Brushes.Gray, 2);
                //              break;
                if(is_overlapping == false)
                {
                    break;
                }
                deg_angle += 10;
                    
            }

            if (is_overlapping == true)
            {
                n_poly_line = null;
                guess_bb = null;
            }

        }

        // This function is used by the boundingbox boundaries to check whether they overlap the respective closed polyline
        public Boolean IsSegmentOverlapping(NPoint st_pt, NPoint end_pt, double opevrlap_Tolerance)
        {
            //         opevrlap_Tolerance = 0.05 * st_pt.Distance(end_pt);
            Boolean overlap_status = false;
            if(m_start_index > m_end_index)
            {
 //               MessageBox.Show("Start Index " + m_end_index + " End Index " + m_end_index);
                return false;
            }
            int threshold_cnt = (int)(0.15 * (m_end_index - m_start_index));
            if (threshold_cnt < 20)
            {
                threshold_cnt = Math.Min(20, (m_end_index - m_start_index));
            }
            NPoint pnt = null;
            for (int i = m_start_index; i < m_end_index; i++)
            {
                NPoint pt = (NPoint)(m_input_points[i]);
                double dist = pt.DistanceFromSegment(st_pt, end_pt, ref pnt);
                if (dist < opevrlap_Tolerance)
                {
                    threshold_cnt--;
                }
                if (threshold_cnt == 0)
                {
                    overlap_status = true;
                    break;
                }
            }
            return overlap_status;
        }



        // low level utility function. Assumption No break between start and end
        /*     public Boolean IsSegmentOverlappingPolylineRegion(int start_index, int end_index , NPoint st_pt, NPoint end_pt, double opevrlap_Tolerance)
             {
                 Boolean overlap_status = false;
                 int near_threshold_cnt = 0, away_threshold_count = 0, cnt=0;
                 cnt = away_threshold_count = near_threshold_cnt = (int)(0.05 * (m_end_index - m_start_index));
                 if (cnt < 5)
                 {
                     cnt = 5;
                 }
                 int l_end_index = end_index;
                 if (l_end_index == 0)
                 {
                     l_end_index = this.m_input_points.Count - 1;
                 }
                 if (cnt > (l_end_index - start_index + 1))
                 {
                     cnt = (l_end_index - start_index + 1);
                 }

                 away_threshold_count = near_threshold_cnt = cnt;

                  for (int i = start_index; i <= l_end_index; i++)
                 {
                     NPoint pt = (NPoint)(m_input_points[i]);
                     double dist = pt.DistanceFromSegment(st_pt, end_pt);
                     if (dist < opevrlap_Tolerance)
                     {
                         near_threshold_cnt--;
                     }
                     else
                     {
                         away_threshold_count--;
                     }

                 }
                 if (near_threshold_cnt <= 0 && away_threshold_count > 0)
                 {
                     overlap_status = true;
                 }
                 if (overlap_status != true)
                 {
                     if (end_index == 0)
                     {
                         NPoint pt = (NPoint)(m_input_points[0]);
                         double dist = pt.DistanceFromSegment(st_pt, end_pt);
                         if (dist < opevrlap_Tolerance)
                         {
                             near_threshold_cnt--;
                         }
                         else
                         {
                             away_threshold_count--;
                         }
                         if (near_threshold_cnt <= 0 && away_threshold_count > 0)
                         {
                             overlap_status = true;
                         }

                     }
                 }

                 return overlap_status;

             }
        */
        private  Boolean OverlapFromClosenessAnalysis(int closer_points, int farther_points)
        {
            closer_points -= 2;
            if (farther_points >= closer_points)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /*
                private Boolean IsSegmentOverlappingPolylineRegion(int start_index, int end_index, NPoint st_pt, NPoint end_pt, 
                                                                   double opevrlap_Tolerance, ref int closer_points, ref int farther_points)
                {
                    if (end_index > start_index)
                    {
                        closer_points = 0;
                        farther_points = 0;

                        for (int i = start_index; i <= end_index; i++)
                        {
                            NPoint pt = (NPoint)(m_input_points[i]);
                            double dist = pt.DistanceFromSegment(st_pt, end_pt);
                            if (dist < opevrlap_Tolerance)
                            {
                                closer_points++;
                            }
                            else
                            {
                                farther_points++;
                            }

                        }
                        Boolean overlap_status = OverlapFromClosenessAnalysis(closer_points, farther_points);
                        return overlap_status;
                    }
                    else
                    {
                        MessageBox.Show("Unexpected condition of start_index >= end_index");
                        return true;
                    }

                }
        */



        public void MarkCollinearPoints(double overlap_fraction)
        {
            if (m_input_points == null)
            {
                return;
            }
            int cnt = m_input_points.Count;
            if (m_collinear_list == null)
            {
                m_collinear_list = new Boolean[cnt];
            }
            /*           Boolean[] mark_list = new Boolean[cnt];  */
            if (m_end_index == -1)
            {
                m_end_index = cnt - 1;
            }
            if (m_end_index < m_start_index)
            {
                MessageBox.Show("Remove Collinear Points not supported for m_end_index < m_start_index");
            }
            for (int i = m_start_index; i <= m_end_index; i++)
            {
                m_collinear_list[i] = false;
            }

            NPrincipalBoundingBox bbox = this.GetPrincipalBoundingBox();
            double overlap_tolerance = bbox.GetOverlapAllowance(overlap_fraction);
            // identify 1st non-coliner point -ve direction
            NPoint pnt1 = null;
            NPoint pnt2 = null;
            NPoint pnt3 = null;

            int s_i = m_start_index;
            int e_i = s_i + 2;
            int l_i = -1;


            while (s_i <= m_end_index && e_i <= m_end_index)
            {

                if (this.IsChordOverlappingPolylineRegion(s_i, e_i, overlap_tolerance) == true)
                {
                    l_i = e_i;
                    e_i++;
                    if (e_i <= m_end_index)
                    {
                        continue;
                    }
                }

                if (l_i != -1)
                {
                    for (int i = s_i + 1; i < l_i; i++)
                    {
                        m_collinear_list[i] = true;
                    }
                    s_i = l_i;
                    l_i = s_i + 2;
                }
                else
                {
                    s_i++;
                    e_i++;
                }



            }



            if (cnt > 2) // at least 3 points
            {
                s_i = cnt - 1;
                e_i = 1;
                if (this.IsChordOverlappingPolylineRegion(s_i, e_i, overlap_tolerance) == true)
                {
                    m_collinear_list[0] = false;
                }

            }
        }

        public void RemoveCollinearPoints(double overlap_fraction)
        {
            if(m_input_points == null)
            {
                return;
            }
            MarkCollinearPoints(overlap_fraction);
            int output_cnt = -1;
            ArrayList output_list = new ArrayList();
            for (int i = m_start_index; i <= m_end_index; i++)
            {
                if (m_collinear_list[i] == false)
                {
                    output_cnt++;
                    output_list.Add(m_input_points[i]);
                }

            }
            m_input_points = output_list;
            m_start_index = 0;
            m_end_index = output_list.Count - 1;


        }
        private double PolyLineAreaUnderSegment(int start_index, int end_index, NPoint st_pt, NPoint end_pt)
        {
            if (end_index > start_index)
            {

                double area_under_curve = 0;
                double segment_distance = 0.0;
                segment_distance = st_pt.Distance(end_pt);
                double prev_h1 = -1;
                NPoint pnt = null;
                for (int i = start_index; i < end_index; i++)
                {
                    NPoint pt = (NPoint)(m_input_points[i]);
                    double val = AreaUnderSegment(i, i + 1, st_pt, end_pt, ref prev_h1, ref pnt);
                    area_under_curve += val;
                }

                return area_under_curve;
            }
            else if(start_index == end_index)
            {
                return 0;
            }
            else
            {
    //            MessageBox.Show("Function PolyLineAreaUnderSegment : Unexpected condition of start_index > end_index");
                return 0;
            }

        }
        private Boolean IsSegmentOverlappingPolylineRegion(int start_index, int end_index, NPoint st_pt, NPoint end_pt,
                                                           double opevrlap_Tolerance )
        {
            if (end_index > start_index)
            {
               /* closer_points = 0;
                farther_points = 0; */
                double area_under_curve = 0;
                double segment_distance = 0.0;
                segment_distance = st_pt.Distance(end_pt);
                double prev_h1 = -1;
                NPoint pnt = null;
                for (int i = start_index; i < end_index; i++)
                {
                    NPoint pt = (NPoint)(m_input_points[i]);
                    double val = AreaUnderSegment(i, i+1, st_pt, end_pt, ref prev_h1, ref pnt);
                    area_under_curve += val;
                }
                
                Boolean overlap_status = OverlapFromAreaUnderCurveAnalysis(area_under_curve, segment_distance, opevrlap_Tolerance);
                return overlap_status;
            }
            else
            {
        //        MessageBox.Show("Unexpected condition of start_index >= end_index");
                return true;
            }

        }

        private bool OverlapFromAreaUnderCurveAnalysis(double area_under_curve, double segment_distance, double overlap_tolerance)
        {
            double val = area_under_curve / segment_distance;
            if (val < overlap_tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double AreaUnderSegment(int i, int j, NPoint st_pt, NPoint end_pt, ref double prev_h1, ref NPoint prev_pnt)
        {
            NPoint pnt1 = (NPoint)m_input_points[i];
            NPoint pnt2 = (NPoint)m_input_points[j];
            double h1 = 0;
            NPoint proj_pnt1 = null;
            NPoint proj_pnt2 = null;
            if(prev_h1 > -1 && prev_pnt != null)
            {
                h1 = prev_h1;
                proj_pnt1 = prev_pnt;
            }
            else
            {
                h1 = pnt1.DistanceFromSegment(st_pt, end_pt, ref proj_pnt1);
            }
            double h2 = pnt2.DistanceFromSegment(st_pt, end_pt, ref proj_pnt2);
            double segment_distance = proj_pnt1.Distance(proj_pnt2);
            // check intersection of pnt1 -> pnt2 & st_pt & end_pt

            NPoint vec1 = pnt1 - proj_pnt1;
            NPoint vec2 = pnt2 - proj_pnt2;

            double dot_product = (vec1.m_x * vec2.m_x) + (vec1.m_y * vec2.m_y) + (vec1.m_z * vec2.m_z);
            double area = 0.0;
            if (dot_product < double.MinValue)
            {
                area = segment_distance * ( ((h1*h1) + (h2*h2))/ (h1 + h2) )/ 2.0;
            }
            else
            {
                area = segment_distance * (h1 + h2) / 2.0;
            }

          
            prev_h1 = h2;
            prev_pnt = proj_pnt2;
            return area;

        }

        public Boolean IsChordOverlappingPolylineRegion(int start_index, int end_index, double overlap_allownace)
        {

            Boolean overlap_status = false;
            NPoint st_pt = null, end_pt = null;
            st_pt = (NPoint)(m_input_points[start_index]);
            end_pt = (NPoint)(m_input_points[end_index]);
            double segment_distance = st_pt.Distance(end_pt);
            double overlap_area = 0;


            if (end_index > start_index)
            {
                if (end_index == (start_index + 1))
                {
                    overlap_status =  true;
                }
                else
                {
                   overlap_area = PolyLineAreaUnderSegment(start_index, end_index, st_pt, end_pt);
                   overlap_status = OverlapFromAreaUnderCurveAnalysis(overlap_area, segment_distance, overlap_allownace);
               }
           }
            else if(end_index == start_index)
            {
                overlap_status =  true;
            }
            else
            {

                double overlap_area_0_end = 0;
                if (end_index > 0)
                {
                    overlap_area_0_end = PolyLineAreaUnderSegment(0, end_index, st_pt, end_pt);
                }
                int pline_end_index = m_input_points.Count - 1;
                double overlap_area_start_0= PolyLineAreaUnderSegment(start_index, pline_end_index, st_pt, end_pt);
                overlap_area = overlap_area_0_end + overlap_area_start_0;
                overlap_status = OverlapFromAreaUnderCurveAnalysis(overlap_area, segment_distance, overlap_allownace);
 
            }


            return (overlap_status);

        }

 
     private ArrayList GetExtremeIndicesPlanerPLine( ref double touching_tolerance ,  ref double overlap_allownace, bool closed_polygon = true, int function_call = 0)
     {
         function_call++;
         if(function_call > 2000)
         {
             return null;
         }
         NPolyLine n_poly_line = null;
         NPrincipalBoundingBox p_box = null;

         GetSplittingBoundingBox(ref n_poly_line, ref p_box, ref touching_tolerance);
        if(p_box == null)
        {
     //           MessageBox.Show("Extreme point finding bounding box not found");
                return null;
        }

        if (overlap_allownace < 0)
        {

            double overlap_fraction = NGraphicsUtilities.GetOverlapTolerance();
            overlap_allownace = p_box.GetOverlapAllowance(overlap_fraction);
        }

         ArrayList point_index_list = null;
         ArrayList output_list = null;

         n_poly_line.GetExtremePointsFromBbox(ref p_box, ref point_index_list);
         
 
         if (point_index_list != null)
         {

                point_index_list = NUtilities.RemoveDuplicateIndexes(point_index_list);

                 NPolyLine head_line = null;
            NPolyLine tail_line = null;
            output_list = new ArrayList();
            if (closed_polygon == false)
            {

                    if (point_index_list.Count < 4)
                {
     //                   MessageBox.Show("Bounding Box Points  " + point_index_list.Count);
                        for (int k = n_poly_line.m_start_index; k < n_poly_line.m_end_index; k++)
                        {
                            ListAddWithDuplicateCheck(output_list, k);
                        }
                        return output_list;
                }
                n_poly_line.GetHeadTailLines(point_index_list, ref head_line, ref tail_line);
                if (head_line != null)
                {
                    ArrayList head_index_list =
                    head_line.GetExtremeIndicesPlanerPLine(ref touching_tolerance, ref overlap_allownace, false, function_call);
                    if (head_index_list != null)
                    {
                        if (head_index_list.Count > 0)
                        {

                            for (int k = 0; k < head_index_list.Count; k++)
                            {
                                ListAddWithDuplicateCheck(output_list, head_index_list[k]);
                            }
                        }
                    }
                }
             }




            int cnt = point_index_list.Count;
            if(cnt > 1)  // minimum 1 segment should be present
             {
                 
                 for (int i = 0; i < cnt; i++)
                 {
                     int start_index, end_index;
                     start_index = (int)(point_index_list[i]);
                     int j = i+1;
                     if(j == cnt)
                     {
                         if(closed_polygon == false)   // do not process second last point and last point.
                         {
                             break;
                         }
                         j = 0;
                     }
                     end_index = (int)(point_index_list[j]);
                     if(start_index == end_index)
                     {
                         continue;
                     }
                     Boolean is_overlapping = n_poly_line.IsChordOverlappingPolylineRegion(start_index, end_index, 
                         overlap_allownace);
                     if ((is_overlapping == false))
                     {
                         // identify new polyline region for recursive computation

                         if (end_index > start_index)
                         {
                             NPolyLine new_p_line = new NPolyLine(n_poly_line.m_input_points, start_index, end_index);
                             ArrayList new_list = new_p_line.GetExtremeIndicesPlanerPLine( ref touching_tolerance, ref overlap_allownace, false, function_call);
                             if (new_list != null)
                             {
                                 if (new_list.Count > 0)
                                 {

                                     for (int k = 0; k < new_list.Count; k++)
                                     {
                                         ListAddWithDuplicateCheck(output_list, new_list[k]);
                                     }
                                 }
                             }
                         }
                         else
                         {
                             if(end_index > 0)
                             {  // case 0 -> end_index
                                 NPolyLine new_p_line = new NPolyLine(n_poly_line.m_input_points, 0, end_index);
                                 ArrayList new_list = new_p_line.GetExtremeIndicesPlanerPLine(ref touching_tolerance, ref overlap_allownace, false, function_call);
                                    if (new_list != null)
                                 {
                                     if (new_list.Count > 0)
                                     {

                                         for (int k = 0; k < new_list.Count; k++)
                                         {
                                             ListAddWithDuplicateCheck(output_list, new_list[k]);
                                         }
                                     }
                                 }
                             }

                            if (start_index < n_poly_line.m_input_points.Count - 1)
                            {
                                NPolyLine new_p_line = new NPolyLine(n_poly_line.m_input_points, start_index, (n_poly_line.m_input_points.Count - 1));
                                ArrayList new_list = new_p_line.GetExtremeIndicesPlanerPLine( ref touching_tolerance, ref overlap_allownace, false, function_call);
                                if (new_list != null)
                                {
                                    if (new_list.Count > 0)
                                    {

                                        for (int k = 0; k < new_list.Count; k++)
                                        {
                                            ListAddWithDuplicateCheck(output_list, new_list[k]);
                                        }
                                    }
                                }
                            }
  
                         }
                     }
                     else
                     {
                         ListAddWithDuplicateCheck(output_list, start_index);
                         ListAddWithDuplicateCheck(output_list, end_index);
                     }
                 }
             }


                if (tail_line != null)
                {
                    ArrayList tail_index_list =
                    tail_line.GetExtremeIndicesPlanerPLine(ref touching_tolerance, ref overlap_allownace, false, function_call);
                    if (tail_index_list != null)
                    {
                        if (tail_index_list.Count > 0)
                        {

                            for (int k = 0; k < tail_index_list.Count; k++)
                            {
                                ListAddWithDuplicateCheck(output_list, tail_index_list[k]);
                            }
                        }
                    }
                }


            }
            return output_list;

     }
      private NPlane GetCurrentPlane()
     {
         int cnt = m_input_points.Count;
         if(cnt < 3)
         {
             return new NPlane(new NPoint(0.0, 0.0, 0.0), new NPoint(1.0, 0.0, 0.0));
         }
         int index1 = 0;
         int index2 = (int)(cnt / 3);
         int index3 = (int)(cnt / 4);

         NPoint vec1 = (NPoint)(m_input_points[index2]) - (NPoint)(m_input_points[index1]);
         NPoint vec2 = (NPoint)(m_input_points[index2]) - (NPoint)(m_input_points[index3]);
         NPoint cross_product = vec1.CrossProduct(vec2);

         NPlane plane = new NPlane((NPoint)(m_input_points[index2]), cross_product);
         return plane;
     }
     public void TransformToXYPlane()
     {
         NPlane plane1 = GetCurrentPlane();
         if(plane1.IsXYPlane())
         {
             return;
         }
         NPlane plane2 = new NPlane(new NPoint(0.0, 0.0, 0.0),new NPoint(0.0, 0.0, 1.0)); // XY Plane
         NLine xsect_line = null;
         Boolean result = plane1.Intersect(plane2, ref xsect_line);
         if (result == true)
         {
             double angle1 = plane1.FindAngle(plane2);
             RotateAboutAxis(xsect_line, angle1);
         }
     }

     private void RotateAboutAxis(NLine xsect_line, double angle1)
     {
         NMatrix mat1 = NPoint.GetMatrixRotationAboutAxis(xsect_line.m_start_point, xsect_line.m_end_point, angle1);
         for (int i = 0; i < m_input_points.Count; i++)
         {
             NPoint pnt = (NPoint)(m_input_points[i]);
             NPoint pnt2 = mat1 * pnt;
             m_input_points[i] = pnt2;
         }
     }

     public ArrayList GetExtremeIndices()
     {
         TransformToXYPlane();
         int max_stack_size = 1000;
       
            double touching_tolerance = -1;
            double overlap_tolerance = -1;
           
            ArrayList index_list = GetExtremeIndicesPlanerPLine(ref touching_tolerance, ref overlap_tolerance); // Top level  call with default parameters
            return index_list;

     }

        //     public ArrayList  Display(Brush brush, double thickness, ArrayList point_index_list=null, int start_index=0, int end_index=-1)
        public ArrayList Display(Brush brush, double thickness)
        {
    //        ArrayList point_index_list = null;
        int start_index = 0,  end_index = -1;
        ArrayList display_object_list = new ArrayList();
        int count = m_input_points.Count;
         NPoint pt1 = new NPoint(0, 0, 0);
         NPoint pt2 = new NPoint(0, 0, 0);

         end_index = count - 1;

         NGraphicsContext gc = NGraphicsUtilities.GetCurrentGC();

         Canvas can = gc.m_canvas;
         double x_fac = gc.m_x_fac;
         double y_fac = gc.m_y_fac;
         double x_offset = gc.m_x_offset;
         double y_offset = gc.m_y_offset;


         for (int i = start_index; i < end_index; i++)
         {


            pt1 = (NPoint)m_input_points[i];
            pt2 = (NPoint)m_input_points[i + 1];




             Object display_obj = gc.AddLine(brush, thickness, (pt1.m_x * x_fac) + x_offset,
                          (pt1.m_y * y_fac) + y_offset,
                          (pt2.m_x * x_fac) + x_offset,
                          (pt2.m_y * y_fac) + y_offset);

             display_object_list.Add(display_obj);


/////////////////////////// for debugging       
             if(NGraphicsUtilities.IsDisplayNumberChecked() == true)
             {
                string number_block = " " + (i);
                NPoint pt = (NPoint)(m_input_points[i]);
                display_obj = gc.AddText(number_block, pt);
                display_object_list.Add(display_obj);
             }

//////////////////////////////////////////


         }
         return display_object_list;


     }
 }
 public class NPointCloud
 {

     public ArrayList m_input_points = new ArrayList();
     public ArrayList m_sorted_points = new ArrayList();
     public ArrayList m_line_array = new ArrayList();
     public NPoint m_b_box_lower = new NPoint(1000000.0, 1000000.0, 1000000.0);
     public NPoint m_b_box_upper = new NPoint(-1000000.0, -1000000.0, -1000000.0);

     public NPolyLine m_input_polyline = null;
     public NPolyLine m_output_polyline = null;


     public NPointCloud()
     {


     }
     public void AddPoint(NPoint pt)
     {
//          pt.ConvertToPolar();
         m_input_points.Add(pt);
         m_b_box_lower.m_x = Math.Min(m_b_box_lower.m_x, pt.m_x);
         m_b_box_lower.m_y = Math.Min(m_b_box_lower.m_y, pt.m_y);
         m_b_box_lower.m_z = Math.Min(m_b_box_lower.m_z, pt.m_z);
         m_b_box_upper.m_x = Math.Max(m_b_box_upper.m_x, pt.m_x);
         m_b_box_upper.m_y = Math.Max(m_b_box_upper.m_y, pt.m_y);
         m_b_box_upper.m_z = Math.Max(m_b_box_upper.m_z, pt.m_z);

     }
     /*
     public void SortPoints()
     {
         int num = m_input_points.Count;
         m_sorted_points.Clear();
         for (int i = 0; i < num; i++)
         {
             m_sorted_points.Add(m_input_points[i]);
         }
         m_line_array.Clear();

         for (int i = 0; i < num; i++)
         {
             ((NPoint)m_input_points[i]).ConvertToPolar();
         }


     }
     */
        public void Clear()
        {
            m_input_points.Clear();
            m_sorted_points.Clear();
            m_line_array.Clear();
        }
        /*
        private void AddBitmap(Canvas can)
        {
            const int width = 1000;
            const int height = 800;


            double x_fac = (double)(width-10) / (Math.PI * 2.0);
            double y_fac = (double)(height-10) / (Math.PI * 2.0);


            WriteableBitmap wbitmap = new WriteableBitmap(
                width, height, 96, 96, PixelFormats.Bgra32, null);
            byte[,,] pixels = new byte[height, width, 4];

            // Clear to black.
            int row, col;
            for (row = 0; row < height; row++)
            {
                for (col = 0; col < width; col++)
                {
                    for (int i = 0; i < 3; i++)
                        pixels[row, col, i] = 255;
                    pixels[row, col, 3] = 255;
                }
            }

            int count = m_input_points.Count;
            byte gre_val = 0;
            for (int i = 0; i < count; i++)
            {
                NPoint pt = (NPoint)m_input_points[i];
                row = (int)(y_fac * pt.m_ang1);
                col = (int)(y_fac * pt.m_ang2);
                gre_val = pt.GreyVal();

                pixels[row, col, 0] = gre_val;
                pixels[row, col, 1] = gre_val;
                pixels[row, col, 2] = gre_val;
                pixels[row, col, 3] = 255;

            }
        

            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[height * width * 4];
            int index = 0;
            for (row = 0; row < height; row++)
            {
                for (col = 0; col < width; col++)
                {
                    for (int i = 0; i < 4; i++)
                        pixels1d[index++] = pixels[row, col, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int stride = 4 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);

            // Create an Image to display the bitmap.
            Image image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);

            can.Children.Add(image);

            //Set the Image source.
            image.Source = wbitmap;
        }

        */



        public void DisplayPointsAsPolyLine()
        {
            const int width = 1300;
            const int height = 800;

            m_input_polyline = new NPolyLine(m_input_points, 0, m_input_points.Count - 1);
            m_input_polyline.TransformToXYPlane();
            NPrincipalBoundingBox bbox = m_input_polyline.GetPrincipalBoundingBox();


            double bbox_x_range, bbox_y_range;



            bbox_x_range = bbox.x_max - bbox.x_min;
            //          bbox_y_range = m_b_box_upper.m_y - m_b_box_lower.m_y;
            bbox_y_range = bbox.y_max - bbox.y_min;

            NGraphicsContext gc =NGraphicsUtilities.GetCurrentGC();
            gc.m_x_fac = (double)(width - 100) / (bbox_x_range);
            gc.m_y_fac = (double)(height - 100) / (bbox_y_range);

            gc.m_y_fac = gc.m_x_fac = Math.Min(gc.m_x_fac, gc.m_y_fac);

            gc.m_x_offset = 50.0 -(bbox.x_min * gc.m_x_fac);
            gc.m_y_offset = 50.0 -(bbox.y_min * gc.m_x_fac);

   //         m_input_polyline = new NPolyLine(m_input_points, 0, m_input_points.Count - 1);
            m_input_polyline.Display(Brushes.LightSteelBlue, 2);
     }


        /*
                public void DisplayBitmap(Canvas can)
                {
                    AddBitmap(can);
                }
        */
        public void DisplayPoints()
        {
            DisplayPointsAsPolyLine();
        }

        public ArrayList RefineBoundaryLineAndDisplay(bool spike_removal_flag, 
                                                        double overlap_fraction, 
                                                         bool collinear_removal_flag)
        {
            
            /*
            m_input_polyline.RemoveCollinearPoints(overlap_fraction);
            ArrayList lst = m_input_polyline.Display(Brushes.Blue, 5);
            return lst;
            */
            ArrayList index_list = m_input_polyline.GetExtremeIndices();


            ArrayList display_item_list = null;


            if (index_list != null)
            {
                index_list.Sort();
                if (spike_removal_flag == true)
                {
                    index_list = m_input_polyline.RemoveSpikes(index_list);
                }
                int count = index_list.Count;
                if (count > 1)
                {
                    if (index_list[0] != index_list[count - 1])
                    {
                        index_list.Add(index_list[0]);  // added to close the polyline 
                        count++;
                    }
                   
                }
                ArrayList pnt_list = new ArrayList();
                for (int i = 0; i < count; i++)
                {
                    pnt_list.Add(m_input_points[(int)(index_list[i])]);
                }

                m_output_polyline = new NPolyLine(pnt_list, 0, count - 1);
                if (collinear_removal_flag == true)
                {
                    m_output_polyline.RemoveCollinearPoints(overlap_fraction);
                }
                display_item_list =  m_output_polyline.Display(Brushes.Red, 5);
            }
            else
            {
    //            MessageBox.Show("No extreme points found");
            }

            return display_item_list;

        }



    }
}

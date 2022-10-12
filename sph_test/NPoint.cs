using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace sph_test
{
    
    public class NPoint            
    {

        public const double m_x_toler = 0.000000001;

        /*   
         *   
         *   
              public double m_rad = 0;
              public double m_ang1 = 0;
              public double m_ang2 = 0;
             public static NPoint m_polar = new NPoint(25, 25, 25);

              public const double MAX_DIST_RANGE = 50;
              public const double MIN_DIST_LIMIT = 0;

              public  byte GreyVal()
              {
                  double grey_val = 255.0 - (255.0 * (m_rad - MIN_DIST_LIMIT) / MAX_DIST_RANGE);
                  return (byte)grey_val;
              }
        
        public void ConvertToPolar()
        {

            NPoint temp_pnt = new NPoint(this);
            m_rad = Distance(m_polar);

            // Transform the point to polar origin

            // calculate angle with positive Z axis
            if (Math.Abs(m_rad) > m_x_toler)
            {
                m_ang1 = Math.Acos((m_z - m_polar.m_z) / m_rad);
            }
            else
            {
                m_ang1 = 0.0;
            }
               
            if(m_ang1 < 0.0)
            {
                MessageBox.Show(@"Negative Angle m_ang1");
            }
            m_ang2 = AtanSpecial((m_y - m_polar.m_y), (m_x - m_polar.m_x));


        }
        */
        public static double AtanSpecial(double dy, double dx)
        {
            double ang = 0.0;
            if (dx < m_x_toler && dx > (-m_x_toler))
            {
                if (dy < m_x_toler && dy > (-m_x_toler))
                {
                    // both dx  and dy are ZERO
                    ang = Math.PI / 4.0;
                }
                else
                {
                    if (dy > 0.0)
                    {
                        ang = Math.PI / 2.0;
                    }
                    else
                    {
                        ang = (Math.PI) * 1.5;

                    }
                }
            }
            else
            {
                ang = Math.Atan2(dy, dx);
                if (ang < 0)
                {
                    ang = ang + (Math.PI * 2);
                }

            }
            return ang;
        }
        // angle between 2 unit vectrs
        public double FindAngle(NPoint vec2)
        {
            NPoint origin = new NPoint(0.0, 0.0, 0.0);
            double dot_product = (m_x * vec2.m_x) + (m_y * vec2.m_y) + (m_z * vec2.m_z);
            double dist_product = (origin.Distance(this)) * (origin.Distance(vec2));
            double val = dot_product / dist_product;
            
            if(val < -0.999999)
            {
                    return Math.PI;

            }
            else if(val > 0.999999)
            {

                    return 0.0;
            }
            double angle = Math.Acos(val);
            return angle;
        }

        public double m_x = 0;
        public double m_y = 0;
        public double m_z = 0;

        public void Unitize()
        {
            NPoint origin = new NPoint(0,0,0);
            double dist = origin.Distance(this);
            m_x /= dist;
            m_y /= dist;
            m_z /= dist;
        }


       public NPoint(double x, double y, double z)
        {
            m_x = x;
            m_y = y;
            m_z = z;
        }


        public NPoint(NPoint pt)
        {
            m_x = pt.m_x;
            m_y = pt.m_y;
            m_z = pt.m_z;
        }
        public double Distance(NPoint other_pt)
        {
            double dist = Math.Sqrt(((m_x - other_pt.m_x) * (m_x - other_pt.m_x)) +
                                    ((m_y - other_pt.m_y) * (m_y - other_pt.m_y)) +
                                    ((m_z - other_pt.m_z) * (m_z - other_pt.m_z)));
            return dist;

        }


        public static NPoint operator -(NPoint p1)
        {
            NPoint pnt = new NPoint(-p1.m_x, -p1.m_y, -p1.m_z);
            return pnt;
        }
        public static NPoint operator -(NPoint p1, NPoint p2)
        {
            NPoint new_pt = new NPoint(
                (p1.m_x - p2.m_x),
                (p1.m_y - p2.m_y),
                (p1.m_z - p2.m_z));
            return new_pt;
        }
        public static NPoint operator +(NPoint p1, NPoint p2)
        {
            NPoint new_pt = new NPoint(
                (p1.m_x + p2.m_x),
                (p1.m_y + p2.m_y),
                (p1.m_z + p2.m_z));
            return new_pt;
        }
        public double DistanceXY(ref NPoint other)
        {
            double distance = Math.Sqrt((other.m_x - m_x) * (other.m_x - m_x) + (other.m_y - m_y) * (other.m_y - m_y));
            return distance;
        }

        public double Distance(ref NPoint other)
        {
            double distance = Math.Sqrt((other.m_x - m_x) * (other.m_x - m_x) + (other.m_y - m_y) * (other.m_y - m_y)
                + (other.m_z - m_z) * (other.m_z - m_z));
            return distance;
        }
        public double DistanceFromSegment(NPoint st, NPoint end, ref NPoint proj_point)
        {
            proj_point = ProjectOnSegment(st, end);
            double distance = this.Distance(proj_point);
            return distance;
        }
        public double DirectionalDistanceFromSegment(NPoint st, NPoint end)
        {
           
            double distance;
            distance = ( (m_x - st.m_x) * (end.m_y - st.m_y) ) - ((m_y-st.m_y)*(end.m_x-st.m_x));

            return distance;
        }
        public NPoint ProjectOnSegment(NPoint st, NPoint end)
        {
            double u_val;
            NPoint del_0_1 = st - this;
            NPoint del_1_2 = end - st;

            double dist = end.Distance(st);

            u_val = -(del_0_1.m_x * del_1_2.m_x +
                del_0_1.m_y * del_1_2.m_y +
                del_0_1.m_z * del_1_2.m_z) / (dist * dist);
            if(u_val < 0)
            {
                return st;
            }
            if(u_val > 1)
            {
                return end;
            }

            double x_val = st.m_x + (del_1_2.m_x * u_val);
            double y_val = st.m_y + (del_1_2.m_y * u_val);
            double z_val = st.m_z + (del_1_2.m_z * u_val);

            NPoint pt = new NPoint(x_val, y_val, z_val);

            return pt;

        }
        public static NPoint operator *(NMatrix m1, NPoint p1)
        {
            NMatrix m2 = new NMatrix(m1.column_count, 1);
            m2.m_data[0, 0] = p1.m_x;
            m2.m_data[1, 0] = p1.m_y;
            m2.m_data[2, 0] = p1.m_z;
            m2.m_data[3, 0] = 1;

            NMatrix m3 = m1 * m2;
            NPoint p2 = new NPoint(m3.m_data[0, 0], m3.m_data[1, 0], m3.m_data[2, 0]);
            return p2;

        }




        public static void GetDirectionCosines(NPoint vector, ref double angle_z, ref double angle_y)
        {
            angle_z = Math.Atan2(vector.m_y, vector.m_x);
            NPoint org_pnt = new NPoint(0.0, 0.0, 0.0);

            double proj = org_pnt.DistanceXY(ref vector);
            angle_y = Math.Atan2(vector.m_z, proj);
        }

        public static NMatrix GetMatrixRotationAboutAxis(NPoint st_pt, NPoint end_pt, double angle)
        {

            NPoint vec1 = end_pt - st_pt;
            double angle_z = 0;
            double angle_y = 0;
            GetDirectionCosines(vec1, ref angle_z, ref angle_y);
            double rotation_angle_about_z = -angle_z;
            double rotation_angle_about_y = (angle_y - (Math.PI / 2.0));
            NPoint dir = st_pt;
            NMatrix t1 = NMatrix.GetTranslationMatrix(-dir);
            NMatrix r2 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, rotation_angle_about_z);
            NMatrix r3 = NMatrix.GetRotationMatrix(AXIS_TYPE.Y_AXIS, rotation_angle_about_y);
            NMatrix r4 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, angle);
            NMatrix r5 = NMatrix.GetRotationMatrix(AXIS_TYPE.Y_AXIS, -rotation_angle_about_y);
            NMatrix r6 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, -rotation_angle_about_z);
            NMatrix t7 = NMatrix.GetTranslationMatrix(dir);
            NMatrix mat1 = t7 * (r6 * (r5 * (r4 * (r3 * (r2 * (t1 ))))));
            return mat1;

        }

        public NPoint RotateAboutAxis(ref NPoint st_pt, ref NPoint end_pt, double angle)
        {

            NPoint vec1 = end_pt - st_pt;
            double angle_z = 0;
            double angle_y = 0;
            GetDirectionCosines(vec1, ref angle_z, ref angle_y);
            double rotation_angle_about_z = -angle_z;
            double rotation_angle_about_y = (angle_y - (Math.PI / 2.0));
            NPoint dir = st_pt;
            NMatrix t1 = NMatrix.GetTranslationMatrix(-dir);
            NMatrix r2 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, rotation_angle_about_z);
            NMatrix r3 = NMatrix.GetRotationMatrix(AXIS_TYPE.Y_AXIS, rotation_angle_about_y);
            NMatrix r4 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, angle);
            NMatrix r5 = NMatrix.GetRotationMatrix(AXIS_TYPE.Y_AXIS, -rotation_angle_about_y);
            NMatrix r6 = NMatrix.GetRotationMatrix(AXIS_TYPE.Z_AXIS, -rotation_angle_about_z);
            NMatrix t7 = NMatrix.GetTranslationMatrix(dir);
            NPoint new_pnt = t7 * (r6 * (r5 * (r4 * (r3 * (r2 * (t1 * this))))));
            return new_pnt;

        }

        internal NPoint CrossProduct(NPoint vec2)
        {
            // find cross product
            double x_val, y_val, z_val;
            x_val = (m_y) * (vec2.m_z) - (m_z) * (vec2.m_y);
            y_val = (m_z) * (vec2.m_x) - (m_x) * (vec2.m_z);
            z_val = (m_x) * (vec2.m_y) - (m_y) * (vec2.m_x);

            return new NPoint(x_val, y_val, z_val);
 
        }





        //public void Display(Canvas can)
        //{

        //    int dotSize = 3;
        //    byte gr = GreyVal();
        //    Color cr = Color.FromRgb(gr, gr, gr);
        //    Ellipse currentDot = new Ellipse();
        //    currentDot.Stroke = new SolidColorBrush(cr);
        //    currentDot.StrokeThickness = 3;
        //    Canvas.SetZIndex(currentDot, 3);
        //    currentDot.Height = dotSize;
        //    currentDot.Width = dotSize;
        //    //          currentDot.Fill = new SolidColorBrush(Colors.Green);

        //    currentDot.Fill = new SolidColorBrush(cr);
        //    Point pt = new Point();
        //    double y = m_ang1 * m_y_angle_height_factor;
        //    double x = m_ang2 * m_x_angle_height_factor;
        //    currentDot.Margin = new Thickness(x,y,0, 0); // Sets the position.
        //    can.Children.Add(currentDot);


        //}
    }
}

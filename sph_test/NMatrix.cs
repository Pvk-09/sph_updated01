using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sph_test
{
    public enum AXIS_TYPE { X_AXIS, Y_AXIS, Z_AXIS };

    public class NMatrix
    {
        public int row_count = 4;
        public int column_count = 4;
        public double[,] m_data;
        public NMatrix(int r_c, int c_c, double[,] data)
        {
            row_count = r_c;
            column_count = c_c;
            m_data = data.Clone() as double[,];
        }

        public NMatrix(int r_c, int c_c)
        {
            row_count = r_c;
            column_count = c_c;
            m_data = new double[r_c, c_c];
        }

        public static NMatrix operator *(NMatrix m1, NMatrix m2)
        {
            NMatrix new_mat = new NMatrix(m2.row_count, m2.column_count);
            for (int i = 0; i < m2.row_count; i++)
            {
                for (int j = 0; j < m2.column_count; j++)
                {
                    new_mat.m_data[i, j] = 0;
                    for (int k = 0; k < m1.column_count; k++)
                    {
                        new_mat.m_data[i, j] += m1.m_data[i, k] * m2.m_data[k, j];
                    }
                }
            }
            return new_mat;
        }
        public static NMatrix GetRotationMatrix(AXIS_TYPE axis, double angle)
        {
            double[,] data;
            switch (axis)
            {
                case AXIS_TYPE.Z_AXIS:
                    {
                        data = new double[4, 4]
                            {
                                {    Math.Cos(angle), -Math.Sin(angle),  0,    0 },
                                {    Math.Sin(angle),  Math.Cos(angle),  0,    0 },
                                {     0,                 0,              1,    0 },
                                {     0,                 0,              0,    1 }

                           };

                        break;
                    }
                case AXIS_TYPE.X_AXIS:
                    {
                        data = new double[4, 4]
                            {
                                {     1,                 0,              0,    0 },
                                {    Math.Cos(angle), -Math.Sin(angle),  0,    0 },
                                {    Math.Sin(angle),  Math.Cos(angle),  0,    0 },
                                {     0,                 0,              0,    1 }

                           };

                        break;
                    }
                case AXIS_TYPE.Y_AXIS:
                    {
                        data = new double[4, 4]
                            {
                                {    Math.Cos(angle),   0,    Math.Sin(angle),    0 },
                                {     0,                1,    0,                  0 },
                                {    -Math.Sin(angle),  0,    Math.Cos(angle),    0 },
                                {     0,                0,    0,                  1 }

                           };

                        break;
                    }
                default:
                    data = null;
                    break;
            }
            NMatrix new_mat = new NMatrix(4, 4, data);
            return new_mat;

        }

        public static NMatrix GetTranslationMatrix(NPoint trans_vector)
        {
            double[,] data;
            data = new double[4, 4]
                {
                    {     1,   0,  0,    trans_vector.m_x },
                    {     0,   1,  0,    trans_vector.m_y },
                    {     0,   0,  1,    trans_vector.m_z },
                    {     0,   0,  0,    1                }

                };

            NMatrix new_mat = new NMatrix(4, 4, data);
            return new_mat;

        }


    }

}

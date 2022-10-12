using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sph_test
{
    public class NLine
    {
        public NPoint m_start_point = null;
        public NPoint m_end_point = null;
        public NLine(NPoint st_pt, NPoint end_point)
        {
            m_start_point = st_pt;
            m_end_point = end_point;
        }
     }
}

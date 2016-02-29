using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class _2_Question : Form
    {
        public _2_Question()
        {
            InitializeComponent();
        }

        private void _2_Question_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 8, 24, 3, 20, 1, 17};
            //int[] A = new int[] { 7, 21, 3, 42, 3, 7};
            int Result = solution(A);
        }

        public int solution(int[] A)
        {
            long MinDistance = Int64.MaxValue;

            Array.Sort(A);

            for (long i = 0; i < A.Length - 1; i++)
            {
                MinDistance = Math.Abs(A[i] - A[i + 1]) < MinDistance ? Math.Abs(A[i] - A[i + 1]) : MinDistance;
            }

            return unchecked((int)MinDistance);
        }
    }
}

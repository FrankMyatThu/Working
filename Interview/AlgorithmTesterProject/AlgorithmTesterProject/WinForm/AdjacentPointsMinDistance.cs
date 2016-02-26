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
    public partial class AdjacentPointsMinDistance : Form
    {
        public AdjacentPointsMinDistance()
        {
            InitializeComponent();
        }

        private void AdjacentPointsMinDistance_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 0,3,3,7,5,3,11,1 };
            int Result = solution(A, 10000);
        }

        // returns the minimum distance between indices of this array that have adjacent values. 
        public int solution(int[] A, int N)
        {
            long MinDistance = 0;
            
            // The function should return −2 if no adjacent indices exist.
            if (A.Length < 2)
            {
                return -2;
            }

            Array.Sort(A);

            for (int i = 0; i < A.Length - 1 ; i++)
            {
                MinDistance = Math.Abs(A[i] - A[i + 1]) < MinDistance ? Math.Abs(A[i] - A[i + 1]) : MinDistance;
            }

            // The function should return −1 if the minimum distance is greater than 100,000,000. 
            if (MinDistance > 100000000)
            {
                return -1;
            }

            return unchecked((int)MinDistance); ;
        }
    }
}

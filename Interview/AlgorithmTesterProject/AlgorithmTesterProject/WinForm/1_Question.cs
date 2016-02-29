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
    public partial class _1_Question : Form
    {
        public _1_Question()
        {
            InitializeComponent();
        }

        private void _1_Question_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 2, 1, 5, -6, 9};
            int result = solution(A);
        }

        public int solution(int[] A)
        {
            long NumberOfPair = 0;

            for (long i = 0; i <= A.Length - 1; i++) {

                long ValueA = A[i];
                for (long j = (i+1); j <= A.Length - 1; j++ ) { 
                    long ValueB = A[j];
                    long SumValueAB = ValueA + ValueB; 

                    if (SumValueAB % 2 == 0){
                        /// Even
                        NumberOfPair++;
                    }
                    
                }

            }

            if (NumberOfPair > 1000000000)
            {
                return -1;
            }

            return unchecked((int)NumberOfPair);
        }
    }
}

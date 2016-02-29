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
    public partial class ArithSliceCount : Form
    {
        public ArithSliceCount()
        {
            InitializeComponent();
        }

        private void ArithSliceCount_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { -1, 1, 3, 3, 3, 2, 1, 0 };
            int result = solution(A);
        }

        //  returns the number of arithmetic slices in A
        public int solution(int[] A)
        {
            long ReturnValue = 0;

            long DifferentValue = 0;
            long ArithmeticSequenceCount = 0;
            long ArithmeticSliceCount = 0;
            for (long i = 0; i < A.Length - 1; i++) 
            {
                if (Math.Abs(A[i] - A[i + 1]) == DifferentValue)
                {
                    ArithmeticSequenceCount++;
                }
                else
                {
                    ArithmeticSequenceCount = 0;
                }
                DifferentValue = Math.Abs(A[i] - A[i + 1]);

                if (ArithmeticSequenceCount >= 1) {
                    ArithmeticSliceCount++;
                }

                
            }

            ReturnValue = ArithmeticSliceCount;

            // The function should return −1 if the result exceeds 1,000,000,000            
            if (ReturnValue > 1000000000)
            {
                return -1;
            }
            return unchecked((int)ReturnValue);
        }
    }
}

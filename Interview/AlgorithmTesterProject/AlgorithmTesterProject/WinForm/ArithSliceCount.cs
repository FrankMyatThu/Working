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
            //int[] A = new int[] { -1, 1, 3, 3, 3, 2, 1, 0 };
            int[] A = new int[] { 3, 2, 1, 0 };
            int result = solution(A);
        }

        public int solution(int[] A)
        {
            long DifferentValue = 0;            
            List<PairClass> ListPairArray = new List<PairClass>();
            for (long i = 0; i < A.Length - 1; i++)
            {
                long ValueA = A[i];
                long InnerLoopCount = 1;
                for (long j = i + 1; j < A.Length; j++ ) {
                    long ValueB = A[j];
                    if (InnerLoopCount >= 2)
                    {
                        if (Math.Abs(ValueA - ValueB) == DifferentValue)
                        {   
                            PairClass _PairClass = new PairClass();
                            _PairClass.StartIndex = i;
                            _PairClass.EndIndex = j;
                            ListPairArray.Add(_PairClass);                            
                        }
                        else
                        {
                            i = j - 1;
                        }
                    }
                    DifferentValue = Math.Abs(ValueA - ValueB);
                    ValueA = ValueB;
                    InnerLoopCount++;
                }
            }

            // The function should return −1 if the result exceeds 1,000,000,000            
            if (ListPairArray.Count > 1000000000)
            {
                return -1;
            }
            return unchecked((int)ListPairArray.Count);
        }

        class PairClass 
        {
            public long StartIndex { get; set; }
            public long EndIndex { get; set; }
        }
    }
}

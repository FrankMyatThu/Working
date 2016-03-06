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
    public partial class BreakTheChain : Form
    {
        public BreakTheChain()
        {
            InitializeComponent();
        }

        private void BreakTheChain_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 5, 2, 4, 6, 3, 7 };
            int Result = solution(A);
        }

        int solution(int[] A)
        {   
            int P = 0;
            int Q = 0;
            List<IndexPair> List_IndexPair = new List<IndexPair>();
            for (int i = 1; i < A.Length - 1; i++)
            {
                P = i;
                for (int j = 2; j < A.Length - 1; j++) {
                    Q = j;
                    // P, Q (0 < P < Q < N − 1, Q − P > 1)
                    if ((Q - P) > 1) {
                        IndexPair _IndexPair = new IndexPair();
                        _IndexPair.StartIndex = P;
                        _IndexPair.EndIndex = Q;
                        List_IndexPair.Add(_IndexPair);
                    }
                }
            }

            int[] ArrayToOrder = new int[List_IndexPair.Count];
            for (int i = 0; i < List_IndexPair.Count; i++) {
                int ValueA = A[List_IndexPair[i].StartIndex];
                int ValueB = A[List_IndexPair[i].EndIndex];
                ArrayToOrder[i] = ValueA + ValueB;
            }

            Array.Sort(ArrayToOrder);
            return ArrayToOrder[0];
        }

        class IndexPair
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
        }
    }
}

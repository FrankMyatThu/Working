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
    public partial class AreaOfSum : Form
    {
        public AreaOfSum()
        {
            InitializeComponent();
        }

        private void AreaOfSum_Load(object sender, EventArgs e)
        {
            int result = solution(-4, 1, 2, 6, 0, -1, 4, 3);
        }

        // returns the area of the sum of the rectangles
        public int solution(int K, int L, int M, int N, int P, int Q, int R, int S)
        {
            long ReturnValue = 0;
            
            // K, L, M, N, P, Q, R and S are integers within the range [−2,147,483,648..2,147,483,647]
            long _K = K;
            long _L = L;
            long _M = M;
            long _N = N;
            long _P = P;
            long _Q = Q;
            long _R = R;
            long _S = S;

            long left = Math.Max(_K, _P);
            long right = Math.Min(_M, _R);
            long bottom = Math.Max(_L, _Q);
            long top = Math.Min(_N, _S);

            if (left < right && bottom < top)
            {
                long interSection = (right - left) * (top - bottom);
                long unionArea = ((_M - _K) * (_N - _L)) + ((_R - _P) * (_S - _Q))
                        - interSection;
                ReturnValue = unionArea;    
            }

            // return −1 if the area of the sum exceeds 2,147,483,647
            if (ReturnValue > 2147483647) {
                return -1;
            }

            return unchecked((int)ReturnValue);

        }
    }
}

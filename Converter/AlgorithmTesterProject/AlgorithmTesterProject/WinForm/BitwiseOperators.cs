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
    public partial class BitwiseOperators : Form
    {
        /// <summary>
        /// http://www.tutorialspoint.com/csharp/csharp_bitwise_operators.htm
        /// </summary>
        public BitwiseOperators()
        {
            InitializeComponent();
        }

        private void BitwiseOperators_Load(object sender, EventArgs e)
        {
            //int Result = solution(5, 7);
            int Result = solution(7, 9);
        }

        // given two non-negative integers M and N, returns their bitwise AND product.
        public int solution(int M, int N)
        {
            //M ≤ N
            if (M > N) throw new Exception("M is greater then N");

            //M and N are integers within the range [0..2,147,483,647];            
            long _M = M;
            long _N = N;
            long ReturnValue = 0;

            long CurrentValue = _M;
            for (long i = _M; i < _N; i++)
            {   
                CurrentValue = CurrentValue & (i + 1);   
            }
            ReturnValue = CurrentValue;

            return unchecked((int)ReturnValue);
        }
    }
}

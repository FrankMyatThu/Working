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
    public partial class NumberSorting : Form
    {
        public NumberSorting()
        {
            InitializeComponent();
        }

        private void NumberSorting_Load(object sender, EventArgs e)
        {
            int[] arr = {64, 34, 25, 12, 22, 11, 90};

            bubbleSort(arr, arr.Length);

            Console.WriteLine("Sorted array : ");
            for (int i = 0; i < arr.Length; i++)
                Console.Write(arr[i] + " ");
        }

        /// <summary>
        /// https://www.geeksforgeeks.org/recursive-bubble-sort/
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        void bubbleSort(int[] arr,int n)
        {
            // Base case
            if (n == 1)
                return;

            // One pass of bubble 
            // sort. After this pass,
            // the largest element
            // is moved (or bubbled) 
            // to end.
            for (int i = 0; i < n - 1; i++)
                if (arr[i] > arr[i + 1])
                {
                    // swap arr[i], arr[i+1]
                    int temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }

            // Largest element is fixed,
            // recur for remaining array
            bubbleSort(arr, n - 1);
        }


    }
}

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
    public partial class OrderingClosestPoints : Form
    {
        public OrderingClosestPoints()
        {
            InitializeComponent();
        }

        private void OrderingClosestPoints_Load(object sender, EventArgs e)
        {
            List<Point_Double> List_PointDouble = new List<Point_Double>();
            List_PointDouble.Add(new Point_Double() { X = 0.0, Y = 0.0 });
            List_PointDouble.Add(new Point_Double() { X = 10.1, Y = -10.20 });
            List_PointDouble.Add(new Point_Double() { X = 33.4, Y = 33.4 });
            List_PointDouble.Add(new Point_Double() { X = -20.0, Y = 20.1 });
            List_PointDouble.Add(new Point_Double() { X = 75.5, Y = 79.10 });

            List<Point_Double> SortedList_Point_Double = OrderByDistance(List_PointDouble);
            var tosee = SortedList_Point_Double;
        }


        /// <summary>
        /// https://codereview.stackexchange.com/questions/139059/order-a-list-of-points-by-closest-distance/139453#139453
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static double Distance(Point_Double p1, Point_Double p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private List<Point_Double> OrderByDistance(List<Point_Double> pointList)
        {
            var orderedList = new List<Point_Double>();

            var currentPoint = pointList[0];

            while (pointList.Count > 1)
            {
                orderedList.Add(currentPoint);
                pointList.RemoveAt(pointList.IndexOf(currentPoint));

                var closestPointIndex = 0;
                var closestDistance = double.MaxValue;

                for (var i = 0; i < pointList.Count; i++)
                {
                    var distance = Distance(currentPoint, pointList[i]);
                    if (distance < closestDistance)
                    {
                        closestPointIndex = i;
                        closestDistance = distance;
                    }
                }

                currentPoint = pointList[closestPointIndex];
            }

            // Add the last point.
            orderedList.Add(currentPoint);

            return orderedList;
        }
    }

    public class Point_Double
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}

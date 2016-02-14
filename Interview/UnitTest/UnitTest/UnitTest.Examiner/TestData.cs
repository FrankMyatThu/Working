using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Examiner
{
    /// <summary>
    ///  Data driven test.
    /// </summary>
    public class TestData
    {
        public static IEnumerable TestCases
        {
            get {
                ArrayList _ArrayList = new ArrayList();
                _ArrayList.Add(new TestCaseData(10, 10, 100));
                _ArrayList.Add(new TestCaseData(20, 20, 400));
                _ArrayList.Add(new TestCaseData(30, 30, 900));
                _ArrayList.Add(new TestCaseData(5, 5, 25));
                foreach (TestCaseData _TestCaseData in _ArrayList)
                {
                    yield return _TestCaseData;
                }
            }
        }
    }
}

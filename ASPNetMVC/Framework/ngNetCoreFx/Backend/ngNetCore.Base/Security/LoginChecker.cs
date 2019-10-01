using ngNetCore.Base.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngNetCore.Base.Security
{
    public class LoginChecker
    {
        public bool IsUserIdle(DateTime UserLastRequestedTime)
        {
            bool IsIdle = false;
            try
            {
                DateTime CurrentDateTime = DateTime.Now;
                double TotalTime = (CurrentDateTime - UserLastRequestedTime).TotalMinutes;
                if (TotalTime > Convert.ToDouble(AppConfiger.ApplicationTokenLifeTime))
                {
                    IsIdle = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsIdle;
        }
    }
}

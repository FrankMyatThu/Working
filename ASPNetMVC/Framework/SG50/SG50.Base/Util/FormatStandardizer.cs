using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Base.Util
{
    public class FormatStandardizer
    {   
        /// <summary>
        ///  ref http://stackoverflow.com/a/2385967/900284
        /// </summary>
        public const string Name_SingleWord = "[A-Za-z,.'-]+";
        public const string Name_MultiWord = "[A-Za-z ,.'-]+";

        /// <summary>
        /// Ref http://stackoverflow.com/a/12019115/900284
        /// </summary>
        public const string UserName = "(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])";

        /// <summary>
        /// AlphaNumericFormat
        /// </summary>
        public const string AlphaNumeric = "[A-Za-z0-9]+";

        /// <summary>
        /// Ref http://stackoverflow.com/a/19605207/900284
        /// This regex will enforce these rules:
        /// At least one upper case english letter
        /// At least one lower case english letter
        /// At least one digit
        /// At least one special character        
        /// </summary>
        public const string Password = "(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).*";
    }
}

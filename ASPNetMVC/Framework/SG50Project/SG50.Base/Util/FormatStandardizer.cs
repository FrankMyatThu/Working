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
        public const string Client_Name_SingleWord = @"/^[A-Za-z0-9,.'-]+$/";
        public const string Server_Name_SingleWord = "[A-Za-z0-9,.'-]+";
        public const string Client_Name_MultiWord = @"/^[A-Za-z0-9 ,.'-]+$/";
        public const string Server_Name_MultiWord = "[A-Za-z0-9 ,.'-]+";

        /// <summary>
        /// Ref http://stackoverflow.com/a/12019115/900284
        /// </summary>                
        public const string Client_UserName = @"/^(?![_.])(?!.*[_.]{2})[a-z0-9._]*[a-z0-9]$/i";
        public const string Server_UserName = "(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])";        

        /// <summary>
        /// AlphaNumericFormat
        /// </summary>
        public const string Client_AlphaNumeric = @"/^[A-Za-z0-9]+$/";
        public const string Server_AlphaNumeric = "[A-Za-z0-9]+";

        /// <summary>
        /// Email Format
        /// </summary>
        public const string Client_EmailFormat = @"/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,4}$/i";
        public const string Server_EmailFormat = @"[A-Za-z0-9._-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}";

        /// <summary>
        /// Ref http://stackoverflow.com/a/19605207/900284
        /// This regex will enforce these rules:
        /// At least one upper case english letter
        /// At least one lower case english letter
        /// At least one digit
        /// At least one special character        
        /// </summary>
        public const string Client_Password = @"/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).*$/";
        public const string Server_Password = "(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).*";

        /// <summary>
        /// Author Frank Myat Thu
        /// https://regex101.com/r/cM9nZ5/2
        /// </summary>
        public const string Server_OrderByClause = @"([A-Za-z0-9_]+\s(ASC|DESC),?\s?){1,}";

        /// <summary>
        /// ref http://stackoverflow.com/a/11040993
        /// </summary>
        public const string Server_GuidFormat = "\b[A-Fa-f0-9]{8}(?:-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}\b";
    }
}

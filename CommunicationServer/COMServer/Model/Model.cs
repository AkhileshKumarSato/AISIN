using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPR_App
{

    #region User Master
    public class User : Common
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Group { get; set; }
    }
    #endregion
}

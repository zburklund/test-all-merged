using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CASP
{
    public partial class Default : System.Web.UI.Page
    {
        /*
        Host: https://login.case.edu
        Context: /cas/
        Login URL: https://login.case.edu/cas/login
        Logout URL: https://login.case.edu/cas/logout
        Validate URL: https://login.case.edu/cas/validate (CAS protocol version 1)
        Service Validate URL: https://login.case.edu/cas/serviceValidate (CAS protocol version 2)
        Service Validate URL: https://login.case.edu/cas/p3/serviceValidate (CAS protocol version 3)
        Running Version: 4.0.1
        CAS Protocol Versions Supported: 1, 2 and 3
        **/
       

        protected void Page_Load(object sender, EventArgs e)
        {            

            bool isAuth = ActiveDirectory.isUserAuthorized("bdm4", "nurs-dept-it");
        }
    }   
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Net;
using System.Xml.Linq;

namespace CASP
{
    public partial class Default : System.Web.UI.Page
    {
        /* 12-2015 Dox.. See Dox.txt in this repo to see the documentation.
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

        const string LOGIN_URL = "https://login.case.edu/cas/login";
        const string SERVICE_VALIDATE_URL = "https://login.case.edu/cas/p3/serviceValidate";


        protected void Page_Load(object sender, EventArgs e)
        {
            string ticket = Request.QueryString["ticket"];
            string service = Request.Url.GetLeftPart(UriPartial.Path);

            //If there is no ticket in the query string. We need to login to SSO
            if (String.IsNullOrEmpty(ticket))
            {
                string login = LOGIN_URL + "?service=" + service;
                Response.Redirect(login);
                return;
            }

            //Back from SSO there should be a ticket to validate.
            string validateurl = SERVICE_VALIDATE_URL + "?ticket=" + ticket + "&service=" + service;
            using (StreamReader reader = new StreamReader(new WebClient().OpenRead(validateurl)))
            {                
                //string validResponse = reader.ReadToEnd(); //just for debugging. 
                /* Valid Service Response
                 * <cas:serviceResponse xmlns:cas='http://www.yale.edu/tp/cas'>
                        <cas:authenticationSuccess>
                            <cas:user>abc12</cas:user>
                            <cas:attributes>
                                <cas:user>abc12</cas:user>
                            </cas:attributes>
                         </cas:authenticationSuccess>
                    </cas:serviceResponse> 
                */

                XDocument doc = XDocument.Load(reader);
              
                
                
            }
            

            //bool isAuth = ActiveDirectory.isUserAuthorized("bdm4", "nurs-dept-it");
        }
    }

    
}
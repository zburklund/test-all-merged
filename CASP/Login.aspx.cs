using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO; //Stream reader
using System.Net; //WebClient
using System.Xml.Linq; //XML parsing of the CAS feed
using System.Web.Security; //FormsAuthentication

namespace CASP
{
    public partial class Default : System.Web.UI.Page
    {
        /* 12-2015 Dox.. See Dox.txt in this repo to see the documentation.
         * 
         * By Ben Margevicius bdm4@case.edu 12/21/2015
         * 
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
            try
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
                string userID = null;

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

                    XDocument doc = XDocument.Load(reader); //Load our Stream into XML 

                    //In order to query the user element we need to get the namespace first. This code is not mine. 
                    var namespaces = doc.Root.Attributes().
                                    Where(a => a.IsNamespaceDeclaration).
                                    GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                                            a => XNamespace.Get(a.Value)).
                                    ToDictionary(g => g.Key,
                                                 g => g.First());

                    XNamespace cas = namespaces["cas"]; //Get the namespace from the previous query

                    //Now get query for namespace + user element to get the user ID.
                    userID = (string)(from el in doc.Descendants(cas + "user")
                                      select el).First();

                    //From here you can do what you want
                    //Session["CASNetworkID"] = userID; //The first version if CASP stored the user ID in a session variable.
                    //For this site I am going to use forms authentication 


                }
                if (!String.IsNullOrEmpty(userID))
                {
                    bool isAuth = ActiveDirectory.isUserAuthorized("bdm4", "staff@nursing.case.edu", false); //Authorize our user if necessary.
                    //bool isAuth = ActiveDirectory.isUserAuthorized("bdm4", new List<string> {"staff@nursing.case.edu", "faculty@nursing.case.edu"}); //Authorize our user if necessary.
                    if (isAuth)
                    {
                        FormsAuthentication.RedirectFromLoginPage(userID, false);
                    }
                    else
                    {
                        throw new HttpException(403, "We're sorry but you are not permitted to access this application.");
                    }
                }
                else
                {
                    throw new HttpException(403, "Could not authenticate with CAS");
                }
            }
            catch (Exception ex)
            {
                ErrorLbl.Text = ex.Message;
                Trace.Write(ex.Message);
            }            
        }
        protected void SignoutBtn_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("https://login.case.edu/cas/logout");
        }
    }

    
}
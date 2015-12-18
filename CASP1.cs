using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Net;

namespace CASP
{
    /**
 * CASP.cs
 * CAS over ASP.NET!
 * 
 * Created by John Tantalo, john.tantalo@case.edu
 * Case Western Reserve University
 * 
 * Modification History:
 * 
 * 12/09/05 jnt5, created class
 * 12/12/05 jnt5, removed cookie check
 * 				  stores CASNetworkID in session instead of cache
 * 				  clears Page session variable after ticket verification
 * 12/13/05 jnt5, removed Page session variable
 * 				  fixed bug which would cause loop due to incorrect service parameter
 * 04/04/06 jnt5, adapted serviceURL code courtesy Ali Cakmak
 * 04/10/06 jnt5, added new comments
 * 12/17/2015 moved to github, bdm4. This uses 
 * References:
 * 
 * http://wiki.case.edu/Central_Authentication_Service
 * https://clearinghouse.ja-sig.org/wiki/display/CAS/CAS+2.0+Protocol+Specification*/
    /**
     * CASP general usage:
     * 
     *	private void Page_Load(object sender, System.EventArgs e)
     *	{
     *		String NetworkID = CASP.Authenticate( "https://login.case.edu/cas/login", "https://login.case.edu/cas/validate", this ) ;
     *	}
     */

    public static class CASP
    {
        /**
         * Authenticates a user with the given login and validation pages. After authentication
         * the user's browser is redirected to the original page.
         */

        public static String Authenticate(String LoginURL, String ValidateURL, Page Page)
        {
            return Authenticate(LoginURL, ValidateURL, Page, Page.Request.Url.AbsoluteUri.Split('?')[0]);
        }

        /**
         * Authenticates a user with the given login and validation pages. After authentication
         * the user's browser is redirected to the location given as the service URL.
         */

        public static String Authenticate(String LoginURL, String ValidateURL, Page Page, String ServiceURL)
        {
            if (Page.Session["CASNetworkID"] != null) // user already logged in
                return Page.Session["CASNetworkID"].ToString();
            else // user hasn't logged in
            {
                if (Page.Request.QueryString["ticket"] != null) // ticket received
                {
                    try // read ticket and request validation
                    {
                        StreamReader Reader = new StreamReader(new WebClient().OpenRead(ValidateURL + "?ticket=" + Page.Request.QueryString["ticket"] + "&service=" + ServiceURL));

                        if ("yes".Equals(Reader.ReadLine())) // ticket validated
                        {
                            // store network id in sesssion, return value

                            return (String)(Page.Session["CASNetworkID"] = Reader.ReadLine());
                            
                        }
                    }
                    catch (WebException) { }
                }

                // ticket was invalid, or didn't exist, so request ticket

                Page.Response.Redirect(LoginURL + "?service=" + ServiceURL, true);
                return null;
            }
        }
    }
}

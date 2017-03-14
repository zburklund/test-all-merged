using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;

namespace CASP
{
    public class ActiveDirectory
    {
        const string caseDomain = "ads.case.edu";
        /// <summary>
        /// Returns the list of ADS groups a person is in
        /// </summary>
        /// <param name="userName">the persons username</param>
        /// <returns>A list of strings that contains all of thier ads groups.</returns>
        public static List<string> GetUserGroupsFromADS(string userName)
        {
            List<string> results = new List<string>();

            using (var context = new PrincipalContext(ContextType.Domain, caseDomain))
            {
                using (PrincipalSearchResult<Principal> source = UserPrincipal.FindByIdentity(context, userName).GetGroups(context))
                {
                    foreach (var group in source)
                    {
                        results.Add(group.SamAccountName);
                    }
                }
            }
            return results;
        }
        /// <summary>
        /// If a user is in an specified active directory group grant them authorization
        /// </summary>
        /// <param name="userName">the username you want to check</param>
        /// <param name="groupName">the name of the ads group</param>
        /// <param name="recursiveSearch">If you want to search groups inside groups. Default is true.</param>
        /// <returns>Return true if userName is in groupName</returns>
        public static bool isUserAuthorized(string userName, string groupName, bool recursiveSearch = true)
        {
            bool isAuth = false;

            using (var context = new PrincipalContext(ContextType.Domain, caseDomain))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName))
                {
                    isAuth = (group != null) && group.GetMembers(recursiveSearch).Any(user => user.SamAccountName == userName);
                }
            }
            return isAuth;
        }


        public static bool isUserAuthorized(string userName, List<string> groupNames, bool recursiveSearch = true)
        {
            bool isAuth = false;
            foreach (var g in groupNames)
            {
                isAuth = isUserAuthorized(userName, g, recursiveSearch);
                if (isAuth) break;
            }
            return isAuth;
        }
    }
}
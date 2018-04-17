using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Lib.Common.Web
{
    public class FormsAuthenticationHelper
    {
        /// <summary>
        /// Creates the new ticket.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <param name="userData">The user data.</param>
        /// <returns></returns>
        public static FormsAuthenticationTicket CreateNewTicket(string username, bool isPersistent, string userData)
        {
            DateTime issueTime = DateTime.Now;
            DateTime expirationTime = issueTime.AddMinutes(30);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1
                , username
                , issueTime
                , expirationTime
                , isPersistent
                , userData);
            return ticket;
        }

        /// <summary>
        /// Create a new ticket based on an old ticket
        /// The issue time and expiration time are set to new. 
        /// The userdata is set to a given value
        /// </summary>
        /// <param name="oldTicket"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static FormsAuthenticationTicket CreateNewTicket(FormsAuthenticationTicket oldTicket, string userData)
        {
            DateTime issueTime = DateTime.Now;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                oldTicket.Version
                , oldTicket.Name
                , issueTime
                , oldTicket.Expiration
                , oldTicket.IsPersistent
                , userData);
            return ticket;
        }

        /// <summary>
        /// Adds the ticket to response.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        public static void AddTicketToResponse(FormsAuthenticationTicket ticket, bool isUpdate)
        {
            string encryptedTicketStr = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicketStr);

            if (ticket.IsPersistent)
            {
                DateTime issueTime = DateTime.Now;
                DateTime expirationTime = issueTime.AddDays(1);
                cookie.Expires = expirationTime;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        public static void SignOut()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-2);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            }

            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Creates the member ticket.
        /// </summary>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetMemberTicket(string userName)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket authTicket = null;

            if (authCookie == null && !string.IsNullOrEmpty(userName))
            {
                CreateTicket(false, userName, userName);
                authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            }

            try
            {
                if (authCookie != null)
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
            }
            catch
            {
            }

            return authTicket;
        }

        /// <summary>
        /// Creates the ticket.
        /// </summary>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userData">The user data.</param>
        public static void CreateTicket(bool rememberMe, string userName, string userData)
        {
            FormsAuthenticationTicket authTicket = CreateNewTicket(userName, rememberMe, userData);
            AddTicketToResponse(authTicket, false);
        }

        /// <summary>
        /// Creates the ticket.
        /// </summary>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        public static void CreateTicket(bool rememberMe, string userName, string userData, bool isUpdate)
        {
            FormsAuthenticationTicket ticket = GetMemberTicket(userName);
            FormsAuthenticationTicket authTicket = CreateNewTicket(ticket, userData);
            AddTicketToResponse(authTicket, isUpdate);
        }
    }
}

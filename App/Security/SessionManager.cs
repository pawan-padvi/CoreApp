using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Security
{
    public class SessionManager
    {
        private readonly ISession _session = null;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private const string _userSession = "User";
        public string AppPath = "";

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        #region :: General Session Methods ::

        public void AddSesssion(string _key, string _value)
        {
            _session.SetString(_key, _value);
        }
        public string GetSesssion(string _key)
        {
            if (!string.IsNullOrEmpty(_session.GetString(_key)))
            {
                return _session.GetString(_key);
            }
            else
            {
                return null;
            }
        }
        public void ClearSession(string _key)
        {
            _session.Remove(_key);
        }
        public void ClearSession()
        {
            _session.Clear();
        }
        #endregion

        #region :: User Session Methods ::
        public void AddUserSesssion(UserSession _value)
        {
            _session.SetString(_userSession, JsonConvert.SerializeObject(_value));
        }
        public void ClearUserSession()
        {
            _session.Remove(_userSession);
        }
        public UserSession GetUserSesssion()
        {
            if (!string.IsNullOrEmpty(_session.GetString(_userSession)))
            {
                return JsonConvert.DeserializeObject<UserSession>(_session.GetString(_userSession));
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
    public class UserSession
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MailID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}

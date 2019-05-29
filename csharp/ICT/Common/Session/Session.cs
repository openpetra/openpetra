//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2019 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Web.SessionState;
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.DB;

using Newtonsoft.Json;

namespace Ict.Common.Session
{
    /// <summary>
    /// Static class for storing sessions.
    /// we are using our own session handling,
    /// since the mono server cannot handle concurrent requests in one session
    /// see also http://serverfault.com/questions/324033/how-do-i-get-concurrent-asp-net-on-linux
    /// and we want to store sessions in the database
    /// </summary>
    public class TSession
    {
        [ThreadStaticAttribute]
        private static string FSessionID;

        private const int SessionValidHours = 24;

        /// get the current session id. if it is not stored in the http context, check the thread
        private static string FindSessionID()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request.Cookies["OpenPetraSessionID"] != null))
            {
                TLogging.LogAtLevel(4, "using session id from HttpContext");
                return HttpContext.Current.Request.Cookies["OpenPetraSessionID"].Value;
            }

            // Session ID not found in the http context, checking the thread
            TLogging.LogAtLevel(4, "(HttpContext.Current != null) : " + (HttpContext.Current != null).ToString());

            if (HttpContext.Current != null)
            {
                TLogging.LogAtLevel(4, "(HttpContext.Current.Request.Cookies[OpenPetraSessionID] != null) : " +
                    (HttpContext.Current.Request.Cookies["OpenPetraSessionID"] != null).ToString());
            }

            // only look in thread if there is no HttpContext.Current; otherwise Threads are reused.
            if (HttpContext.Current == null)
            {
                string sessionId = FSessionID;

                if ((sessionId != null) && (sessionId.Length > 0))
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in thread. SessionID = " + sessionId);

                    return sessionId;
                }

                TLogging.LogAtLevel(1,
                    "FindSessionID: Session ID not found in the thread!!! thread id: " + Thread.CurrentThread.ManagedThreadId.ToString());
            }
            else
            {
                TLogging.LogAtLevel(1, "FindSessionID: Session ID not found in the http context");
            }

            return string.Empty;
        }

        /// <summary>
        /// set the session id for this current thread
        /// </summary>
        /// <param name="ASessionID"></param>
        public static void InitThread(string ASessionID)
        {
            TLogging.LogAtLevel(1, "Running InitThread for ASessionID = " + ASessionID);
            TLogging.LogAtLevel(1, "thread id " + Thread.CurrentThread.ManagedThreadId.ToString());

            FSessionID = ASessionID;
        }

        private static bool HasValidSession(string ASessionID, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("HasValidSession", ADataBase);

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;
            bool Result = false;

            db.WriteTransaction(ref t,
                ref SubmissionOK,
                delegate
                {
                    string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ? and s_valid_until_d > NOW()";
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = ASessionID;
                    
                    if (Convert.ToInt32(db.ExecuteScalar(sql, t, parameters)) == 1)
                    {
                        Result = true;
                    }
                    else
                    {
                        // clean all old sessions
                        sql = "DELETE FROM PUB_s_session WHERE s_valid_until_d > NOW()";
                        db.ExecuteNonQuery(sql, t);
                        SubmissionOK = true;
                    }
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return Result;
        }

        private static SortedList <string, object> GetSession(string ASessionID, TDataBase ADataBase)
        {
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[0].Value = ASessionID;

            string sql = "SELECT s_session_values_c FROM s_session WHERE s_session_id_c = ?";
            string jsonString = ADataBase.ExecuteScalar(sql, ADataBase.Transaction, parameters).ToString();
            return JsonConvert.DeserializeObject<SortedList <string, object>>(jsonString);
        }

        private static SortedList <string, object> GetSession(TDataBase ADataBase = null)
        {
            string sessionID = GetSessionID();
            SortedList <string, object> result = null;
            TDataBase db = DBAccess.Connect("GetSession", ADataBase);

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t,
                ref SubmissionOK,
                delegate
                {
                    if (HasValidSession(sessionID, db))
                    {
                        result = GetSession(sessionID, db);
                    }
                    else
                    {
                        result = new SortedList <string, object>();
                    }

                    SubmissionOK = true;
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return result;
        }

        private static void StoreSession(string ASessionID, SortedList <string, object> ASession, TDataBase ADataBase)
        {
            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
            parameters[0].Value = JsonConvert.SerializeObject(ASession);
            parameters[1] = new OdbcParameter("s_valid_until_d", OdbcType.DateTime);
            parameters[1].Value = DateTime.Now.AddHours(SessionValidHours);
            parameters[2] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[2].Value = ASessionID;

            if (HasValidSession(ASessionID, ADataBase))
            {
                string sql = "UPDATE s_session SET s_session_values_c = ?, s_valid_until_d = ? WHERE s_session_id_c = ?";
                ADataBase.ExecuteNonQuery(sql, ADataBase.Transaction, parameters);
            }
            else
            {
                string sql = "INSERT INTO s_session (s_session_values_c, s_valid_until_d, s_session_id_c) VALUES (?,?,?)";
                ADataBase.ExecuteNonQuery(sql, ADataBase.Transaction, parameters);
            }
        }

        private static void RemoveSession(string ASessionID)
        {
            TDataBase db = DBAccess.Connect("RemoveSession");
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = ASessionID;

                    string sql = "DELETE FROM  s_session WHERE s_session_id_c = ?";
                    db.ExecuteNonQuery(sql, t, parameters);
                    SubmissionOK = true;
                });
    
            db.CloseDBConnection();
        }

        /// <summary>
        /// gets the current session id, or creates a new session id if it does not exist yet
        /// </summary>
        public static string GetSessionID()
        {
            string sessionID = FindSessionID();

            if ((sessionID != string.Empty) && !HasValidSession(sessionID))
            {
                TLogging.LogAtLevel(
                    1,
                    "GetSessionID: client is using a session ID that is not valid anymore! (sessionID = " + sessionID +
                    ") => throwing away current session id!");

                // the client is using a session ID that is not valid anymore
                // throw away current session id
                InitThread(string.Empty);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                }

                sessionID = string.Empty;
            }

            if (sessionID == string.Empty)
            {
                TLogging.LogAtLevel(1, "GetSessionID: sessionID == string.Empty! --> creating new session");
                TLogging.LogAtLevel(1, "thread id " + Thread.CurrentThread.ManagedThreadId.ToString());
                sessionID = Guid.NewGuid().ToString();

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Request.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                }

                TLogging.LogAtLevel(1, "GetSessionID: new sessionID = " + sessionID);

                InitThread(sessionID);
            }
            else
            {
                // TLogging.LogAtLevel(4, "GetSessionID: sessionID = " + sessionID);
            }

            return sessionID;
        }

        /// <summary>
        /// set a session variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetVariable(string name, object value)
        {
            TLogging.Log("SetVariable " + name);
            // HttpContext.Current.Session[name] = value;
            TDataBase db = DBAccess.Connect("SessionSetVariable");

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    string sessionID = GetSessionID();
                    SortedList <string, object>session;

                    if (HasValidSession(sessionID, db))
                    {
                        session = GetSession(sessionID, db);
                    }
                    else
                    {
                        session = new SortedList <string, object>();
                    }

                    if (session.Keys.Contains(name))
                    {
                        session[name] = value;
                    }
                    else
                    {
                        session.Add(name, value);
                    }
TLogging.Log("Before StoreSession");
                    StoreSession(sessionID, session, db);
TLogging.Log("After StoreSession");
                    SubmissionOK = true;
                });

            db.CloseDBConnection();
        }

        /// <summary>
        /// returns true if variable exists and is not null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasVariable(string name)
        {
            SortedList <string, object>session = GetSession();

            if (session.Keys.Contains(name) && (session[name] != null))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// get a session variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetVariable(string name)
        {
            // return HttpContext.Current.Session[name];
            SortedList <string, object>session = GetSession();

            if (session.Keys.Contains(name))
            {
                return session[name];
            }

            return null;
        }

        /// <summary>
        /// get a session variable with default boolean
        /// if the variable does not exist, the default is returned
        /// </summary>
        /// <returns></returns>
        public static object GetVariable(string name, bool ADefault)
        {
            // return HttpContext.Current.Session[name];
            SortedList <string, object>session = GetSession();

            if (session.Keys.Contains(name))
            {
                return session[name];
            }

            return ADefault;
        }

        /// <summary>
        /// get a session variable with default string
        /// if the variable does not exist, the default is returned
        /// </summary>
        /// <returns></returns>
        public static object GetVariable(string name, string ADefault)
        {
            // return HttpContext.Current.Session[name];
            SortedList <string, object>session = GetSession();

            if (session.Keys.Contains(name))
            {
                return session[name];
            }

            return ADefault;
        }

        /// <summary>
        /// clear the current session
        /// </summary>
        static public void Clear()
        {
            // HttpContext.Current.Session.Clear();

            TLogging.LogAtLevel(1, "TSession.Clear got called");

            string sessionId = GetSessionID();

            TLogging.LogAtLevel(1, "TSession.Clear: sessionID = " + sessionId);

            if (sessionId.Length > 0)
            {
                RemoveSession(sessionId);
                HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                HttpContext.Current.Response.Cookies.Remove("OpenPetraSessionID");
                // thread might be reused???
                FSessionID = null;
                TLogging.LogAtLevel(1, "TSession.Clear: cleared session with sessionID = " + sessionId);
            }
        }
    }
}

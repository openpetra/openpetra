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
using System.Linq;

using Ict.Common;
using Ict.Common.DB;

using Newtonsoft.Json;

namespace Ict.Common.Session
{
    /// <summary>
    /// Static class for storing sessions.
    /// we are using our own session handling,
    /// since we want to store sessions in the database,
    /// and we want to run tests without HttpContext.
    /// </summary>
    public class TSession
    {
        [ThreadStaticAttribute]
        private static string FSessionID;
        [ThreadStaticAttribute]
        private static SortedList <string, string> FSessionValues;
        [ThreadStaticAttribute]
        private static DateTime FSessionValidUntil;

        private const int SessionValidHours = 24;
        
        private static SortedList <string, string> GetSessionValues(string ASessionID)
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
            {
                if ((HttpContext.Current.Session["SessionID"] != null) && (HttpContext.Current.Session["SessionID"].ToString() == ASessionID))
                {
                    return (SortedList <string, string>) HttpContext.Current.Session["SessionValues"];
                }
            }
            else if ((FSessionID == ASessionID) && (FSessionValidUntil.CompareTo(DateTime.Now) > 0))
            {
                return FSessionValues;
            }

            return null;
        }

        private static void SetSessionValues(string ASessionID, SortedList <string, string> AValues)
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
            {
                HttpContext.Current.Session["SessionID"] = ASessionID;
                HttpContext.Current.Session["SessionValues"] = AValues;
            }
            else if ((FSessionID == ASessionID) || (FSessionID == String.Empty))
            {
                FSessionValidUntil = DateTime.Now.AddHours(SessionValidHours);
                FSessionID = ASessionID;
                FSessionValues = AValues;
            }
        }

        private static void ClearSession()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
            {
                HttpContext.Current.Session.Clear();
            }
            else
            {
                FSessionValidUntil = DateTime.Now.AddHours(-1);
                FSessionID = String.Empty;
                FSessionValues = null;
            }
        }

        /// get the current session id. if it is not stored in the http context, check the thread
        private static string FindSessionID()
        {
            string sessionId;

            // only look in thread if there is no HttpContext.Current; otherwise Threads are reused.
            if (HttpContext.Current == null)
            {
                sessionId = FSessionID;

                if ((sessionId != null) && (sessionId.Length > 0))
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in thread. SessionID = " + sessionId);

                    return sessionId;
                }

                TLogging.LogAtLevel(1,
                    "FindSessionID: Session ID not found in the thread!!! thread id: " + Thread.CurrentThread.ManagedThreadId.ToString());
                return String.Empty;
            }
            else if (HttpContext.Current.Request.Cookies.AllKeys.Contains("OpenPetraSessionID"))
            {
                sessionId = HttpContext.Current.Request.Cookies["OpenPetraSessionID"].Value;
                if (sessionId != String.Empty)
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in HttpContext. SessionID = " + sessionId);
                    return sessionId;
                }
            }

            TLogging.LogAtLevel(1, "FindSessionID: Session ID not found in the HttpContext");
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
            if (GetSessionValues(ASessionID) != null)
            {
                return true;
            }

            //if (ADataBase == null) TLogging.LogStackTrace();
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
                        sql = "DELETE FROM PUB_s_session WHERE s_valid_until_d < NOW()";
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

        private static SortedList <string, string> GetSession(string ASessionID, TDataBase ADataBase)
        {
            SortedList <string, string> result = null;
            result = GetSessionValues(ASessionID);
            if (result != null)
            {
                return result;
            }

            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[0].Value = ASessionID;

            string sql = "SELECT s_session_values_c FROM s_session WHERE s_session_id_c = ?";
            string jsonString = ADataBase.ExecuteScalar(sql, ADataBase.Transaction, parameters).ToString();
            result = JsonConvert.DeserializeObject<SortedList <string, string>>(jsonString);
            SetSessionValues(ASessionID, result);
            return result;
        }

        private static SortedList <string, string> GetSession(TDataBase ADataBase = null)
        {
            string sessionID = GetSessionID(ADataBase);
            SortedList <string, string> result = null;

            result = GetSessionValues(sessionID);
            if (result != null)
            {
                return result;
            }

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
                        result = new SortedList <string, string>();
                        SetSessionValues(sessionID, result);
                    }

                    SubmissionOK = true;
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return result;
        }

        private static void StoreSession(string ASessionID, SortedList <string, string> ASession, TDataBase ADataBase)
        {
            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
            parameters[0].Value = JsonConvert.SerializeObject(ASession);
            parameters[1] = new OdbcParameter("s_valid_until_d", OdbcType.DateTime);
            parameters[1].Value = DateTime.Now.AddHours(SessionValidHours);
            parameters[2] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[2].Value = ASessionID;

            string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ?";
            OdbcParameter[] parameters2 = new OdbcParameter[1];
            parameters2[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters2[0].Value = ASessionID;
            
            if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, ADataBase.Transaction, parameters2)) == 1)
            {
                sql = "UPDATE PUB_s_session SET s_session_values_c = ?, s_valid_until_d = ? WHERE s_session_id_c = ?";
            }
            else
            {
                sql = "INSERT INTO PUB_s_session (s_session_values_c, s_valid_until_d, s_session_id_c) VALUES (?,?,?)";
            }

            ADataBase.ExecuteNonQuery(sql, ADataBase.Transaction, parameters);

            SetSessionValues(ASessionID, ASession);
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

            SetSessionValues(ASessionID, null);
    
            db.CloseDBConnection();
        }

        /// <summary>
        /// gets the current session id, or creates a new session id if it does not exist yet
        /// </summary>
        public static string GetSessionID(TDataBase ADataBase = null)
        {
            string sessionID = FindSessionID();

            if ((sessionID != string.Empty) && !HasValidSession(sessionID, ADataBase))
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
                sessionID = Guid.NewGuid().ToString();

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Request.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                }
                else
                {
                    TLogging.LogAtLevel(4, "thread id " + Thread.CurrentThread.ManagedThreadId.ToString());
                    InitThread(sessionID);
                }

                TLogging.LogAtLevel(1, "GetSessionID: new sessionID = " + sessionID);

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
        /// <param name="ADataBase"></param>
        public static void SetVariable(string name, object value, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("SessionSetVariable", ADataBase);

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    string sessionID = GetSessionID(db);
                    SortedList <string, string>session;

                    if (HasValidSession(sessionID, db))
                    {
                        session = GetSession(sessionID, db);
                    }
                    else
                    {
                        session = new SortedList <string, string>();
                    }

                    if (session.Keys.Contains(name))
                    {
                        session[name] = (new TVariant(value)).EncodeToString();
                    }
                    else
                    {
                        session.Add(name, (new TVariant(value)).EncodeToString());
                    }

                    StoreSession(sessionID, session, db);

                    SubmissionOK = true;
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }
        }

        /// <summary>
        /// returns true if variable exists and is not null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasVariable(string name)
        {
            SortedList <string, string>session = GetSession();

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
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        public static object GetVariable(string name, TDataBase ADataBase = null)
        {
            SortedList <string, string>session = GetSession(ADataBase);

            if (session.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(session[name]).ToObject();
            }

            return null;
        }

        /// <summary>
        /// get a session variable, not decoded yet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        public static TVariant GetVariant(string name, TDataBase ADataBase = null)
        {
            SortedList <string, string>session = GetSession(ADataBase);

            if (session.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(session[name]);
            }

            return new TVariant((object)null);
        }

        /// <summary>
        /// get a session variable with default boolean
        /// if the variable does not exist, the default is returned
        /// </summary>
        /// <returns></returns>
        public static object GetVariable(string name, bool ADefault)
        {
            SortedList <string, string>session = GetSession();

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
            SortedList <string, string>session = GetSession();

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
            TLogging.LogAtLevel(1, "TSession.Clear got called");

            string sessionId = GetSessionID();

            TLogging.LogAtLevel(1, "TSession.Clear: sessionID = " + sessionId);

            if (sessionId.Length > 0)
            {
                RemoveSession(sessionId);
                HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                HttpContext.Current.Response.Cookies.Remove("OpenPetraSessionID");
                ClearSession();
                TLogging.LogAtLevel(1, "TSession.Clear: cleared session with sessionID = " + sessionId);
            }
        }
    }
}

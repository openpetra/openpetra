//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2024 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Linq;

using Ict.Common;
using Ict.Common.DB;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
        private const int SessionValidHours = 24;

        private static Mutex FDeleteSessionMutex = new Mutex(); // STATIC_OK: Mutex

        // these variables are only used per thread, they are initialized for each request.
        [ThreadStatic]
        private static string FSessionID;
        [ThreadStatic]
        private static SortedList <string, string> FSessionValues;

        /// <summary>
        /// Set the session id for this current thread.
        /// Each request has its own thread.
        /// Threads can be reused for different users.
        /// </summary>
        public static void InitThread(string AThreadDescription, string AConfigFileName, HttpRequest ARequest, string ASessionID = null)
        {
            TLogWriter.ResetStaticVariables();
            TLogging.ResetStaticVariables();

            new TAppSettingsManager(AConfigFileName);
            new TLogging(TSrvSetting.ServerLogFile);
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

            string httprequest = "";
            if (ARequest != null)
            {
                httprequest = " for path " + ARequest.Path;
            }
            
            TLogging.LogAtLevel(4, AThreadDescription + ": Running InitThread for thread id " + Thread.CurrentThread.ManagedThreadId.ToString() + httprequest);

            FSessionID = ASessionID;
            FSessionValues = null;

            string sessionID;

            if (ASessionID == null)
            {
                sessionID = FindSessionID(ARequest);
            }
            else
            {
                sessionID = ASessionID;
            }

            TDataBase db = null;

            try
            {
                // avoid dead lock on parallel logins
                if (!FDeleteSessionMutex.WaitOne(5000))
                {
                    throw new Exception("Server is too busy");
                }
            }
            catch(AbandonedMutexException ex)
            {
                TLogging.Log("Mutex was abandoned");

                // Whether or not the exception was thrown, the current
                // thread owns the mutex, and must release it.
                if (ex.Mutex != null) ex.Mutex.ReleaseMutex();
                throw new Exception("AbandonedMutex has been cleared");
            }

            try
            {
                db = ConnectDB("SessionInitThread");

                TDBTransaction t = new TDBTransaction();
                bool SubmissionOK = false;
                bool newSession = false;

                db.WriteTransaction(ref t,
                    ref SubmissionOK,
                    delegate
                    {
                        // get the session ID, or start a new session
                        // load the session values from the database
                        // update the session last access in the database
                        // clean old sessions
                        newSession = InitSession(sessionID, t, ARequest);

                        SubmissionOK = true;
                    });

                if (newSession)
                {
                    // use a separate transaction to clean old sessions
                    db.WriteTransaction(ref t,
                        ref SubmissionOK,
                        delegate
                        {
                            CleanOldSessions(t);
                            SubmissionOK = true;
                        });
                }
            }
            finally
            {
                if (db != null)
                {
                    db.CloseDBConnection();
                }

                FDeleteSessionMutex.ReleaseMutex();
            }
        }

        /// get the current session id from the http context
        private static string FindSessionID(HttpRequest ARequest)
        {
            if ((ARequest != null) && (ARequest.Cookies.Keys.Contains("OpenPetraSessionID")))
            {
                string sessionId = ARequest.Cookies["OpenPetraSessionID"];
                if (sessionId != String.Empty)
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in HttpContext. SessionID: " + sessionId);
                    return sessionId;
                }
            }

            TLogging.LogAtLevel(1, "FindSessionID: Session ID not found in the HttpContext");
            return string.Empty;
        }

        /// <summary>
        /// gets the current session id, or creates a new session id if it does not exist yet.
        /// loads the session values or initializes them.
        /// clean old sessions from the database.
        /// </summary>
        /// <returns>true if new session was started</returns>
        private static bool InitSession(string ASessionID, TDBTransaction AWriteTransaction, HttpRequest ARequest)
        {
            string sessionID = ASessionID;
            bool newSession = false;

            // is that session still valid?
            if ((sessionID != string.Empty) && !HasValidSession(sessionID, AWriteTransaction))
            {
                TLogging.LogAtLevel(1,"TSession: session ID is not valid anymore: " + sessionID);

                sessionID = string.Empty;

#if DISABLED_DOTNET
                if (ARequest != null)
                {
                    ARequest.Cookies.Remove("OpenPetraSessionID");
                }
#endif
            }

            // we need a new session
            if (sessionID == string.Empty)
            {
                sessionID = Guid.NewGuid().ToString();
                TLogging.LogAtLevel(1, "TSession: Creating new session: " + sessionID + " in Thread " + Thread.CurrentThread.ManagedThreadId.ToString());

#if DISABLED_DOTNET
                if (ARequest != null)
                {
                    HttpCookie cookie = new HttpCookie("OpenPetraSessionID", sessionID);
                    // SameSite is not support by Mono 6.6 yet
                    // cookie.SameSite = SameSiteMode.Strict;

                    if (ARequest.Headers["X-Forwarded-Proto"] != null)
                    {
                        cookie.Secure = "https" == ARequest.Headers["X-Forwarded-Proto"].Split(',').FirstOrDefault();
                    }
                    else
                    {
                        cookie.Secure = ARequest.Url.Scheme == "https";
                    }

                    ARequest.Cookies.Add(cookie);
                    ARequest.Response.Cookies.Add(cookie);
                }
#endif

                // store new session
                FSessionID = sessionID;
                FSessionValues = new SortedList <string, string>();
                SaveSession(AWriteTransaction);

                newSession = true;
            }
            else
            {
                TLogging.LogAtLevel(4, "TSession: Loading valid session from database: " + sessionID + " in Thread " + Thread.CurrentThread.ManagedThreadId.ToString());
                FSessionID = sessionID;
                LoadSession(AWriteTransaction);
                UpdateLastAccessTime(AWriteTransaction);
            }

            return newSession;
        }

        private static bool HasValidSession(string ASessionID, TDBTransaction AReadTransaction)
        {
            string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ? and s_valid_until_d > NOW()";
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[0].Value = ASessionID;
            
            if (Convert.ToInt32(AReadTransaction.DataBaseObj.ExecuteScalar(sql, AReadTransaction, parameters)) == 1)
            {
                return true;
            }

            return false;
        }

        private static TDataBase ConnectDB(string AConnectionName)
        {
            return DBAccess.Connect(AConnectionName);
        }

        private static void UpdateLastAccessTime(TDBTransaction AWriteTransaction)
        {
            OdbcParameter[] parameters = new OdbcParameter[2];
            parameters[0] = new OdbcParameter("s_modification_id_t", OdbcType.DateTime);
            parameters[0].Value = DateTime.Now;
            parameters[1] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[1].Value = FSessionID;
            string sql = "UPDATE PUB_s_session SET s_modification_id_t = ? WHERE s_session_id_c = ?";
            AWriteTransaction.DataBaseObj.ExecuteNonQuery(sql, AWriteTransaction, parameters);
        }

        private static void LoadSession(TDBTransaction AReadTransaction)
        {
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[0].Value = FSessionID;

            string sql = "SELECT s_session_values_c FROM s_session WHERE s_session_id_c = ?";
            string jsonString = AReadTransaction.DataBaseObj.ExecuteScalar(sql, AReadTransaction, parameters).ToString();
            FSessionValues = JsonConvert.DeserializeObject<SortedList <string, string>>(jsonString);
        }

        // TODO: drop this method once all databases have been upgraded to version 2022.06
        private static bool CheckForSessionUserIDColumn(TDBTransaction AWriteTransaction)
        {
            bool ColumnExists = false;

            try
            {
                string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_user_id_c = 'TEST'";
                if (Convert.ToInt32(AWriteTransaction.DataBaseObj.ExecuteScalar(sql, AWriteTransaction)) == 0)
                {
                    ColumnExists = true;
                }
            }
            catch (System.Exception)
            {
                // the column must be added
            }

            return ColumnExists;
        }

        private static void SaveSession(TDBTransaction AWriteTransaction)
        {
            string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ?";
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[0].Value = FSessionID;
            string SerializedSessionValues = JsonConvert.SerializeObject(FSessionValues);

            if (SerializedSessionValues.Length > 65500)
            {
                throw new Exception("TSession.SaveSession: the session should not get that big: " + SerializedSessionValues.Length.ToString());
            }

            if (Convert.ToInt32(AWriteTransaction.DataBaseObj.ExecuteScalar(sql, AWriteTransaction, parameters)) == 1)
            {
                parameters = new OdbcParameter[2];
                parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
                parameters[0].Value = SerializedSessionValues;
                parameters[1] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                parameters[1].Value = FSessionID;
                sql = "UPDATE PUB_s_session SET s_session_values_c = ? WHERE s_session_id_c = ?";

                // check if the field is already available.
                // this can be dropped when all databases have been upgraded.
                bool SessionUserIDColumnExists = CheckForSessionUserIDColumn(AWriteTransaction);

                if (SessionUserIDColumnExists && FSessionValues.Keys.Contains("UserID"))
                {
                    parameters = new OdbcParameter[3];
                    parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
                    parameters[0].Value = SerializedSessionValues;
                    parameters[1] = new OdbcParameter("s_user_id_c", OdbcType.VarChar);
                    parameters[1].Value = GetVariant("UserID").ToString();
                    parameters[2] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[2].Value = FSessionID;
                    sql = "UPDATE PUB_s_session SET s_session_values_c = ?, s_user_id_c = ? WHERE s_session_id_c = ?";
                }
            }
            else
            {
                parameters = new OdbcParameter[3];
                parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
                parameters[0].Value = SerializedSessionValues;
                parameters[1] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                parameters[1].Value = FSessionID;
                parameters[2] = new OdbcParameter("s_valid_until_d", OdbcType.DateTime);
                parameters[2].Value = DateTime.Now.AddHours(SessionValidHours);
                sql = "INSERT INTO PUB_s_session (s_session_values_c, s_session_id_c, s_valid_until_d) VALUES (?,?,?)";
            }

            AWriteTransaction.DataBaseObj.ExecuteNonQuery(sql, AWriteTransaction, parameters);
        }

        /// clean all old sessions
        static private void CleanOldSessions(TDBTransaction AWriteTransaction)
        {
            string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_valid_until_d < NOW()";
            if (Convert.ToInt32(AWriteTransaction.DataBaseObj.ExecuteScalar(sql, AWriteTransaction)) > 0)
            {
                sql = "DELETE FROM PUB_s_session WHERE s_valid_until_d < NOW()";
                AWriteTransaction.DataBaseObj.ExecuteNonQuery(sql, AWriteTransaction);
            }
        }

        /// <summary>
        /// set a session variable.
        /// store to database immediately
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetVariable(string name, object value)
        {
            TDataBase db = ConnectDB("SessionSetVariable");

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    if (FSessionValues.Keys.Contains(name))
                    {
                        FSessionValues[name] = (new TVariant(value)).EncodeToString();
                    }
                    else
                    {
                        FSessionValues.Add(name, (new TVariant(value)).EncodeToString());
                    }

                    SaveSession(t);

                    SubmissionOK = true;
                });

            db.CloseDBConnection();
        }

        /// remove all variables that start with a name, eg. PROGRESSTRACKER
        public static void ClearVariables(string ANameStartsWith)
        {
            TDataBase db = ConnectDB("SessionClearVariables");

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    bool finished = false;

                    while (!finished)
                    {
                        finished = true;

                        foreach (string name in FSessionValues.Keys)
                        {
                            if (name.StartsWith(ANameStartsWith))
                            {
                                FSessionValues.Remove(name);
                                finished = false;
                                break;
                            }
                        }
                    }

                    SaveSession(t);

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
            bool result = false;

            if ((FSessionValues != null) && FSessionValues.Keys.Contains(name) && (FSessionValues[name] != null))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// get a session variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetVariable(string name)
        {
            if ((FSessionValues != null) && FSessionValues.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(FSessionValues[name]).ToObject();
            }

            return null;
        }

        /// <summary>
        /// get a session variable, not decoded yet
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TVariant GetVariant(string name)
        {
            if ((FSessionValues != null) && FSessionValues.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(FSessionValues[name]);
            }

            return new TVariant((object)null);
        }

        /// get the session values from the database again
        public static void RefreshFromDatabase()
        {
            TDataBase db = ConnectDB("SessionRefresh");
            TDBTransaction t = new TDBTransaction();

            db.ReadTransaction(ref t,
                delegate
                {
                    LoadSession(t);
                });

            db.CloseDBConnection();
        }

        /// get the session id, to pass to a sub thread
        public static string GetSessionID()
        {
            return FSessionID;
        }

        private static void RemoveSession()
        {
            TDataBase db = ConnectDB("RemoveSession");
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = FSessionID;
                    string sql = "DELETE FROM s_session WHERE s_session_id_c = ?";

                    if (FSessionValues.Keys.Contains("UserID"))
                    {
                        string UserID = GetVariant("UserID").ToString();

                        if (UserID != "DEMO")
                        {
                            parameters = new OdbcParameter[2];
                            parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                            parameters[0].Value = FSessionID;
                            parameters[1] = new OdbcParameter("s_user_id_c", OdbcType.VarChar);
                            parameters[1].Value = UserID;
                            sql = "DELETE FROM s_session WHERE s_session_id_c = ? OR s_user_id_c = ?";
                        }
                    }

                    db.ExecuteNonQuery(sql, t, parameters);
                    SubmissionOK = true;
                });

            db.CloseDBConnection();
        }

        /// <summary>
        /// close the current session
        /// </summary>
        public static void CloseSession()
        {
            TLogging.LogAtLevel(1, "TSession.CloseSession got called: " + FSessionID);

            RemoveSession();

            FSessionID = String.Empty;
            FSessionValues = null;

#if DISABLED_DOTNET
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                HttpContext.Current.Response.Cookies.Remove("OpenPetraSessionID");
                HttpContext.Current.Session.Clear();
            }
#endif
        }
    }
}

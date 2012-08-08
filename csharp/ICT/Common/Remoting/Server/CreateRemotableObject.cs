//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// create a object that can be remoted to a client
    /// </summary>
    public class TCreateRemotableObject
    {
        private static string GetTypeString(Type t)
        {
            string parametertype = t.ToString().Replace("&", string.Empty).Replace("System.Void", "void");

            if (parametertype.Contains("`2"))
            {
                parametertype = parametertype.Replace("`2", string.Empty).Replace("[", "<").Replace("]", ">");
            }

            if (parametertype.Contains("`1"))
            {
                parametertype = parametertype.Replace("`1", string.Empty).Replace("[", "<").Replace("]", ">");
            }

            return parametertype;
        }

        private static string CreateCodeForInterface(Type AInterfaceToImplement)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("using Ict.Common.Remoting.Client;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace Ict.Common.Remoting {");
            sb.Append(Environment.NewLine);
            sb.Append("  [Serializable]");
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("  public class TempRemotableObject: {0}",
                    AInterfaceToImplement));
            sb.Append("  {");
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("    private {0} RemoteObject = null;",
                    AInterfaceToImplement));
            sb.Append(Environment.NewLine);
            sb.Append("    private string FObjectURI;");
            sb.Append(Environment.NewLine);
            sb.Append("    public TempRemotableObject(string AObjectURI) { FObjectURI = AObjectURI; } ");
            sb.Append(Environment.NewLine);
            sb.Append("    private void InitRemoteObject()");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("      RemoteObject = ({0})TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof({0}));",
                    AInterfaceToImplement));
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);

            foreach (PropertyInfo propertyInfo in AInterfaceToImplement.GetProperties())
            {
                sb.Append("    public " + GetTypeString(propertyInfo.PropertyType) + " " + propertyInfo.Name);
                sb.Append(Environment.NewLine);
                sb.Append("    {");
                sb.Append(Environment.NewLine);

                MethodInfo getter = propertyInfo.GetGetMethod();

                if (getter != null)
                {
                    sb.Append("      get");
                    sb.Append(Environment.NewLine);
                    sb.Append("      {");
                    sb.Append(Environment.NewLine);
                    sb.Append("        if (RemoteObject == null) { InitRemoteObject(); }");
                    sb.Append(Environment.NewLine);
                    sb.Append("        return RemoteObject." + propertyInfo.Name + ";");
                    sb.Append("      }");
                    sb.Append(Environment.NewLine);
                }

                MethodInfo setter = propertyInfo.GetSetMethod();

                if (setter != null)
                {
                    sb.Append("      set");
                    sb.Append(Environment.NewLine);
                    sb.Append("      {");
                    sb.Append(Environment.NewLine);
                    sb.Append("        if (RemoteObject == null) { InitRemoteObject(); }");
                    sb.Append(Environment.NewLine);
                    sb.Append("        RemoteObject." + propertyInfo.Name + " = value;");
                    sb.Append("      }");
                    sb.Append(Environment.NewLine);
                }

                sb.Append("   }");
                sb.Append(Environment.NewLine);
            }

            foreach (MethodInfo m in AInterfaceToImplement.GetMethods())
            {
                if (m.Name.StartsWith("get_") || m.Name.StartsWith("set_"))
                {
                    // ignore property accessors
                    continue;
                }

                string methodActualParameters = string.Empty;
                string methodParameters = string.Empty;

                foreach (ParameterInfo p in m.GetParameters())
                {
                    if (methodActualParameters.Length > 0)
                    {
                        methodActualParameters += ",";
                        methodParameters += ",";
                    }

                    if (p.IsOut)
                    {
                        methodActualParameters += "out ";
                        methodParameters += "out ";
                    }
                    else if (p.ParameterType.IsByRef)
                    {
                        methodActualParameters += "ref ";
                        methodParameters += "ref ";
                    }

                    methodActualParameters += p.Name;

                    methodParameters += GetTypeString(p.ParameterType) + " " + p.Name;
                }

                string ReturnType = GetTypeString(m.ReturnType);

                sb.Append(String.Format("    public {0} {1}({2})",
                        ReturnType,
                        m.Name,
                        methodParameters));
                sb.Append(Environment.NewLine);
                sb.Append("    {");
                sb.Append(Environment.NewLine);
                sb.Append("      if (RemoteObject == null) { InitRemoteObject(); }");
                sb.Append(Environment.NewLine);
                sb.Append("      ");

                if (ReturnType != "void")
                {
                    sb.Append("return ");
                }

                sb.Append(String.Format("RemoteObject.{0}({1});",
                        m.Name, methodActualParameters));
                sb.Append(Environment.NewLine);
                sb.Append("    }");
                sb.Append(Environment.NewLine);
            }

            sb.Append("}}");

            return sb.ToString();
        }

        private static SortedList <string, Assembly>AssembliesForInterface = new SortedList <string, Assembly>();

        private static Assembly CreateAssembly(Type AInterfaceToImplement)
        {
            CSharpCodeProvider csc = new CSharpCodeProvider(
                new Dictionary <string, string>() {
                    { "CompilerVersion", "v4.0" }
                });
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = "Temp.Remoting." + AInterfaceToImplement + ".dll";
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");

            parameters.ReferencedAssemblies.Add(TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Common.dll");
            parameters.ReferencedAssemblies.Add(TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Common.Data.dll");
            parameters.ReferencedAssemblies.Add(
                TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Common.Remoting.Shared.dll");
            parameters.ReferencedAssemblies.Add(
                TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Common.Remoting.Client.dll");
            parameters.ReferencedAssemblies.Add(
                TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Common.Verification.dll");

            // reference all Shared dlls in the bin directory
            string[] dllnames = Directory.GetFiles(TAppSettingsManager.ApplicationDirectory, "Ict.Petra.Shared*.dll");

            foreach (string dllName in dllnames)
            {
                parameters.ReferencedAssemblies.Add(dllName);
            }

            string src = CreateCodeForInterface(AInterfaceToImplement);

            if (TLogging.DebugLevel >= 10)
            {
                TLogging.Log("CreateAssembly for " + AInterfaceToImplement);
                TLogging.Log(src);
            }

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, src);

            if (results.Errors.HasErrors)
            {
                TLogging.Log(src);

                foreach (CompilerError error in results.Errors)
                {
                    TLogging.Log(error.ToString());
                }

                throw new Exception("errors in CreateRemotableObject");
            }

            return results.CompiledAssembly;
        }

        /// <summary>
        /// create a object that can be remoted to a client
        /// </summary>
        public static object CreateRemotableObject(Type AInterfaceToImplement, ICrossDomainService ObjectToRemote)
        {
            if (!AssembliesForInterface.ContainsKey(AInterfaceToImplement.ToString()))
            {
                AssembliesForInterface.Add(AInterfaceToImplement.ToString(),
                    CreateAssembly(AInterfaceToImplement));
            }

            Assembly GeneratedAssembly = AssembliesForInterface[AInterfaceToImplement.ToString()];

            // need to calculate the URI for this object and pass it to the new namespace object
            string ObjectURI = TConfigurableMBRObject.BuildRandomURI(ObjectToRemote.GetType().ToString());

            // we need to add the service in the main domain
            DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

            try
            {
                // create the object
                Type type = GeneratedAssembly.GetType("Ict.Common.Remoting.TempRemotableObject");
                return Activator.CreateInstance(type, new object[] { ObjectURI });
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                throw;
            }
        }
    }
}
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
namespace GenerateSharedCode
{
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using NamespaceHierarchy;

class AssemblyHelper
{
    public static String DLLPath = "invalid";
    static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
    {
        //Console.WriteLine(args.Name);
        //Console.WriteLine(args.Name.Split(',')[0]);
        if ((args.Name.IndexOf("System") == 0) || (args.Name.IndexOf("Borland.") == 0))
        {
            return System.Reflection.Assembly.ReflectionOnlyLoad(args.Name);
        }

        return System.Reflection.Assembly.ReflectionOnlyLoadFrom(
            Path.GetDirectoryName(DLLPath) + Path.DirectorySeparatorChar +
            Path.GetFileName(args.Name.Split(',')[0]) + ".dll");
    }

    public static Assembly MyLoadAssembly(String ADLLPath)
    {
        // see http://blogs.msdn.com/jmstall/archive/2006/11/22/reflection-type-load-exception.aspx
        DLLPath = ADLLPath;
        AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
        return Assembly.ReflectionOnlyLoadFrom(ADLLPath);
    }

    private static void TestAnalysisAll(String ADLLPath)
    {
        Assembly a = MyLoadAssembly(ADLLPath);

        try
        {
            foreach (Type t in a.GetTypes())
            {
                Console.WriteLine(t.ToString());

                //TestAnalysis(ADLLPath, t.ToString());
            }
        }
        catch (ReflectionTypeLoadException e)
        {
            foreach (Exception e2 in e.LoaderExceptions)
            {
                System.Console.WriteLine(e2.Message);
            }
        }
    }

    private static void TestAnalysis(String ADLLPath, String AClassName)
    {
        Assembly a = MyLoadAssembly(ADLLPath);
        Type t = a.GetType(AClassName);

        if (t == null)
        {
            throw new Exception("Cannot find " + AClassName + " in " + ADLLPath);
        }

        Console.WriteLine(t.Name + " derived from: ");

        foreach (Type ti in t.GetInterfaces())
        {
            Console.WriteLine("   " + ti.Name);
        }

        foreach (MethodInfo m in t.GetMethods())
        {
            Console.WriteLine(m.ReturnType + " " + m.Name);

            foreach (ParameterInfo p in m.GetParameters())
            {
                Console.WriteLine("  " + p.ParameterType.ToString() + " " + p.Name);
            }
        }
    }
}
}
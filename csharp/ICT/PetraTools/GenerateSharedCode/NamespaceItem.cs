/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Ict.Tools.CodeGeneration;

namespace NamespaceHierarchy
{
/// <summary>
/// to be parsed from xml file
/// </summary>
public class SubNamespace
{
    string name;
    string description;
    string group;
    string module;

    /// <summary>
    /// constructor
    /// </summary>
    public SubNamespace()
    {
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="name"></param>
    public SubNamespace(string name)
    {
        this.name = name;
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="AName"></param>
    /// <param name="ADescription"></param>
    /// <param name="AGroup"></param>
    /// <param name="AModule"></param>
    public SubNamespace(string AName, string ADescription, string AGroup, string AModule)
    {
        this.name = AName;
        this.description = ADescription;
        this.group = AGroup;
        this.module = AModule;
    }

    /// <summary>
    /// name of the namespace
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    /// <summary>
    /// description
    /// </summary>
    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    /// <summary>
    /// part of which group
    /// </summary>
    public string Group
    {
        get
        {
            return group;
        }
        set
        {
            group = value;
        }
    }

    /// <summary>
    /// part of which module
    /// </summary>
    public string Module
    {
        get
        {
            return module;
        }
        set
        {
            module = value;
        }
    }

    /// <summary>
    /// the children of this namespace
    /// </summary>
    public List <SubNamespace>Children = new List <SubNamespace>();

    /// <summary>
    /// parse the namespaces from a file
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static List <TopNamespace>ReadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            new Exception("file not found");
        }

        FileStream fs = new FileStream(filename, FileMode.Open);

        XmlSerializer x = new XmlSerializer(typeof(List <TopNamespace> ));

        List <TopNamespace>items = null;

        try
        {
            items = (List <TopNamespace> )x.Deserialize(fs);
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            fs.Close();
        }

        return items;
    }
}

/// <summary>
/// top namespace
/// </summary>
public class TopNamespace
{
    /// <summary>
    /// name of namespace
    /// </summary>
    public string Name = "";

    /// <summary>
    /// description
    /// </summary>
    public string Description = "";

    /// <summary>
    /// the children of this namespace
    /// </summary>
    public List <SubNamespace>SubNamespaces = new List <SubNamespace>();

    private static String FindModuleName = "";

    /// <summary>
    /// find a module in the top namespace
    /// </summary>
    /// <param name="AList"></param>
    /// <param name="AModule"></param>
    /// <returns></returns>
    public static Int32 FindModuleIndex(List <TopNamespace>AList, String AModule)
    {
        FindModuleName = AModule;
        return AList.FindIndex(TopNamespace.FindModule);
    }

    /// <summary>
    /// check if this is the current module
    /// </summary>
    /// <param name="AModule"></param>
    /// <returns></returns>
    public static bool FindModule(TopNamespace AModule)
    {
        if (AModule.Name == FindModuleName)      //FindModuleName
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/// <summary>
/// for generating the code
/// </summary>
public class CommonNamespace
{
    /// <summary>
    /// write the code for the namespace; use this namespace; recursive method
    /// </summary>
    /// <param name="tw"></param>
    /// <param name="ParentNamespace"></param>
    /// <param name="ParentInterfaceName"></param>
    /// <param name="sn"></param>
    /// <param name="children"></param>
    public static void WriteUsingNamespace(AutoGenerationWriter tw,
        String ParentNamespace,
        String ParentInterfaceName,
        SubNamespace sn,
        List <SubNamespace>children)
    {
        // don't automatically write the end points.
        if (children.Count == 0)
        {
            return;
        }

        foreach (SubNamespace child in children)
        {
            //        if (child.Children.Count > 0)
            {
                tw.WriteLine("using " + ParentNamespace + "." + child.Name + ';');
                WriteUsingNamespace(tw, ParentNamespace + "." + child.Name, ParentInterfaceName + child.Name, child, child.Children);
            }
        }
    }
}
}
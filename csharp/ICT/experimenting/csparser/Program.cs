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
using System.Text;
using DDW;
using DDW.Collections;
using Ict.Tools.CodeGeneration;

namespace csparserTest
{
class Program
{
    public static int Main(string[] args)
    {
        CSParser parser = new CSParser("../../Program.cs");

        ClassNode classnode = parser.GetFirstClass();
        MethodNode methodnode = parser.GetMethod(classnode, "Main");

        // get the return type of this method
        StringBuilder sb = new StringBuilder();

        methodnode.Type.ToSource(sb);
        Console.WriteLine(sb.ToString());

        parser = new CSParser("U:/OpenPetra/csharp/ICT/Petra/Server/lib/MPartner/ServerLookups.cs");
        classnode = parser.GetFirstClass();
        methodnode = parser.GetMethod(classnode, "MergedPartnerDetails");
        sb = new StringBuilder();
        methodnode.Type.ToSource(sb);
        Console.WriteLine(sb.ToString());

        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);

        return 0;
    }
}
}
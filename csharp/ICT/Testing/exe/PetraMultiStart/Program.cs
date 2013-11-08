/*
 * Test program for many logins at the same time; 
 * this program now also can run defined randomly.
 *  
 * use progress/compiler/grantall.p for creating the test users
 *
 * for Parameters see app-sample.config
 *   testscript
 *
 * Author:  Timotheus Pokorra, Christian Kendel (C# translation)
 *
 * @Version $Revision: 1.1 $ / $Date: 2009/06/30 11:00:58 $
 */
 
using System;

namespace PetraMultiStart
{
    class Program
    {
        public static void Main(string[] args)
        {
            main.RunTest();                
        }
    }
}
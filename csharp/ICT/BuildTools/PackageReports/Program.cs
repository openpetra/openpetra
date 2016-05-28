//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PackageReports
{
    class Program
    {
        static string reportsFolder = @"C:\OpenPetra\Tim\20160127\XmlReports";
        static string outputFolder = @"C:\OpenPetra\Tim\20160127\delivery";

        static string sourcetable = "s_report_template";
        static string newtable = "s_report_latest";

        static string createtableschema =
            @"
 create table s_report_latest
 (s_template_id_i integer NOT NULL,
  s_report_type_c character varying(100) NOT NULL,
  s_report_variant_c character varying(100) NOT NULL,
  s_author_c character varying(100) NOT NULL,
  s_default_l boolean NOT NULL DEFAULT false,
  s_private_l boolean NOT NULL DEFAULT false,
  s_private_default_l boolean NOT NULL DEFAULT false,
  s_readonly_l boolean NOT NULL DEFAULT false,
  s_xml_text_c text NOT NULL,
  s_date_created_d date DEFAULT ('now'::text)::date,
  s_created_by_c character varying(20),
  s_date_modified_d date,
  s_modified_by_c character varying(20),
  s_modification_id_t timestamp without time zone,
  CONSTRAINT s_report_latest_pk PRIMARY KEY (s_template_id_i));"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ;

        static string updateExistingQuery =
            @"
UPDATE s_report_template
SET s_xml_text_c=s_report_latest.s_xml_text_c, s_readonly_l=TRUE
FROM s_report_latest
WHERE s_report_template.s_template_id_i = s_report_latest.s_template_id_i
	and s_report_template.s_report_type_c = s_report_latest.s_report_type_c;"                                                                                                                                                                                                      ;

        static string replaceMissingQuery =
            @"
INSERT INTO s_report_template
(s_template_id_i,  s_report_type_c, s_report_variant_c,
 s_author_c, s_default_l, s_private_l,
 s_private_default_l, s_readonly_l, s_xml_text_c,
 s_date_created_d, s_created_by_c,  s_date_modified_d,
 s_modified_by_c, s_modification_id_t)

SELECT  RL.s_template_id_i,  RL.s_report_type_c, RL.s_report_variant_c,
	RL.s_author_c, RL.s_default_l, RL.s_private_l,
	RL.s_private_default_l, RL.s_readonly_l, RL.s_xml_text_c,
	RL.s_date_created_d, RL.s_created_by_c,  RL.s_date_modified_d,
	RL.s_modified_by_c, RL.s_modification_id_t

FROM s_report_latest RL

LEFT OUTER JOIN s_report_template RT
ON RL.s_template_id_i = RT.s_template_id_i
and RL.s_report_type_c = RT.s_report_type_c

WHERE RT.s_template_id_i is null;"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ;


        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Arguments needed: <XMLReportsFolderPath> <DeliveryPath>");
                Console.WriteLine("");
                Environment.Exit(-1);
            }

            reportsFolder = args[0];
            outputFolder = args[1];

            if (!reportsFolder.EndsWith(@"\"))
            {
                reportsFolder = reportsFolder + @"\";
            }

            if (!outputFolder.EndsWith(@"\"))
            {
                outputFolder = outputFolder + @"\";
            }

            Console.WriteLine("Taking reports from: {0}", reportsFolder);
            Console.WriteLine("Writing SQL in: {0}", outputFolder);


            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(reportsFolder);
            StreamWriter sw = new StreamWriter(outputFolder + "/" + "allreports.sql", false);
            sw.WriteLine(FileHeader());

            foreach (FileInfo fi in di.GetFiles("*.sql"))
            {
                if (fi.Name != "FastReportsBackup.sql")
                {
                    Console.WriteLine(fi.Name);
                    string thisFile = "";
                    thisFile = File2String(fi.FullName);
                    sw.WriteLine();
                    sw.WriteLine("/*" + fi.Name + "*/");
                    sw.Write(ModifyFile(thisFile));
                    sw.WriteLine();
                }
            }

            sw.WriteLine();

            sw.WriteLine(FileFooter());
            sw.WriteLine();
            sw.Flush();
            sw.Close();
            sw.Dispose();

            Environment.Exit(0);
        }

        static string File2String(string filename)
        {
            string temp = "";
            StreamReader sr = new StreamReader(filename);

            temp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return temp;
        }

        static string ModifyFile(string input)
        {
            input = input.Replace(sourcetable, newtable);
            return input;
        }

        static string FileHeader()
        {
            StringBuilder header = new StringBuilder();

            header.AppendLine("/* HEADER */");
            header.AppendLine("BEGIN TRANSACTION;");
            header.AppendLine(createtableschema);
            header.AppendLine("/* END OF HEADER */");

            return header.ToString();
        }

        static string FileFooter()
        {
            StringBuilder footer = new StringBuilder();

            footer.AppendLine("/* FOOTER */");

            footer.AppendLine("/* Update Existing Query */");
            footer.AppendLine(updateExistingQuery);

            footer.AppendLine("/* Replace Missing Query */");
            footer.AppendLine(replaceMissingQuery);

            footer.AppendLine("/* FOOTER */");


            footer.AppendLine("DROP TABLE " + newtable + ";");
            footer.AppendLine("COMMIT TRANSACTION;");
            footer.AppendLine("/* END OF FOOTER */");

            return footer.ToString();
        }
    }
}
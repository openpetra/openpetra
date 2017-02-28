//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2016 by OM International
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
using System.Data;
using Ict.Common.DB;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Common.Remoting.Server;
using Ict.Common;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MCommon;
using System.Linq;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// SQL Tool for Partner Reports
    /// </summary>
    public class TPartnerReportTools
    {
        /// <summary>
        /// Adds the conditions for a UC_AddressFilter to a Query
        /// </summary>
        /// <param name="AParameters"></param>
        public static string UCAddressFilterDataViewRowFilter(Dictionary <String, TVariant>AParameters)
        {
            string returnstring = "TRUE ";

            if (AParameters["param_only_addresses_valid_on"].ToBool())
            {
                returnstring += @" AND SendMail = true";
                returnstring += " AND '" + AParameters["param_today"].ToDate().ToString("yyyy-MM-dd") + "' >= DateEffective AND ('" +
                                AParameters["param_today"].ToDate().ToString("yyyy-MM-dd") + "' <= DateGoodUntil OR DateGoodUntil IS NULL)";
            }

            if (AParameters["param_city"].ToString() != String.Empty)
            {
                returnstring += " AND City = '" + AParameters["param_city"].ToString() + "'";
            }

            if (AParameters["param_postcode_from"].ToString() != String.Empty)
            {
                returnstring += " AND PostalCode >= '" + AParameters["param_postcode_from"].ToString() + "'";
            }

            if (AParameters["param_postcode_to"].ToString() != String.Empty)
            {
                returnstring += " AND PostalCode <= '" + AParameters["param_postcode_to"].ToString() + "'";
            }

            /*
             * if (AParameters["param_region"].ToString() != String.Empty)
             * {
             *  returnstring += " AND  >= " + AParameters["param_postcode_from"].ToString();
             * }
             */
            if (AParameters["param_country"].ToString() != String.Empty)
            {
                returnstring += " AND CountryCode = '" + AParameters["param_country"].ToString() + "'";
            }

            return returnstring;
        }

        /// <summary>
        /// Adds the conditions for a UC_ExtractChkFilter to a Query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns></returns>
        public static string UCExtractChkFilterSQLConditions(Dictionary <String, TVariant>AParameters)
        {
            string query = "";

            if (AParameters["param_active"].ToBool())
            {
                query += " AND p_status_code_c = 'ACTIVE' ";
            }

            if (AParameters["param_families_only"].ToBool())
            {
                query += " AND p_partner.p_partner_class_c LIKE 'FAMILY%' ";
            }

            if (AParameters["param_exclude_no_solicitations"].ToBool())
            {
                query += "AND NOT p_partner.p_no_solicitations_l";
            }

            return query;
        }

        /// <summary>
        /// Adds the primary Phone Email and Fax to a DataTable
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKeyColumn"></param>
        /// <param name="ADbAdapter"></param>
        /// <param name="AIncludeMobile"></param>
        /// <param name="AIncludeAlternateTelephone"></param>
        /// <param name="AIncludeURL"></param>
        public static void AddPrimaryPhoneEmailFaxToTable(DataTable ADataTable, int APartnerKeyColumn, TReportingDbAdapter ADbAdapter,
            Boolean AIncludeMobile = false, Boolean AIncludeAlternateTelephone = false, Boolean AIncludeURL = false)
        {
            TDBTransaction Transaction = null;
            String SelectMobile = "";
            String SelectAlternateTelephone = "";
            String SelectURL = "";
            int IdxMobile = 0;
            int IdxAlternateTelephone = 0;
            int IdxURL = 0;
            int NextIdx = 4;

            List <string>partnerlist = new List <string>();

            foreach (DataRow dr in ADataTable.Rows)
            {
                partnerlist.Add(dr[APartnerKeyColumn].ToString());
            }

            if (partnerlist.Count == 0)
            {
                partnerlist.Add("-1");
            }

            if (AIncludeMobile)
            {
                SelectMobile =
                    @", (
                                      SELECT '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = pattribute.p_value_country_c)|| ' ' || p_value_c

                                      FROM p_partner_attribute AS pattribute

                                      JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                      WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND p_current_l AND p_category_code_c = 'Phone' AND pattribute.p_attribute_type_c = 'Mobile Phone' LIMIT 1
                                   ) AS Mobile"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ;
            }

            if (AIncludeAlternateTelephone)
            {
                SelectAlternateTelephone =
                    @", (
                                      SELECT '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = pattribute.p_value_country_c)|| ' ' || p_value_c

                                      FROM p_partner_attribute AS pattribute

                                      JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                      WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND NOT p_primary_l AND p_current_l AND p_category_code_c = 'Phone' AND pattribute.p_attribute_type_c = 'Phone' LIMIT 1
                                    ) AS AlternateTelephone"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ;
            }

            if (AIncludeURL)
            {
                SelectURL =
                    @", (
                                      SELECT p_value_c

                                      FROM p_partner_attribute AS pattribute

                                      JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                      WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND p_category_code_c = 'Digital Media' AND pattribute.p_attribute_type_c = 'Web Site' LIMIT 1
                                    ) AS URL"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ;
            }

            DataTable PhoneFaxMailDT = new DataTable();
            string Query =
                @"SELECT
                                p_partner_key_n AS partner_key,
                                (
                                    SELECT '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = pattribute.p_value_country_c)|| ' ' || p_value_c

                                    FROM p_partner_attribute AS pattribute

                                    JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                    WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND p_primary_l AND p_category_code_c = 'Phone' AND pattribute.p_attribute_type_c != 'Fax' LIMIT 1
                                ) AS Primary_Phone,

                                (
                                    SELECT p_value_c

                                    FROM p_partner_attribute AS pattribute

                                    JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                    WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND p_primary_l AND p_category_code_c = 'E-Mail' LIMIT 1
                                ) AS Primary_Email,

                                (
                                    SELECT '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = pattribute.p_value_country_c)|| ' ' || p_value_c

                                    FROM p_partner_attribute AS pattribute

                                    JOIN p_partner_attribute_type ON p_partner_attribute_type.p_attribute_type_c = pattribute.p_attribute_type_c

                                    WHERE pattribute.p_partner_key_n = partner.p_partner_key_n AND p_current_l AND p_category_code_c = 'Phone' AND pattribute.p_attribute_type_c = 'Fax' LIMIT 1
                                ) AS Fax"
                +
                SelectMobile
                +
                SelectAlternateTelephone
                +
                SelectURL
                +
                @" FROM p_partner AS partner
                             WHERE p_partner_key_n IN("
                +
                String.Join(",", partnerlist) + ")";

            ADbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PhoneFaxMailDT = ADbAdapter.RunQuery(Query, "PhoneFaxMail", Transaction);
                });

            ADataTable.Columns.Add("Primary_Phone");
            ADataTable.Columns.Add("Primary_Email");
            ADataTable.Columns.Add("Fax");

            if (AIncludeMobile)
            {
                ADataTable.Columns.Add("Mobile");
                IdxMobile = NextIdx;
                NextIdx++;
            }

            if (AIncludeAlternateTelephone)
            {
                ADataTable.Columns.Add("Alternate_Telephone");
                IdxAlternateTelephone = NextIdx;
                NextIdx++;
            }

            if (AIncludeURL)
            {
                ADataTable.Columns.Add("URL");
                IdxURL = NextIdx;
                NextIdx++;
            }

            DataView dv = ADataTable.DefaultView;

            foreach (DataRow dr in PhoneFaxMailDT.Rows)
            {
                dv.RowFilter = ADataTable.Columns[APartnerKeyColumn].ColumnName + " = " + dr[0].ToString();
                dv[0]["Primary_Phone"] = dr[1];
                dv[0]["Primary_Email"] = dr[2];
                dv[0]["Fax"] = dr[3];

                if (AIncludeMobile)
                {
                    dv[0]["Mobile"] = dr[IdxMobile];
                }

                if (AIncludeAlternateTelephone)
                {
                    dv[0]["Alternate_Telephone"] = dr[IdxAlternateTelephone];
                }

                if (AIncludeURL)
                {
                    dv[0]["URL"] = dr[IdxURL];
                }
            }

            dv.RowFilter = String.Empty;

            ADataTable = dv.ToTable();
        }

        /// <summary>
        /// Converts the field names from the database to a readable name
        /// </summary>
        /// <param name="ADataTable"></param>
        public static void ConvertDbFieldNamesToReadable(DataTable ADataTable)
        {
            for (int i = 0; i < ADataTable.Columns.Count; i++)
            {
                string[] parts = ADataTable.Columns[i].ColumnName.Split('_');

                //Replace the prefixes
                string[] prefixes =
                {
                    "p", "addr", "a", "pm", "pt", "u", "s", "pc", "ar"
                };

                for (int z = 0; z < parts.Length; z++)
                {
                    foreach (string prefix in prefixes)
                    {
                        if (parts[z] == prefix)
                        {
                            parts[z] = String.Empty;
                        }
                    }
                }

                string NewColumnName = String.Empty;
                int removeLastChar = parts.Length > 1 && parts[parts.Length - 1].Length <= 1 ? 1 : 0;

                for (int z = 0; z < parts.Length - removeLastChar; z++)
                {
                    if (parts[z] != String.Empty)
                    {
                        NewColumnName += char.ToUpper(parts[z][0]).ToString() + parts[z].Substring(1);
                    }
                }

                if (!ADataTable.Columns.Contains(NewColumnName))
                {
                    ADataTable.Columns[i].ColumnName = NewColumnName;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKeyColumn"></param>
        /// <param name="AParameters"></param>
        /// <param name="ADateKey"></param>
        /// <param name="ADbAdapter"></param>
        public static void AddFieldNameToTable(DataTable ADataTable,
            int APartnerKeyColumn,
            Dictionary <String, TVariant>AParameters,
            String ADateKey,
            TReportingDbAdapter ADbAdapter)
        {
            TDBTransaction Transaction = null;

            DateTime CurrentDate = DateTime.Today;

            if (AParameters.ContainsKey(ADateKey))
            {
                CurrentDate = AParameters[ADateKey].ToDate();
            }

            List <string>partnerlist = new List <string>();

            foreach (DataRow dr in ADataTable.Rows)
            {
                partnerlist.Add("(" + dr[APartnerKeyColumn].ToString() + ")");
            }

            if (partnerlist.Count == 0)
            {
                partnerlist.Add("(-1)");
            }

            DataTable PartnerAndField = new DataTable();
            string Query = @"
                            WITH partnertable AS (VALUES "                                +
                           String.Join(",",
                partnerlist) +
                           @"),
                            persontable AS

                                (SELECT

                                    p_partner_key_n AS Partner_Key,
                                    (SELECT p_partner_short_name_c FROM p_partner WHERE p_partner_key_n = staff.pm_receiving_field_n) AS Field_Name

                                FROM
                                    pm_staff_data AS staff

                                WHERE

                                    staff.p_partner_key_n IN((SELECT * FROM partnertable))
                                    AND pm_start_of_commitment_d <= '"
                           +
                           CurrentDate.ToString("yyyy-MM-dd") + @"'

                                    AND(pm_end_of_commitment_d >= '"                                                                        +
                           CurrentDate.ToString(
                "yyyy-MM-dd") +
                           @"' OR pm_end_of_commitment_d IS NULL)
                                )

                            SELECT

                                partner.p_partner_key_n,
                                    CASE WHEN partner.p_partner_class_c = 'PERSON' THEN string_agg(Field_Name, ',')
                                        ELSE CASE WHEN partner.p_partner_class_c = 'FAMILY' THEN(
                                            SELECT(SELECT p_partner_short_name_c FROM p_partner WHERE p_partner_key_n = p_field_key_n)

                                            FROM p_partner_gift_destination

                                            WHERE p_partner_key_n = partner.p_partner_key_n
                                            AND p_date_effective_d <= '"
                           +
                           CurrentDate.ToString("yyyy-MM-dd") + "' AND (p_date_expires_d >= '" + CurrentDate.ToString("yyyy-MM-dd") +
                           @"
                                                ' OR p_date_expires_d = NULL))

                                        ELSE CASE WHEN partner.p_partner_class_c = 'UNIT' THEN
                                            CASE WHEN(SELECT u_unit_type_code_c FROM p_unit WHERE p_unit.p_partner_key_n = partner.p_partner_key_n) IN('A', 'F', 'D')

                                                THEN partner.p_partner_short_name_c

                                                ELSE(
                                                    SELECT(SELECT p_partner_short_name_c FROM p_partner WHERE p_partner_key_n = um_parent_unit_key_n)

                                                    FROM um_unit_structure

                                                    WHERE um_child_unit_key_n = partner.p_partner_key_n
                                                     )

                                            END
                                        ELSE ''

                                    END END END AS Field_Name
                            FROM persontable
                            RIGHT JOIN p_partner AS partner ON persontable.Partner_Key = partner.p_partner_key_n
                            WHERE partner.p_partner_key_n IN((SELECT * FROM partnertable))

                            GROUP BY partner.p_partner_key_n"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   ;

            ADbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PartnerAndField = ADbAdapter.RunQuery(Query, "PartnerAndField", Transaction);
                });

            ADataTable.Columns.Add("Field", typeof(string));

            DataView dv = ADataTable.DefaultView;

            foreach (DataRow dr in PartnerAndField.Rows)
            {
                dv.RowFilter = ADataTable.Columns[APartnerKeyColumn].ColumnName + " = " + dr[0].ToString();
                dv[0]["Field"] = dr[1];
            }

            dv.RowFilter = String.Empty;

            ADataTable = dv.ToTable();
        }

        /// <summary>
        /// Maps a comma seperated list of column names with use of the given dictionary.
        /// </summary>
        /// <param name="ASortingColumnsAsText">Comma seperated list of Column Names.</param>
        /// <param name="AColumns">Column Collection of the DataTable to check if the given names are valid.</param>
        /// <param name="AMappingDictionaryWithoutBlanks">The Dictionary to use for the translation. Key is the old name, values is the new name. </param>
        /// <returns></returns>
        public static string ColumnMapping(string ASortingColumnsAsText,
            DataColumnCollection AColumns,
            Dictionary <string, string>AMappingDictionaryWithoutBlanks)
        {
            return ColumnNameMapping(ASortingColumnsAsText, AColumns.Cast <DataColumn>().Select(
                    x => x.ColumnName).ToArray(), AMappingDictionaryWithoutBlanks);
        }

        /// <summary>
        /// Maps a comma seperated list of column names with use of the given dictionary.
        /// </summary>
        /// <param name="ASortingColumnsAsText">Comma seperated list of Column Names.</param>
        /// <param name="ADataTableColumns">Column Names of the DataTable as a string[] to check if the given names are valid.</param>
        /// <param name="AMappingDictionaryWithoutBlanks">The Dictionary to use for the translation. Key is the old name, values is the new name. </param>
        /// <returns></returns>
        public static string ColumnNameMapping(string ASortingColumnsAsText,
            string[] ADataTableColumns,
            Dictionary <string, string>AMappingDictionaryWithoutBlanks)
        {
            List <string>list = new List <string>();

            string[] parts = ASortingColumnsAsText.Split(',');

            foreach (string part in parts)
            {
                foreach (KeyValuePair <string, string>entry in AMappingDictionaryWithoutBlanks)
                {
                    if (part.Replace(" ", "") == entry.Key)
                    {
                        list.Add(entry.Value);
                        AMappingDictionaryWithoutBlanks.Remove(entry.Key);
                        break;
                    }
                    else
                    {
                        list.Add(part.Replace(" ", ""));
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                bool delete = true;

                foreach (string column in ADataTableColumns)
                {
                    if (column == list[i])
                    {
                        delete = false;
                    }
                }

                if (delete)
                {
                    TLogging.Log(String.Format(Catalog.GetString(
                                "FastReport Sorting Error: The column name '{0}' couldn't be found in the DataTable. Therefore it has been ignored."),
                            list[i]));
                    list.Remove(list[i]);
                    i--;
                }
            }

            return String.Join(",", list);
        }
    }
}
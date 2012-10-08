//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, jomammele
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
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GuidedTranslation
{
/// <summary>
/// process the pot file for double items
/// </summary>
public class TProcessPot
{
    //contains a list with all items, and its different types of writing
    private static List <ItemWithDerivates>ListWithAllItems = new List <ItemWithDerivates>();


    /// <summary>
    /// a line in a po translation file starts with either msgid or msgstr, and can cover several lines.
    /// the text is in quotes.
    /// </summary>
    private static string ParsePoLine(StreamReader sr, ref string ALine, out StringCollection AOriginalLines)
    {
        AOriginalLines = new StringCollection();
        AOriginalLines.Add(ALine);

        string messageId = String.Empty;
        StringHelper.GetNextCSV(ref ALine, " ");
        string quotedMessage = StringHelper.GetNextCSV(ref ALine, " ");

        if (quotedMessage.StartsWith("\""))
        {
            quotedMessage = quotedMessage.Substring(1, quotedMessage.Length - 2);
        }

        messageId += quotedMessage;

        ALine = sr.ReadLine();

        while (ALine.StartsWith("\""))
        {
            AOriginalLines.Add(ALine);
            messageId += ALine.Substring(1, ALine.Length - 2);
            ALine = sr.ReadLine();
        }

        return messageId;
    }

    /// <summary>
    /// Process the Pot-file for double ocurrences
    /// </summary>
    public static void ProcessPot(string ATemplateFile, string AAnalizeResultsFolder)
    {
        Console.WriteLine("ATemplateFile= " + ATemplateFile);

        Console.WriteLine("AAnalizeResultsFolder= " + AAnalizeResultsFolder);

        Directory.CreateDirectory(AAnalizeResultsFolder);

        StreamReader sr = new StreamReader(ATemplateFile);
        Encoding enc = new UTF8Encoding(false);

        StreamWriter swReadme = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "README.txt"), false, enc);

        swReadme.WriteLine("This folder contains the files which are created analyzing the openpetra-template." + Environment.NewLine +
            Environment.NewLine +
            "The following files are generated:" + Environment.NewLine +
            "analysissummary - contains a summary of the different item-types" + Environment.NewLine +
            Environment.NewLine +
            "firstoccurence - the first ocurrence of an item" + Environment.NewLine +
            "firstoccurence_xword - the first ocurrence of all items consisting of only x word(s)" + Environment.NewLine +
            Environment.NewLine +
            "doubles - all items with their derivates (same item but with different additional characters.,:;&() or casing)");
        swReadme.Close();

        StreamWriter sw_summary = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "analysissummary.txt"), false, enc);
        //contains the first ocurrence of an item
        StreamWriter sw_firstoccurence = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "firstoccurence.txt"), false, enc);

        StreamWriter sw_firstoccurence1word = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "firstoccurence_1word.txt"), false, enc);
        StreamWriter sw_firstoccurence2words = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder,
                "firstoccurence_2words.txt"), false, enc);
        StreamWriter sw_firstoccurence3words = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder,
                "firstoccurence_3words.txt"), false, enc);
        StreamWriter sw_firstoccurence4words = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder,
                "firstoccurence_4words.txt"), false, enc);
        StreamWriter sw_firstoccurence5andmorewords =
            new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "firstoccurence_5andmorewords.txt"), false, enc);


        //contains all items which only differ from the first item in characters like . , ( ) and &
        StreamWriter sw_doubles = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "doubles.txt"), false, enc);
        //conatins all items which only differ from the first item in characters like . , and & and because of chase differences
        //StreamWriter sw_doubles_casedifferences = new StreamWriter(System.IO.Path.Combine(AAnalizeResultsFolder, "doubles_casedifferences.txt"), false, enc);

        sw_firstoccurence.WriteLine("item without .,;:&() --- original item");
        sw_firstoccurence1word.WriteLine("item without .,;:&() --- original item");
        sw_firstoccurence2words.WriteLine("item without .,;:&() --- original item");
        sw_firstoccurence3words.WriteLine("item without .,;:&() --- original item");
        sw_firstoccurence4words.WriteLine("item without .,;:&() --- original item");
        sw_firstoccurence5andmorewords.WriteLine("item without .,;:&() --- original item");
        //sw_doubles.WriteLine("item without .,;:&() --- original item");

        sw_firstoccurence.WriteLine("---------------------------");
        sw_firstoccurence1word.WriteLine("---------------------------");
        sw_firstoccurence2words.WriteLine("---------------------------");
        sw_firstoccurence3words.WriteLine("---------------------------");
        sw_firstoccurence4words.WriteLine("---------------------------");
        sw_firstoccurence5andmorewords.WriteLine("---------------------------");
        //sw_doubles.WriteLine("---------------------------");

        StringCollection firstocurrences = new StringCollection();
        int counterfirstocurrences_all = 0;
        int counterfirstocurrences_1word = 0;
        int counterfirstocurrences_2words = 0;
        int counterfirstocurrences_3words = 0;
        int counterfirstocurrences_4words = 0;
        int counterfirstocurrences_5andmorewords = 0;

        string messageSource = "";
        string line = sr.ReadLine();
        int counter = 0;

        while (line != null)
        {
            counter++;

            if (!line.StartsWith("msgid"))
            {
                if (line.StartsWith("#:"))
                {
                    messageSource = AdaptString(line, "#: ");
                }

                line = sr.ReadLine();
            }
            else if ((line != null) && line.StartsWith("msgid"))
            {
                StringCollection OriginalLines;
                string messageId = ParsePoLine(sr, ref line, out OriginalLines);


                //MessageId_Without = messageId without special characters . , : ; ( ) and &
                string MessageId_Without = AdaptString(messageId, "&");

                //replace all '.' with ''
                MessageId_Without = MessageId_Without.Replace(".", "");
                MessageId_Without = MessageId_Without.Replace(",", "");
                MessageId_Without = MessageId_Without.Replace(";", "");
                MessageId_Without = MessageId_Without.Replace(":", "");
                MessageId_Without = MessageId_Without.Replace("(", "");
                MessageId_Without = MessageId_Without.Replace(")", "");
                //MessageId_Without = MessageId_Without.Replace(" ", "");

                //remove beginning spaces from strings
                while (MessageId_Without.StartsWith(" "))
                {
                    MessageId_Without = MessageId_Without.Remove(0, 1);
                }

                //remove last spaces from strings
                while (MessageId_Without.EndsWith(" "))
                {
                    MessageId_Without = MessageId_Without.Remove(MessageId_Without.Length - 1, 1);
                }

                //the original message but without additional characters and Upper Case
                string MessageId_Without_Upper = MessageId_Without.ToUpper();

                AddItemToListWithAllItems(messageId, MessageId_Without, MessageId_Without_Upper, messageSource);

                //check if item is already in contained in firstocurrences
                //if not then add it to firstocurrences and add it to other files according to its number of words
                if (!firstocurrences.Contains(MessageId_Without))
                {
                    counterfirstocurrences_all++;

                    firstocurrences.Add(MessageId_Without);

                    int numberOfSpaces = MessageId_Without.Split(' ').Length - 1;

                    string stringToWrite = MessageId_Without + " --- \"" + messageId + "\"";

                    sw_firstoccurence.WriteLine(stringToWrite);

                    if (numberOfSpaces == 0) //one word
                    {
                        counterfirstocurrences_1word++;
                        sw_firstoccurence1word.WriteLine(stringToWrite);
                    }
                    else if (numberOfSpaces == 1)  //two words
                    {
                        counterfirstocurrences_2words++;
                        sw_firstoccurence2words.WriteLine(stringToWrite);
                    }
                    else if (numberOfSpaces == 2) //three words
                    {
                        counterfirstocurrences_3words++;
                        sw_firstoccurence3words.WriteLine(stringToWrite);
                    }
                    else if (numberOfSpaces == 3) //four words
                    {
                        counterfirstocurrences_4words++;
                        sw_firstoccurence4words.WriteLine(stringToWrite);
                    }
                    else //five and more words
                    {
                        counterfirstocurrences_5andmorewords++;
                        sw_firstoccurence5andmorewords.WriteLine(stringToWrite);
                    }

                    foreach (string s in OriginalLines)
                    {
                        //sw_firstoccurence.WriteLine(s);
                    }
                }

                //reset messageSource
                messageSource = "";
            }
        }

        sw_firstoccurence1word.WriteLine("---------------------------");
        sw_firstoccurence2words.WriteLine("---------------------------");
        sw_firstoccurence3words.WriteLine("---------------------------");
        sw_firstoccurence4words.WriteLine("---------------------------");
        sw_firstoccurence5andmorewords.WriteLine("---------------------------");


        sw_firstoccurence1word.WriteLine(counterfirstocurrences_1word + " items have 1 word.");
        sw_firstoccurence2words.WriteLine(counterfirstocurrences_2words + " items have 2 words.");
        sw_firstoccurence3words.WriteLine(counterfirstocurrences_3words + " items have 3 words.");
        sw_firstoccurence4words.WriteLine(counterfirstocurrences_4words + " items have 4 words.");
        sw_firstoccurence5andmorewords.WriteLine(counterfirstocurrences_5andmorewords + " items have 5 or more words.");

        int NumberOfItemsWithOneDerivate = 0;
        int NumberOfItemsWithTwoDerivates = 0;
        int NumberOfItemsWithThreeDerivates = 0;
        int NumberOfItemsWithFourDerivates = 0;
        int NumberOfItemsWithFiveDerivates = 0;
        int NumberOfItemsWithSixDerivates = 0;
        int NumberOfItemsWithSevenDerivates = 0;
        int NumberOfItemsWithMoreThanSevenDerivates = 0;

        //fill sw_doubles with data
        for (int i = 0; i < ListWithAllItems.Count; i++) // Loop through List with for
        {
            string StringToWrite = ListWithAllItems[i].ReturnAsString();

            if (StringToWrite != "-1")
            {
                sw_doubles.WriteLine(StringToWrite);
            }

            //increment according counter
            //int NumberOfDerivates = ;
            switch (ListWithAllItems[i].NumberOfDerivates())
            {
                case 1:
                    NumberOfItemsWithOneDerivate++;
                    break;

                case 2:
                    NumberOfItemsWithTwoDerivates++;
                    break;

                case 3:
                    NumberOfItemsWithThreeDerivates++;
                    break;

                case 4:
                    NumberOfItemsWithFourDerivates++;
                    break;

                case 5:
                    NumberOfItemsWithFiveDerivates++;
                    break;

                case 6:
                    NumberOfItemsWithSixDerivates++;
                    break;

                case 7:
                    NumberOfItemsWithSevenDerivates++;
                    break;

                default:
                    NumberOfItemsWithMoreThanSevenDerivates++;
                    break;
            }
        }

        sw_doubles.WriteLine("The following items appear x times (with different case or additional characters)");
        sw_doubles.WriteLine(NumberOfItemsWithOneDerivate + " items appear once.");
        sw_doubles.WriteLine(NumberOfItemsWithTwoDerivates + " items appear twice.");
        sw_doubles.WriteLine(NumberOfItemsWithThreeDerivates + " items appear three times.");
        sw_doubles.WriteLine(NumberOfItemsWithFourDerivates + " items appear four times.");
        sw_doubles.WriteLine(NumberOfItemsWithFiveDerivates + " items appear five times.");
        sw_doubles.WriteLine(NumberOfItemsWithSixDerivates + " items appear six times.");
        sw_doubles.WriteLine(NumberOfItemsWithSevenDerivates + " items appear seven times.");
        sw_doubles.WriteLine(NumberOfItemsWithMoreThanSevenDerivates + " items appear more than seven times.");


        sw_summary.WriteLine("analyzed file: " + ATemplateFile);
        sw_summary.WriteLine("");
        sw_summary.WriteLine(counterfirstocurrences_all + " items contained in template");
        sw_summary.WriteLine(counterfirstocurrences_1word + " items have 1 word.");
        sw_summary.WriteLine(counterfirstocurrences_2words + " items have 2 words.");
        sw_summary.WriteLine(counterfirstocurrences_3words + " items have 3 words.");
        sw_summary.WriteLine(counterfirstocurrences_4words + " items have 4 words.");
        sw_summary.WriteLine(counterfirstocurrences_5andmorewords + " items have 5 or more words.");
        sw_summary.WriteLine("---------------------------");

        sw_summary.WriteLine("The following items appear x times (with different case or additional characters)");
        sw_summary.WriteLine(NumberOfItemsWithOneDerivate + " items appear once.");
        sw_summary.WriteLine(NumberOfItemsWithTwoDerivates + " items appear twice.");
        sw_summary.WriteLine(NumberOfItemsWithThreeDerivates + " items appear three times.");
        sw_summary.WriteLine(NumberOfItemsWithFourDerivates + " items appear four times.");
        sw_summary.WriteLine(NumberOfItemsWithFiveDerivates + " items appear five times.");
        sw_summary.WriteLine(NumberOfItemsWithSixDerivates + " items appear six times.");
        sw_summary.WriteLine(NumberOfItemsWithSevenDerivates + " items appear seven times.");
        sw_summary.WriteLine(NumberOfItemsWithMoreThanSevenDerivates + " items appear more than seven times.");

        sr.Close();
        sw_summary.Close();
        sw_firstoccurence.Close();
        sw_firstoccurence1word.Close();
        sw_firstoccurence2words.Close();
        sw_firstoccurence3words.Close();
        sw_firstoccurence4words.Close();
        sw_firstoccurence5andmorewords.Close();
        sw_doubles.Close();
        //sw_doubles_casedifferences.Close();
    }

    /// <summary>
    /// remove a substring from a String
    /// </summary>
    /// <param name="ALine">String from which the substring should be removed</param>
    /// <param name="ARemoveString">substring to remove</param>
    private static string AdaptString(string ALine, string ARemoveString)
    {
        string ALine_part;

        if (ALine.Contains(ARemoveString))
        {
            ALine_part = ALine.Remove(ALine.IndexOf(ARemoveString), ARemoveString.Length);
        }
        else
        {
            ALine_part = ALine;
        }

        return ALine_part;
    }

    //add an Item to the List that contains all Items
    private static void AddItemToListWithAllItems(string messageId, string MessageId_Without, string MessageId_Without_Upper, string messageSource)
    {
        /*if(MessageId_Without.Equals("Help"))
         * {
         *  string test = "";
         *  //for debugger
         * }*/

        OriginalItem MyOriginalItem = new OriginalItem();

        MyOriginalItem.OriginalString = messageId;
        MyOriginalItem.SourceLocation = messageSource;

        //check if item alreay exists in lists
        //if yes then add a new derivate to the existing item
        //if not then add a new Item


        bool Found = false;

        for (int i = 0; i < ListWithAllItems.Count; i++) // Loop through List with for
        {
            //check if item is already in list
            if (ListWithAllItems[i].StringWithoutAdditionalCharacters.Equals(MessageId_Without_Upper))
            {
                //item without additional characters found
                //check if the actual derivate is already in ItemWithDerivates and if not then add it as a new derivate
                Found = true;
                ListWithAllItems[i].AddNewDerivate(MyOriginalItem);
            }

//            if(ListWithAllItems[i].StringWithoutAdditionalCharacters.Equals("Help"))
            {
            }
        }

        if (!Found)
        {
            ItemWithDerivates MyItemWithDerivates = new ItemWithDerivates();
            MyItemWithDerivates.StringWithoutAdditionalCharacters = MessageId_Without_Upper;
            MyItemWithDerivates.AddNewDerivate(MyOriginalItem);
            ListWithAllItems.Add(MyItemWithDerivates);
        }
    }
}
}
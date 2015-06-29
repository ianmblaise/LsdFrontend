using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LsdLibrary;

namespace LsdFrontend
{
    class Program
    {
        static void Main(string[] args)
        {
            // Subscribe to the events 
            LolSkillData.GameDataRetrieved += LolSkillData_GameDataRetrieved;
            LolSkillData.GameDataDownloadStarting += LolSkillData_GameDataDownloadStarting;
            LolSkillData.GameDataParsed += LolSkillData_GameDataParsed;

          while(true)
            {
                Console.WriteLine("Hi, enter some summoner names, seperated by commas.");

                var readLine = Console.ReadLine();
                if (readLine == null) continue;

                var names = readLine.Split(',');

                foreach (var lsd in from t in names where t != null select t.TrimStart(' ') into name select name.TrimEnd(' ') into name select new LolSkillData(name, "NA"))
                {
                    lsd.DownloadGameDataString();
                }
            }
        }
        /// <summary>
        ///     todo: IMPLEMENT THIS FUNCTION NEXT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LolSkillData_GameDataParsed(object sender, LsdDownloadEventArgs e)
        {
            var lsd = (LolSkillData) sender;

            if (lsd == null)
            {
                Console.WriteLine("no data from sender.");
                return;
            }

            var dictData = (Dictionary<string, string>) e.Result;

            if (dictData == null || dictData.Keys.Count < 1)
            {
                Console.WriteLine("No keys or null dictionary.");
                return;
            }

            Console.WriteLine("---------------------------\n" + lsd.SearchName + "\'s game.\n--------------------------");

            foreach (var item in dictData)
            {
                    Console.WriteLine(item.Key + " = " + item.Value);
            }
        }

        private static void LolSkillData_GameDataDownloadStarting(object sender, LsdDownloadEventArgs e)
        {
            var senderObj = (LolSkillData) sender;
            if (senderObj == null)
            {
                throw new ArgumentNullException(nameof(sender), "The sender has no value.");
            }
            Console.WriteLine("Starting a download for : " + senderObj.SearchName + " on " + senderObj.Region);
        }

        private static void LolSkillData_GameDataRetrieved(object sender, LsdDownloadEventArgs e)
        {
            var senderObj = (LolSkillData) sender;
            if (senderObj == null)
            {
                throw new ArgumentNullException(nameof(sender), "The sender has no value.");
            }

            if (e.Result == null)
            {
                throw new Exception("The value of Result was not valid. ",
                    new Exception("Result was null! Here is what I know: " + e.Message));
            }
        }
    }
}

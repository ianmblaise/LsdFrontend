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
            LolSkillData.GameDataRetrieved += LolSkillData_GameDataRetrieved;
            LolSkillData.GameDataDownloadStarting += LolSkillData_GameDataDownloadStarting;
            LolSkillData.GameDataParsed += LolSkillData_GameDataParsed;

            Console.WriteLine("Hi, enter some summoner names, seperated by commas.");
            var readLine = Console.ReadLine();
            if (readLine != null)
            {
                string[] names = readLine.Split(',');

                for (var i = 0; i < names.Length; i++)
                {
                    if (names[i] == null)
                    {
                        continue;
                    }

                    string name = names[i].TrimStart(' ');
                    name = name.TrimEnd(' ');
                    LolSkillData lsd = new LolSkillData(name, "NA");
                    lsd.DownloadGameDataString();
                }
            }
            Console.ReadLine();
        }
        /// <summary>
        ///     todo: IMPLEMENT THIS FUNCTION NEXT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LolSkillData_GameDataParsed(object sender, LsdDownloadEventArgs e)
        {
            //TODO: WORK ON ASAP
        }

        private static void LolSkillData_GameDataDownloadStarting(object sender, LsdDownloadEventArgs e)
        {
            LolSkillData senderObj = (LolSkillData) sender;
            if (senderObj == null)
            {
                throw new ArgumentNullException(nameof(sender), "The sender has no value.");
            }
            Console.WriteLine("Starting a download for : " + senderObj.SearchName + " on " + senderObj.Region);
        }

        private static void LolSkillData_GameDataRetrieved(object sender, LsdDownloadEventArgs e)
        {
            LolSkillData senderObj = (LolSkillData)sender;
            if (senderObj == null)
            {
                throw new ArgumentNullException(nameof(sender), "The sender has no value.");
            }

            Console.WriteLine($"Got data for {senderObj.SearchName} So now parse the page.");

            if (e.Result == null)
            {
                throw new Exception("The value of Result was not valid. ", new Exception("Result was null! Here is what I know: " + e.Message));
            }
            
        }
    }
}

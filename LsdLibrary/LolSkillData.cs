using System;
using System.Collections.Generic;
using System.Net;

namespace LsdLibrary
{
    public class LolSkillData
    {
        public delegate void LsdDownloadEventHandler(object sender, LsdDownloadEventArgs e);

        public string SearchName { get; }

        public string Region { get; }

        public LolSkillData(string searchName, string region)
        {
            SearchName = searchName;
            Region = region;
        }

        /// <summary>
        ///     Downloads the players LolSkill game page of the current <see cref="LolSkillData"/> object.
        ///     <para>If there is no value assigned to the <b>SearchName</b> then throws an <see cref="ArgumentNullException"/>.
        ///     Raises the events <see cref="GameDataDownloadStarting"/> and <see cref="GameDataRetrieved"/>, respectively.</para>
        /// </summary>
        public void DownloadGameDataString()
        {
            // -- Throw an exception if the SearchName is blank. -- // 
            if (string.IsNullOrEmpty(SearchName))
            {
                throw new Exception("SearchName value was invalid.");
            }
            
            //------Invoke GameDataDownloadStarted------//
            OnGameDataDownloadStarted(this, new LsdDownloadEventArgs("Started downloading page for " + SearchName));

            //------Start downloading the game data page-----//
            WebClient webCli = new WebClient();
            //------Subscribe to the event that notifies all subscribers that a download finished-----//
            webCli.DownloadStringCompleted += WebCli_DownloadStringCompleted;
            webCli.DownloadStringAsync(new Uri("http://www.lolskill.net/game/" + $"{Region}/{SearchName}"));
        }

        private void WebCli_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {            
            //------Invoke GameDataRetrieved-----//
            OnGameDataRetrieved(this, new LsdDownloadEventArgs("Finished downloading page for " + SearchName, e.Result));
        }

        public Dictionary<string, string> ParseGameData(string gameData)
        {
            if (string.IsNullOrEmpty(gameData))
            {
                throw new Exception("No data to parse!");
            }

            Dictionary<string, string> dataRead = new Dictionary<string, string>();



            return dataRead;


        } 

        public static event LsdDownloadEventHandler GameDataRetrieved;
        public static event LsdDownloadEventHandler GameDataParsed;
        public static event LsdDownloadEventHandler GameDataDownloadStarting;

        public void OnGameDataDownloadStarted(object sender, LsdDownloadEventArgs e)
        {
            GameDataDownloadStarting?.Invoke(sender, e);
        }

        public void OnGameDataParsed(object sender, LsdDownloadEventArgs e)
        {
            GameDataParsed?.Invoke(sender, e);
        }

        public void OnGameDataRetrieved(object sender, LsdDownloadEventArgs e)
        {
            GameDataRetrieved?.Invoke(sender, e);
        }
    }
}

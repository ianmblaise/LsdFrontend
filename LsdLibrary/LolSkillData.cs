using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;

namespace LsdLibrary
{
    public class LolSkillData
    {
        public delegate void LsdDownloadEventHandler(object sender, LsdDownloadEventArgs e);

        public string SearchName { get; }

        public string Region { get; }

        public ParseKeys LatestData = new ParseKeys();

        /// <summary>
        ///     LolSkillData is used for downloading summoner data from LolSkill. 
        /// </summary>
        /// <param name="searchName">Player to search for.</param>
        /// <param name="region">Region they play on.</param>
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
                Console.WriteLine("bad input. ");
                return;
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
            //------Invoke GameDataRetrieved if the download string contains any character(s)-----//
            if(e.Error != null || e.Result.Length < 1)
            { 
                Console.WriteLine("Result for player " + SearchName + " was not worthy.");
                return;
            }

            OnGameDataRetrieved(this, new LsdDownloadEventArgs("Finished downloading page for " + SearchName, e.Result));
            var result = new Dictionary<string, string>();
            bool succeeded = ParseGameData(e.Result, out result);
            if (succeeded)
            {
                OnGameDataParsed(this, new LsdDownloadEventArgs("Parsed data", result));
            }
        }

        private bool ParseGameData(string gameData, out Dictionary<string, string> rez)
        {
            if (string.IsNullOrEmpty(gameData))
            {
                rez = null;
                return false;
            }
            // Dictionary<string, string> dataRead = ParseKeys.GameDataKeys.ToDictionary(key => key.Key, key => GetStringInBetween(key.Value, gameData));

            // 
            var dataRead = new Dictionary<string, string>();

            //--------------------------------------PLAYERS GAMECARDS----------------------------------------------//
            int counter = 0;
            using (StringReader sr = new StringReader(gameData))
            {
                string currentLineVal;
                string currentPlayerName = "NULL";
                while ((currentLineVal = sr.ReadLine()) != null)
                { 
                    
                    //--PLAYER NAME--//
                    if (currentLineVal.StartsWith("<div class=\"summonername\""))
                    {
                        counter++;
                        currentPlayerName = "player" + counter;
                            string playersName = Uri.UnescapeDataString(GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.name"],
                            currentLineVal));

                        // Console.WriteLine("--------> Name: " + playersName);
                        dataRead["game.player." + currentPlayerName + ".name"] = playersName;

                        if (counter <= 5)
                        {
                            // Console.WriteLine("--------> Team: " + "Blue");
                            dataRead["game.player." + currentPlayerName + ".team"] = "blue";
                        }

                        if (counter > 5)
                        {
                            // Console.WriteLine("--------> Team: " + "Red");
                            dataRead["game.player." + currentPlayerName + ".team"] = "red";
                        }
                    }

                    //---CHAMP NAME----//
                    if (currentLineVal.StartsWith("<div class=\"champion tooltip\""))
                    {
                        string cName = GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.champion"],
                            currentLineVal);
                      //  Console.WriteLine("--------> Champ: " + cName);
                        dataRead["game.player." + currentPlayerName + ".champion"] = cName;
                    }
                    //---SUMMONER SPELLS---//
                    if (currentLineVal.StartsWith("<div class=\"spell tooltip\""))
                    {
                        // 4 = Flash
                        // 11 = Smite
                        // 14 = Ignite
                        // 7 = Heal
                        // 3 = Exhaust
                        // 12 = Teleport
                        int spellId = int.Parse(GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.spell"],
                            currentLineVal));
                    //    Console.WriteLine("--------> Spell: " + spellId);
                        if (!dataRead.ContainsKey("game.player." + currentPlayerName + ".spellone"))
                        {
                            dataRead["game.player." + currentPlayerName + ".spellone"] = spellId.ToString();
                        }
                        else
                        {
                            dataRead["game.player." + currentPlayerName + ".spelltwo"] = spellId.ToString();
                        }


                    }

                    if (!currentLineVal.StartsWith("<div class=\"skillscore tooltip\"")) continue;
                    var lsScore =
                        GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.lolskillscore"],
                            currentLineVal);

                    var performance =
                        GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.performance"],
                            currentLineVal);

                    var experience =
                        GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.experience"],
                            currentLineVal);

                    var generalSkill =
                        GetStringInBetween(ParseKeys.GameDataKeys["game.item.player.generalskill"],
                            currentLineVal);

                    dataRead["game.player." + currentPlayerName + ".lolskillscore"] = lsScore;
                    dataRead["game.player." + currentPlayerName + ".performance"] = performance;
                    dataRead["game.player." + currentPlayerName + ".experience"] = experience;
                    dataRead["game.player." + currentPlayerName + ".generalskill"] = generalSkill;
                }
            }

            //------------------------------------END GAME CARDS-------------------------------------------//

                if (dataRead.Keys.Count < 1)
                {
                    rez = null;
                    return false;
                }

            
            rez = dataRead;
            return true;
        }

        public static string GetStringInBetween(string[] tags, string bodyWithTags, bool trimTags = true)
        {
            int startIndex = bodyWithTags.IndexOf(tags[0], StringComparison.Ordinal);

            if (startIndex < 0)
            {
                return string.Empty;
            }

            bodyWithTags = bodyWithTags.Substring(startIndex + tags[0].Length);
            int endIndex = bodyWithTags.IndexOf(tags[1], StringComparison.Ordinal);
            if (endIndex < 0)
            {
                return string.Empty;
            }
            string result = bodyWithTags.Substring(0, endIndex);
            if (result.Length < 1)
            {
                result = string.Empty;
            }

            if (!trimTags)
            {
                result = tags[0] + result + tags[1];
            }

            return result;
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

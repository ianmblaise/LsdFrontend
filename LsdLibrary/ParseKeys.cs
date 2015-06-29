using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LsdLibrary
{
    public class ParseKeys
    {
        public static Dictionary<string, string[]> GameDataKeys = new Dictionary<string, string[]>
        {
            
            { "game.container.headline" , new [] { "<div class=\"headline\">" , "</div></div>" } },
            { "game.container.headline.box" , new [] { "<div class=\"box\">", "</div>"} },
            { "game.item.headline.box.playername" , new [] {"<h2>", "</h2>"}},
            { "game.item.headline.box.currentgame" , new[] {"<h3>", "</h3>"}},
            
            /* gamecard */
            { "game.item.player.summonerid", new[] {"<div class=\"gamecard blueteam\" data-summoner-id=\"", "\">"} },
            { "game.item.player.name", new[] { "summoner/NA/", "\">"}},
        
            /* champion tooltip */
            { "game.item.player.champion", new[] {"a href=\"champion/", "\">"}},
            
            /* spell tooltip */
            { "game.item.player.spell", new[] {"data-spell-id=\"", "\""} },
            
            /* skillscore tooltip */
            { "game.item.player.lolskillscore", new[] {" has a LoLSkillScore of <b>", "</b>"} },
            { "game.item.player.performance", new[] {"Performance: <b>", "</b>"} },
            { "game.item.player.generalskill", new[] {"<br>General Skill: <b>", "</b>"} },
            { "game.item.player.experience", new[] {"Experience: <b>", "</b>"} },
            
            /* wins tooltip */
            { "game.item.player.rankedwins" , new [] { "<b>Ranked Wins: ", "</b>" } },
        };

        /// <summary>
        ///     This object will store the latest data that has been parsed. 
        /// </summary>
        public Dictionary<string, string> Latest = new Dictionary<string, string>(); 
    }
}

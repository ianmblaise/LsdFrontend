using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LsdLibrary
{
    public static class ParseKeys
    {
        public static Dictionary<string, string[]> GameDataKeys = new Dictionary<string, string[]>
        {
            //-----Html tags that have the data we want within them.-----//
            {"game.headline", new []{"<div class=\"headline\">", "</div></div>"}},
            {"game.headline.box", new [] {"<div class=\"box\">", "</div>"}},
            {"game.headline.box.playername", new [] {"<h2>", "</h2>"}}, 
            {"game.headline.box.currentgame", new[] {"<h3>", "</h3>"}}
                
        };
    }
}

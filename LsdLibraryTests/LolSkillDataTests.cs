using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LsdLibraryTests
{
    [TestClass]
    public class LolSkillDataTests
    {
        public static string TestName;
        public static string TestRegion;

        
        public LolSkillDataTests(string testName, string testRegion)
        {
            TestName = testName;
            TestRegion = testRegion;
        }

        [TestMethod]
        public void DownloadGameDataStringTest()
        {
            var SearchName = "xelle";
            // -- Throw an exception if the SearchName is blank. -- // 
            if (string.IsNullOrEmpty(SearchName))
            {
                Assert.Fail("Failed because the SearchName was invalid.");
            }

            //------Start downloading the game data page-----//
            WebClient webCli = new WebClient();
            string data = webCli.DownloadString(new Uri("http://www.lolskill.net/game/NA/" + SearchName));
            if (data.Length < 1)
            {
                Assert.Fail("Didn't get valid data from the DownloadString method.");
            }

            Assert.IsTrue(data.Length > 0 && data.Contains(SearchName));

        }

        [TestMethod()]
        public void ParseGameDataTest()
        {
            Assert.Fail();
        }
    }
}
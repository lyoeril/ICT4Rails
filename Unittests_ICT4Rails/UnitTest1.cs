using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICT4Rails;

namespace Unittests_ICT4Rails
{
    [TestClass]
    public class UnitTest1
    {
        //Fields
        private Medewerker medewerker;        
        private Reservering reservering;
        private Spoor spoor;
        private Onderhoud onderhoud;
        private Status status;
        private Tram tram;
        private TramType type;

        [TestInitialize]
        public void Initialize()
        {
            medewerker = new Medewerker("testnaam", "testemail", "testfunctie", "testadres", "testpostcode", "testwachtwoord");
            spoor = new Spoor(1, 1, true);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}

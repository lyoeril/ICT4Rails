using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICT4Rails;

namespace Unittests_ICT4Rails
{
    [TestClass]
    public class TestICT4Rails
    {
        //Fields
        private Medewerker medewerker;
        private Spoor spoor;
        private TramType tramType;
        private Status status;
        private Reservering reservering;       
        private Onderhoud onderhoud;       
        private Tram tram;

        private DateTime reserveringDatum;
        private DateTime starttijd;
        private DateTime eindtijd;
        private DateTime uitrijdtijd;
        

        [TestInitialize]
        public void Initialize()
        {
            reserveringDatum = new DateTime(2015, 10, 15);
            starttijd = new DateTime(2015, 6, 1, 12, 32, 30);
            eindtijd = new DateTime(2015, 6, 2, 14, 00, 00);
            uitrijdtijd = new DateTime(2015, 10, 15, 20, 00, 00);
            
            medewerker = new Medewerker("testnaam", "testemail", "testfunctie", "testadres", "testpostcode", "testwachtwoord");
            spoor = new Spoor(1, 1, true);
            tramType = new TramType("testtypenaam", "testbeschrijving", 1);
            status = new Status("teststatusnaam", "testbijzonderheden");
            tram = new Tram(tramType, status, 1, 1, true, uitrijdtijd);
            reservering = new Reservering(tram, spoor, reserveringDatum);
            onderhoud = new Onderhoud(medewerker, tram, starttijd, eindtijd, "testopmerking", "testsoort");
        }

        [TestMethod]
        public void TestMedewerker()
        {
            //Properties
            Assert.AreEqual("testnaam", medewerker.Naam);
            Assert.AreEqual("testemail", medewerker.Email);
            Assert.AreEqual("testfunctie", medewerker.Functie);
            Assert.AreEqual("testadres", medewerker.Adres);
            Assert.AreEqual("testpostcode", medewerker.Postcode);
            Assert.AreEqual("testwachtwoord", medewerker.Wachtwoord);

            //In-en uitloggen
            //TODO

            //ToString
            Assert.AreEqual("Naam: testnaamEmailadres: testemailFunctie: testfunctieAdres: testadresPostcode: testpostcode", medewerker.ToString());
            medewerker = new Medewerker("", "", "", "", "", "");
            Assert.AreEqual("Naam: OnbekendEmailadres: OnbekendFunctie: OnbekendAdres: OnbekendPostcode: Onbekend", medewerker.ToString());            
        }

        [TestMethod]
        public void TestOnderhoud()
        {
            Assert.AreEqual(medewerker, onderhoud.Medewerker);
            Assert.AreEqual(tram, onderhoud.Tram);
            Assert.AreEqual(starttijd, onderhoud.Starttijd);
            Assert.AreEqual(eindtijd, onderhoud.Eindtijd);
            Assert.AreEqual("testopmerking", onderhoud.Opmerking);
            Assert.AreEqual("testsoort", onderhoud.Soort);

            Assert.AreEqual("", onderhoud.ToString());
        }
    }
}

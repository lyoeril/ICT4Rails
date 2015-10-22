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
        private Administratie administratie;
        private Gebruiker gebruiker;

        private DateTime reserveringDatum;
        private DateTime starttijd;
        private DateTime eindtijd;
        private DateTime uitrijdtijd;
        

        [TestInitialize]
        public void Initialize()
        {
            reserveringDatum = new DateTime(2015, 10, 15);
            starttijd = new DateTime(2015, 6, 2, 14, 00, 00);
            eindtijd = new DateTime(2015, 6, 2, 14, 00, 00);
            uitrijdtijd = new DateTime(2015, 10, 15, 20, 00, 00);
            
            medewerker = new Medewerker(1, "testnaam", "testemail", "testfunctie", "testadres", "testpostcode");
            spoor = new Spoor(1, 1, true);
            tramType = new TramType("testtypenaam", "testbeschrijving", 1);
            status = new Status("teststatusnaam", "testbijzonderheden");
            tram = new Tram(tramType, status, 1, 1, true, uitrijdtijd);
            reservering = new Reservering(tram, spoor, reserveringDatum);
            onderhoud = new Onderhoud(1, medewerker, tram, starttijd, eindtijd, "testopmerking", "testsoort");
            administratie = new Administratie();
            gebruiker = new Gebruiker(1, "testgebruikersnaam", "testwachtwoord");
        }
        
        [TestMethod]
        public void TestMedewerker()
        {
            //Properties
            Assert.AreEqual("testnaam", medewerker.Naam);
            Assert.AreEqual("testemail", medewerker.Email);
            Assert.AreEqual("testfunctie", medewerker.Functie);
            Assert.AreEqual("testadres", medewerker.Adres);
<<<<<<< HEAD
            Assert.AreEqual("testpostcode", medewerker.Postcode);
=======

            Assert.AreEqual("testpostcode", medewerker.Postcode);     
>>>>>>> origin/master

            //ToString      

            Assert.AreEqual("ID: 1 - Naam: testnaam - Emailadres: testemail - Functie: testfunctie - Adres: testadres - Postcode: testpostcode", medewerker.ToString(), "Fout bij medewerker");
<<<<<<< HEAD
            Assert.AreEqual("testpostcode", medewerker.Postcode);

            //ToString
            Assert.AreEqual("Naam: testnaam - Emailadres: testemail - Functie: testfunctie - Adres: testadres - Postcode: testpostcode", medewerker.ToString());
            medewerker = new Medewerker(1, "", "", "", "", "");
            Assert.AreEqual("Naam: Onbekend - Emailadres: Onbekend - Functie: Onbekend - Adres: Onbekend - Postcode: Onbekend", medewerker.ToString());            
            Assert.AreEqual("ID: 1Naam: testnaamEmailadres: testemailFunctie: testfunctieAdres: testadresPostcode: testpostcode", medewerker.ToString(), "Fout bij medewerker");
=======

>>>>>>> origin/master
            medewerker = new Medewerker(1, "", "", "", "", "");
            Assert.AreEqual("ID: 1 - Naam: Onbekend - Emailadres: Onbekend - Functie: Onbekend - Adres: Onbekend - Postcode: Onbekend", medewerker.ToString());   
        }
        
        [TestMethod]
        public void TestOnderhoud()
        {            
            Assert.AreEqual(medewerker, onderhoud.Medewerker);              
            Assert.AreEqual(tram, onderhoud.Tram);
            Assert.AreEqual("testopmerking", onderhoud.Opmerking);
            Assert.AreEqual("testsoort", onderhoud.Soort);
            Assert.AreEqual(eindtijd, onderhoud.Eindtijd);
            
            //TODO
            Assert.AreEqual("ID: 1 - Tram: 1 - Medewerker: testnaam - Soort: testsoort - Starttijd: 14:00:00 - Eindtijd: 14:00:00 - Opmerking: testopmerking", onderhoud.ToString());
            
            onderhoud = new Onderhoud(1, null, null, starttijd, eindtijd, "", "");
            Assert.AreEqual("Nummer: 1 - Lijn: 1 - Type: testtypenaam - Status: teststatusnaam - Beschikbaar: Ja - Uitrijdatum: 15-10-2015 00:00:00 - Uitrijtijd: 20:00:00", tram.ToString());
        }

        [TestMethod]
        public void TestSpoor()
        {
            Assert.AreEqual(1, spoor.Spoornummer);
            Assert.AreEqual(1, spoor.Lijn);
            Assert.AreEqual(true, spoor.Beschikbaar);

            Assert.AreEqual("Spoornummer: 1 - Lijn: 1 - Beschikbaar: Ja", spoor.ToString());
            
            spoor = new Spoor(0, 0, false);
            Assert.AreEqual("Spoornummer: Onbekend - Lijn: Onbekend - Beschikbaar: Nee", spoor.ToString());  
            
        }

        [TestMethod]
        public void TestStatus()
        {
            Assert.AreEqual("teststatusnaam", status.Naam);
            Assert.AreEqual("testbijzonderheden", status.Bijzonderheden);

            Assert.AreEqual("Naam: teststatusnaam - Bijzonderheden: testbijzonderheden", status.ToString());
            
            status = new Status(null, null);
            Assert.AreEqual("Naam: Onbekend - Bijzonderheden: Onbekend", status.ToString());
            
            status = new Status("", "");
            Assert.AreEqual("Naam: Onbekend - Bijzonderheden: Onbekend", status.ToString());
        }

        [TestMethod]
        public void TramTypeTest()
        {
            tramType = new TramType("testtypenaam", "testbeschrijving", 1);
            Assert.AreEqual("testtypenaam", tramType.Naam);
            Assert.AreEqual("testbeschrijving", tramType.Beschrijving);
            Assert.AreEqual(1, tramType.Lengte);

            Assert.AreEqual("Naam: testtypenaam - Beschrijving: testbeschrijving - Lengte: 1", tramType.ToString());
            
            tramType = new TramType("", "", 0);

            Assert.AreEqual("Naam: Onbekend - Beschrijving: Onbekend - Lengte: Onbekend", tramType.ToString());            
        }

        [TestMethod]
        public void TramTest()
        {
            tram = new Tram(tramType, status, 1, 1, true, uitrijdtijd);
            Assert.AreEqual(tramType, tram.Type);
            Assert.AreEqual(status, tram.Status);
            Assert.AreEqual(1, tram.Lijn);
            Assert.AreEqual(1, tram.Nummer);
            Assert.AreEqual(true, tram.Beschikbaar);
            Assert.AreEqual(uitrijdtijd, tram.Uitrijdtijd);

            //ToString
            Assert.AreEqual("Nummer: 1 - Lijn: 1 - Type: testtypenaam - Status: teststatusnaam - Beschikbaar: Ja - Uitrijdatum: 15-10-2015 00:00:00 - Uitrijtijd: 20:00:00", tram.ToString());
             
            tram = new Tram(null, null, 0, 0, false, uitrijdtijd);
            Assert.AreEqual("Nummer: Onbekend - Lijn: Onbekend - Type: Onbekend - Status: Onbekend - Beschikbaar: Nee - Uitrijdatum: 15-10-2015 00:00:00 - Uitrijtijd: 20:00:00", tram.ToString()); 
        }

        [TestMethod]
        public void ReserveringTest()
        {
            reservering = new Reservering(tram, spoor, reserveringDatum);
            Assert.AreEqual(reserveringDatum, reservering.Datum);
            Assert.AreEqual("Tram: 1 - Spoor: 1 - Datum: 15-10-2015 00:00:00", reservering.ToString());
            reservering = new Reservering(null, null, reserveringDatum);
            Assert.AreEqual("Tram: Onbekend - Spoor: Onbekend - Datum: 15-10-2015 00:00:00", reservering.ToString());   
        }

        [TestMethod]
        public void AdministratieMedewerkerTest()
        {
            //Add bestaande medewerker moet false geven
            Assert.IsTrue(administratie.AddMedewerker(medewerker));
            
            try
            {
                administratie.AddMedewerker(medewerker);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
            
             
            //remove medewerker
            Assert.IsTrue(administratie.RemoveMedewerker(medewerker), "Remove Medewerker Fout");

            //remove niet bestaande medewerker
            Assert.IsFalse(administratie.RemoveMedewerker(medewerker));
        }

        [TestMethod]
        public void AdministratieOnderhoudTest()
        {
            //Toevoegen van een onderhoudsbeurt aan de administratie
            Assert.IsTrue(administratie.AddOnderhoudsbeurt(onderhoud));

            //Toevoegen van een al toegevoegde onderhoudsbeurt
            try
            {
                administratie.AddOnderhoudsbeurt(onderhoud);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }

            //Verwijderen van een bestaande onderhoudsbeurt
            Assert.IsTrue(administratie.RemoveOnderhoudsbeurt(onderhoud));

            //Verwijderen van een niet-bestaande onderhoudsbeurt
            try
            {
                administratie.RemoveOnderhoudsbeurt(onderhoud);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }
        
        [TestMethod]
        public void AdministratieTramTest()
        {
            //toevoegen
            Assert.IsTrue(administratie.AddTram(tram));

            try
            {
                administratie.AddTram(tram);
                Assert.Fail();
            }
            catch (Exception) { }


            //Verwijderen
            Assert.IsTrue(administratie.RemoveTram(tram));
            Assert.IsFalse(administratie.RemoveTram(tram));

        }

        [TestMethod]
        public void DatabaseTest()
        {
            //TODO
        }

        [TestMethod]
        public void GebruikerTest()
        {
            gebruiker = new Gebruiker(1, "testgebruikersnaam", "testwachtwoord");
            Assert.AreEqual(1, gebruiker.ID);
            Assert.AreEqual("testgebruikersnaam", gebruiker.GebruikersNaam);

            Assert.IsTrue(gebruiker.LogIn("testwachtwoord"));
            Assert.IsFalse(gebruiker.LogIn("foutwachtwoord"));
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AWGP;
using Microsoft.Xna.Framework;

namespace TestAWGP
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestParticle
    {
        private Particle p;
 
        public TestParticle()
        {
            //
            // TODO: Add constructor logic here
            //
            p = new Particle();           
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetLife()
        {
            //
            // TODO: Add test logic here
            //
            Assert.AreEqual(0.0f,p.GetLife());
        }

        [TestMethod]
        public void TestGetMass()
        {
            //
            // TODO: Add test logic here
            //
            Assert.AreEqual(0.0f, p.GetMass());
        }

        [TestMethod]
        public void TestSetGetPostions()
        {
            //
            // TODO: Add test logic here
            //

            p.position = new Vector2(10.0f, 10.0f);
            Assert.AreEqual(10.0f, p.position.X);
            Assert.AreEqual(10.0f, p.position.Y);
                      
        }
    }
}

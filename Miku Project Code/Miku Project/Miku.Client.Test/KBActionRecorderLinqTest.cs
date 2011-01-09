using Miku.Client.Models.Recorders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Miku.Client;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Windows.Forms;
using Miku.Client.Models.Hooks;


namespace Miku.Client.Test
{
    
    /// <summary>
    ///This is a test class for KBActionRecorderLinqTest and is intended
    ///to contain all KBActionRecorderLinqTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KBActionRecorderLinqTest
    {

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for KBActionRecorderLinq Constructor
        ///</summary>
        [TestMethod()]
        public void KBActionRecorderLinqConstructorTest()
        {
            string actionsListFileName = "actionsListFileName"; // TODO: Initialize to an appropriate value
            KBActionRecorderLinq target = new KBActionRecorderLinq(actionsListFileName);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for AddKeyboradActionNodesToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddKeyboradActionNodesToActionListTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            List<Win32API.KeyEvent> datas = new List<Win32API.KeyEvent>();
            IEnumerable<XElement> re = new List<XElement>() 
            {
                new XElement(new XElement(
                "Action", new XAttribute("Type", "KeyboardAct"),
                new XAttribute("DelayTime", 10.ToString()),
               new XElement("KeyData", ((int)Keys.A).ToString()), new XAttribute("FriendlyName", Keys.A.ToString()),
               new XElement("KeyEvent", KeyboardEvents.WM_KeyDown.ToString())))
            };
            target.AddKeyboradActionNodesToActionList(datas, re);
            Assert.AreEqual(datas.Count, 1);
        }

        /// <summary>
        ///A test for GetDatas
        ///</summary>
        [TestMethod()]
        public void GetDatasTest()
        {
            KBActionRecorderLinq target = new KBActionRecorderLinq();
            target.WriteData(KeyboardEvents.WM_KeyDown.ToString(), Keys.A, 10);
            target.SaveActions();
            Win32API.KeyEvent[] expected = new Win32API.KeyEvent[]
            {
                new Win32API.KeyEvent()
                {
                    dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown,
                    bVk = Convert.ToInt32(Keys.A),
                    delayTime = 10
                }
            }; 
            Win32API.KeyEvent[] actual;
            actual = target.GetDatas();
            Assert.AreEqual(expected[0], actual[0]);
            
        }

        /// <summary>
        ///A test for GetKeyboradActionNodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void GetKeyboradActionNodesTest()
        {
            KBActionRecorderLinq_Accessor target = new KBActionRecorderLinq_Accessor();
            target.WriteData(KeyboardEvents.WM_KeyDown.ToString(), Keys.A, 10);
            target.SaveActions();
           
            IEnumerable<XElement> actual;
            actual = target.GetKeyboradActionNodes();
            Assert.IsNotNull(actual);                       
        }

        /// <summary>
        ///A test for JudgeKeyevent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void JudgeKeyeventTest()
        {
            KBActionRecorderLinq_Accessor target = new KBActionRecorderLinq_Accessor();
            Win32API.KeyEvent keyEvent = new Win32API.KeyEvent() 
            {
                dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown
            };
            Win32API.KeyEvent keyEventExpected = new Win32API.KeyEvent()
            {
                dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown
            };
            string eventName = KeyboardEvents.WM_KeyDown.ToString(); 
            target.JudgeKeyevent(ref keyEvent, eventName);
            
            Assert.AreEqual(keyEventExpected, keyEvent);
           
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        public void WriteDataTest()
        {
            KBActionRecorderLinq target = new KBActionRecorderLinq(); 
            string keyEvents = KeyboardEvents.WM_KeyDown.ToString(); 
            Keys myKey = Keys.A;
            int delayTime = 10; 
            target.WriteData(keyEvents, myKey, delayTime);
            target.SaveActions();
            Win32API.KeyEvent Expected = new Win32API.KeyEvent()
            {
                dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown,
                bVk = Convert.ToInt32(Keys.A),
                delayTime = 10
            };
            var actual = target.GetDatas()[0];
            
            Assert.AreEqual(Expected,actual);
            
           
            
        }
    }
}

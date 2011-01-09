using Miku.Client.Models.Recorders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Miku.Client;
using System.Windows.Forms;
using Miku.Client.Models.Hooks;

namespace Miku.Client.Test
{
    
    
    /// <summary>
    ///This is a test class for KBMActionRecorderLinqTest and is intended
    ///to contain all KBMActionRecorderLinqTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KBMActionRecorderLinqTest
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
        ///A test for KBMActionRecorderLinq Constructor
        ///</summary>
        [TestMethod()]
        public void KBMActionRecorderLinqConstructorTest()
        {
            string actionsListFileName = "actionsListFileName"; // TODO: Initialize to an appropriate value
            KBMActionRecorderLinq target = new KBMActionRecorderLinq(actionsListFileName);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for KBMActionRecorderLinq Constructor
        ///</summary>
        [TestMethod()]
        public void KBMActionRecorderLinqConstructorTest1()
        {
            KBMActionRecorderLinq target = new KBMActionRecorderLinq();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for AddActionNodesToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddActionNodesToActionListTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            List<object> datas = new List<object>();
            IEnumerable<XElement> re = new List<XElement>()
            {
                new XElement(
                "Action", new XAttribute("Type", "MouseAct"),
                new XAttribute("DelayTime", 10),
             new XElement("Position", new Win32API.POINT(0, 0)),
             new XElement("MouseEvent", "WM_LBUTTONDOWN")),
              new XElement(new XElement(
                "Action", new XAttribute("Type", "KeyboardAct"),
                new XAttribute("DelayTime", 10.ToString()),
               new XElement("KeyData", ((int)Keys.A).ToString()), new XAttribute("FriendlyName", Keys.A.ToString()),
               new XElement("KeyEvent", KeyboardEvents.WM_KeyDown.ToString())))
            };
            target.AddActionNodesToActionList(datas, re);
            Assert.AreEqual(datas.Count, 2);
        }

        /// <summary>
        ///A test for AddKeyboardActionToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddKeyboardActionToActionListTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            List<object> datas = new List<object>();     
            XElement item = new XElement(
                "Action", new XAttribute("Type", "KeyboardAct"), 
                new XAttribute("DelayTime", 10),
              new XElement("KeyData", (int)Keys.A),
              new XElement("KeyEvent", KeyboardEvents.WM_KeyDown.ToString()));
            target.AddKeyboardActionToActionList(datas, item);
            Assert.IsNotNull(datas);
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
            List<Win32API.KeyEvent> datas =new List<Win32API.KeyEvent>();
            IEnumerable<XElement> re = new List<XElement>() 
            {
                new XElement(new XElement(
                "Action", new XAttribute("Type", "KeyboardAct"),
                new XAttribute("DelayTime", 10.ToString()),
               new XElement("KeyData", ((int)Keys.A).ToString()), new XAttribute("FriendlyName", Keys.A.ToString()),
               new XElement("KeyEvent", KeyboardEvents.WM_KeyDown.ToString())))
            };
            target.AddKeyboradActionNodesToActionList(datas, re);
            Assert.AreEqual(datas.Count,1);
        }

        /// <summary>
        ///A test for AddMouseActionNodesToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddMouseActionNodesToActionListTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            List<Win32API.MouseEvent> datas = new List<Win32API.MouseEvent>();
            IEnumerable<XElement> re = new List<XElement>() 
            {
                new XElement(
                "Action", new XAttribute("Type", "MouseAct"),
                new XAttribute("DelayTime", 10),
             new XElement("Position", new Win32API.POINT(0, 0)),
             new XElement("MouseEvent", "WM_LBUTTONDOWN"))
            };
            target.AddMouseActionNodesToActionList(datas, re);
            Assert.AreEqual(datas.Count, 1);
        }

        /// <summary>
        ///A test for AddMouseActionToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddMouseActionToActionListTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            List<object> datas =new List<object>();      
            XElement item = new XElement(
                "Action", new XAttribute("Type", "MouseAct"),
                new XAttribute("DelayTime", 10),
             new XElement("Position", new Win32API.POINT(0, 0)),
             new XElement("MouseEvent", "WM_LBUTTONDOWN"));
            target.AddMouseActionToActionList(datas, item);
            Assert.IsNotNull(datas);
        }

        /// <summary>
        ///A test for GetActionNodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void GetActionNodesTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            target.WriteData(KeyboardEvents.WM_KeyDown.ToString(), Keys.A, 10);
            target.SaveActions();            
            IEnumerable<XElement> actual;
            actual = target.GetActionNodes();
            Assert.IsNotNull( actual);           
        }

        /// <summary>
        ///A test for GetDatas
        ///</summary>
        [TestMethod()]
        public void GetDatasTest()
        {
            KBMActionRecorderLinq target = new KBMActionRecorderLinq();
            target.WriteData("WM_LBUTTONDOWN", new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) }, 10);
            target.SaveActions();
            target.WriteData(KeyboardEvents.WM_KeyDown.ToString(), Keys.A, 10);
            target.SaveActions();
            object[] expected = new object[] 
            { 
                new Win32API.MouseEvent() 
                {
                    delayTime = 10, 
                    dwFlags = Miku.Client.Win32API.MouseEventFlag.LeftDown ,
                    pt=new Win32API.POINT(0, 0)
                },
                new Win32API.KeyEvent()
                {
                    dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown,
                    bVk = Convert.ToInt32(Keys.A),
                    delayTime = 10
                }
            };            
            object[] actual;
            actual = target.GetDatas();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }

        /// <summary>
        ///A test for GetKeyboardActionDatas
        ///</summary>
        [TestMethod()]
        public void GetKeyboardActionDatasTest()
        {
            KBMActionRecorderLinq target = new KBMActionRecorderLinq();
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
            actual = target.GetKeyboardActionDatas();

            Assert.AreEqual(expected[0], actual[0]);
        }

        /// <summary>
        ///A test for GetKeyboradActionNodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void GetKeyboradActionNodesTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();            
            target.WriteData(KeyboardEvents.WM_KeyDown.ToString(), Keys.A, 10);
            target.SaveActions();
            IEnumerable<XElement> expected = new List<XElement>()
            {
                new XElement(
                "Action", new XAttribute("Type", "KeyboardAct"),
                new XAttribute("DelayTime", 10.ToString()),
               new XElement("KeyData", ((int)Keys.A).ToString()), new XAttribute("FriendlyName", Keys.A.ToString()),
               new XElement("KeyEvent", KeyboardEvents.WM_KeyDown.ToString()))
            };
            IEnumerable<XElement> actual;
            actual = target.GetKeyboradActionNodes();
            Assert.IsNotNull(actual);
           
        }

        /// <summary>
        ///A test for GetMouseActionDatas
        ///</summary>
        [TestMethod()]
        public void GetMouseActionDatasTest()
        {
            KBMActionRecorderLinq target = new KBMActionRecorderLinq();
            target.WriteData("WM_LBUTTONDOWN", new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) }, 10);
            target.SaveActions();
            Win32API.MouseEvent[] expected = new Win32API.MouseEvent[]
            {
                 new Win32API.MouseEvent() 
                {
                    delayTime = 10, 
                    dwFlags = Miku.Client.Win32API.MouseEventFlag.LeftDown ,
                    pt=new Win32API.POINT(0, 0)
                }
            };
            Win32API.MouseEvent[] actual;
            actual = target.GetMouseActionDatas();
            Assert.AreEqual(expected[0], actual[0]);
        }

        /// <summary>
        ///A test for GetMouseActionNodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void GetMouseActionNodesTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            target.WriteData("WM_LBUTTONDOWN", new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) }, 10);
            target.SaveActions();
            IEnumerable<XElement> actual;
            actual = target.GetMouseActionNodes();
            Assert.IsNotNull(actual);
            //Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for JudgeKeyevent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void JudgeKeyeventTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            Win32API.KeyEvent keyEvent = new Win32API.KeyEvent()
            {
                dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyUp,
                bVk = Convert.ToInt32(Keys.A),
                delayTime = 10
            };
            Win32API.KeyEvent keyEventExpected = new Win32API.KeyEvent()
            {
                dwFlags = Miku.Client.Win32API_Accessor.KBEventFlag.KeyDown,
                bVk = Convert.ToInt32(Keys.A),
                delayTime = 10,    
            };

            string eventName = KeyboardEvents.WM_KeyDown.ToString(); 
            target.JudgeKeyevent(ref keyEvent, eventName); 
            Assert.AreEqual(keyEventExpected, keyEvent);
        }

        
        /// <summary>
        ///A test for JudgeMouseevent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void JudgeMouseeventTest()
        {
            KBMActionRecorderLinq_Accessor target = new KBMActionRecorderLinq_Accessor();
            XElement item = new XElement(
                "Action", new XAttribute("Type", "MouseAct"),
                new XAttribute("DelayTime", 10),
             new XElement("Position", new Win32API.POINT(0, 0)),
             new XElement("MouseEvent", "WM_LBUTTONDOWN"));
            Win32API.MouseEvent mouseEvent = new Win32API.MouseEvent()
            {
                delayTime = 10,
                dwFlags = Miku.Client.Win32API.MouseEventFlag.LeftDown,
                pt = new Win32API.POINT(0, 0)
            };
            Win32API.MouseEvent mouseEventExpected = new Win32API.MouseEvent()
            {
                delayTime = 10,
                
                pt = new Win32API.POINT(0, 0)
            };
            target.JudgeMouseevent(item, ref mouseEventExpected);
            
            Assert.AreEqual(mouseEventExpected, mouseEvent);
           
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        public void WriteDataTest()
        {
            KBMActionRecorderLinq target = new KBMActionRecorderLinq(); 
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
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        public void WriteDataTest1()
        {
            MActionRecorderLinq target = new MActionRecorderLinq();
            string mouseEvents = "WM_LBUTTONDOWN";
            MSLLHOOKSTRUCT myMouse = new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) };
            int delayTime = 10;
            target.WriteData(mouseEvents, myMouse, delayTime);
            target.SaveActions();
            Win32API.MouseEvent Expected = new Win32API.MouseEvent()
            {
                dwFlags = Win32API.MouseEventFlag.LeftDown,
                delayTime = 10,
                pt = new Win32API.POINT(0, 0)
            };
            var actual = target.GetDatas()[0];
            Assert.AreEqual(Expected, actual);
        }
               
    }
}


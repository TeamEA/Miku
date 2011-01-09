using Miku.Client.Models.Recorders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Miku.Client;
using System.Collections.Generic;
using System.Xml.Linq;
using Miku.Client.Models.Hooks;

namespace Miku.Client.Test
{
    
    
    /// <summary>
    ///This is a test class for MActionRecorderLinqTest and is intended
    ///to contain all MActionRecorderLinqTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MActionRecorderLinqTest
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
        ///A test for MActionRecorderLinq Constructor
        ///</summary>
        [TestMethod()]
        public void MActionRecorderLinqConstructorTest()
        {
            string actionsListFileName = "actionsListFileName"; 
            MActionRecorderLinq target = new MActionRecorderLinq(actionsListFileName);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for MActionRecorderLinq Constructor
        ///</summary>
        [TestMethod()]
        public void MActionRecorderLinqConstructorTest1()
        {
            MActionRecorderLinq target = new MActionRecorderLinq();
            Assert.IsNotNull(target);
            //Assert.IsNull(target);
        }

        /// <summary>
        ///A test for AddMouseActionNodesToActionList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void AddMouseActionNodesToActionListTest()
        {
            MActionRecorderLinq_Accessor target = new MActionRecorderLinq_Accessor();            
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
        ///A test for GetDatas
        ///</summary>
        [TestMethod()]
        public void GetDatasTest()
        {
            MActionRecorderLinq target = new MActionRecorderLinq();
            target.WriteData("WM_LBUTTONDOWN", new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) }, 10);
            target.SaveActions();
            Win32API.MouseEvent[] expected = new Win32API.MouseEvent[]
            {
                new Win32API.MouseEvent()
                { 
                    dwFlags=Win32API.MouseEventFlag.LeftDown,
                    pt=new Win32API.POINT(0, 0),
                    delayTime=10
                }
                
            };
            Win32API.MouseEvent[] actual;
            actual = target.GetDatas();
            
            Assert.AreEqual(expected[0], actual[0]);
            
        }

        /// <summary>
        ///A test for GetMouseActionNodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void GetMouseActionNodesTest()
        {
            MActionRecorderLinq_Accessor target = new MActionRecorderLinq_Accessor();
            target.WriteData("WM_LBUTTONDOWN", new MSLLHOOKSTRUCT() { pt = new Win32API.POINT(0, 0) }, 10);
            target.SaveActions();            
            IEnumerable<XElement> actual;
            actual = target.GetMouseActionNodes();            
            Assert.IsNotNull(actual);
          
        }

        /// <summary>
        ///A test for JudgeMouseevent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Miku.Client.exe")]
        public void JudgeMouseeventTest()
        {
            MActionRecorderLinq_Accessor target = new MActionRecorderLinq_Accessor();            
            Win32API.MouseEvent mouseEvent = new Win32API.MouseEvent()
            {
                dwFlags=Win32API.MouseEventFlag.LeftDown
            };
            XElement item = new XElement(
                "Action", new XAttribute("Type", "MouseAct"),
                new XAttribute("DelayTime", 10),
              new XElement("Position", null),
              new XElement("MouseEvent", mouseEvent));
            Win32API.MouseEvent mouseEventExpected = new Win32API.MouseEvent() 
            { 
                dwFlags = Win32API.MouseEventFlag.LeftDown 
            };  
            target.JudgeMouseevent(item, ref mouseEvent);
            
            Assert.AreEqual(mouseEventExpected, mouseEvent);           
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        public void WriteDataTest()
        {
            MActionRecorderLinq target = new MActionRecorderLinq(); 
            string mouseEvents = "WM_LBUTTONDOWN";
            MSLLHOOKSTRUCT myMouse = new MSLLHOOKSTRUCT() { pt=new Win32API.POINT(0,0)};
            int delayTime = 10; 
            target.WriteData(mouseEvents, myMouse, delayTime);           
            target.SaveActions();
            Win32API.MouseEvent Expected =  new Win32API.MouseEvent()
            {
                dwFlags = Win32API.MouseEventFlag.LeftDown,               
                delayTime = 10,
                pt=new Win32API.POINT(0,0)
            };
            var actual = target.GetDatas()[0];
            
            Assert.AreEqual(Expected, actual);
        }
    }
}

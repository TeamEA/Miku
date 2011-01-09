using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Miku.Client.Models.Hooks;
using Miku.Client;

namespace Miku.Client.Models.Recorders
{
    public class KBActionRecorderLinq : ActionRecorderLinq
    {
        public KBActionRecorderLinq()
            : base()
        {
        }

        public KBActionRecorderLinq(string actionsListFileName)
            : base(actionsListFileName)
        {
        }
        /// <summary>
        /// Writes the action data.
        /// </summary>
        /// <param name="keyEvents">The key events.</param>
        /// <param name="myKey">My key.</param>
        /// <param name="delayTime">The delay time.</param>
        public void WriteData(string keyEvents, System.Windows.Forms.Keys myKey, int delayTime)
        {
            XElement newAction = new XElement("Action", new XAttribute("Type", "KeyboardAct"), new XAttribute("DelayTime", delayTime.ToString()),
               new XElement("KeyData", ((int)myKey).ToString(), new XAttribute("FriendlyName", myKey.ToString())),
               new XElement("KeyEvent", keyEvents));
            xRoot.Add(newAction);
        }

        #region Get Keyboard Action Datas
        /// <summary>
        /// Gets the action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.KeyEvent[] GetDatas()
        {

            List<Win32API.KeyEvent> datas = new List<Win32API.KeyEvent>();

            var re = GetKeyboradActionNodes();

            AddKeyboradActionNodesToActionList(datas, re);

            return datas.ToArray();

        }

        private IEnumerable<XElement> GetKeyboradActionNodes()
        {
            xRoot = XElement.Load(actionsListFileName);

            var re = from act in xRoot.Elements("Action")
                     where act.Attribute("Type").Value.ToString() == "KeyboardAct"
                     select act;
            return re;
        }

        private  void AddKeyboradActionNodesToActionList(List<Win32API.KeyEvent> datas, IEnumerable<XElement> re)
        {
            foreach (var item in re)
            {
                Win32API.KeyEvent keyEvent = new Win32API.KeyEvent();
                keyEvent.delayTime = Convert.ToInt32(item.Attribute("DelayTime").Value);

                keyEvent.bVk = Convert.ToInt32(item.Element("KeyData").Value);

                string eventName = (string)(item.Element("KeyEvent").Value);

                JudgeKeyevent(ref keyEvent, eventName);

                datas.Add(keyEvent);
            }
        }

        private  void JudgeKeyevent(ref Win32API.KeyEvent keyEvent, string eventName)
        {
            if (eventName == KeyboardEvents.WM_KeyUp.ToString())
            {
                keyEvent.dwFlags = Win32API.KBEventFlag.KeyUp;
            }
            else if (eventName == KeyboardEvents.WM_KeyDown.ToString())
            {
                keyEvent.dwFlags = Win32API.KBEventFlag.KeyDown;
            }
            else
            {
                throw new InvalidOperationException("KeyEvent初始化失败");
            }
        }

        #endregion
    }

}

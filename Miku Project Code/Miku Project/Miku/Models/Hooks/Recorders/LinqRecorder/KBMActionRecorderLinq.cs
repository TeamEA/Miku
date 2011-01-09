using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Miku.Client.Models.Hooks;
using Miku.Client;

namespace Miku.Client.Models.Recorders
{
    public class KBMActionRecorderLinq : ActionRecorderLinq
    {
        public KBMActionRecorderLinq()
            : base(){}

        public KBMActionRecorderLinq(string actionsListFileName)
            : base(actionsListFileName){}

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

        /// <summary>
        /// Writes the action data.
        /// </summary>
        /// <param name="mouseEvents">The mouse events.</param>
        /// <param name="myMouse">My mouse.</param>
        /// <param name="delayTime">The delay time.</param>
        public void WriteData(string mouseEvents, MSLLHOOKSTRUCT myMouse, int delayTime)
        {
            XElement newAction = new XElement("Action", new XAttribute("Type", "MouseAct"), new XAttribute("DelayTime", delayTime.ToString()),
             new XElement("Position", myMouse.pt.ToString()),
             new XElement("MouseEvent", mouseEvents));
            xRoot.Add(newAction);
        }

        /// <summary>
        /// Gets the recorded action datas.
        /// </summary>
        /// <returns></returns>
        public object[] GetDatas()
        {
            List<object> datas = new List<object>();

            var re = GetActionNodes();

            AddActionNodesToActionList(datas, re);

            return datas.ToArray();

        }

        private IEnumerable<XElement> GetActionNodes()
        {
            xRoot = XElement.Load(actionsListFileName);

            var re = from act in xRoot.Elements("Action")
                     select act;
            return re;
        }

        private  void AddActionNodesToActionList(List<object> datas, IEnumerable<XElement> re)
        {
            foreach (var item in re)
            {
                if (item.Attribute("Type").Value.ToString() == "KeyboardAct")
                {
                    AddKeyboardActionToActionList(datas, item);
                }
                else if (item.Attribute("Type").Value.ToString() == "MouseAct")
                {

                    AddMouseActionToActionList(datas, item);
                }

            }            
        }

        private  void AddMouseActionToActionList(List<object> datas, XElement item)
        {
            Win32API.MouseEvent mouseEvent = new Win32API.MouseEvent();
            mouseEvent.delayTime = Convert.ToInt32(item.Attribute("DelayTime").Value);

            string pointInfo = item.Element("Position").Value;
            pointInfo = pointInfo.Substring(1, pointInfo.Length - 2);
            string[] xy = (pointInfo.Split(','));
            mouseEvent.pt.x = Convert.ToInt32(xy[0]);
            mouseEvent.pt.y = Convert.ToInt32(xy[1]);

            JudgeMouseevent(item, ref mouseEvent);

            datas.Add(mouseEvent);
        }

        private  void AddKeyboardActionToActionList(List<object> datas, XElement item)
        {
            Win32API.KeyEvent keyEvent = new Win32API.KeyEvent();
            keyEvent.delayTime = Convert.ToInt32(item.Attribute("DelayTime").Value);

            keyEvent.bVk = Convert.ToInt32(item.Element("KeyData").Value);

            string eventName = (string)(item.Element("KeyEvent").Value);
            if (eventName == KeyboardEvents.WM_KeyUp.ToString())
            {
                keyEvent.dwFlags = Win32API.KBEventFlag.KeyUp;
            }
            else if (eventName == KeyboardEvents.WM_KeyDown.ToString())
            {
                keyEvent.dwFlags = Win32API.KBEventFlag.KeyDown;
            }
            datas.Add(keyEvent);
        }

        

        #region Get Mouse Action Datas

        /// <summary>
        /// Gets the recorded action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.MouseEvent[] GetMouseActionDatas()
        {
            List<Win32API.MouseEvent> datas = new List<Win32API.MouseEvent>();

            var re = GetMouseActionNodes();

            AddMouseActionNodesToActionList(datas, re);

            return datas.ToArray();
        }

        private IEnumerable<XElement> GetMouseActionNodes()
        {
            xRoot = XElement.Load(this.actionsListFileName);

            var re = from act in xRoot.Elements("Action")
                     where act.Attribute("Type").Value.ToString() == "MouseAct"
                     select act;
            return re;
        }

        private  void AddMouseActionNodesToActionList(List<Win32API.MouseEvent> datas, IEnumerable<XElement> re)
        {
            foreach (var item in re)
            {
                Win32API.MouseEvent mouseEvent = new Win32API.MouseEvent();
                mouseEvent.delayTime = Convert.ToInt32(item.Attribute("DelayTime").Value);

                string pointInfo = item.Element("Position").Value;
                pointInfo = pointInfo.Substring(1, pointInfo.Length - 2);
                string[] xy = (pointInfo.Split(','));
                mouseEvent.pt.x = Convert.ToInt32(xy[0]);
                mouseEvent.pt.y = Convert.ToInt32(xy[1]);

                JudgeMouseevent(item, ref mouseEvent);

                datas.Add(mouseEvent);
            }
        }

        private  void JudgeMouseevent(XElement item, ref Win32API.MouseEvent mouseEvent)
        {
            string eventName = (string)(item.Element("MouseEvent").Value);
            switch (eventName)
            {
                case "WM_MOUSEMOVE":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.Move;
                    break;
                case "WM_LBUTTONDOWN":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.LeftDown;
                    break;
                case "WM_LBUTTONUP":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.LeftUp;
                    break;
                case "WM_LBUTTONDBLCLK":
                    //MouseEvent.dwFlags  = Win32API.MouseEventFlag
                    break;
                case "WM_RBUTTONDOWN":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.RightDown;
                    break;
                case "WM_RBUTTONUP":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.RightUp;
                    break;
                case "WM_RBUTTONDBLCLK":
                    break;
                case "WM_MBUTTONDOWN":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.MiddleDown;
                    break;
                case "WM_MBUTTONUP":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.MiddleUp;
                    break;
                case "WM_MBUTTONDBLCLK":
                    break;
                case "WM_MOUSEWHEEL":
                    mouseEvent.dwFlags = Win32API.MouseEventFlag.Wheel;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Get Keyboard Action Datas
        /// <summary>
        /// Gets the Keyboard action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.KeyEvent[] GetKeyboardActionDatas()
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
                //throw new Exception();
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Client.Models.Hooks;
using System.Xml.Linq;
using Miku.Client;

namespace Miku.Client.Models.Recorders
{
    public class MActionRecorderLinq : ActionRecorderLinq
    {
        public MActionRecorderLinq()
            : base()
        {
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

        #region Get Mouse Action Datas

        /// <summary>
        /// Gets the recorded action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.MouseEvent[] GetDatas()
        {
            List<Win32API.MouseEvent> datas = new List<Win32API.MouseEvent>();

            var re = GetMouseActionNodes();

            AddMouseActionNodesToActionList(datas, re);

            return datas.ToArray();
        }

        private IEnumerable<XElement> GetMouseActionNodes()
        {
            xRoot = XElement.Load(this.actionsListTmpFileName);

            var re = from act in xRoot.Elements("Action")
                     where act.Attribute("Type").Value.ToString() == "MouseAct"
                     select act;
            return re;
        }

        private static void AddMouseActionNodesToActionList(List<Win32API.MouseEvent> datas, IEnumerable<XElement> re)
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

        private static void JudgeMouseevent(XElement item, ref Win32API.MouseEvent mouseEvent)
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

    }
}

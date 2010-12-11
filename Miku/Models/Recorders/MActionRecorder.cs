using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Miku.Client.Models.Hooks;

namespace Miku.Client.Models.Recorders
{
    public class MActionRecorder : ActionRecorder
    {
        public MActionRecorder()
            :base()
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
            XmlElement newAction = actionsXmlDoc.CreateElement("Action");
            XmlAttribute newActionAttris = actionsXmlDoc.CreateAttribute("Type");
            newActionAttris.InnerText = ActionTypes.MouseAct.ToString();
            newAction.SetAttributeNode(newActionAttris);
            newActionAttris = actionsXmlDoc.CreateAttribute("DelayTime");
            newActionAttris.InnerText = delayTime.ToString();
            newAction.SetAttributeNode(newActionAttris);

            XmlElement newKeyData = actionsXmlDoc.CreateElement("Position");
            newKeyData.InnerText = myMouse.pt.ToString();
            newAction.AppendChild(newKeyData);

            XmlElement newkeyEvent = actionsXmlDoc.CreateElement("MouseEvent");
            newkeyEvent.InnerText = mouseEvents;
            newAction.AppendChild(newkeyEvent);

            actionsXmlDoc.DocumentElement.AppendChild(newAction);
        }

        /// <summary>
        /// Gets the recorded action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.MouseEvent[] GetDatas()
        {
            List<Win32API.MouseEvent> datas = new List<Win32API.MouseEvent>();
            XmlReader reader = XmlReader.Create(this.actionsListTmpFileName);
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Action")
                {
                    //效率，会初始化两次
                    if (reader.HasAttributes)
                    {
                        Win32API.MouseEvent mouseEvent = new Win32API.MouseEvent();
                        mouseEvent.delayTime = Convert.ToInt32(reader.GetAttribute("DelayTime"));

                        while (reader.Read())
                        {
                            //throgh if statement never be true
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Position")
                            {
                                string pointInfo = reader.ReadInnerXml();
                                pointInfo = pointInfo.Substring(1, pointInfo.Length - 2);
                                string[] xy = (pointInfo.Split(','));
                                mouseEvent.pt.x = Convert.ToInt32(xy[0]);
                                mouseEvent.pt.y = Convert.ToInt32(xy[1]);

                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "MouseEvent")
                                {
                                    string eventName = reader.ReadInnerXml();
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
                                    break;
                                }
                                else
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MouseEvent")
                                        {
                                            string eventName = reader.ReadInnerXml();
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
                                            break;
                                        }

                                    }
                                    break;
                                }

                            }

                        }

                        datas.Add(mouseEvent);
                    }

                }

            }
            reader.Close();

            return datas.ToArray();
        }
    }


}

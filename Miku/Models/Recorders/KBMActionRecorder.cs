using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Client.Models.Hooks;
using System.Xml;

namespace Miku.Client.Models.Recorders
{
    public class KBMActionRecorder : ActionRecorder
    {

        public KBMActionRecorder()
            :base()
        {

        }

        /// <summary>
        /// Writes the action data.
        /// </summary>
        /// <param name="keyEvents">The key events.</param>
        /// <param name="myKey">My key.</param>
        /// <param name="delayTime">The delay time.</param>
        public void WriteData(string keyEvents, System.Windows.Forms.Keys myKey,int delayTime)
        {
            XmlElement newAction = actionsXmlDoc.CreateElement("Action");
            XmlAttribute newActionAttris = actionsXmlDoc.CreateAttribute("Type");
            newActionAttris.InnerText = ActionTypes.KeyboardAct.ToString();
            newAction.SetAttributeNode(newActionAttris);
            newActionAttris = actionsXmlDoc.CreateAttribute("DelayTime");
            newActionAttris.InnerText = delayTime.ToString();
            newAction.SetAttributeNode(newActionAttris);

            XmlElement newKeyData = actionsXmlDoc.CreateElement("KeyData");
            newKeyData.InnerText = ((int)myKey).ToString();
            XmlAttribute newKeyDataAttris = actionsXmlDoc.CreateAttribute("FriendlyName");
            newKeyDataAttris.InnerText = myKey.ToString();
            newKeyData.SetAttributeNode(newKeyDataAttris);
            newAction.AppendChild(newKeyData);

            XmlElement newkeyEvent = actionsXmlDoc.CreateElement("KeyEvent");
            newkeyEvent.InnerText = keyEvents;
            newAction.AppendChild(newkeyEvent);

            actionsXmlDoc.DocumentElement.AppendChild(newAction);
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
        public object[] GetDatas()
        {
            List<object> datas = new List<object>();
            XmlReader reader = XmlReader.Create(this.actionsListFileName);
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Action")
                {
                    //效率，会初始化两次
                    if (reader.HasAttributes)
                    {
                        string type = reader.GetAttribute("Type");
                        int delayTime = Convert.ToInt32(reader.GetAttribute("DelayTime"));

                        if (type == "KeyboardAct")
                        {
                            Win32API.KeyEvent keyEvent = new Win32API.KeyEvent();
                            keyEvent.delayTime = delayTime;

                            while (reader.Read())
                            {
                                //throgh if statement never be true
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "KeyData")
                                {

                                    keyEvent.bVk = Convert.ToInt32(reader.ReadInnerXml());

                                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "KeyEvent")
                                    {
                                        string eventName = reader.ReadInnerXml();
                                        if (eventName == KeyboardEvents.WM_KeyUp.ToString())
                                        {
                                            keyEvent.dwFlags = Win32API.KBEventFlag.KeyUp;
                                        }
                                        else if (eventName == KeyboardEvents.WM_KeyDown.ToString())
                                        {
                                            keyEvent.dwFlags = Win32API.KBEventFlag.KeyDown;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        keyEvent = JudgeKeyevent(reader, keyEvent);
                                        break;
                                    }

                                }

                            }

                            datas.Add(keyEvent);
                        }
                        else if (type == "MouseAct")
                        {
                            Win32API.MouseEvent MouseEvent = new Win32API.MouseEvent();
                            MouseEvent.delayTime = delayTime;

                            while (reader.Read())
                            {
                                //throgh if statement never be true
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Position")
                                {
                                    string pointInfo = reader.ReadInnerXml();
                                    pointInfo = pointInfo.Substring(1, pointInfo.Length - 2);
                                    string[] xy = (pointInfo.Split(','));
                                    MouseEvent.pt.x = Convert.ToInt32(xy[0]);
                                    MouseEvent.pt.y = Convert.ToInt32(xy[1]);

                                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "MouseEvent")
                                    {
                                        string eventName = reader.ReadInnerXml();
                                        MouseEvent = JudgeMouseEvent(MouseEvent, eventName);
                                        break;
                                    }
                                    else
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MouseEvent")
                                            {
                                                string eventName = reader.ReadInnerXml();
                                                MouseEvent = JudgeMouseEvent(MouseEvent, eventName);
                                                break;
                                            }
                                        }
                                        break;
                                    }

                                }

                            }

                            datas.Add(MouseEvent);
                        }

                    }

                }

            }
            reader.Close();
            return datas.ToArray();
        
        }

        private Win32API.MouseEvent JudgeMouseEvent(Win32API.MouseEvent MouseEvent, string eventName)
        {
            switch (eventName)
            {
                case "WM_MOUSEMOVE":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.Move;
                    break;
                case "WM_LBUTTONDOWN":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.LeftDown;
                    break;
                case "WM_LBUTTONUP":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.LeftUp;
                    break;
                case "WM_LBUTTONDBLCLK":
                    //MouseEvent.dwFlags  = Win32API.MouseEventFlag
                    break;
                case "WM_RBUTTONDOWN":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.RightDown;
                    break;
                case "WM_RBUTTONUP":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.RightUp;
                    break;
                case "WM_RBUTTONDBLCLK":
                    break;
                case "WM_MBUTTONDOWN":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.MiddleDown;
                    break;
                case "WM_MBUTTONUP":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.MiddleUp;
                    break;
                case "WM_MBUTTONDBLCLK":
                    break;
                case "WM_MOUSEWHEEL":
                    MouseEvent.dwFlags = Win32API.MouseEventFlag.Wheel;
                    break;
                default:
                    break;
            }
            return MouseEvent;
        }

        private Win32API.KeyEvent JudgeKeyevent(XmlReader reader, Win32API.KeyEvent keyEvent)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "KeyEvent")
                {
                    string eventName = reader.ReadInnerXml();
                    if (eventName == KeyboardEvents.WM_KeyUp.ToString())
                    {
                        keyEvent.dwFlags = Win32API.KBEventFlag.KeyUp;
                    }
                    else if (eventName == KeyboardEvents.WM_KeyDown.ToString())
                    {
                        keyEvent.dwFlags = Win32API.KBEventFlag.KeyDown;
                    }
                    break;
                }

            }
            return keyEvent;
        }
    }
}

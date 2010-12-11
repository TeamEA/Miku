using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Xml;
using Miku.Client.Models.Hooks;

enum ActionTypes { KeyboardAct, MouseAct }

namespace Miku.Client.Models.Recorders
{
    public class KBActionRecorder:ActionRecorder
    {
        public KBActionRecorder()
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
        /// Gets the action datas.
        /// </summary>
        /// <returns></returns>
        public Win32API.KeyEvent[] GetDatas()
        {
            List<Win32API.KeyEvent> datas = new List<Win32API.KeyEvent>();
            XmlReader reader = XmlReader.Create(this.actionsListTmpFileName);
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Action")
                {
                    //效率，会初始化两次
                    if (reader.HasAttributes)
                    {
                        Win32API.KeyEvent keyEvent = new Win32API.KeyEvent();
                        keyEvent.delayTime = Convert.ToInt32(reader.GetAttribute("DelayTime"));

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
                                    break;
                                }

                            }

                        }

                        datas.Add(keyEvent);
                    }
                  
                }
                
            }
            reader.Close();

            return datas.ToArray();
        }
    }
}

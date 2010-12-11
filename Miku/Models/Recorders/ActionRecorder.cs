using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Miku.Client.Models.Recorders
{
    /// <summary>
    /// The recorder than records the actions
    /// </summary>
    public abstract class ActionRecorder
    {
        protected XmlWriterSettings xmlWritterSettings;
        protected XmlDocument actionsXmlDoc;
        protected string actionsListTmpFileName;

        public ActionRecorder()
        {
            actionsListTmpFileName = "actionlist.tmp";

            xmlWritterSettings = new XmlWriterSettings();
            xmlWritterSettings.Indent = true;
            xmlWritterSettings.Encoding = Encoding.UTF8;
            XmlWriter actionsDocWriter = XmlWriter.Create(actionsListTmpFileName, xmlWritterSettings);
            actionsDocWriter.WriteStartDocument();
            actionsDocWriter.WriteStartElement("Actions");
            actionsDocWriter.WriteEndElement();
            actionsDocWriter.WriteEndDocument();
            actionsDocWriter.Flush();
            actionsDocWriter.Close();

            actionsXmlDoc = new XmlDocument();
            actionsXmlDoc.Load(actionsListTmpFileName);
        }

        /// <summary>
        /// Saves the recorded actions.
        /// </summary>
        public void SaveActions()
        {
            actionsXmlDoc.Save(actionsListTmpFileName);
        }
    }
}

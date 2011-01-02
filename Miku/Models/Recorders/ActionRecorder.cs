using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Miku.Client.Models.Recorders
{
    /// <summary>
    /// The recorder than records the actions
    /// </summary>
    public abstract class ActionRecorder:IActionRecorder
    {
        protected XmlWriterSettings xmlWritterSettings;
        protected XmlDocument actionsXmlDoc;
        protected string actionsListFileName;
        protected const string actionsListTmpFileName = "actionlist.tmp";

        public ActionRecorder()
        {
            actionsListFileName = actionsListTmpFileName;

            xmlWritterSettings = new XmlWriterSettings();
            xmlWritterSettings.Indent = true;
            xmlWritterSettings.Encoding = Encoding.UTF8;
            XmlWriter actionsDocWriter = XmlWriter.Create(actionsListFileName, xmlWritterSettings);
            actionsDocWriter.WriteStartDocument();
            actionsDocWriter.WriteStartElement("Actions");
            actionsDocWriter.WriteEndElement();
            actionsDocWriter.WriteEndDocument();
            actionsDocWriter.Flush();
            actionsDocWriter.Close();

            actionsXmlDoc = new XmlDocument();
            actionsXmlDoc.Load(actionsListFileName);
        }

        #region IActionRecorder
        /// <summary>
        /// Saves the recorded actions.
        /// </summary>
        public void SaveActions()
        {
            actionsXmlDoc.Save(actionsListFileName);
        }


        public void SaveRecordedFileAs(string newFilePath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

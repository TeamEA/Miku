using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Miku.Client.Models.Recorders
{
    public abstract class ActionRecorderLinq : IActionRecorder
    {
        protected XDocument xDoc;
        protected XElement xRoot;
        protected string actionsListFileName;
        protected const string actionsListTmpFileName = "actionlist.tmp";

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRecorderLinq"/> class.
        /// </summary>
        public ActionRecorderLinq()
        {
            actionsListFileName = actionsListTmpFileName;

            InitializeRecordedFile();
        }

        public ActionRecorderLinq(string actionsListFileName)
        {
            this.actionsListFileName = actionsListFileName;

            InitializeRecordedFile();
        }

        /// <summary>
        /// Initializes the recorded file.
        /// </summary>
        private void InitializeRecordedFile()
        {
            InitializeRecordedFileAttributes();
            InitializeRecordedFileFormat();
            InitializeRootOfActionListTree();
        }

        /// <summary>
        /// Initializes the recorded file's attributes.
        /// </summary>
        private void InitializeRecordedFileAttributes()
        {
            if (File.Exists(this.actionsListFileName))
            {
                File.Delete(this.actionsListFileName);
                Stream stream = File.Create(this.actionsListFileName);
                stream.Close();
                File.SetAttributes(actionsListFileName, FileAttributes.Hidden | FileAttributes.Temporary);
            }
        }

        /// <summary>
        /// Initializes the recorded file format.
        /// </summary>
        private void  InitializeRecordedFileFormat()
        {
            xDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Actions"));
            Stream stream = File.OpenWrite(this.actionsListFileName);
            xDoc.Save(stream);
            stream.Flush();
            stream.Close();
        }

        /// <summary>
        /// Initializes the root of action list tree.
        /// </summary>
        private void InitializeRootOfActionListTree()
        {
            Stream stream = File.OpenRead(this.actionsListFileName);
            xRoot = XElement.Load(stream);
            stream.Close();
        }

        /// <summary>
        /// Saves the recorded actions.
        /// </summary>
        public void SaveActions()
        {
            Stream stream = File.OpenWrite(this.actionsListFileName);
            xRoot.Save(stream);
            stream.Close();
        }

        /// <summary>
        /// Saves the recorded file to a new Path.
        /// </summary>
        /// <param name="newFilePath">The new file path.</param>
        public void SaveRecordedFileAs(string newFilePath)
        {
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Copy(this.actionsListFileName, newFilePath);
            File.SetAttributes(newFilePath, FileAttributes.Normal);
        }
    }
}

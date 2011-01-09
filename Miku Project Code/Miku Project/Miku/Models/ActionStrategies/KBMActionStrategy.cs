using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using Miku.Client.Models.Hooks;
using Miku.Client.Models.Simulators;
using System.Threading;
using Miku.Client.Models.Recorders;


namespace Miku.Client.Models.ActionStrategies
{
    public class KBMActionStrategy : ActionStrategy
    {
        private KBMActionRecorderLinq kbmActionRecorder;
        private KBMLLHook kbmHOOK;
        private KBMSimulator kbmSimulator;

        public KBMActionStrategy()
            : this(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]))
        {

        }

        public KBMActionStrategy(IntPtr hookedInstance)
        {
            kbmHOOK = new KBMLLHook(hookedInstance);
            kbmActionRecorder = new KBMActionRecorderLinq();
            kbmSimulator = new KBMSimulator();
        }

        #region ActionStrategy

        public override void StartRecordActions()
        {
            if (this.kbmHOOK.SetHook())
            {
                this.kbmHOOK.MouseEvent += new MouseEventHandler(kbmHOOK_MouseEvent);
                this.kbmHOOK.KeyboardEvent += new KeyboardEventHandler(kbmHOOK_KeyboardEvent);
            }
            else
            {
                throw new Exception("Can not Set a Hook!");
            }
        }

        /// <summary>
        /// HOOK MouseEvent event.
        /// </summary>
        /// <param name="mouseEvent">The mouse event.</param>
        /// <param name="mouse">The mouse.</param>
        /// <param name="delayTime">The delay time.</param>
        void kbmHOOK_MouseEvent(MouseEvents mouseEvent, MSLLHOOKSTRUCT mouse, int delayTime)
        {
            OnRecording(mouseEvent.ToString() + "|" + mouse.pt.ToString());
            this.kbmActionRecorder.WriteData(mouseEvent.ToString(), mouse, delayTime);
        }

        /// <summary>
        ///  HOOK Keyboard event.
        /// </summary>
        /// <param name="keyEvent">The key event.</param>
        /// <param name="keyData">The key data.</param>
        /// <param name="delayTime">The delay time.</param>
        void kbmHOOK_KeyboardEvent(KeyboardEvents keyEvent, System.Windows.Forms.Keys keyData, int delayTime)
        {
            OnRecording(keyEvent.ToString() + "|" + keyData.ToString());
            this.kbmActionRecorder.WriteData(keyEvent.ToString(), keyData, delayTime);
        }

        /// <summary>
        /// Stops to record actions.
        /// </summary>
        public override void StopRecordActions()
        {
            this.kbmHOOK.UnHook();
            this.kbmActionRecorder.SaveActions();
        }

        /// <summary>
        /// Starts to  playback.
        /// </summary>
        public override void StartPlayback()
        {
            object[] actions = kbmActionRecorder.GetDatas();

            Thread.Sleep(1000);

            foreach (var item in actions)
            {
                if (item is Win32API.MouseEvent)
                {
                    kbmSimulator.Simulate(((Win32API.MouseEvent)item).dwFlags, ((Win32API.MouseEvent)item).pt.x, ((Win32API.MouseEvent)item).pt.y, 0, UIntPtr.Zero);
                    Thread.Sleep(((Win32API.MouseEvent)item).delayTime);
                }
                else if (item is Win32API.KeyEvent)
                {
                    kbmSimulator.Simulate(((Win32API.KeyEvent)item).bVk, ((Win32API.KeyEvent)item).vScan, ((Win32API.KeyEvent)item).dwFlags, ((Win32API.KeyEvent)item).dwExtraInfo);
                    Thread.Sleep(((Win32API.KeyEvent)item).delayTime);
                }
            }
        }


        public override void StartPlaybackWithExistingFile(string filePath)
        {
            KBMActionRecorderLinq tmpKbmActionRecorder = new KBMActionRecorderLinq(filePath);

            object[] actions = tmpKbmActionRecorder.GetDatas();

            Thread.Sleep(1000);

            foreach (var item in actions)
            {
                if (item is Win32API.MouseEvent)
                {
                    kbmSimulator.Simulate(((Win32API.MouseEvent)item).dwFlags, ((Win32API.MouseEvent)item).pt.x, ((Win32API.MouseEvent)item).pt.y, 0, UIntPtr.Zero);
                    Thread.Sleep(((Win32API.MouseEvent)item).delayTime);
                }
                else if (item is Win32API.KeyEvent)
                {
                    kbmSimulator.Simulate(((Win32API.KeyEvent)item).bVk, ((Win32API.KeyEvent)item).vScan, ((Win32API.KeyEvent)item).dwFlags, ((Win32API.KeyEvent)item).dwExtraInfo);
                    Thread.Sleep(((Win32API.KeyEvent)item).delayTime);
                }
            }
        }

        /// <summary>
        /// Saves the actions.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public override void SaveActions(string filepath)
        {
            kbmActionRecorder.SaveRecordedFileAs(filepath);
        }
        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using Miku.Client.Models.Hooks;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Miku.Client.Models.Simulators;
using System.Runtime.InteropServices;
using System.Threading;
using Miku.Client.Models.Recorders;

namespace Miku.Client.Models.ActionStrategies
{
    public class KBActionStrategy : ActionStrategy
    {
        KBDLLHook kbHook;
        KeyboardSimulator kbSimulator;
        KBActionRecorderLinq kbActionRecorder;

        public KBActionStrategy()
            : this(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]))
        {

        }

        public KBActionStrategy(IntPtr hookedInstance)
        {
            kbHook = new KBDLLHook(hookedInstance);
            kbActionRecorder = new KBActionRecorderLinq();
            kbSimulator = new KeyboardSimulator();
        }

        #region ActionStrategy

        /// <summary>
        /// Starts to record actions.
        /// </summary>
        public override void StartRecordActions()
        {
            if (this.kbHook.SetHook())
            {
                this.kbHook.KeyboardEvent += new KeyboardEventHandler(KBHook_KeyboardEvent);
            }
            else
            {
                throw new Exception("Can not Set a Hook!");
            }
        }

        /// <summary>
        /// Stops to record actions.
        /// </summary>
        public override void StopRecordActions()
        {
            this.kbHook.UnHook();
            this.kbActionRecorder.SaveActions();
        }

        /// <summary>
        /// Starts to  playback.
        /// </summary>
        public override void StartPlayback()
        {
            Win32API.KeyEvent[] actions = kbActionRecorder.GetDatas();

            Thread.Sleep(1000);

            foreach (var item in actions)
            {
                kbSimulator.Simulate(item.bVk, 0, item.dwFlags, 0);
                Thread.Sleep(item.delayTime);
            }

        }

        public override void StartPlaybackWithExistingFile(string filePath)
        {
            KBActionRecorderLinq tmpKbActionRecorder = new KBActionRecorderLinq(filePath);

            Win32API.KeyEvent[] actions = tmpKbActionRecorder.GetDatas();

            Thread.Sleep(1000);

            foreach (var item in actions)
            {
                kbSimulator.Simulate(item.bVk, 0, item.dwFlags, 0);
                Thread.Sleep(item.delayTime);
            }
        }

        /// <summary>
        /// Saves the actions.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public override void SaveActions(string filepath)
        {
            kbActionRecorder.SaveRecordedFileAs(filepath);
        }

        #endregion

        /// <summary>
        ///  the hook_ keyboard event.
        /// </summary>
        /// <param name="keyEvent">The key event.</param>
        /// <param name="key">The key.</param>
        /// <param name="time">The time.</param>
        void KBHook_KeyboardEvent(KeyboardEvents keyEvent, System.Windows.Forms.Keys key,int time)
        {
            //录制键盘按键
            this.kbActionRecorder.WriteData(keyEvent.ToString(), key,time);

            OnRecording(key.ToString());
        }
    }
}

﻿/*
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Threading;
using Miku.Client;
using Miku.Client.Models.Hooks;
using Miku.Client.Models.Simulators;
using System.Reflection;
using System.Runtime.InteropServices;
using Miku.Client.Models.Recorders;

namespace Miku.Client.Models.ActionStrategies
{
    public class MActionStrategy : ActionStrategy
    {
        MSLLHook mHook;
        MouseSimulator mSimulator;
        MActionRecorderLinq mActionRecorder;

        public MActionStrategy()
            : this(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]))
        {
        }

        public MActionStrategy(IntPtr hookedInstance)
        {
            mHook = new MSLLHook(hookedInstance);
            mActionRecorder = new MActionRecorderLinq();
            mSimulator = new MouseSimulator();
        }

        #region ActionStrategy

        public override void StartRecordActions()
        {
            if (this.mHook.SetHook())
            {
                this.mHook.MouseEvent += new MouseEventHandler(MHook_MouseEvent);
            }
            else
            {
               // throw new Exception("勾取失败");
            }
        }

        public override void StopRecordActions()
        {
            this.mHook.UnHook();
            mActionRecorder.SaveActions();
        }

        public override void StartPlayback()
        {
            Win32API.MouseEvent[] actions = mActionRecorder.GetDatas();

            Thread.Sleep(1000);

            foreach (var item in actions)
            {
                mSimulator.Simulate(item.dwFlags , item.pt.x, item.pt.y, 0, UIntPtr.Zero);
                Thread.Sleep(item.delayTime);
            }
        }

        public override void SaveActions(string filepath)
        {
            mActionRecorder.SaveRecordedFileAs(filepath);
        }
        #endregion

        void MHook_MouseEvent( MouseEvents mouseEvent, MSLLHOOKSTRUCT myMouse,int delayTime)
        {
            OnRecording(mouseEvent.ToString()+"|"+myMouse.pt.ToString());
            mActionRecorder.WriteData(mouseEvent.ToString(), myMouse, delayTime);
        }
    }
}

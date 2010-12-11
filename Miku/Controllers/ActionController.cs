using System;
using System.Collections.Generic;
using System.Text;
using Miku.Client.Models.Hooks;
using System.Runtime.InteropServices;
using System.Reflection;
using Miku.Client.Models.ActionStrategies;
using Miku.Client.Views;

namespace Miku.Client.Controllers
{
    public enum RecordStrategies { Mouse,Keyboard,MouseAndKeyboard};
    /// <summary>
    /// Control to request the response from the GUI
    /// </summary>
    public class ActionController : IActionController
    {
        private IActionView actionView;
        private IActionStrategy actionStrategy;

        public ActionController(IActionView actionView, RecordStrategies recordStrategy)
            : this(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), actionView,recordStrategy)
        {
        }

        public ActionController(IntPtr hookedInstance, IActionView actionView, RecordStrategies recordStrategy)
        {
            
            switch (recordStrategy)
            {
                case RecordStrategies.Mouse:
                    actionStrategy = new MActionStrategy();
                    break;
                case RecordStrategies.Keyboard:
                    actionStrategy = new KBActionStrategy();
                    break;
                case RecordStrategies.MouseAndKeyboard:
                    actionStrategy = new KBMActionStrategy();
                    break;
                default:
                    break;
            }
            this.actionView = actionView;
        }

        #region IActionController

        /// <summary>
        /// Requests starting to record actions.
        /// </summary>
        public void RequestStartRecordActions()
        {
            this.actionView.ResponseStartRecording();

            this.actionStrategy.OnRecordingEvent += new OnRecordingEventHandler(this.actionView.ResponseKeepRecording);
            this.actionStrategy.StartRecordActions();
        }

        /// <summary>
        /// Requests stopping to record actions.
        /// </summary>
        public void RequestStopRecordActions()
        {
            this.actionView.ResponseStopRecording();

            this.actionStrategy.StopRecordActions();
        }

        /// <summary>
        /// Requests  starting to playback.
        /// </summary>
        public void RequestStartPlayback()
        {
            this.actionView.ResponseStartPlayback();

            this.actionStrategy.StartPlayback();
        }

        /// <summary>
        /// Requests saving actions.
        /// </summary>
        public void RequestSaveActions()
        {
            string filepath = String.Empty;

            this.actionView.ResponseSaveActions(ref filepath);

            if (filepath != String.Empty)
            {
                this.actionStrategy.SaveActions(filepath);
            }
            
        }
    }
    	#endregion
}

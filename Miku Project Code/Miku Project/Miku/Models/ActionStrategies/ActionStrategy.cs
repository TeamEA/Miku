using System;
using System.Collections.Generic;
using System.Text;
using Miku.Client.Models.Hooks;

namespace Miku.Client.Models.ActionStrategies
{
    public abstract class ActionStrategy : IActionStrategy
    {
        /// <summary>
        /// Starts to record actions.
        /// </summary>
        public virtual void StartRecordActions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops to record actions.
        /// </summary>
        public virtual void StopRecordActions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts to  playback.
        /// </summary>
        public virtual void StartPlayback()
        {
            throw new NotImplementedException();
        }

        public event OnRecordingEventHandler OnRecordingEvent;


        /// <summary>
        /// Called when [recording].
        /// </summary>
        /// <param name="recodingInfo">The recoding info.</param>
        public void OnRecording(string recodingInfo)
        {
            OnRecordingEventHandler handler = OnRecordingEvent;
            if (handler != null)
            {
                this.OnRecordingEvent(recodingInfo);
            }
        }

        /// <summary>
        /// Saves the actions.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public virtual void SaveActions(string filepath)
        {
            throw new NotImplementedException();
        }


        public virtual void StartPlaybackWithExistingFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}

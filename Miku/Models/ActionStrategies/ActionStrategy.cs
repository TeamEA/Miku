using System;
using System.Collections.Generic;
using System.Text;
using Miku.Client.Models.Hooks;

namespace Miku.Client.Models.ActionStrategies
{
    public abstract class ActionStrategy : IActionStrategy
    {
        public virtual void StartRecordActions()
        {
            throw new NotImplementedException();
        }

        public virtual void StopRecordActions()
        {
            throw new NotImplementedException();
        }

        public virtual void StartPlayback()
        {
            throw new NotImplementedException();
        }

        public event OnRecordingEventHandler OnRecordingEvent;


        public void OnRecording(string recodingInfo)
        {
            if (OnRecordingEvent != null)
            {
                this.OnRecordingEvent(recodingInfo);
            }
        }

        public virtual void SaveActions(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}

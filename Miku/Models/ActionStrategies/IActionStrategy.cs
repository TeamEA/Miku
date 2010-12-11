using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Models.ActionStrategies
{
    public delegate void OnRecordingEventHandler(string recordInfo);

    /// <summary>
    /// The interface that the ActionStrategy must realize
    /// </summary>
    public interface IActionStrategy
    {
        void StartRecordActions();
        void StopRecordActions();
        void StartPlayback();
        void SaveActions(string filepath);
        event OnRecordingEventHandler OnRecordingEvent;
        void OnRecording(string recordInfo);
    }
}

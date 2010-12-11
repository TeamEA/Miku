using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Views
{
    /// <summary>
    /// The interface that the ActionView need to implement
    /// </summary>
    public interface IActionView
    {
        void ResponseStartRecording();
        void ResponseStopRecording();
        void ResponseKeepRecording(string recordInfo);
        void ResponseStartPlayback();
        void ResponseSaveActions(ref string filepath);
    }
}

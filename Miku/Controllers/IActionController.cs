using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Controllers
{
    /// <summary>
    /// The interface that the ActionController need to implement
    /// </summary>
    public interface IActionController
    {
        void RequestStartRecordActions();
        void RequestStopRecordActions();
        void RequestStartPlayback();
        void RequestSaveActions();
    }
}

using System;
namespace Miku.Client.Models.Recorders
{
    interface IActionRecorder
    {
        void SaveActions();
        void SaveRecordedFileAs(string newFilePath);
    }
}

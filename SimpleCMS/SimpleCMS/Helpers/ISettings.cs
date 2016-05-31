using System;
namespace SimpleCMS
{
    interface ISettings
    {
        string AppName { get; set; }
        string AppRegInfo { get; set; }
    }
}

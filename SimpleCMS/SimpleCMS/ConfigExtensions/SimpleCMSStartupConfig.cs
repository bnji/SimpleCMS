using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public partial class SimpleCMSStartupConfig
    {
        public static void Configure(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Any connection or hub wire up and configuration should go here
            System.AppDomain.CurrentDomain.Load(typeof(Hubs.MessageHub).Assembly.FullName);
            app.MapSignalR();
        }
    }
}
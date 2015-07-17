using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JP.Shared.PermissiveXFrameHeader.Modules
{
    public class PermissiveXFrameHeaderModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        protected void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            // General requests
            if (!context.Items.Contains("AllowFraming"))
                context.Items.Add("AllowFraming", String.Empty);

            // IPFS requests
            if (!context.Items.Contains("FrameOptionsHeaderSet"))
                context.Items.Add("FrameOptionsHeaderSet", String.Empty);
        }

        protected void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            // XLViewer
            context.Response.Headers.Remove("X-FRAME-OPTIONS");
        }
    }
}

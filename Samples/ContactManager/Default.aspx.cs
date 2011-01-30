// <copyright>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace ContactManager
{
    using System;
    using System.Linq;

    using Microsoft.ServiceModel.Http;

    public partial class Default : System.Web.UI.Page
    {
        protected string ViewAsButtonCaption { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetViewAsButtonCaption();
            var repository = new ContactRepository();
            this.Repeater1.DataSource = repository.GetAll().OrderBy(c => c.Name);
            this.Repeater1.DataBind();
        }

        // Chooses the correct caption based on the accept headers
        // Chrome and FF send "application/png"
        private void SetViewAsButtonCaption()
        {
            var accept = this.Request.Headers["Accept"];
            var formats = new string[] { "application/xml", "text/xml", "image/png" };
            var format = ContentNegotiationHelper.GetBestMatch(accept, formats);

            if (format.Equals("image/png"))
            {
                this.ViewAsButtonCaption = "Image";
            }
            else
            {
                this.ViewAsButtonCaption = "Xml";
            }
        }
    }
}
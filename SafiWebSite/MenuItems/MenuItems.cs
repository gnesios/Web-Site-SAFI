using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Navigation;

namespace SafiWebSite.MenuItems
{
    [ToolboxItemAttribute(false)]
    public class MenuItems : WebPart
    {
        protected override void CreateChildControls()
        {
            try
            {
                string formatedLinks = this.RetrieveFormatedLinksFromTopNavigationBar();
                LiteralControl menuScript = new LiteralControl();

                if (string.IsNullOrEmpty(formatedLinks))
                {
                    menuScript.Text = "No existen elementos de menú definidos.";
                }
                else
                {
                    menuScript.Text = string.Format(
                        "<div class='u-full-width menu_bar ms-dialogHidden'>" +
                        "<nav id='push-menu' class='cbp-spmenu cbp-spmenu-vertical cbp-spmenu-right'>" +
                        "<a class='home' href='/'></a><div class='separator'></div>" +
                        "{0}" +
                        "<div class='separator hidden'></div>" +
                        "<a class='hidden' href='https://www.safimsconline.com/' target='_blank'>SAFI MSC Online</a>" +
                        "</nav>" +
                        "</div>",
                        formatedLinks);
                }

                this.Controls.Add(menuScript);
            }
            catch (Exception ex)
            {
                LiteralControl errorMessage = new LiteralControl();
                errorMessage.Text = ex.Message;

                this.Controls.Clear();
                this.Controls.Add(errorMessage);
            }
        }

        /// <summary>
        /// Recupera los valores definidos para la 'navegacio global' del sitio web.
        /// </summary>
        /// <returns></returns>
        private string RetrieveFormatedLinksFromTopNavigationBar()
        {
            string formatedLinks = "";

            using (SPSite sps = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb spw = sps.OpenWeb())
            {
                SPNavigationNodeCollection topLinkBar = spw.Navigation.TopNavigationBar;

                foreach (SPNavigationNode item in topLinkBar)
                {
                    if (item.Properties["NodeType"].ToString() == "AuthoredLinkPlain")
                    {
                        formatedLinks = formatedLinks + string.Format(
                            "<a href='{0}'>{1}</a><div class='separator'></div>",
                            item.Url, item.Title);
                    }
                }

                formatedLinks = formatedLinks.Remove(formatedLinks.LastIndexOf("<div class='separator'></div>"));
            }

            return formatedLinks;
        }
    }
}

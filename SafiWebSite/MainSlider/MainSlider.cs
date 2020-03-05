using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SafiWebSite.MainSlider
{
    [ToolboxItemAttribute(false)]
    public class MainSlider : WebPart
    {
        #region Web part parameters
        private string spPictureLibrary;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Imágenes"),
        WebDescription("Nombre de la biblioteca de imágenes a usar."),
        Category("Configuración")]
        public string SpPictureLibrary
        {
            get { return spPictureLibrary; }
            set { spPictureLibrary = value; }
        }

        public enum Types { PERSONAS, EMPRESAS };
        private Types spSliderType;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Tipo"),
        WebDescription("Tipo de rotador."),
        Category("Configuración")]
        public Types SpSliderType
        {
            get { return spSliderType; }
            set { spSliderType = value; }
        }
        #endregion

        public MainSlider() : base()
        {
            SpPictureLibrary = "Imágenes Rotador Principal";
        }

        protected override void CreateChildControls()
        {
            try
            {
                string formatedPictures = this.GetFormatedPicturesFromLibrary(
                    SpPictureLibrary.Trim(), SpSliderType.ToString());
                LiteralControl sliderScript = new LiteralControl();

                sliderScript.Text = string.Format(
                    "<div id='da-slider-personas' class='da-slider-personas'>" +
                    "{0}" +
                    "<nav class='da-arrows'>" +
                    "<span class='da-arrows-prev'></span>" +
                    "<span class='da-arrows-next'></span>" +
                    "</nav>" +
                    "</div>",
                    formatedPictures);

                this.Controls.Add(sliderScript);
            }
            catch
            {
                LiteralControl errorMessage = new LiteralControl();
                errorMessage.Text = "El control no fué configurado correctamente.";

                this.Controls.Clear();
                this.Controls.Add(errorMessage);
            }
        }

        /// <summary>
        /// Recupera los valores ya formateados para las imágenes de la biblioteca dada.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        private string GetFormatedPicturesFromLibrary(string libraryName, string type)
        {
            string formatedPictures = "";

            using (SPSite sps = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb spw = sps.OpenWeb())
            {
                SPQuery query = new SPQuery();
                query.Query = string.Format("<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>" +
                    "<Where><And><And>" +
                    "<Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq>" +
                    "<Eq><FieldRef Name='Tipo_x0020_rotador' /><Value Type='Text'>{0}</Value></Eq>" +
                    "</And><Eq><FieldRef Name='DocIcon' /><Value Type='Computed'>png</Value></Eq>" +
                    "</And></Where>",
                    type);
                SPListItemCollection pictures = spw.Lists[libraryName].GetItems(query);

                foreach (SPListItem picture in pictures)
                {
                    string title = picture.Title;
                    string url = "/" + picture.Url;
                    string link = "javascript:;";
                    try
                    {//Si no existe el campo 'Enlace' la operacion continua
                        if (picture["Enlace"] != null || !string.IsNullOrEmpty(picture["Enlace"].ToString()))
                            link = picture["Enlace"].ToString().Remove(picture["Enlace"].ToString().IndexOf(','));
                    }
                    catch { }

                    if (!picture.Name.Contains("-mov"))
                    {
                        formatedPictures = formatedPictures + string.Format(
                            "<div class='da-slide'>" +
                            "<div class='da-link'><a href='{0}'><img src='{1}' alt='{2}' /></a></div>" +
                            "<div class='da-link-mov'><a href='{0}'><img src='{3}' alt='{2}' /></a></div>" +
                            "<div class='da-img'><img src='{4}' alt='{2}' /></div>" +
                            "</div>",
                            link, url, title, url.Replace(".png", "-mov.png"), url.Replace(".png", ".jpg"));
                    }
                }

                return formatedPictures;
            }
        }
    }
}

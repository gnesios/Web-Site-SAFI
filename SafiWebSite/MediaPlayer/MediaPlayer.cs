using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SafiWebSite.MediaPlayer
{
    [ToolboxItemAttribute(false)]
    public class MediaPlayer : WebPart
    {
        #region Web part parameters
        private string spFileUrl;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("URL Archivo"),
        WebDescription("Dirección URL del archivo multimedia (mp4/mp3)."),
        Category("Configuración")]
        public string SpFileUrl
        {
            get { return spFileUrl; }
            set { spFileUrl = value; }
        }

        private string spPoster;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Poster"),
        WebDescription("Imagen mostrada antes de la reproducción."),
        Category("Configuración")]
        public string SpPoster
        {
            get { return spPoster; }
            set { spPoster = value; }
        }

        public enum PreloadType { none, auto, metadata };
        private PreloadType spPreload;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Auto-carga"),
        WebDescription("Descarga el contenido automáticamente."),
        Category("Configuración")]
        public PreloadType SpPreload
        {
            get { return spPreload; }
            set { spPreload = value; }
        }

        private int spWidth;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Ancho"),
        WebDescription("Ancho del control de reproducción (pixeles)."),
        Category("Configuración")]
        public int SpWidth
        {
            get { return spWidth; }
            set { spWidth = value; }
        }

        private int spHeight;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Alto"),
        WebDescription("Alto del control de reproducción (pixeles)."),
        Category("Configuración")]
        public int SpHeight
        {
            get { return spHeight; }
            set { spHeight = value; }
        }

        private bool spAutoplay;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Auto-reproducción"),
        WebDescription("Reproduce automáticamente el contenido."),
        Category("Configuración")]
        public bool SpAutoplay
        {
            get { return spAutoplay; }
            set { spAutoplay = value; }
        }
        #endregion

        public MediaPlayer() : base()
        {
            SpFileUrl = "";
            SpPoster = "";
            SpPreload = PreloadType.none;
            SpWidth = 640;
            SpHeight = 360;
            SpAutoplay = false;
        }

        protected override void CreateChildControls()
        {
            try
            {
                if (string.IsNullOrEmpty(spFileUrl))
                {
                    this.Controls.Clear();
                    this.Controls.Add(new LiteralControl("Debe configurar éste control antes de usarlo."));
                    return;
                }

                string theAutoplay = "";
                if (SpAutoplay)
                {
                    theAutoplay = "autoplay";
                    SpPreload = PreloadType.auto;
                }

                LiteralControl mediaScript;
                mediaScript = new LiteralControl();

                if (SpFileUrl.Trim().EndsWith(".mp4"))
                {
                    #region Video script (video-js)
                    mediaScript.Text = string.Format(
                        "<link href='/_catalogs/masterpage/safi/media/video-js.css' rel='stylesheet' type='text/css'>" +
                        "<script src='/_catalogs/masterpage/safi/media/video.min.js'></script>" +
                        "<script type='text/javascript'>" +
                        "_V_.options.flash.swf = '/_layouts/MediaControls/Player/video-js.swf';" +
                        "</script>" +
                        "<video id='theVideo' class='video-js vjs-default-skin' " +
                        "controls {3} preload='{4}' width='{0}' height='{1}'" +
                        "poster='{2}' data-setup='{{}}'>" +
                        "<source src='{5}' type='video/mp4' />" +
                        "</video>",
                        SpWidth, SpHeight, SpPoster.Trim(), theAutoplay, SpPreload.ToString(), SpFileUrl.Trim());
                    #endregion
                }
                else if (SpFileUrl.Trim().EndsWith(".mp3"))
                {
                    #region Audio script
                    mediaScript.Text = string.Format(
                        "<script src='/_catalogs/masterpage/safi/media/audio.min.js'></script>" +
                        "<script type='text/javascript'>" +
                        "audiojs.events.ready(" +
                        "function() {{" +
                        "var as = audiojs.createAll();" +
                        "}});" +
                        "</script>" +
                        "<audio src='{0}' {1} preload='{2}' />",
                        SpFileUrl.Trim(), theAutoplay, SpPreload.ToString());
                    #endregion
                }
                else
                    mediaScript.Text = "Error en la configuración del control.";

                this.Controls.Add(mediaScript);
            }
            catch(Exception ex)
            {
                Literal errorMessage = new Literal();
                errorMessage.Text = ex.Message;

                this.Controls.Clear();
                this.Controls.Add(errorMessage);
            }
        }
    }
}

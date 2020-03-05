using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace SafiWebSite.ContactForm
{
    [ToolboxItemAttribute(false)]
    public partial class ContactForm : WebPart
    {
        const string CONTACT_LIST = "Formulario Contacto";

        #region Web part parameters
        private string spSiteKey;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Site Key"),
        WebDescription("Llave de sitio usada por el control reCPATCHA."),
        Category("Configuración")]
        public string SpSiteKey
        {
            get { return spSiteKey; }
            set { spSiteKey = value; }
        }

        private string spSecretKey;
        [Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        WebDisplayName("Secret Key"),
        WebDescription("Llave secreta usada por el control reCPATCHA."),
        Category("Configuración")]
        public string SpSecretKey
        {
            get { return spSecretKey; }
            set { spSecretKey = value; }
        }
        #endregion
        
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ContactForm()
        {
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid && ValidateCatpcha())
                {
                    //NO FUNCIONA EN SANDBOX
                    using (SPSite sps = new SPSite(SPContext.Current.Web.Url))
                    using (SPWeb spw = sps.OpenWeb())
                    {
                        SPListItemCollection itemsFormularioContacto = spw.Lists[CONTACT_LIST].Items;
                        SPListItem newItem = itemsFormularioContacto.Add();

                        newItem["Title"] = txbApellido.Text;
                        newItem["Nombre contacto"] = txbNombre.Text;
                        newItem["Teléfono contacto"] = txbTelefono.Text;
                        newItem["Correo contacto"] = txbCorreo.Text;
                        newItem["Ciudad contacto"] = ddlCiudad.SelectedValue;
                        newItem["Zona contacto"] = txbZona.Text;
                        newItem["Mensaje contacto"] = txbMensaje.Text;
                        newItem.Update();

                        lblCaptcha.Text = "";

                        //Close the modal dialog. The 'CONTACTO' value is the [returnValue] of the CloseDialogCallback() method of the custom.js
                        this.Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup('CONTACTO');</script>");
                        this.Context.Response.Flush();
                        this.Context.Response.End();
                    }
                }
                else
                {
                    lblCaptcha.Text = "Debe responder correctamente esta prueba.";
                    btnSend.Focus();
                    this.Page.SetFocus(btnSend);
                }
            }
            catch (Exception ex)
            {
                lblCaptcha.Text = "ERROR >> " + ex.Message;
            }
        }

        #region reCAPTCHA
        private bool ValidateCatpcha()
        {
            string Response = this.Page.Request["g-recaptcha-response"];//Getting Response String Appned to Post Method
            bool Valid = false;

            string SecretKey = SpSecretKey;
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                SecretKey, Response));

            try
            {
                using (System.Net.WebResponse wResponse = req.GetResponse())
                {
                    using (System.IO.StreamReader readStream = new System.IO.StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        MyObject data = js.Deserialize<MyObject>(jsonResponse);// Deserialize Json 
                        Valid = Convert.ToBoolean(data.success);
                    }
                }

                return Valid;
            }
            catch (System.Net.WebException ex)
            {
                throw ex;
            }
        }

        private class MyObject { public string success { get; set; } }
        #endregion
    }
}

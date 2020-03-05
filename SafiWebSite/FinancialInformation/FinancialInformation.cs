using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SafiWebSite.FinancialInformation
{
    [ToolboxItemAttribute(false)]
    public class FinancialInformation : WebPart
    {
        const string PRODUCTS_LIST = "Productos SAFI";
        const string NAME_CT = "Producto SAFI";

        protected override void CreateChildControls()
        {
            try
            {
                string formatedProducts = this.RetrieveFormatedProducts();
                LiteralControl sliderScript = new LiteralControl();

                sliderScript.Text = string.Format(
                    "<div class='u-full-width financial_bar ms-dialogHidden'><div class='container'>" +
                    "<ul class='newsticker'>" +
                    "{0}" +
                    "<li>Las tasas están calculadas sobre la base de rendimientos pasados y están sujetas a variaciones.</li>" +
                    "<li>El valor de la cuota de los fondos de inversión es variable por tanto,</li>" +
                    "<li>la rentabilidad o ganancia obtenida en el pasado por el fondo, no significa necesariamente, que se repita en el futuro.</li>" +
                    "<nav id='da-buttons'><span id='prev-button'></span><span id='next-button'></span></nav>" +
                    "</ul>" +
                    //"<p class='description'>Las tasas están calculadas sobre la base de rendimientos pasados y están sujetas a variaciones. El valor de la cuota de los fondos de inversión es variable por tanto, la rentabilidad o ganancia obtenida en el pasado por el fondo, no significa necesariamente, que se repita en el futuro.</p>" +
                    "</div></div>",
                    formatedProducts);

                this.Controls.Add(sliderScript);
            }
            catch
            {
                //LiteralControl errorMessage = new LiteralControl();
                //errorMessage.Text = "El control no fué configurado correctamente.";

                this.Controls.Clear();
                //this.Controls.Add(errorMessage);
            }
        }

        /// <summary>
        /// Recupera la información asociada a informacion financiera de la lista de parámetros del sitio.
        /// </summary>
        /// <returns></returns>
        private string RetrieveFormatedProducts()
        {
            string formatedProducts = "";

            using (SPSite sps = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb spw = sps.OpenWeb())
            {
                SPQuery query = new SPQuery();
                query.Query = "<OrderBy><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>" +
                    "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>";
                SPListItemCollection products = spw.Lists[PRODUCTS_LIST].GetItems(query);

                foreach (SPListItem product in products)
                {
                    if (product.ContentType.Name == NAME_CT)
                    {
                        formatedProducts = formatedProducts + string.Format(
                            "<li><b>{0}</b> Tasa: {2}% | Cartera: {1} {3} | Valor de cuota: {1} {4}</li>",
                            product.Title.Trim(), product["Moneda"].ToString(),
                            string.Format("{0:,0.00}", double.Parse(product["Valor tasa"].ToString()) * 100),
                            string.Format("{0:0,0.00}", double.Parse(product["Valor cartera"].ToString())),
                            string.Format("{0:0,0.00000}", double.Parse(product["Valor cuota"].ToString())));
                    }
                    else
                    {
                        formatedProducts = formatedProducts + string.Format("<li>Datos al {0} | Tasa a 30 días</li>",
                            DateTime.Parse(product["Fecha actualización"].ToString()).ToLongDateString());
                    }
                }
            }

            return formatedProducts;
        }
    }
}

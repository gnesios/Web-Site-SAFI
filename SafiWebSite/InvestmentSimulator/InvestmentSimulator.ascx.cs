using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace SafiWebSite.InvestmentSimulator
{
    [ToolboxItemAttribute(false)]
    public partial class InvestmentSimulator : WebPart
    {
        const string PRODUCTS_LIST = "Productos SAFI";
        const string NAME_CT = "Producto SAFI";
        const string DATE_CT = "Producto SAFI (Fecha)";

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InvestmentSimulator()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            try
            {
                this.GetSimulatorInputs();
            }
            catch (Exception ex)
            {
                ltrMessage.Text = ex.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GetSimulatorInputs()
        {
            using (SPSite sps = new SPSite(SPContext.Current.Web.Url))
            using (SPWeb spw = sps.OpenWeb())
            {
                SPQuery query = new SPQuery();
                query.Query = "<OrderBy><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>" +
                    "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>";
                SPListItemCollection products = spw.Lists[PRODUCTS_LIST].GetItems(query);

                List<string> formatedProducts = this.GetFormatedProducts(products);

                ltrProducts.Text = string.Format(
                    "<section id='section-1'>{0}</section>" +
                    "<section id='section-2'>{1}</section>",
                    formatedProducts[0], formatedProducts[1]);
                theDate.Text = formatedProducts[2];
            }
        }

        /// <summary>
        /// Recupera los productos segun su moneda [USD][BOB][fecha].
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private List<string> GetFormatedProducts(SPListItemCollection products)
        {
            List<string> formatedProducts = new List<string>(3);

            string formatedProductsUSD = "";
            string formatedProductsBOB = "";
            string formatedDate = "";

            foreach (SPListItem product in products)
            {
                if (product.ContentType.Name == NAME_CT && product["Tipo producto"].ToString() == "ABIERTO")
                {
                    string logo = product["Logotipo"].ToString();
                    if (logo.Contains(",")) { logo = product["Logotipo"].ToString().Split(',')[0].Trim(); }

                    string script = string.Format(
                        "<label class='radiob' for='ID{0}'>" +
                        "<input type='radio' value='{1}' name='{2}' id='ID{0}' onclick='GetRate(\"ID{0}\");'><span><img src='{3}' alt='' /></span>" +
                        "</label>",
                        product.ID, product["Valor tasa"].ToString(), product["Moneda"].ToString(), logo);

                    if (product["Moneda"].ToString() == "USD" || product["Moneda"].ToString() == "$us.")
                    {
                        formatedProductsUSD = formatedProductsUSD + script;
                    }
                    else if (product["Moneda"].ToString() == "BOB" || product["Moneda"].ToString() == "Bs.")
                    {
                        formatedProductsBOB = formatedProductsBOB + script;
                    }
                }
                else if (product.ContentType.Name == DATE_CT)
                {
                    formatedDate = DateTime.Parse(product["Fecha actualización"].ToString()).ToLongDateString();
                }
            }

            formatedProducts.Add(formatedProductsUSD);
            formatedProducts.Add(formatedProductsBOB);
            formatedProducts.Add(formatedDate);

            return formatedProducts;
        }
    }
}

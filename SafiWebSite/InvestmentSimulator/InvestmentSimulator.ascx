<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvestmentSimulator.ascx.cs" Inherits="SafiWebSite.InvestmentSimulator.InvestmentSimulator" %>

<asp:Literal runat="server" ID="ltrMessage" Text=""></asp:Literal>

<div id="tabs" class="tabs">
    <nav>
        <ul>
            <li><a href="#section-1" class="icon-USD" onclick="HiddeBlock()"><span>D&oacute;lares</span></a></li>
            <li><a href="#section-2" class="icon-BOB" onclick="HiddeBlock()"><span>Bolivianos</span></a></li>
        </ul>
    </nav>
    <div class="container content">
        <!--PRODCUTS SELECTION-->
        <asp:Literal runat="server" ID="ltrProducts" Text=""></asp:Literal>
        <div id="block1">
            <p>Tasa a 30 días*:</p>
            <label id="theRate"></label>
            <!--FORM FIELDS-->
            <div class="form-body">
                <label for="fldName"><span>Inversi&oacute;n (<label ID="theCurrency"></label>): </span><input id="theAmount" type="text" name="fldName" value="" /></label>
                <p><label><input type="submit" value="Calcular" onclick="ExecuteProcedure();" /></label></p>
                <p id="block2">Rendimiento obtenido al mes**: <label id="theResult"></label></p>
            </div>
            <p class="footnote">* Tasa a 30 días registrada al <asp:Label runat="server" ID="theDate"></asp:Label>. La tasa esta calculada sobre la base de rendimientos pasados y están sujetas a variaciones.<br/>El valor de la cuota de los fondos de inversión es variable por tanto, la rentabilidad o ganancia obtenida en el pasado, no significa necesariamente que se repita en el futuro.</p>
            <p class="footnote">** Considérese que la rentabilidad utilizada para el cálculo de su ganancia, toma en cuenta los rendimientos que ha tenido el Fondo en el pasado, los cuales están sujetos a variación diaria, por tanto los montos calculados corresponden a inversiones realizadas 30 días atrás.</p>
        </div>
    </div>
</div>
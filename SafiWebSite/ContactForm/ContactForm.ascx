<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.ascx.cs" Inherits="SafiWebSite.ContactForm.ContactForm" %>

<div class="form">
    <div class="form-head">
	    <h4>Formulario de Contacto</h4>
        <img src="/_catalogs/masterpage/safi/images/icon_02.png" alt="" />
    </div>
    <div class="form-body">
        <label for="txbNombre">
            <span class="tags">Nombre:<br />
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbNombre" SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbNombre" MaxLength="50" />
	    </label>
	    <label for="txbApellido">
            <span class="tags">Apellido:<br />
                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbApellido" SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbApellido" MaxLength="50" />
	    </label>
	    <label for="txbTelefono">
            <span class="tags">Teléfono:<br />
                <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbTelefono" SetFocusOnError="true" />
                <asp:RegularExpressionValidator ID="rxvTelefono" runat="server" Text="No válido."
                    Display="Dynamic" ControlToValidate="txbTelefono" ValidationExpression="\d+" SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbTelefono" MaxLength="8" />
	    </label>
	    <label for="txbCorreo">
            <span class="tags">Correo electrónico:<br />
                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbCorreo" SetFocusOnError="true" />
                <asp:RegularExpressionValidator ID="rxvCorreo" runat="server" Text="No válida."
                    Display="Dynamic" ControlToValidate="txbCorreo" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbCorreo" MaxLength="50" />
	    </label>
	    <label for="ddlCiudad">
            <span class="tags">Departamento:<br />
                <asp:RequiredFieldValidator runat="server" ID="rfvCiudad" Text="Requerido."
                    InitialValue="" Display="Dynamic" ControlToValidate="ddlCiudad" SetFocusOnError="true" />
            </span>
	        <asp:DropDownList runat="server" ID="ddlCiudad">
                <asp:ListItem>COCHABAMBA</asp:ListItem>
                <asp:ListItem Selected="True">LA PAZ</asp:ListItem>
                <asp:ListItem>ORURO</asp:ListItem>
                <asp:ListItem>POTOSÍ</asp:ListItem>
                <asp:ListItem>SANTA CRUZ</asp:ListItem>
                <asp:ListItem>SUCRE</asp:ListItem>
                <asp:ListItem>TARIJA</asp:ListItem>
                <asp:ListItem>OTRA</asp:ListItem>
	        </asp:DropDownList>
	    </label>
	    <label for="txbZona">
            <span class="tags">Barrio/Zona:<br />
                <asp:RequiredFieldValidator ID="rfvZona" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbZona" SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbZona" MaxLength="150" />
	    </label>
	    <label for="txbMensaje">
            <span class="tags">Me interesa:<br />
                <asp:RequiredFieldValidator ID="rfvMensaje" runat="server" Text="Requerido."
                    Display="Dynamic" ControlToValidate="txbMensaje" SetFocusOnError="true" />
            </span>
	        <asp:TextBox runat="server" ID="txbMensaje" MaxLength="500" TextMode="MultiLine" />
	    </label>
	    <p class="note">Todos los campos son obligatorios.</p>
    
        <script src='https://www.google.com/recaptcha/api.js'></script>
        <div class="g-recaptcha" data-sitekey="<%= SpSiteKey %>"></div>
        <asp:Label runat="server" ID="lblCaptcha" CssClass="message"></asp:Label>
    
        <p><asp:Button runat="server" ID="btnSend" Text="Enviar" OnClick="btnSend_Click" /></p>
    </div>
</div>

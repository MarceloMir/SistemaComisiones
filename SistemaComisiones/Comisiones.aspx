<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Comisiones.aspx.vb" Inherits="SistemaComisiones.Comisiones" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calcular comisiones por rango de fechas</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="container mt-5">
    <form id="form1" runat="server">
        <div class="card p-4 shadow-sm mb-4">
            <h2>Calcular comisiones por rango de fechas</h2>

            <div class="mb-3 mt-3">
                <label class="form-label">Fecha Inicio:</label>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <div class="mb-3">
                <label class="form-label">Fecha Fin:</label>
                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <asp:Button ID="btnCalcular" runat="server" Text="Calcular" CssClass="btn btn-primary" OnClick="btnCalcular_Click" />
        </div>

        <!-- Tabla equivalente al *ngIf y *ngFor de Angular -->
        <asp:Panel ID="pnlResultado" runat="server" CssClass="card p-4 shadow-sm" Visible="false">
            <h3>Resultados</h3>
            <asp:GridView ID="gvComisiones" runat="server" CssClass="table table-striped table-bordered mt-2" 
                AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" />
                    <asp:BoundField DataField="TotalVentas" HeaderText="Total Ventas" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Comision" HeaderText="Comisión" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
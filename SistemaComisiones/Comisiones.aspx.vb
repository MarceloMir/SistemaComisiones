Public Class Comisiones
    Inherits System.Web.UI.Page

    Protected WithEvents txtFechaInicio As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFechaFin As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCalcular As System.Web.UI.WebControls.Button
    Protected WithEvents gvComisiones As System.Web.UI.WebControls.GridView
    Protected WithEvents pnlResultado As System.Web.UI.WebControls.Panel

    Private datosComision As New ComisionData()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Inicializar con el mes actual por comodidad
            txtFechaInicio.Text = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd")
            txtFechaFin.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Protected Sub btnCalcular_Click(sender As Object, e As EventArgs)
        Dim fechaInicio As DateTime
        Dim fechaFin As DateTime

        If DateTime.TryParse(txtFechaInicio.Text, fechaInicio) AndAlso DateTime.TryParse(txtFechaFin.Text, fechaFin) Then
            ' Ajustar fecha fin al final del día para incluir todas las transacciones
            fechaInicio = fechaInicio.Date
            fechaFin = fechaFin.AddDays(1).AddSeconds(-1)

            Dim dtResultado As DataTable = datosComision.CalcularComisionesPorFecha(fechaInicio, fechaFin)

            gvComisiones.DataSource = dtResultado
            gvComisiones.DataBind()

            ' Mostrar la tabla solo si hay registros (equivalente al *ngIf del repositorio)
            pnlResultado.Visible = (dtResultado.Rows.Count > 0)
        End If
    End Sub
End Class
Imports System.Data
Imports System.Data.SqlClient

Public Class ComisionData
    Private ReadOnly conexionString As String = "Server=DESKTOP-VTAMRG6;Database=DBComisionesNuevo;Trusted_Connection=True;"

    Public Function CalcularComisionesPorFecha(fechaInicio As DateTime, fechaFin As DateTime) As DataTable
        Dim dtVentas As New DataTable()
        Dim dtReglas As New DataTable()
        Dim dtResultado As New DataTable()

        ' Definir la estructura de la tabla resultado final
        dtResultado.Columns.Add("Vendedor", GetType(String))
        dtResultado.Columns.Add("TotalVentas", GetType(Decimal))
        dtResultado.Columns.Add("Comision", GetType(Decimal))

        Using con As New SqlConnection(conexionString)
            con.Open()

            ' 1. Obtener las ventas en el rango de fechas uniendo con Vendedor (usando Nombres como definiste en tu SQL)
            Dim queryVentas As String = "SELECT v.Id, v.FechaVenta, v.Monto, v.VendedorId, vend.Nombres AS VendedorNombre " &
                                        "FROM Venta v INNER JOIN Vendedor vend ON v.VendedorId = vend.Id " &
                                        "WHERE v.FechaVenta >= @FechaInicio AND v.FechaVenta <= @FechaFin"
            Using cmdVentas As New SqlCommand(queryVentas, con)
                cmdVentas.Parameters.AddWithValue("@FechaInicio", fechaInicio)
                cmdVentas.Parameters.AddWithValue("@FechaFin", fechaFin)
                Using da As New SqlDataAdapter(cmdVentas)
                    da.Fill(dtVentas)
                End Using
            End Using

            ' 2. Obtener las reglas de comisiones ordenadas de mayor a menor monto mínimo
            Dim queryReglas As String = "SELECT Id, Porcentaje, MontoMinimo FROM Regla ORDER BY MontoMinimo DESC"
            Using cmdReglas As New SqlCommand(queryReglas, con)
                Using da As New SqlDataAdapter(cmdReglas)
                    da.Fill(dtReglas)
                End Using
            End Using
        End Using

        ' 3. Validar si hay filas para evitar errores de conversión
        If dtVentas.Rows.Count = 0 Then
            Return dtResultado
        End If

        ' 4. Agrupar por Vendedor de forma segura usando conversiones directas de tipo CType
        Dim gruposVendedor = From row In dtVentas.AsEnumerable()
                             Group row By Vendedor = row.Item("VendedorNombre").ToString() Into Group

        For Each grupo In gruposVendedor
            Dim totalVentas As Decimal = 0
            For Each r In grupo.Group
                totalVentas += Convert.ToDecimal(r.Item("Monto"))
            Next

            Dim porcentajeAplicado As Decimal = 0

            ' Buscar la regla correspondiente según el acumulado de ventas
            For Each reglaRow As DataRow In dtReglas.Rows
                Dim montoMinimo As Decimal = Convert.ToDecimal(reglaRow.Item("MontoMinimo"))
                If totalVentas >= montoMinimo Then
                    porcentajeAplicado = Convert.ToDecimal(reglaRow.Item("Porcentaje"))
                    Exit For
                End If
            Next

            Dim comisionTotal As Decimal = totalVentas * porcentajeAplicado

            ' Añadir al DataTable final
            dtResultado.Rows.Add(grupo.Vendedor, totalVentas, comisionTotal)
        Next

        Return dtResultado
    End Function
End Class
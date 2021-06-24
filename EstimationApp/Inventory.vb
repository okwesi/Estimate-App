Imports System.Data.SQLite

Module Inventory

    Public Sub Dgvitems()
        Dim sda As New SQLiteDataAdapter
        Dim dbinding As New BindingSource
        Dim dbDataset As New DataTable
        Try
            Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))
                Dim getitems As String = "Select * from Items"
                Dim command As New SQLiteCommand(getitems, connection)
                sda.SelectCommand = command
                sda.Fill(dbDataset)
                dbinding.DataSource = dbDataset
                MainForm.dgvitems2.DataSource = dbinding
                sda.Update(dbDataset)
                connection.Open()
                Dim reader As SQLiteDataReader = command.ExecuteReader
                reader.Read()
                reader.Close()
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Public Sub DgvItems2CellClicked(ByVal search As String)
        Dim sda As New SQLiteDataAdapter
        Dim dbinding As New BindingSource
        Try
            Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False"))
                Dim query As String = ("SELECT * FROM items where item_id = '" & search & "' ")
                Dim command As New SQLiteCommand(query, connection)

                Dim adapter As New SQLiteDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)

                connection.Open()
                Dim reader As SQLiteDataReader = command.ExecuteReader
                reader.Read()
                MainForm.txtInvItemno.Text = table(0)(0)
                MainForm.txtInvItemname.Text = table(0)(1)
                MainForm.txtInvItemprice.Text = table(0)(2)

                reader.Dispose()
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If

                table.Dispose()
                adapter.Dispose()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            sda.Dispose()
            dbinding.Dispose()
        End Try
    End Sub

    Public Sub Additem()
        Using connectionString As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

            Dim additem As String = "insert into items
                    VALUES('" & MainForm.txtInvItemno.Text & "','" & MainForm.txtInvItemname.Text & "',
                           '" & MainForm.txtInvItemprice.Text & "') "
            Dim insertcommand As New SQLiteCommand(additem, connectionString)
            connectionString.Open()
            insertcommand.ExecuteNonQuery()
            connectionString.Close()
            connectionString.Dispose()
            Dgvitems()
            Functionss.Datagridview()
        End Using
    End Sub

    Public Sub Updateitem()
        Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))
            Dim updateitem As String = "update items set item_name = '" & MainForm.txtInvItemname.Text & "',
                                        item_price='" & MainForm.txtInvItemprice.Text & "' where item_id='" & MainForm.txtInvItemno.Text & "' "
            Dim updatecommand As New SQLiteCommand(updateitem, connection)
            connection.Open()
            updatecommand.ExecuteNonQuery()
            connection.Close()
            connection.Dispose()
            Dgvitems()
            Functionss.Datagridview()
        End Using
    End Sub

    Public Sub Removeitem()
        Using connectionString As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

            Dim removeitem As String = "delete from Items where item_id='" & MainForm.txtInvItemno.Text & "' "
            Dim removecommand As New SQLiteCommand(removeitem, connectionString)
            connectionString.Open()
            removecommand.ExecuteNonQuery()
            connectionString.Close()
            connectionString.Dispose()
            Dgvitems()
            Functionss.Datagridview()
        End Using
        MainForm.txtInvItemno.Text = Nothing
        MainForm.txtInvItemname.Text = Nothing
        MainForm.txtInvItemprice.Text = Nothing
    End Sub

End Module
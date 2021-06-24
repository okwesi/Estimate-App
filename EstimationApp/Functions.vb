Imports System.Data.SQLite
Imports System.Data.DataTable

Module Functionss

    'the fuctions brings the  panels to the front and docks it to the parent panel
    'then checks the panel button to indicate the activeness of the panel
    Public Sub Home()

        MainForm.pnlhome.BringToFront()
        MainForm.pnlhome.Dock = DockStyle.Fill
        MainForm.homebtn.Checked = True
    End Sub

    'Public Sub Recieptpanel()
    '    MainForm.pnlreceipt.BringToFront()
    '    MainForm.pnlreceipt.Dock = DockStyle.Fill
    '    MainForm.btnreceiptpanel.Checked = True
    'End Sub

    Public Sub PrintPanel()
        MainForm.PrintPanel.BringToFront()
        MainForm.PrintPanel.Dock = DockStyle.Fill
        MainForm.printpanelbtn.Checked = True
    End Sub

    'the Functions connects the interface to the Database and reads items data to the item table on the home screen
    Public Sub Datagridview()
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
                MainForm.dgvitems.DataSource = dbinding
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

    'the Functions makes a live search on the items table according to what the user is typing in the search textbox
    Public Sub Livesearch()

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
                MainForm.dgvitems.DataSource = dbinding
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
        Dim dv As New DataView(dbDataset)
        dv.RowFilter = String.Format("item_name  Like '%{0}%'", MainForm.searchtxt.Text)
        MainForm.dgvitems.DataSource = dv

        Dim dv1 As New DataView(dbDataset)
        dv1.RowFilter = String.Format("item_name  Like '%{0}%'", MainForm.txtInventorysearch.Text)
        MainForm.dgvitems2.DataSource = dv1

    End Sub

    'the fuction pulls the data of the clikced item in the item table and fills the forms on the reciept panel.
    Public Sub DgvItemsCellClicked(ByVal search As String)
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
                MainForm.txtitemno.Text = table(0)(0)
                'item name from the receipt panel
                MainForm.txtitemname.Text = table(0)(1)
                'item price from the reciept panel
                MainForm.txtprice.Text = table(0)(2)

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

    Public Sub insert()
        Using connectionString As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

            Dim insertreceipt As String = "insert into receipt_item(Item_id,receipt_id,Name,Price,Qty,Subtotal)
                    VALUES('" & MainForm.txtitemno.Text & "','" & MainForm.txtreceiptno.Text & "',
                           '" & MainForm.txtitemname.Text & "', '" & MainForm.txtprice.Text & "',
                           '" & MainForm.txtquantity.Text & "','" & MainForm.txtsubtotal.Text & "' ) "
            Dim insertcommand As New SQLiteCommand(insertreceipt, connectionString)
            connectionString.Open()
            insertcommand.ExecuteNonQuery()
            connectionString.Close()
        End Using
        MainForm.txtitemno.Text = Nothing
        MainForm.txtitemname.Text = Nothing
        MainForm.txtprice.Text = Nothing
        MainForm.txtquantity.Text = Nothing
    End Sub

    Public Sub dgvdisplayitems()
        Dim dbgettable As New DataTable
        Dim sda As New SQLiteDataAdapter
        Dim dbinding As New BindingSource

        Using connectionString As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

            Dim gettable As String = "SELECT * FROM receipt_item WHERE receipt_id = '" & MainForm.txtreceiptno.Text & "' "
            Dim gettablecommand As New SQLiteCommand(gettable, connectionString)
            sda.SelectCommand = gettablecommand
            sda.Fill(dbgettable)
            dbinding.DataSource = dbgettable
            MainForm.dgvdisplay.DataSource = dbinding
            sda.Update(dbgettable)
            connectionString.Open()
            Dim gettablereader As SQLiteDataReader = gettablecommand.ExecuteReader
            gettablereader.Read()
            gettablereader.Close()
            connectionString.Close()
        End Using
    End Sub

    Public Sub update()
        Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

            Dim updatereceipt As String = "update receipt_item set Qty='" & MainForm.txtquantity.Text & "',
            subtotal='" & MainForm.txtsubtotal.Text & "' WHERE item_id='" & MainForm.txtitemno.Text &
            "' AND receipt_id = '" & MainForm.txtreceiptno.Text & "' "

            Dim updatecommand As New SQLiteCommand(updatereceipt, connection)
            connection.Open()
            updatecommand.ExecuteNonQuery()
            connection.Close()
            dgvdisplayitems()
            MainForm.txtitemno.Text = Nothing
            MainForm.txtitemname.Text = Nothing
            MainForm.txtprice.Text = Nothing
            MainForm.txtquantity.Text = Nothing
        End Using
    End Sub

    Public Sub Add()

        If Len(MainForm.txtreceiptno.Text) = 0 Or Len(MainForm.txtquantity.Text) = 0 Then
            MsgBox("Generate Receipt number Or Enter Count", MsgBoxStyle.Critical, "Warning")
        Else
            Try
                Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

                    Dim getpassword As String = "Select * from receipt_item where Item_id = '" & MainForm.txtitemno.Text & "' and receipt_id = '" & MainForm.txtreceiptno.Text & "' "
                    Dim selectcommand As New SQLiteCommand(getpassword, connection)
                    connection.Open()
                    Dim reader As SQLiteDataReader = selectcommand.ExecuteReader
                    Dim thereornot As Boolean = reader.Read()
                    reader.Close()
                    connection.Close()

                    If thereornot = False Then
                        insert()
                        dgvdisplayitems()
                    Else
                        update()
                    End If
                End Using
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

    End Sub

    Public Sub View2(ByVal search As String)
        Dim sda As New SQLiteDataAdapter
        Dim dbinding As New BindingSource
        Try
            Using connection As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False"))
                Dim query As String = ("SELECT * FROM receipt_item where item_id = '" & search & "'and receipt_id = '" & MainForm.txtreceiptno.Text & "' ")
                Dim command As New SQLiteCommand(query, connection)
                Dim adapter As New SQLiteDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)

                connection.Open()
                Dim reader As SQLiteDataReader = command.ExecuteReader
                reader.Read()
                MainForm.txtitemno.Text = table(0)(0)
                MainForm.txtreceiptno.Text = table(0)(1)
                MainForm.txtitemname.Text = table(0)(2)
                MainForm.txtprice.Text = table(0)(3)
                MainForm.txtquantity.Text = table(0)(4)
                MainForm.txtsubtotal.Text = table(0)(5)

                reader.Dispose()
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Public Sub Remove()
        If Len(MainForm.txtreceiptno.Text) = 0 Or Len(MainForm.txtquantity.Text) = 0 Then
            MsgBox("No Item Chosen", MsgBoxStyle.Critical, "Warning!!")
        Else
            Using connectionString As New SQLiteConnection(String.Format("Datasource = " & Application.StartupPath & "\databases\database1.db; Read Only = False "))

                Dim removeitem As String = "delete from receipt_item where item_id='" & MainForm.txtitemno.Text & "' AND receipt_id = '" & MainForm.txtreceiptno.Text & "' "
                Dim removecommand As New SQLiteCommand(removeitem, connectionString)
                connectionString.Open()
                removecommand.ExecuteNonQuery()
                connectionString.Close()

                dgvdisplayitems()

            End Using
        End If
        MainForm.txtitemno.Text = Nothing
        MainForm.txtitemname.Text = Nothing
        MainForm.txtprice.Text = Nothing
        MainForm.txtquantity.Text = Nothing
    End Sub

End Module
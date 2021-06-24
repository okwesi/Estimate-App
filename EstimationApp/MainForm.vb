Public Class MainForm
    Dim table As New DataTable("ItemRecieptTable")

    Private Sub cout(sender As Object, e As EventArgs) Handles Me.Load
        Functionss.Home()
        lblDateTime.Text = DateTime.Now
        Functionss.Datagridview()

    End Sub

    Private Sub btnclose_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        Me.Close()
    End Sub

    Private Sub homebtn_Click(sender As Object, e As EventArgs) Handles homebtn.Click
        Functionss.Home()
    End Sub

    'Private Sub btnreceiptpanel_Click(sender As Object, e As EventArgs) Handles btnreceiptpanel.Click
    '    Functionss.Recieptpanel()
    'End Sub

    Private Sub printpanelbtn_Click(sender As Object, e As EventArgs) Handles printpanelbtn.Click
        Functionss.PrintPanel()
    End Sub

    Private Sub searchtxt_TextChanged(sender As Object, e As EventArgs) Handles searchtxt.TextChanged
        Functionss.Livesearch()
    End Sub

    Public Sub dgvitems_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvitems.CellDoubleClick
        Dim num As Int16 = Nothing
        If e.RowIndex >= 0 Then
            num = e.RowIndex
            Dim row As DataGridViewRow
            row = Me.dgvitems.Rows(num)
            Dim itemno = row.Cells("item_id").Value.ToString
            Dim s As String = itemno
            Functionss.DgvItemsCellClicked(s)
            ''pnlreceipt.BringToFront()
            ''pnlreceipt.Dock = DockStyle.Fill
        End If
        txtquantity.Select()

    End Sub

    Private Sub dgvdisplay_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvdisplay.CellDoubleClick
        Dim num As Int16 = Nothing
        If e.RowIndex >= 0 Then
            num = e.RowIndex
            Dim row As DataGridViewRow
            row = Me.dgvdisplay.Rows(num)
            txtitemno.Text = row.Cells("item_id").Value.ToString
            Dim s As String = txtitemno.Text
            Functionss.View2(s)

        End If
    End Sub

    Private Sub dgvitems2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvitems2.CellClick
        Dim num As Int16 = Nothing
        If e.RowIndex >= 0 Then
            num = e.RowIndex
            Dim row As DataGridViewRow
            row = Me.dgvitems2.Rows(num)
            txtInvItemno.Text = row.Cells("item_id").Value.ToString
            Dim search As String = txtInvItemno.Text
            Inventory.DgvItems2CellClicked(search)
        End If
    End Sub

    Private Sub btngen_Click(sender As Object, e As EventArgs) Handles btngen.Click
        Dim random As New Random()
        txtreceiptno.Text = random.Next(10000, 90000000)
    End Sub

    Private Sub txtquantity_TextChanged(sender As Object, e As EventArgs) Handles txtquantity.TextChanged
        If Len(txtquantity.Text) > 0 Then
            txtsubtotal.Text = CDbl(txtquantity.Text) * CDbl(txtprice.Text)
        Else
            txtsubtotal.Text = Nothing
        End If
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Functionss.Add()
    End Sub

    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        Functionss.Remove()
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If dgvdisplay.Rows.Count = 0 Then
            MsgBox("ERROR!!, You have not entered any item")
        Else
            SubmitReciept.Gvreceipt()
            btnadd.Checked = True
            PrintPanel.BringToFront()
            PrintPanel.Dock = DockStyle.Fill
            printpanelbtn.Checked = True
            btnadd.Checked = False
        End If
    End Sub

    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        Try
            If GVreceipt.Rows.Count = 0 Then
                MsgBox("No item was entered!!")
            Else
                PrintDocument1.DefaultPageSettings.Landscape = True
                PrintDocument1.Print()
                dgvdisplay.DataSource = Nothing
                dgvdisplay.Rows.Clear()
                txtreceiptno.Text = Nothing
                table.Clear()
                GVreceipt.DataSource = table
                GVreceipt.DataSource = Nothing
                'GVreceipt.Rows.Clear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.receipt.Width, Me.receipt.Height)
        receipt.DrawToBitmap(bm, New Rectangle(5, 5, Me.receipt.Width, Me.receipt.Height))
        e.Graphics.DrawImage(bm, 0, 0)
        PrintDocument1.DefaultPageSettings.Landscape = True

    End Sub

    Private Sub txtcustomername_TextChanged(sender As Object, e As EventArgs) Handles txtcustomername.TextChanged
        lblcustomername.Text = txtcustomername.Text.ToUpper
        If Len(txtcustomername.Text) > 0 Then
            btnprint.Enabled = True
        Else
            btnprint.Enabled = False
        End If
    End Sub

    Private Sub GunaImageButton1_Click(sender As Object, e As EventArgs) Handles GunaImageButton1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub inventorypnbtn_Click(sender As Object, e As EventArgs) Handles inventorypnbtn.Click
        inventorypanel.BringToFront()
        inventorypanel.Dock = DockStyle.Fill
        Inventory.Dgvitems()
    End Sub

    Private Sub btnInventoryAdd_Click(sender As Object, e As EventArgs) Handles btnInventoryAdd.Click
        Inventory.Additem()
        dgvitems.Refresh()
    End Sub

    Private Sub btnInvClear_Click(sender As Object, e As EventArgs) Handles btnInvClear.Click
        txtInvItemno.Text = Nothing
        txtInvItemname.Text = Nothing
        txtInvItemprice.Text = Nothing
    End Sub

    Private Sub btnInventoryupdate_Click(sender As Object, e As EventArgs) Handles btnInventoryupdate.Click
        Inventory.Updateitem()
    End Sub

    Private Sub btnInventoryremove_Click(sender As Object, e As EventArgs) Handles btnInventoryremove.Click
        Inventory.Removeitem()
        dgvitems.Refresh()
    End Sub

    Private Sub txtInventorysearch_TextChanged(sender As Object, e As EventArgs) Handles txtInventorysearch.TextChanged
        Functionss.Livesearch()
    End Sub

End Class
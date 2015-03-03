Imports System.Media
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Telerik.WinControls.UI

Public Class AddPiece

    Dim _comboBox As Windows.Forms.ComboBox
    Dim _addButton As RadButton
    Dim _copyButton As RadButton
    Dim _copy As Boolean

    Public Sub AddMark(combo As Windows.Forms.ComboBox, addButton As RadButton, copyButton As RadButton, copy As Boolean)

        _comboBox = combo
        _addButton = addButton
        _copyButton = copyButton
        _copy = copy
        Location = Windows.Forms.Cursor.Position
        Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(Me)

    End Sub

    Private Sub Confirm_Click(sender As Object, e As EventArgs) Handles Confirm.Click

        If _copy Then

            Clean.Populate = False

        End If

        Dim doc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Using doc.LockDocument()
            Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
            Try

                Dim bt As BlockTable
                bt = transaction.GetObject(dwg.BlockTableId, OpenMode.ForRead)

                If bt.Has(PieceName.Text) Then

                    MsgBox("Member Already Exists!")
                    SystemSounds.Beep.Play()

                Else

                    _comboBox.Items.Add(PieceName.Text)
                    Dim point As Integer = _comboBox.Items.IndexOf(PieceName.Text)
                    _comboBox.SelectedIndex = point

                    If _comboBox.Enabled = False Then

                        _comboBox.Enabled = True

                    End If

                    _addButton.Enabled = False
                    _copyButton.Enabled = False

                    Close()

                End If

            Catch ex As Exception

                editor.WriteMessage("! A Problem Has Occured - " + ex.Message)
            Finally
                transaction.Dispose()
                editor.WriteMessage(ControlChars.Lf)
            End Try
        End Using

        ' End Transaction

    End Sub
   
End Class

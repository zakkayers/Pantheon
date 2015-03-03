Imports System.Media
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput

Public Class StandardSlabForm

    Public Sub Jolt()

        Clean.Populate = True

        If Lists.StandardSlabList.Count <> 0 Then

            For i = 0 To Lists.StandardSlabList.Count - 1

                MarkCombo.Items.Add(Lists.StandardSlabList.Item(i).Mark)

            Next
            Copy.Enabled = True
            MarkCombo.Enabled = True
            MarkCombo.SelectedIndex = MarkCombo.Items.Count - 1
        End If

        MarkCombo.DropDownStyle = ComboBoxStyle.DropDownList
        Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(Me)
    End Sub

    Private Sub Add_Click_1(sender As Object, e As EventArgs) Handles Add.Click

        Dim piece As AddPiece = New AddPiece()

        piece.AddMark(MarkCombo, Add, Copy, False)
    End Sub

    Private Sub Copy_Click(sender As Object, e As EventArgs) Handles Copy.Click

        Dim piece As AddPiece = New AddPiece()

        piece.AddMark(MarkCombo, Add, Copy, True)

    End Sub

    Private Sub Create_Click(sender As Object, e As EventArgs) Handles Create.Click

        Try

            Dim doc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
            Dim editor As Editor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor
            Dim dwg As Database = editor.Document.Database

            ' Start Transaction

            Using doc.LockDocument()
                Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                Try

                    ' Check For Block
                    Dim bt As BlockTable
                    bt = transaction.GetObject(dwg.BlockTableId, OpenMode.ForRead)

                    Dim markName As String = MarkCombo.Text

                    If bt.Has(markName) Then

                        MsgBox("Member """ + markName + """ Already Exists In Current Drawing." + Environment.NewLine + "Rename The Member To Continue Creation")
                        Clean.Populate = False

                    Else

                        Lists.StandardSlabList.Add(New StandardSlabObject(MarkCombo.Text,
                                                              Converter.StringToDistance(WidthField.Text),
                                                              Converter.StringToDistance(LengthField.Text),
                                                              NotchYes.IsChecked,
                                                              NotchNo.IsChecked,
                                                               Converter.StringToDistance(NotchDepthField.Text)))

                        transaction.Commit()
                        Close()
                        Lists.StandardSlabList.Item(MarkCombo.SelectedIndex).Draw()

                        editor.WriteMessage(ControlChars.Lf)

                    End If

                Catch ex As Exception

                    editor.WriteMessage("! A Problem Has Occured - " + ex.Message)
                Finally
                    transaction.Dispose()
                    editor.WriteMessage(ControlChars.Lf)
                End Try
            End Using

            ' End Transaction

        Catch ex As System.Exception

            MsgBox("Data Has Been Entered Incorrectly")
            SystemSounds.Beep.Play()

        End Try

       
    End Sub

    Private Sub MarkCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MarkCombo.SelectedIndexChanged

        If Clean.Populate Then
            If MarkCombo.SelectedIndex <> -1 Then

                Dim obj As Integer = MarkCombo.SelectedIndex

                If Lists.StandardSlabList.Count <> 0 Then

                    If MarkCombo.SelectedIndex > Lists.StandardSlabList.Count - 1 Then

                        WidthField.Text = ""
                        LengthField.Text = ""
                        NotchDepthField.Text = "1-1/2"""
                        NotchYes.IsChecked = True
                        NotchNo.IsChecked = False

                    Else

                        WidthField.Text = Converter.DistanceToString(Lists.StandardSlabList.Item(obj).Width)
                        LengthField.Text = Converter.DistanceToString(Lists.StandardSlabList.Item(obj).Length)
                        NotchDepthField.Text = Converter.DistanceToString(Lists.StandardSlabList.Item(obj).NotchAmount)
                        NotchYes.IsChecked = Lists.StandardSlabList.Item(obj).NotchYes
                        NotchNo.IsChecked = Lists.StandardSlabList.Item(obj).NotchNo

                    End If

                End If

            End If
        End If
    End Sub

    Private Sub NotchYes_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles NotchYes.ToggleStateChanged

        NotchDepthField.Enabled = True

    End Sub

    Private Sub NotchNo_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles NotchNo.ToggleStateChanged

        NotchDepthField.Enabled = False

    End Sub
End Class

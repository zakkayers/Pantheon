Imports System.Media
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime
Imports Telerik.WinControls.UI

Public Class OffCenWorkFrameForm

    Public Sub Jolt()

        Clean.Populate = True

        If Lists.OffCenWorkframeList.Count <> 0 Then

            For i = 0 To Lists.OffCenWorkframeList.Count - 1

                MarkCombo.Items.Add(Lists.OffCenWorkframeList.Item(i).Mark)

            Next
            MarkCombo.Enabled = True
            Copy.Enabled = True
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

            Dim leftPitchValue As Double
            Dim rightPitchValue As Double

            If Double.TryParse(LeftPitchField.Text, leftPitchValue) Then
                leftPitchValue = leftPitchValue / 12

                If Double.TryParse(RightPitchField.Text, rightPitchValue) Then
                    rightPitchValue = rightPitchValue / 12

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

                                Lists.OffCenWorkframeList.Add(New OffCenWorkFrameObject(markName,
                                                                                          Converter.StringToDistance(LeftHeightField.Text),
                                                                                          Converter.StringToDistance(LeftWidthField.Text),
                                                                                          Converter.StringToDistance(LeftLengthField.Text),
                                                                                          leftPitchValue,
                                                                                          Converter.StringToDistance(LeftPurlinField.Text),
                                                                                          Converter.StringToDistance(LeftGirtField.Text),
                                                                                          LeftFlush.IsChecked,
                                                                                          LeftBypass.IsChecked,
                                                                                          Converter.StringToDistance(RightHeightField.Text),
                                                                                          Converter.StringToDistance(RightWidthField.Text),
                                                                                          Converter.StringToDistance(RightLengthField.Text),
                                                                                          rightPitchValue,
                                                                                          Converter.StringToDistance(RightPurlinField.Text),
                                                                                          Converter.StringToDistance(RightGirtField.Text),
                                                                                          RightFlush.IsChecked,
                                                                                          RightBypass.IsChecked))

                                transaction.Commit()

                                Close()

                                Lists.OffCenWorkframeList.Item(MarkCombo.SelectedIndex).Draw()

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

                Else
                    MsgBox("Pitch Value Entered Incorrectly")
                    SystemSounds.Beep.Play()

                End If
            End If

        Catch ex As System.Exception

            MsgBox("Data Has Been Entered Incorrectly")
            SystemSounds.Beep.Play()

        End Try
    End Sub

    Private Sub LeftFlush_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
       Handles LeftFlush.ToggleStateChanged

        If LeftFlush.IsChecked Then

            LeftBypass.IsChecked = False

        End If
    End Sub

    Private Sub LeftBypass_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
        Handles LeftBypass.ToggleStateChanged

        If LeftBypass.IsChecked Then

            LeftFlush.IsChecked = False

        End If
    End Sub

    Private Sub RightFlush_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
       Handles RightFlush.ToggleStateChanged

        If RightFlush.IsChecked Then

            RightBypass.IsChecked = False

        End If
    End Sub

    Private Sub RightBypass_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
        Handles RightBypass.ToggleStateChanged

        If RightBypass.IsChecked Then

            RightFlush.IsChecked = False

        End If
    End Sub

    Private Sub MarkCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MarkCombo.SelectedIndexChanged


        If Clean.Populate Then
            If MarkCombo.SelectedIndex <> -1 Then

                Dim obj As Integer = MarkCombo.SelectedIndex

                If Lists.OffCenWorkframeList.Count <> 0 Then

                    If MarkCombo.SelectedIndex > Lists.OffCenWorkframeList.Count - 1 Then

                        LeftWidthField.Text = ""
                        LeftHeightField.Text = ""
                        LeftLengthField.Text = ""
                        LeftPitchField.Text = ""
                        LeftPurlinField.Text = "8"""
                        LeftGirtField.Text = "8"""
                        LeftFlush.IsChecked = True
                        LeftBypass.IsChecked = False
                        RightWidthField.Text = ""
                        RightHeightField.Text = ""
                        RightLengthField.Text = ""
                        RightPitchField.Text = ""
                        RightPurlinField.Text = "8"""
                        RightGirtField.Text = "8"""
                        RightFlush.IsChecked = True
                        RightBypass.IsChecked = False

                    Else

                        LeftWidthField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).LeftWidth)
                        LeftHeightField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).LeftEave)
                        LeftLengthField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).LeftLength)
                        LeftPitchField.Text = Lists.OffCenWorkframeList.Item(obj).LeftPitch * 12
                        LeftPurlinField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).LeftPurlin)
                        LeftGirtField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).LeftGirt)

                        LeftFlush.IsChecked = Lists.OffCenWorkframeList.Item(obj).LeftFlush
                        LeftBypass.IsChecked = Lists.OffCenWorkframeList.Item(obj).LeftBypass

                        RightWidthField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).RightWidth)
                        RightHeightField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).RightEave)
                        RightLengthField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).RightLength)
                        RightPitchField.Text = Lists.OffCenWorkframeList.Item(obj).RightPitch * 12
                        RightPurlinField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).RightPurlin)
                        RightGirtField.Text = Converter.DistanceToString(Lists.OffCenWorkframeList.Item(obj).RightGirt)

                        RightFlush.IsChecked = Lists.OffCenWorkframeList.Item(obj).RightFlush
                        RightBypass.IsChecked = Lists.OffCenWorkframeList.Item(obj).RightBypass

                    End If

                End If

            End If
        End If
    End Sub

End Class

Imports System.Media
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime
Imports Telerik.WinControls.UI

Public Class SymWorkFrameForm

    Public Sub Jolt()

        Clean.Populate = True

        If Lists.SymWorkframeList.Count <> 0 Then

            For i = 0 To Lists.SymWorkframeList.Count - 1

                MarkCombo.Items.Add(Lists.SymWorkframeList.Item(i).Mark)

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

            Dim pitchValue As Double
            If Double.TryParse(PitchField.Text, pitchValue) Then
                pitchValue = pitchValue / 12

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

                            Dim sideText As String = SideBayField.Text
                            Dim endText As String = EndBayField.Text

                            sideText = sideText.Replace(" ", "")
                            endText = endText.Replace(" ", "")

                            Dim sideParts As String() = sideText.Split(New Char() {","c})
                            Dim endParts As String() = endText.Split(New Char() {","c})

                            Dim sideBays(sideParts.Count) As Double
                            Dim endBays(endParts.Count) As Double

                            If sideText <> "" Then

                                For i = 0 To sideParts.Count - 1
                                    sideBays(i) = Converter.StringToDistance(sideParts(i), DistanceUnitFormat.Architectural)
                                Next
                            End If

                            If endText <> "" Then
                                For i = 0 To endParts.Count - 1
                                    endBays(i) = Converter.StringToDistance(endParts(i), DistanceUnitFormat.Architectural)
                                Next
                            End If

                            Lists.SymWorkframeList.Add(New SymWorkFrameObject(markName,
                                                                                      Converter.StringToDistance(HeightField.Text),
                                                                                      Converter.StringToDistance(WidthField.Text),
                                                                                      Converter.StringToDistance(LengthField.Text),
                                                                                      pitchValue,
                                                                                      Converter.StringToDistance(PurlinField.Text),
                                                                                      Converter.StringToDistance(GirtField.Text),
                                                                                      sideBays,
                                                                                      endBays,
                                                                                      Flush.IsChecked,
                                                                                      Bypass.IsChecked))

                            transaction.Commit()

                            Close()

                            Lists.SymWorkframeList.Item(MarkCombo.SelectedIndex).Draw()

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

        Catch ex As System.Exception

            MsgBox("Data Has Been Entered Incorrectly")
            SystemSounds.Beep.Play()

        End Try
    End Sub

    Private Sub Flush_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
        Handles Flush.ToggleStateChanged

        If Flush.IsChecked Then

            Bypass.IsChecked = False

        End If
    End Sub

    Private Sub Bypass_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) _
        Handles Bypass.ToggleStateChanged

        If Bypass.IsChecked Then

            Flush.IsChecked = False

        End If
    End Sub

    Private Sub MarkCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MarkCombo.SelectedIndexChanged


        If Clean.Populate Then
            If MarkCombo.SelectedIndex <> -1 Then

                Dim obj As Integer = MarkCombo.SelectedIndex

                If Lists.SymWorkframeList.Count <> 0 Then

                    If MarkCombo.SelectedIndex > Lists.SymWorkframeList.Count - 1 Then

                        WidthField.Text = ""
                        HeightField.Text = ""
                        LengthField.Text = ""
                        PitchField.Text = ""
                        PurlinField.Text = "8"""
                        GirtField.Text = "8"""
                        SideBayField.Text = "0"""
                        EndBayField.Text = "0"""
                        Flush.IsChecked = True
                        Bypass.IsChecked = False

                    Else
                        Dim sideText As String = ""
                        Dim endText As String = ""

                        For i = 0 To Lists.SymWorkframeList.Item(obj).SideBays.Count() - 1

                            sideText +=
                                Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).SideBays(i),
                                                           DistanceUnitFormat.Architectural, 0) + " , "

                        Next

                        For i = 0 To Lists.SymWorkframeList.Item(obj).EndBays.Count() - 1

                            endText +=
                                Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).EndBays(i),
                                                           DistanceUnitFormat.Architectural, 0) + " , "

                        Next

                        WidthField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).Width)
                        HeightField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).EaveHeight)
                        LengthField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).Length)
                        PitchField.Text = Lists.SymWorkframeList.Item(obj).Pitch * 12
                        PurlinField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).PurlinWeb)
                        GirtField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).GirtWeb)

                        SideBayField.Text = sideText
                        EndBayField.Text = endText
                        Clean.CleanText(SideBayField)
                        Clean.CleanText(EndBayField)

                        Flush.IsChecked = Lists.SymWorkframeList.Item(obj).Flush
                        Bypass.IsChecked = Lists.SymWorkframeList.Item(obj).Bypass



                    End If

                End If

            End If
        End If
    End Sub

   


End Class

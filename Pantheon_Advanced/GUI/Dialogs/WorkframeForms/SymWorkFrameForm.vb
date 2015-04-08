Imports System.Media
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime
Imports Telerik.WinControls.UI
Imports Application = Autodesk.AutoCAD.ApplicationServices.Application

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
        Application.ShowModelessDialog(Me)
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

                Dim doc As Document = Application.DocumentManager.MdiActiveDocument
                Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
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

                            Dim sideBayText As String = SideBayField.Text
                            Dim endBayText As String = EndBayField.Text

                            sideBayText = sideBayText.Replace(" ", "")
                            endBayText = endBayText.Replace(" ", "")

                            Dim sideParts As String() = sideBayText.Split(New Char() {","c})
                            Dim endParts As String() = endBayText.Split(New Char() {","c})

                            Dim sideList As List(Of String) = New List(Of String)()
                            Dim endList As List(Of String) = New List(Of String)()

                            ''Find SideMult Bays
                            If sideBayText <> "" Then

                                For i = 0 To sideParts.Count - 1

                                    If sideParts(i).Contains("*") Then

                                        Dim sideMult As String() = sideParts(i).Split(New Char() {"*"c})
                                        Dim multiplier As Integer = Integer.Parse(sideMult(0))
                                        For j = 1 To multiplier
                                            sideList.Add(sideMult(1))
                                        Next

                                    Else
                                        sideList.Add(sideParts(i))
                                    End If
                                Next

                            End If

                            ''Find EndMult Bays
                            If endBayText <> "" Then

                                For i = 0 To endParts.Count - 1

                                    If endParts(i).Contains("*") Then

                                        Dim endMult As String() = endParts(i).Split(New Char() {"*"c})
                                        Dim multiplier As Integer = Integer.Parse(endMult(0))
                                        For j = 0 To multiplier - 1
                                            endList.Add(endMult(1))
                                        Next

                                    Else
                                        endList.Add(endParts(i))
                                    End If
                                Next

                            End If

                            Dim sideBays(sideList.Count) As Double
                            Dim endBays(endList.Count) As Double

                            If sideBayText <> "" Then

                                For i = 0 To sideList.Count - 1
                                    sideBays(i) = Converter.StringToDistance(sideList(i), DistanceUnitFormat.Architectural)
                                Next
                            End If

                            If endBayText <> "" Then
                                For i = 0 To endList.Count - 1
                                    endBays(i) = Converter.StringToDistance(endList(i), DistanceUnitFormat.Architectural)
                                Next
                            End If


                            Lists.SymWorkframeList.Add(New SymWorkFrameObject(markName, Converter.StringToDistance(HeightField.Text), Converter.StringToDistance(WidthField.Text), Converter.StringToDistance(LengthField.Text), pitchValue, Converter.StringToDistance(PurlinField.Text), Converter.StringToDistance(GirtField.Text), SideBayField.Text, EndBayField.Text, sideBays, endBays, Flush.IsChecked, Bypass.IsChecked))

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

    Private Sub Flush_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Flush.ToggleStateChanged

        If Flush.IsChecked Then

            Bypass.IsChecked = False

        End If
    End Sub

    Private Sub Bypass_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Bypass.ToggleStateChanged

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
                        SideBayField.Text = ""
                        EndBayField.Text = ""
                        Flush.IsChecked = True
                        Bypass.IsChecked = False

                    Else
                       

                        WidthField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).Width)
                        HeightField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).EaveHeight)
                        LengthField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).Length)
                        PitchField.Text = Lists.SymWorkframeList.Item(obj).Pitch * 12
                        PurlinField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).PurlinWeb)
                        GirtField.Text = Converter.DistanceToString(Lists.SymWorkframeList.Item(obj).GirtWeb)

                        SideBayField.Text = Lists.SymWorkframeList.Item(obj).SideBayText
                        EndBayField.Text = Lists.SymWorkframeList.Item(obj).EndBayText
                        
                        Flush.IsChecked = Lists.SymWorkframeList.Item(obj).Flush
                        Bypass.IsChecked = Lists.SymWorkframeList.Item(obj).Bypass


                    End If

                End If

            End If
        End If
    End Sub
End Class

Imports System.Media
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime
Imports Telerik.WinControls.UI

Public Class StraightColumnForm

    Public Sub Jolt()

        Clean.Populate = True

        If Lists.StraightColumnList.Count <> 0 Then
            For i = 0 To Lists.StraightColumnList.Count - 1

                MarkCombo.Items.Add(Lists.StraightColumnList.Item(i).Mark)

            Next

            Copy.Enabled = True
            MarkCombo.Enabled = True
            MarkCombo.SelectedIndex = MarkCombo.Items.Count - 1

        Else

            ComboGirt1.SelectedIndex = 0
            ComboGirt2.SelectedIndex = 0
            ComboGirt3.SelectedIndex = 0
            ComboGirt4.SelectedIndex = 0
            ComboGirt5.SelectedIndex = 0

            CableCombo.SelectedIndex = 0
            AnchorCombo.SelectedIndex = 0

        End If

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

    Private Sub Flush_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Flush.ToggleStateChanged
        If Flush.IsChecked Then
            Block.IsChecked = False
            Block.Enabled = False
            Bypass.IsChecked = False
        End If
    End Sub

    Private Sub Bypass_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles Bypass.ToggleStateChanged
        If Bypass.IsChecked Then
            Block.Enabled = True
            Flush.IsChecked = False
        End If
    End Sub

    Private Sub AddGirt1_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles AddGirt1.ToggleStateChanged
        Dim toggle As Boolean = AddGirt1.IsChecked
        ComboGirt1.Enabled = toggle
        ElevGirt1.Enabled = toggle
        BraceGirt1.Enabled = toggle
    End Sub

    Private Sub AddGirt2_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles AddGirt2.ToggleStateChanged
        Dim toggle As Boolean = AddGirt2.IsChecked
        ComboGirt2.Enabled = toggle
        ElevGirt2.Enabled = toggle
        BraceGirt2.Enabled = toggle
    End Sub

    Private Sub AddGirt3_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles AddGirt3.ToggleStateChanged
        Dim toggle As Boolean = AddGirt3.IsChecked
        ComboGirt3.Enabled = toggle
        ElevGirt3.Enabled = toggle
        BraceGirt3.Enabled = toggle
    End Sub

    Private Sub AddGirt4_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles AddGirt4.ToggleStateChanged
        Dim toggle As Boolean = AddGirt4.IsChecked
        ComboGirt4.Enabled = toggle
        ElevGirt4.Enabled = toggle
        BraceGirt4.Enabled = toggle
    End Sub

    Private Sub AddGirt5_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles AddGirt5.ToggleStateChanged
        Dim toggle As Boolean = AddGirt5.IsChecked
        ComboGirt5.Enabled = toggle
        ElevGirt5.Enabled = toggle
        BraceGirt5.Enabled = toggle
    End Sub

    Private Sub CableCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles CableCheck.ToggleStateChanged
        Dim toggle = CableCheck.IsChecked
        CableCombo.Enabled = toggle
        CableFromFlange.Enabled = toggle
        CableFromTop.Enabled = toggle
        CableFromBottom.Enabled = toggle
    End Sub

    Private Sub Create_Click(sender As Object, e As EventArgs) Handles Create.Click

        Try


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


                        ' Create Girt Elevation List

                        Dim girtList As List(Of GirtObject) = New List(Of GirtObject)()
                        girtList.Add(New GirtObject(AddGirt1.IsChecked, "None", 0, False))
                        girtList.Add(New GirtObject(AddGirt2.IsChecked, "None", 0, False))
                        girtList.Add(New GirtObject(AddGirt3.IsChecked, "None", 0, False))
                        girtList.Add(New GirtObject(AddGirt4.IsChecked, "None", 0, False))
                        girtList.Add(New GirtObject(AddGirt5.IsChecked, "None", 0, False))


                        If AddGirt1.IsChecked Then
                            girtList.Item(0) = New GirtObject(AddGirt1.IsChecked, ComboGirt1.Text, Converter.StringToDistance(ElevGirt1.Text), BraceGirt1.IsChecked)
                        End If

                        If AddGirt2.IsChecked Then
                            girtList.Item(1) = New GirtObject(AddGirt2.IsChecked, ComboGirt2.Text, Converter.StringToDistance(ElevGirt2.Text), BraceGirt2.IsChecked)
                        End If

                        If AddGirt3.IsChecked Then
                            girtList.Item(2) = New GirtObject(AddGirt3.IsChecked, ComboGirt3.Text, Converter.StringToDistance(ElevGirt3.Text), BraceGirt3.IsChecked)
                        End If

                        If AddGirt4.IsChecked Then
                            girtList.Item(3) = New GirtObject(AddGirt4.IsChecked, ComboGirt4.Text, Converter.StringToDistance(ElevGirt4.Text), BraceGirt4.IsChecked)
                        End If

                        If AddGirt5.IsChecked Then
                            girtList.Item(4) = New GirtObject(AddGirt5.IsChecked, ComboGirt5.Text, Converter.StringToDistance(ElevGirt5.Text), BraceGirt5.IsChecked)
                        End If

                        ' Create Cable Object

                        Dim cableObj As CableObject = New CableObject(False, "1/4""", 3, 6, 6)

                        If CableCheck.IsChecked Then
                            cableObj.Check = True
                            cableObj.Diameter = CableCombo.Text
                            cableObj.FromFlange = Converter.StringToDistance(CableFromFlange.Text)
                            cableObj.FromTop = Converter.StringToDistance(CableFromTop.Text)
                            cableObj.FromBottom = Converter.StringToDistance(CableFromBottom.Text)
                        End If

                        ' Create Anchor Object

                        Dim anchorObj As AnchorObject = New AnchorObject(AnchorCombo.Text, Converter.StringToDistance(AnchorFromFlange.Text), Converter.StringToDistance(AnchorBetween.Text))

                        ' Create Column Object

                        Lists.StraightColumnList.Add(New StraightColumnObject(MarkCombo.Text,
                                                                              Flush.IsChecked,
                                                                              Bypass.IsChecked,
                                                                              girtList, cableObj,
                                                                              anchorObj,
                                                                              LeanToHoles.IsChecked,
                                                                              Block.IsChecked,
                                                                              Converter.StringToDistance(EaveThickness.Text),
                                                                              Converter.StringToDistance(EaveWidth.Text),
                                                                              Converter.StringToDistance(StiffenerThickness.Text),
                                                                              Converter.StringToDistance(StiffenerWidth.Text),
                                                                              Converter.StringToDistance(OuterFlangeThickness.Text),
                                                                              Converter.StringToDistance(OuterFlangeWidth.Text),
                                                                              Converter.StringToDistance(InnerFlangeThickness.Text),
                                                                              Converter.StringToDistance(InnerFlangeWidth.Text),
                                                                              Converter.StringToDistance(WebThickness.Text),
                                                                              Converter.StringToDistance(WebDepth.Text),
                                                                              Converter.StringToDistance(HaunchThickness.Text),
                                                                              Converter.StringToDistance(HaunchWidth.Text),
                                                                              Converter.StringToDistance(HaunchLength.Text),
                                                                              Converter.StringToDistance(BaseThickness.Text),
                                                                              Converter.StringToDistance(BaseWidth.Text)))

                        transaction.Commit()

                        Close()

                        Lists.StraightColumnList.Item(MarkCombo.SelectedIndex).Draw()

                        editor.WriteMessage(ControlChars.Lf)

                    End If

                Catch ex As System.Exception

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

                If Lists.StraightColumnList.Count <> 0 Then

                    If MarkCombo.SelectedIndex > Lists.StraightColumnList.Count - 1 Then

                        ' Toggle Buttons
                        Flush.IsChecked = True
                        Bypass.IsChecked = False
                        LeanToHoles.IsChecked = False
                        Block.IsChecked = False

                        ' Girt Group
                        AddGirt1.IsChecked = False
                        AddGirt2.IsChecked = False
                        AddGirt3.IsChecked = False
                        AddGirt4.IsChecked = False
                        AddGirt5.IsChecked = False

                        ComboGirt1.SelectedIndex = 0
                        ComboGirt2.SelectedIndex = 0
                        ComboGirt3.SelectedIndex = 0
                        ComboGirt4.SelectedIndex = 0
                        ComboGirt5.SelectedIndex = 0

                        ElevGirt1.Text = ""
                        ElevGirt2.Text = ""
                        ElevGirt3.Text = ""
                        ElevGirt4.Text = ""
                        ElevGirt5.Text = ""

                        BraceGirt1.IsChecked = False
                        BraceGirt2.IsChecked = False
                        BraceGirt3.IsChecked = False
                        BraceGirt4.IsChecked = False
                        BraceGirt5.IsChecked = False

                        ' Cable Group

                        CableCheck.IsChecked = False
                        CableCombo.SelectedIndex = 0
                        CableFromFlange.Text = "3"""
                        CableFromTop.Text = "6"""
                        CableFromBottom.Text = "6"""

                        ' Anchor Bolt Group

                        AnchorCombo.SelectedIndex = 0
                        AnchorFromFlange.Text = "4"""
                        AnchorBetween.Text = "4"""

                        ' Measurements

                        EaveThickness.Text = ""
                        EaveWidth.Text = ""

                        StiffenerThickness.Text = ""
                        StiffenerWidth.Text = ""

                        OuterFlangeThickness.Text = ""
                        OuterFlangeWidth.Text = ""

                        InnerFlangeThickness.Text = ""
                        InnerFlangeWidth.Text = ""

                        WebThickness.Text = "0.1345"""
                        WebDepth.Text = ""

                        HaunchThickness.Text = ""
                        HaunchWidth.Text = ""
                        HaunchLength.Text = ""

                        BaseThickness.Text = "1/2"""
                        BaseWidth.Text = "8"""

                    Else

                        ' Toggle Buttons
                        Flush.IsChecked = Lists.StraightColumnList.Item(obj).Flush
                        Bypass.IsChecked = Lists.StraightColumnList.Item(obj).Bypass
                        LeanToHoles.IsChecked = Lists.StraightColumnList.Item(obj).FlangeHoles
                        Block.IsChecked = Lists.StraightColumnList.Item(obj).Block

                        ' Girt Group
                        AddGirt1.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(0).Add
                        AddGirt2.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(1).Add
                        AddGirt3.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(2).Add
                        AddGirt4.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(3).Add
                        AddGirt5.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(4).Add

                        ' Girt 1

                        If Lists.StraightColumnList.Item(obj).GirtList.Item(0).Add Then

                            Dim point As Integer = ComboGirt1.Items.IndexOf(Lists.StraightColumnList.Item(obj).GirtList.Item(0).Type)
                            ComboGirt1.SelectedIndex = point
                            ElevGirt1.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).GirtList.Item(0).Elevation)
                            BraceGirt1.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(0).Brace

                        Else

                            ComboGirt1.SelectedIndex = 0
                            ElevGirt1.Text = ""
                            BraceGirt1.IsChecked = False

                        End If

                        ' Girt 2

                        If Lists.StraightColumnList.Item(obj).GirtList.Item(1).Add Then

                            Dim point As Integer = ComboGirt2.Items.IndexOf(Lists.StraightColumnList.Item(obj).GirtList.Item(1).Type)
                            ComboGirt2.SelectedIndex = point
                            ElevGirt2.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).GirtList.Item(1).Elevation)
                            BraceGirt2.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(1).Brace

                        Else

                            ComboGirt2.SelectedIndex = 0
                            ElevGirt2.Text = ""
                            BraceGirt2.IsChecked = False

                        End If

                        ' Girt 3

                        If Lists.StraightColumnList.Item(obj).GirtList.Item(2).Add Then

                            Dim point As Integer = ComboGirt3.Items.IndexOf(Lists.StraightColumnList.Item(obj).GirtList.Item(2).Type)
                            ComboGirt3.SelectedIndex = point
                            ElevGirt3.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).GirtList.Item(2).Elevation)
                            BraceGirt3.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(2).Brace

                        Else

                            ComboGirt3.SelectedIndex = 0
                            ElevGirt3.Text = ""
                            BraceGirt3.IsChecked = False

                        End If

                        'Girt 4

                        If Lists.StraightColumnList.Item(obj).GirtList.Item(3).Add Then

                            Dim point As Integer = ComboGirt4.Items.IndexOf(Lists.StraightColumnList.Item(obj).GirtList.Item(3).Type)
                            ComboGirt4.SelectedIndex = point
                            ElevGirt4.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).GirtList.Item(3).Elevation)
                            BraceGirt4.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(3).Brace

                        Else

                            ComboGirt4.SelectedIndex = 0
                            ElevGirt4.Text = ""
                            BraceGirt4.IsChecked = False

                        End If

                        ' Girt 5

                        If Lists.StraightColumnList.Item(obj).GirtList.Item(4).Add Then

                            Dim point As Integer = ComboGirt5.Items.IndexOf(Lists.StraightColumnList.Item(obj).GirtList.Item(4).Type)
                            ComboGirt5.SelectedIndex = point
                            ElevGirt5.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).GirtList.Item(4).Elevation)
                            BraceGirt5.IsChecked = Lists.StraightColumnList.Item(obj).GirtList.Item(4).Brace

                        Else

                            ComboGirt5.SelectedIndex = 0
                            ElevGirt5.Text = ""
                            BraceGirt5.IsChecked = False

                        End If


                        ' Cable Group

                        CableCheck.IsChecked = Lists.StraightColumnList.Item(obj).CableObj.Check

                        Dim cabPoint As Integer = CableCombo.Items.IndexOf(Lists.StraightColumnList.Item(obj).CableObj.Diameter)
                        CableCombo.SelectedIndex = cabPoint

                        CableFromFlange.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).CableObj.FromFlange)
                        CableFromTop.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).CableObj.FromTop)
                        CableFromBottom.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).CableObj.FromBottom)

                        ' Anchor Bolt Group

                        Dim aPoint As Integer = AnchorCombo.Items.IndexOf(Lists.StraightColumnList.Item(obj).AnchorObj.AnchorSize)
                        AnchorCombo.SelectedIndex = aPoint


                        AnchorFromFlange.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).AnchorObj.FromFlange)
                        AnchorBetween.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).AnchorObj.Between)

                        ' Measurements

                        EaveThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).EaveThick)
                        EaveWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).EaveWidth)

                        StiffenerThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).StiffThick)
                        StiffenerWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).StiffWidth)

                        OuterFlangeThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).OuterThick)
                        OuterFlangeWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).OuterWidth)

                        InnerFlangeThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).InnerThick)
                        InnerFlangeWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).InnerWidth)

                        WebThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).WebThick)
                        WebDepth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).WebDepth)

                        HaunchThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).HaunchThick)
                        HaunchWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).HaunchWidth)
                        HaunchLength.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).HaunchLength)

                        BaseThickness.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).BaseThick)
                        BaseWidth.Text = Converter.DistanceToString(Lists.StraightColumnList.Item(obj).BaseWidth)

                    End If

                End If

            End If
        End If
    End Sub
End Class

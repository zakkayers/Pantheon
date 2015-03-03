Imports Autodesk.Windows

Public Class Tools

    ' Method: CREATERIBBON
    ' Creates & Populates The "Tools" Ribbon Tab
    Public Sub CreateRibbon(ByVal control As RibbonControl)

        ' Create Ribbon Control Object

        If (control IsNot Nothing) Then

            Dim ribbonTab As RibbonTab = control.FindTab("TOOLS")

            If (ribbonTab IsNot Nothing) Then

                control.Tabs.Remove(ribbonTab)

            End If

            ribbonTab = Sculpt.MakeRibbonTab("Tools", "Tools")

            ' Add Ribbon To AutoCAD
            control.Tabs.Add(ribbonTab)

            ' Add Ribbon Components

            AddContent(ribbonTab)

        End If
    End Sub

    Dim _calculate As RibbonItemCollection = New RibbonItemCollection()
    Dim _gutter As RibbonItemCollection = New RibbonItemCollection()
    Dim _hipValley As RibbonItemCollection = New RibbonItemCollection()

    ' METHOD: ADDCONTENT
    ' Adds All Content To The Ribbon
    Private Sub AddContent(ByVal ribbonTab As RibbonTab)

        ' Initialize RibbonTabs
        CalcTab()
        GutterTab()
        HipValTab()

        ' Create Ribbon Panels
        Dim calculate As RibbonPanelSource = Sculpt.MakeRibbonPanel("Calculate", My.Resources.Jobber_L, ribbonTab, _calculate)
        Dim gutter As RibbonPanelSource = Sculpt.MakeRibbonPanel("Gutter Calc", My.Resources.GutCap_L, ribbonTab, _gutter)
        Dim hipValley As RibbonPanelSource = Sculpt.MakeRibbonPanel("Hip/Valley Calc", My.Resources.Hip_L, ribbonTab, _hipValley)

        ' Tag Ribbon 
        calculate.Tag = "Calculate Panel"
        gutter.Tag = "Gutter Panel"
        hipValley.Tag = "Hip/Valley Panel"

    End Sub

    ' METHOD: CALCTAB
    ' Creates The "Calculate" Ribbon Tab
    Private Sub CalcTab()

        Dim jobber As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Jobber", "Displays A Jobber Calculator For Use", My.Resources.Jobber_S, My.Resources.Jobber_L, "")
        Dim triSolv As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "TriSolv", "Displays A Triangle Solveing Calculator For Use", My.Resources.TriSolv_S, My.Resources.TriSolv_L, "")
        Dim bCount As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Bolt Count", "Displays A Count Of Bolts For Selected Members", My.Resources.BoltCount_S, My.Resources.BoltCount_L, "")
        Dim sCount As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Screw Count", "Displays A Count Of Screws For Selected Members", My.Resources.ScrewCount_S, My.Resources.ScrewCount_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(jobber)
        row.Items.Add(triSolv)
        row.Items.Add(bCount)
        row.Items.Add(sCount)

        _calculate.Add(row)

    End Sub

    ' METHOD: GUTTERTAB
    ' Creates The "Gutter" Ribbon Tab
    Private Sub GutterTab()

        Dim gutCap As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Capacity", "Displays A Gutter Capacity Calculator For Use", My.Resources.GutCap_S, My.Resources.GutCap_L, "")
        Dim elbow As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Elbow", "Displays An Elbow Sheet For Use", My.Resources.Elbow_S, My.Resources.Elbow_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(gutCap)
        row.Items.Add(elbow)

        _gutter.Add(row)

    End Sub

    ' METHOD: HIPVALTAB
    ' Creates The "HipValley" Ribbon Tab
    Private Sub HipValTab()

        Dim hipCalc As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Hip", "Displays A Hip Calculator For Use", My.Resources.Hip_S, My.Resources.Hip_L, "")
        Dim valleyCalc As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Valley", "Displays A Valley Calculator For Use", My.Resources.Valley_S, My.Resources.Valley_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(hipCalc)
        row.Items.Add(valleyCalc)

        _hipValley.Add(row)

    End Sub

End Class

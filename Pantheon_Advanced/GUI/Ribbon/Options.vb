Imports Autodesk.Windows

Public Class Options

    ' Method: CREATERIBBON
    ' Creates & Populates The "PANTHEON OPTIONS" Ribbon Tab
    Public Sub CreateRibbon(ByVal control As RibbonControl)

        ' Create Ribbon Control Object

        If (control IsNot Nothing) Then

            Dim ribbonTab As RibbonTab = control.FindTab("PANTHEONOPTIONS")

            If (ribbonTab IsNot Nothing) Then

                control.Tabs.Remove(ribbonTab)

            End If

            ribbonTab = Sculpt.MakeRibbonTab("Pantheon Options", "PantheonOptions")

            ' Add Ribbon To AutoCAD
            control.Tabs.Add(ribbonTab)

            ' Add Ribbon Components

            AddContent(ribbonTab)

        End If
    End Sub

    Dim _prefs As RibbonItemCollection = New RibbonItemCollection()
    Dim _manual As RibbonItemCollection = New RibbonItemCollection()
    Dim _tutorials As RibbonItemCollection = New RibbonItemCollection()
    Dim _about As RibbonItemCollection = New RibbonItemCollection()

    ' METHOD: ADDCONTENT
    ' Adds All Content To The Ribbon
    Private Sub AddContent(ByVal ribbonTab As RibbonTab)

        ' Initialize RibbonTabs
        PrefTab()
        ManualTab()
        TutTab()
        AbTab()

        ' Create Ribbon Panels
        Dim prefs As RibbonPanelSource = Sculpt.MakeRibbonPanel("Preferences", My.Resources.Settings_L, ribbonTab, _prefs)
        Dim manual As RibbonPanelSource = Sculpt.MakeRibbonPanel("Manual", My.Resources.Manual_L, ribbonTab, _manual)
        Dim tutorials As RibbonPanelSource = Sculpt.MakeRibbonPanel("Tutorials", My.Resources.ThreeD_L, ribbonTab, _tutorials)
        Dim about As RibbonPanelSource = Sculpt.MakeRibbonPanel("About Pantheon", My.Resources.About_L, ribbonTab, _about)

        ' Tag Ribbon 
        prefs.Tag = "Preference panel"
        manual.Tag = "Manual Panel"
        tutorials.Tag = "Tutorial Panel"
        about.Tag = "About Panel"

    End Sub

    ' METHOD: PREFTAB
    '  Creates The Preference Panel
    Private Sub PrefTab()

        Dim settings As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Settings", "Opens The Settings Panel", My.Resources.Settings_S, My.Resources.Settings_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(settings)

        _prefs.Add(row)

    End Sub

    ' METHOD: MANUALTAB
    '  Creates The Manual Panel
    Private Sub ManualTab()

        Dim manual As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Manual", "Opens The Pantheon Manual", My.Resources.Manual_S, My.Resources.Manual_L, "")
        Dim trouble As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Troubleshoot", "Troubleshoot Pantheon", My.Resources.Troubleshoot_S, My.Resources.Troubleshoot_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(manual)
        row.Items.Add(trouble)

        _manual.Add(row)

    End Sub

    ' METHOD: TUTTAB
    '  Creates The Tutorials Panel
    Private Sub TutTab()

        Dim twoD As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "2D", "View 2D Drafting Tutorials", My.Resources.TwoD_S, My.Resources.TwoD_L, "")
        Dim threeD As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "3D", "View 3D Modeling Tutorials", My.Resources.ThreeD_S, My.Resources.ThreeD_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(twoD)
        row.Items.Add(threeD)

        _tutorials.Add(row)

    End Sub

    ' METHOD: ABTAB
    '  Creates The About Panel
    Private Sub AbTab()

        Dim about As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "About", "Display Pantheon Information", My.Resources.About_S, My.Resources.About_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(about)

        _about.Add(row)

    End Sub

End Class

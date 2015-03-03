Imports Autodesk.Windows

Public Class Information

    Dim _customer As RibbonItemCollection = New RibbonItemCollection()
    Dim _quirks As RibbonItemCollection = New RibbonItemCollection()
    Dim _colors As RibbonItemCollection = New RibbonItemCollection()
    Dim _additions As RibbonItemCollection = New RibbonItemCollection()

    ' Method: CREATERIBBON
    ' Creates & Populates The "Information" Ribbon Tab
    Public Sub CreateRibbon(ByVal control As RibbonControl)

        ' Create Ribbon Control Object

        If (control IsNot Nothing) Then

            Dim ribbonTab As RibbonTab = control.FindTab("INFORMATION")

            If (ribbonTab IsNot Nothing) Then

                control.Tabs.Remove(ribbonTab)

            End If

            ribbonTab = Sculpt.MakeRibbonTab("Information", "Information")

            ' Add Ribbon To AutoCAD
            control.Tabs.Add(ribbonTab)

            ' Add Ribbon Components

            AddContent(ribbonTab)

        End If
    End Sub

    ' METHOD: ADDCONTENT
    ' Adds All Ribbon Content To Ribbon Tab
    Private Sub AddContent(ByVal ribbonTab As RibbonTab)

        'Initialize Ribbon Contents
        CustomerTab()
        ColorTab()
        AdditionTab()
        QuirkTab()

        ' Create Ribbon Panels
        Dim customer As RibbonPanelSource = Sculpt.MakeRibbonPanel("Customer", My.Resources.Cowell_L, ribbonTab, _customer)
        Dim colors As RibbonPanelSource = Sculpt.MakeRibbonPanel("Colors", My.Resources.Studio_L, ribbonTab, _colors)
        Dim additions As RibbonPanelSource = Sculpt.MakeRibbonPanel("Additions", My.Resources.AdditionStudio_L, ribbonTab, _additions)
        Dim quirks As RibbonPanelSource = Sculpt.MakeRibbonPanel("Quirks", My.Resources.QuirkSearch_L, ribbonTab, _quirks)

        ' Tag Ribbon 
        customer.Tag = "Customer Panel"
        colors.Tag = "Colors Panel"
        additions.Tag = "Additions Panel"
        quirks.Tag = "Quirks Panel"

    End Sub

    ' METHOD: CUSTOMERTAB
    ' Populates The Customer Tab In Ribbon Tab
    Private Sub CustomerTab()

        Dim companyField As RibbonCombo = New RibbonCombo()
        companyField.Description = "Company Logo To Provide On Title Block"
        companyField.ShowText = True

        Dim customerField As RibbonTextBox = New RibbonTextBox()
        customerField.Description = "Customer Name"

        Dim jobField As RibbonTextBox = New RibbonTextBox()
        jobField.Description = "Job Number"

        Dim break As RibbonPanelBreak = New RibbonPanelBreak()

        Dim cowellButton As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Cowell", "Insert Cowell Title Block",
                                                             My.Resources.Cowell_S, My.Resources.Cowell_L, " ")
        Dim whiteButton As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Mike White", "Insert Mike White Title Block",
                                                             My.Resources.White_S, My.Resources.White_L, " ")
        Dim rrButton As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Railroad Yard", "Insert Railroad",
                                                             My.Resources.RailRoad_S, My.Resources.RailRoad_L, " ")
        Dim blankButton As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Blank", "Insert Blank Title Block",
                                                             My.Resources.Blank_S, My.Resources.Blank_L, " ")

        cowellButton.ShowText = False
        whiteButton.ShowText = False
        rrButton.ShowText = False
        blankButton.ShowText = False

        companyField.Items.Add(cowellButton)
        companyField.Items.Add(whiteButton)
        companyField.Items.Add(rrButton)

        Dim row As RibbonRowPanel = New RibbonRowPanel()
        Dim hideRow As RibbonRowPanel = New RibbonRowPanel()
        Dim rowBreak As RibbonRowBreak = New RibbonRowBreak()

        row.Items.Add(companyField)
        row.Items.Add(rowBreak)
        row.Items.Add(customerField)
        row.Items.Add(rowBreak)
        row.Items.Add(jobField)

        hideRow.Items.Add(cowellButton)
        hideRow.Items.Add(whiteButton)
        hideRow.Items.Add(rrButton)
        hideRow.Items.Add(blankButton)

        _customer.Add(row)
        _customer.Add(break)
        _customer.Add(hideRow)

    End Sub

    ' METHOD: COLORTAB  
    ' Populates The Color Tab In Ribbon Tab
    Private Sub ColorTab()

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        Dim quirkSearch As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Color Studio", "Select Structure Colors & Sheeting Options", My.Resources.Studio_S,
                                                            My.Resources.Studio_L, " ")

        row.Items.Add(quirkSearch)

        _colors.Add(row)

    End Sub

    ' METHOD: ADDITIONTAB  
    ' Populates The Addition Tab In Ribbon Tab
    Private Sub AdditionTab()

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        Dim quirkSearch As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Addition Studio", "Select Various Additions & Steel Options", My.Resources.AdditionStudio_S,
                                                            My.Resources.AdditionStudio_L, " ")

        row.Items.Add(quirkSearch)

        _additions.Add(row)

    End Sub

    ' METHOD: QUIRKTAB  
    ' Populates The Quirk Tab In Ribbon Tab
    Private Sub QuirkTab()

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        Dim quirkSearch As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Quirk Search", "View And Apply Various Customer Quirks", My.Resources.QuirkSearch_S,
                                                            My.Resources.QuirkSearch_L, " ")

        row.Items.Add(quirkSearch)

        _quirks.Add(row)

    End Sub

End Class


Imports Autodesk.Windows

Public Class Generate
    ' METHOD: CREATERIBBON
    ' Creates The "Generate" Ribbon Tab
    Public Sub CreateRibbon(ByVal control As RibbonControl)

        ' Create Ribbon Control Object

        If (control IsNot Nothing) Then

            Dim ribbonTab As RibbonTab = control.FindTab("GENERATE")

            If (ribbonTab IsNot Nothing) Then

                control.Tabs.Remove(ribbonTab)

            End If

            ribbonTab = Sculpt.MakeRibbonTab("Generate", "Generate")

            ' Add Ribbon To AutoCAD
            control.Tabs.Add(ribbonTab)

            ' Add Ribbon Components

            AddContent(ribbonTab)

        End If
    End Sub

    Dim _eSheets As RibbonItemCollection = New RibbonItemCollection()
    Dim _details As RibbonItemCollection = New RibbonItemCollection()
    Dim _section As RibbonItemCollection = New RibbonItemCollection()
    Dim _order As RibbonItemCollection = New RibbonItemCollection()
    Dim _plot As RibbonItemCollection = New RibbonItemCollection()

    ' METHOD: ADDCONTENT
    ' Adds All Content To The Ribbon
    Private Sub AddContent(ByVal ribbonTab As RibbonTab)
        ' Initialize Ribbon Tabs
        EsheetTab()
        DetailTab()
        SectionTab()
        OrderTab()
        PlotTab()

        ' Create Ribbon Panels

        ' Dim eSheets As RibbonPanelSource = Sculpt.MakeRibbonPanel("E-Sheets", My.Resources.eSheetSingle_L, ribbonTab, _eSheets)
        Dim details As RibbonPanelSource = Sculpt.MakeRibbonPanel("Details", My.Resources.DetailSingle_L, ribbonTab, _details)
        Dim section As RibbonPanelSource = Sculpt.MakeRibbonPanel("Sections", My.Resources.SectionSingle_L, ribbonTab, _section)
        Dim order As RibbonPanelSource = Sculpt.MakeRibbonPanel("Orders", My.Resources.ShipSingle_L, ribbonTab, _order)
        ' Dim plot As RibbonPanelSource = Sculpt.MakeRibbonPanel("Plotting", My.Resources.PlotSingle_L, ribbonTab, _plot)

        ' Tag Ribbon 

        'eSheets.Tag = "E-Sheets Panel"
        details.Tag = "Details Panel"
        section.Tag = "Section Panel"
        order.Tag = "Orders Panel"
        ' plot.Tag = "Steel Frame Panel"

    End Sub

    ' METHOD: ESHEETTAB
    ' Creates The eSheet Tab
    Private Sub EsheetTab()

        Dim createSingle As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single", "Creates A Single E-Sheet Of Specified Frame Line", My.Resources.eSheetSingle_S, My.Resources.eSheetSingle_L, "")
        Dim createMultiple As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Multiple", "Creates Multiple E-Sheets Of Specified Frame Lines", My.Resources.eSheetMult_S, My.Resources.eSheetMult_L, "")
        Dim createAll As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "All", "Creates A All E-Sheets Of Each Frame Line", My.Resources.eSheetAll_S, My.Resources.eSheetAll_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        row.Items.Add(createSingle)
        row.Items.Add(break)
        row.Items.Add(createMultiple)
        row.Items.Add(break)
        row.Items.Add(createAll)

        _eSheets.Add(row)

    End Sub

    ' METHOD: DETAILTAB
    ' Creates The Detail Tab
    Private Sub DetailTab()

        Dim memberList As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Member List", "Displays A Customizable Member List", My.Resources.MemberList_S, My.Resources.MemberList_L, "")

        Dim genSingle As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single", "Generates A Single Detail Drawing Of Selected Member", My.Resources.DetailSingle_S, My.Resources.DetailSingle_L, "")
        Dim genMultiple As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Multiple", "Generates Multiple eDetail Drawings Of Selected Members", My.Resources.DetailMult_S, My.Resources.DetailMult_L, "")
        Dim genAll As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "All", "Generates All Detail Drawings Of Members Specified In Member List", My.Resources.DetailAll_S, My.Resources.DetailAll_L, "")


        Dim leftRow As RibbonRowPanel = New RibbonRowPanel()
        Dim rightRow As RibbonRowPanel = New RibbonRowPanel
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        leftRow.Items.Add(memberList)

        rightRow.Items.Add(genSingle)
        rightRow.Items.Add(break)
        rightRow.Items.Add(genMultiple)
        rightRow.Items.Add(break)
        rightRow.Items.Add(genAll)

        _details.Add(leftRow)
        _details.Add(rightRow)


    End Sub

    ' METHOD: SECTIONSTAB
    ' Creates The Section Tab
    Private Sub SectionTab()

        'Dim sectionBox As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Section Box", "Creates A Section Boundary For Generating Sections", My.Resources.SectionBox_S, My.Resources.SectionBox_L, "")

        ' Dim cutSingle As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single", "Cuts A Single Section Drawing Of A Selection Box Boundary", My.Resources.SectionSingle_S, My.Resources.SectionSingle_L, "")
        'Dim cutMultiple As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Multiple", "Cuts Multiple Section Drawings Of Selection Box Boundaries", My.Resources.SectionMult_S, My.Resources.SectionMult_L, "")
        ' Dim cutAll As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "All", "Cuts All Section Drawings Of Selection Box Boundaries", My.Resources.SectionAll_S, My.Resources.SectionAll_L, "")

        Dim studio As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Section Studio", "Opens The Section Studio", My.Resources.SectionStudio_S, My.Resources.SectionStudio_L, "")

        Dim leftRow As RibbonRowPanel = New RibbonRowPanel()
        ' Dim rightRow As RibbonRowPanel = New RibbonRowPanel()
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        'rightRow.Items.Add(cutSingle)
        'rightRow.Items.Add(break)
        'rightRow.Items.Add(cutMultiple)
        'rightRow.Items.Add(break)
        'rightRow.Items.Add(cutAll)

        ' leftRow.Items.Add(sectionBox)
        leftRow.Items.Add(studio)

        _section.Add(leftRow)
        ' _section.Add(rightRow)

    End Sub

    ' METHOD: ORDERTAB
    ' Creates The Order Tab
    Private Sub OrderTab()

        Dim shipList As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Shipping List", "Displays A Customizable Shipping List", My.Resources.ShipList_S, My.Resources.ShipList_L, "")

        Dim genSingle As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single", "Generates A Single Shipping Form For All Components Of A Type", My.Resources.ShipSingle_S, My.Resources.ShipSingle_L, "")
        Dim genMultiple As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Multiple", "Generates Multiple Shipping Forms For Selected Components", My.Resources.ShipMult_S, My.Resources.ShipMult_L, "")
        Dim genAll As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "All", "Generates All Shipping Forms For Pantheon Project", My.Resources.ShipAll_S, My.Resources.ShipAll_L, "")

        Dim leftRow As RibbonRowPanel = New RibbonRowPanel()
        Dim rightRow As RibbonRowPanel = New RibbonRowPanel()
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        leftRow.Items.Add(shipList)

        rightRow.Items.Add(genSingle)
        rightRow.Items.Add(break)
        rightRow.Items.Add(genMultiple)
        rightRow.Items.Add(break)
        rightRow.Items.Add(genAll)

        _order.Add(leftRow)
        _order.Add(rightRow)

    End Sub

    ' METHOD: PLOTTAB
    ' Creates The Plot Tab
    Private Sub PlotTab()

        Dim plotSetup As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Plotter", "Specify The Default Plotter", My.Resources.Plotter_S, My.Resources.Plotter_L, "")

        Dim pSingle As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single", "Prints A Member, List, eSheet, Or Detail", My.Resources.PlotSingle_S, My.Resources.PlotSingle_L, "")
        Dim pMultiple As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Multiple", "Prints Multiple Members, Lists, eSheets, And Details", My.Resources.PlotMult_S, My.Resources.PlotMult_L, "")
        Dim pAll As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "All", "Prints All Forms For This Pantheon Project", My.Resources.PlotAll_S, My.Resources.PlotAll_L, "")

        Dim leftRow As RibbonRowPanel = New RibbonRowPanel()
        Dim rightRow As RibbonRowPanel = New RibbonRowPanel()
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        leftRow.Items.Add(plotSetup)

        rightRow.Items.Add(pSingle)
        rightRow.Items.Add(break)
        rightRow.Items.Add(pMultiple)
        rightRow.Items.Add(break)
        rightRow.Items.Add(pAll)

        _plot.Add(leftRow)
        _plot.Add(rightRow)

    End Sub

End Class

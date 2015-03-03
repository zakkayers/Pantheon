
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.Windows

Public Class Design

    ' Method: CREATERIBBON
    ' Creates & Populates The "Design" Ribbon Tab
    Public Sub CreateRibbon(ByVal control As RibbonControl)

        ' Create Ribbon Control Object

        If (control IsNot Nothing) Then

            Dim ribbonTab As RibbonTab = control.FindTab("DESIGN")

            If (ribbonTab IsNot Nothing) Then

                control.Tabs.Remove(ribbonTab)

            End If

            ribbonTab = Sculpt.MakeRibbonTab("Design", "Design")

            ' Add Ribbon To AutoCAD
            control.Tabs.Add(ribbonTab)

            ' Add Ribbon Components

            AddContent(ribbonTab)

            ribbonTab.IsActive = True

        End If
    End Sub

    Dim _workframe As RibbonItemCollection = New RibbonItemCollection()
    Dim _foundation As RibbonItemCollection = New RibbonItemCollection()
    Dim _steelframe As RibbonItemCollection = New RibbonItemCollection()
    Dim _columns As RibbonItemCollection = New RibbonItemCollection()
    Dim _rafters As RibbonItemCollection = New RibbonItemCollection()
    Dim _overhangs As RibbonItemCollection = New RibbonItemCollection()
    Dim _posts As RibbonItemCollection = New RibbonItemCollection()
    Dim _beams As RibbonItemCollection = New RibbonItemCollection()
    Dim _openings As RibbonItemCollection = New RibbonItemCollection()
    Dim _braces As RibbonItemCollection = New RibbonItemCollection()
    Dim _views As RibbonItemCollection = New RibbonItemCollection()

    ' METHOD: ADDCONTENT
    ' Adds All Ribbon Content To Ribbon Tab
    Private Sub AddContent(ByVal ribbonTab As RibbonTab)

        ' Initialize Ribbon Contents
        ViewTab()
        WorkframeTab()
        FoundationTab()
        ' SteelFrameTab()
        ColumnTab()
        RafterTab()
        OverHangTab()
        PostTab()
        BeamTab()
        OpeningTab()
        BracesTab()


        ' Create Ribbon Panels
        Dim views As RibbonPanelSource = Sculpt.MakeRibbonPanel("View", My.Resources.WorkframeView_L, ribbonTab, _views)
        Dim workframe As RibbonPanelSource = Sculpt.MakeRibbonPanel("Work Frames", My.Resources.SymWork_L, ribbonTab, _workframe)
        Dim foundation As RibbonPanelSource = Sculpt.MakeRibbonPanel("Foundation", My.Resources.Slab_L, ribbonTab, _foundation)
        ' Dim steelFrame As RibbonPanelSource = Sculpt.MakeRibbonPanel("Steel Frames", My.Resources.SymFrame_L, ribbonTab, _steelframe)
        Dim columns As RibbonPanelSource = Sculpt.MakeRibbonPanel("Columns", My.Resources.StraightCol_L, ribbonTab, _columns)
        Dim rafters As RibbonPanelSource = Sculpt.MakeRibbonPanel("Rafters", My.Resources.StraightRaf_L, ribbonTab, _rafters)
        Dim overhangs As RibbonPanelSource = Sculpt.MakeRibbonPanel("OverHangs", My.Resources.CeeOver_L, ribbonTab, _overhangs)
        Dim posts As RibbonPanelSource = Sculpt.MakeRibbonPanel("Posts", My.Resources.CeeAngle_L, ribbonTab, _posts)
        Dim beams As RibbonPanelSource = Sculpt.MakeRibbonPanel("Beams", My.Resources.RolledIShape_L, ribbonTab, _beams)
        Dim openings As RibbonPanelSource = Sculpt.MakeRibbonPanel("Openings", My.Resources.DoorEndwall_L, ribbonTab, _openings)
        Dim braces As RibbonPanelSource = Sculpt.MakeRibbonPanel("Braces", My.Resources.Flange_L, ribbonTab, _braces)


        ' Tag Ribbon Panels
        views.Tag = "View Panel"
        workframe.Tag = "Workframe Panel"
        foundation.Tag = "Foundation Panel"
        ' steelFrame.Tag = "Steel Frame Panel"
        columns.Tag = "Column Panel"
        rafters.Tag = "Rafter Panel"
        overhangs.Tag = "OverHang Panel"
        posts.Tag = "Post Panel"
        beams.Tag = "Beam Panel"
        openings.Tag = "Opening Panel"
        braces.Tag = "Brace Panel"


    End Sub

    ' METHOD: VIEWTAB
    ' Creates The Views Tab
    Private Sub ViewTab()

        Dim wire As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Workframe", "Changes Layer State To Display Only WorkFrame Layers", My.Resources.WorkframeView_S, My.Resources.WorkframeView_L, " ")
        Dim steel As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Steel", "Changes Layer State To Display Steel Layers", My.Resources.Steelview_S, My.Resources.Steelview_L, " ")
        Dim sheets As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Sheets", "Changes Layer State To Display Only Sheeting Layers", My.Resources.SheetView_S, My.Resources.SheetView_L, " ")
        Dim eSheetView As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "eSheet View", "Converts Current Selected Members To eSheet View", My.Resources.eSheetView_S, My.Resources.eSheetView_L, "")
        Dim detailView As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Detail View", "Converts Current Selected Members To Detial View", My.Resources.DetailView_S, My.Resources.DetailView_L, "")

        Dim viewButtons As RibbonButton() = {eSheetView, detailView}

        Dim view As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", viewButtons)

        Dim buttons As RibbonButton() = {wire, steel, sheets}

        Dim views As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons)

        Dim rightRow As RibbonRowPanel = New RibbonRowPanel()
        Dim leftRow As RibbonRowPanel = New RibbonRowPanel()

        'rightRow.Items.Add(views)
        leftRow.Items.Add(view)


        '_views.Add(rightRow)
        _views.Add(leftRow)

    End Sub

    ' Method: WORKFRAMETAB
    ' Creates The Workframe Tab
    Private Sub WorkframeTab()

        Dim symmetricalWorkFrame As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Symmetrical", "Creates A Symmetrical Workframe",
                                                                     My.Resources.SymWork_S, My.Resources.SymWork_L, "_workframe _symmetrical ")
        Dim singleSlopeWorkFrame As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Single Slope", "Creates A Single Slope Workframe",
                                                                     My.Resources.SingleWork_S, My.Resources.SingleWork_L, "_workframe single ")
        Dim offCenWorkFrame As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Off-Centered Ridge", "Creates An Off-Centered Ridge Workframe",
                                                                     My.Resources.OffCenWork_S, My.Resources.OffCenWork_L, "_workframe offcenter ")

        Dim row As RibbonRowPanel = New RibbonRowPanel()
        Dim break As RibbonRowBreak = New RibbonRowBreak()

        row.Items.Add(symmetricalWorkFrame)
        row.Items.Add(break)
        row.Items.Add(singleSlopeWorkFrame)
        row.Items.Add(break)
        row.Items.Add(offCenWorkFrame)


        _workframe.Add(row)

    End Sub

    ' Method: FOUNDATIONTAB
    ' Creates The Foundation Tab
    Private Sub FoundationTab()

        Dim slabStandard As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Standard Slab", "Creates A Foundation Slab", My.Resources.Slab_S, My.Resources.Slab_L, "_slab _standard ")
        ' Dim slabSloped As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Sloped Slab", "Creates A Sloped Foundation Slab", My.Resources.SlopedSlab_S, My.Resources.SlopedSlab_L, "_slab _sloping ")

        Dim stem As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Stem Wall", "Creates A Stem Wall", My.Resources.Stem_S, My.Resources.Stem_L, " ")
        Dim brick As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Brick Wall", "Creates A Brick Wall", My.Resources.Brick_S, My.Resources.Brick_L, " ")
        Dim wood As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Wood Wall", "Creates A Wood Wall", My.Resources.Wood_S, My.Resources.Wood_L, " ")

        'Dim slabButtons As RibbonButton() = {slabStandard, slabSloped}
        Dim wallButtons As RibbonButton() = {stem, brick, wood}

        'Dim slabSplit As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", slabButtons)
        Dim wallSplit As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", wallButtons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(slabStandard)
        row.Items.Add(wallSplit)

        _foundation.Add(row)

    End Sub

    ' Method: STEELFRAMETAB
    ' Creates The Foundation Tab
    Private Sub SteelFrameTab()

        Dim gable As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Gable", "Creates A Steel Gabled Frame", My.Resources.SymFrame_S, My.Resources.SymFrame_L, " ")
        Dim lean As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Single Slope", "Creates A Steel Single Slope Frame", My.Resources.SingFrame_S, My.Resources.SingFrame_L, " ")
        Dim portal As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Portal", "Creates A Steel Portal Frame", My.Resources.PortFrame_S, My.Resources.PortFrame_L, " ")

        Dim buttons As RibbonButton() = {gable, lean, portal}

        Dim frame As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(frame)

        _steelframe.Add(row)

    End Sub

    ' Method: COLUMNTAB
    ' Creates The Column Tab
    Private Sub ColumnTab()

        Dim straight As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Straight", "Creates A Straight Column", My.Resources.StraightCol_S, My.Resources.StraightCol_L, " ")
        Dim taper As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Tapered", "Creates A Tapered Column", My.Resources.TaperCol_S, My.Resources.TaperCol_L, " ")
        Dim portal As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Portal", "Creates A Portal Column", My.Resources.PortalCol_S, My.Resources.PortalCol_L, " ")
        Dim support As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Support", "Creates A Support Column", My.Resources.SupportCol_S, My.Resources.SupportCol_L, " ")
        Dim wind As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Wind", "Creates A Wind Column", My.Resources.WindCol_S, My.Resources.WindCol_L, " ")
        Dim tube As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Tube", "Creates A Tube Column", My.Resources.TubeCol_S, My.Resources.TubeCol_L, " ")

        Dim buttons As RibbonButton() = {straight, taper, portal, support, wind, tube}

        Dim columns As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(columns)

        _columns.Add(row)

    End Sub

    ' Method: RAFTERTAB
    ' Creates The Rafter Tab
    Private Sub RafterTab()

        Dim straight As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Straight", "Creates A Straight Rafter", My.Resources.StraightRaf_S, My.Resources.StraightRaf_L, " ")
        Dim taper As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Tapered", "Creates A Tapered Rafter", My.Resources.TaperRaf_S, My.Resources.TaperRaf_L, " ")
        Dim portal As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Portal", "Creates A Portal Rafter", My.Resources.PortalRaf_S, My.Resources.PortalRaf_L, " ")
        Dim mixed As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mixed", "Creates A Mixed Size Rafter", My.Resources.MixRaf_S, My.Resources.MixRaf_L, " ")

        Dim cee As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee", "Creates A Cee Rafter", My.Resources.CeeRaf_S, My.Resources.CeeRaf_L, " ")
        Dim mill As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill", "Creates A Mill Rafter", My.Resources.MillRaf_S, My.Resources.MillRaf_L, " ")

        Dim buttons As RibbonButton() = {straight, taper, portal, mixed}
        Dim buttons2 As RibbonButton() = {cee, mill}

        Dim rafters As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons)
        Dim rafters2 As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons2)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(rafters)
        row.Items.Add(rafters2)

        _rafters.Add(row)

    End Sub

    ' METHOD: OVERHANGTAB
    ' Creates The OverHang Tab
    Private Sub OverHangTab()

        Dim ceeOver As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee", "Creates A Cee OverHang", My.Resources.CeeOver_S, My.Resources.CeeOver_L, " ")
        Dim millOVer As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill", "Creates A Mill OverHang Beam", My.Resources.MillOver_S, My.Resources.MillOver_L, " ")

        Dim buttons As RibbonButton() = {ceeOver, millOVer}

        Dim overHang As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", buttons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(overHang)

        _overhangs.Add(row)

    End Sub

    ' METHOD: POSTTAB
    ' Creates The Post Tab
    Private Sub PostTab()

        Dim ceeAngle As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee w/ Angle", "Creates A Cee Post With An Angle Connection", My.Resources.CeeAngle_S, My.Resources.CeeAngle_L, "")
        Dim ceeClip As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee w/ Clip", "Creates A Cee Post With A Clip Connection", My.Resources.CeeClip_S, My.Resources.CeeClip_L, "")
        Dim ceeBr2 As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee w/ BR1", "Creates A Cee Post With A BR2 Clip Connection", My.Resources.CeeBr1_S, My.Resources.CeeBr1_L, "")
        Dim ceeCenter As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee w/ Center", "Creates A Cee Post With A Center Post Connection", My.Resources.CeeCenter_S, My.Resources.CeeCenter_L, "")
        Dim ceeTurned As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee - Turned", "Creates A Turned Corner Cee Post", My.Resources.CeeTurned_S, My.Resources.CeeTurned_L, "")


        Dim millAngle As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill w/ Angle", "Creates A Mill Post With An Angle Connection", My.Resources.MillAngle_S, My.Resources.MillAngle_L, "")
        Dim millClip As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill w/ Clip", "Creates A Mill Post With A Clip Connection", My.Resources.MillClip_S, My.Resources.MillClip_L, "")
        Dim millBr2 As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill w/ BR1", "Creates A Mill Post With A BR2 Clip Connection", My.Resources.MillBr1_S, My.Resources.MillBr1_L, "")
        Dim millCenter As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill w/ Center", "Creates A Mill Post With A Center Post Connection", My.Resources.MillCenter_S, My.Resources.MillCenter_L, "")
        Dim millPortal As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Mill w/ Portal", "Creates A Mill Post With A Portal Rafter Connection", My.Resources.MillPortal_S, My.Resources.MillPortal_L, "")

        Dim ceeButtons As RibbonButton() = {ceeAngle, ceeClip, ceeBr2, ceeCenter, ceeTurned}
        Dim millButtons As RibbonButton() = {millAngle, millClip, millBr2, millCenter, millPortal}

        Dim cees As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", ceeButtons)
        Dim mills As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", millButtons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(cees)
        row.Items.Add(mills)

        _posts.Add(row)

    End Sub

    ' METHOD: BEAMTAB
    ' Creates The Post Tab
    Private Sub BeamTab()

        Dim wSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Rolled I Section", "Creates A Rolled I Section", My.Resources.RolledISect_S, My.Resources.RolledISect_L, "")
        Dim ceeSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee Section", "Creates A Cee Section", My.Resources.CeeSect_S, My.Resources.CeeSect_L, "")
        Dim channelSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Channel Section", "Creates A Channel Section", My.Resources.ChannelSect_S, My.Resources.ChannelSect_L, "")
        Dim angleSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Angle Section", "Creates An Angle Section", My.Resources.AngleSect_S, My.Resources.AngleSect_L, "")
        Dim tSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "T Section", "Creates A T Section", My.Resources.TSect_S, My.Resources.TSect_L, "")
        Dim tubeSection As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Tube Section", "Creates A Tube Section", My.Resources.TubeSect_S, My.Resources.TubeSect_L, "")

        Dim wShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Rolled I Shape", "Creates A Rolled I Shape", My.Resources.RolledIShape_S, My.Resources.RolledIShape_L, "")
        Dim ceeShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cee Shape", "Creates A Cee Shape", My.Resources.CeeShape_S, My.Resources.CeeShape_L, "")
        Dim channelShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Channel Shape", "Creates A Channel Shape", My.Resources.ChannelShape_S, My.Resources.ChannelShape_L, "")
        Dim angleShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Angle Shape", "Creates An Angle Shape", My.Resources.AngleShape_S, My.Resources.AngleShape_L, "")
        Dim tShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "T Shape", "Creates A T Shape", My.Resources.TShape_S, My.Resources.TShape_L, "")
        Dim tubeShape As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Tube Shape", "Creates A Tube Shape", My.Resources.TubeShape_S, My.Resources.TubeShape_L, "")

        Dim sectionButtons As RibbonButton() = {wSection, ceeSection, channelSection, angleSection, tSection, tubeSection}
        Dim shapeButtons As RibbonButton() = {wShape, ceeShape, channelShape, angleShape, tShape, tubeShape}

        Dim sections As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", sectionButtons)
        Dim shapes As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", shapeButtons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(sections)
        ' row.Items.Add(shapes)

        _beams.Add(row)

    End Sub

    ' METHOD: OPENINGTAB
    ' Creates The Openings Tab
    Private Sub OpeningTab()

        Dim doorEndwall As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Endwall", "Creates An Endwall Door Opening", My.Resources.DoorEndwall_S, My.Resources.DoorEndwall_L, "")
        Dim doorSidewall As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Sidewall", "Creates A Sidewall Door Opening", My.Resources.DoorSidewall_S, My.Resources.DoorSidewall_L, "")
        Dim doorUnder As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Under Girt", "Creates A Door Opening Under A Girt", My.Resources.DoorGirt_S, My.Resources.DoorGirt_L, "")

        Dim windowUnder As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Under Girt", "Creates A Window Opening Under A Girt", My.Resources.WindowGirt_S, My.Resources.WindowGirt_L, "")
        Dim windowBetween As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Between Girts", "Creates A Window Opening Between Girts", My.Resources.WindowBetweenGirt_S, My.Resources.WindowBetweenGirt_L, "")

        Dim doorButtons As RibbonButton() = {doorEndwall, doorSidewall, doorUnder}
        Dim windowButtons As RibbonButton() = {windowUnder, windowBetween}

        Dim door As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", doorButtons)
        Dim window As RibbonSplitButton = Sculpt.MakeSplitButton("Vertical", "Large", windowButtons)

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(door)
        row.Items.Add(window)

        _openings.Add(row)

    End Sub

    ' METHOD: BRACESTAB
    ' Creates The Braces Tab
    Private Sub BracesTab()

        Dim flange As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Flange", "Creates A Flange Brace", My.Resources.Flange_S, My.Resources.Flange_L, "")
        Dim cable As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cable", "Creates A Cable Brace", My.Resources.Cable_S, My.Resources.Cable_L, "")
        Dim stiffener As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Stiffener", "Creates A Stiffner", My.Resources.Stiffener_S, My.Resources.Stiffener_L, "")
        Dim cripple As RibbonButton = Sculpt.MakeButton("Vertical", "Large", "Cripples", "Creates A Pair Of Cripples", My.Resources.Cripples_S, My.Resources.Cripples_L, "")

        Dim row As RibbonRowPanel = New RibbonRowPanel()

        row.Items.Add(flange)
        row.Items.Add(cable)
        row.Items.Add(stiffener)
        row.Items.Add(cripple)

        _braces.Add(row)

    End Sub


End Class

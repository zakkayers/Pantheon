'      - PANTHEON -
' (C) Copyright 2015 by  
'     Zachariah Ayers  
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Windows
Imports Autodesk.Windows

' This line is not mandatory, but improves loading performances
<Assembly: CommandClass(GetType(Pantheon_Advanced.MyCommands))> 

Namespace Pantheon_Advanced
    ' This class is instantiated by AutoCAD for each document when
    ' a command is called by the user the first time in the context
    ' of a given document. In other words, non static data in this class
    ' is implicitly per-document!
    Public Class MyCommands
        ' ------------------ INTERFACE ------------------ '

        ' COMMAND: PANTHEON
        ' Initializes Pantheon Ribbon, Palette Set, & Functions
        <CommandMethod("PANTHEON")>
        Public Sub Pantheon()

            ' Create Ribbon
            Dim ribbon As RibbonControl = ComponentManager.Ribbon

            If (ribbon IsNot Nothing) Then

                ' Create Tab Objects
                Dim information As Information = New Information()
                Dim steelDesign As Design = New Design()
                Dim generate As Generate = New Generate()
                Dim tools As Tools = New Tools()
                Dim options As Options = New Options()

                ' Add Tabs To Ribbon
                information.CreateRibbon(ribbon)
                steelDesign.CreateRibbon(ribbon)
                generate.CreateRibbon(ribbon)
                tools.CreateRibbon(ribbon)
                options.CreateRibbon(ribbon)

                ' Add Items To Application Menu & Quick Access Toolbar
                Dim quick As QuickAccess = New QuickAccess()

                quick.AppMenu()
                quick.QuickToolbar()

                ' Create Palette
                PantheonPalette()

            End If
        End Sub

        ' COMMAND: PANTHEONPALETTE
        ' Initializes Pantheon Palette Set

        Friend Shared PaletteSet As PaletteSet = Nothing

        <CommandMethod("PANTHEONPALETT")>
        Public Sub PantheonPalette()

            If PaletteSet Is Nothing Then
                PaletteSet = New PaletteSet("")

                ' Create Shape Palette
                Dim shapePal As Shapes = New Shapes()
                PaletteSet.Add("Shapes", shapePal)

                ' Create Steel Palette
                Dim steelPal As Steel = New Steel()
                PaletteSet.Add("Steel", steelPal)

                ' Create Pitch Palette
                Dim pitPal As Pitch = New Pitch()
                PaletteSet.Add("Pitch", pitPal)

                ' Create Symbol Palette
                Dim symPal As Symbols = New Symbols()
                PaletteSet.Add("Symbols", symPal)

                ' Create Angle Palette
                Dim angPal As Angles = New Angles()
                PaletteSet.Add("Angles", angPal)

                ' Create Plate Palette
                Dim platePal As Plates = New Plates()
                PaletteSet.Add("Plates", platePal)

                ' Create Sheet Palette
                Dim panPal As Panels = New Panels()
                PaletteSet.Add("Panels", panPal)

                ' Create Trim Palette
                Dim trimPal As Trim = New Trim()
                PaletteSet.Add("Trim", trimPal)

                ' Create Fastener Palette
                Dim fastPal As Fasteners = New Fasteners()
                PaletteSet.Add("Fasteners", fastPal)

                PaletteSet.Size = symPal.Size

            End If

            PaletteSet.Visible = True
        End Sub

        ' ------------------ INITIALIZERS ------------------ '

        Shared ReadOnly ZeeShape As ZeeShape = New ZeeShape()
        Shared ReadOnly CeeShape As CeeShape = New CeeShape()
        Shared ReadOnly ECee As EaveShape = New EaveShape()

        ' ------------------ COMMANDS ------------------ '

        ' --- WORKFRAMES --- ' 

        ' COMMAND: WORKFRAME
        ' Initializes Selected Workframe Window & Draws Specified Workframe
        <CommandMethod("WORKFRAME")>
        Public Shared Sub WorkFrame()

            Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor

            Dim keywordOptions As PromptKeywordOptions = New PromptKeywordOptions(ControlChars.Lf + "Enter WorkFrame Type [SYMmetrical/SINgleslope/Offcenter]", "Symmetrical Singleslope Offcenter")
            Dim keywordResult As PromptResult = editor.GetKeywords(keywordOptions)
            If (keywordResult.Status = PromptStatus.OK) Then

                Select Case keywordResult.StringResult

                    Case "Symmetrical"

                        Dim form As New SymWorkFrameForm

                        form.Jolt()

                    Case "Singleslope"

                        Dim form As New SingWorkFrameForm

                        form.Jolt()

                    Case "Offcenter"

                        Dim form As New OffCenWorkFrameForm

                        form.Jolt()

                End Select

            End If
        End Sub

        ' --- FOUNDATION --- '

        ' COMMAND: SLAB
        ' Initializes The Slab Windows & Draws A Slab
        <CommandMethod("SLAB")>
        Public Shared Sub Slab()

            Dim form As New StandardSlabForm

            form.Jolt()
        End Sub

        ' COMMAND: WALL
        ' Initializes Selected Wall Window & Draws The Specified Wall
        <CommandMethod("WALL")>
        Public Shared Sub Wall()

            Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor

            Dim keywordOptions As PromptKeywordOptions = New PromptKeywordOptions(ControlChars.Lf + "Enter Wall Type [Brick/Stem/Wood]", "Brick Stem Wood")
            Dim keywordResult As PromptResult = editor.GetKeywords(keywordOptions)
            If (keywordResult.Status = PromptStatus.OK) Then

                Select Case keywordResult.StringResult

                    Case "Brick"

                        Dim form As New WallForm

                        form.Jolt("Brick Wall")

                    Case "Stem"

                        Dim form As New WallForm

                        form.Jolt("Stem Wall")

                    Case "Wood"

                        Dim form As New WallForm

                        form.Jolt("Wood Wall")

                End Select

            End If
        End Sub

        ' --- COLUMNS --- ' 

        ' COMMAND: COLUMN
        ' Initializes Selected Column Window & Draws The Specified Column
        <CommandMethod("COLUMN")>
        Public Shared Sub Column()

            Dim editor As Editor =
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor


            Dim keywordOptions As PromptKeywordOptions = New PromptKeywordOptions(ControlChars.Lf + "Enter Column Type [Portal/Straight/SUPport/Tapered/TUbe/Wind]", "Portal Straight Support Tapered Tube Wind")
            Dim keywordResult As PromptResult = editor.GetKeywords(keywordOptions)

            If (keywordResult.Status = PromptStatus.OK) Then

                Select Case keywordResult.StringResult

                    Case "Portal"

                        ' Dim form As New PortalColumnForm()


                    Case "Straight"

                        Dim form As New StraightColumnForm()
                        form.Jolt()

                    Case "Support"

                        ' TODO

                    Case "Tapered"

                        'Dim form As New TaperColumnForm()


                    Case "Tube"

                        ' TODO

                    Case "Wind"

                        ' TODO

                End Select

            End If

        End Sub


        ' --- SHAPES --- '

        ' COMMAND: ZEE
        ' Creates A Zee Shape In AutoCAD
        <CommandMethod("ZEE")>
        Public Shared Sub Zee()
            ZeeShape.Draw()
        End Sub

        ' COMMAND: CEE
        ' Creates A Cee Shape In AutoCAD
        <CommandMethod("Cee")>
        Public Shared Sub Cee()
            CeeShape.Draw()
        End Sub

        ' COMMAND: EAVECEE
        ' Creates An Eave Strut Cee Shape In AutoCAD
        <CommandMethod("EAVECEE")>
        Public Shared Sub EaveCee()
            eCee.Draw()
        End Sub

    End Class
End Namespace
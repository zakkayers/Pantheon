Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class OffCenWorkFrameObject

    Public Mark As String

    Public LeftPeak As Double
    Public LeftEave As Double
    Public LeftWidth As Double
    Public LeftLength As Double
    Public LeftPitch As Double
    Public LeftPurlin As Double
    Public LeftGirt As Double
    Public LeftFlush As Boolean
    Public LeftBypass As Boolean

    Public RightPeak As Double
    Public RightEave As Double
    Public RightWidth As Double
    Public RightLength As Double
    Public RightPitch As Double
    Public RightPurlin As Double
    Public RightGirt As Double
    Public RightFlush As Boolean
    Public RightBypass As Boolean

    Public SideBayText As String
    Public EndBayText As String
    Public SideBays() As Double
    Public EndBays() As Double

    Public Sub New(ByVal mark As String, ByVal leftEave As Double, ByVal leftWidth As Double, ByVal leftLength As Double,
                   ByVal leftPitch As Double, ByVal leftPurlin As Double, ByVal leftGirt As Double,
                   ByVal leftFlush As Boolean, ByVal leftBypass As Boolean,
                   ByVal rightEave As Double, ByVal rightWidth As Double, ByVal rightLength As Double,
                   ByVal rightPitch As Double, ByVal rightPurlin As Double, ByVal rightGirt As Double,
                   ByVal rightFlush As Boolean, ByVal rightBypass As Boolean, ByVal sideBayText As String, ByVal endBayText As String,
                   ByVal sideBays As Double(), ByVal endBays As Double())

        Me.Mark = mark

        Me.LeftEave = leftEave
        Me.LeftWidth = leftWidth
        Me.LeftLength = leftLength
        Me.LeftPitch = leftPitch
        Me.LeftPurlin = leftPurlin
        Me.LeftGirt = leftGirt
        Me.LeftFlush = leftFlush
        Me.LeftBypass = leftBypass

        Me.RightEave = rightEave
        Me.RightWidth = rightWidth
        Me.RightLength = rightLength
        Me.RightPitch = rightPitch
        Me.RightPurlin = rightPurlin
        Me.RightGirt = rightGirt
        Me.RightFlush = rightFlush
        Me.RightBypass = rightBypass

        Me.SideBayText = sideBayText
        Me.EndBayText = endBayText
        Me.SideBays = sideBays
        Me.EndBays = endBays

        Calculate.RunIn(leftWidth, leftPitch)
        LeftPeak = leftEave + Calculate.Rise

        Calculate.RunIn(rightWidth, rightPitch)
        RightPeak = rightEave + Calculate.Rise

    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Const layerName As String = "WorkFrame"
        Const layerDesc As String = "Standard WorkFrame Layer"
        Const layerColor As Integer = 20
        Const specialColor As Integer = 3
        Const offsetColor As Integer = 2

        ' Start Transaction

        Using doc.LockDocument()
            Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
            Try

                ' Set Layer To Specified Layer Name
                Dim ltb As LayerTable = DirectCast(transaction.GetObject(dwg.LayerTableId, OpenMode.ForRead), LayerTable)
                If Not ltb.Has(layerName) Then
                    ltb.UpgradeOpen()
                    Dim newLayer As New LayerTableRecord()
                    newLayer.Name = layerName
                    newLayer.Description = layerDesc
                    newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                    ltb.Add(newLayer)
                    transaction.AddNewlyCreatedDBObject(newLayer, True)
                End If

                dwg.Clayer = ltb(layerName)

                ' Create Block
                Dim bt As BlockTable
                bt = transaction.GetObject(dwg.BlockTableId, OpenMode.ForRead)

                If bt.Has(Mark) Then

                    editor.WriteMessage(ControlChars.Lf + "Member """ + Mark + """ Already Exists In Block Iserts.")

                Else

                    Dim entryOption As PromptPointOptions = New PromptPointOptions("Select Bottom Left Entry Point")
                    Dim entryResult As PromptPointResult = editor.GetPoint(entryOption)

                    If entryResult.Status = PromptStatus.OK Then

                        Using btr As New BlockTableRecord
                            btr.Name = Mark

                            ' Block Insertion Point

                            Dim entryPoint As Point3d = New Point3d(entryResult.Value.X, entryResult.Value.Y, entryResult.Value.Z)
                            btr.Origin = entryPoint

                            bt.UpgradeOpen()
                            Dim btrId As ObjectId = bt.Add(btr)
                            transaction.AddNewlyCreatedDBObject(btr, True)

                            ' RIGHT SIDE

                            ' Base
                            Dim rightBasePts As Point3d() = New Point3d() {entryPoint,
                                                                     New Point3d(entryPoint.X, entryPoint.Y + RightWidth, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + RightLength, entryPoint.Y + RightWidth, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + RightLength, entryPoint.Y, entryPoint.Z)}

                            Dim rightBaseFront As Line = New Line(rightBasePts(0), rightBasePts(3))
                            Dim rightBaseLeft As Line = New Line(rightBasePts(0), rightBasePts(1))
                            Dim rightBaseBack As Line = New Line(rightBasePts(1), rightBasePts(2))
                            Dim rightBaseRight As Line = New Line(rightBasePts(2), rightBasePts(3))

                            rightBaseFront.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            rightBaseLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            rightBaseBack.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            rightBaseRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)

                            btr.AppendEntity(rightBaseFront)
                            btr.AppendEntity(rightBaseLeft)
                            btr.AppendEntity(rightBaseBack)
                            btr.AppendEntity(rightBaseRight)

                            transaction.AddNewlyCreatedDBObject(rightBaseFront, True)
                            transaction.AddNewlyCreatedDBObject(rightBaseLeft, True)
                            transaction.AddNewlyCreatedDBObject(rightBaseBack, True)
                            transaction.AddNewlyCreatedDBObject(rightBaseRight, True)

                            ' FRONT ENDWALL

                            Dim rightFrontPts As Point3d() = New Point3d() {rightBasePts(1),
                                                                    New Point3d(rightBasePts(1).X, rightBasePts(1).Y, rightBasePts(1).Z + RightPeak),
                                                                    New Point3d(rightBasePts(0).X, rightBasePts(0).Y, rightBasePts(0).Z + RightEave),
                                                                     rightBasePts(0)}

                            Dim rightFrontLeft As Line = New Line(rightFrontPts(0), rightFrontPts(1))
                            Dim rightFrontLeftTop As Line = New Line(rightFrontPts(1), rightFrontPts(2))
                            Dim rightFrontRight As Line = New Line(rightFrontPts(2), rightFrontPts(3))

                            Solve.RunIn(RightPurlin, RightPitch)
                            Dim rightPurSlope = Solve.Slope

                            Dim rightPurPointLeft As Point3d
                            Dim rightPurPointRight As Point3d

                            If RightBypass Then

                                Solve.RunIn(RightGirt, RightPitch)
                                Dim girtRise = Solve.Rise

                                rightPurPointLeft = New Point3d(rightFrontPts(1).X, rightFrontPts(1).Y - RightGirt, rightFrontPts(1).Z - rightPurSlope + girtRise)
                                rightPurPointRight = New Point3d(rightFrontPts(2).X, rightFrontPts(2).Y + RightGirt, rightFrontPts(2).Z - rightPurSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(rightFrontPts(0).X, rightFrontPts(0).Y - RightGirt, rightFrontPts(0).Z), rightPurPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(rightFrontPts(3).X, rightFrontPts(3).Y + RightGirt, rightFrontPts(3).Z), rightPurPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                rightPurPointLeft = New Point3d(rightFrontPts(1).X, rightFrontPts(1).Y, rightFrontPts(1).Z - rightPurSlope)
                                rightPurPointRight = New Point3d(rightFrontPts(2).X, rightFrontPts(2).Y, rightFrontPts(2).Z - rightPurSlope)

                            End If

                            Dim rightPurLineLeft As Line = New Line(rightPurPointLeft, rightPurPointRight)

                            rightPurLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            rightFrontLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightFrontLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightFrontRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            btr.AppendEntity(rightFrontLeft)
                            btr.AppendEntity(rightFrontLeftTop)
                            btr.AppendEntity(rightFrontRight)
                            btr.AppendEntity(rightPurLineLeft)

                            transaction.AddNewlyCreatedDBObject(rightFrontLeft, True)
                            transaction.AddNewlyCreatedDBObject(rightFrontLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(rightFrontRight, True)
                            transaction.AddNewlyCreatedDBObject(rightPurLineLeft, True)


                            ' Back Endwall
                            Dim rightBackPts As Point3d() = New Point3d() {New Point3d(rightFrontPts(0).X + RightLength, rightFrontPts(0).Y, rightFrontPts(0).Z),
                                                                      New Point3d(rightFrontPts(1).X + RightLength, rightFrontPts(1).Y, rightFrontPts(1).Z),
                                                                      New Point3d(rightFrontPts(2).X + RightLength, rightFrontPts(2).Y, rightFrontPts(2).Z),
                                                                      New Point3d(rightFrontPts(3).X + RightLength, rightFrontPts(3).Y, rightFrontPts(3).Z)}

                            Dim rightBackLeft As Line = New Line(rightBackPts(0), rightBackPts(1))
                            Dim rightBackLeftTop As Line = New Line(rightBackPts(1), rightBackPts(2))
                            Dim rightBackRight As Line = New Line(rightBackPts(2), rightBackPts(3))

                            rightBackLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightBackLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightBackRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            If RightBypass Then

                                Solve.RunIn(RightGirt, RightPitch)
                                Dim girtRise = Solve.Rise

                                rightPurPointLeft = New Point3d(rightBackPts(1).X, rightBackPts(1).Y - RightGirt, rightBackPts(1).Z - rightPurSlope + girtRise)
                                rightPurPointRight = New Point3d(rightBackPts(2).X, rightBackPts(2).Y + RightGirt, rightBackPts(2).Z - rightPurSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(rightBackPts(0).X, rightBackPts(0).Y - RightGirt, rightBackPts(0).Z), rightPurPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(rightBackPts(3).X, rightBackPts(3).Y + RightGirt, rightBackPts(3).Z), rightPurPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                rightPurPointLeft = New Point3d(rightBackPts(1).X, rightBackPts(1).Y, rightBackPts(1).Z - rightPurSlope)
                                rightPurPointRight = New Point3d(rightBackPts(2).X, rightBackPts(2).Y, rightBackPts(2).Z - rightPurSlope)

                            End If

                            Dim rightBackPurLine As Line = New Line(rightPurPointLeft, rightPurPointRight)

                            rightBackPurLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            btr.AppendEntity(rightBackLeft)
                            btr.AppendEntity(rightBackLeftTop)
                            btr.AppendEntity(rightBackRight)
                            btr.AppendEntity(rightBackPurLine)

                            transaction.AddNewlyCreatedDBObject(rightBackLeft, True)
                            transaction.AddNewlyCreatedDBObject(rightBackLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(rightBackRight, True)
                            transaction.AddNewlyCreatedDBObject(rightBackPurLine, True)


                            ' Side Bays

                            Dim rightLastSideBay As Double = 0

                            For i = 0 To SideBays.Count() - 1

                                Dim sidePts As Point3d() = New Point3d() {New Point3d(rightFrontPts(0).X + SideBays(i) + rightLastSideBay, rightFrontPts(0).Y, rightFrontPts(0).Z),
                                                                      New Point3d(rightFrontPts(1).X + SideBays(i) + rightLastSideBay, rightFrontPts(1).Y, rightFrontPts(1).Z),
                                                                      New Point3d(rightFrontPts(2).X + SideBays(i) + rightLastSideBay, rightFrontPts(2).Y, rightFrontPts(2).Z),
                                                                      New Point3d(rightFrontPts(3).X + SideBays(i) + rightLastSideBay, rightFrontPts(3).Y, rightFrontPts(3).Z)}

                                Dim sideLeft As Line = New Line(sidePts(0), sidePts(1))
                                Dim sideLeftTop As Line = New Line(sidePts(1), sidePts(2))
                                Dim sideRight As Line = New Line(sidePts(2), sidePts(3))

                                sideLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                                If RightBypass Then

                                    Solve.RunIn(RightGirt, RightPitch)
                                    Dim girtRise = Solve.Rise

                                    rightPurPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y - RightGirt, sidePts(1).Z - rightPurSlope + girtRise)
                                    rightPurPointRight = New Point3d(sidePts(2).X, sidePts(2).Y + RightGirt, sidePts(2).Z - rightPurSlope + girtRise)

                                    Dim leftGirtLine As Line = New Line(New Point3d(sidePts(0).X, sidePts(0).Y - RightGirt, sidePts(0).Z), rightPurPointLeft)
                                    Dim rightGirtLine As Line = New Line(New Point3d(sidePts(3).X, sidePts(3).Y + RightGirt, sidePts(3).Z), rightPurPointRight)

                                    leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                    rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                    btr.AppendEntity(leftGirtLine)
                                    btr.AppendEntity(rightGirtLine)

                                    transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                    transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                                Else

                                    rightPurPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y, sidePts(1).Z - rightPurSlope)
                                    rightPurPointRight = New Point3d(sidePts(2).X, sidePts(2).Y, sidePts(2).Z - rightPurSlope)

                                End If

                                Dim sidePurLineLeft As Line = New Line(rightPurPointLeft, rightPurPointRight)

                                sidePurLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(sideLeft)
                                btr.AppendEntity(sideLeftTop)
                                btr.AppendEntity(sideRight)
                                btr.AppendEntity(sidePurLineLeft)

                                transaction.AddNewlyCreatedDBObject(sideLeft, True)
                                transaction.AddNewlyCreatedDBObject(sideLeftTop, True)
                                transaction.AddNewlyCreatedDBObject(sideRight, True)
                                transaction.AddNewlyCreatedDBObject(sidePurLineLeft, True)

                                rightLastSideBay += SideBays(i)
                            Next


                            ' ANNOTATIONS

                            Dim rightCount As Integer = 1

                            Dim rightAnnoAnchor As Point3d = New Point3d(rightFrontPts(3).X, rightFrontPts(3).Y - dwg.Dimtxt * 2, rightFrontPts(3).Z)
                            Dim rightAnnot As Line = New Line(rightFrontPts(0), rightAnnoAnchor)
                            Dim rightBubble As Circle = New Circle()
                            rightBubble.Diameter = dwg.Dimtxt * 1.5
                            Dim rightCircleAnchor As Point3d = New Point3d(rightAnnoAnchor.X, rightAnnoAnchor.Y - (rightBubble.Diameter / 2), rightAnnoAnchor.Z)
                            rightBubble.Center = rightCircleAnchor
                            rightBubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim rightText As MText = New MText()
                            rightText.SetDatabaseDefaults()
                            rightText.Location = rightCircleAnchor
                            rightText.TextStyleId = dwg.Textstyle
                            rightText.TextHeight = dwg.Dimtxt
                            rightText.Attachment = AttachmentPoint.MiddleCenter
                            rightText.SetAttachmentMovingLocation(rightText.Attachment)
                            rightText.Width = 0.0
                            rightText.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightText.Contents = "1"
                            rightCount += 1

                            rightLastSideBay = 0

                            btr.AppendEntity(rightAnnot)
                            btr.AppendEntity(rightBubble)
                            btr.AppendEntity(rightText)
                            transaction.AddNewlyCreatedDBObject(rightAnnot, True)
                            transaction.AddNewlyCreatedDBObject(rightBubble, True)
                            transaction.AddNewlyCreatedDBObject(rightText, True)

                            For i = 0 To SideBays.Count() - 2

                                If SideBays(i) + rightLastSideBay <> 0 Then

                                    Dim sideAnchor As Point3d = New Point3d(rightFrontPts(3).X + rightLastSideBay + SideBays(i), rightFrontPts(3).Y - dwg.Dimtxt * 2, rightFrontPts(3).Z)
                                    Dim sideAnnot As Line = New Line(New Point3d(rightFrontPts(0).X + rightLastSideBay + SideBays(i), rightFrontPts(0).Y, rightFrontPts(0).Z), sideAnchor)
                                    sideAnnot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    Dim sideBubble As Circle = New Circle()
                                    sideBubble.Diameter = dwg.Dimtxt * 1.5
                                    Dim sideCircleAnchor As Point3d = New Point3d(sideAnchor.X, sideAnchor.Y - (sideBubble.Diameter / 2), sideAnchor.Z)
                                    sideBubble.Center = sideCircleAnchor
                                    sideBubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    ' PIECE MARK
                                    Dim sideText As MText = New MText()
                                    sideText.SetDatabaseDefaults()
                                    sideText.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    sideText.Location = sideCircleAnchor
                                    sideText.TextStyleId = dwg.Textstyle
                                    sideText.TextHeight = dwg.Dimtxt
                                    sideText.Attachment = AttachmentPoint.MiddleCenter
                                    sideText.SetAttachmentMovingLocation(sideText.Attachment)
                                    sideText.Width = 0.0
                                    sideText.Contents = rightCount.ToString()

                                    btr.AppendEntity(sideAnnot)
                                    btr.AppendEntity(sideBubble)
                                    btr.AppendEntity(sideText)
                                    transaction.AddNewlyCreatedDBObject(sideAnnot, True)
                                    transaction.AddNewlyCreatedDBObject(sideBubble, True)
                                    transaction.AddNewlyCreatedDBObject(sideText, True)

                                    rightLastSideBay += SideBays(i)

                                    rightCount += 1

                                End If


                            Next

                            Dim rightBackAnchor As Point3d = New Point3d(rightBackPts(3).X, rightBackPts(3).Y - dwg.Dimtxt * 2, rightBackPts(3).Z)
                            Dim rightBackAnnot As Line = New Line(New Point3d(rightBackPts(0).X, rightBackPts(0).Y, rightBackPts(0).Z), rightBackAnchor)
                            rightBackAnnot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            Dim rightBackBubble As Circle = New Circle()
                            rightBackBubble.Diameter = dwg.Dimtxt * 1.5
                            Dim rightBackCircleAnchor As Point3d = New Point3d(rightBackAnchor.X, rightBackAnchor.Y - (rightBackBubble.Diameter / 2), rightBackAnchor.Z)
                            rightBackBubble.Center = rightBackCircleAnchor
                            rightBackBubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim rightBackText As MText = New MText()
                            rightBackText.SetDatabaseDefaults()
                            rightBackText.Location = rightBackCircleAnchor
                            rightBackText.TextStyleId = dwg.Textstyle
                            rightBackText.TextHeight = dwg.Dimtxt
                            rightBackText.Attachment = AttachmentPoint.MiddleCenter
                            rightBackText.SetAttachmentMovingLocation(rightBackText.Attachment)
                            rightBackText.Width = 0.0
                            rightBackText.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rightBackText.Contents = rightCount.ToString()

                            btr.AppendEntity(rightBackAnnot)
                            btr.AppendEntity(rightBackBubble)
                            btr.AppendEntity(rightBackText)
                            transaction.AddNewlyCreatedDBObject(rightBackAnnot, True)
                            transaction.AddNewlyCreatedDBObject(rightBackBubble, True)
                            transaction.AddNewlyCreatedDBObject(rightBackText, True)

                            ' LEFT SIDE

                            ' Base 
                            Dim basePts As Point3d() = New Point3d() {New Point3d(entryPoint.X, entryPoint.Y + RightWidth, entryPoint.Z),
                                                                     New Point3d(entryPoint.X, entryPoint.Y + LeftWidth, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + LeftLength, entryPoint.Y + LeftWidth, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + LeftLength, entryPoint.Y, entryPoint.Z)}

                            Dim baseFront As Line = New Line(basePts(0), basePts(3))
                            Dim baseLeft As Line = New Line(basePts(0), basePts(1))
                            Dim baseBack As Line = New Line(basePts(1), basePts(2))
                            Dim baseRight As Line = New Line(basePts(2), basePts(3))

                            baseFront.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            baseLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            baseBack.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
                            baseRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)

                            btr.AppendEntity(baseFront)
                            btr.AppendEntity(baseLeft)
                            btr.AppendEntity(baseBack)
                            btr.AppendEntity(baseRight)

                            transaction.AddNewlyCreatedDBObject(baseFront, True)
                            transaction.AddNewlyCreatedDBObject(baseLeft, True)
                            transaction.AddNewlyCreatedDBObject(baseBack, True)
                            transaction.AddNewlyCreatedDBObject(baseRight, True)

                            ' Front Endwall

                            Dim frontPts As Point3d() = New Point3d() {basePts(1),
                                                                      New Point3d(basePts(1).X, basePts(1).Y, basePts(1).Z + LeftEave),
                                                                      New Point3d(basePts(0).X, basePts(0).Y, basePts(0).Z + LeftPeak),
                                                                       basePts(0)}

                            Dim frontLeft As Line = New Line(frontPts(0), frontPts(1))
                            Dim frontLeftTop As Line = New Line(frontPts(1), frontPts(2))
                            Dim frontRight As Line = New Line(frontPts(2), frontPts(3))

                            Solve.RunIn(LeftPurlin, LeftPitch)
                            Dim purSlope = Solve.Slope

                            Dim purPointLeft As Point3d
                            Dim purPointRight As Point3d

                            If LeftBypass Then

                                Solve.RunIn(LeftGirt, LeftPitch)
                                Dim girtRise = Solve.Rise

                                purPointLeft = New Point3d(frontPts(1).X, frontPts(1).Y - LeftGirt, frontPts(1).Z - purSlope + girtRise)
                                purPointRight = New Point3d(frontPts(2).X, frontPts(2).Y + LeftGirt, frontPts(2).Z - purSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(frontPts(0).X, frontPts(0).Y - LeftGirt, frontPts(0).Z), purPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(frontPts(3).X, frontPts(3).Y + LeftGirt, frontPts(3).Z), purPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                purPointLeft = New Point3d(frontPts(1).X, frontPts(1).Y, frontPts(1).Z - purSlope)
                                purPointRight = New Point3d(frontPts(2).X, frontPts(2).Y, frontPts(2).Z - purSlope)

                            End If

                            Dim purLineLeft As Line = New Line(purPointLeft, purPointRight)

                            purLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            frontLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            frontLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            frontRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            btr.AppendEntity(frontLeft)
                            btr.AppendEntity(frontLeftTop)
                            btr.AppendEntity(frontRight)
                            btr.AppendEntity(purLineLeft)

                            transaction.AddNewlyCreatedDBObject(frontLeft, True)
                            transaction.AddNewlyCreatedDBObject(frontLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(frontRight, True)
                            transaction.AddNewlyCreatedDBObject(purLineLeft, True)

                            ' Back Endwall
                            Dim backPts As Point3d() = New Point3d() {New Point3d(frontPts(0).X + LeftLength, frontPts(0).Y, frontPts(0).Z),
                                                                      New Point3d(frontPts(1).X + LeftLength, frontPts(1).Y, frontPts(1).Z),
                                                                      New Point3d(frontPts(2).X + LeftLength, frontPts(2).Y, frontPts(2).Z),
                                                                      New Point3d(frontPts(3).X + LeftLength, frontPts(3).Y, frontPts(3).Z)}

                            Dim backLeft As Line = New Line(backPts(0), backPts(1))
                            Dim backLeftTop As Line = New Line(backPts(1), backPts(2))
                            Dim backRight As Line = New Line(backPts(2), backPts(3))

                            backLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            If LeftBypass Then

                                Solve.RunIn(LeftGirt, LeftPitch)
                                Dim girtRise = Solve.Rise

                                purPointLeft = New Point3d(backPts(1).X, backPts(1).Y - LeftGirt, backPts(1).Z - purSlope + girtRise)
                                purPointRight = New Point3d(backPts(2).X, backPts(2).Y + LeftGirt, backPts(2).Z - purSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(backPts(0).X, backPts(0).Y - LeftGirt, backPts(0).Z), purPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(backPts(3).X, backPts(3).Y + LeftGirt, backPts(3).Z), purPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                purPointLeft = New Point3d(backPts(1).X, backPts(1).Y, backPts(1).Z - purSlope)
                                purPointRight = New Point3d(backPts(2).X, backPts(2).Y, backPts(2).Z - purSlope)

                            End If

                            Dim backPurLine As Line = New Line(purPointLeft, purPointRight)

                            backPurLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            btr.AppendEntity(backLeft)
                            btr.AppendEntity(backLeftTop)
                            btr.AppendEntity(backRight)
                            btr.AppendEntity(backPurLine)

                            transaction.AddNewlyCreatedDBObject(backLeft, True)
                            transaction.AddNewlyCreatedDBObject(backLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(backRight, True)
                            transaction.AddNewlyCreatedDBObject(backPurLine, True)

                            ' Side Bays

                            Dim lastSideBay As Double = 0

                            For i = 0 To SideBays.Count() - 1

                                Dim sidePts As Point3d() = New Point3d() {New Point3d(frontPts(0).X + SideBays(i) + lastSideBay, frontPts(0).Y, frontPts(0).Z),
                                                                      New Point3d(frontPts(1).X + SideBays(i) + lastSideBay, frontPts(1).Y, frontPts(1).Z),
                                                                      New Point3d(frontPts(2).X + SideBays(i) + lastSideBay, frontPts(2).Y, frontPts(2).Z),
                                                                      New Point3d(frontPts(3).X + SideBays(i) + lastSideBay, frontPts(3).Y, frontPts(3).Z)}

                                Dim sideLeft As Line = New Line(sidePts(0), sidePts(1))
                                Dim sideLeftTop As Line = New Line(sidePts(1), sidePts(2))
                                Dim sideRight As Line = New Line(sidePts(2), sidePts(3))

                                sideLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                                If LeftBypass Then

                                    Solve.RunIn(LeftGirt, LeftPitch)
                                    Dim girtRise = Solve.Rise

                                    purPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y - LeftGirt, sidePts(1).Z - purSlope + girtRise)
                                    purPointRight = New Point3d(sidePts(2).X, sidePts(2).Y + LeftGirt, sidePts(2).Z - purSlope + girtRise)

                                    Dim leftGirtLine As Line = New Line(New Point3d(sidePts(0).X, sidePts(0).Y - LeftGirt, sidePts(0).Z), purPointLeft)
                                    Dim rightGirtLine As Line = New Line(New Point3d(sidePts(3).X, sidePts(3).Y + LeftGirt, sidePts(3).Z), purPointRight)

                                    leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                    rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                    btr.AppendEntity(leftGirtLine)
                                    btr.AppendEntity(rightGirtLine)

                                    transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                    transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                                Else

                                    purPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y, sidePts(1).Z - purSlope)
                                    purPointRight = New Point3d(sidePts(2).X, sidePts(2).Y, sidePts(2).Z - purSlope)

                                End If

                                Dim sidePurLineLeft As Line = New Line(purPointLeft, purPointRight)

                                sidePurLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(sideLeft)
                                btr.AppendEntity(sideLeftTop)
                                btr.AppendEntity(sideRight)
                                btr.AppendEntity(sidePurLineLeft)

                                transaction.AddNewlyCreatedDBObject(sideLeft, True)
                                transaction.AddNewlyCreatedDBObject(sideLeftTop, True)
                                transaction.AddNewlyCreatedDBObject(sideRight, True)
                                transaction.AddNewlyCreatedDBObject(sidePurLineLeft, True)

                                lastSideBay += SideBays(i)
                            Next


                            ' ANNOTATIONS

                            Dim count As Integer = 1

                            Dim annoAnchor As Point3d = New Point3d(frontPts(0).X, frontPts(0).Y + dwg.Dimtxt * 2, frontPts(0).Z)
                            Dim annot As Line = New Line(frontPts(3), annoAnchor)
                            Dim bubble As Circle = New Circle()
                            bubble.Diameter = dwg.Dimtxt * 1.5
                            Dim circleAnchor As Point3d = New Point3d(annoAnchor.X, annoAnchor.Y + (bubble.Diameter / 2), annoAnchor.Z)
                            bubble.Center = circleAnchor
                            bubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim text As MText = New MText()
                            text.SetDatabaseDefaults()
                            text.Location = circleAnchor
                            text.TextStyleId = dwg.Textstyle
                            text.TextHeight = dwg.Dimtxt
                            text.Attachment = AttachmentPoint.MiddleCenter
                            text.SetAttachmentMovingLocation(text.Attachment)
                            text.Width = 0.0
                            text.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            text.Contents = "1"
                            count += 1

                            lastSideBay = 0

                            btr.AppendEntity(annot)
                            btr.AppendEntity(bubble)
                            btr.AppendEntity(text)
                            transaction.AddNewlyCreatedDBObject(annot, True)
                            transaction.AddNewlyCreatedDBObject(bubble, True)
                            transaction.AddNewlyCreatedDBObject(text, True)

                            For i = 0 To SideBays.Count() - 2

                                If SideBays(i) + lastSideBay <> 0 Then

                                    Dim sideAnchor As Point3d = New Point3d(frontPts(0).X + lastSideBay + SideBays(i), frontPts(0).Y + dwg.Dimtxt * 2, frontPts(0).Z)
                                    Dim sideAnnot As Line = New Line(New Point3d(frontPts(3).X + lastSideBay + SideBays(i), frontPts(3).Y, frontPts(3).Z), sideAnchor)
                                    sideAnnot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    Dim sideBubble As Circle = New Circle()
                                    sideBubble.Diameter = dwg.Dimtxt * 1.5
                                    Dim sideCircleAnchor As Point3d = New Point3d(sideAnchor.X, sideAnchor.Y + (sideBubble.Diameter / 2), sideAnchor.Z)
                                    sideBubble.Center = sideCircleAnchor
                                    sideBubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    ' PIECE MARK
                                    Dim sideText As MText = New MText()
                                    sideText.SetDatabaseDefaults()
                                    sideText.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    sideText.Location = sideCircleAnchor
                                    sideText.TextStyleId = dwg.Textstyle
                                    sideText.TextHeight = dwg.Dimtxt
                                    sideText.Attachment = AttachmentPoint.MiddleCenter
                                    sideText.SetAttachmentMovingLocation(text.Attachment)
                                    sideText.Width = 0.0
                                    sideText.Contents = count.ToString()

                                    btr.AppendEntity(sideAnnot)
                                    btr.AppendEntity(sideBubble)
                                    btr.AppendEntity(sideText)
                                    transaction.AddNewlyCreatedDBObject(sideAnnot, True)
                                    transaction.AddNewlyCreatedDBObject(sideBubble, True)
                                    transaction.AddNewlyCreatedDBObject(sideText, True)

                                    lastSideBay += SideBays(i)

                                    count += 1

                                End If


                            Next

                            Dim backAnchor As Point3d = New Point3d(backPts(0).X, backPts(0).Y + dwg.Dimtxt * 2, backPts(0).Z)
                            Dim backAnnot As Line = New Line(New Point3d(backPts(3).X, backPts(3).Y, backPts(3).Z), backAnchor)
                            backAnnot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            Dim backBubble As Circle = New Circle()
                            backBubble.Diameter = dwg.Dimtxt * 1.5
                            Dim backCircleAnchor As Point3d = New Point3d(backAnchor.X, backAnchor.Y + (backBubble.Diameter / 2), backAnchor.Z)
                            backBubble.Center = backCircleAnchor
                            backBubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim backText As MText = New MText()
                            backText.SetDatabaseDefaults()
                            backText.Location = backCircleAnchor
                            backText.TextStyleId = dwg.Textstyle
                            backText.TextHeight = dwg.Dimtxt
                            backText.Attachment = AttachmentPoint.MiddleCenter
                            backText.SetAttachmentMovingLocation(text.Attachment)
                            backText.Width = 0.0
                            backText.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backText.Contents = count.ToString()

                            btr.AppendEntity(backAnnot)
                            btr.AppendEntity(backBubble)
                            btr.AppendEntity(backText)
                            transaction.AddNewlyCreatedDBObject(backAnnot, True)
                            transaction.AddNewlyCreatedDBObject(backBubble, True)
                            transaction.AddNewlyCreatedDBObject(backText, True)

                           

                            ' ENDBAYS
                            Dim endCount As Integer = 1

                            Dim lSideannoAnchor As Point3d = New Point3d(frontPts(0).X - dwg.Dimtxt * 2, frontPts(0).Y, frontPts(0).Z)
                            Dim lSideannot As Line = New Line(backPts(0), lSideannoAnchor)
                            lSideannot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            Dim lSidebubble As Circle = New Circle()
                            lSidebubble.Diameter = dwg.Dimtxt * 1.5
                            Dim lSidecircleAnchor As Point3d = New Point3d(lSideannoAnchor.X - (lSidebubble.Diameter / 2), lSideannoAnchor.Y, lSideannoAnchor.Z)
                            lSidebubble.Center = lSidecircleAnchor
                            lSidebubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim lSidetext As MText = New MText()
                            lSidetext.SetDatabaseDefaults()
                            lSidetext.Location = lSidecircleAnchor
                            lSidetext.TextStyleId = dwg.Textstyle
                            lSidetext.TextHeight = dwg.Dimtxt
                            lSidetext.Attachment = AttachmentPoint.MiddleCenter
                            lSidetext.SetAttachmentMovingLocation(text.Attachment)
                            lSidetext.Width = 0.0
                            lSidetext.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            lSidetext.Contents = Mind.AnnoCount(endCount)
                            endCount += 1

                            btr.AppendEntity(lSideannot)
                            btr.AppendEntity(lSidebubble)
                            btr.AppendEntity(lSidetext)
                            transaction.AddNewlyCreatedDBObject(lSideannot, True)
                            transaction.AddNewlyCreatedDBObject(lSidebubble, True)
                            transaction.AddNewlyCreatedDBObject(lSidetext, True)

                            Dim lastEndBay As Double = 0

                            For i = 0 To EndBays.Count() - 2

                                If EndBays(i) + lastEndBay <> 0 Then

                                    Dim endannoAnchor As Point3d = New Point3d(frontPts(0).X - dwg.Dimtxt * 2, frontPts(0).Y - EndBays(i) - lastEndBay, frontPts(0).Z)
                                    Dim endannot As Line = New Line(New Point3d(backPts(0).X, backPts(0).Y - EndBays(i) - lastEndBay, backPts(0).Z), endannoAnchor)
                                    endannot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    Dim endbubble As Circle = New Circle()
                                    endbubble.Diameter = dwg.Dimtxt * 1.5
                                    Dim endcircleAnchor As Point3d = New Point3d(endannoAnchor.X - (endbubble.Diameter / 2), endannoAnchor.Y, endannoAnchor.Z)
                                    endbubble.Center = endcircleAnchor
                                    endbubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                                    ' PIECE MARK
                                    Dim endtext As MText = New MText()
                                    endtext.SetDatabaseDefaults()
                                    endtext.Location = endcircleAnchor
                                    endtext.TextStyleId = dwg.Textstyle
                                    endtext.TextHeight = dwg.Dimtxt
                                    endtext.Attachment = AttachmentPoint.MiddleCenter
                                    endtext.SetAttachmentMovingLocation(text.Attachment)
                                    endtext.Width = 0.0
                                    endtext.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                    endtext.Contents = Mind.AnnoCount(endCount)
                                    endCount += 1

                                    btr.AppendEntity(endannot)
                                    btr.AppendEntity(endbubble)
                                    btr.AppendEntity(endtext)
                                    transaction.AddNewlyCreatedDBObject(endannot, True)
                                    transaction.AddNewlyCreatedDBObject(endbubble, True)
                                    transaction.AddNewlyCreatedDBObject(endtext, True)

                                    lastEndBay += EndBays(i)

                                End If

                            Next

                            Dim rSideannoAnchor As Point3d = New Point3d(frontPts(3).X - dwg.Dimtxt * 2, frontPts(3).Y, frontPts(3).Z)
                            Dim rSideannot As Line = New Line(backPts(3), rSideannoAnchor)
                            rSideannot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            Dim rSidebubble As Circle = New Circle()
                            rSidebubble.Diameter = dwg.Dimtxt * 1.5
                            Dim rSidecircleAnchor As Point3d = New Point3d(rSideannoAnchor.X - (rSidebubble.Diameter / 2), rSideannoAnchor.Y, rSideannoAnchor.Z)
                            rSidebubble.Center = rSidecircleAnchor
                            rSidebubble.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            ' PIECE MARK
                            Dim rSidetext As MText = New MText()
                            rSidetext.SetDatabaseDefaults()
                            rSidetext.Location = rSidecircleAnchor
                            rSidetext.TextStyleId = dwg.Textstyle
                            rSidetext.TextHeight = dwg.Dimtxt
                            rSidetext.Attachment = AttachmentPoint.MiddleCenter
                            rSidetext.SetAttachmentMovingLocation(text.Attachment)
                            rSidetext.Width = 0.0
                            rSidetext.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            rSidetext.Contents = Mind.AnnoCount(endCount)

                            btr.AppendEntity(rSideannot)
                            btr.AppendEntity(rSidebubble)
                            btr.AppendEntity(rSidetext)
                            transaction.AddNewlyCreatedDBObject(rSideannot, True)
                            transaction.AddNewlyCreatedDBObject(rSidebubble, True)
                            transaction.AddNewlyCreatedDBObject(rSidetext, True)




                            ' ADD Block Table Record To Block Table
                            Dim ms As BlockTableRecord = DirectCast(transaction.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), 
                                                        BlockTableRecord)

                            Dim br As New BlockReference(entryPoint, btrId)

                            ms.AppendEntity(br)
                            transaction.AddNewlyCreatedDBObject(br, True)

                        End Using
                    End If
                End If

                ' Open the active viewport
                Dim acVportTblRec As ViewportTableRecord
                acVportTblRec = transaction.GetObject(doc.Editor.ActiveViewportId, _
                                                  OpenMode.ForWrite)

                ' Rotate the view direction of the current viewport
                acVportTblRec.ViewDirection = New Vector3d(-1, -1, 1)
                doc.Editor.UpdateTiledViewportsFromDatabase()

                transaction.Commit()

                Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
                doc.SendStringToExecute("_zoom _extents ", True, False, False)

                editor.WriteMessage(ControlChars.Lf)

            Catch ex As Exception

                editor.WriteMessage("! A Problem Has Occured - " + ex.Message)
            Finally
                transaction.Dispose()
                editor.WriteMessage(ControlChars.Lf)
            End Try
        End Using

        ' End Transaction


    End Sub
End Class

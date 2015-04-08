
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime


Public Class SymWorkFrameObject

    Public Mark As String
    Public EaveHeight As Double
    Public Width As Double
    Public Length As Double
    Public Pitch As Double
    Public PurlinWeb As Double
    Public GirtWeb As Double

    Public SideBayText As String
    Public EndBayText As String
    Public SideBays As Double()
    Public EndBays As Double()

    Public Flush As Boolean
    Public Bypass As Boolean

    Public Sub New(ByVal mark As String, ByVal eaveHeight As Double, ByVal width As Double, ByVal length As Double,
                       ByVal pitch As Double, ByVal purlinWeb As Double, ByVal girtWeb As Double, ByVal sideBayText As String, ByVal endBayText As String,
                       ByVal sideBays As Double(), ByVal endBays As Double(),
                       ByVal flush As Boolean, ByVal bypass As Boolean)

        Me.EaveHeight = eaveHeight
        Me.Width = width
        Me.Length = length
        Me.Pitch = pitch
        Me.PurlinWeb = purlinWeb
        Me.GirtWeb = girtWeb
        Me.Flush = flush
        Me.Bypass = bypass

        Me.SideBayText = sideBayText
        Me.EndBayText = endBayText
        Me.SideBays = sideBays
        Me.EndBays = endBays

        Me.Mark = mark

    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Const layerName As String = "WorkFrames"
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

                            ' Base 
                            Dim basePts As Point3d() = New Point3d() {entryPoint,
                                                                     New Point3d(entryPoint.X, entryPoint.Y + Width, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + Length, entryPoint.Y + Width, entryPoint.Z),
                                                                     New Point3d(entryPoint.X + Length, entryPoint.Y, entryPoint.Z)}

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
                            Solve.RunIn(Width / 2, Pitch)
                            Dim peakRise = Solve.Rise + EaveHeight


                            Dim frontPts As Point3d() = New Point3d() {basePts(1),
                                                                      New Point3d(basePts(1).X, basePts(1).Y, basePts(1).Z + EaveHeight),
                                                                      New Point3d(basePts(1).X, basePts(1).Y - (Width / 2), basePts(1).Z + peakRise),
                                                                      New Point3d(basePts(0).X, basePts(0).Y, basePts(1).Z + EaveHeight),
                                                                       basePts(0)}
                            Dim frontLeft As Line = New Line(frontPts(0), frontPts(1))
                            Dim frontLeftTop As Line = New Line(frontPts(1), frontPts(2))
                            Dim frontRightTop As Line = New Line(frontPts(2), frontPts(3))
                            Dim frontRight As Line = New Line(frontPts(3), frontPts(4))

                            Solve.RunIn(PurlinWeb, Pitch)
                            Dim purSlope = Solve.Slope

                            Dim purPointLeft As Point3d
                            Dim purPointMid As Point3d
                            Dim purPointRight As Point3d

                            If Bypass Then

                                Solve.RunIn(GirtWeb, Pitch)
                                Dim girtRise = Solve.Rise

                                purPointLeft = New Point3d(frontPts(1).X, frontPts(1).Y - GirtWeb, frontPts(1).Z - purSlope + girtRise)
                                purPointMid = New Point3d(frontPts(2).X, frontPts(2).Y, frontPts(2).Z - purSlope)
                                purPointRight = New Point3d(frontPts(3).X, frontPts(3).Y + GirtWeb, frontPts(3).Z - purSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(frontPts(0).X, frontPts(0).Y - GirtWeb, frontPts(0).Z), purPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(frontPts(4).X, frontPts(4).Y + GirtWeb, frontPts(4).Z), purPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                purPointLeft = New Point3d(frontPts(1).X, frontPts(1).Y, frontPts(1).Z - purSlope)
                                purPointMid = New Point3d(frontPts(2).X, frontPts(2).Y, frontPts(2).Z - purSlope)
                                purPointRight = New Point3d(frontPts(3).X, frontPts(3).Y, frontPts(3).Z - purSlope)

                            End If

                            Dim purLineLeft As Line = New Line(purPointLeft, purPointMid)
                            Dim purLineRight As Line = New Line(purPointMid, purPointRight)


                            purLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                            purLineRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            frontLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            frontLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            frontRightTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            frontRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            btr.AppendEntity(frontLeft)
                            btr.AppendEntity(frontLeftTop)
                            btr.AppendEntity(frontRightTop)
                            btr.AppendEntity(frontRight)
                            btr.AppendEntity(purLineLeft)
                            btr.AppendEntity(purLineRight)

                            transaction.AddNewlyCreatedDBObject(frontLeft, True)
                            transaction.AddNewlyCreatedDBObject(frontLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(frontRightTop, True)
                            transaction.AddNewlyCreatedDBObject(frontRight, True)
                            transaction.AddNewlyCreatedDBObject(purLineLeft, True)
                            transaction.AddNewlyCreatedDBObject(purLineRight, True)

                            ' Back Endwall
                            Dim backPts As Point3d() = New Point3d() {New Point3d(frontPts(0).X + Length, frontPts(0).Y, frontPts(0).Z),
                                                                      New Point3d(frontPts(1).X + Length, frontPts(1).Y, frontPts(1).Z),
                                                                      New Point3d(frontPts(2).X + Length, frontPts(2).Y, frontPts(2).Z),
                                                                      New Point3d(frontPts(3).X + Length, frontPts(3).Y, frontPts(3).Z),
                                                                      New Point3d(frontPts(4).X + Length, frontPts(4).Y, frontPts(4).Z)}

                            Dim backLeft As Line = New Line(backPts(0), backPts(1))
                            Dim backLeftTop As Line = New Line(backPts(1), backPts(2))
                            Dim backRightTop As Line = New Line(backPts(2), backPts(3))
                            Dim backRight As Line = New Line(backPts(3), backPts(4))

                            backLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backRightTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                            backRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                            If Bypass Then

                                Solve.RunIn(GirtWeb, Pitch)
                                Dim girtRise = Solve.Rise

                                purPointLeft = New Point3d(backPts(1).X, backPts(1).Y - GirtWeb, backPts(1).Z - purSlope + girtRise)
                                purPointMid = New Point3d(backPts(2).X, backPts(2).Y, backPts(2).Z - purSlope)
                                purPointRight = New Point3d(backPts(3).X, backPts(3).Y + GirtWeb, backPts(3).Z - purSlope + girtRise)

                                Dim leftGirtLine As Line = New Line(New Point3d(backPts(0).X, backPts(0).Y - GirtWeb, backPts(0).Z), purPointLeft)
                                Dim rightGirtLine As Line = New Line(New Point3d(backPts(4).X, backPts(4).Y + GirtWeb, backPts(4).Z), purPointRight)

                                leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(leftGirtLine)
                                btr.AppendEntity(rightGirtLine)

                                transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                            Else

                                purPointLeft = New Point3d(backPts(1).X, backPts(1).Y, backPts(1).Z - purSlope)
                                purPointMid = New Point3d(backPts(2).X, backPts(2).Y, backPts(2).Z - purSlope)
                                purPointRight = New Point3d(backPts(3).X, backPts(3).Y, backPts(3).Z - purSlope)

                            End If

                            Dim backPurLineLeft As Line = New Line(purPointLeft, purPointMid)
                            Dim backPurLineRight As Line = New Line(purPointMid, purPointRight)

                            backPurLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                            backPurLineRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                            btr.AppendEntity(backLeft)
                            btr.AppendEntity(backLeftTop)
                            btr.AppendEntity(backRightTop)
                            btr.AppendEntity(backRight)
                            btr.AppendEntity(backPurLineLeft)
                            btr.AppendEntity(backPurLineRight)

                            transaction.AddNewlyCreatedDBObject(backLeft, True)
                            transaction.AddNewlyCreatedDBObject(backLeftTop, True)
                            transaction.AddNewlyCreatedDBObject(backRightTop, True)
                            transaction.AddNewlyCreatedDBObject(backRight, True)
                            transaction.AddNewlyCreatedDBObject(backPurLineLeft, True)
                            transaction.AddNewlyCreatedDBObject(backPurLineRight, True)

                            ' Side Bays

                            Dim lastSideBay As Double = 0

                            For i = 0 To SideBays.Count() - 1

                                Dim sidePts As Point3d() = New Point3d() {New Point3d(frontPts(0).X + SideBays(i) + lastSideBay, frontPts(0).Y, frontPts(0).Z),
                                                                      New Point3d(frontPts(1).X + SideBays(i) + lastSideBay, frontPts(1).Y, frontPts(1).Z),
                                                                      New Point3d(frontPts(2).X + SideBays(i) + lastSideBay, frontPts(2).Y, frontPts(2).Z),
                                                                      New Point3d(frontPts(3).X + SideBays(i) + lastSideBay, frontPts(3).Y, frontPts(3).Z),
                                                                      New Point3d(frontPts(4).X + SideBays(i) + lastSideBay, frontPts(4).Y, frontPts(4).Z)}

                                Dim sideLeft As Line = New Line(sidePts(0), sidePts(1))
                                Dim sideLeftTop As Line = New Line(sidePts(1), sidePts(2))
                                Dim sideRightTop As Line = New Line(sidePts(2), sidePts(3))
                                Dim sideRight As Line = New Line(sidePts(3), sidePts(4))

                                sideLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideLeftTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideRightTop.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
                                sideRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)

                                If Bypass Then

                                    Solve.RunIn(GirtWeb, Pitch)
                                    Dim girtRise = Solve.Rise

                                    purPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y - GirtWeb, sidePts(1).Z - purSlope + girtRise)
                                    purPointMid = New Point3d(sidePts(2).X, sidePts(2).Y, sidePts(2).Z - purSlope)
                                    purPointRight = New Point3d(sidePts(3).X, sidePts(3).Y + GirtWeb, sidePts(3).Z - purSlope + girtRise)

                                    Dim leftGirtLine As Line = New Line(New Point3d(sidePts(0).X, sidePts(0).Y - GirtWeb, sidePts(0).Z), purPointLeft)
                                    Dim rightGirtLine As Line = New Line(New Point3d(sidePts(4).X, sidePts(4).Y + GirtWeb, sidePts(4).Z), purPointRight)

                                    leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                    rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                    btr.AppendEntity(leftGirtLine)
                                    btr.AppendEntity(rightGirtLine)

                                    transaction.AddNewlyCreatedDBObject(leftGirtLine, True)
                                    transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

                                Else

                                    purPointLeft = New Point3d(sidePts(1).X, sidePts(1).Y, sidePts(1).Z - purSlope)
                                    purPointMid = New Point3d(sidePts(2).X, sidePts(2).Y, sidePts(2).Z - purSlope)
                                    purPointRight = New Point3d(sidePts(3).X, sidePts(3).Y, sidePts(3).Z - purSlope)

                                End If

                                Dim sidePurLineLeft As Line = New Line(purPointLeft, purPointMid)
                                Dim sidePurLineRight As Line = New Line(purPointMid, purPointRight)

                                sidePurLineLeft.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)
                                sidePurLineRight.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, offsetColor)

                                btr.AppendEntity(sideLeft)
                                btr.AppendEntity(sideLeftTop)
                                btr.AppendEntity(sideRightTop)
                                btr.AppendEntity(sideRight)
                                btr.AppendEntity(sidePurLineLeft)
                                btr.AppendEntity(sidePurLineRight)

                                transaction.AddNewlyCreatedDBObject(sideLeft, True)
                                transaction.AddNewlyCreatedDBObject(sideLeftTop, True)
                                transaction.AddNewlyCreatedDBObject(sideRightTop, True)
                                transaction.AddNewlyCreatedDBObject(sideRight, True)
                                transaction.AddNewlyCreatedDBObject(sidePurLineLeft, True)
                                transaction.AddNewlyCreatedDBObject(sidePurLineRight, True)

                                lastSideBay += SideBays(i)
                            Next


                            ' ANNOTATIONS

                            Dim count As Integer = 1

                            Dim annoAnchor As Point3d = New Point3d(frontPts(0).X, frontPts(0).Y + dwg.Dimtxt * 2, frontPts(0).Z)
                            Dim annot As Line = New Line(frontPts(4), annoAnchor)
                            annot.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, specialColor)
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
                                    Dim sideAnnot As Line = New Line(New Point3d(frontPts(4).X + lastSideBay + SideBays(i), frontPts(4).Y, frontPts(4).Z), sideAnchor)
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
                            Dim backAnnot As Line = New Line(New Point3d(backPts(4).X, backPts(4).Y, backPts(4).Z), backAnchor)
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

                            Dim rSideannoAnchor As Point3d = New Point3d(frontPts(4).X - dwg.Dimtxt * 2, frontPts(4).Y, frontPts(4).Z)
                            Dim rSideannot As Line = New Line(backPts(4), rSideannoAnchor)
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

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class StraightColumnObject

    Public Mark As String

    Public Flush As Boolean
    Public Bypass As Boolean

    Public GirtList As List(Of GirtObject)
    Public CableObj As CableObject
    Public AnchorObj As AnchorObject

    Public FlangeHoles As Boolean
    Public Block As Boolean

    Public EaveThick As Double
    Public EaveWidth As Double

    Public StiffThick As Double
    Public StiffWidth As Double

    Public OuterThick As Double
    Public OuterWidth As Double

    Public InnerThick As Double
    Public InnerWidth As Double

    Public WebThick As Double
    Public WebDepth As Double
    Public HaunchThick As Double
    Public HaunchWidth As Double
    Public HaunchLength As Double


    Public BaseThick As Double
    Public BaseWidth As Double


    Public Sub New(ByVal mark As String, ByVal flush As Boolean, ByVal bypass As Boolean,
                   girtList As List(Of GirtObject), cableObj As CableObject, anchorObj As AnchorObject,
                   ByVal flangeHoles As Boolean, ByVal block As Boolean,
                   ByVal eaveThick As Double, ByVal eaveWidth As Double, ByVal stiffThick As Double, ByVal stiffWidth As Double,
                   ByVal outerThick As Double, ByVal outerWidth As Double, ByVal innerThick As Double, ByVal innerWidth As Double,
                   ByVal webThick As Double, ByVal webDepth As Double, ByVal haunchThick As Double, ByVal haunchWidth As Double, ByVal haunchLength As Double,
                   ByVal baseThick As Double, ByVal baseWidth As Double)

        Me.Mark = mark

        Me.Flush = flush
        Me.Bypass = bypass
        Me.GirtList = girtList
        Me.CableObj = cableObj
        Me.AnchorObj = anchorObj
        Me.FlangeHoles = flangeHoles
        Me.Block = block
        Me.EaveThick = eaveThick
        Me.EaveWidth = eaveWidth
        Me.StiffThick = stiffThick
        Me.StiffWidth = stiffWidth
        Me.OuterThick = outerThick
        Me.OuterWidth = outerWidth
        Me.InnerThick = innerThick
        Me.InnerWidth = innerWidth
        Me.WebThick = webThick
        Me.WebDepth = webDepth
        Me.HaunchThick = haunchThick
        Me.HaunchWidth = haunchWidth
        Me.HaunchLength = haunchLength
        Me.BaseThick = baseThick
        Me.BaseWidth = baseWidth

    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Const eSheetLayerName As String = "Columns - E-Sheet View"
        Const eSheetLayerDesc As String = "E-Sheet View Of Columns"
        Const eSheetLayerColor As Integer = 7

        Const detailLayerName As String = "Columns - Detail View"
        Const detailLayerDesc As String = "Detail View Of Columns"
        Const detailLayerColor As Integer = 7

        Dim baseOptions As PromptPointOptions = New PromptPointOptions(ControlChars.Lf + "Select Base Point : ")
        Dim baseResults As PromptPointResult = editor.GetPoint(baseOptions)

        If (baseResults.Status = PromptStatus.OK) Then

            Dim eaveOptions As PromptPointOptions = New PromptPointOptions(ControlChars.Lf + "Select Eave Point : ")
            Dim eaveResults As PromptPointResult = editor.GetPoint(eaveOptions)

            If (eaveResults.Status = PromptStatus.OK) Then

                Dim pitchOptions As PromptPointOptions =
                        New PromptPointOptions(ControlChars.Lf + "Select Pitch Point : ")
                Dim pitchResults As PromptPointResult = editor.GetPoint(pitchOptions)

                If (pitchResults.Status = PromptStatus.OK) Then

                    ' Start Transaction

                    Using doc.LockDocument()
                        Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                        Try

                            ' Create E-Sheet & Detail Layers
                            Dim ltb As LayerTable = DirectCast(transaction.GetObject(dwg.LayerTableId, OpenMode.ForRead), LayerTable)
                            If Not ltb.Has(eSheetLayerName) Then
                                ltb.UpgradeOpen()
                                Dim newLayer As New LayerTableRecord()
                                newLayer.Name = eSheetLayerName
                                newLayer.Description = eSheetLayerDesc
                                newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, eSheetLayerColor)
                                ltb.Add(newLayer)
                                transaction.AddNewlyCreatedDBObject(newLayer, True)
                            End If

                            If Not ltb.Has(detailLayerName) Then
                                ltb.UpgradeOpen()
                                Dim newLayer As New LayerTableRecord()
                                newLayer.Name = detailLayerName
                                newLayer.Description = detailLayerDesc
                                newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, detailLayerColor)
                                ltb.Add(newLayer)
                                transaction.AddNewlyCreatedDBObject(newLayer, True)
                            End If

                            ' Create Block
                            Dim bt As BlockTable
                            bt = transaction.GetObject(dwg.BlockTableId, OpenMode.ForRead)


                            Using btr As New BlockTableRecord
                                btr.Name = Mark

                                ' Block Insertion Point
                                Dim entryPoint As Point3d = New Point3d(baseResults.Value.X, baseResults.Value.Y, 0)
                                btr.Origin = entryPoint

                                bt.UpgradeOpen()
                                Dim btrId As ObjectId = bt.Add(btr)
                                transaction.AddNewlyCreatedDBObject(btr, True)

                                ' Draw Column

                                ' Set To E-Sheet View
                                dwg.Clayer = ltb(eSheetLayerName)

                                Dim basePoint As Point2d = New Point2d(baseResults.Value.X, baseResults.Value.Y)
                                Dim eavePoint As Point2d = New Point2d(eaveResults.Value.X, eaveResults.Value.Y)
                                Dim pitchPoint As Point2d = New Point2d(pitchResults.Value.X, pitchResults.Value.Y)

                                If (pitchPoint.Y >= eavePoint.Y) Then

                                    If (pitchPoint.X > basePoint.X) Then

                                        If (Flush) Then

                                            ' Straight Standard Flush

                                            Dim eaveHeight = eavePoint.Y - basePoint.Y
                                            Dim pitch = TriSolv.GetTruePitch(eavePoint.Y, pitchPoint.Y,
                                                                     pitchPoint.X - eavePoint.X)

                                            ' Base Plate

                                            Dim baseBotLeft As Point2d = basePoint
                                            Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + BaseThick)
                                            Dim baseTopRight As Point2d = New Point2d(baseBotLeft.X + (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y + BaseThick)
                                            Dim baseBotRight As Point2d = New Point2d(baseBotLeft.X + (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y)

                                            Dim basePlate As Polyline = New Polyline

                                            basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                            basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                            basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                            ' Left Flange

                                            Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + BaseThick + 0.0625)

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveCapSlope = TriSolv.Slope

                                            Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - 0.0625)
                                            Dim lFlangeTopRight As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                         basePoint.Y + eaveHeight -
                                                                                         eaveCapSlope - 0.0625)
                                            Dim lFlangeBotRight As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                         basePoint.Y + BaseThick +
                                                                                         0.0625)

                                            Dim leftFlange As Polyline = New Polyline

                                            leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                            ' Web

                                            Dim webBotLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                    basePoint.Y + BaseThick)

                                            TriSolv.RunIn(OuterThick, pitch)
                                            Dim leftFlangeRise = TriSolv.Rise

                                            TriSolv.RunIn(WebDepth, pitch)
                                            Dim webRise = TriSolv.Rise

                                            Dim webTopLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                    basePoint.Y + eaveHeight -
                                                                                    eaveCapSlope + leftFlangeRise)
                                            Dim webTopRight As Point2d = New Point2d(webTopLeft.X + WebDepth,
                                                                                     webTopLeft.Y + webRise)
                                            Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                     basePoint.Y + BaseThick)

                                            Dim web As Polyline = New Polyline

                                            web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                            web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                            web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                            web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                            web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                            ' Haunch Plate

                                            TriSolv.RunIn(HaunchThick, pitch)
                                            Dim haunchRise = TriSolv.Rise

                                            Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight +
                                                                                       leftFlangeRise + webRise +
                                                                                       haunchRise + 3 -
                                                                                       HaunchLength)
                                            Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight +
                                                                                       leftFlangeRise + webRise +
                                                                                       haunchRise + 3)
                                            Dim haunchTopRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight +
                                                                                        webRise + leftFlangeRise +
                                                                                        haunchRise + 3)
                                            Dim haunchBotRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight +
                                                                                        webRise + leftFlangeRise +
                                                                                        haunchRise + 3 -
                                                                                        HaunchLength)

                                            Dim haunch As Polyline = New Polyline

                                            haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                            haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                            haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                            haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                            haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                            ' Right Flange

                                            Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                        webBotRight.Y + 0.0625)
                                            Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                        haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeTopRight As Point2d =
                                                    New Point2d(haunchBotLeft.X + InnerThick,
                                                                haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeBotRight As Point2d =
                                                    New Point2d(webBotRight.X + InnerThick, webBotRight.Y + 0.0625)

                                            Dim rightFlange As Polyline = New Polyline

                                            rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                            ' Eave Cap

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveRise = TriSolv.Rise

                                            TriSolv.RunIn(eaveRise, pitch)
                                            Dim yTol = TriSolv.Rise

                                            Dim eaveBotLeft As Point2d = New Point2d(baseBotLeft.X + eaveRise,
                                                                                     baseBotLeft.Y + eaveHeight -
                                                                                     eaveCapSlope + yTol)
                                            Dim eaveTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + eaveHeight)
                                            Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 - haunchRise)
                                            Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 - haunchRise -
                                                                                      eaveCapSlope)

                                            Dim eaveCap As Polyline = New Polyline

                                            eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                            ' Stiffener

                                            Dim stiffBotLeft As Point2d =
                                                    New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                haunchTopRight.Y - 3 - (HaunchLength - 6))
                                            Dim stiffTopLeft As Point2d =
                                                    New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                haunchTopRight.Y + StiffThick - 3 -
                                                                (HaunchLength - 6))
                                            Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                       haunchTopRight.Y + StiffThick - 3 -
                                                                                       (HaunchLength - 6))
                                            Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                       haunchTopRight.Y - 3 -
                                                                                       (HaunchLength - 6))

                                            Dim stiffener As Polyline = New Polyline

                                            stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                            stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                            stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                            btr.AppendEntity(basePlate)
                                            btr.AppendEntity(leftFlange)
                                            btr.AppendEntity(web)
                                            btr.AppendEntity(haunch)
                                            btr.AppendEntity(rightFlange)
                                            btr.AppendEntity(eaveCap)
                                            btr.AppendEntity(stiffener)

                                            transaction.AddNewlyCreatedDBObject(basePlate, True)
                                            transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                            transaction.AddNewlyCreatedDBObject(web, True)
                                            transaction.AddNewlyCreatedDBObject(haunch, True)
                                            transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                            transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                            transaction.AddNewlyCreatedDBObject(stiffener, True)

                                            ' Girts 

                                            For i = 0 To GirtList.Count - 1

                                                If GirtList.Item(i).Add Then

                                                    Select Case GirtList.Item(i).Type

                                                        Case "8"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X + 8, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "10"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X + 10, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "8"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X + 8, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                        Case "10"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X + 10, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                    End Select


                                                    If GirtList.Item(i).Brace Then

                                                        Dim braceLeft As Point3d = New Point3d(basePoint.X + 4, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                        Dim braceRight As Point3d = New Point3d(basePoint.X + OuterThick + WebDepth, braceLeft.Y, 0)

                                                        Dim brace As Line = New Line(braceLeft, braceRight)

                                                        Dim text As MText = New MText()
                                                        text.SetDatabaseDefaults()
                                                        text.Location = New Point3d(braceRight.X + InnerThick + 1, braceRight.Y, 0)
                                                        text.TextStyleId = dwg.Textstyle
                                                        text.TextHeight = dwg.Dimtxt
                                                        text.Attachment = AttachmentPoint.MiddleLeft
                                                        text.SetAttachmentMovingLocation(text.Attachment)
                                                        text.Width = 0.0

                                                        text.Contents = "FBX"

                                                        btr.AppendEntity(brace)
                                                        btr.AppendEntity(text)
                                                        transaction.AddNewlyCreatedDBObject(brace, True)
                                                        transaction.AddNewlyCreatedDBObject(text, True)

                                                    End If

                                                End If

                                            Next

                                            ' Cable Holes

                                            If CableObj.Check Then

                                                Dim topHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                Dim bottomHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                Dim topSlot As Circle = New Circle()
                                                Dim botSlot As Circle = New Circle()

                                                topSlot.Center = topHolePoint
                                                botSlot.Center = bottomHolePoint

                                                topSlot.Diameter = 1
                                                botSlot.Diameter = 1

                                                btr.AppendEntity(topSlot)
                                                btr.AppendEntity(botSlot)

                                                transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                transaction.AddNewlyCreatedDBObject(botSlot, True)

                                            End If

                                        Else

                                            ' Straight Standard Bypass

                                            Dim girtOptions As PromptPointOptions =
                                                    New PromptPointOptions(
                                                        ControlChars.Lf + "Select Outer Steel Line : ")
                                            Dim girtResult As PromptPointResult = editor.GetPoint(girtOptions)

                                            If (girtResult.Status = PromptStatus.OK) Then

                                                Dim eaveHeight = eavePoint.Y - basePoint.Y
                                                Dim girtOffset = Math.Abs(basePoint.X - girtResult.Value.X)
                                                Dim pitch = TriSolv.GetTruePitch(eavePoint.Y, pitchPoint.Y,
                                                                         pitchPoint.X - eavePoint.X)

                                                TriSolv.RunIn(girtOffset - 0.25, pitch)
                                                Dim offSetRise = TriSolv.Rise


                                                ' Base Plate

                                                Dim baseBotLeft As Point2d = basePoint
                                                Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                         baseBotLeft.Y + BaseThick)
                                                Dim baseTopRight As Point2d =
                                                        New Point2d(baseBotLeft.X + (OuterThick +
                                                                                     WebDepth +
                                                                                     InnerThick),
                                                                    baseTopLeft.Y)
                                                Dim baseBotRight As Point2d = New Point2d(baseTopRight.X, baseBotLeft.Y)

                                                Dim basePlate As Polyline = New Polyline

                                                basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                                basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                                basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                                ' Left Flange

                                                Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + BaseThick +
                                                                                            0.0625)

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveCapSlope = TriSolv.Slope

                                                Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + eaveHeight -
                                                                                            eaveCapSlope - 0.0625)
                                                Dim lFlangeTopRight As Point2d =
                                                        New Point2d(basePoint.X + OuterThick,
                                                                    basePoint.Y + eaveHeight -
                                                                    eaveCapSlope - 0.0625)
                                                Dim lFlangeBotRight As Point2d =
                                                        New Point2d(basePoint.X + OuterThick,
                                                                    basePoint.Y + BaseThick +
                                                                    0.0625)

                                                Dim leftFlange As Polyline = New Polyline

                                                leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                                ' Web

                                                Dim webBotLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                        basePoint.Y + BaseThick)

                                                TriSolv.RunIn(OuterThick, pitch)
                                                Dim leftFlangeRise = TriSolv.Rise

                                                TriSolv.RunIn(WebDepth, pitch)
                                                Dim webRise = TriSolv.Rise

                                                Dim webTopLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope + leftFlangeRise)
                                                Dim webTopRight As Point2d = New Point2d(webTopLeft.X + WebDepth,
                                                                                         webTopLeft.Y + webRise)
                                                Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                         basePoint.Y + BaseThick)

                                                Dim web As Polyline = New Polyline

                                                web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                                web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                                web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                                web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                                web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                                ' Haunch Plate

                                                TriSolv.RunIn(HaunchThick, pitch)
                                                Dim haunchRise = TriSolv.Rise

                                                Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight +
                                                                                           leftFlangeRise + webRise +
                                                                                           haunchRise + 3 -
                                                                                           HaunchLength)
                                                Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight +
                                                                                           leftFlangeRise + webRise +
                                                                                           haunchRise + 3)
                                                Dim haunchTopRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight +
                                                                                            webRise + leftFlangeRise +
                                                                                            haunchRise + 3)
                                                Dim haunchBotRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight +
                                                                                            webRise + leftFlangeRise +
                                                                                            haunchRise + 3 -
                                                                                            HaunchLength)

                                                Dim haunch As Polyline = New Polyline

                                                haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                                haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                                haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                                haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                                haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                                ' Right Flange

                                                Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                            webBotRight.Y + 0.0625)
                                                Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                            haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeTopRight As Point2d =
                                                        New Point2d(haunchBotLeft.X + InnerThick,
                                                                    haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeBotRight As Point2d =
                                                        New Point2d(webBotRight.X + InnerThick,
                                                                    webBotRight.Y + 0.0625)

                                                Dim rightFlange As Polyline = New Polyline

                                                rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                                ' Eave Cap

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveRise = TriSolv.Rise

                                                TriSolv.RunIn(eaveRise, pitch)
                                                Dim yTol = TriSolv.Rise

                                                Dim eaveBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - girtOffset + 0.25 + eaveRise,
                                                                    baseBotLeft.Y + eaveHeight - offSetRise -
                                                                    eaveCapSlope + yTol)
                                                Dim eaveTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - girtOffset + 0.25,
                                                                    baseBotLeft.Y + eaveHeight - offSetRise)
                                                Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 -
                                                                                          haunchRise)
                                                Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 -
                                                                                          haunchRise -
                                                                                          eaveCapSlope)

                                                Dim eaveCap As Polyline = New Polyline

                                                eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                                ' Stiffener

                                                Dim stiffBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                    haunchTopRight.Y - 3 - (HaunchLength - 6))
                                                Dim stiffTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                    haunchTopRight.Y + StiffThick - 3 -
                                                                    (HaunchLength - 6))
                                                Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                           haunchTopRight.Y + StiffThick -
                                                                                           3 -
                                                                                           (HaunchLength - 6))
                                                Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                           haunchTopRight.Y - 3 -
                                                                                           (HaunchLength - 6))

                                                Dim stiffener As Polyline = New Polyline

                                                stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                                stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                                stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                                ' Gusset

                                                Dim gusset As Line = New Line(
                                                    New Point3d(eaveBotLeft.X, eaveBotLeft.Y, 0),
                                                    New Point3d(baseBotLeft.X, eaveBotLeft.Y - offSetRise, 0))

                                                btr.AppendEntity(basePlate)
                                                btr.AppendEntity(leftFlange)
                                                btr.AppendEntity(web)
                                                btr.AppendEntity(haunch)
                                                btr.AppendEntity(rightFlange)
                                                btr.AppendEntity(eaveCap)
                                                btr.AppendEntity(stiffener)

                                                If Not Block Then

                                                    btr.AppendEntity(gusset)
                                                    transaction.AddNewlyCreatedDBObject(gusset, True)

                                                End If
                                                
                                                transaction.AddNewlyCreatedDBObject(basePlate, True)
                                                transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                                transaction.AddNewlyCreatedDBObject(web, True)
                                                transaction.AddNewlyCreatedDBObject(haunch, True)
                                                transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                                transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                                transaction.AddNewlyCreatedDBObject(stiffener, True)

                                                ' Girts 

                                                For i = 0 To GirtList.Count - 1

                                                    If GirtList.Item(i).Add Then

                                                        Select Case GirtList.Item(i).Type

                                                            Case "8"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X - 8, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X + 8, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "10"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X - 10, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X + 10, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "8"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X - 8, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X + 8, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                            Case "10"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X - 10, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X + 10, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                        End Select


                                                        If GirtList.Item(i).Brace Then

                                                            Dim braceLeft As Point3d = New Point3d(basePoint.X - 2, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                            Dim braceRight As Point3d = New Point3d(basePoint.X + OuterThick + WebDepth, braceLeft.Y, 0)

                                                            Dim brace As Line = New Line(braceLeft, braceRight)

                                                            Dim text As MText = New MText()
                                                            text.SetDatabaseDefaults()
                                                            text.Location = New Point3d(braceRight.X + InnerThick + 1, braceRight.Y, 0)
                                                            text.TextStyleId = dwg.Textstyle
                                                            text.TextHeight = dwg.Dimtxt
                                                            text.Attachment = AttachmentPoint.MiddleLeft
                                                            text.SetAttachmentMovingLocation(text.Attachment)
                                                            text.Width = 0.0

                                                            text.Contents = "FBX"

                                                            btr.AppendEntity(brace)
                                                            btr.AppendEntity(text)
                                                            transaction.AddNewlyCreatedDBObject(brace, True)
                                                            transaction.AddNewlyCreatedDBObject(text, True)

                                                        End If

                                                    End If

                                                Next

                                                ' Cable Holes

                                                If CableObj.Check Then

                                                    Dim topHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                    Dim bottomHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                    Dim topSlot As Circle = New Circle()
                                                    Dim botSlot As Circle = New Circle()

                                                    topSlot.Center = topHolePoint
                                                    botSlot.Center = bottomHolePoint

                                                    topSlot.Diameter = 1
                                                    botSlot.Diameter = 1

                                                    btr.AppendEntity(topSlot)
                                                    btr.AppendEntity(botSlot)

                                                    transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                    transaction.AddNewlyCreatedDBObject(botSlot, True)

                                                End If

                                                ' Block

                                                If Block Then

                                                    Dim bPlateTopLeft As Point2d = New Point2d(basePoint.X - girtOffset, eavePoint.Y - offSetRise)
                                                    Dim bPlateTopRight As Point2d = New Point2d(bPlateTopLeft.X + 0.25, bPlateTopLeft.Y)
                                                    Dim bPlateBotRight As Point2d = New Point2d(bPlateTopRight.X, bPlateTopRight.Y - 8)
                                                    Dim bPlateBotLeft As Point2d = New Point2d(bPlateTopLeft.X, bPlateBotRight.Y)

                                                    Dim bPlate As Polyline = New Polyline

                                                    bPlate.AddVertexAt(0, bPlateTopLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(1, bPlateTopRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(2, bPlateBotRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(3, bPlateBotLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(4, bPlateTopLeft, 0, 0, 0)

                                                    btr.AppendEntity(bPlate)
                                                    transaction.AddNewlyCreatedDBObject(bPlate, True)

                                                    Dim botPlateBotLeft As Point2d = bPlateBotRight
                                                    Dim botPlateTopLeft As Point2d = New Point2d(botPlateBotLeft.X, botPlateBotLeft.Y + 0.25)
                                                    Dim botPlateTopRight As Point2d = New Point2d(basePoint.X, botPlateTopLeft.Y)
                                                    Dim botPLateBotRight As Point2d = New Point2d(botPlateTopRight.X, botPlateBotLeft.Y)

                                                    Dim botPlate As Polyline = New Polyline

                                                    botPlate.AddVertexAt(0, botPlateBotLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(1, botPlateTopLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(2, botPlateTopRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(3, botPLateBotRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(4, botPlateBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(botPlate)
                                                    transaction.AddNewlyCreatedDBObject(botPlate, True)

                                                    Dim bWebBotLeft As Point2d = botPlateTopLeft
                                                    Dim bWebTopRight As Point2d = New Point2d(botPlateTopRight.X, eavePoint.Y - eaveCapSlope)
                                                    Dim bWebTopLeft As Point2d = New Point2d(bWebBotLeft.X, bWebTopRight.Y - offSetRise)
                                                    Dim bWebBotRight As Point2d = botPlateTopRight

                                                    Dim bWeb As Polyline = New Polyline

                                                    bWeb.AddVertexAt(0, bWebBotLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(1, bWebTopLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(2, bWebTopRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(3, bWebBotRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(4, bWebBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(bWeb)
                                                    transaction.AddNewlyCreatedDBObject(bWeb, True)

                                                End If

                                            End If

                                        End If

                                    ElseIf (pitchPoint.X < basePoint.X) Then

                                        If (Flush) Then

                                            ' Straight Standard Flush REVERSE

                                            Dim eaveHeight = eavePoint.Y - basePoint.Y
                                            Dim pitch = TriSolv.GetTruePitch(eavePoint.Y, pitchPoint.Y,
                                                                     eavePoint.X - pitchPoint.X)

                                            ' Base Plate

                                            Dim baseBotLeft As Point2d = basePoint
                                            Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + BaseThick)
                                            Dim baseTopRight As Point2d = New Point2d(baseBotLeft.X - (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y + BaseThick)
                                            Dim baseBotRight As Point2d = New Point2d(baseBotLeft.X - (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y)

                                            Dim basePlate As Polyline = New Polyline

                                            basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                            basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                            basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                            ' Left Flange

                                            Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + BaseThick + 0.0625)

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveCapSlope = TriSolv.Slope

                                            Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - 0.0625)
                                            Dim lFlangeTopRight As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                         basePoint.Y + eaveHeight -
                                                                                         eaveCapSlope - 0.0625)
                                            Dim lFlangeBotRight As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                         basePoint.Y + BaseThick +
                                                                                         0.0625)

                                            Dim leftFlange As Polyline = New Polyline

                                            leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                            ' Web

                                            Dim webBotLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                    basePoint.Y + BaseThick)

                                            TriSolv.RunIn(OuterThick, pitch)
                                            Dim leftFlangeRise = TriSolv.Rise

                                            TriSolv.RunIn(WebDepth, pitch)
                                            Dim webRise = TriSolv.Rise

                                            Dim webTopLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                    basePoint.Y + eaveHeight -
                                                                                    eaveCapSlope + leftFlangeRise)
                                            Dim webTopRight As Point2d = New Point2d(webTopLeft.X - WebDepth,
                                                                                     webTopLeft.Y + webRise)
                                            Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                     basePoint.Y + BaseThick)

                                            Dim web As Polyline = New Polyline

                                            web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                            web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                            web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                            web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                            web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                            ' Haunch Plate

                                            TriSolv.RunIn(HaunchThick, pitch)
                                            Dim haunchRise = TriSolv.Rise

                                            Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight +
                                                                                       leftFlangeRise + webRise +
                                                                                       haunchRise + 3 -
                                                                                       HaunchLength)
                                            Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight +
                                                                                       leftFlangeRise + webRise +
                                                                                       haunchRise + 3)
                                            Dim haunchTopRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight +
                                                                                        webRise + leftFlangeRise +
                                                                                        haunchRise + 3)
                                            Dim haunchBotRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight +
                                                                                        webRise + leftFlangeRise +
                                                                                        haunchRise + 3 -
                                                                                        HaunchLength)

                                            Dim haunch As Polyline = New Polyline

                                            haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                            haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                            haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                            haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                            haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                            ' Right Flange

                                            Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                        webBotRight.Y + 0.0625)
                                            Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                        haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeTopRight As Point2d =
                                                    New Point2d(haunchBotLeft.X - InnerThick,
                                                                haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeBotRight As Point2d =
                                                    New Point2d(webBotRight.X - InnerThick, webBotRight.Y + 0.0625)

                                            Dim rightFlange As Polyline = New Polyline

                                            rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                            ' Eave Cap

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveRise = TriSolv.Rise

                                            TriSolv.RunIn(eaveRise, pitch)
                                            Dim yTol = TriSolv.Rise

                                            Dim eaveBotLeft As Point2d = New Point2d(baseBotLeft.X - eaveRise,
                                                                                     baseBotLeft.Y + eaveHeight -
                                                                                     eaveCapSlope + yTol)
                                            Dim eaveTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + eaveHeight)
                                            Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 - haunchRise)
                                            Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 - haunchRise -
                                                                                      eaveCapSlope)

                                            Dim eaveCap As Polyline = New Polyline

                                            eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                            ' Stiffener

                                            Dim stiffBotLeft As Point2d =
                                                    New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                haunchTopRight.Y - 3 - (HaunchLength - 6))
                                            Dim stiffTopLeft As Point2d =
                                                    New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                haunchTopRight.Y + StiffThick - 3 -
                                                                (HaunchLength - 6))
                                            Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                       haunchTopRight.Y + StiffThick - 3 -
                                                                                       (HaunchLength - 6))
                                            Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                       haunchTopRight.Y - 3 -
                                                                                       (HaunchLength - 6))

                                            Dim stiffener As Polyline = New Polyline

                                            stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                            stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                            stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                            btr.AppendEntity(basePlate)
                                            btr.AppendEntity(leftFlange)
                                            btr.AppendEntity(web)
                                            btr.AppendEntity(haunch)
                                            btr.AppendEntity(rightFlange)
                                            btr.AppendEntity(eaveCap)
                                            btr.AppendEntity(stiffener)

                                            transaction.AddNewlyCreatedDBObject(basePlate, True)
                                            transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                            transaction.AddNewlyCreatedDBObject(web, True)
                                            transaction.AddNewlyCreatedDBObject(haunch, True)
                                            transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                            transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                            transaction.AddNewlyCreatedDBObject(stiffener, True)

                                            ' Girts 

                                            For i = 0 To GirtList.Count - 1

                                                If GirtList.Item(i).Add Then

                                                    Select Case GirtList.Item(i).Type

                                                        Case "8"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X - 8, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "10"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X - 10, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "8"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X - 8, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                        Case "10"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X - 10, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                    End Select


                                                    If GirtList.Item(i).Brace Then

                                                        Dim braceLeft As Point3d = New Point3d(basePoint.X - 4, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                        Dim braceRight As Point3d = New Point3d(basePoint.X - OuterThick - WebDepth, braceLeft.Y, 0)

                                                        Dim brace As Line = New Line(braceLeft, braceRight)

                                                        Dim text As MText = New MText()
                                                        text.SetDatabaseDefaults()
                                                        text.Location = New Point3d(braceRight.X - InnerThick - 1, braceRight.Y, 0)
                                                        text.TextStyleId = dwg.Textstyle
                                                        text.TextHeight = dwg.Dimtxt
                                                        text.Attachment = AttachmentPoint.MiddleRight
                                                        text.SetAttachmentMovingLocation(text.Attachment)
                                                        text.Width = 0.0

                                                        text.Contents = "FBX"

                                                        btr.AppendEntity(brace)
                                                        btr.AppendEntity(text)
                                                        transaction.AddNewlyCreatedDBObject(brace, True)
                                                        transaction.AddNewlyCreatedDBObject(text, True)

                                                    End If

                                                End If

                                            Next

                                            ' Cable Holes

                                            If CableObj.Check Then

                                                Dim topHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                Dim bottomHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                Dim topSlot As Circle = New Circle()
                                                Dim botSlot As Circle = New Circle()

                                                topSlot.Center = topHolePoint
                                                botSlot.Center = bottomHolePoint

                                                topSlot.Diameter = 1
                                                botSlot.Diameter = 1

                                                btr.AppendEntity(topSlot)
                                                btr.AppendEntity(botSlot)

                                                transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                transaction.AddNewlyCreatedDBObject(botSlot, True)

                                            End If

                                        Else

                                            ' Straight Standard Bypass REVERSE

                                            Dim girtOptions As PromptPointOptions =
                                                    New PromptPointOptions(
                                                        ControlChars.Lf + "Select Outer Steel Line : ")
                                            Dim girtResult As PromptPointResult = editor.GetPoint(girtOptions)

                                            If (girtResult.Status = PromptStatus.OK) Then

                                                Dim eaveHeight = eavePoint.Y - basePoint.Y
                                                Dim girtOffset = Math.Abs(basePoint.X - girtResult.Value.X)
                                                Dim pitch = TriSolv.GetTruePitch(eavePoint.Y, pitchPoint.Y,
                                                                         eavePoint.X - pitchPoint.X)

                                                TriSolv.RunIn(girtOffset - 0.25, pitch)
                                                Dim offSetRise = TriSolv.Rise


                                                ' Base Plate
                                                Dim baseBotLeft As Point2d = basePoint
                                                Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                         baseBotLeft.Y + BaseThick)
                                                Dim baseTopRight As Point2d =
                                                        New Point2d(baseBotLeft.X - (OuterThick +
                                                                                     WebDepth +
                                                                                     InnerThick),
                                                                    baseTopLeft.Y)
                                                Dim baseBotRight As Point2d = New Point2d(baseTopRight.X, baseBotLeft.Y)

                                                Dim basePlate As Polyline = New Polyline

                                                basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                                basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                                basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                                ' Left Flange

                                                Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + BaseThick +
                                                                                            0.0625)

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveCapSlope = TriSolv.Slope

                                                Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + eaveHeight -
                                                                                            eaveCapSlope - 0.0625)
                                                Dim lFlangeTopRight As Point2d =
                                                        New Point2d(basePoint.X - OuterThick,
                                                                    basePoint.Y + eaveHeight -
                                                                    eaveCapSlope - 0.0625)
                                                Dim lFlangeBotRight As Point2d =
                                                        New Point2d(basePoint.X - OuterThick,
                                                                    basePoint.Y + BaseThick +
                                                                    0.0625)

                                                Dim leftFlange As Polyline = New Polyline

                                                leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                                ' Web

                                                Dim webBotLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                        basePoint.Y + BaseThick)

                                                TriSolv.RunIn(OuterThick, pitch)
                                                Dim leftFlangeRise = TriSolv.Rise

                                                TriSolv.RunIn(WebDepth, pitch)
                                                Dim webRise = TriSolv.Rise

                                                Dim webTopLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope + leftFlangeRise)
                                                Dim webTopRight As Point2d = New Point2d(webTopLeft.X - WebDepth,
                                                                                         webTopLeft.Y + webRise)
                                                Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                         basePoint.Y + BaseThick)

                                                Dim web As Polyline = New Polyline

                                                web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                                web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                                web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                                web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                                web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                                ' Haunch Plate

                                                TriSolv.RunIn(HaunchThick, pitch)
                                                Dim haunchRise = TriSolv.Rise

                                                Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight +
                                                                                           leftFlangeRise + webRise +
                                                                                           haunchRise + 3 -
                                                                                           HaunchLength)
                                                Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight +
                                                                                           leftFlangeRise + webRise +
                                                                                           haunchRise + 3)
                                                Dim haunchTopRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight +
                                                                                            webRise + leftFlangeRise +
                                                                                            haunchRise + 3)
                                                Dim haunchBotRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight +
                                                                                            webRise + leftFlangeRise +
                                                                                            haunchRise + 3 -
                                                                                            HaunchLength)

                                                Dim haunch As Polyline = New Polyline

                                                haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                                haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                                haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                                haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                                haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                                ' Right Flange

                                                Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                            webBotRight.Y + 0.0625)
                                                Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                            haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeTopRight As Point2d =
                                                        New Point2d(haunchBotLeft.X - InnerThick,
                                                                    haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeBotRight As Point2d =
                                                        New Point2d(webBotRight.X - InnerThick,
                                                                    webBotRight.Y + 0.0625)

                                                Dim rightFlange As Polyline = New Polyline

                                                rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                                ' Eave Cap

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveRise = TriSolv.Rise

                                                TriSolv.RunIn(eaveRise, pitch)
                                                Dim yTol = TriSolv.Rise

                                                Dim eaveBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + girtOffset - 0.25 - eaveRise,
                                                                    baseBotLeft.Y + eaveHeight - offSetRise -
                                                                    eaveCapSlope + yTol)
                                                Dim eaveTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + girtOffset - 0.25,
                                                                    baseBotLeft.Y + eaveHeight - offSetRise)
                                                Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 -
                                                                                          haunchRise)
                                                Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 -
                                                                                          haunchRise -
                                                                                          eaveCapSlope)

                                                Dim eaveCap As Polyline = New Polyline

                                                eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                                ' Stiffener

                                                Dim stiffBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                    haunchTopRight.Y - 3 - (HaunchLength - 6))
                                                Dim stiffTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                    haunchTopRight.Y + StiffThick - 3 -
                                                                    (HaunchLength - 6))
                                                Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                           haunchTopRight.Y + StiffThick -
                                                                                           3 -
                                                                                           (HaunchLength - 6))
                                                Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                           haunchTopRight.Y - 3 -
                                                                                           (HaunchLength - 6))

                                                Dim stiffener As Polyline = New Polyline

                                                stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                                stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                                stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                                Dim gusset As Line = New Line(
                                                    New Point3d(eaveBotLeft.X, eaveBotLeft.Y, 0),
                                                    New Point3d(baseBotLeft.X, eaveBotLeft.Y - offSetRise, 0))

                                                If Not Block Then

                                                    btr.AppendEntity(gusset)
                                                    transaction.AddNewlyCreatedDBObject(gusset, True)

                                                End If

                                                btr.AppendEntity(basePlate)
                                                btr.AppendEntity(leftFlange)
                                                btr.AppendEntity(web)
                                                btr.AppendEntity(haunch)
                                                btr.AppendEntity(rightFlange)
                                                btr.AppendEntity(eaveCap)
                                                btr.AppendEntity(stiffener)

                                                transaction.AddNewlyCreatedDBObject(basePlate, True)
                                                transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                                transaction.AddNewlyCreatedDBObject(web, True)
                                                transaction.AddNewlyCreatedDBObject(haunch, True)
                                                transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                                transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                                transaction.AddNewlyCreatedDBObject(stiffener, True)

                                                ' Girts 

                                                For i = 0 To GirtList.Count - 1

                                                    If GirtList.Item(i).Add Then

                                                        Select Case GirtList.Item(i).Type

                                                            Case "8"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X + 8, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X - 8, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "10"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X + 10, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X - 10, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "8"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X + 8, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X - 8, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                            Case "10"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X + 10, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X - 10, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                        End Select


                                                        If GirtList.Item(i).Brace Then

                                                            Dim braceLeft As Point3d = New Point3d(basePoint.X + 2, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                            Dim braceRight As Point3d = New Point3d(basePoint.X - OuterThick - WebDepth, braceLeft.Y, 0)

                                                            Dim brace As Line = New Line(braceLeft, braceRight)

                                                            Dim text As MText = New MText()
                                                            text.SetDatabaseDefaults()
                                                            text.Location = New Point3d(braceRight.X - InnerThick - 1, braceRight.Y, 0)
                                                            text.TextStyleId = dwg.Textstyle
                                                            text.TextHeight = dwg.Dimtxt
                                                            text.Attachment = AttachmentPoint.MiddleRight
                                                            text.SetAttachmentMovingLocation(text.Attachment)
                                                            text.Width = 0.0

                                                            text.Contents = "FBX"

                                                            btr.AppendEntity(brace)
                                                            btr.AppendEntity(text)
                                                            transaction.AddNewlyCreatedDBObject(brace, True)
                                                            transaction.AddNewlyCreatedDBObject(text, True)

                                                        End If

                                                    End If

                                                Next

                                                ' Cable Holes

                                                If CableObj.Check Then

                                                    Dim topHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                    Dim bottomHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                    Dim topSlot As Circle = New Circle()
                                                    Dim botSlot As Circle = New Circle()

                                                    topSlot.Center = topHolePoint
                                                    botSlot.Center = bottomHolePoint

                                                    topSlot.Diameter = 1
                                                    botSlot.Diameter = 1

                                                    btr.AppendEntity(topSlot)
                                                    btr.AppendEntity(botSlot)

                                                    transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                    transaction.AddNewlyCreatedDBObject(botSlot, True)

                                                End If

                                                ' Block

                                                If Block Then

                                                    Dim bPlateTopLeft As Point2d = New Point2d(basePoint.X + girtOffset, eavePoint.Y - offSetRise)
                                                    Dim bPlateTopRight As Point2d = New Point2d(bPlateTopLeft.X - 0.25, bPlateTopLeft.Y)
                                                    Dim bPlateBotRight As Point2d = New Point2d(bPlateTopRight.X, bPlateTopRight.Y - 8)
                                                    Dim bPlateBotLeft As Point2d = New Point2d(bPlateTopLeft.X, bPlateBotRight.Y)

                                                    Dim bPlate As Polyline = New Polyline

                                                    bPlate.AddVertexAt(0, bPlateTopLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(1, bPlateTopRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(2, bPlateBotRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(3, bPlateBotLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(4, bPlateTopLeft, 0, 0, 0)

                                                    btr.AppendEntity(bPlate)
                                                    transaction.AddNewlyCreatedDBObject(bPlate, True)

                                                    Dim botPlateBotLeft As Point2d = bPlateBotRight
                                                    Dim botPlateTopLeft As Point2d = New Point2d(botPlateBotLeft.X, botPlateBotLeft.Y + 0.25)
                                                    Dim botPlateTopRight As Point2d = New Point2d(basePoint.X, botPlateTopLeft.Y)
                                                    Dim botPLateBotRight As Point2d = New Point2d(botPlateTopRight.X, botPlateBotLeft.Y)

                                                    Dim botPlate As Polyline = New Polyline

                                                    botPlate.AddVertexAt(0, botPlateBotLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(1, botPlateTopLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(2, botPlateTopRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(3, botPLateBotRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(4, botPlateBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(botPlate)
                                                    transaction.AddNewlyCreatedDBObject(botPlate, True)

                                                    Dim bWebBotLeft As Point2d = botPlateTopLeft
                                                    Dim bWebTopRight As Point2d = New Point2d(botPlateTopRight.X, eavePoint.Y - eaveCapSlope)
                                                    Dim bWebTopLeft As Point2d = New Point2d(bWebBotLeft.X, bWebTopRight.Y - offSetRise)
                                                    Dim bWebBotRight As Point2d = botPlateTopRight

                                                    Dim bWeb As Polyline = New Polyline

                                                    bWeb.AddVertexAt(0, bWebBotLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(1, bWebTopLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(2, bWebTopRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(3, bWebBotRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(4, bWebBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(bWeb)
                                                    transaction.AddNewlyCreatedDBObject(bWeb, True)

                                                End If

                                            End If

                                        End If

                                    End If

                                ElseIf (pitchPoint.Y < eavePoint.Y) Then

                                    If (pitchPoint.X > basePoint.X) Then

                                        If (Flush) Then

                                            ' Straight HIGH SIDE Flush

                                            Dim eaveHeight = eavePoint.Y - basePoint.Y
                                            Dim pitch = TriSolv.GetTruePitch(pitchPoint.Y, eavePoint.Y,
                                                                     pitchPoint.X - eavePoint.X)

                                            ' Base Plate

                                            Dim baseBotLeft As Point2d = basePoint
                                            Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + BaseThick)
                                            Dim baseTopRight As Point2d = New Point2d(baseBotLeft.X + (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y + BaseThick)
                                            Dim baseBotRight As Point2d = New Point2d(baseBotLeft.X + (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y)

                                            Dim basePlate As Polyline = New Polyline

                                            basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                            basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                            basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                            ' Left Flange

                                            Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + BaseThick + 0.0625)

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveCapSlope = TriSolv.Slope

                                            TriSolv.RunIn(OuterThick, pitch)
                                            Dim leftFlangeRise = TriSolv.Rise

                                            Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - leftFlangeRise -
                                                                                        0.0625)
                                            Dim lFlangeTopRight As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                         basePoint.Y + eaveHeight -
                                                                                         eaveCapSlope - leftFlangeRise -
                                                                                         0.0625)
                                            Dim lFlangeBotRight As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                         basePoint.Y + BaseThick +
                                                                                         0.0625)

                                            Dim leftFlange As Polyline = New Polyline

                                            leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                            ' Web

                                            Dim webBotLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                    basePoint.Y + BaseThick)

                                            TriSolv.RunIn(WebDepth, pitch)
                                            Dim webRise = TriSolv.Rise

                                            Dim webTopLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                    basePoint.Y + eaveHeight -
                                                                                    eaveCapSlope - leftFlangeRise)
                                            Dim webTopRight As Point2d = New Point2d(webTopLeft.X + WebDepth,
                                                                                     webTopLeft.Y - webRise)
                                            Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                     basePoint.Y + BaseThick)

                                            Dim web As Polyline = New Polyline

                                            web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                            web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                            web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                            web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                            web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                            ' Haunch Plate

                                            TriSolv.RunIn(HaunchThick, pitch)
                                            Dim haunchRise = TriSolv.Rise

                                            Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight -
                                                                                       leftFlangeRise - webRise -
                                                                                       haunchRise + 3 -
                                                                                       HaunchLength)
                                            Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight -
                                                                                       leftFlangeRise - webRise -
                                                                                       haunchRise + 3)
                                            Dim haunchTopRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight -
                                                                                        webRise - leftFlangeRise -
                                                                                        haunchRise + 3)
                                            Dim haunchBotRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight -
                                                                                        webRise - leftFlangeRise -
                                                                                        haunchRise + 3 -
                                                                                        HaunchLength)

                                            Dim haunch As Polyline = New Polyline

                                            haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                            haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                            haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                            haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                            haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                            ' Right Flange

                                            Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                        webBotRight.Y + 0.0625)
                                            Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                        haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeTopRight As Point2d =
                                                    New Point2d(haunchBotLeft.X + InnerThick,
                                                                haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeBotRight As Point2d =
                                                    New Point2d(webBotRight.X + InnerThick, webBotRight.Y + 0.0625)

                                            Dim rightFlange As Polyline = New Polyline

                                            rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                            ' Eave Cap

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveRise = TriSolv.Rise

                                            TriSolv.RunIn(eaveRise, pitch)
                                            Dim yTol = TriSolv.Rise

                                            Dim eaveBotLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + eaveHeight -
                                                                                     eaveCapSlope)
                                            Dim eaveTopLeft As Point2d = New Point2d(baseBotLeft.X + eaveRise,
                                                                                     baseBotLeft.Y + eaveHeight - yTol)
                                            Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 + haunchRise)
                                            Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 + haunchRise -
                                                                                      eaveCapSlope)

                                            Dim eaveCap As Polyline = New Polyline

                                            eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                            ' Stiffener

                                            Dim stiffBotLeft As Point2d =
                                                    New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                haunchTopRight.Y - 3 - (HaunchLength - 6))
                                            Dim stiffTopLeft As Point2d =
                                                    New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                haunchTopRight.Y + StiffThick - 3 -
                                                                (HaunchLength - 6))
                                            Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                       haunchTopRight.Y + StiffThick - 3 -
                                                                                       (HaunchLength - 6))
                                            Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                       haunchTopRight.Y - 3 -
                                                                                       (HaunchLength - 6))

                                            Dim stiffener As Polyline = New Polyline

                                            stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                            stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                            stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                            btr.AppendEntity(basePlate)
                                            btr.AppendEntity(leftFlange)
                                            btr.AppendEntity(web)
                                            btr.AppendEntity(haunch)
                                            btr.AppendEntity(rightFlange)
                                            btr.AppendEntity(eaveCap)
                                            btr.AppendEntity(stiffener)

                                            transaction.AddNewlyCreatedDBObject(basePlate, True)
                                            transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                            transaction.AddNewlyCreatedDBObject(web, True)
                                            transaction.AddNewlyCreatedDBObject(haunch, True)
                                            transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                            transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                            transaction.AddNewlyCreatedDBObject(stiffener, True)

                                            ' Girts 

                                            For i = 0 To GirtList.Count - 1

                                                If GirtList.Item(i).Add Then

                                                    Select Case GirtList.Item(i).Type

                                                        Case "8"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X + 8, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "10"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X + 10, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "8"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X + 8, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                        Case "10"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X + 10, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                    End Select


                                                    If GirtList.Item(i).Brace Then

                                                        Dim braceLeft As Point3d = New Point3d(basePoint.X + 4, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                        Dim braceRight As Point3d = New Point3d(basePoint.X + OuterThick + WebDepth, braceLeft.Y, 0)

                                                        Dim brace As Line = New Line(braceLeft, braceRight)

                                                        Dim text As MText = New MText()
                                                        text.SetDatabaseDefaults()
                                                        text.Location = New Point3d(braceRight.X + InnerThick + 1, braceRight.Y, 0)
                                                        text.TextStyleId = dwg.Textstyle
                                                        text.TextHeight = dwg.Dimtxt
                                                        text.Attachment = AttachmentPoint.MiddleLeft
                                                        text.SetAttachmentMovingLocation(text.Attachment)
                                                        text.Width = 0.0

                                                        text.Contents = "FBX"

                                                        btr.AppendEntity(brace)
                                                        btr.AppendEntity(text)
                                                        transaction.AddNewlyCreatedDBObject(brace, True)
                                                        transaction.AddNewlyCreatedDBObject(text, True)

                                                    End If

                                                End If

                                            Next

                                            ' Cable Holes

                                            If CableObj.Check Then

                                                Dim topHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                Dim bottomHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                Dim topSlot As Circle = New Circle()
                                                Dim botSlot As Circle = New Circle()

                                                topSlot.Center = topHolePoint
                                                botSlot.Center = bottomHolePoint

                                                topSlot.Diameter = 1
                                                botSlot.Diameter = 1

                                                btr.AppendEntity(topSlot)
                                                btr.AppendEntity(botSlot)

                                                transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                transaction.AddNewlyCreatedDBObject(botSlot, True)

                                            End If

                                        Else

                                            ' Straight HIGH SIDE Bypass

                                            Dim girtOptions As PromptPointOptions =
                                                    New PromptPointOptions(
                                                        ControlChars.Lf + "Select Outer Steel Line : ")
                                            Dim girtResult As PromptPointResult = editor.GetPoint(girtOptions)

                                            If (girtResult.Status = PromptStatus.OK) Then

                                                Dim eaveHeight = eavePoint.Y - basePoint.Y
                                                Dim girtOffset = Math.Abs(basePoint.X - girtResult.Value.X)
                                                Dim pitch = TriSolv.GetTruePitch(pitchPoint.Y, eavePoint.Y,
                                                                         pitchPoint.X - eavePoint.X)

                                                TriSolv.RunIn(girtOffset - 0.25, pitch)
                                                Dim offSetRise = TriSolv.Rise

                                                ' Base Plate

                                                Dim baseBotLeft As Point2d = basePoint
                                                Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                         baseBotLeft.Y + BaseThick)
                                                Dim baseTopRight As Point2d =
                                                        New Point2d(baseBotLeft.X + (OuterThick +
                                                                                     InnerThick +
                                                                                     WebDepth),
                                                                    baseBotLeft.Y + BaseThick)
                                                Dim baseBotRight As Point2d =
                                                        New Point2d(baseBotLeft.X + (OuterThick +
                                                                                     InnerThick +
                                                                                     WebDepth),
                                                                    baseBotLeft.Y)

                                                Dim basePlate As Polyline = New Polyline

                                                basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                                basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                                basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                                ' Left Flange

                                                Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + BaseThick +
                                                                                            0.0625)

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveCapSlope = TriSolv.Slope

                                                TriSolv.RunIn(OuterThick, pitch)
                                                Dim leftFlangeRise = TriSolv.Rise

                                                Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + eaveHeight -
                                                                                            eaveCapSlope -
                                                                                            leftFlangeRise - 0.0625)
                                                Dim lFlangeTopRight As Point2d =
                                                        New Point2d(basePoint.X + OuterThick,
                                                                    basePoint.Y + eaveHeight -
                                                                    eaveCapSlope - leftFlangeRise - 0.0625)
                                                Dim lFlangeBotRight As Point2d =
                                                        New Point2d(basePoint.X + OuterThick,
                                                                    basePoint.Y + BaseThick +
                                                                    0.0625)

                                                Dim leftFlange As Polyline = New Polyline

                                                leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                                ' Web

                                                Dim webBotLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                        basePoint.Y + BaseThick)

                                                TriSolv.RunIn(WebDepth, pitch)
                                                Dim webRise = TriSolv.Rise

                                                Dim webTopLeft As Point2d = New Point2d(basePoint.X + OuterThick,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - leftFlangeRise)
                                                Dim webTopRight As Point2d = New Point2d(webTopLeft.X + WebDepth,
                                                                                         webTopLeft.Y - webRise)
                                                Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                         basePoint.Y + BaseThick)

                                                Dim web As Polyline = New Polyline

                                                web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                                web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                                web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                                web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                                web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                                ' Haunch Plate

                                                TriSolv.RunIn(HaunchThick, pitch)
                                                Dim haunchRise = TriSolv.Rise

                                                Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight -
                                                                                           leftFlangeRise - webRise -
                                                                                           haunchRise + 3 -
                                                                                           HaunchLength)
                                                Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight -
                                                                                           leftFlangeRise - webRise -
                                                                                           haunchRise + 3)
                                                Dim haunchTopRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight -
                                                                                            webRise - leftFlangeRise -
                                                                                            haunchRise + 3)
                                                Dim haunchBotRight As Point2d = New Point2d(webTopRight.X + HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight -
                                                                                            webRise - leftFlangeRise -
                                                                                            haunchRise + 3 -
                                                                                            HaunchLength)

                                                Dim haunch As Polyline = New Polyline

                                                haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                                haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                                haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                                haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                                haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                                ' Right Flange

                                                Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                            webBotRight.Y + 0.0625)
                                                Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                            haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeTopRight As Point2d =
                                                        New Point2d(haunchBotLeft.X + InnerThick,
                                                                    haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeBotRight As Point2d =
                                                        New Point2d(webBotRight.X + InnerThick,
                                                                    webBotRight.Y + 0.0625)

                                                Dim rightFlange As Polyline = New Polyline

                                                rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                                ' Eave Cap

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveRise = TriSolv.Rise

                                                TriSolv.RunIn(eaveRise, pitch)
                                                Dim yTol = TriSolv.Rise

                                                Dim eaveBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - girtOffset + 0.25,
                                                                    baseBotLeft.Y + eaveHeight + offSetRise -
                                                                    eaveCapSlope)
                                                Dim eaveTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - girtOffset + 0.25 + eaveRise,
                                                                    baseBotLeft.Y + eaveHeight + offSetRise - yTol)
                                                Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 +
                                                                                          haunchRise)
                                                Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 +
                                                                                          haunchRise -
                                                                                          eaveCapSlope)

                                                Dim eaveCap As Polyline = New Polyline

                                                eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                                ' Stiffener

                                                Dim stiffBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                    haunchTopRight.Y - 3 - (HaunchLength - 6))
                                                Dim stiffTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + OuterThick + 0.0625,
                                                                    haunchTopRight.Y + StiffThick - 3 -
                                                                    (HaunchLength - 6))
                                                Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                           haunchTopRight.Y + StiffThick -
                                                                                           3 -
                                                                                           (HaunchLength - 6))
                                                Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X - 0.0625,
                                                                                           haunchTopRight.Y - 3 -
                                                                                           (HaunchLength - 6))

                                                Dim stiffener As Polyline = New Polyline

                                                stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                                stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                                stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                                btr.AppendEntity(basePlate)
                                                btr.AppendEntity(leftFlange)
                                                btr.AppendEntity(web)
                                                btr.AppendEntity(haunch)
                                                btr.AppendEntity(rightFlange)
                                                btr.AppendEntity(eaveCap)
                                                btr.AppendEntity(stiffener)

                                                transaction.AddNewlyCreatedDBObject(basePlate, True)
                                                transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                                transaction.AddNewlyCreatedDBObject(web, True)
                                                transaction.AddNewlyCreatedDBObject(haunch, True)
                                                transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                                transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                                transaction.AddNewlyCreatedDBObject(stiffener, True)

                                                ' Girts 

                                                For i = 0 To GirtList.Count - 1

                                                    If GirtList.Item(i).Add Then

                                                        Select Case GirtList.Item(i).Type

                                                            Case "8"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X - 8, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X + 8, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "10"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X - 10, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X + 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X + 10, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X - 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "8"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X - 8, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X + 8, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                            Case "10"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X - 10, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X + 10, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                        End Select


                                                        If GirtList.Item(i).Brace Then

                                                            Dim braceLeft As Point3d = New Point3d(basePoint.X - 2, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                            Dim braceRight As Point3d = New Point3d(basePoint.X + OuterThick + WebDepth, braceLeft.Y, 0)

                                                            Dim brace As Line = New Line(braceLeft, braceRight)

                                                            Dim text As MText = New MText()
                                                            text.SetDatabaseDefaults()
                                                            text.Location = New Point3d(braceRight.X + InnerThick + 1, braceRight.Y, 0)
                                                            text.TextStyleId = dwg.Textstyle
                                                            text.TextHeight = dwg.Dimtxt
                                                            text.Attachment = AttachmentPoint.MiddleLeft
                                                            text.SetAttachmentMovingLocation(text.Attachment)
                                                            text.Width = 0.0

                                                            text.Contents = "FBX"

                                                            btr.AppendEntity(brace)
                                                            btr.AppendEntity(text)
                                                            transaction.AddNewlyCreatedDBObject(brace, True)
                                                            transaction.AddNewlyCreatedDBObject(text, True)

                                                        End If

                                                    End If

                                                Next

                                                ' Cable Holes

                                                If CableObj.Check Then

                                                    Dim topHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                    Dim bottomHolePoint As Point3d = New Point3d(basePoint.X + CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                    Dim topSlot As Circle = New Circle()
                                                    Dim botSlot As Circle = New Circle()

                                                    topSlot.Center = topHolePoint
                                                    botSlot.Center = bottomHolePoint

                                                    topSlot.Diameter = 1
                                                    botSlot.Diameter = 1

                                                    btr.AppendEntity(topSlot)
                                                    btr.AppendEntity(botSlot)

                                                    transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                    transaction.AddNewlyCreatedDBObject(botSlot, True)

                                                End If

                                                ' Block

                                                If Block Then

                                                    Dim bPlateTopLeft As Point2d = New Point2d(basePoint.X - girtOffset, eavePoint.Y + offSetRise)
                                                    Dim bPlateTopRight As Point2d = New Point2d(bPlateTopLeft.X + 0.25, bPlateTopLeft.Y)
                                                    Dim bPlateBotRight As Point2d = New Point2d(bPlateTopRight.X, bPlateTopRight.Y - 8)
                                                    Dim bPlateBotLeft As Point2d = New Point2d(bPlateTopLeft.X, bPlateBotRight.Y)

                                                    Dim bPlate As Polyline = New Polyline

                                                    bPlate.AddVertexAt(0, bPlateTopLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(1, bPlateTopRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(2, bPlateBotRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(3, bPlateBotLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(4, bPlateTopLeft, 0, 0, 0)

                                                    btr.AppendEntity(bPlate)
                                                    transaction.AddNewlyCreatedDBObject(bPlate, True)

                                                    Dim botPlateBotLeft As Point2d = bPlateBotRight
                                                    Dim botPlateTopLeft As Point2d = New Point2d(botPlateBotLeft.X, botPlateBotLeft.Y + 0.25)
                                                    Dim botPlateTopRight As Point2d = New Point2d(basePoint.X, botPlateTopLeft.Y)
                                                    Dim botPLateBotRight As Point2d = New Point2d(botPlateTopRight.X, botPlateBotLeft.Y)

                                                    Dim botPlate As Polyline = New Polyline

                                                    botPlate.AddVertexAt(0, botPlateBotLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(1, botPlateTopLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(2, botPlateTopRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(3, botPLateBotRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(4, botPlateBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(botPlate)
                                                    transaction.AddNewlyCreatedDBObject(botPlate, True)

                                                    Dim bWebBotLeft As Point2d = botPlateTopLeft
                                                    Dim bWebTopRight As Point2d = New Point2d(botPlateTopRight.X, eavePoint.Y - eaveCapSlope)
                                                    Dim bWebTopLeft As Point2d = New Point2d(bWebBotLeft.X, bWebTopRight.Y - offSetRise)
                                                    Dim bWebBotRight As Point2d = botPlateTopRight

                                                    Dim bWeb As Polyline = New Polyline

                                                    bWeb.AddVertexAt(0, bWebBotLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(1, bWebTopLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(2, bWebTopRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(3, bWebBotRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(4, bWebBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(bWeb)
                                                    transaction.AddNewlyCreatedDBObject(bWeb, True)

                                                End If


                                            End If

                                        End If

                                    ElseIf (pitchPoint.X < basePoint.X) Then

                                        If (Flush) Then

                                            ' Straight HIGH SIDE Flush REVERSE

                                            Dim eaveHeight = eavePoint.Y - basePoint.Y
                                            Dim pitch = TriSolv.GetTruePitch(pitchPoint.Y, eavePoint.Y,
                                                                     eavePoint.X - pitchPoint.X)

                                            ' Base Plate

                                            Dim baseBotLeft As Point2d = basePoint
                                            Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + BaseThick)
                                            Dim baseTopRight As Point2d = New Point2d(baseBotLeft.X - (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y + BaseThick)
                                            Dim baseBotRight As Point2d = New Point2d(baseBotLeft.X - (OuterThick +
                                                                                                       InnerThick +
                                                                                                       WebDepth),
                                                                                      baseBotLeft.Y)

                                            Dim basePlate As Polyline = New Polyline

                                            basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                            basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                            basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                            basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                            ' Left Flange

                                            Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + BaseThick + 0.0625)

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveCapSlope = TriSolv.Slope

                                            TriSolv.RunIn(OuterThick, pitch)
                                            Dim leftFlangeRise = TriSolv.Rise

                                            Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - leftFlangeRise -
                                                                                        0.0625)
                                            Dim lFlangeTopRight As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                         basePoint.Y + eaveHeight -
                                                                                         eaveCapSlope - leftFlangeRise -
                                                                                         0.0625)
                                            Dim lFlangeBotRight As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                         basePoint.Y + BaseThick +
                                                                                         0.0625)

                                            Dim leftFlange As Polyline = New Polyline

                                            leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                            leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                            leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                            ' Web

                                            Dim webBotLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                    basePoint.Y + BaseThick)

                                            TriSolv.RunIn(WebDepth, pitch)
                                            Dim webRise = TriSolv.Rise

                                            Dim webTopLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                    basePoint.Y + eaveHeight -
                                                                                    eaveCapSlope - leftFlangeRise)
                                            Dim webTopRight As Point2d = New Point2d(webTopLeft.X - WebDepth,
                                                                                     webTopLeft.Y - webRise)
                                            Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                     basePoint.Y + BaseThick)

                                            Dim web As Polyline = New Polyline

                                            web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                            web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                            web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                            web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                            web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                            ' Haunch Plate

                                            TriSolv.RunIn(HaunchThick, pitch)
                                            Dim haunchRise = TriSolv.Rise

                                            Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight -
                                                                                       leftFlangeRise - webRise -
                                                                                       haunchRise + 3 -
                                                                                       HaunchLength)
                                            Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                       baseBotLeft.Y + eaveHeight -
                                                                                       leftFlangeRise - webRise -
                                                                                       haunchRise + 3)
                                            Dim haunchTopRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight -
                                                                                        webRise - leftFlangeRise -
                                                                                        haunchRise + 3)
                                            Dim haunchBotRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                        baseBotLeft.Y + eaveHeight -
                                                                                        webRise - leftFlangeRise -
                                                                                        haunchRise + 3 -
                                                                                        HaunchLength)

                                            Dim haunch As Polyline = New Polyline

                                            haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                            haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                            haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                            haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                            haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                            ' Right Flange

                                            Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                        webBotRight.Y + 0.0625)
                                            Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                        haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeTopRight As Point2d =
                                                    New Point2d(haunchBotLeft.X - InnerThick,
                                                                haunchBotLeft.Y - 0.0625)
                                            Dim rFlangeBotRight As Point2d =
                                                    New Point2d(webBotRight.X - InnerThick, webBotRight.Y + 0.0625)

                                            Dim rightFlange As Polyline = New Polyline

                                            rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                            rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                            rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                            ' Eave Cap

                                            TriSolv.RunIn(EaveThick, pitch)
                                            Dim eaveRise = TriSolv.Rise

                                            TriSolv.RunIn(eaveRise, pitch)
                                            Dim yTol = TriSolv.Rise

                                            Dim eaveBotLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                     baseBotLeft.Y + eaveHeight -
                                                                                     eaveCapSlope)
                                            Dim eaveTopLeft As Point2d = New Point2d(baseBotLeft.X - eaveRise,
                                                                                     baseBotLeft.Y + eaveHeight - yTol)
                                            Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 + haunchRise)
                                            Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                      haunchTopLeft.Y - 3 + haunchRise -
                                                                                      eaveCapSlope)

                                            Dim eaveCap As Polyline = New Polyline

                                            eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                            eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                            eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                            ' Stiffener

                                            Dim stiffBotLeft As Point2d =
                                                    New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                haunchTopRight.Y - 3 - (HaunchLength - 6))
                                            Dim stiffTopLeft As Point2d =
                                                    New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                haunchTopRight.Y + StiffThick - 3 -
                                                                (HaunchLength - 6))
                                            Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                       haunchTopRight.Y + StiffThick - 3 -
                                                                                       (HaunchLength - 6))
                                            Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                       haunchTopRight.Y - 3 -
                                                                                       (HaunchLength - 6))

                                            Dim stiffener As Polyline = New Polyline

                                            stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                            stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                            stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                            stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                            btr.AppendEntity(basePlate)
                                            btr.AppendEntity(leftFlange)
                                            btr.AppendEntity(web)
                                            btr.AppendEntity(haunch)
                                            btr.AppendEntity(rightFlange)
                                            btr.AppendEntity(eaveCap)
                                            btr.AppendEntity(stiffener)

                                            transaction.AddNewlyCreatedDBObject(basePlate, True)
                                            transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                            transaction.AddNewlyCreatedDBObject(web, True)
                                            transaction.AddNewlyCreatedDBObject(haunch, True)
                                            transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                            transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                            transaction.AddNewlyCreatedDBObject(stiffener, True)

                                            ' Girts 

                                            For i = 0 To GirtList.Count - 1

                                                If GirtList.Item(i).Add Then

                                                    Select Case GirtList.Item(i).Type

                                                        Case "8"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X - 8, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "10"" Zee Girt"

                                                            Dim topPoint As Point2d = New Point2d(basePoint.X, GirtList.Item(i).Elevation + basePoint.Y)
                                                            Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                            Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                            Dim botPoint As Point2d = New Point2d(topPoint.X - 10, topPoint.Y)
                                                            Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                            Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                            Dim zee As Polyline = New Polyline

                                                            zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                            zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                            zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                            zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                            zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                            zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                            btr.AppendEntity(zee)
                                                            transaction.AddNewlyCreatedDBObject(zee, True)

                                                        Case "8"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X - 8, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                        Case "10"" Cee Girt"

                                                            Dim botLeft As Point2d = New Point2d(basePoint.X, basePoint.Y + GirtList.Item(i).Elevation)
                                                            Dim botRight As Point2d = New Point2d(botLeft.X - 10, botLeft.Y)
                                                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                            Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                            Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                            Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                            Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                            Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                            Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                            Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                            Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                            Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                            Dim cee As Polyline = New Polyline

                                                            cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                            cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                            cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                            cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                            cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                            cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                            cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                            cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                            cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                            cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                            cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                            cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                            cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                            btr.AppendEntity(cee)

                                                            transaction.AddNewlyCreatedDBObject(cee, True)

                                                    End Select


                                                    If GirtList.Item(i).Brace Then

                                                        Dim braceLeft As Point3d = New Point3d(basePoint.X - 4, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                        Dim braceRight As Point3d = New Point3d(basePoint.X - OuterThick - WebDepth, braceLeft.Y, 0)

                                                        Dim brace As Line = New Line(braceLeft, braceRight)

                                                        Dim text As MText = New MText()
                                                        text.SetDatabaseDefaults()
                                                        text.Location = New Point3d(braceRight.X - InnerThick - 1, braceRight.Y, 0)
                                                        text.TextStyleId = dwg.Textstyle
                                                        text.TextHeight = dwg.Dimtxt
                                                        text.Attachment = AttachmentPoint.MiddleRight
                                                        text.SetAttachmentMovingLocation(text.Attachment)
                                                        text.Width = 0.0

                                                        text.Contents = "FBX"

                                                        btr.AppendEntity(brace)
                                                        btr.AppendEntity(text)
                                                        transaction.AddNewlyCreatedDBObject(brace, True)
                                                        transaction.AddNewlyCreatedDBObject(text, True)

                                                    End If

                                                End If

                                            Next

                                            ' Cable Holes

                                            If CableObj.Check Then

                                                Dim topHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                Dim bottomHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                Dim topSlot As Circle = New Circle()
                                                Dim botSlot As Circle = New Circle()

                                                topSlot.Center = topHolePoint
                                                botSlot.Center = bottomHolePoint

                                                topSlot.Diameter = 1
                                                botSlot.Diameter = 1

                                                btr.AppendEntity(topSlot)
                                                btr.AppendEntity(botSlot)

                                                transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                transaction.AddNewlyCreatedDBObject(botSlot, True)

                                            End If

                                        Else

                                            ' Straight HIGH SIDE Bypass Column REVERSE

                                            Dim girtOptions As PromptPointOptions =
                                                    New PromptPointOptions(
                                                        ControlChars.Lf + "Select Outer Steel Line : ")
                                            Dim girtResult As PromptPointResult = editor.GetPoint(girtOptions)

                                            If (girtResult.Status = PromptStatus.OK) Then

                                                Dim eaveHeight = eavePoint.Y - basePoint.Y
                                                Dim girtOffset = Math.Abs(basePoint.X - girtResult.Value.X)
                                                Dim pitch = TriSolv.GetTruePitch(pitchPoint.Y, eavePoint.Y,
                                                                         eavePoint.X - pitchPoint.X)

                                                TriSolv.RunIn(girtOffset - 0.25, pitch)
                                                Dim offSetRise = TriSolv.Rise

                                                ' Base Plate

                                                Dim baseBotLeft As Point2d = basePoint
                                                Dim baseTopLeft As Point2d = New Point2d(baseBotLeft.X,
                                                                                         baseBotLeft.Y + BaseThick)
                                                Dim baseTopRight As Point2d =
                                                        New Point2d(baseBotLeft.X - (OuterThick +
                                                                                     InnerThick +
                                                                                     WebDepth),
                                                                    baseBotLeft.Y + BaseThick)
                                                Dim baseBotRight As Point2d =
                                                        New Point2d(baseBotLeft.X - (OuterThick +
                                                                                     InnerThick +
                                                                                     WebDepth),
                                                                    baseBotLeft.Y)

                                                Dim basePlate As Polyline = New Polyline

                                                basePlate.AddVertexAt(0, baseBotLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(1, baseTopLeft, 0, 0, 0)
                                                basePlate.AddVertexAt(2, baseTopRight, 0, 0, 0)
                                                basePlate.AddVertexAt(3, baseBotRight, 0, 0, 0)
                                                basePlate.AddVertexAt(4, baseBotLeft, 0, 0, 0)

                                                ' Left Flange

                                                Dim lFlangeBotLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + BaseThick +
                                                                                            0.0625)

                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveCapSlope = TriSolv.Slope

                                                TriSolv.RunIn(OuterThick, pitch)
                                                Dim leftFlangeRise = TriSolv.Rise

                                                Dim lFlangeTopLeft As Point2d = New Point2d(basePoint.X,
                                                                                            basePoint.Y + eaveHeight -
                                                                                            eaveCapSlope -
                                                                                            leftFlangeRise -
                                                                                            0.0625)
                                                Dim lFlangeTopRight As Point2d =
                                                        New Point2d(basePoint.X - OuterThick,
                                                                    basePoint.Y + eaveHeight -
                                                                    eaveCapSlope - leftFlangeRise -
                                                                    0.0625)
                                                Dim lFlangeBotRight As Point2d =
                                                        New Point2d(basePoint.X - OuterThick,
                                                                    basePoint.Y + BaseThick +
                                                                    0.0625)

                                                Dim leftFlange As Polyline = New Polyline

                                                leftFlange.AddVertexAt(0, lFlangeBotLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(1, lFlangeTopLeft, 0, 0, 0)
                                                leftFlange.AddVertexAt(2, lFlangeTopRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(3, lFlangeBotRight, 0, 0, 0)
                                                leftFlange.AddVertexAt(4, lFlangeBotLeft, 0, 0, 0)

                                                ' Web

                                                Dim webBotLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                        basePoint.Y + BaseThick)

                                                TriSolv.RunIn(WebDepth, pitch)
                                                Dim webRise = TriSolv.Rise

                                                Dim webTopLeft As Point2d = New Point2d(basePoint.X - OuterThick,
                                                                                        basePoint.Y + eaveHeight -
                                                                                        eaveCapSlope - leftFlangeRise)
                                                Dim webTopRight As Point2d = New Point2d(webTopLeft.X - WebDepth,
                                                                                         webTopLeft.Y - webRise)
                                                Dim webBotRight As Point2d = New Point2d(webTopRight.X,
                                                                                         basePoint.Y + BaseThick)

                                                Dim web As Polyline = New Polyline

                                                web.AddVertexAt(0, webBotLeft, 0, 0, 0)
                                                web.AddVertexAt(1, webTopLeft, 0, 0, 0)
                                                web.AddVertexAt(2, webTopRight, 0, 0, 0)
                                                web.AddVertexAt(3, webBotRight, 0, 0, 0)
                                                web.AddVertexAt(4, webBotLeft, 0, 0, 0)

                                                ' Haunch Plate

                                                TriSolv.RunIn(HaunchThick, pitch)
                                                Dim haunchRise = TriSolv.Rise


                                                Dim haunchBotLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight -
                                                                                           leftFlangeRise - webRise -
                                                                                           haunchRise + 3 -
                                                                                           HaunchLength)
                                                Dim haunchTopLeft As Point2d = New Point2d(webTopRight.X,
                                                                                           baseBotLeft.Y + eaveHeight -
                                                                                           leftFlangeRise - webRise -
                                                                                           haunchRise + 3)
                                                Dim haunchTopRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight -
                                                                                            webRise - leftFlangeRise -
                                                                                            haunchRise + 3)
                                                Dim haunchBotRight As Point2d = New Point2d(webTopRight.X - HaunchThick,
                                                                                            baseBotLeft.Y + eaveHeight -
                                                                                            webRise - leftFlangeRise -
                                                                                            haunchRise + 3 -
                                                                                            HaunchLength)

                                                Dim haunch As Polyline = New Polyline

                                                haunch.AddVertexAt(0, haunchBotLeft, 0, 0, 0)
                                                haunch.AddVertexAt(1, haunchTopLeft, 0, 0, 0)
                                                haunch.AddVertexAt(2, haunchTopRight, 0, 0, 0)
                                                haunch.AddVertexAt(3, haunchBotRight, 0, 0, 0)
                                                haunch.AddVertexAt(4, haunchBotLeft, 0, 0, 0)

                                                ' Right Flange

                                                Dim rFlangeBotLeft As Point2d = New Point2d(webBotRight.X,
                                                                                            webBotRight.Y + 0.0625)
                                                Dim rFlangeTopLeft As Point2d = New Point2d(haunchBotLeft.X,
                                                                                            haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeTopRight As Point2d =
                                                        New Point2d(haunchBotLeft.X - InnerThick,
                                                                    haunchBotLeft.Y - 0.0625)
                                                Dim rFlangeBotRight As Point2d =
                                                        New Point2d(webBotRight.X - InnerThick,
                                                                    webBotRight.Y + 0.0625)

                                                Dim rightFlange As Polyline = New Polyline

                                                rightFlange.AddVertexAt(0, rFlangeBotLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(1, rFlangeTopLeft, 0, 0, 0)
                                                rightFlange.AddVertexAt(2, rFlangeTopRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(3, rFlangeBotRight, 0, 0, 0)
                                                rightFlange.AddVertexAt(4, rFlangeBotLeft, 0, 0, 0)

                                                ' Eave Cap


                                                TriSolv.RunIn(EaveThick, pitch)
                                                Dim eaveRise = TriSolv.Rise

                                                TriSolv.RunIn(eaveRise, pitch)
                                                Dim yTol = TriSolv.Rise

                                                Dim eaveBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + girtOffset - 0.25,
                                                                    baseBotLeft.Y + eaveHeight + offSetRise -
                                                                    eaveCapSlope)
                                                Dim eaveTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X + girtOffset - 0.25 - eaveRise,
                                                                    baseBotLeft.Y + eaveHeight + offSetRise - yTol)
                                                Dim eaveTopRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 +
                                                                                          haunchRise)
                                                Dim eaveBotRight As Point2d = New Point2d(haunchBotLeft.X,
                                                                                          haunchTopLeft.Y - 3 +
                                                                                          haunchRise -
                                                                                          eaveCapSlope)

                                                Dim eaveCap As Polyline = New Polyline

                                                eaveCap.AddVertexAt(0, eaveBotLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(1, eaveTopLeft, 0, 0, 0)
                                                eaveCap.AddVertexAt(2, eaveTopRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(3, eaveBotRight, 0, 0, 0)
                                                eaveCap.AddVertexAt(4, eaveBotLeft, 0, 0, 0)

                                                ' Stiffener

                                                Dim stiffBotLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                    haunchTopRight.Y - 3 - (HaunchLength - 6))
                                                Dim stiffTopLeft As Point2d =
                                                        New Point2d(baseBotLeft.X - OuterThick - 0.0625,
                                                                    haunchTopRight.Y + StiffThick - 3 -
                                                                    (HaunchLength - 6))
                                                Dim stiffTopRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                           haunchTopRight.Y + StiffThick -
                                                                                           3 -
                                                                                           (HaunchLength - 6))
                                                Dim stiffBotRight As Point2d = New Point2d(rFlangeBotLeft.X + 0.0625,
                                                                                           haunchTopRight.Y - 3 -
                                                                                           (HaunchLength - 6))

                                                Dim stiffener As Polyline = New Polyline

                                                stiffener.AddVertexAt(0, stiffBotLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(1, stiffTopLeft, 0, 0, 0)
                                                stiffener.AddVertexAt(2, stiffTopRight, 0, 0, 0)
                                                stiffener.AddVertexAt(3, stiffBotRight, 0, 0, 0)
                                                stiffener.AddVertexAt(4, stiffBotLeft, 0, 0, 0)

                                                btr.AppendEntity(basePlate)
                                                btr.AppendEntity(leftFlange)
                                                btr.AppendEntity(web)
                                                btr.AppendEntity(haunch)
                                                btr.AppendEntity(rightFlange)
                                                btr.AppendEntity(eaveCap)
                                                btr.AppendEntity(stiffener)


                                                transaction.AddNewlyCreatedDBObject(basePlate, True)
                                                transaction.AddNewlyCreatedDBObject(leftFlange, True)
                                                transaction.AddNewlyCreatedDBObject(web, True)
                                                transaction.AddNewlyCreatedDBObject(haunch, True)
                                                transaction.AddNewlyCreatedDBObject(rightFlange, True)
                                                transaction.AddNewlyCreatedDBObject(eaveCap, True)
                                                transaction.AddNewlyCreatedDBObject(stiffener, True)

                                                ' Girts 

                                                For i = 0 To GirtList.Count - 1

                                                    If GirtList.Item(i).Add Then

                                                        Select Case GirtList.Item(i).Type

                                                            Case "8"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X + 8, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X - 8, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "10"" Zee Girt"

                                                                Dim topPoint As Point2d = New Point2d(basePoint.X + 10, GirtList.Item(i).Elevation + basePoint.Y)
                                                                Dim botRight As Point2d = New Point2d(topPoint.X, topPoint.Y - 2.5)
                                                                Dim topToe As Point2d = New Point2d(botRight.X - 0.6875, botRight.Y - 0.6875)
                                                                Dim botPoint As Point2d = New Point2d(topPoint.X - 10, topPoint.Y)
                                                                Dim botLeft As Point2d = New Point2d(botPoint.X, botPoint.Y + 2.5)
                                                                Dim botToe As Point2d = New Point2d(botLeft.X + 0.6875, botLeft.Y + 0.6875)

                                                                Dim zee As Polyline = New Polyline

                                                                zee.AddVertexAt(0, botToe, 0, 0, 0)
                                                                zee.AddVertexAt(1, botLeft, 0, 0, 0)
                                                                zee.AddVertexAt(2, botPoint, 0, 0, 0)
                                                                zee.AddVertexAt(3, topPoint, 0, 0, 0)
                                                                zee.AddVertexAt(4, botRight, 0, 0, 0)
                                                                zee.AddVertexAt(5, topToe, 0, 0, 0)

                                                                btr.AppendEntity(zee)
                                                                transaction.AddNewlyCreatedDBObject(zee, True)

                                                            Case "8"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X + 8, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X - 8, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                            Case "10"" Cee Girt"

                                                                Dim botLeft As Point2d = New Point2d(basePoint.X + 10, basePoint.Y + GirtList.Item(i).Elevation)
                                                                Dim botRight As Point2d = New Point2d(botLeft.X - 10, botLeft.Y)
                                                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + 2.5)
                                                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                                                Dim toeLeft As Point2d = New Point2d(topLeft.X - 1, topLeft.Y)
                                                                Dim toeRight As Point2d = New Point2d(topRight.X + 1, topRight.Y)

                                                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y - 0.25)
                                                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X - 0.25, insideTopLeft1.Y)
                                                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X - 0.25, botLeft.Y + 0.25)
                                                                Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y - 0.25)
                                                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y - 0.25)

                                                                Dim cee As Polyline = New Polyline

                                                                cee.AddVertexAt(0, toeLeft, 0, 0, 0)
                                                                cee.AddVertexAt(1, topLeft, 0, 0, 0)
                                                                cee.AddVertexAt(2, botLeft, 0, 0, 0)
                                                                cee.AddVertexAt(3, botRight, 0, 0, 0)
                                                                cee.AddVertexAt(4, topRight, 0, 0, 0)
                                                                cee.AddVertexAt(5, toeRight, 0, 0, 0)
                                                                cee.AddVertexAt(6, insideTopRight1, 0, 0, 0)
                                                                cee.AddVertexAt(7, insideTopRight2, 0, 0, 0)
                                                                cee.AddVertexAt(8, insideBotRight, 0, 0, 0)
                                                                cee.AddVertexAt(9, insideBotLeft, 0, 0, 0)
                                                                cee.AddVertexAt(10, insideTopLeft2, 0, 0, 0)
                                                                cee.AddVertexAt(11, insideTopLeft1, 0, 0, 0)
                                                                cee.AddVertexAt(12, toeLeft, 0, 0, 0)

                                                                btr.AppendEntity(cee)

                                                                transaction.AddNewlyCreatedDBObject(cee, True)

                                                        End Select


                                                        If GirtList.Item(i).Brace Then

                                                            Dim braceLeft As Point3d = New Point3d(basePoint.X + 2, basePoint.Y + GirtList.Item(i).Elevation - 1, 0)
                                                            Dim braceRight As Point3d = New Point3d(basePoint.X - OuterThick - WebDepth, braceLeft.Y, 0)

                                                            Dim brace As Line = New Line(braceLeft, braceRight)

                                                            Dim text As MText = New MText()
                                                            text.SetDatabaseDefaults()
                                                            text.Location = New Point3d(braceRight.X - InnerThick - 1, braceRight.Y, 0)
                                                            text.TextStyleId = dwg.Textstyle
                                                            text.TextHeight = dwg.Dimtxt
                                                            text.Attachment = AttachmentPoint.MiddleRight
                                                            text.SetAttachmentMovingLocation(text.Attachment)
                                                            text.Width = 0.0

                                                            text.Contents = "FBX"

                                                            btr.AppendEntity(brace)
                                                            btr.AppendEntity(text)
                                                            transaction.AddNewlyCreatedDBObject(brace, True)
                                                            transaction.AddNewlyCreatedDBObject(text, True)

                                                        End If

                                                    End If

                                                Next

                                                ' Cable Holes

                                                If CableObj.Check Then

                                                    Dim topHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, eavePoint.Y - CableObj.FromTop, 0)
                                                    Dim bottomHolePoint As Point3d = New Point3d(basePoint.X - CableObj.FromFlange, basePoint.Y + CableObj.FromBottom, 0)

                                                    Dim topSlot As Circle = New Circle()
                                                    Dim botSlot As Circle = New Circle()

                                                    topSlot.Center = topHolePoint
                                                    botSlot.Center = bottomHolePoint

                                                    topSlot.Diameter = 1
                                                    botSlot.Diameter = 1

                                                    btr.AppendEntity(topSlot)
                                                    btr.AppendEntity(botSlot)

                                                    transaction.AddNewlyCreatedDBObject(topSlot, True)
                                                    transaction.AddNewlyCreatedDBObject(botSlot, True)

                                                End If

                                                ' Block

                                                If Block Then

                                                    Dim bPlateTopLeft As Point2d = New Point2d(basePoint.X + girtOffset, eavePoint.Y + offSetRise)
                                                    Dim bPlateTopRight As Point2d = New Point2d(bPlateTopLeft.X - 0.25, bPlateTopLeft.Y)
                                                    Dim bPlateBotRight As Point2d = New Point2d(bPlateTopRight.X, bPlateTopRight.Y - 8)
                                                    Dim bPlateBotLeft As Point2d = New Point2d(bPlateTopLeft.X, bPlateBotRight.Y)

                                                    Dim bPlate As Polyline = New Polyline

                                                    bPlate.AddVertexAt(0, bPlateTopLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(1, bPlateTopRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(2, bPlateBotRight, 0, 0, 0)
                                                    bPlate.AddVertexAt(3, bPlateBotLeft, 0, 0, 0)
                                                    bPlate.AddVertexAt(4, bPlateTopLeft, 0, 0, 0)

                                                    btr.AppendEntity(bPlate)
                                                    transaction.AddNewlyCreatedDBObject(bPlate, True)

                                                    Dim botPlateBotLeft As Point2d = bPlateBotRight
                                                    Dim botPlateTopLeft As Point2d = New Point2d(botPlateBotLeft.X, botPlateBotLeft.Y + 0.25)
                                                    Dim botPlateTopRight As Point2d = New Point2d(basePoint.X, botPlateTopLeft.Y)
                                                    Dim botPLateBotRight As Point2d = New Point2d(botPlateTopRight.X, botPlateBotLeft.Y)

                                                    Dim botPlate As Polyline = New Polyline

                                                    botPlate.AddVertexAt(0, botPlateBotLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(1, botPlateTopLeft, 0, 0, 0)
                                                    botPlate.AddVertexAt(2, botPlateTopRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(3, botPLateBotRight, 0, 0, 0)
                                                    botPlate.AddVertexAt(4, botPlateBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(botPlate)
                                                    transaction.AddNewlyCreatedDBObject(botPlate, True)

                                                    Dim bWebBotLeft As Point2d = botPlateTopLeft
                                                    Dim bWebTopRight As Point2d = New Point2d(botPlateTopRight.X, eavePoint.Y - eaveCapSlope)
                                                    Dim bWebTopLeft As Point2d = New Point2d(bWebBotLeft.X, bWebTopRight.Y - offSetRise)
                                                    Dim bWebBotRight As Point2d = botPlateTopRight

                                                    Dim bWeb As Polyline = New Polyline

                                                    bWeb.AddVertexAt(0, bWebBotLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(1, bWebTopLeft, 0, 0, 0)
                                                    bWeb.AddVertexAt(2, bWebTopRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(3, bWebBotRight, 0, 0, 0)
                                                    bWeb.AddVertexAt(4, bWebBotLeft, 0, 0, 0)

                                                    btr.AppendEntity(bWeb)
                                                    transaction.AddNewlyCreatedDBObject(bWeb, True)

                                                End If


                                            End If

                                        End If

                                    End If

                                End If


                                ' ADD Block Table Record To Block Table
                                Dim ms As BlockTableRecord = DirectCast(transaction.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), 
                                                            BlockTableRecord)

                                Dim br As New BlockReference(entryPoint, btrId)

                                ms.AppendEntity(br)
                                transaction.AddNewlyCreatedDBObject(br, True)

                            End Using


                            transaction.Commit()

                            editor.WriteMessage(ControlChars.Lf)

                        Catch ex As Exception

                            editor.WriteMessage("! A Problem Has Occured - " + ex.Message)
                        Finally
                            transaction.Dispose()
                            editor.WriteMessage(ControlChars.Lf)
                        End Try
                    End Using

                    ' End Transaction

                End If
            End If
        End If
    End Sub

End Class

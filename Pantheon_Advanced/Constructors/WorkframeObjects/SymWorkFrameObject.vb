
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

    Public Flush As Boolean
    Public Bypass As Boolean

    Public Sub New(ByVal mark As String, ByVal eaveHeight As Double, ByVal width As Double, ByVal length As Double,
                       ByVal pitch As Double, ByVal purlinWeb As Double, ByVal girtWeb As Double,
                       ByVal flush As Boolean, ByVal bypass As Boolean)

        Me.EaveHeight = eaveHeight
        Me.Width = width
        Me.Length = length
        Me.Pitch = pitch
        Me.PurlinWeb = purlinWeb
        Me.GirtWeb = girtWeb
        Me.Flush = flush
        Me.Bypass = bypass

        Me.Mark = mark

    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Const layerName As String = "WorkFrames"
        Const layerDesc As String = "Standard WorkFrame Layer"
        Const specialColor As Integer = 3


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

                Dim entryOption As PromptPointOptions = New PromptPointOptions("Select Bottom Left Entry Point")
                Dim entryResult As PromptPointResult = editor.GetPoint(entryOption)

                If entryResult.Status = PromptStatus.OK Then
                    Using btr As New BlockTableRecord
                        btr.Name = Mark

                        ' Block Insertion Point
                        Dim entryPoint As Point3d = New Point3d(entryResult.Value.X, entryResult.Value.Y, 0)
                        btr.Origin = entryPoint

                        bt.UpgradeOpen()
                        Dim btrId As ObjectId = bt.Add(btr)
                        transaction.AddNewlyCreatedDBObject(btr, True)


                        If (Flush) Then

                            TriSolv.RunIn(Width / 2, Pitch)
                            Dim peakRise As Double = TriSolv.Rise

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + EaveHeight)
                            Dim peakPoint As Point2d = New Point2d(botLeft.X + (Width / 2), topLeft.Y + peakRise)
                            Dim topRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y + EaveHeight)
                            Dim botRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y)

                            Dim symWorkFrame As Polyline = New Polyline
                            symWorkFrame.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            symWorkFrame.AddVertexAt(0, botLeft, 0, 0, 0)
                            symWorkFrame.AddVertexAt(1, topLeft, 0, 0, 0)
                            symWorkFrame.AddVertexAt(2, peakPoint, 0, 0, 0)
                            symWorkFrame.AddVertexAt(3, topRight, 0, 0, 0)
                            symWorkFrame.AddVertexAt(4, botRight, 0, 0, 0)
                            symWorkFrame.AddVertexAt(5, botLeft, 0, 0, 0)

                            TriSolv.RunIn(PurlinWeb, Pitch)
                            Dim purlinSlope As Double = TriSolv.Slope

                            Dim purLeft As Point2d = New Point2d(topLeft.X, topLeft.Y - purlinSlope)
                            Dim purPeak As Point2d = New Point2d(peakPoint.X, peakPoint.Y - purlinSlope)
                            Dim purRight As Point2d = New Point2d(topRight.X, topRight.Y - purlinSlope)

                            Dim purLine As Polyline = New Polyline
                            purLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            purLine.AddVertexAt(0, purLeft, 0, 0, 0)
                            purLine.AddVertexAt(1, purPeak, 0, 0, 0)
                            purLine.AddVertexAt(2, purRight, 0, 0, 0)

                            Dim midBot As Point3d = New Point3d(peakPoint.X, botLeft.Y, 0)
                            Dim midTop As Point3d = New Point3d(peakPoint.X, peakPoint.Y, 0)

                            Dim midLine As Line = New Line(midBot, midTop)
                            midLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1)

                            btr.AppendEntity(symWorkFrame)
                            btr.AppendEntity(purLine)
                            btr.AppendEntity(midLine)

                            transaction.AddNewlyCreatedDBObject(symWorkFrame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)
                            transaction.AddNewlyCreatedDBObject(midLine, True)

                        ElseIf (Bypass) Then

                            TriSolv.RunIn(Width / 2, Pitch)
                            Dim peakRise As Double = TriSolv.Rise

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + EaveHeight)
                            Dim peakPoint As Point2d = New Point2d(botLeft.X + (Width / 2), topLeft.Y + peakRise)
                            Dim topRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y + EaveHeight)
                            Dim botRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y)

                            Dim symWorkFrame As Polyline = New Polyline
                            symWorkFrame.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            symWorkFrame.AddVertexAt(0, botLeft, 0, 0, 0)
                            symWorkFrame.AddVertexAt(1, topLeft, 0, 0, 0)
                            symWorkFrame.AddVertexAt(2, peakPoint, 0, 0, 0)
                            symWorkFrame.AddVertexAt(3, topRight, 0, 0, 0)
                            symWorkFrame.AddVertexAt(4, botRight, 0, 0, 0)
                            symWorkFrame.AddVertexAt(5, botLeft, 0, 0, 0)

                            TriSolv.RunIn(PurlinWeb, Pitch)
                            Dim purlinSlope As Double = TriSolv.Slope

                            TriSolv.RunIn(GirtWeb, Pitch)
                            Dim girtRise As Double = TriSolv.Rise

                            Dim purLeft As Point2d = New Point2d(topLeft.X + GirtWeb, topLeft.Y - purlinSlope + girtRise)
                            Dim purPeak As Point2d = New Point2d(peakPoint.X, peakPoint.Y - purlinSlope)
                            Dim purRight As Point2d = New Point2d(topRight.X - GirtWeb,
                                                                  topRight.Y - purlinSlope + girtRise)

                            Dim purLine As Polyline = New Polyline
                            purLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            purLine.AddVertexAt(0, purLeft, 0, 0, 0)
                            purLine.AddVertexAt(1, purPeak, 0, 0, 0)
                            purLine.AddVertexAt(2, purRight, 0, 0, 0)

                            Dim girtLeft As Line = New Line(New Point3d(botLeft.X + GirtWeb, botLeft.Y, 0),
                                                            New Point3d(botLeft.X + GirtWeb, purLeft.Y, 0))
                            Dim girtRight As Line = New Line(New Point3d(botRight.X - GirtWeb, botLeft.Y, 0),
                                                             New Point3d(botRight.X - GirtWeb, purRight.Y, 0))

                            girtLeft.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)
                            girtRight.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            Dim midBot As Point3d = New Point3d(peakPoint.X, botLeft.Y, 0)
                            Dim midTop As Point3d = New Point3d(peakPoint.X, peakPoint.Y, 0)

                            Dim midLine As Line = New Line(midBot, midTop)
                            midLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1)

                            btr.AppendEntity(symWorkFrame)
                            btr.AppendEntity(purLine)
                            btr.AppendEntity(girtLeft)
                            btr.AppendEntity(girtRight)
                            btr.AppendEntity(midLine)

                            transaction.AddNewlyCreatedDBObject(symWorkFrame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)
                            transaction.AddNewlyCreatedDBObject(girtLeft, True)
                            transaction.AddNewlyCreatedDBObject(girtRight, True)
                            transaction.AddNewlyCreatedDBObject(midLine, True)

                        End If

                        ' ADD Block Table Record To Block Table
                        Dim ms As BlockTableRecord = DirectCast(transaction.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), 
                                                    BlockTableRecord)

                        Dim br As New BlockReference(entryPoint, btrId)

                        ms.AppendEntity(br)
                        transaction.AddNewlyCreatedDBObject(br, True)

                    End Using
                End If

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

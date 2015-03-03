Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime

Public Class SingWorkFrameObject

    Public Mark As String
    Public LowHeight As Double
    Public HighHeight As Double
    Public Width As Double
    Public Length As Double
    Public Pitch As Double
    Public PurlinWeb As Double
    Public GirtWeb As Double

    Public Flush As Boolean
    Public Bypass As Boolean

    Public Sub New(ByVal mark As String, ByVal lowHeight As Double, ByVal highHeight As Double, ByVal width As Double, ByVal length As Double,
                       ByVal pitch As Double, ByVal purlinWeb As Double, ByVal girtWeb As Double,
                       ByVal flush As Boolean, ByVal bypass As Boolean)

        Me.LowHeight = lowHeight
        Me.HighHeight = highHeight
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

        Const layerName As String = "WorkFrame"
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

                            Dim entryPoint As Point3d = New Point3d(entryResult.Value.X, entryResult.Value.Y, entryResult.Value.Z)
                            btr.Origin = entryPoint

                            bt.UpgradeOpen()
                            Dim btrId As ObjectId = bt.Add(btr)
                            transaction.AddNewlyCreatedDBObject(btr, True)

                           
                        If (Flush) Then

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + LowHeight)
                            Dim topRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y + HighHeight)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim singleWorkFrame As Polyline = New Polyline

                            singleWorkFrame.AddVertexAt(0, botLeft, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(1, topLeft, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(2, topRight, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(3, botRight, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(4, botLeft, 0, 0, 0)

                            singleWorkFrame.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            TriSolv.RunIn(PurlinWeb, Pitch)
                            Dim purlinSlope As Double = TriSolv.Slope

                            Dim purLine As Line = New Line(New Point3d(topLeft.X, topLeft.Y - purlinSlope, 0),
                                                           New Point3d(topRight.X, topRight.Y - purlinSlope, 0))
                            purLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(singleWorkFrame)
                            btr.AppendEntity(purLine)

                            transaction.AddNewlyCreatedDBObject(singleWorkFrame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)

                        Else

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + LowHeight)
                            Dim topRight As Point2d = New Point2d(botLeft.X + Width, botLeft.Y + HighHeight)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim singleWorkFrame As Polyline = New Polyline

                            singleWorkFrame.AddVertexAt(0, botLeft, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(1, topLeft, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(2, topRight, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(3, botRight, 0, 0, 0)
                            singleWorkFrame.AddVertexAt(4, botLeft, 0, 0, 0)

                            singleWorkFrame.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            TriSolv.RunIn(PurlinWeb, Pitch)
                            Dim purlinSlope As Double = TriSolv.Slope

                            TriSolv.RunIn(GirtWeb, Pitch)
                            Dim girtRise As Double = TriSolv.Rise

                            Dim purLeft As Point3d = New Point3d(topLeft.X + GirtWeb,
                                                                 topLeft.Y - purlinSlope + girtRise, 0)
                            Dim purRight As Point3d = New Point3d(topRight.X - GirtWeb,
                                                                  topRight.Y - purlinSlope - girtRise, 0)
                            Dim purLine As Line = New Line(purLeft, purRight)

                            purLine.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            Dim girtLineLeft As Line = New Line(New Point3d(purLeft.X, botLeft.Y, 0), purLeft)
                            Dim girtLineRight As Line = New Line(New Point3d(purRight.X, botLeft.Y, 0), purRight)

                            girtLineLeft.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)
                            girtLineRight.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(singleWorkFrame)
                            btr.AppendEntity(purLine)
                            btr.AppendEntity(girtLineLeft)
                            btr.AppendEntity(girtLineRight)

                            transaction.AddNewlyCreatedDBObject(singleWorkFrame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)
                            transaction.AddNewlyCreatedDBObject(girtLineLeft, True)
                            transaction.AddNewlyCreatedDBObject(girtLineRight, True)


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

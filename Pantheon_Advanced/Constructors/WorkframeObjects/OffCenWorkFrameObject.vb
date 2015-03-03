Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class OffCenWorkFrameObject

    Public Mark As String

    Public LeftEave As Double
    Public LeftWidth As Double
    Public LeftLength As Double
    Public LeftPitch As Double
    Public LeftPurlin As Double
    Public LeftGirt As Double
    Public LeftFlush As Boolean
    Public LeftBypass As Boolean

    Public RightEave As Double
    Public RightWidth As Double
    Public RightLength As Double
    Public RightPitch As Double
    Public RightPurlin As Double
    Public RightGirt As Double
    Public RightFlush As Boolean
    Public RightBypass As Boolean

    Public Sub New(ByVal mark As String, ByVal leftEave As Double, ByVal leftWidth As Double, ByVal leftLength As Double,
                   ByVal leftPitch As Double, ByVal leftPurlin As Double, ByVal leftGirt As Double,
                   ByVal leftFlush As Boolean, ByVal leftBypass As Boolean,
                   ByVal rightEave As Double, ByVal rightWidth As Double, ByVal rightLength As Double,
                   ByVal rightPitch As Double, ByVal rightPurlin As Double, ByVal rightGirt As Double,
                   ByVal rightFlush As Boolean, ByVal rightBypass As Boolean)

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

                        If (LeftFlush) Then

                            TriSolv.RunIn(LeftWidth, LeftPitch)
                            Dim peak = TriSolv.Rise

                            TriSolv.RunIn(LeftPurlin, LeftPitch)
                            Dim purlinSlope = TriSolv.Slope

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + LeftEave)
                            Dim topRight As Point2d = New Point2d(botLeft.X + LeftWidth, topLeft.Y + peak)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim frame As Polyline = New Polyline
                            frame.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            frame.AddVertexAt(0, botLeft, 0, 0, 0)
                            frame.AddVertexAt(1, topLeft, 0, 0, 0)
                            frame.AddVertexAt(2, topRight, 0, 0, 0)
                            frame.AddVertexAt(3, botRight, 0, 0, 0)
                            frame.AddVertexAt(4, botLeft, 0, 0, 0)

                            Dim purLeft As Point3d = New Point3d(topLeft.X, topLeft.Y - purlinSlope, 0)
                            Dim purPeak As Point3d = New Point3d(topRight.X, topRight.Y - purlinSlope, 0)

                            Dim purLine As Line = New Line(purLeft, purPeak)
                            purLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(frame)
                            btr.AppendEntity(purLine)

                            transaction.AddNewlyCreatedDBObject(frame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)

                        Else

                            TriSolv.RunIn(LeftWidth, LeftPitch)
                            Dim peak = TriSolv.Rise

                            TriSolv.RunIn(LeftPurlin, LeftPitch)
                            Dim purlinSlope = TriSolv.Slope

                            TriSolv.RunIn(LeftGirt, LeftPitch)
                            Dim girtRise = TriSolv.Rise

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + LeftEave)
                            Dim topRight As Point2d = New Point2d(botLeft.X + LeftWidth, topLeft.Y + peak)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim frame As Polyline = New Polyline
                            frame.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            frame.AddVertexAt(0, botLeft, 0, 0, 0)
                            frame.AddVertexAt(1, topLeft, 0, 0, 0)
                            frame.AddVertexAt(2, topRight, 0, 0, 0)
                            frame.AddVertexAt(3, botRight, 0, 0, 0)
                            frame.AddVertexAt(4, botLeft, 0, 0, 0)

                            Dim purLeft As Point3d = New Point3d(topLeft.X + LeftGirt,
                                                                 topLeft.Y - purlinSlope + girtRise, 0)
                            Dim purPeak As Point3d = New Point3d(topRight.X, topRight.Y - purlinSlope, 0)

                            Dim purLine As Line = New Line(purLeft, purPeak)
                            purLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            Dim leftGirtLine As Line = New Line(New Point3d(purLeft.X, botLeft.Y, 0),
                                                                New Point3d(purLeft.X, purLeft.Y, 0))

                            leftGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(frame)
                            btr.AppendEntity(purLine)
                            btr.AppendEntity(leftGirtLine)

                            transaction.AddNewlyCreatedDBObject(frame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)
                            transaction.AddNewlyCreatedDBObject(leftGirtLine, True)

                        End If

                        If (RightFlush) Then

                            TriSolv.RunIn(RightWidth, RightPitch)
                            Dim peak = TriSolv.Rise

                            TriSolv.RunIn(RightPurlin, RightPitch)
                            Dim purlinSlope = TriSolv.Slope

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X + LeftWidth, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + RightEave + peak)
                            Dim topRight As Point2d = New Point2d(botLeft.X + RightWidth, botLeft.Y + RightEave)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim frame As Polyline = New Polyline
                            frame.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            frame.AddVertexAt(0, botLeft, 0, 0, 0)
                            frame.AddVertexAt(1, topLeft, 0, 0, 0)
                            frame.AddVertexAt(2, topRight, 0, 0, 0)
                            frame.AddVertexAt(3, botRight, 0, 0, 0)
                            frame.AddVertexAt(4, botLeft, 0, 0, 0)

                            Dim purLeft As Point3d = New Point3d(topLeft.X, topLeft.Y - purlinSlope, 0)
                            Dim purPeak As Point3d = New Point3d(topRight.X, topRight.Y - purlinSlope, 0)

                            Dim purLine As Line = New Line(purLeft, purPeak)
                            purLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(frame)
                            btr.AppendEntity(purLine)

                            transaction.AddNewlyCreatedDBObject(frame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)

                        Else

                            TriSolv.RunIn(RightWidth, RightPitch)
                            Dim peak = TriSolv.Rise

                            TriSolv.RunIn(RightPurlin, RightPitch)
                            Dim purlinSlope = TriSolv.Slope

                            TriSolv.RunIn(LeftGirt, LeftPitch)
                            Dim girtRise = TriSolv.Rise

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X + LeftWidth, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + RightEave + peak)
                            Dim topRight As Point2d = New Point2d(botLeft.X + RightWidth, botLeft.Y + RightEave)
                            Dim botRight As Point2d = New Point2d(topRight.X, botLeft.Y)

                            Dim frame As Polyline = New Polyline
                            frame.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 3)

                            frame.AddVertexAt(0, botLeft, 0, 0, 0)
                            frame.AddVertexAt(1, topLeft, 0, 0, 0)
                            frame.AddVertexAt(2, topRight, 0, 0, 0)
                            frame.AddVertexAt(3, botRight, 0, 0, 0)
                            frame.AddVertexAt(4, botLeft, 0, 0, 0)

                            Dim purLeft As Point3d = New Point3d(topLeft.X, topLeft.Y - purlinSlope, 0)
                            Dim purRight As Point3d = New Point3d(topRight.X - RightGirt,
                                                                  topRight.Y - purlinSlope + girtRise, 0)

                            Dim purLine As Line = New Line(purLeft, purRight)
                            purLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            Dim rightGirtLine As Line = New Line(New Point3d(purRight.X, botRight.Y, 0),
                                                                 New Point3d(purRight.X, purRight.Y, 0))

                            rightGirtLine.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 2)

                            btr.AppendEntity(frame)
                            btr.AppendEntity(purLine)
                            btr.AppendEntity(rightGirtLine)

                            transaction.AddNewlyCreatedDBObject(frame, True)
                            transaction.AddNewlyCreatedDBObject(purLine, True)
                            transaction.AddNewlyCreatedDBObject(rightGirtLine, True)

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

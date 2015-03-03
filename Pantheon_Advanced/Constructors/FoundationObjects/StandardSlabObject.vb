Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class StandardSlabObject

    Public Mark As String
    Public Width As Double
    Public Length As Double
    Public NotchYes As Boolean
    Public NotchNo As Boolean
    Public NotchAmount As Double

    Public Sub New(ByVal mark As String, ByVal width As Double, ByVal length As Double, ByVal notchYes As Boolean, ByVal notchNo As Boolean, ByVal notchAmount As Double)

        Me.Mark = mark
        Me.Width = width
        Me.Length = length
        Me.NotchYes = notchYes
        Me.NotchNo = notchNo
        Me.NotchAmount = notchAmount

    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Const layerName As String = "SLAB"
        Const layerDesc As String = "Foundation Layer"
        Const layerColor As Integer = 8

       

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
                        newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, layerColor)
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

                            Dim botLeft As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                            Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + Width)
                            Dim toPRight As Point2d = New Point2d(botLeft.X + Length, botLeft.Y + Width)
                            Dim botRight As Point2d = New Point2d(botLeft.X + Length, botLeft.Y)

                            Dim slab As Polyline = New Polyline()
                            slab.AddVertexAt(0, botLeft, 0, 0, 0)
                            slab.AddVertexAt(1, topLeft, 0, 0, 0)
                            slab.AddVertexAt(2, toPRight, 0, 0, 0)
                            slab.AddVertexAt(3, botRight, 0, 0, 0)
                            slab.AddVertexAt(4, botLeft, 0, 0, 0)

                            slab.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 7)
                            btr.AppendEntity(slab)

                            transaction.AddNewlyCreatedDBObject(slab, True)

                            If NotchYes Then

                                '' Open the Linetype table for read
                                Dim acLineTypTbl As LinetypeTable
                                acLineTypTbl = transaction.GetObject(dwg.LinetypeTableId, _
                                                                 OpenMode.ForRead)

                                Dim sLineTypName As String = "HIDDEN"

                                If acLineTypTbl.Has(sLineTypName) = False Then

                                    '' Load the Hidden Linetype
                                    dwg.LoadLineTypeFile(sLineTypName, "acad.lin")
                                End If


                                Dim doubleNotch As Double = NotchAmount * 2

                                Dim notchBotLeft As Point2d = New Point2d(entryResult.Value.X - NotchAmount, entryResult.Value.Y - NotchAmount)
                                Dim notchTopLeft As Point2d = New Point2d(notchBotLeft.X, notchBotLeft.Y + Width + doubleNotch)
                                Dim notchTopRight As Point2d = New Point2d(notchTopLeft.X + Length + doubleNotch, notchTopLeft.Y)
                                Dim notchBotRight As Point2d = New Point2d(notchTopRight.X, notchBotLeft.Y)

                                Dim notch As Polyline = New Polyline()
                                notch.AddVertexAt(0, notchBotLeft, 0, 0, 0)
                                notch.AddVertexAt(1, notchTopLeft, 0, 0, 0)
                                notch.AddVertexAt(2, notchTopRight, 0, 0, 0)
                                notch.AddVertexAt(3, notchBotRight, 0, 0, 0)
                                notch.AddVertexAt(4, notchBotLeft, 0, 0, 0)

                                notch.Linetype = sLineTypName

                                btr.AppendEntity(notch)
                                transaction.AddNewlyCreatedDBObject(notch, True)


                            End If



                            ' ADD Block Table Record To Block Table
                            Dim ms As BlockTableRecord = DirectCast(transaction.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), 
                                                        BlockTableRecord)

                            Dim br As New BlockReference(entryPoint, btrId)

                            ms.AppendEntity(br)
                            transaction.AddNewlyCreatedDBObject(br, True)

                        End Using

                    End If
                End If

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


    End Sub

End Class

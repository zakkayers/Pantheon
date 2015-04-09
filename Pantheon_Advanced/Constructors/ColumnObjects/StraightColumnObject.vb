Imports Autodesk.AutoCAD.ApplicationServices
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

    Public Pitch As Double

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

    Public Bolts As Boolean
    Public BoltLengthString As String
    Public BoltLength As Double


    Public Sub New(ByVal mark As String, ByVal flush As Boolean, ByVal bypass As Boolean,
                   girtList As List(Of GirtObject), cableObj As CableObject, anchorObj As AnchorObject,
                   ByVal flangeHoles As Boolean, ByVal block As Boolean, ByVal eaveThick As Double,
                   ByVal eaveWidth As Double, ByVal stiffThick As Double, ByVal stiffWidth As Double,
                   ByVal outerThick As Double, ByVal outerWidth As Double, ByVal innerThick As Double,
                   ByVal innerWidth As Double, ByVal webThick As Double, ByVal webDepth As Double,
                   ByVal haunchThick As Double, ByVal haunchWidth As Double, ByVal haunchLength As Double,
                   ByVal baseThick As Double, ByVal baseWidth As Double, ByVal bolts As Boolean,
                   ByVal boltLengthString As String, ByVal boltLength As Double)

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
        Me.Bolts = bolts
        Me.BoltLengthString = boltLengthString
        Me.BoltLength = boltLength
    End Sub

    Public Sub Draw()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

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

                            Make.Layers()

                            ' Create Block
                            Dim bt As BlockTable
                            bt = transaction.GetObject(dwg.BlockTableId, OpenMode.ForRead)


                            Using btr As New BlockTableRecord
                                btr.Name = Mark

                                ' Block Insertion Point
                                Dim entryPoint As Point3d = New Point3d(baseResults.Value.X, baseResults.Value.Y,
                                                                        baseResults.Value.Z)
                                btr.Origin = entryPoint

                                bt.UpgradeOpen()
                                Dim btrId As ObjectId = bt.Add(btr)
                                transaction.AddNewlyCreatedDBObject(btr, True)

                                ' Set To Plate Layer
                                Dim ltb As LayerTable = DirectCast(transaction.GetObject(dwg.LayerTableId, OpenMode.ForRead), 
                                                                   LayerTable)
                                dwg.Clayer = ltb("Plates")

                                ' Draw Column

                                Dim basePoint As Point3d = New Point3d(baseResults.Value.X, baseResults.Value.Y, baseResults.Value.Z)
                                Dim eavePoint As Point3d = New Point3d(eaveResults.Value.X, eaveResults.Value.Y, eaveResults.Value.Z)
                                Dim pitchPoint As Point3d = New Point3d(pitchResults.Value.X, pitchResults.Value.Y, pitchResults.Value.Z)

                                ' BASE PLATE

                                Using basePlate As Solid3d = New Solid3d()

                                    Dim baseLength = WebDepth + OuterThick + InnerThick

                                    basePlate.CreateBox(BaseWidth, baseLength, BaseThick)


                                    '' Add the new object to the block table record and the transaction
                                    btr.AppendEntity(basePlate)
                                    transaction.AddNewlyCreatedDBObject(basePlate, True)

                                    Dim boltHoles As Point3d() = New Point3d() _
                                            {New Point3d(-2, (baseLength / 2) - AnchorObj.FromFlange, 0),
                                             New Point3d(2, (baseLength / 2) - AnchorObj.FromFlange, 0),
                                             New Point3d(-2, (baseLength / 2) - (AnchorObj.FromFlange + AnchorObj.Between), 0),
                                             New Point3d(2, (baseLength / 2) - (AnchorObj.FromFlange + AnchorObj.Between), 0)}


                                    For Each hole In boltHoles

                                        Using baseHole As Solid3d = New Solid3d()

                                            baseHole.CreateFrustum(BaseThick, AnchorObj.Diam, AnchorObj.Diam, AnchorObj.Diam)
                                            baseHole.TransformBy(
                                                Matrix3d.Displacement(New Point3d(hole.X, hole.Y, hole.Z) - Point3d.Origin))

                                            btr.AppendEntity(baseHole)
                                            transaction.AddNewlyCreatedDBObject(baseHole, True)

                                            basePlate.BooleanOperation(BooleanOperationType.BoolSubtract, baseHole)

                                        End Using

                                    Next

                                End Using


                                ' ADD Block Table Record To Block Table
                                Dim ms As BlockTableRecord = DirectCast(transaction.GetObject(bt(BlockTableRecord.ModelSpace),
                                                                                              OpenMode.ForWrite), 
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

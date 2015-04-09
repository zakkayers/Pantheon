Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput

Public Class Make

    Public Shared Sub Layers()

        MakeLayer("Columns", "Standard Column Layer", 150)
        MakeLayer("Plates", "Standard Plate Layer", 2)
        MakeLayer("Angles", "Standard Plate Layer", 3)
        MakeLayer("Bolts", "Standard Cowell Bolts", 30)

    End Sub


    Private Shared Sub MakeLayer(ByVal layerName As String, ByVal description As String, ByVal color As Integer)

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        ' Start Transaction

        Using doc.LockDocument()
            Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
            Try

                ' Create E-Sheet & Detail Layers
                Dim ltb As LayerTable = DirectCast(transaction.GetObject(dwg.LayerTableId, OpenMode.ForRead), LayerTable)
                If Not ltb.Has(layerName) Then
                    ltb.UpgradeOpen()
                    Dim newLayer As New LayerTableRecord()
                    newLayer.Name = layerName
                    newLayer.Description = description
                    newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, color)
                    ltb.Add(newLayer)
                    transaction.AddNewlyCreatedDBObject(newLayer, True)
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

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class WShape

    Public Name As String
    Public LetterCall As String
    Public NumCall As String

    Public TopThick As Double
    Public TopWidth As Double

    Public WebThick As Double
    Public WebDepth As Double

    Public BotThick As Double
    Public BotWidth As Double

    Public Sub New(ByVal name As String, ByVal topT As Double, ByVal topW As Double,
                   ByVal webT As Double, ByVal webD As Double, ByVal botT As Double, ByVal botW As Double)

        Me.Name = name

        Dim parts As String() = name.Split("x")

        LetterCall = parts(0)
        NumCall = parts(1)

        TopThick = topT
        TopWidth = topW

        WebThick = webT
        WebDepth = webD

        BotThick = botT
        BotWidth = botW

    End Sub

    Public Sub DrawBeamShape()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Dim entryPoint As PromptPointOptions = New PromptPointOptions(ControlChars.Lf + "Select Entry Point : ")
        Dim entryResult As PromptPointResult = editor.GetPoint(entryPoint)

        If (entryResult.Status = PromptStatus.OK) Then

            ' Start Transaction

            Using doc.LockDocument()
                Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                Try

                    Dim entry As Point2d = New Point2d(entryResult.Value.X, entryResult.Value.Y)
                    Dim fullDepth = WebDepth + TopThick + BotThick

                    Dim pt1 As Point2d = New Point2d(entry.X - (TopWidth / 2), entry.Y - (fullDepth / 2))
                    Dim pt2 As Point2d = New Point2d(pt1.X + TopWidth, pt1.Y)
                    Dim pt3 As Point2d = New Point2d(pt2.X, pt1.Y - TopThick)
                    Dim pt4 As Point2d = New Point2d(pt3.X - (TopWidth / 2) + (WebThick / 2), pt3.Y)
                    Dim pt5 As Point2d = New Point2d(pt4.X, pt4.Y - WebDepth)
                    Dim pt6 As Point2d = New Point2d(entry.X + (BotWidth / 2), pt5.Y)
                    Dim pt7 As Point2d = New Point2d(pt6.X, pt6.Y - BotThick)
                    Dim pt8 As Point2d = New Point2d(pt7.X - BotWidth, pt7.Y)
                    Dim pt9 As Point2d = New Point2d(pt8.X, pt8.Y + BotThick)
                    Dim pt10 As Point2d = New Point2d(pt5.X - WebThick, pt5.Y)
                    Dim pt11 As Point2d = New Point2d(pt10.X, pt4.Y)
                    Dim pt12 As Point2d = New Point2d(pt1.X, pt11.Y)

                    Dim beam As Polyline = New Polyline

                    beam.AddVertexAt(0, pt1, 0, 0, 0)
                    beam.AddVertexAt(1, pt2, 0, 0, 0)
                    beam.AddVertexAt(2, pt3, 0, 0, 0)
                    beam.AddVertexAt(3, pt4, 0, 0, 0)
                    beam.AddVertexAt(4, pt5, 0, 0, 0)
                    beam.AddVertexAt(5, pt6, 0, 0, 0)
                    beam.AddVertexAt(6, pt7, 0, 0, 0)
                    beam.AddVertexAt(7, pt8, 0, 0, 0)
                    beam.AddVertexAt(8, pt9, 0, 0, 0)
                    beam.AddVertexAt(9, pt10, 0, 0, 0)
                    beam.AddVertexAt(10, pt11, 0, 0, 0)
                    beam.AddVertexAt(11, pt12, 0, 0, 0)
                    beam.AddVertexAt(12, pt1, 0, 0, 0)

                    Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite)

                    btr.AppendEntity(beam)

                    transaction.AddNewlyCreatedDBObject(beam, True)
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
    End Sub

End Class

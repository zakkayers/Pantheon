Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry

Public Class CeeShape
    Public Guage As Integer = 16
    Public Web As Double = 8
    Public Flange As Double = 3.5

    Public JambHor As Boolean
    Public JambVer As Boolean
    Public PostHor As Boolean
    Public PostVer As Boolean

    Public Sub Draw()

        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim keywordOptions As PromptKeywordOptions =
                New PromptKeywordOptions(ControlChars.Lf + "Enter Cee Type [Jamb/Post]", "Jamb Post")
        Dim keywordResult As PromptResult = editor.GetKeywords(keywordOptions)

        If keywordResult.Status = PromptStatus.OK Then

            Select Case keywordResult.StringResult

                Case "Jamb"

                    Dim orientOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Enter Orientation [Horizontal/Vertical]",
                                                     "Horizontal Vertical")
                    Dim orientResult As PromptResult = editor.GetKeywords(orientOptions)

                    If orientResult.Status = PromptStatus.OK Then

                        Select Case orientResult.StringResult

                            Case "Horizontal"

                                JambHor = True
                                JambVer = False
                                PostHor = False
                                PostVer = False

                                Flange = 2.5

                                DrawCee()

                            Case "Vertical"

                                JambHor = False
                                JambVer = True
                                PostHor = False
                                PostVer = False

                                Flange = 2.5

                                DrawCee()

                        End Select

                    End If

                Case "Post"

                    Dim orientOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Enter Orientation [Horizontal/Vertical]",
                                                     "Horizontal Vertical")
                    Dim orientResult As PromptResult = editor.GetKeywords(orientOptions)

                    If orientResult.Status = PromptStatus.OK Then

                        Select Case orientResult.StringResult

                            Case "Horizontal"

                                JambHor = False
                                JambVer = False
                                PostHor = True
                                PostVer = False

                                Flange = 3.5

                                DrawCee()

                            Case "Vertical"

                                JambHor = False
                                JambVer = False
                                PostHor = False
                                PostVer = True

                                Flange = 3.5

                                DrawCee()

                        End Select

                    End If

            End Select

        End If
    End Sub

    Public Sub DrawCee()

        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim editor As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim dwg As Database = editor.Document.Database

        Dim ceeOptions As PromptPointOptions =
                New PromptPointOptions(ControlChars.Lf + "Select Cee Mid Point or [Guage/Flange/Web] : ",
                                       "Guage Flange Web")
        Dim ceeResult As PromptPointResult = editor.GetPoint(ceeOptions)

        If ceeResult.Status = PromptStatus.Keyword Then

            Select Case ceeResult.StringResult

                Case "Guage"

                    Dim guageOptions As PromptKeywordOptions =
                            New PromptKeywordOptions(ControlChars.Lf + "Select Guage [Twelve/Fourteen/Sixteen] : ",
                                                     "Twelve Fourteen Sixteen")
                    Dim guageResult As PromptResult = editor.GetKeywords(guageOptions)

                    If (guageResult.Status = PromptStatus.OK) Then

                        Select Case guageResult.StringResult

                            Case "Twelve"
                                Guage = 12
                                DrawCee()
                            Case "Fourteen"
                                Guage = 14
                                DrawCee()
                            Case "Sixteen"
                                Guage = 16
                                DrawCee()
                        End Select

                    End If

                Case "Flange"
                    Dim flangeOptions As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Flange Length In Inches : ")
                    Dim flangeResult As PromptDoubleResult = editor.GetDistance(flangeOptions)

                    If (flangeResult.Status = PromptStatus.OK) Then

                        Flange = flangeResult.Value
                        DrawCee()

                    End If
                Case "Web"
                    Dim webOptions As PromptDistanceOptions =
                            New PromptDistanceOptions(ControlChars.Lf + "Enter Web Depth In Inches : ")
                    Dim webResult As PromptDoubleResult = editor.GetDistance(webOptions)

                    If (webResult.Status = PromptStatus.OK) Then

                        Web = webResult.Value
                        DrawCee()

                    End If
            End Select

        ElseIf ceeResult.Status = PromptStatus.OK Then

            ' Start Transaction
            Using doc.LockDocument()
                Dim transaction As Transaction = dwg.TransactionManager.StartTransaction()
                Try

                    If (PostHor Or JambHor) Then


                        Dim orientOption As PromptPointOptions =
                                New PromptPointOptions(ControlChars.Lf + "Select Orientation Point : ")
                        Dim orientResult As PromptPointResult = editor.GetPoint(orientOption)

                        If orientResult.Status = PromptStatus.OK Then

                            If orientResult.Value.Y > ceeResult.Value.Y Then


                                Dim botLeft As Point2d = New Point2d(ceeResult.Value.X - (Web / 2), ceeResult.Value.Y)
                                Dim botRight As Point2d = New Point2d(botLeft.X + Web, botLeft.Y)
                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y + Flange)
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


                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)
                                btr.AppendEntity(cee)

                                transaction.AddNewlyCreatedDBObject(cee, True)

                            Else

                                Dim botLeft As Point2d = New Point2d(ceeResult.Value.X - (Web / 2), ceeResult.Value.Y)
                                Dim botRight As Point2d = New Point2d(botLeft.X + Web, botLeft.Y)
                                Dim topLeft As Point2d = New Point2d(botLeft.X, botLeft.Y - Flange)
                                Dim topRight As Point2d = New Point2d(botRight.X, topLeft.Y)
                                Dim toeLeft As Point2d = New Point2d(topLeft.X + 1, topLeft.Y)
                                Dim toeRight As Point2d = New Point2d(topRight.X - 1, topRight.Y)

                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X, toeLeft.Y + 0.25)
                                Dim insideTopLeft2 As Point2d = New Point2d(topLeft.X + 0.25, insideTopLeft1.Y)
                                Dim insideBotLeft As Point2d = New Point2d(topLeft.X + 0.25, botLeft.Y - 0.25)
                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y - 0.25)
                                Dim insideTopRight2 As Point2d = New Point2d(insideBotRight.X, topRight.Y + 0.25)
                                Dim insideTopRight1 As Point2d = New Point2d(toeRight.X, toeRight.Y + 0.25)

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


                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)
                                btr.AppendEntity(cee)

                                transaction.AddNewlyCreatedDBObject(cee, True)

                            End If

                        End If

                    ElseIf (PostVer Or JambVer) Then

                        Dim orientOption As PromptPointOptions =
                                New PromptPointOptions(ControlChars.Lf + "Select Orientation Point : ")
                        Dim orientResult As PromptPointResult = editor.GetPoint(orientOption)

                        If orientResult.Status = PromptStatus.OK Then

                            If orientResult.Value.X > ceeResult.Value.X Then

                                Dim botLeft As Point2d = New Point2d(ceeResult.Value.X, ceeResult.Value.Y + (Web / 2))
                                Dim botRight As Point2d = New Point2d(botLeft.X, botLeft.Y - Web)
                                Dim topLeft As Point2d = New Point2d(botLeft.X + Flange, botLeft.Y)
                                Dim topRight As Point2d = New Point2d(botRight.X + Flange, botRight.Y)
                                Dim toeLeft As Point2d = New Point2d(topLeft.X, topLeft.Y - 1)
                                Dim toeRight As Point2d = New Point2d(topRight.X, topRight.Y + 1)

                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X - 0.25, toeLeft.Y)
                                Dim insideTopLeft2 As Point2d = New Point2d(insideTopLeft1.X, topLeft.Y - 0.25)
                                Dim insideBotLeft As Point2d = New Point2d(botLeft.X + 0.25, botLeft.Y - 0.25)
                                Dim insideBotRight As Point2d = New Point2d(botRight.X + 0.25, botRight.Y + 0.25)
                                Dim insideTopRight2 As Point2d = New Point2d(topRight.X - 0.25, topRight.Y + 0.25)
                                Dim insideTopRight1 As Point2d = New Point2d(insideTopRight2.X, toeRight.Y)

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


                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)
                                btr.AppendEntity(cee)

                                transaction.AddNewlyCreatedDBObject(cee, True)

                            Else

                                Dim botLeft As Point2d = New Point2d(ceeResult.Value.X, ceeResult.Value.Y + (Web / 2))
                                Dim botRight As Point2d = New Point2d(botLeft.X, botLeft.Y - Web)
                                Dim topLeft As Point2d = New Point2d(botLeft.X - Flange, botLeft.Y)
                                Dim topRight As Point2d = New Point2d(botRight.X - Flange, botRight.Y)
                                Dim toeLeft As Point2d = New Point2d(topLeft.X, topLeft.Y - 1)
                                Dim toeRight As Point2d = New Point2d(topRight.X, topRight.Y + 1)

                                Dim insideTopLeft1 As Point2d = New Point2d(toeLeft.X + 0.25, toeLeft.Y)
                                Dim insideTopLeft2 As Point2d = New Point2d(insideTopLeft1.X, topLeft.Y - 0.25)
                                Dim insideBotLeft As Point2d = New Point2d(botLeft.X - 0.25, botLeft.Y - 0.25)
                                Dim insideBotRight As Point2d = New Point2d(botRight.X - 0.25, botRight.Y + 0.25)
                                Dim insideTopRight2 As Point2d = New Point2d(topRight.X + 0.25, topRight.Y + 0.25)
                                Dim insideTopRight1 As Point2d = New Point2d(insideTopRight2.X, toeRight.Y)

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


                                Dim btr As BlockTableRecord = transaction.GetObject(dwg.CurrentSpaceId,
                                                                                    OpenMode.ForWrite)
                                btr.AppendEntity(cee)

                                transaction.AddNewlyCreatedDBObject(cee, True)

                            End If

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

        End If
    End Sub
End Class

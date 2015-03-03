
Imports Autodesk.Windows
Imports Autodesk.Windows.ToolBars

' METHOD: APPMENU
' Adds "Save Pantheon" & "Load Panehton" Buttons To The Application Menu
Public Class QuickAccess
    Public Sub AppMenu()


        Dim menu As ApplicationMenu = ComponentManager.ApplicationMenu

        If menu IsNot Nothing AndAlso menu.MenuContent IsNot Nothing Then

            ' Create Application Menu Item

            Dim save As ApplicationMenuItem = Sculpt.MakeAppMenuitem("Save Pantheon", "Save Current Pantheon Design For Later Use", My.Resources.Save_S, My.Resources.Save_L, " ")
            Dim load As ApplicationMenuItem = Sculpt.MakeAppMenuitem("Load Pantheon", "Load Previously Save Pantheon Design", My.Resources.Load_S, My.Resources.Load_L, " ")

            ' Add Menu Content

            menu.MenuContent.Items.Add(save)
            menu.MenuContent.Items.Add(load)

        End If
    End Sub

    ' METHOD: QUICKTOOLBAR
    ' Adds "Save Pantheon" & "Load Panehton" Buttons To The Quick Access Toolbar
    Public Sub QuickToolbar()


        Dim qat As QuickAccessToolBarSource = ComponentManager.QuickAccessToolBar

        If qat IsNot Nothing Then

            ' Create Buttons

            Dim save As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Save Pantheon Project",
                                                       "Save Current Pantheon Design For Later Use", My.Resources.Save_S,
                                                       My.Resources.Save_L, " ")
            Dim load As RibbonButton = Sculpt.MakeButton("Standard", "Standard", "Load Pantheon Project",
                                                       "Load Previously Saved Pantheon Design", My.Resources.Load_S,
                                                       My.Resources.Load_L, " ")

            ' Add To Quick Access Toolbar

            qat.AddStandardItem(save)
            qat.AddStandardItem(load)

        End If
    End Sub
End Class



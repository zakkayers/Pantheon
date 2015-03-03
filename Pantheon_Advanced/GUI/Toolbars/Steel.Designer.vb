<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Steel
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RakeAngle = New Telerik.WinControls.UI.RadButton()
        Me.CornerAngle = New Telerik.WinControls.UI.RadButton()
        Me.BaseAngle = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel4 = New Telerik.WinControls.UI.RadLabel()
        Me.EaveDown = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.EaveUp = New Telerik.WinControls.UI.RadButton()
        Me.CeeBase = New Telerik.WinControls.UI.RadButton()
        Me.CeeGirt = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.ZeeGirt = New Telerik.WinControls.UI.RadButton()
        Me.ZeePurlin = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.VisualStudio2012DarkTheme1 = New Telerik.WinControls.Themes.VisualStudio2012DarkTheme()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RakeAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CornerAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BaseAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EaveDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EaveUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CeeBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CeeGirt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ZeeGirt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ZeePurlin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.RadPanel1.Controls.Add(Me.RakeAngle)
        Me.RadPanel1.Controls.Add(Me.CornerAngle)
        Me.RadPanel1.Controls.Add(Me.BaseAngle)
        Me.RadPanel1.Controls.Add(Me.RadLabel4)
        Me.RadPanel1.Controls.Add(Me.EaveDown)
        Me.RadPanel1.Controls.Add(Me.RadLabel3)
        Me.RadPanel1.Controls.Add(Me.EaveUp)
        Me.RadPanel1.Controls.Add(Me.CeeBase)
        Me.RadPanel1.Controls.Add(Me.CeeGirt)
        Me.RadPanel1.Controls.Add(Me.RadLabel2)
        Me.RadPanel1.Controls.Add(Me.ZeeGirt)
        Me.RadPanel1.Controls.Add(Me.ZeePurlin)
        Me.RadPanel1.Controls.Add(Me.RadLabel1)
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(140, 550)
        Me.RadPanel1.TabIndex = 0
        Me.RadPanel1.ThemeName = "VisualStudio2012Dark"
        '
        'RakeAngle
        '
        Me.RakeAngle.Image = Global.Pantheon.My.Resources.Resources.RakeAngle
        Me.RakeAngle.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RakeAngle.Location = New System.Drawing.Point(96, 238)
        Me.RakeAngle.Name = "RakeAngle"
        Me.RakeAngle.Size = New System.Drawing.Size(40, 40)
        Me.RakeAngle.TabIndex = 10
        Me.RakeAngle.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.RakeAngle, "Rake Angle Line")
        '
        'CornerAngle
        '
        Me.CornerAngle.Image = Global.Pantheon.My.Resources.Resources.CornerAngle
        Me.CornerAngle.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.CornerAngle.Location = New System.Drawing.Point(50, 238)
        Me.CornerAngle.Name = "CornerAngle"
        Me.CornerAngle.Size = New System.Drawing.Size(40, 40)
        Me.CornerAngle.TabIndex = 9
        Me.CornerAngle.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.CornerAngle, "Corner Angle Line")
        '
        'BaseAngle
        '
        Me.BaseAngle.Image = Global.Pantheon.My.Resources.Resources.BaseAngle
        Me.BaseAngle.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BaseAngle.Location = New System.Drawing.Point(4, 238)
        Me.BaseAngle.Name = "BaseAngle"
        Me.BaseAngle.Size = New System.Drawing.Size(40, 40)
        Me.BaseAngle.TabIndex = 8
        Me.BaseAngle.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.BaseAngle, "Base Angle Line")
        '
        'RadLabel4
        '
        Me.RadLabel4.Location = New System.Drawing.Point(5, 214)
        Me.RadLabel4.Name = "RadLabel4"
        Me.RadLabel4.Size = New System.Drawing.Size(40, 18)
        Me.RadLabel4.TabIndex = 7
        Me.RadLabel4.Text = "Angles"
        Me.RadLabel4.ThemeName = "VisualStudio2012Dark"
        '
        'EaveDown
        '
        Me.EaveDown.Image = Global.Pantheon.My.Resources.Resources.Eave_DSD
        Me.EaveDown.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.EaveDown.Location = New System.Drawing.Point(50, 168)
        Me.EaveDown.Name = "EaveDown"
        Me.EaveDown.Size = New System.Drawing.Size(40, 40)
        Me.EaveDown.TabIndex = 6
        Me.EaveDown.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.EaveDown, "Eave Line DSD")
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(4, 144)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(29, 18)
        Me.RadLabel3.TabIndex = 5
        Me.RadLabel3.Text = "Eave"
        Me.RadLabel3.ThemeName = "VisualStudio2012Dark"
        '
        'EaveUp
        '
        Me.EaveUp.Image = Global.Pantheon.My.Resources.Resources.Eave_DSU
        Me.EaveUp.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.EaveUp.Location = New System.Drawing.Point(4, 168)
        Me.EaveUp.Name = "EaveUp"
        Me.EaveUp.Size = New System.Drawing.Size(40, 40)
        Me.EaveUp.TabIndex = 5
        Me.EaveUp.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.EaveUp, "Eave Line - DSU")
        '
        'CeeBase
        '
        Me.CeeBase.Image = Global.Pantheon.My.Resources.Resources.Cee_Base
        Me.CeeBase.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.CeeBase.Location = New System.Drawing.Point(50, 98)
        Me.CeeBase.Name = "CeeBase"
        Me.CeeBase.Size = New System.Drawing.Size(40, 40)
        Me.CeeBase.TabIndex = 4
        Me.CeeBase.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.CeeBase, "Base Cee Line")
        '
        'CeeGirt
        '
        Me.CeeGirt.Image = Global.Pantheon.My.Resources.Resources.Cee_Girt
        Me.CeeGirt.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.CeeGirt.Location = New System.Drawing.Point(4, 98)
        Me.CeeGirt.Name = "CeeGirt"
        Me.CeeGirt.Size = New System.Drawing.Size(40, 40)
        Me.CeeGirt.TabIndex = 3
        Me.CeeGirt.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.CeeGirt, "Cee Girt Line")
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(4, 74)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(30, 18)
        Me.RadLabel2.TabIndex = 2
        Me.RadLabel2.Text = "Cees"
        Me.RadLabel2.ThemeName = "VisualStudio2012Dark"
        '
        'ZeeGirt
        '
        Me.ZeeGirt.Image = Global.Pantheon.My.Resources.Resources.Zee_Girt
        Me.ZeeGirt.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.ZeeGirt.Location = New System.Drawing.Point(50, 28)
        Me.ZeeGirt.Name = "ZeeGirt"
        Me.ZeeGirt.Size = New System.Drawing.Size(40, 40)
        Me.ZeeGirt.TabIndex = 1
        Me.ZeeGirt.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.ZeeGirt, "Zee Girt Line")
        '
        'ZeePurlin
        '
        Me.ZeePurlin.Image = Global.Pantheon.My.Resources.Resources.Zee_Purlin
        Me.ZeePurlin.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.ZeePurlin.Location = New System.Drawing.Point(4, 28)
        Me.ZeePurlin.Name = "ZeePurlin"
        Me.ZeePurlin.Size = New System.Drawing.Size(40, 40)
        Me.ZeePurlin.TabIndex = 0
        Me.ZeePurlin.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.ZeePurlin, "Purlin Line")
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(4, 4)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(29, 18)
        Me.RadLabel1.TabIndex = 0
        Me.RadLabel1.Text = "Zees"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'Steel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.RadPanel1)
        Me.MaximumSize = New System.Drawing.Size(140, 0)
        Me.MinimumSize = New System.Drawing.Size(140, 550)
        Me.Name = "Steel"
        Me.Size = New System.Drawing.Size(140, 550)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RakeAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CornerAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BaseAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EaveDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EaveUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CeeBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CeeGirt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ZeeGirt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ZeePurlin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadLabel4 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents EaveDown As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents EaveUp As Telerik.WinControls.UI.RadButton
    Friend WithEvents CeeBase As Telerik.WinControls.UI.RadButton
    Friend WithEvents CeeGirt As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents ZeeGirt As Telerik.WinControls.UI.RadButton
    Friend WithEvents ZeePurlin As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents RakeAngle As Telerik.WinControls.UI.RadButton
    Friend WithEvents CornerAngle As Telerik.WinControls.UI.RadButton
    Friend WithEvents BaseAngle As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class

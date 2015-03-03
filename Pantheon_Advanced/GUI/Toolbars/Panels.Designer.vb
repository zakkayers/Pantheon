<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Panels
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
        Me.Monarch = New Telerik.WinControls.UI.RadButton()
        Me.MultiRib = New Telerik.WinControls.UI.RadButton()
        Me.MaxRib = New Telerik.WinControls.UI.RadButton()
        Me.UPanel = New Telerik.WinControls.UI.RadButton()
        Me.SheetArea = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RPanel = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.BreezeTheme1 = New Telerik.WinControls.Themes.BreezeTheme()
        Me.AquaTheme1 = New Telerik.WinControls.Themes.AquaTheme()
        Me.VisualStudio2012DarkTheme1 = New Telerik.WinControls.Themes.VisualStudio2012DarkTheme()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.Monarch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiRib, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MaxRib, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SheetArea, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.RadPanel1.Controls.Add(Me.Monarch)
        Me.RadPanel1.Controls.Add(Me.MultiRib)
        Me.RadPanel1.Controls.Add(Me.MaxRib)
        Me.RadPanel1.Controls.Add(Me.UPanel)
        Me.RadPanel1.Controls.Add(Me.SheetArea)
        Me.RadPanel1.Controls.Add(Me.RadLabel2)
        Me.RadPanel1.Controls.Add(Me.RPanel)
        Me.RadPanel1.Controls.Add(Me.RadLabel1)
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(140, 550)
        Me.RadPanel1.TabIndex = 0
        Me.RadPanel1.ThemeName = "VisualStudio2012Dark"
        '
        'Monarch
        '
        Me.Monarch.Image = Global.Pantheon.My.Resources.Resources.Monarch
        Me.Monarch.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.Monarch.Location = New System.Drawing.Point(5, 249)
        Me.Monarch.Name = "Monarch"
        Me.Monarch.Size = New System.Drawing.Size(130, 32)
        Me.Monarch.TabIndex = 23
        Me.Monarch.Text = " "
        Me.Monarch.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.Monarch, "Monarch Panel")
        '
        'MultiRib
        '
        Me.MultiRib.Image = Global.Pantheon.My.Resources.Resources.MultiRib
        Me.MultiRib.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.MultiRib.Location = New System.Drawing.Point(5, 211)
        Me.MultiRib.Name = "MultiRib"
        Me.MultiRib.Size = New System.Drawing.Size(130, 32)
        Me.MultiRib.TabIndex = 22
        Me.MultiRib.Text = " "
        Me.MultiRib.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.MultiRib, "MultiRib Panel")
        '
        'MaxRib
        '
        Me.MaxRib.Image = Global.Pantheon.My.Resources.Resources.MaxRib
        Me.MaxRib.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.MaxRib.Location = New System.Drawing.Point(5, 173)
        Me.MaxRib.Name = "MaxRib"
        Me.MaxRib.Size = New System.Drawing.Size(130, 32)
        Me.MaxRib.TabIndex = 21
        Me.MaxRib.Text = " "
        Me.MaxRib.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.MaxRib, "MaxRib Panel")
        '
        'UPanel
        '
        Me.UPanel.Image = Global.Pantheon.My.Resources.Resources.uPanel
        Me.UPanel.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.UPanel.Location = New System.Drawing.Point(5, 135)
        Me.UPanel.Name = "UPanel"
        Me.UPanel.Size = New System.Drawing.Size(130, 32)
        Me.UPanel.TabIndex = 20
        Me.UPanel.Text = " "
        Me.UPanel.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.UPanel, "U-Panel")
        '
        'SheetArea
        '
        Me.SheetArea.Image = Global.Pantheon.My.Resources.Resources.SheetArea
        Me.SheetArea.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.SheetArea.Location = New System.Drawing.Point(5, 27)
        Me.SheetArea.Name = "SheetArea"
        Me.SheetArea.Size = New System.Drawing.Size(80, 40)
        Me.SheetArea.TabIndex = 18
        Me.SheetArea.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.SheetArea, "Sheet Specified Area")
        '
        'RadLabel2
        '
        Me.RadLabel2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadLabel2.Location = New System.Drawing.Point(5, 4)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(98, 18)
        Me.RadLabel2.TabIndex = 17
        Me.RadLabel2.Text = "Panel Generation"
        Me.RadLabel2.ThemeName = "VisualStudio2012Dark"
        '
        'RPanel
        '
        Me.RPanel.Image = Global.Pantheon.My.Resources.Resources.RPanel
        Me.RPanel.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RPanel.Location = New System.Drawing.Point(5, 97)
        Me.RPanel.Name = "RPanel"
        Me.RPanel.Size = New System.Drawing.Size(130, 32)
        Me.RPanel.TabIndex = 15
        Me.RPanel.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.RPanel, "R-Panel")
        '
        'RadLabel1
        '
        Me.RadLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadLabel1.Location = New System.Drawing.Point(5, 73)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(79, 18)
        Me.RadLabel1.TabIndex = 16
        Me.RadLabel1.Text = "Panel Profiles"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'Panels
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.RadPanel1)
        Me.MaximumSize = New System.Drawing.Size(140, 0)
        Me.MinimumSize = New System.Drawing.Size(140, 550)
        Me.Name = "Panels"
        Me.Size = New System.Drawing.Size(140, 550)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.Monarch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiRib, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MaxRib, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SheetArea, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents Monarch As Telerik.WinControls.UI.RadButton
    Friend WithEvents MultiRib As Telerik.WinControls.UI.RadButton
    Friend WithEvents MaxRib As Telerik.WinControls.UI.RadButton
    Friend WithEvents UPanel As Telerik.WinControls.UI.RadButton
    Friend WithEvents SheetArea As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents BreezeTheme1 As Telerik.WinControls.Themes.BreezeTheme
    Friend WithEvents AquaTheme1 As Telerik.WinControls.Themes.AquaTheme
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents RPanel As Telerik.WinControls.UI.RadButton

End Class

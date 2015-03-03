<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StandardSlabForm
    Inherits Telerik.WinControls.UI.ShapedForm

    'Form overrides dispose to clean up the component list.
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
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RoundRectShapeTitle = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RadTitleBar1 = New Telerik.WinControls.UI.RadTitleBar()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.Add = New Telerik.WinControls.UI.RadButton()
        Me.MarkCombo = New System.Windows.Forms.ComboBox()
        Me.WidthField = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.LengthField = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel4 = New Telerik.WinControls.UI.RadLabel()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.NotchDepthField = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel5 = New Telerik.WinControls.UI.RadLabel()
        Me.NotchNo = New Telerik.WinControls.UI.RadRadioButton()
        Me.NotchYes = New Telerik.WinControls.UI.RadRadioButton()
        Me.Create = New Telerik.WinControls.UI.RadButton()
        Me.Copy = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.VisualStudio2012DarkTheme1 = New Telerik.WinControls.Themes.VisualStudio2012DarkTheme()
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Add, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WidthField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LengthField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.NotchDepthField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NotchNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NotchYes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Create, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Copy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RoundRectShapeTitle
        '
        Me.RoundRectShapeTitle.BottomLeftRounded = False
        Me.RoundRectShapeTitle.BottomRightRounded = False
        '
        'RadTitleBar1
        '
        Me.RadTitleBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.RadTitleBar1.Location = New System.Drawing.Point(1, 1)
        Me.RadTitleBar1.Name = "RadTitleBar1"
        '
        '
        '
        Me.RadTitleBar1.RootElement.ApplyShapeToControl = True
        Me.RadTitleBar1.RootElement.Shape = Me.RoundRectShapeTitle
        Me.RadTitleBar1.Size = New System.Drawing.Size(169, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel1
        '
        Me.RadLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadLabel1.Location = New System.Drawing.Point(12, 30)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(29, 18)
        Me.RadLabel1.TabIndex = 38
        Me.RadLabel1.Text = "Slab"
        Me.RadLabel1.ThemeName = "VisualStudio2012Dark"
        '
        'Add
        '
        Me.Add.Image = Global.Pantheon.My.Resources.Resources.Add
        Me.Add.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.Add.Location = New System.Drawing.Point(12, 54)
        Me.Add.Name = "Add"
        Me.Add.Size = New System.Drawing.Size(25, 25)
        Me.Add.TabIndex = 36
        Me.Add.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.Add, "Add New Member")
        '
        'MarkCombo
        '
        Me.MarkCombo.Enabled = False
        Me.MarkCombo.FormattingEnabled = True
        Me.MarkCombo.Location = New System.Drawing.Point(74, 58)
        Me.MarkCombo.Name = "MarkCombo"
        Me.MarkCombo.Size = New System.Drawing.Size(84, 21)
        Me.MarkCombo.TabIndex = 37
        '
        'WidthField
        '
        Me.WidthField.Location = New System.Drawing.Point(58, 108)
        Me.WidthField.Name = "WidthField"
        Me.WidthField.Size = New System.Drawing.Size(100, 21)
        Me.WidthField.TabIndex = 39
        Me.WidthField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.WidthField.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(16, 109)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(36, 18)
        Me.RadLabel3.TabIndex = 41
        Me.RadLabel3.Text = "Width"
        Me.RadLabel3.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel2
        '
        Me.RadLabel2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadLabel2.Location = New System.Drawing.Point(12, 85)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(84, 18)
        Me.RadLabel2.TabIndex = 40
        Me.RadLabel2.Text = "Measurements"
        Me.RadLabel2.ThemeName = "VisualStudio2012Dark"
        '
        'LengthField
        '
        Me.LengthField.Location = New System.Drawing.Point(58, 135)
        Me.LengthField.Name = "LengthField"
        Me.LengthField.Size = New System.Drawing.Size(100, 21)
        Me.LengthField.TabIndex = 42
        Me.LengthField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.LengthField.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel4
        '
        Me.RadLabel4.Location = New System.Drawing.Point(16, 136)
        Me.RadLabel4.Name = "RadLabel4"
        Me.RadLabel4.Size = New System.Drawing.Size(41, 18)
        Me.RadLabel4.TabIndex = 43
        Me.RadLabel4.Text = "Length"
        Me.RadLabel4.ThemeName = "VisualStudio2012Dark"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.NotchDepthField)
        Me.RadGroupBox1.Controls.Add(Me.RadLabel5)
        Me.RadGroupBox1.Controls.Add(Me.NotchNo)
        Me.RadGroupBox1.Controls.Add(Me.NotchYes)
        Me.RadGroupBox1.HeaderText = "Panel Notch"
        Me.RadGroupBox1.Location = New System.Drawing.Point(16, 171)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(142, 102)
        Me.RadGroupBox1.TabIndex = 44
        Me.RadGroupBox1.Text = "Panel Notch"
        Me.RadGroupBox1.ThemeName = "VisualStudio2012Dark"
        '
        'NotchDepthField
        '
        Me.NotchDepthField.Location = New System.Drawing.Point(6, 70)
        Me.NotchDepthField.Name = "NotchDepthField"
        Me.NotchDepthField.Size = New System.Drawing.Size(100, 21)
        Me.NotchDepthField.TabIndex = 45
        Me.NotchDepthField.Text = "1-1/2"""
        Me.NotchDepthField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.NotchDepthField.ThemeName = "VisualStudio2012Dark"
        '
        'RadLabel5
        '
        Me.RadLabel5.Location = New System.Drawing.Point(6, 46)
        Me.RadLabel5.Name = "RadLabel5"
        Me.RadLabel5.Size = New System.Drawing.Size(71, 18)
        Me.RadLabel5.TabIndex = 44
        Me.RadLabel5.Text = "Notch Depth"
        Me.RadLabel5.ThemeName = "VisualStudio2012Dark"
        '
        'NotchNo
        '
        Me.NotchNo.Location = New System.Drawing.Point(52, 22)
        Me.NotchNo.Name = "NotchNo"
        Me.NotchNo.Size = New System.Drawing.Size(38, 18)
        Me.NotchNo.TabIndex = 1
        Me.NotchNo.Text = "No"
        Me.NotchNo.ThemeName = "VisualStudio2012Dark"
        '
        'NotchYes
        '
        Me.NotchYes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.NotchYes.Location = New System.Drawing.Point(6, 22)
        Me.NotchYes.Name = "NotchYes"
        Me.NotchYes.Size = New System.Drawing.Size(40, 18)
        Me.NotchYes.TabIndex = 0
        Me.NotchYes.TabStop = True
        Me.NotchYes.Text = "Yes"
        Me.NotchYes.ThemeName = "VisualStudio2012Dark"
        Me.NotchYes.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'Create
        '
        Me.Create.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Create.Location = New System.Drawing.Point(16, 279)
        Me.Create.Name = "Create"
        Me.Create.Size = New System.Drawing.Size(142, 40)
        Me.Create.TabIndex = 56
        Me.Create.Text = "Create"
        Me.Create.ThemeName = "VisualStudio2012Dark"
        '
        'Copy
        '
        Me.Copy.Enabled = False
        Me.Copy.Image = Global.Pantheon.My.Resources.Resources.EditName
        Me.Copy.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.Copy.Location = New System.Drawing.Point(43, 54)
        Me.Copy.Name = "Copy"
        Me.Copy.Size = New System.Drawing.Size(25, 25)
        Me.Copy.TabIndex = 62
        Me.Copy.ThemeName = "VisualStudio2012Dark"
        Me.ToolTip1.SetToolTip(Me.Copy, "Copy Current Member")
        '
        'StandardSlabForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(171, 328)
        Me.Controls.Add(Me.Copy)
        Me.Controls.Add(Me.Create)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.LengthField)
        Me.Controls.Add(Me.RadLabel4)
        Me.Controls.Add(Me.WidthField)
        Me.Controls.Add(Me.RadLabel3)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.Add)
        Me.Controls.Add(Me.MarkCombo)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.Name = "StandardSlabForm"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = ""
        Me.ThemeName = "VisualStudio2012Dark"
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Add, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WidthField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LengthField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.NotchDepthField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NotchNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NotchYes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Create, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Copy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Add As Telerik.WinControls.UI.RadButton
    Friend WithEvents MarkCombo As System.Windows.Forms.ComboBox
    Friend WithEvents WidthField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents LengthField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel4 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents NotchDepthField As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel5 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents NotchNo As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents NotchYes As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents Create As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Copy As Telerik.WinControls.UI.RadButton
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
End Class


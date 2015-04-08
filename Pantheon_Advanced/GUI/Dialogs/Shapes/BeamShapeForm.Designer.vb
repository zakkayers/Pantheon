<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BeamShapeForm
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
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape
        Me.RoundRectShapeTitle = New Telerik.WinControls.RoundRectShape
        Me.RadTitleBar1 = New Telerik.WinControls.UI.RadTitleBar
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RoundRectShapeForm
        '
        Me.RoundRectShapeForm.BottomLeftRounded = True
        Me.RoundRectShapeForm.BottomRightRounded = True
        Me.RoundRectShapeForm.TopLeftRounded = True
        Me.RoundRectShapeForm.TopRightRounded = True
        '
        'RoundRectShapeTitle
        '
        Me.RoundRectShapeTitle.BottomLeftRounded = False
        Me.RoundRectShapeTitle.BottomRightRounded = False
        Me.RoundRectShapeTitle.TopLeftRounded = True
        Me.RoundRectShapeTitle.TopRightRounded = True
        '
        'RadTitleBar1
        '
        Me.RadTitleBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadTitleBar1.Text = "BeamShapeForm"
        Me.RadTitleBar1.Location = New System.Drawing.Point(1, 1)
        Me.RadTitleBar1.Name = "RadTitleBar1"
        '
        '
        '
        Me.RadTitleBar1.RootElement.ApplyShapeToControl = True
        Me.RadTitleBar1.RootElement.Shape = Me.RoundRectShapeTitle
        Me.RadTitleBar1.Size = New System.Drawing.Size(318, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.Text = "BeamShapeForm"
        '
        'BeamShapeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(320, 310)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.Name = "BeamShapeForm"
        Me.Shape = Me.RoundRectShapeForm
        Me.Text = "BeamShapeForm"
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
End Class


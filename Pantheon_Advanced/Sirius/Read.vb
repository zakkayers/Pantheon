Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Media.Imaging

Public Class Read

    ' FUNCTION: LOADIMAGE
    ' This Function Loads A Bitmap Image From Resources And Makes All (192,192,192) Colors Transparent
    Public Shared Function LoadImage(ByVal pic As Bitmap) As BitmapImage

        Dim ms As New MemoryStream()
        pic.MakeTransparent(Color.FromArgb(192, 192, 192))
        pic.Save(ms, ImageFormat.Png)
        Dim bitmap As New BitmapImage
        bitmap.BeginInit()
        bitmap.StreamSource = ms
        bitmap.EndInit()
        Return bitmap
    End Function

End Class

Imports System.Drawing.Imaging
Imports Tesseract


Public Class Form1


    Public Function GrayScaleFilter(ByVal image As Bitmap) As Bitmap
        Dim grayScale As Bitmap = New Bitmap(image.Width, image.Height)
        Dim y As Int32 = 0
        Do While (y < grayScale.Height)
            Dim x As Int32 = 0
            Do While (x < grayScale.Width)
                Dim c As Color = image.GetPixel(x, y)
                Dim gs As Int32 = CType(((c.R * 0.3) _
                            + ((c.G * 0.59) _
                            + (c.B * 0.11))), Int32)
                grayScale.SetPixel(x, y, Color.FromArgb(gs, gs, gs))
                x = (x + 1)
            Loop

            y = (y + 1)
        Loop

        Return grayScale
    End Function



    Public Function invertimg(bm As Bitmap) As Bitmap
        '   bm = New Bitmap(PictureBox1.Image)
        Dim X As Integer
        Dim Y As Integer
        Dim r As Integer
        Dim g As Integer
        Dim b As Integer

        For X = 0 To bm.Width - 1
            For Y = 0 To bm.Height - 1
                r = 255 - (bm.GetPixel(X, Y)).R
                g = 255 - bm.GetPixel(X, Y).G
                b = 255 - bm.GetPixel(X, Y).B
                '  MsgBox("r: " & r & "g: " & g & "b: " & b)

                bm.SetPixel(X, Y, Color.FromArgb(r, g, b))
            Next Y
        Next X
        Return bm
    End Function
    Dim r As Rectangle

    Dim g As Graphics



    Private Sub setBrightnes(ByVal Brightness As Single)
        ' Brightness should be -1 (black) to 0 (neutral) to 1 (white)

        Dim colorMatrixVal As Single()() = {
           New Single() {1, 0, 0, 0, 0},
           New Single() {0, 1, 0, 0, 0},
           New Single() {0, 0, 1, 0, 0},
           New Single() {0, 0, 0, 1, 0},
           New Single() {Brightness, Brightness, Brightness, 0, 1}}

        Dim colorMatrix As New ColorMatrix(colorMatrixVal)
        Dim ia As New ImageAttributes

        ia.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

        g.DrawImage(imgt, r, 0, 0, imgt.Width, imgt.Height, GraphicsUnit.Pixel, ia)
        PictureBox4.Refresh()

    End Sub
    Dim imgt As Image
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub analisy()

        Dim pn1 As Bitmap = New Bitmap(PictureBox1.Image)

        Dim pn2 As Bitmap = GrayScaleFilter(pn1)
        PictureBox2.Image = pn2

        PictureBox3.Image = invertimg(pn2)
        imgt = PictureBox3.Image
        PictureBox4.Image = New Bitmap(PictureBox3.Width, PictureBox3.Height, PixelFormat.Format32bppArgb)

        g = Graphics.FromImage(PictureBox4.Image)
        r = New Rectangle(0, 0, PictureBox4.Width, PictureBox4.Height)
        g.DrawImage(imgt, r)
        setBrightnes(0.3)

        Dim ocrEngine = New TesseractEngine(".\tessdata", "eng", EngineMode.Default, False)
        ocrEngine.SetVariable("tessedit_char_whitelist", "0123456789") ' If digit only
        ' ocrEngine.SetVariable("edges_max_children_per_outline", "40")

        Dim imageWithText As Image = PictureBox4.Image
        ocrEngine.DefaultPageSegMode = PageSegMode.SparseText
        Dim Page = ocrEngine.Process(imageWithText)

        TextBox1.Text = page.GetText

        '   MsgBox(Val(TextBox1.Text.ToString))
    End Sub

    Dim op As New System.Windows.Forms.OpenFileDialog


    Dim x As String
    Dim y As String
    Private Sub CaptureScreen()

        Dim graph As Graphics = Nothing
        Try
            '
            Dim frmleft As System.Drawing.Point = Me.Bounds.Location
            Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
            Dim tx As Integer = TextBox2.Text
            Dim ty As Integer = TextBox3.Text
            Dim bmp As New Bitmap(tx, ty)

            graph = Graphics.FromImage(bmp)
            ' x = screenWidth - 207
            ' y = screenHeight - (screenHeight - 200)
            '  Label1.Text = "X: " & MousePosition.X & "  Y: " & MousePosition.Y
            x = MousePosition.X
            y = MousePosition.Y
            graph.CopyFromScreen(x, y, 0, 0, bmp.Size)
            ' graph.CopyFromScreen(screenWidth - 207, screenHeight - (screenHeight - 200), 0, 0, bmp.Size)

            '   PictureBox4.Image = bmp
            ' PictureBox1.Image = GrayScaleFilter(bmp)
            Dim bmt As Bitmap = New Bitmap(bmp, 100, 107)
            PictureBox1.Image = bmt

            bmp.Dispose()
            graph.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '  op.ShowDialog()

        op.ShowDialog()

        PictureBox1.Image = Image.FromFile(op.FileName)




    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        CaptureScreen()
        analisy()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class

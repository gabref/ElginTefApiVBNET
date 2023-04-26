﻿Imports System
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Runtime.InteropServices
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes



Public Class ColetaPinPad

    Private Const hrLogs As String = vbLf & "=================================" & vbLf


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        lblValorColetado.Content = "_"
        txtLogs.Text = "INICIANDO OPERAÇÃO"
        Dim cmbIndex As Integer = cmbTipoColeta.SelectedIndex

        If cmbIndex = -1 Then
            MessageBox.Show("Selecione o tipo da operação")
            cmbTipoColeta.Focus()
            Return
        End If

        Dim _intptr As IntPtr = ControleApi.IniciarOperacaoTEF("{}")
        Dim start As String = Marshal.PtrToStringAnsi(_intptr)
        txtLogs.Text += start
        Dim tipoColeta As Integer = cmbIndex + 1
        Dim confirmar As Boolean = rdbConfirm.IsChecked
        txtLogs.Text += hrLogs
        txtLogs.Text += vbLf & "OPÇÕES ESCOLHIDAS" & vbLf & "tipoColeta: " & tipoColeta & vbLf & "confirmar: " & confirmar
        Dim _intptrColeta As IntPtr = ControleApi.RealizarColetaPinPad(tipoColeta, confirmar)
        Dim coleta As String = Marshal.PtrToStringAnsi(_intptrColeta)
        Dim resultadoPinPad As String

        If Integer.Parse(getRetorno(coleta)) = 1 Then
            resultadoPinPad = getStringValue(jsonify(coleta), "tef", "resultadoCapturaPinPad")
            txtLogs.Text += vbLf & "RESULTADO CAPTURA: " & resultadoPinPad
            lblValorColetado.Content = resultadoPinPad
        Else
            finalizar(getStringValue(jsonify(coleta), "tef", "mensagemResultado"))
            Return
        End If

        txtLogs.Text += hrLogs
        txtLogs.Text += vbLf & "RETORNO DLL: RealizarcoletaPinPad"
        txtLogs.Text += coleta

        If confirmar Then
            finalizar(vbLf & "FIM DA OPERAÇÃO")
            Return
        End If

        txtLogs.Text += hrLogs
        txtLogs.Text += vbLf & "INICIANDO CONFIRMAÇÃO"

        Select Case tipoColeta
            Case 1
                resultadoPinPad = FormatRG(resultadoPinPad)
            Case 2
                resultadoPinPad = FormatCPF(resultadoPinPad)
            Case 3
                resultadoPinPad = FormatCNPJ(resultadoPinPad)
            Case 4
                resultadoPinPad = FormatPhone(resultadoPinPad)
            Case Else
        End Select

        Dim ptrRealizaConfirmacao As IntPtr = ControleApi.ConfirmarCapturaPinPad(tipoColeta, resultadoPinPad)
        Dim retornoConfirmacao As String = Marshal.PtrToStringAnsi(ptrRealizaConfirmacao)

        If Integer.Parse(getRetorno(retornoConfirmacao)) = 1 Then
            resultadoPinPad = getStringValue(jsonify(retornoConfirmacao), "tef", "resultadoCapturaPinPad")
            txtLogs.Text += vbLf & "RESULTADO CONFIRMAÇÃO: " & resultadoPinPad
            lblValorColetado.Content = resultadoPinPad
            txtLogs.Text = vbLf & "RETORNO DLL: ConfirmarCapturaPinPad" & retornoConfirmacao
        Else
            finalizar(getStringValue(jsonify(retornoConfirmacao), "tef", "mensagemResultado"))
            Return
        End If

        finalizar("FIM DA OPERAÇÃO")
    End Sub

    Private Sub finalizar(reason As String)
        txtLogs.Text += hrLogs
        txtLogs.Text += "FINALIZANDO OPERAÇÃO - REASON: " & reason
        Dim _ptrRetorno As IntPtr = ControleApi.FinalizarOperacaoTEF(1)
        Dim retorno As String = Marshal.PtrToStringAnsi(_ptrRetorno)

        If Integer.Parse(getRetorno(retorno)) = 1 Then
            txtLogs.Text += hrLogs
            txtLogs.Text += "FINALIZADA OPERAÇÃO COM SUCESSO!"
        Else
            txtLogs.Text += hrLogs
            txtLogs.Text += "FINALIZADA OPERAÇÃO COM ERRO!"
            txtLogs.Text += retorno
        End If
    End Sub

    Private Function getRetorno(resp As String) As String
        Dim _jsonDic As IDictionary(Of String, Object) = jsonify(resp)
        Return getStringValue(_jsonDic, "tef", "retorno")
    End Function

    Private Function jsonify(jsonString As String) As IDictionary(Of String, Object)
        If jsonString IsNot Nothing Then
            Dim _Dic As IDictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonString)
            Return _Dic
        Else
            Dim _Dic As IDictionary(Of String, Object) = New Dictionary(Of String, Object)()
            Return _Dic
        End If
    End Function

    Private Function getStringValue(dicJson As IDictionary(Of String, Object), keyOut As String, keyIn As String) As String
        Dim value As Object = Nothing, valueIn As Object = Nothing

        If dicJson IsNot Nothing AndAlso dicJson.TryGetValue(keyOut, value) Then
            Dim _Dic As IDictionary(Of String, Object) = value.ToObject(Of Dictionary(Of String, Object))()

            If _Dic IsNot Nothing AndAlso _Dic.TryGetValue(keyIn, valueIn) Then
                Console.WriteLine(valueIn.[GetType]())
                Return valueIn.ToString()
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function

    Private Function getStringValue(dicJson As IDictionary(Of String, Object), key As String) As String
        Dim value As Object = Nothing

        If dicJson IsNot Nothing AndAlso dicJson.TryGetValue(key, value) Then
            Return value.ToString()
        Else
            Return String.Empty
        End If
    End Function

    Private Function MultiInsert(str As String, insertChar As String, ParamArray positions As Integer()) As String
        Dim sb As StringBuilder = New StringBuilder(str.Length + (positions.Length * insertChar.Length))
        Dim posLookup As HashSet(Of Integer) = New HashSet(Of Integer)(positions)

        For i As Integer = 0 To str.Length - 1
            sb.Append(str(i))
            If posLookup.Contains(i) Then sb.Append(insertChar)
        Next

        Return sb.ToString()
    End Function

    Private Function FormatCPF(cpf As String) As String
        Return MultiInsert(cpf, ".", 2, 5).Insert(11, "-")
    End Function

    Private Function FormatRG(rg As String) As String
        Return MultiInsert(rg, ".", 1, 4).Insert(10, "-")
    End Function

    Private Function FormatCNPJ(cnpj As String) As String
        Return MultiInsert(cnpj, ".", 1, 4).Insert(9, "/").Insert(14, "-")
    End Function

    Private Function FormatPhone(phone As String) As String
        Return "(" & phone.Insert(2, ")").Insert(8, "-")
    End Function

    Private Sub txtLogs_TextChanged(sender As Object, e As TextChangedEventArgs)
        txtLogs.ScrollToEnd()
    End Sub

End Class

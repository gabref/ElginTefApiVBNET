Imports System.Runtime.InteropServices

Public Class ControleApi
    Public Shared Property ModoOperacao As String = String.Empty


    ' ===================================================================== //
    ' ============ CARREGAMENTO DAS FUNÇÕES DA DLL ================== //
    ' ===================================================================== //

    Public Const PATH As String = "..\..\E1_Tef01.dll"

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function GetProdutoTef() As Integer
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function GetClientTCP() As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function SetClientTCP(ip As String, porta As Integer) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function ConfigurarDadosPDV(textoPinpad As String, versaoAC As String, nomeEstabelecimento As String, loja As String, identificadorPontoCaptura As String) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function IniciarOperacaoTEF(dadosCaptura As String) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function RecuperarOperacaoTEF(dadosCaptura As String) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function RealizarPagamentoTEF(codigoOperacao As Integer, dadosCaptura As String, novaTransacao As Boolean) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function RealizarPixTEF(dadosCaptura As String, novaTransacao As Boolean) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function RealizarAdmTEF(codigoOperacao As Integer, dadosCaptura As String, novaTransacao As Boolean) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function ConfirmarOperacaoTEF(id As Integer, acao As Integer) As IntPtr
    End Function

    <DllImport(PATH, CallingConvention:=CallingConvention.StdCall)>
    Friend Shared Function FinalizarOperacaoTEF(id As Integer) As IntPtr
    End Function

End Class

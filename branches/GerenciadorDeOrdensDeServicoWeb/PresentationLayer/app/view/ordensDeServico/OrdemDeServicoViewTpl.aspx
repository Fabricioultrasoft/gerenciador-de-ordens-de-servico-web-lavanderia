<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdemDeServicoViewTpl.aspx.cs" Inherits="GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.view.ordensDeServico.OrdemDeServicoViewTpl" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataTransferObjects" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.ordensDeServico" %>
<%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql" %>

<%
    OrdemDeServico os;
    List<Erro> erros;
    UInt32 numero;
    UInt32.TryParse( Request.QueryString["numero"], out numero );
    
    erros = GerenciadorDeOrdensDeServico.preencher( out os, numero);
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ordem de Servi&ccedil;o - numero: </title>
    <style type="text/css">
        .bold { font-weight: bold;}
        .center { text-align: center; }
        .tableOS  
        {
            background-color: White;
            font-family: Arial;
            font-size: 12;
            border-top: 1px solid #781A28;
            border-left: 1px solid #781A28;
            width: 100%;
        }
        .tableOS td
        {
            border-right: 1px solid #781A28;
            border-bottom: 1px solid #781A28;
            color: #781A28;
            padding: 3px;
        }
    </style>
</head>
<body>

<% 
    if( erros.Count > 0 ) {
        foreach( Erro err in erros ) { %>
            <p>Erro: <%=err.mensagem %><br />Solu&ccedil;&atilde;o: <%= err.solucao %></p>
<%     
        }
    } else if( os.codigo == 0 ) { %>
        <p>Ordem de Servi&ccedil;o de n&uacute;mero <%= numero %> n&atilde;o foi encontrada.</p>
<% 
    } else {
        StringBuilder fones = new StringBuilder();
        StringBuilder emails = new StringBuilder();
        StringBuilder enderecos = new StringBuilder();
        
        foreach( MeioDeContato meio in os.cliente.meiosDeContato ) {
            if( meio.tipoDeContato.codigo == 1 || meio.tipoDeContato.codigo == 2 || meio.tipoDeContato.codigo == 3 )
                fones.AppendFormat( "{0}  ", meio.contato );
            else if( meio.tipoDeContato.codigo == 4 )
                emails.AppendFormat( "{0}  ", meio.contato );
        }
        
        foreach(Endereco end in os.cliente.enderecos) {
            enderecos.AppendFormat( "{0}, n&ordm; {1} - {2}<br />", end.logradouro.nome, end.numero, end.bairro.nome, end.cidade );
        }
        if( os.cliente.enderecos.Count > 0 ) { 
            enderecos.Remove( enderecos.Length - 6, 6 );// remove o ultimo <br />
        }
%>
    <table class="tableOS" cellspacing="0">
    <tbody>
        <tr>
            <td style="border-right: none; text-align:center;"><img src="/PresentationLayer/resources/images/gabbeh.jpg" alt="Gabbeh" /></td>
            <td colspan="4">
                <p class="center" style="font: normal 12px times; margin: 0px;" >
                    Rua Cel. Francisco Andrade Coutinho, 132<br />
                    Cambu&iacute; - Campinas - SP<br />
                    e-mail: gabbeh@uol.com.br<br />
                    <span class="bold">Fone (19) 3294-6711 / 3294-2495 - Fax: 3294-8786</span>    
                </p>
            </td>
            <td>
                <p class="center" style=" color: Red; margin: 5px;" >N&ordm; <%= os.numero %></p>
                <p class="center" style="margin: 0px;" >DATA: <%= os.dataDeAbertura.ToString( "dd/MM/yyyy" ) %></p>
            </td>
        </tr>
        <tr><td colspan="6" >NOME: <%= os.cliente.nome %></td></tr>
        <tr><td colspan="6" >ENDERE&Ccedil;O: <%= enderecos %></td></tr>
        <tr><td colspan="3" >FONE: <%= fones %></td><td colspan="3" >E-MAIL: <%= emails %></td></tr>
        <tr><td colspan="4" class="center" >TAPETES</td><td rowspan="2" class="center" >SERVI&Ccedil;OS</td><td rowspan="2" class="center" >TOTAL</td></tr>
        <tr><td class="center" >NOME</td><td class="center" >COMPR.</td><td class="center" >LARG.</td><td class="center" >&Aacute;REA</td></tr>
<%
        foreach( Item item in os.itens ) {
            StringBuilder servicos = new StringBuilder();
            foreach( ServicoDoItem serv in item.servicosDoItem ) {
                servicos.AppendFormat( "{0} - R${1}, ", serv.servico.nome, serv.valor.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" )) );
            }
            if( item.servicosDoItem.Count > 0 ) {
                servicos.Remove( servicos.Length - 2, 2 );// remove a ultima virgula
            } 
%>            
        <tr><td><%= item.tapete.nome %></td><td class="center" ><%= item.comprimento %></td><td class="center" ><%= item.largura %></td><td class="center" ><%= item.area %></td><td class="center" ><%= servicos %></td><td class="center" >R$<%= item.valor.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) %></td></tr>
<%      } %>
        
        <tr><td colspan="6" >OBS.: <%= os.observacoes %></td></tr>
    </tbody>
    </table>

<%  } %>

</body>
</html>

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
    Boolean print;
    UInt32.TryParse( Request["numero"], out numero );
    Boolean.TryParse( Request["print"], out print );
    
    erros = GerenciadorDeOrdensDeServico.preencher( out os, numero);
%>

<%  if( print ) { %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Ordem de Servi&ccedil;o - numero: </title>
    <link href="/PresentationLayer/resources/css/app.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        window.print();
    </script>
</head>
<body>
<%  } %>

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
            <td colspan="5">
                <table class="innerTableOS" cellspacing="0" ><tbody><tr>
                    <td id="tdImg"><img src="/PresentationLayer/resources/images/gabbeh.jpg" alt="Gabbeh" /></td>
                    <td>
                        <p id="infoContato" >
                            Rua Cel. Francisco Andrade Coutinho, 132<br />
                            Cambu&iacute; - Campinas - SP<br />
                            e-mail: gabbeh@uol.com.br<br />
                            <span class="bold">Fone (19) 3294-6711 / 3294-2495 - Fax: 3294-8786</span>    
                        </p>
                    </td>
                </tr></tbody></table>
            </td>
            <td>
                <p id="numOS">N&ordm; <%= os.numero %></p>
                <p id="dataOS">DATA: <%= os.dataDeAbertura.ToString( "dd/MM/yyyy" ) %></p>
            </td>
        </tr>
        <tr><td colspan="6" class="left" >NOME: <%= os.cliente.nome %></td></tr>
        <tr><td colspan="6" class="left" >ENDERE&Ccedil;O: <%= enderecos %></td></tr>
        
        
        
        <tr>
            <td colspan="6" id="parentfoneMailOS" >
                <table cellspacing="0" class="innerTableOS" id="foneMailOS" >
                    <tbody>
                        <tr><td class="left" >FONE: <%= fones %></td><td class="left" >E-MAIL: <%= emails %></td></tr>                
                    </tbody>
                </table>
            </td>
        </tr>

        
        
        <tr><td colspan="4" >TAPETES</td><td rowspan="2" >SERVI&Ccedil;OS</td><td rowspan="2" >VALORES</td></tr>
        <tr><td >NOME</td><td class="comprLargAreaOS">COMPR.</td><td class="comprLargAreaOS">LARG.</td><td class="comprLargAreaOS">&Aacute;REA</td></tr>
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
        <tr><td><%= item.tapete.nome %></td><td><%= item.comprimento %></td><td><%= item.largura %></td><td><%= item.area %></td><td><%= servicos %></td><td>R$<%= item.valor.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) %></td></tr>
<%      } %>
        
        <%-- <tr><td colspan="4" style="border-bottom: none;" ></td><td>VALOR ORIGINAL</td><td>R$<%= os.valorOriginal.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) )%></td></tr> --%>
        <tr><td colspan="4" ></td><td class="bold" >TOTAL</td><td class="bold" >R$<%= os.valorFinal.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) )%></td></tr>
        <tr><td colspan="6" class="left" >OBS.: <%= os.observacoes %></td></tr>
    </tbody>
    </table>

<%  } %>

<%  if( print ) { %>
</body>
</html>
<%  } %>
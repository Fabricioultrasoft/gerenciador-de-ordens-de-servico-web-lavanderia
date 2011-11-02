using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.UI;
using iTextSharp.text.pdf;
using iTextSharp;
using iTextSharp.text.html;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.view.relatorios {
	/// <summary>
	/// Summary description for ClientesTpl
	/// </summary>
	public class ClientesTpl : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {

			JavaScriptSerializer js = new JavaScriptSerializer();
			String colunas = context.Request.QueryString["colunas"];
			UInt32 start = 0;
			UInt32 limit = 0;
			List<Filter> filters;
			List<Sorter> sorters;

			#region DADOS DA REQUISICAO
			if( String.IsNullOrEmpty( colunas ) ) {
				context.Response.Write( "Nenhuma coluna do relat&oacute;rio foi selecionada!" );
				return;
			}

			UInt32.TryParse( context.Request.QueryString["start"], out start );
			UInt32.TryParse( context.Request.QueryString["limit"], out limit );

			// trata o problema de indice comecar em 0 no mysql
			if( start > 0 ) start--;

			// filtros
			if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
				filters = js.Deserialize<List<Filter>>( context.Request.QueryString["filter"] );
			} else {
				filters = new List<Filter>();
			}

			// ordenacao
			if( String.IsNullOrEmpty( context.Request.QueryString["sort"] ) == false ) {
				sorters = js.Deserialize<List<Sorter>>( context.Request.QueryString["sort"] );
			} else {
				sorters = new List<Sorter>();
			}
			#endregion

			StringBuilder sql = new StringBuilder();
			MySqlFilter filter = MySqlClientesDao.getFilter( filters );
			String sortClause = MySqlClientesDao.getSort( sorters );

			sql.AppendFormat( "SELECT {0} FROM tb_clientes ", construirColunasSql( colunas ) );
			sql.AppendLine( "INNER JOIN tb_tipos_clientes ON tb_tipos_clientes.cod_tipo_cliente = tb_clientes.cod_tipo_cliente " );
			sql.AppendLine( filter.whereClause );
			sql.AppendLine( ( sortClause.Trim().Length > 0 ) ? sortClause : " ORDER BY cod_cliente " );
			if( limit > 0 )
				sql.AppendLine( " LIMIT " + start + "," + limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			if( filter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( filter.parametersList.ToArray() );
			}

			MySqlDataReader reader = cmd.ExecuteReader();

			switch( context.Request.QueryString["reportView"] ) {
				case "html":
					gerarHtml( context, reader );
					break;
				case "xls":
					gerarExcel( context, reader );
					break;
				case "pdf":
					gerarPdf( context, reader );
					break;
			}

			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();
		}

		private static void gerarHtml( HttpContext context, MySqlDataReader reader ) {

			context.Response.ContentType = "text/html";

			context.Response.Write( "<html><head><title>Relatório de Clientes</title>"
				+ "<link rel='icon' href='/PresentationLayer/resources/images/favicon.png' />"
				+ "<link rel='stylesheet' type='text/css' href='/PresentationLayer/resources/css/relatorios.css' /></head><body>" );

			context.Response.Write( "<table width=100% border=0 cellspacing=1 ><caption id='caption'>Gerando...</caption><thead><tr>" );

			// escreve o NOME das colunas de acordo com os ALIAS DO SELECT
			for( int i = 0; i < reader.FieldCount; i++ ) {
				context.Response.Write( "<th>" + reader.GetName( i ) + "</th>" );
			}
			context.Response.Write( "</tr></thead><tbody>" );

			// escreve os DADOS de cada registro
			long count = 0;
			while( reader.Read() ) {
				context.Response.Write( "<tr>" );
				for( int i = 0; i < reader.FieldCount; i++ ) {
					context.Response.Write( "<td>" + reader[i] + "</td>" );
				}
				context.Response.Write( "</tr>" );
				count++;
			}
			context.Response.Write( "</tbody></table>" );
			context.Response.Write( "<script type='text/javascript'>document.getElementById('caption').innerHTML = 'Total: "+ count +" registro(s) encontrado(s)';window.print();</script>" );
			context.Response.Write( "</body></html>" );

		}

		private static void gerarExcel( HttpContext context, MySqlDataReader reader ) {

			StringBuilder html = new StringBuilder();

			context.Response.ClearContent();
			context.Response.ContentType = "application/vnd.ms-excel";
			context.Response.AddHeader( "content-disposition", "attachment; filename=relatorio.xls" );

			html.Append( "<html><head><title>Relatório de Clientes</title></head><body>" );

			html.Append( "<table width=100% border=0 cellspacing=1 ><caption id='caption'>[QTD_RECORDS]</caption><thead><tr>" );

			// escreve o NOME das colunas de acordo com os ALIAS DO SELECT
			for( int i = 0; i < reader.FieldCount; i++ ) {
				html.Append( "<th>" + reader.GetName( i ) + "</th>" );
			}
			html.Append( "</tr></thead><tbody>" );

			// escreve os DADOS de cada registro
			long count = 0;
			while( reader.Read() ) {
				html.Append( "<tr>" );
				for( int i = 0; i < reader.FieldCount; i++ ) {
					html.Append( "<td>" + reader[i] + "</td>" );
				}
				html.Append( "</tr>" );
				count++;
			}
			html.Append( "</tbody></table>" );
			html.Append( "</body></html>" );
			html.Replace( "[QTD_RECORDS]", "Total: "+ count +" registro(s) encontrado(s)" );

			context.Response.Write( html.ToString() );
			context.Response.End();
		}

		private static void gerarPdf( HttpContext context, MySqlDataReader reader ) {

			StringBuilder html = new StringBuilder();

			context.Response.ClearContent();
			context.Response.ContentType = "application/pdf";
			context.Response.AddHeader( "content-disposition", "attachment; filename=relatorio.pdf" );

			html.Append( "<html><head><title>Relatório de Clientes</title></head><body>" );

			html.Append( "<table width=100% border=0 cellspacing=1 ><caption id='caption'>[QTD_RECORDS]</caption><thead><tr>" );

			// escreve o NOME das colunas de acordo com os ALIAS DO SELECT
			for( int i = 0; i < reader.FieldCount; i++ ) {
				html.Append( "<th>" + reader.GetName( i ) + "</th>" );
			}
			html.Append( "</tr></thead><tbody>" );

			// escreve os DADOS de cada registro
			long count = 0;
			while( reader.Read() ) {
				html.Append( "<tr>" );
				for( int i = 0; i < reader.FieldCount; i++ ) {
					html.Append( "<td>" + reader[i] + "</td>" );
				}
				html.Append( "</tr>" );
				count++;
			}
			html.Append( "</tbody></table>" );
			html.Append( "</body></html>" );

			html.Replace( "[QTD_RECORDS]", "Total: "+ count +" registro(s) encontrado(s)" );

			Document document = new Document();
			PdfWriter.GetInstance( document, context.Response.OutputStream );
			document.Open();
			HTMLWorker htmlWorker = new HTMLWorker( document );
			htmlWorker.Parse( new StringReader( html.ToString() ) );
			document.Close();
			context.Response.Write( document );
			context.Response.End();
		}

		private static String construirColunasSql( String colunasAlias ) {

			List<String> colunasSql = new List<String>();
			String[] cols = colunasAlias.Split( new char[] { ',' } );

			for( int i = 0; i < cols.Length; i++ ) {
				switch( cols[i] ) {
					case "codigo":
						colunasSql.Add( "tb_clientes.cod_cliente AS 'Cod.' " );
						break;
					case "nome":
						colunasSql.Add( "nom_cliente AS Nome" );
						break;
					case "conjuge":
						colunasSql.Add( "nom_conjuge AS Conjuge" );
						break;
					case "tipoCliente":
						colunasSql.Add( "nom_tipo_cliente AS Tipo" );
						break;
					case "nascimento":
						colunasSql.Add( "dat_nascimento AS 'Nasc.' " );
						break;
					case "sexo":
						colunasSql.Add( "( CASE WHEN int_sexo = 1 THEN 'Masc.' ELSE 'Femi.' END ) AS Sexo" );
						break;
					case "rg":
						colunasSql.Add( "txt_rg AS RG" );
						break;
					case "cpf":
						colunasSql.Add( "txt_cpf AS CPF" );
						break;
					case "observacoes":
						colunasSql.Add( "txt_observacoes AS 'Obs.' " );
						break;
					case "meiosDeContato":
						colunasSql.Add( "( SELECT GROUP_CONCAT(CONCAT(nom_tipo_meio_de_contato,': ',txt_meio_de_contato, '; ')) FROM tb_meios_de_contato A INNER JOIN tb_tipos_meios_de_contato B ON A.cod_tipo_meio_de_contato = B.cod_tipo_meio_de_contato WHERE A.cod_cliente = tb_clientes.cod_cliente ) AS 'Meios Cont.' " );
						break;
					case "Endereco":
						colunasSql.Add( "( SELECT GROUP_CONCAT(CONCAT(nom_logradouro,', ', CAST(int_numero AS CHAR), '. ', nom_bairro, ' - ', nom_cidade, '; ')) FROM tb_enderecos A INNER JOIN tb_logradouros B ON A.cod_logradouro = B.cod_logradouro INNER JOIN tb_bairros C ON B.cod_bairro = C.cod_bairro INNER JOIN tb_cidades D ON C.cod_cidade = D.cod_cidade WHERE A.cod_cliente = tb_clientes.cod_cliente ) AS 'Ends.' " );
						break;
				}
			}

			return String.Join( ",", colunasSql.ToArray() );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
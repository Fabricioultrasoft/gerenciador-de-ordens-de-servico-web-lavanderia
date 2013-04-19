using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Reflection;
using System.IO;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers {
	public class Compartilhado {

		/// <summary>
		/// Metodo usado para construir a resposta em forma de json para o navegador,
		/// este constroi a parte da mensagem de erro para o usuario
		/// </summary>
		/// <param name="json"></param>
		/// <param name="erros"></param>
		public static void construirParteDoJsonMensagensDeErros( ref StringBuilder json, List<Erro> erros ) {
			json.AppendLine( " \"message\": [" );
			foreach( Erro err in erros ) {
				tratarCaracteresEspeciais<Erro>( err );
				json.Append( " \" " );
				json.Append( "<p>" );
				json.Append( "<h6>Erro</h6>" );
				json.AppendFormat( "<b>Codigo:</b> {0}<br/>", err.numeroDoErro );
				json.AppendFormat( "<b>Causa:</b> {0}<br/>", err.mensagem );
				json.AppendFormat( "<b>Solução:</b> {0}", err.solucao );
				json.Append( "</p>" );
				json.Append( " \"," );
			}
			if( erros.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.AppendLine( "]," );
		}

		public static T tratarCaracteresEspeciais<T>( T obj ) {
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties();
			foreach( PropertyInfo property in properties ) {

				if( property.PropertyType == typeof( String ) ) {
					property.SetValue( obj,
						( (String) property.GetValue( obj,null ) )
							.Replace( @"\", @"\\" )
							.Replace( "\n", "\\n" )
							.Replace( "\r", "\\r" )
							.Replace( "'", "\"" )
							.Replace( "\"", "\\\"" )
					,null);
				}
			}

			return obj;
		}

		public static void gerarRelatorioTxt( String title, HttpContext context, MySqlDataReader reader ) {

			context.Response.ContentType = "text/html";

			context.Response.Write( "<html><head><title>" + title + "</title>"
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

		public static void gerarRelatorioExcel( String title, HttpContext context, MySqlDataReader reader ) {

			StringBuilder html = new StringBuilder();

			context.Response.ClearContent();
			context.Response.ContentType = "application/vnd.ms-excel";
			context.Response.AddHeader( "content-disposition", "attachment; filename=" + title.Replace(" ","_") + ".xls" );

			html.Append( "<html><head><title>" + title + "</title></head><body>" );

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

		public static void gerarRelatorioPdf( String title, HttpContext context, MySqlDataReader reader ) {

			StringBuilder html = new StringBuilder();

			context.Response.ClearContent();
			context.Response.ContentType = "application/pdf";
			context.Response.AddHeader( "content-disposition", "attachment; filename=" + title.Replace( " ", "_" ) + ".pdf" );

			html.Append( "<html><head><title>" + title + "</title></head><body>" );

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

			iTextSharp.text.Document document = new iTextSharp.text.Document();
			iTextSharp.text.pdf.PdfWriter.GetInstance( document, context.Response.OutputStream );
			document.Open();
			iTextSharp.text.html.simpleparser.HTMLWorker htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker( document );
			htmlWorker.Parse( new StringReader( html.ToString() ) );
			document.Close();
			context.Response.Write( document );
			context.Response.End();
		}
	}
}
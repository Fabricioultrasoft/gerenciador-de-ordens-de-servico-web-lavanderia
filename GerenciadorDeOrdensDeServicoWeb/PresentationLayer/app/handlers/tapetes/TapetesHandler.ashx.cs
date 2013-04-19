using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using System.Text;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.tapetes {
	/// <summary>
	/// Summary description for TapetesHandler
	/// </summary>
	public class TapetesHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createTapetes( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					response = readTapetes( start, limit );
					break;
				case "update":
					response = updateTapetes( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyTapetes( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createTapetes( String records ) {
			List<Tapete> tapetes = jsonToTapetes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTapetes.cadastrar( ref tapetes );

			#region CONSTROI O JSON
			formatarSaida( ref tapetes );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tapetes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Tapete tapete in tapetes ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tapete.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tapete.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\", ", tapete.descricao );
				jsonResposta.AppendFormat( " \"ativo\": {0} ", tapete.ativo.ToString().ToLower() );
				jsonResposta.Append( " }\n," );
			}
			if( tapetes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readTapetes( UInt32 start, UInt32 limit ) {
			List<Tapete> tapetes;
			List<Erro> erros;
			erros = GerenciadorDeTapetes.preencher( out tapetes, start, limit );
			long qtdRegistros = GerenciadorDeTapetes.count();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaida( ref tapetes );
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Tapete tapete in tapetes ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0}, ", tapete.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tapete.nome );
					jsonResposta.AppendFormat( " \"descricao\": \"{0}\", ", tapete.descricao );
					jsonResposta.AppendFormat( " \"ativo\": {0} ", tapete.ativo.ToString().ToLower() );
					jsonResposta.Append( "}," );
				}
				if( tapetes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
				jsonResposta.AppendLine( "]" );
			}

			// SE NAO, preenche o json com as mensagens de erros
			else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String updateTapetes( String records ) {
			List<Tapete> tapetes = jsonToTapetes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTapetes.atualizar( tapetes );

			#region CONSTROI O JSON
			formatarSaida( ref tapetes );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tapetes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Tapete tapete in tapetes ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tapete.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tapete.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\", ", tapete.descricao );
				jsonResposta.AppendFormat( " \"ativo\": {0} ", tapete.ativo.ToString().ToLower() );
				jsonResposta.Append( "}," );
			}
			if( tapetes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyTapetes( String records ) {
			List<Tapete> tapetes = jsonToTapetes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTapetes.excluir( tapetes );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tapetes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados excluidos com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": []" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		public static List<Tapete> jsonToTapetes( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Deserialize<List<Tapete>>( json );
		}

		public static void formatarSaida( ref List<Tapete> tapetes ) {
			for( int i = 0; i < tapetes.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Tapete>( tapetes[i] );
				//tapete.nome = tapete.nome.Replace( @"\", @"\\" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" ).Replace( "\"", "\\\"" );//.Replace( "&", "&amp;" ).Replace( "<", "&lt;" ).Replace( ">", "&gt;" );
				//tapete.descricao = tapete.descricao.Replace( @"\", @"\\" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" ).Replace( "\"", "\\\"" );//.Replace( "&", "&amp;" ).Replace( "<", "&lt;" ).Replace( ">", "&gt;" );
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
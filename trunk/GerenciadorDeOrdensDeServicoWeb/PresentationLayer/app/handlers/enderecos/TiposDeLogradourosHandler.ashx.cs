using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.enderecos {
	/// <summary>
	/// Summary description for TiposDeLogradourosHandler
	/// </summary>
	public class TiposDeLogradourosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createTiposDeLogradouros( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					response = readTiposDeLogradouros( start, limit );
					break;
				case "update":
					response = updateTiposDeLogradouros( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyTiposDeLogradouros( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createTiposDeLogradouros( String records ) {
			List<TipoDeLogradouro> tiposDeLogradouros = jsonToTiposDeLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeLogradouros.cadastrar( ref tiposDeLogradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tiposDeLogradouros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tipoDeLogradouro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", tipoDeLogradouro.nome );
				jsonResposta.Append( " }," );
			}
			if( tiposDeLogradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readTiposDeLogradouros( UInt32 start, UInt32 limit ) {
			List<TipoDeLogradouro> tiposDeLogradouros;
			List<Erro> erros;
			erros = GerenciadorDeTiposDeLogradouros.preencher( out tiposDeLogradouros, start, limit );

			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", tiposDeLogradouros.Count );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0},", tipoDeLogradouro.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", tipoDeLogradouro.nome );
					jsonResposta.Append( "}," );
				}
				if( tiposDeLogradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateTiposDeLogradouros( String records ) {
			List<TipoDeLogradouro> tiposDeLogradouros = jsonToTiposDeLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeLogradouros.atualizar( tiposDeLogradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tiposDeLogradouros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tipoDeLogradouro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", tipoDeLogradouro.nome );
				jsonResposta.Append( " }," );
			}
			if( tiposDeLogradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyTiposDeLogradouros( String records ) {
			List<TipoDeLogradouro> tiposDeLogradouros = jsonToTiposDeLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeLogradouros.excluir( tiposDeLogradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tiposDeLogradouros.Count + "," );

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

		public static List<TipoDeLogradouro> jsonToTiposDeLogradouros( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Deserialize<List<TipoDeLogradouro>>( json );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
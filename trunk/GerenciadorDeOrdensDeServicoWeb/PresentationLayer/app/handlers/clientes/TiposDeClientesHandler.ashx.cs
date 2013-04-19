using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes;
using System.Web.Script.Serialization;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.clientes {
	/// <summary>
	/// Summary description for TiposDeClientesHandler
	/// </summary>
	public class TiposDeClientesHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createTiposDeClientes( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					bool? ativo = null;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					if( String.IsNullOrEmpty( context.Request.QueryString["ativo"] ) == false ) {
						ativo = bool.Parse( context.Request.QueryString["ativo"] );
					}

					response = readTiposDeClientes( start, limit, ativo );
					break;
				case "update":
					response = updateTiposDeClientes( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyTiposDeClientes( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createTiposDeClientes( String records ) {
			List<TipoDeCliente> tiposDeClientes = jsonToTiposDeClientes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeClientes.cadastrar( ref tiposDeClientes );

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + tiposDeClientes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [" );
			foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tipoDeCliente.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tipoDeCliente.nome );
				jsonResposta.AppendFormat( " \"ativo\": {0} ", tipoDeCliente.ativo.ToString().ToLower() );
				jsonResposta.Append( " }," );
			}
			if( tiposDeClientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readTiposDeClientes( UInt32 start, UInt32 limit, bool? ativo ) {
			List<TipoDeCliente> tiposDeClientes;
			List<Erro> erros;
			erros = GerenciadorDeTiposDeClientes.preencher( out tiposDeClientes, start, limit, ativo );
			long qtdRegistros = GerenciadorDeTiposDeClientes.count();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0},", tipoDeCliente.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tipoDeCliente.nome );
					jsonResposta.AppendFormat( " \"ativo\": {0} ", tipoDeCliente.ativo.ToString().ToLower() );
					jsonResposta.Append( "}," );
				}
				if( tiposDeClientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateTiposDeClientes( String records ) {
			List<TipoDeCliente> tiposDeClientes = jsonToTiposDeClientes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeClientes.atualizar( tiposDeClientes );

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + tiposDeClientes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [" );
			foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", tipoDeCliente.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", tipoDeCliente.nome );
				jsonResposta.AppendFormat( " \"ativo\": {0} ", tipoDeCliente.ativo.ToString().ToLower() );
				jsonResposta.Append( " }," );
			}
			if( tiposDeClientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyTiposDeClientes( String records ) {
			List<TipoDeCliente> tiposDeClientes = jsonToTiposDeClientes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeTiposDeClientes.excluir( tiposDeClientes );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + tiposDeClientes.Count + "," );

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

		public static List<TipoDeCliente> jsonToTiposDeClientes( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Deserialize<List<TipoDeCliente>>( json );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
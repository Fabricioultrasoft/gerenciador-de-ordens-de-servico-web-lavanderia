using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos;
using System.Text;
using System.Web.Script.Serialization;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.enderecos {
	/// <summary>
	/// Summary description for EstadosHandler
	/// </summary>
	public class EstadosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;
			
			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createEstados( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					UInt32 codigoPais = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					UInt32.TryParse( context.Request.QueryString["codigoPais"], out codigoPais );

					response = readEstados( start, limit, codigoPais );
					break;
				case "update":
					response = updateEstados( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyEstados( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createEstados( String records ) {
			List<Estado> estados = jsonToEstados( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeEstados.cadastrar( ref estados );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + estados.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Estado estado in estados ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", estado.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( estados.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "    ]\n" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readEstados( UInt32 start, UInt32 limit, UInt32 codigoPais ) {
			List<Estado> estados;
			List<Erro> erros;
			erros = GerenciadorDeEstados.preencher( out estados, start, limit, codigoPais );
			long qtdRegistros = GerenciadorDeEstados.count();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Estado estado in estados ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0},", estado.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", estado.nome );
					jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", estado.pais.codigo );
					jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", estado.pais.nome );
					jsonResposta.Append( "}," );
				}
				if( estados.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateEstados( String records ) {
			List<Estado> estados = jsonToEstados( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeEstados.atualizar( estados );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + estados.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Estado estado in estados ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", estado.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( estados.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyEstados( String records ) {
			List<Estado> estados = jsonToEstados( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeEstados.excluir( estados );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + estados.Count + "," );

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

		public static List<Estado> jsonToEstados( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Estado> estados = new List<Estado>();

			List<Dictionary<String, String>> list = js.Deserialize<List<Dictionary<String, String>>>( json );

			foreach( Dictionary<String, String> estadoTemp in list ) {
				Estado estado = new Estado();
				estado.codigo = UInt32.Parse(estadoTemp["codigo"]);
				estado.nome = estadoTemp["nome"].Trim();
				estado.pais.codigo = UInt32.Parse(estadoTemp["codigoPais"]);
				estado.pais.nome = estadoTemp["nomePais"].Trim();

				estados.Add( estado );
			}

			return estados;
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
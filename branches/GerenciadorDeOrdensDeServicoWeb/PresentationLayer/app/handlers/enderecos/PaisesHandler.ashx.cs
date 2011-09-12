using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Web.Script.Serialization;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.enderecos {
	/// <summary>
	/// Summary description for PaisesHandler
	/// </summary>
	public class PaisesHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createPaises( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					response = readPaises( start, limit );
					break;
				case "update":
					response = updatePaises( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyPaises( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createPaises( String records ) {
			List<Pais> paises = jsonToPaises( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDePaises.cadastrarListaDePaises( ref paises );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + paises.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Pais pais in paises ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", pais.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( paises.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readPaises(UInt32 start, UInt32 limit) {
			List<Pais> paises;
			List<Erro> erros;
			erros = GerenciadorDePaises.preencherListaDePaises( out paises, start, limit );
			long qtdRegistros = GerenciadorDePaises.countPaises();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Pais pais in paises ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0},", pais.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", pais.nome );
					jsonResposta.Append( "}," );
				}
				if( paises.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updatePaises( String records ) {
			List<Pais> paises = jsonToPaises( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDePaises.atualizarListaDePaises( paises );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + paises.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Pais pais in paises ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", pais.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( paises.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyPaises( String records ) {
			List<Pais> paises = jsonToPaises( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDePaises.excluirListaDePaises( paises );
			
			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + paises.Count + "," );

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
		
		public static List<Pais> jsonToPaises(String json) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Deserialize<List<Pais>>( json );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
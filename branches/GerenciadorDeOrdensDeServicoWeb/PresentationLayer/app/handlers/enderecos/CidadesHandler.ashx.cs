using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.enderecos {
	/// <summary>
	/// Summary description for CidadesHandler
	/// </summary>
	public class CidadesHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createCidades( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					UInt32 codigoEstado = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					UInt32.TryParse( context.Request.QueryString["codigoEstado"], out codigoEstado );

					response = readCidades( start, limit, codigoEstado );
					break;
				case "update":
					response = updateCidades( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyCidades( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createCidades( String records ) {
			List<Cidade> cidades = jsonToCidades( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeCidades.cadastrarListaDeCidades( ref cidades );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + cidades.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Cidade cidade in cidades ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", cidade.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", cidade.estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( cidades.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "    ]\n" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readCidades( UInt32 start, UInt32 limit, UInt32 codigoEstado ) {
			List<Cidade> cidades;
			List<Erro> erros;
			erros = GerenciadorDeCidades.preencherListaDeCidades( out cidades, start, limit, codigoEstado );
			long qtdRegistros = GerenciadorDeCidades.countCidades();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Cidade cidade in cidades ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0}, ", cidade.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", cidade.nome );
					jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", cidade.estado.codigo );
					jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", cidade.estado.nome );
					jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", cidade.estado.pais.codigo );
					jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", cidade.estado.pais.nome );
					jsonResposta.Append( "}," );
				}
				if( cidades.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateCidades( String records ) {
			List<Cidade> cidades = jsonToCidades( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeCidades.atualizarListaDeCidades( cidades );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + cidades.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Cidade cidade in cidades ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", cidade.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", cidade.estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( cidades.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyCidades( String records ) {
			List<Cidade> cidades = jsonToCidades( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeCidades.excluirListaDeCidades( cidades );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + cidades.Count + "," );

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

		public static List<Cidade> jsonToCidades( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Cidade> cidades = new List<Cidade>();

			List<Dictionary<String, String>> list = js.Deserialize<List<Dictionary<String, String>>>( json );

			foreach( Dictionary<String, String> estadoTemp in list ) {
				Cidade cidade = new Cidade();
				cidade.codigo = UInt32.Parse( estadoTemp["codigo"] );
				cidade.nome = estadoTemp["nome"].Trim();
				cidade.estado.codigo = UInt32.Parse( estadoTemp["codigoEstado"] );
				cidade.estado.nome = estadoTemp["nomeEstado"].Trim();
				cidade.estado.pais.codigo = UInt32.Parse( estadoTemp["codigoPais"] );
				cidade.estado.pais.nome = estadoTemp["nomePais"].Trim();

				cidades.Add( cidade );
			}

			return cidades;
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
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
	/// Summary description for BairrosHandler
	/// </summary>
	public class BairrosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createBairros( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					UInt32 codigoCidade = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					UInt32.TryParse( context.Request.QueryString["codigoCidade"], out codigoCidade );

					response = readBairros( start, limit, codigoCidade );
					break;
				case "update":
					response = updateBairros( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyBairros( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createBairros( String records ) {
			List<Bairro> bairros = jsonToBairros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeBairros.cadastrarListaDeBairros( ref bairros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + bairros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Bairro bairro in bairros ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", bairro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", bairro.nome );
				jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", bairro.cidade.codigo );
				jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", bairro.cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", bairro.cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", bairro.cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", bairro.cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", bairro.cidade.estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( bairros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "    ]\n" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readBairros( UInt32 start, UInt32 limit, UInt32 codigoCidade ) {
			List<Bairro> bairros;
			List<Erro> erros;
			erros = GerenciadorDeBairros.preencherListaDeBairros( out bairros, start, limit, codigoCidade );
			long qtdRegistros = GerenciadorDeBairros.countBairros();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Bairro bairro in bairros ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0}, ", bairro.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", bairro.nome );
					jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", bairro.cidade.codigo );
					jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", bairro.cidade.nome );
					jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", bairro.cidade.estado.codigo );
					jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", bairro.cidade.estado.nome );
					jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", bairro.cidade.estado.pais.codigo );
					jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", bairro.cidade.estado.pais.nome );
					jsonResposta.Append( "}," );
				}
				if( bairros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateBairros( String records ) {
			List<Bairro> bairros = jsonToBairros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeBairros.atualizarListaDeBairros( bairros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + bairros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Bairro bairro in bairros ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", bairro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", bairro.nome );
				jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", bairro.cidade.codigo );
				jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", bairro.cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", bairro.cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", bairro.cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", bairro.cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", bairro.cidade.estado.pais.nome );
				jsonResposta.Append( " },\n" );
			}
			if( bairros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyBairros( String records ) {
			List<Bairro> bairros = jsonToBairros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeBairros.excluirListaDeBairros( bairros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + bairros.Count + "," );

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

		public static List<Bairro> jsonToBairros( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Bairro> bairros = new List<Bairro>();

			List<Dictionary<String, String>> list = js.Deserialize<List<Dictionary<String, String>>>( json );

			foreach( Dictionary<String, String> estadoTemp in list ) {
				Bairro bairro = new Bairro();
				bairro.codigo = UInt32.Parse( estadoTemp["codigo"] );
				bairro.nome = estadoTemp["nome"].Trim();
				bairro.cidade.codigo = UInt32.Parse( estadoTemp["codigoCidade"] );
				bairro.cidade.nome = estadoTemp["nomeCidade"].Trim();
				bairro.cidade.estado.codigo = UInt32.Parse( estadoTemp["codigoEstado"] );
				bairro.cidade.estado.nome = estadoTemp["nomeEstado"].Trim();
				bairro.cidade.estado.pais.codigo = UInt32.Parse( estadoTemp["codigoPais"] );
				bairro.cidade.estado.pais.nome = estadoTemp["nomePais"].Trim();

				bairros.Add( bairro );
			}

			return bairros;
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
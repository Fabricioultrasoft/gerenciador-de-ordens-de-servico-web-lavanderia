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
	/// Summary description for LogradourosHandler
	/// </summary>
	public class LogradourosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createLogradouros( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					UInt32 codigoBairro = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					UInt32.TryParse( context.Request.QueryString["codigoBairro"], out codigoBairro );

					response = readLogradouros( start, limit, codigoBairro );
					break;
				case "update":
					response = updateLogradouros( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyLogradouros( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createLogradouros( String records ) {
			List<Logradouro> logradouros = jsonToLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeLogradouros.cadastrarListaDeLogradouros( ref logradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + logradouros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Logradouro logradouro in logradouros ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", logradouro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", logradouro.nome );
				jsonResposta.AppendFormat( " \"cep\": \"{0}\", ", logradouro.cep );
				jsonResposta.AppendFormat( " \"codigoTipoDeLogradouro\": {0}, ", logradouro.tipoDeLogradouro.codigo );
				jsonResposta.AppendFormat( " \"nomeTipoDeLogradouro\": \"{0}\", ", logradouro.tipoDeLogradouro.nome );
				jsonResposta.AppendFormat( " \"codigoBairro\": {0}, ", logradouro.bairro.codigo );
				jsonResposta.AppendFormat( " \"nomeBairro\": \"{0}\", ", logradouro.bairro.nome );
				jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", logradouro.bairro.cidade.codigo );
				jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", logradouro.bairro.cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", logradouro.bairro.cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", logradouro.bairro.cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", logradouro.bairro.cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", logradouro.bairro.cidade.estado.pais.nome );
				jsonResposta.Append( " }\n," );
			}
			if( logradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "    ]\n" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readLogradouros( UInt32 start, UInt32 limit, UInt32 codigoBairro ) {
			List<Logradouro> logradouros;
			List<Erro> erros;
			erros = GerenciadorDeLogradouros.preencherListaDeLogradouros( out logradouros, start, limit, codigoBairro );
			long qtdRegistros = GerenciadorDeLogradouros.countLogradouros();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			jsonResposta.Append( "{" );
			jsonResposta.AppendFormat( " \"total\": {0}, \n", qtdRegistros );

			// SE nao houve erros
			// ENTAO cria os dados a serem retornados
			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"data\": [" );
				foreach( Logradouro logradouro in logradouros ) {
					jsonResposta.Append( "{" );
					jsonResposta.AppendFormat( " \"codigo\": {0}, ", logradouro.codigo );
					jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", logradouro.nome );
					jsonResposta.AppendFormat( " \"cep\": \"{0}\", ", logradouro.cep );
					jsonResposta.AppendFormat( " \"codigoTipoDeLogradouro\": {0}, ", logradouro.tipoDeLogradouro.codigo );
					jsonResposta.AppendFormat( " \"nomeTipoDeLogradouro\": \"{0}\", ", logradouro.tipoDeLogradouro.nome );
					jsonResposta.AppendFormat( " \"codigoBairro\": {0}, ", logradouro.bairro.codigo );
					jsonResposta.AppendFormat( " \"nomeBairro\": \"{0}\", ", logradouro.bairro.nome );
					jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", logradouro.bairro.cidade.codigo );
					jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", logradouro.bairro.cidade.nome );
					jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", logradouro.bairro.cidade.estado.codigo );
					jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", logradouro.bairro.cidade.estado.nome );
					jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", logradouro.bairro.cidade.estado.pais.codigo );
					jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", logradouro.bairro.cidade.estado.pais.nome );
					jsonResposta.Append( "}," );
				}
				if( logradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
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

		private String updateLogradouros( String records ) {
			List<Logradouro> logradouros = jsonToLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeLogradouros.atualizarListaDeLogradouros( logradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + logradouros.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados alterados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Logradouro logradouro in logradouros ) {
				jsonResposta.Append( "\t\t{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", logradouro.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", logradouro.nome );
				jsonResposta.AppendFormat( " \"cep\": \"{0}\", ", logradouro.cep );
				jsonResposta.AppendFormat( " \"codigoTipoDeLogradouro\": {0}, ", logradouro.tipoDeLogradouro.codigo );
				jsonResposta.AppendFormat( " \"nomeTipoDeLogradouro\": \"{0}\", ", logradouro.tipoDeLogradouro.nome );
				jsonResposta.AppendFormat( " \"codigoBairro\": {0}, ", logradouro.bairro.codigo );
				jsonResposta.AppendFormat( " \"nomeBairro\": \"{0}\", ", logradouro.bairro.nome );
				jsonResposta.AppendFormat( " \"codigoCidade\": {0}, ", logradouro.bairro.cidade.codigo );
				jsonResposta.AppendFormat( " \"nomeCidade\": \"{0}\", ", logradouro.bairro.cidade.nome );
				jsonResposta.AppendFormat( " \"codigoEstado\": {0}, ", logradouro.bairro.cidade.estado.codigo );
				jsonResposta.AppendFormat( " \"nomeEstado\": \"{0}\", ", logradouro.bairro.cidade.estado.nome );
				jsonResposta.AppendFormat( " \"codigoPais\": {0}, ", logradouro.bairro.cidade.estado.pais.codigo );
				jsonResposta.AppendFormat( " \"nomePais\": \"{0}\" ", logradouro.bairro.cidade.estado.pais.nome );
				jsonResposta.Append( " },\n" );
			}
			if( logradouros.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyLogradouros( String records ) {
			List<Logradouro> logradouros = jsonToLogradouros( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeLogradouros.excluirListaDeLogradouros( logradouros );

			#region CONSTROI O JSON
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + logradouros.Count + "," );

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

		public static List<Logradouro> jsonToLogradouros( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Logradouro> logradouros = new List<Logradouro>();

			List<Dictionary<String, String>> list = js.Deserialize<List<Dictionary<String, String>>>( json );

			foreach( Dictionary<String, String> estadoTemp in list ) {
				Logradouro logradouro = new Logradouro();
				logradouro.codigo = UInt32.Parse( estadoTemp["codigo"] );
				logradouro.nome = estadoTemp["nome"].Trim();
				logradouro.cep = estadoTemp["cep"].Trim();
				logradouro.bairro.codigo = UInt32.Parse( estadoTemp["codigoBairro"] );
				logradouro.bairro.nome = estadoTemp["nomeBairro"].Trim();
				logradouro.bairro.cidade.codigo = UInt32.Parse( estadoTemp["codigoCidade"] );
				logradouro.bairro.cidade.nome = estadoTemp["nomeCidade"].Trim();
				logradouro.bairro.cidade.estado.codigo = UInt32.Parse( estadoTemp["codigoEstado"] );
				logradouro.bairro.cidade.estado.nome = estadoTemp["nomeEstado"].Trim();
				logradouro.bairro.cidade.estado.pais.codigo = UInt32.Parse( estadoTemp["codigoPais"] );
				logradouro.bairro.cidade.estado.pais.nome = estadoTemp["nomePais"].Trim();
				logradouro.tipoDeLogradouro.codigo = UInt32.Parse( estadoTemp["codigoTipoDeLogradouro"] );
				logradouro.tipoDeLogradouro.nome = estadoTemp["nomeTipoDeLogradouro"].Trim();

				logradouros.Add( logradouro );
			}

			return logradouros;
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.servicos;
using System.Globalization;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.servicos {
	/// <summary>
	/// Summary description for ServicosHandler
	/// </summary>
	public class ServicosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					//response = createClientes( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					
					response = readServicos( start, limit );
					break;
				case "update":
					//response = updateClientes( context.Request.Form["records"] );
					break;
				case "destroy":
					//response = destroyClientes( context.Request.Form["records"], context );
					break;

				case "readServico":
					UInt32 codigoServico = 0;
					UInt32.TryParse( context.Request.QueryString["codigoServico"], out codigoServico );

					response = readServico( codigoServico );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String readServicos( UInt32 start, UInt32 limit ) {
			List<Servico> servicos;
			List<Erro> erros;

			erros = GerenciadorDeServicos.preencherListaDeServicos( out servicos, start, limit );
			long qtdRegistros = GerenciadorDeServicos.countServicos();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaidaServicos( ref servicos );
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + qtdRegistros + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Foram encontrados " + qtdRegistros + " registros\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [" );
			foreach( Servico servico in servicos ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " codigo: {0},", servico.codigo );
				jsonResposta.AppendFormat( " nome: \"{0}\",", servico.nome );
				jsonResposta.AppendFormat( " descricao: \"{0}\",", servico.descricao );
				jsonResposta.AppendFormat( " codigoCobradoPor: {0},", (int) servico.cobradoPor );
				jsonResposta.AppendFormat( " nomeCobradoPor: \"{0}\" ", servico.cobradoPor );
				jsonResposta.Append( "}" );
			}
			if( servicos.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readServico( UInt32 codigoServico ) {
			Servico servico;
			List<Erro> erros;

			erros = GerenciadorDeServicos.preencherServico( codigoServico, out servico );
			
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			Compartilhado.tratarCaracteresEspeciais<Servico>( servico );
			formatarSaidaValores( servico.valores );

			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": 1," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"1 registro encontrado\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( " \"text\": \".\", " );
			jsonResposta.AppendFormat( " \"codigoServico\": {0},", servico.codigo );
			jsonResposta.AppendFormat( " \"nome\": \"{0}\",", servico.nome );
			jsonResposta.AppendFormat( " \"descricao\": \"{0}\",", servico.descricao );
			jsonResposta.AppendFormat( " \"codigoCobradoPor\": {0},", (int) servico.cobradoPor );
			jsonResposta.AppendFormat( " \"nomeCobradoPor\": \"{0}\", ", servico.cobradoPor );
			montarValoresJson( ref jsonResposta, servico.valores );
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		public static void formatarSaidaServicos( ref List<Servico> servicos ) {
			for( int i = 0; i < servicos.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Servico>( servicos[i] );
			}
		}

		public static void formatarSaidaValores( List<ValorDeServico> valores ) {
			for( int i = 0; i < valores.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<ValorDeServico>( valores[i] );
			}
		}

		private static void montarValoresJson(ref StringBuilder json, List<ValorDeServico> valores) {
			json.Append( " \"valores\": [" );
			foreach( ValorDeServico val in valores ) {
				json.Append( "{" );
				json.AppendFormat( " \"codigo\": {0},", val.codigo );
				json.AppendFormat( " \"codigoServico\": {0},", val.codigoServico );
				json.AppendFormat( " \"valor\": {0},", val.valorInicial.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"valorAcima10m2\": {0},", val.valorAcima10m2.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"codigoTapete\": {0},", val.tapete.codigo );
				json.AppendFormat( " \"nomeTapete\": \"{0}\",", val.tapete.nome );
				json.AppendFormat( " \"codigoTipoDeCliente\": {0},", val.tipoDeCliente.codigo );
				json.AppendFormat( " \"nomeTipoDeCliente\": \"{0}\", ", String.IsNullOrEmpty( val.tipoDeCliente.nome ) ? "Todos" : val.tipoDeCliente.nome );
				json.AppendFormat( " \"iconCls\": \"{0}\", ", val.tipoDeCliente.codigo == 0 ? "tapete-thumb" : "tapete-estrela-thumb" );
				json.AppendFormat( " \"leaf\": {0}, ", val.valoresEspeciais.Count == 0 ? "true" : "false" );
				montarValoresJson( ref json, val.valoresEspeciais );
				json.Append( "}," );
			}
			if( valores.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.Append( "]" );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
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
using System.Web.SessionState;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.servicos {
	/// <summary>
	/// Summary description for ServicosHandler
	/// </summary>
	public class ServicosHandler : IHttpHandler, IRequiresSessionState {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createServicos( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					bool apenasDadosBasicos = false;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					Boolean.TryParse( context.Request.QueryString["apenasDadosBasicos"], out apenasDadosBasicos );

					context.Session["readServicos_start"] = start;
					context.Session["readServicos_limit"] = limit;
					context.Session["readServicos_apenasDadosBasicos"] = apenasDadosBasicos;

					response = readServicos( start, limit, apenasDadosBasicos );
					break;
				case "readEspecificos":
					UInt32 codigoTapete = 0;
					UInt32 codigoTipoDeCliente = 0;

					UInt32.TryParse( context.Request.QueryString["codigoTapete"], out codigoTapete );
					UInt32.TryParse( context.Request.QueryString["codigoTipoDeCliente"], out codigoTipoDeCliente );

					response = readServicosEspecificos( codigoTapete,codigoTipoDeCliente );
					break;
				case "update":
					response = updateServicos( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyServicos( context.Request.Form["records"] );
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

		private String createServicos( String records ) {
			List<Servico> servicos = jsonToServicos( records );
			List<Erro> erros = GerenciadorDeServicos.cadastrarListaDeServicos( ref servicos );

			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaidaServicos( ref servicos );
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + servicos.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [" );
			foreach( Servico servico in servicos ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0},", servico.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\",", servico.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\",", servico.descricao );
				jsonResposta.AppendFormat( " \"codigoCobradoPor\": {0},", (int) servico.cobradoPor );
				jsonResposta.AppendFormat( " \"nomeCobradoPor\": \"{0}\" ", servico.cobradoPor );
				jsonResposta.Append( "}," );
			}
			if( servicos.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readServicos( UInt32 start, UInt32 limit, bool apenasDadosBasicos ) {
			List<Servico> servicos;
			List<Erro> erros;

			erros = GerenciadorDeServicos.preencherListaDeServicos( out servicos, start, limit, apenasDadosBasicos );
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
				jsonResposta.AppendFormat( " \"codigo\": {0},", servico.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\",", servico.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\",", servico.descricao );
				jsonResposta.AppendFormat( " \"codigoCobradoPor\": {0},", (int) servico.cobradoPor );
				jsonResposta.AppendFormat( " \"nomeCobradoPor\": \"{0}\" ", servico.cobradoPor );
				jsonResposta.Append( "}," );
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
			jsonResposta.Append( " \"expanded\": true, " );
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

		private String readServicosEspecificos( UInt32 codigoTapete, UInt32 codigoTipoDeCliente ) {
			List<Servico> servicos;
			List<Erro> erros;

			erros = GerenciadorDeServicos.preencherListaDeServicosEspecificos( out servicos, codigoTapete, codigoTipoDeCliente );
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaidaServicos( ref servicos );
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + servicos.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Foram encontrados " + servicos.Count + " registros\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [" );
			foreach( Servico servico in servicos ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0},", servico.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\",", servico.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\",", servico.descricao );
				jsonResposta.AppendFormat( " \"codigoCobradoPor\": {0},", (int) servico.cobradoPor );
				jsonResposta.AppendFormat( " \"nomeCobradoPor\": \"{0}\", ", servico.cobradoPor );
				jsonResposta.AppendFormat( " \"codigoTapete\": {0}, ", servico.valores[0].tapete.codigo );
				jsonResposta.AppendFormat( " \"nomeTapete\": \"{0}\", ", servico.valores[0].tapete.nome );
				jsonResposta.AppendFormat( " \"codigoTipoDeCliente\": {0}, ", servico.valores[0].tipoDeCliente.codigo );
				jsonResposta.AppendFormat( " \"nomeTipoDeCliente\": \"{0}\", ", (servico.valores[0].tipoDeCliente.codigo > 0) ? servico.valores[0].tipoDeCliente.nome : "Todos" );
				jsonResposta.AppendFormat( " \"valor\": {0}, ", servico.valores[0].valorInicial.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				jsonResposta.AppendFormat( " \"valorAcima10m2\": {0} ", servico.valores[0].valorAcima10m2.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				jsonResposta.Append( "}," );
			}
			if( servicos.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );
			
			// fim do json
			jsonResposta.Append( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String updateServicos( String records ) {
			List<Servico> servicos = jsonToServicos( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeServicos.atualizarListaDeServicos( ref servicos );

			#region CONSTROI O JSON
			formatarSaidaServicos( ref servicos );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + servicos.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Servico servico in servicos ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0},", servico.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\",", servico.nome );
				jsonResposta.AppendFormat( " \"descricao\": \"{0}\",", servico.descricao );
				jsonResposta.AppendFormat( " \"codigoCobradoPor\": {0},", (int) servico.cobradoPor );
				jsonResposta.AppendFormat( " \"nomeCobradoPor\": \"{0}\" ", servico.cobradoPor );
				jsonResposta.Append( " }," );
			}
			if( servicos.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyServicos( String records ) {
			List<Servico> servicos = jsonToServicos( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeServicos.excluirListaDeServicos( servicos );

			#region CONSTROI O JSON
			formatarSaidaServicos( ref servicos );
			jsonResposta.Append( "{" );
			jsonResposta.Append( "    \"total\": " + servicos.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.Append( "    \"success\": true," );
				jsonResposta.Append( "    \"message\": [\"Dados excluidos com sucesso\"]," );
			} else {
				jsonResposta.Append( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.Append( "    \"data\": [] }" );
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

		private static void montarValoresJson( ref StringBuilder json, List<ValorDeServico> valores ) {
			json.Append( " \"children\": [" );
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
				//json.AppendFormat( " {0} ", val.valoresEspeciais.Count == 0 ? "\"leaf\": true," : "" );
				montarValoresJson( ref json, val.valoresEspeciais );
				json.Append( "}," );
			}
			if( valores.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.Append( "]" );
		}

		public static List<Servico> jsonToServicos( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Servico> servicos = new List<Servico>();

			List<Dictionary<String, Object>> list = js.Deserialize<List<Dictionary<String, Object>>>( json );

			foreach( Dictionary<String, Object> servicoTemp in list ) {
				Servico servico = new Servico();

				servico.codigo = UInt32.Parse( servicoTemp["codigo"].ToString() );
				servico.nome = servicoTemp["nome"].ToString();
				servico.descricao = servicoTemp["descricao"].ToString();
				servico.cobradoPor = (CobradoPor) Enum.Parse( typeof( CobradoPor ), servicoTemp["codigoCobradoPor"].ToString() );
				if( servicoTemp["valores"].ToString() != "" ) {
					servico.valores.AddRange( recuperarValores( servicoTemp["valores"], js ) );
				}
				servicos.Add( servico );
			}

			return servicos;
		}

		private static List<ValorDeServico> recuperarValores( Object objJson, JavaScriptSerializer js ) {
			List<ValorDeServico> valores = new List<ValorDeServico>();
			StringBuilder valoresJson = new StringBuilder();

			js.Serialize( objJson, valoresJson );

			foreach( Dictionary<String, Object> valorTemp in js.Deserialize<List<Dictionary<String, Object>>>( valoresJson.ToString() ) ) {
				ValorDeServico valor = new ValorDeServico();

				valor.codigo = UInt32.Parse( valorTemp["codigo"].ToString() );
				valor.codigoPai = UInt32.Parse( valorTemp["codigoPai"].ToString() );
				valor.codigoServico = UInt32.Parse( valorTemp["codigoServico"].ToString() );
				valor.tapete.codigo = UInt32.Parse( valorTemp["codigoTapete"].ToString() );
				valor.tapete.nome = valorTemp["nomeTapete"].ToString();
				valor.tipoDeCliente.codigo = UInt32.Parse( valorTemp["codigoTipoDeCliente"].ToString() );
				valor.tipoDeCliente.nome = valorTemp["nomeTipoDeCliente"].ToString();
				valor.valorInicial = Double.Parse( valorTemp["valor"].ToString() );
				valor.valorAcima10m2 = Double.Parse( valorTemp["valorAcima10m2"].ToString() );
				valor.valoresEspeciais.AddRange( recuperarValores( valorTemp["valoresAdicionais"], js ) );

				valores.Add( valor );
			}


			return valores;
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
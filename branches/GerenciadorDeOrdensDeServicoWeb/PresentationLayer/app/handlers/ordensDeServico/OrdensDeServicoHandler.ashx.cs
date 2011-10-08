using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.ordensDeServico;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.servicos;
using System.Globalization;
using System.Web.SessionState;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.clientes;
using GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.usuarios;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.ordensDeServico {
	/// <summary>
	/// Summary description for Handler1
	/// </summary>
	public class OrdensDeServicoHandler : IHttpHandler, IRequiresSessionState {

		private HttpContext thisContext;

		public void ProcessRequest( HttpContext context ) {

			thisContext = context;

			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createOrdensDeServico( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					String filters = String.Empty;
					String sorters = String.Empty;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
						filters = context.Request.QueryString["filter"];
					}
					if( String.IsNullOrEmpty( context.Request.QueryString["sort"] ) == false ) {
						sorters = context.Request.QueryString["sort"];
					}

					response = readOrdensDeServico( start, limit, filters, sorters );
					break;
				case "update":
					response = updateOrdensDeServico( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyOrdensDeServico( context.Request.Form["records"] );
					break;
				case "readStatus":
					response = readStatus();
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );

			Usuario usu = (Usuario) thisContext.Session["usuario"];
			foreach( OrdemDeServico os in ordensDeServico ) {
				if( os.usuario.codigo == 0 ) {
					os.usuario.codigo = usu.codigo;
				}
			}

			StringBuilder json = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.cadastrar( ref ordensDeServico );

			formatarSaida( ordensDeServico );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", ordensDeServico.Count );

			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.Append( " \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}

			json.AppendFormat( " \"data\": {0}", ordensDeServicoToJson( ordensDeServico ) );

			json.Append( "}" );

			return json.ToString();
		}

		private String readOrdensDeServico( UInt32 start, UInt32 limit, String jsonFilters, String jsonSorters ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<OrdemDeServico> ordensDeServico;
			List<Erro> erros;
			List<Filter> filters;
			List<Sorter> sorters;

			filters = js.Deserialize<List<Filter>>( jsonFilters );
			// se 'filters' for continuar sendo uma referencia nula, cria um objeto vazio dela
			if( filters == null ) {
				filters = new List<Filter>();
			}

			sorters = js.Deserialize<List<Sorter>>( jsonSorters );
			// se 'sorters' for continuar sendo uma referencia nula, cria um objeto vazio dela
			if( sorters == null ) {
				sorters = new List<Sorter>();
			}


			erros = GerenciadorDeOrdensDeServico.preencher( out ordensDeServico, start, limit, filters, sorters );
			long qtdRegistros = GerenciadorDeOrdensDeServico.count( filters );
			StringBuilder json = new StringBuilder();

			formatarSaida( ordensDeServico );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", qtdRegistros );

			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.AppendFormat( " \"message\": [\"Foram encontrados {0} registros\"],", qtdRegistros );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}

			json.AppendFormat( " \"data\": {0}", ordensDeServicoToJson( ordensDeServico ) );
			json.Append( "}" );

			return json.ToString();
		}

		private String updateOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );
			StringBuilder json = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.atualizar( ref ordensDeServico );

			formatarSaida( ordensDeServico );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", ordensDeServico.Count );

			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.Append( " \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}

			json.AppendFormat( " \"data\": {0}", ordensDeServicoToJson( ordensDeServico ) );
			json.Append( "}" );

			return json.ToString();
		}

		private String destroyOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );
			StringBuilder json = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.excluir( ordensDeServico );

			formatarSaida( ordensDeServico );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", ordensDeServico.Count );

			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.Append( " \"message\": [\"Dados excluidos com sucesso\"]," );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}

			json.Append( " \"data\": [] }" );

			return json.ToString();
		}

		private String readStatus() {
			List<Status> statusList = new List<Status>();
			List<Erro> erros = GerenciadorDeOrdensDeServico.preencher( out statusList );
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaida( statusList );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( " \"total\": " + statusList.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"message\": [\"Foram encontrados " + statusList.Count + " registros\"]," );
			} else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( " \"data\": [" );
			foreach( Status status in statusList ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", status.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\" ", status.nome );
				jsonResposta.Append( "}," );
			}
			if( statusList.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		public String ordemDeServicoToJson( OrdemDeServico os ) {
			StringBuilder json = new StringBuilder();
			formatarSaida( os.itens );

			json.Append( "{" );
			json.AppendFormat( " \"codigo\": {0}, ", os.codigo );
			json.AppendFormat( " \"numero\": {0}, ", os.numero );
			json.AppendFormat( " \"valorOriginal\": {0}, ", os.valorOriginal.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
			json.AppendFormat( " \"valorFinal\": {0}, ", os.valorFinal.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
			json.AppendFormat( " \"codigoStatus\": {0}, ", os.status.codigo );
			json.AppendFormat( " \"nomeStatus\": \"{0}\", ", os.status.nome );
			json.AppendFormat( " \"dataDeAbertura\": \"{0}\", ", os.dataDeAbertura.ToString( "dd/MM/yyyy" ) );
			json.AppendFormat( " \"previsaoDeConclusao\": \"{0}\", ", os.previsaoDeConclusao.ToString( "dd/MM/yyyy" ) );
			json.AppendFormat( " \"dataDeFechamento\": \"{0}\", ", ( os.dataDeFechamento.CompareTo( DateTime.MinValue ) > 0 ) ? os.dataDeFechamento.ToString( "dd/MM/yyyy" ) : "---" );
			json.AppendFormat( " \"observacoes\": \"{0}\", ", os.observacoes );
			json.AppendFormat( " \"codigoCliente\": {0}, ", os.cliente.codigo );
			json.AppendFormat( " \"nomeCliente\": \"{0}\", ", os.cliente.nome );
			json.AppendFormat( " \"cliente\": {0}, ", ClientesHandler.clienteToJson( os.cliente ) );
			json.AppendFormat( " \"usuario\": {0}, ", UsuariosHandler.usuarioToJson( os.usuario ) );


			json.Append( " \"itens\": [" );
			foreach( Item item in os.itens ) {

				json.Append( "{" );
				json.AppendFormat( " \"codigo\": {0}, ", item.codigo );
				json.AppendFormat( " \"codigoOrdemDeServico\": {0}, ", os.codigo );
				json.AppendFormat( " \"codigoTapete\": {0}, ", item.tapete.codigo );
				json.AppendFormat( " \"nomeTapete\": \"{0}\", ", item.tapete.nome );
				json.AppendFormat( " \"comprimento\": {0}, ", item.comprimento.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"largura\": {0}, ", item.largura.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"area\": {0}, ", item.area.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"valor\": {0}, ", item.valor.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
				json.AppendFormat( " \"observacoes\": \"{0}\", ", item.observacoes );

				json.Append( " \"servicosDoItem\": [" );
				foreach( ServicoDoItem servicoDoItem in item.servicosDoItem ) {
					json.Append( "{" );
					json.AppendFormat( " \"codigo\": {0}, ", servicoDoItem.codigo );
					json.AppendFormat( " \"codigoItem\": {0}, ", item.codigo );
					json.AppendFormat( " \"codigoServico\": {0}, ", servicoDoItem.servico.codigo );
					json.AppendFormat( " \"nomeServico\": \"{0}\", ", servicoDoItem.servico.nome );
					json.AppendFormat( " \"servico\": {0}, ", ServicosHandler.servicoEspecificoToJson( servicoDoItem.servico ) );
					json.AppendFormat( " \"quantidade_m_m2\": {0}, ", servicoDoItem.quantidade_m_m2.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
					json.AppendFormat( " \"valor\": {0} ", servicoDoItem.valor.ToString( "F", CultureInfo.CreateSpecificCulture( "en-US" ) ) );
					json.Append( "}," );
				}
				if( item.servicosDoItem.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
				json.Append( "]}," );

			}
			if( os.itens.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.Append( "]}" );

			return json.ToString();
		}

		public String ordensDeServicoToJson( List<OrdemDeServico> ordensDeServico ) {
			StringBuilder json = new StringBuilder();

			json.Append( "[" );
			foreach( OrdemDeServico os in ordensDeServico ) {
				json.AppendFormat( "{0},", ordemDeServicoToJson( os ) );
			}
			if( ordensDeServico.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.Append( "]" );

			return json.ToString();
		}

		public static List<OrdemDeServico> jsonToOrdensDeServico( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<OrdemDeServico> ordensDeServico = new List<OrdemDeServico>();

			List<Dictionary<String, Object>> list = js.Deserialize<List<Dictionary<String, Object>>>( json );

			foreach( Dictionary<String, Object> ordemTemp in list ) {
				OrdemDeServico ordem = new OrdemDeServico();

				ordem.codigo = UInt32.Parse( ordemTemp["codigo"].ToString() );
				ordem.numero = UInt32.Parse( ordemTemp["numero"].ToString() );
				ordem.valorOriginal = Double.Parse( ordemTemp["valorOriginal"].ToString() );
				ordem.valorFinal = Double.Parse( ordemTemp["valorFinal"].ToString() );
				try {
					ordem.status.codigo = UInt32.Parse( ordemTemp["codigoStatus"].ToString() );
					ordem.status.nome = ordemTemp["nomeStatus"].ToString();
				} catch { }
				ordem.dataDeAbertura = DateTime.Parse( ordemTemp["dataDeAbertura"].ToString() );
				ordem.previsaoDeConclusao = DateTime.Parse( ordemTemp["previsaoDeConclusao"].ToString() );
				try { ordem.dataDeFechamento = DateTime.Parse( ordemTemp["dataDeFechamento"].ToString() ); } catch { }
				ordem.observacoes = ordemTemp["observacoes"].ToString();

				try {
					StringBuilder cliTemp = new StringBuilder();
					js.Serialize( ordemTemp["cliente"], cliTemp );
					ordem.cliente = ClientesHandler.jsonToCliente( cliTemp.ToString() );
				} catch {
					ordem.cliente.codigo = UInt32.Parse( ordemTemp["codigoCliente"].ToString() );
					ordem.cliente.nome = ordemTemp["nomeCliente"].ToString();
				}

				try {
					StringBuilder usuTemp = new StringBuilder();
					js.Serialize(ordemTemp["usuario"],usuTemp);
					ordem.usuario = UsuariosHandler.jsonToUsuario( usuTemp.ToString() );
				} catch { }

				StringBuilder itensJson = new StringBuilder();
				js.Serialize( ordemTemp["itens"], itensJson );
				foreach( Dictionary<String, Object> itensTemp in js.Deserialize<List<Dictionary<String, Object>>>( itensJson.ToString() ) ) {
					Item item = new Item();

					item.codigo = UInt32.Parse( itensTemp["codigo"].ToString() );
					item.codigoOrdemDeServico = UInt32.Parse( itensTemp["codigoOrdemDeServico"].ToString() );
					item.tapete.codigo = UInt32.Parse( itensTemp["codigoTapete"].ToString() );
					item.tapete.nome = itensTemp["nomeTapete"].ToString();
					item.comprimento = float.Parse( itensTemp["comprimento"].ToString() );
					item.largura = float.Parse( itensTemp["largura"].ToString() );
					item.valor = Double.Parse( itensTemp["valor"].ToString() );
					item.observacoes = itensTemp["observacoes"].ToString();

					item.servicosDoItem.AddRange( jsonToServicosDoItem( itensTemp["servicosDoItem"], js ) );

					ordem.itens.Add( item );
				}

				ordensDeServico.Add( ordem );
			}

			return ordensDeServico;
		}

		public static List<ServicoDoItem> jsonToServicosDoItem( Object json, JavaScriptSerializer js ) {
			List<ServicoDoItem> itensServicos = new List<ServicoDoItem>();
			StringBuilder servicosJson = new StringBuilder();
			js.Serialize( json, servicosJson );

			List<Dictionary<String, Object>> list = js.Deserialize<List<Dictionary<String, Object>>>( servicosJson.ToString() );

			foreach( Dictionary<String, Object> servicosDoItemTemp in list ) {
				ServicoDoItem servicoDoItem = new ServicoDoItem();

				servicoDoItem.codigo = UInt32.Parse( servicosDoItemTemp["codigo"].ToString() );
				servicoDoItem.quantidade_m_m2 = UInt32.Parse( servicosDoItemTemp["quantidade_m_m2"].ToString() );
				servicoDoItem.valor = Double.Parse( servicosDoItemTemp["valor"].ToString() );
				servicoDoItem.servico = ServicosHandler.jsonToServicoEspecifico( servicosDoItemTemp["servico"], js );

				itensServicos.Add( servicoDoItem );
			}

			return itensServicos;
		}

		public static void formatarSaida( List<OrdemDeServico> ordensDeServico ) {
			for( int i = 0; i < ordensDeServico.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<OrdemDeServico>( ordensDeServico[i] );
			}
		}

		public static void formatarSaida( List<Item> itens ) {
			for( int i = 0; i < itens.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Item>( itens[i] );
			}
		}

		public static void formatarSaida( List<Status> statusList ) {
			for( int i = 0; i < statusList.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Status>( statusList[i] );
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
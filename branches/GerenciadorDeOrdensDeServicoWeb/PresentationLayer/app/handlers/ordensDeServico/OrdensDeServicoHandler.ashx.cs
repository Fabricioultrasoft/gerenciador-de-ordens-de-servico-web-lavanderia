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

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.ordensDeServico {
	/// <summary>
	/// Summary description for Handler1
	/// </summary>
	public class OrdensDeServicoHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
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
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.cadastrar( ref ordensDeServico );

			#region CONSTROI O JSON
			formatarSaida( ref ordensDeServico );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( " \"total\": " + ordensDeServico.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( " \"data\": [" );
			foreach( OrdemDeServico os in ordensDeServico ) {
				jsonResposta.Append( "{" );
				jsonResposta.Append( "}," );
			}
			if( ordensDeServico.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
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
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaida( ref ordensDeServico );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( " \"total\": " + qtdRegistros + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"message\": [\"Foram encontrados " + qtdRegistros + " registros\"]," );
			} else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( " \"data\": [" );
			foreach( OrdemDeServico os in ordensDeServico ) {
				jsonResposta.Append( "{" );
				jsonResposta.Append( "}," );
			}
			if( ordensDeServico.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String updateOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.atualizar( ref ordensDeServico );

			#region CONSTROI O JSON
			formatarSaida( ref ordensDeServico );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( " \"total\": " + ordensDeServico.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( " \"data\": [" );
			foreach( OrdemDeServico os in ordensDeServico ) {
				jsonResposta.Append( "{" );
				jsonResposta.Append( "}," );
			}
			if( ordensDeServico.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyOrdensDeServico( String records ) {
			List<OrdemDeServico> ordensDeServico = jsonToOrdensDeServico( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeOrdensDeServico.excluir( ordensDeServico );

			#region CONSTROI O JSON
			formatarSaida( ref ordensDeServico );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( " \"total\": " + ordensDeServico.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( " \"success\": true," );
				jsonResposta.AppendLine( " \"message\": [\"Dados excluidos com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( " \"data\": [] }" );
			#endregion

			return jsonResposta.ToString();
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
					ordem.status.codigo = UInt32.Parse(ordemTemp["codigoStatus"].ToString());
					ordem.status.nome = ordemTemp["nomeStatus"].ToString();
				} catch {}
				ordem.dataDeAbertura = DateTime.Parse( ordemTemp["dataDeAbertura"].ToString() );
				ordem.previsaoDeConclusao = DateTime.Parse( ordemTemp["previsaoDeConclusao"].ToString() );
				try { ordem.dataDeEncerramento = DateTime.Parse( ordemTemp["dataDeEncerramento"].ToString() ); } catch{}
				ordem.observacoes = ordemTemp["observacoes"].ToString();
				
				ordem.cliente.codigo = UInt32.Parse( ordemTemp["codigoCliente"].ToString() );
				ordem.cliente.nome = ordemTemp["nomeCliente"].ToString();

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


					item.itensServicos.AddRange( jsonToServicosDoItem( itensTemp["servicosDoItem"], js ) );
					
					ordem.itens.Add( item );
				}

				ordensDeServico.Add( ordem );
			}

			return ordensDeServico;
		}

		public static List<ItemServico> jsonToServicosDoItem( Object json, JavaScriptSerializer js ) {
			List<ItemServico> itensServicos = new List<ItemServico>();
			StringBuilder servicosJson = new StringBuilder();
			js.Serialize( json, servicosJson );

			List<Dictionary<String, Object>> list = js.Deserialize<List<Dictionary<String, Object>>>( servicosJson.ToString() );

			foreach( Dictionary<String, Object> servicosDoItemTemp in list ) {
				ItemServico servicoDoItem = new ItemServico();

				servicoDoItem.codigo = UInt32.Parse(servicosDoItemTemp["codigo"].ToString());
				servicoDoItem.quantidade_m_m2 = UInt32.Parse(servicosDoItemTemp["quantidade_m_m2"].ToString());
				servicoDoItem.valor = Double.Parse(servicosDoItemTemp["valor"].ToString());
				servicoDoItem.servico = ServicosHandler.jsonToServicoEspecifico(servicosDoItemTemp["servico"],js);

				itensServicos.Add( servicoDoItem );
			}

			return itensServicos;
		}

		public static void formatarSaida( ref List<OrdemDeServico> ordensDeServico ) {
			for( int i = 0; i < ordensDeServico.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<OrdemDeServico>( ordensDeServico[i] );
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
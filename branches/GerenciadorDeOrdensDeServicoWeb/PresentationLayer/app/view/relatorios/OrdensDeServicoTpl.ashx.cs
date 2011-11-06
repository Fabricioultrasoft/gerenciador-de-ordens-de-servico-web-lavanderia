using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.ordensDeServico;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.view.relatorios {
	/// <summary>
	/// Summary description for OrdensDeServicoTpl
	/// </summary>
	public class OrdensDeServicoTpl : IHttpHandler {

		private const String TITLE = "Relatório de Ordens de Serviço";

		public void ProcessRequest( HttpContext context ) {

			JavaScriptSerializer js = new JavaScriptSerializer();
			String colunas = context.Request.QueryString["colunas"];
			UInt32 start = 0;
			UInt32 limit = 0;
			long count = 0;
			List<Filter> filters;
			List<Sorter> sorters;

			#region DADOS DA REQUISICAO
			if( String.IsNullOrEmpty( colunas ) ) {
				context.Response.Write( "Nenhuma coluna do relat&oacute;rio foi selecionada!" );
				return;
			}

			UInt32.TryParse( context.Request.QueryString["start"], out start );
			UInt32.TryParse( context.Request.QueryString["limit"], out limit );

			// trata o problema de indice comecar em 0 no mysql
			if( start > 0 ) start--;

			// filtros
			if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
				filters = js.Deserialize<List<Filter>>( context.Request.QueryString["filter"] );
			} else {
				filters = new List<Filter>();
			}

			// ordenacao
			if( String.IsNullOrEmpty( context.Request.QueryString["sort"] ) == false ) {
				sorters = js.Deserialize<List<Sorter>>( context.Request.QueryString["sort"] );
			} else {
				sorters = new List<Sorter>();
			}
			#endregion

			StringBuilder sql = new StringBuilder();
			MySqlFilter filter = MySqlOrdensDeServicoDao.getFilter( filters );
			String sortClause = MySqlOrdensDeServicoDao.getSort( sorters );

			sql.AppendFormat( "SELECT {0} FROM tb_ordens_de_servico ", construirColunasSql( colunas ) );
			sql.AppendLine( "INNER JOIN tb_clientes ON tb_clientes.cod_cliente = tb_ordens_de_servico.cod_cliente " );
			sql.AppendLine( "INNER JOIN tb_status_os ON tb_status_os.cod_status_os = tb_ordens_de_servico.cod_status_os " );
			sql.AppendLine( filter.whereClause );
			sql.AppendLine( ( sortClause.Trim().Length > 0 ) ? sortClause : " ORDER BY num_os " );
			if( limit > 0 )
				sql.AppendLine( " LIMIT " + start + "," + limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			if( filter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( filter.parametersList.ToArray() );
			}

			MySqlDataReader reader = cmd.ExecuteReader();

			switch( context.Request.QueryString["reportView"] ) {
				case "txt":
					Compartilhado.gerarRelatorioTxt( TITLE, context, reader );
					break;
				case "xls":
					Compartilhado.gerarRelatorioExcel( TITLE, context, reader );
					break;
				case "pdf":
					Compartilhado.gerarRelatorioPdf( TITLE, context, reader );
					break;
			}

			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();

		}

		private String construirColunasSql( String colunasAlias ) {

			List<String> colunasSql = new List<String>();
			String[] cols = colunasAlias.Split( new char[] { ',' } );
			
			for( int i = 0; i < cols.Length; i++ ) {
				switch( cols[i] ) {
					case "numero":
						colunasSql.Add( "num_os AS 'Num.' " );
						break;
					case "codigoCliente":
						colunasSql.Add( "tb_clientes.cod_cliente AS 'Cod. Cliente' " );
						break;
					case "nomeCliente":
						colunasSql.Add( "tb_clientes.nom_cliente AS 'Cliente' " );
						break;
					case "valorOriginal":
						colunasSql.Add( "val_original AS 'Val. Orig.' " );
						break;
					case "valorFinal":
						colunasSql.Add( "val_final AS 'Val. Final' " );
						break;
					case "status":
						colunasSql.Add( "tb_status_os.nom_status_os AS Status" );
						break;
					case "dataAbertura":
						colunasSql.Add( "dat_abertura AS Abertura" );
						break;
					case "previsaoConclusao":
						colunasSql.Add( "dat_prev_conclusao AS 'Prev. Concl.' " );
						break;
					case "dataFechamento":
						colunasSql.Add( "dat_fechamento AS Fechamento " );
						break;
					case "observacoes":
						colunasSql.Add( "tb_ordens_de_servico.txt_observacoes AS 'Obs.' " );
						break;
					case "tapetes":
						colunasSql.Add( "( SELECT GROUP_CONCAT(nom_tapete) FROM tb_tapetes A INNER JOIN tb_itens_os B ON B.cod_tapete = A.cod_tapete WHERE B.cod_ordem_de_servico = tb_ordens_de_servico.cod_ordem_de_servico ) AS Tapetes " );
						break;
					case "itens":
						colunasSql.Add(
							"( SELECT GROUP_CONCAT(CONCAT( 'Tap:', nom_tapete, ', Compr:', CAST(flt_comprimento AS CHAR), ', Larg:', CAST(flt_largura AS CHAR), ', Area:', CAST(dbl_area AS CHAR), ', R$', CAST(val_item AS CHAR), "
							+ " ', Serv:',( "
							+ "     SELECT GROUP_CONCAT(nom_servico) FROM tb_servicos C "
							+ "     INNER JOIN tb_itens_servicos D ON D.cod_servico = C.cod_servico "
							+ "     WHERE D.cod_item_os = A.cod_item_os "
							+ " ))) "
							+ " FROM tb_itens_os A "
							+ " INNER JOIN tb_tapetes B ON B.cod_tapete = A.cod_tapete "
							+ " WHERE A.cod_ordem_de_servico = tb_ordens_de_servico.cod_ordem_de_servico) AS Itens"
						);
						break;
					}
			}

			return String.Join( ",", colunasSql.ToArray() );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
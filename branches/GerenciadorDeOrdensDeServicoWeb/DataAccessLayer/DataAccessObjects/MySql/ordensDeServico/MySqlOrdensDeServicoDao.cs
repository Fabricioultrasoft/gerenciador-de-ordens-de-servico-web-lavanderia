using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.ordensDeServico {
	public class MySqlOrdensDeServicoDao {

		#region INSERT
		private static const String INSERT_OS =
			"INSERT INTO tb_ordens_de_servico ("
			+ "	cod_cliente "
			+ "	,cod_usuario "
			+ "	,cod_status_os "
			+ "	,num_os "
			+ "	,val_original "
			+ "	,val_final "
			+ "	,dat_abertura "
			+ "	,dat_prev_entrega "
			+ "	,txt_observacoes "
			+ "	,dat_atualizacao "
			+ ") VALUES ( "
			+ "	@codCliente "
			+ "	,@codUsuario "
			+ "	,@codStatusOS "
			+ "	,@numOS "
			+ "	,@valOriginal "
			+ "	,@valFinal "
			+ "	,@datAbertura "
			+ "	,@datPrevEntrega "
			+ "	,@txtObservacoes "
			+ "	,NOW()); "
			+ "SELECT LAST_INSERT_ID() ";

		private static const String INSERT_ITENS =
			"INSERT INTO tb_itens_os ( "
			+ "	cod_tapete "
			+ "	,cod_ordem_de_servico "
			+ "	,flt_comprimento "
			+ "	,flt_largura "
			+ "	,dbl_area "
			+ "	,txt_observacoes "
			+ ") VALUES ( "
			+ "	@codTapete "
			+ "	,@codOrdemDeServico "
			+ "	,@fltComprimento "
			+ "	,@fltLargura "
			+ "	,@dblArea "
			+ "	,@txtObservacoes); "
			+ "SELECT LAST_INSERT_ID() ";

		private static const String INSERT_ITENS_SERVICOS = 
			"INSERT INTO tb_itens_servicos ( "
			+ "	cod_item_os "
			+ "	,cod_servico "
			+ "	,qtd_m_m2 "
			+ "	,val_item_servico "
			+ ") VALUES ( "
			+ "	@codItemOS "
			+ "	,@codServico "
			+ "	,@qtdMm2 "
			+ "	,@valItemServico); "
			+ "SELECT LAST_INSERT_ID() ";
		#endregion

		#region SELECT
		private static const String SELECT_OS =
			"SELECT "
			+ "	cod_ordem_de_servico "
			+ "	,cod_cliente "
			+ "	,cod_usuario "
			+ "	,tb_status_os.cod_status_os "
			+ "	,tb_status_os.nom_status_os "
			+ "	,num_os "
			+ "	,val_original "
			+ "	,val_final "
			+ "	,dat_abertura "
			+ "	,dat_prev_entrega "
			+ "	,data_fechamento "
			+ "	,txt_observacoes "
			+ "	,dat_cadastro "
			+ "	,dat_atualizacao "
			+ "FROM tb_ordens_de_servico "
			+ "INNER JOIN tb_status_os ON tb_status_os.cod_status_os = tb_ordens_de_servico.cod_status_os ";

		private static const String SELECT_ITEM =
			"SELECT "
			+ "	cod_item_os "
			+ "	,cod_tapete "
			+ "	,cod_ordem_de_servico "
			+ "	,flt_comprimento "
			+ "	,flt_largura "
			+ "	,dbl_area "
			+ "	,txt_observacoes "
			+ "FROM tb_itens_os ";

		private static const String SELECT_ITENS_SERVICOS =
			"SELECT "
			+ "	cod_item_servico "
			+ "	,cod_item_os "
			+ "	,cod_servico "
			+ "	,qtd_m_m2 "
			+ "	,val_item_servico "
			+ "FROM tb_itens_servicos ";
		#endregion


		public static long count() {
			long count = 0;

			String sql = " SELECT COUNT(cod_ordem_de_servico) "
					+ " FROM tb_ordens_de_servico "; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );

			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static long count( List<Filter> filters ) {
			long count = 0;
			MySqlFilter mySqlFilter = getFilter( filters );

			String sql = " SELECT COUNT(cod_ordem_de_servico) "
					+ " FROM tb_ordens_de_servico "+ mySqlFilter.whereClause; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			if( mySqlFilter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( mySqlFilter.parametersList.ToArray() );
			}
			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static List<Erro> insert( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( OrdemDeServico os in ordensDeServico ) {

				MySqlCommand cmd = new MySqlCommand( INSERT_OS, conn );
				cmd.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = os.cliente.codigo;
				cmd.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = os.usuario.codigo;
				cmd.Parameters.Add( "@codStatusOS", MySqlDbType.UInt32 ).Value = os.status.codigo;
				cmd.Parameters.Add( "@numOS", MySqlDbType.UInt32 ).Value = os.numero;
				cmd.Parameters.Add( "@valOriginal", MySqlDbType.Double ).Value = os.valorOriginal;
				cmd.Parameters.Add( "@valFinal", MySqlDbType.Double ).Value = os.valorFinal;
				cmd.Parameters.Add( "@datAbertura", MySqlDbType.DateTime ).Value = os.dataDeAbertura;
				cmd.Parameters.Add( "@datPrevEntrega", MySqlDbType.DateTime ).Value = os.previsaoDeConclusao;
				cmd.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = os.observacoes;
				os.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				if( os.codigo <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível inserir a Ordem de Servi&ccedil;o: " + os.numero, "Tente inseri-la novamente" ) );
					continue;
				}

				foreach( Item item in os.itens ) {

					MySqlCommand cmd2 = new MySqlCommand( INSERT_ITENS, conn );
					cmd2.Parameters.Add( "@codTapete", MySqlDbType.UInt32 ).Value = item.tapete.codigo;
					cmd2.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = os.codigo;
					cmd2.Parameters.Add( "@fltComprimento", MySqlDbType.Float ).Value = item.comprimento;
					cmd2.Parameters.Add( "@fltLargura", MySqlDbType.Float ).Value = item.largura;
					cmd2.Parameters.Add( "@dblArea", MySqlDbType.Double ).Value = item.area;
					cmd2.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = item.observacoes;
					item.codigo = UInt32.Parse( cmd2.ExecuteScalar().ToString() );
					cmd2.Dispose();

					if( item.codigo <= 0 ) {
						erros.Add( new Erro( 0, "Não foi possível inserir o item: " + item.tapete.nome, "Tente inseri-lo novamente" ) );
						continue;
					}


					foreach( ServicoDoItem servItem in item.servicosDoItem ) {

						MySqlCommand cmd3 = new MySqlCommand( INSERT_ITENS_SERVICOS, conn );
						cmd3.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = item.codigo;
						cmd3.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servItem.servico.codigo;
						cmd3.Parameters.Add( "@qtdMm2", MySqlDbType.Double ).Value = servItem.quantidade_m_m2;
						cmd3.Parameters.Add( "@valItemServico", MySqlDbType.Double ).Value = servItem.valor;
						servItem.codigo = UInt32.Parse( cmd3.ExecuteScalar().ToString() );
						cmd3.Dispose();

						if( servItem.codigo <= 0 ) {
							erros.Add( new Erro( 0, "Não foi possível inserir o servi&ccedil;o: " + servItem.servico.nome + " do item: " + item.tapete.nome, "Tente inseri-lo novamente" ) );
						}
					}
				}
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static OrdemDeServico select( List<Filter> filters ) {
			OrdemDeServico ordemDeServico = new OrdemDeServico();

			MySqlFilter mySqlFilter = getFilter( filters );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlConnection connAux = MySqlConnectionWizard.getConnection();

			conn.Open();
			connAux.Open();

			MySqlCommand cmd = new MySqlCommand( SELECT_OS + mySqlFilter.whereClause, conn );
			if( mySqlFilter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( mySqlFilter.parametersList.ToArray() );
			}

			MySqlDataReader reader = cmd.ExecuteReader();
			if( reader.Read() ) {

				ordemDeServico.codigo = reader.GetUInt32( "cod_ordem_de_servico" );
				ordemDeServico.status.codigo = reader.GetUInt32( "cod_status_os" );
				ordemDeServico.status.nome = reader.GetString( "nom_status_os" );
				ordemDeServico.numero = reader.GetUInt32( "num_os" );
				ordemDeServico.valorOriginal = reader.GetDouble( "val_original" );
				ordemDeServico.valorFinal = reader.GetDouble( "val_final" );
				ordemDeServico.dataDeAbertura = reader.GetDateTime( "dat_abertura" );
				ordemDeServico.previsaoDeConclusao = reader.GetDateTime( "dat_prev_entrega" );
				try { ordemDeServico.dataDeFechamento = reader.GetDateTime( "data_fechamento" ); } catch { }
				try { ordemDeServico.observacoes = reader.GetString( "txt_observacoes" ); } catch { }

				ordemDeServico.dataDeCadastro = reader.GetDateTime( "dat_cadastro" );
				ordemDeServico.dataDeAtualizacao = reader.GetDateTime( "dat_atualizacao" );

				Cliente cliAux = ordemDeServico.cliente;
				MySqlClientesDao.fillCliente( reader.GetUInt32( "cod_cliente" ), ref cliAux, connAux );
				Usuario usuAux = ordemDeServico.usuario;
				MySqlUsuariosDao.fillUsuario( reader.GetUInt32( "cod_usuario" ), ref usuAux, connAux );
				List<Item> itensAux = ordemDeServico.itens;
				MySqlOrdensDeServicoDao.fillItens( ordemDeServico.codigo, ref itensAux, connAux );
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();
			connAux.Close(); connAux.Dispose();

			return ordemDeServico;
		}

		public static List<OrdemDeServico> select( UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<OrdemDeServico> ordensDeServico = new List<OrdemDeServico>();
			StringBuilder sql = new StringBuilder();

			return ordensDeServico;
		}

		public static List<Erro> update( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			return erros;
		}

		public static List<Erro> delete( List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			// so podem ser excluidas as ordens de servico que nao foram canceladas ou finalizadas
			String sql = "DELETE FROM tb_ordens_de_servico WHERE cod_ordem_de_servico = @codOrdemDeServico AND cod_status_os = 1 ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( OrdemDeServico ordem in ordensDeServico ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = ordem.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir a Ordem de Servi&ccedil;o: " + ordem.numero, "Tente excluí-la novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Status> selectStatus() {
			List<Status> statusList = new List<Status>();
			StringBuilder sql = new StringBuilder();

			sql.Append( "SELECT " );
			sql.Append( "	cod_status_os " );
			sql.Append( "	,nom_status_os " );
			sql.Append( "FROM tb_status_os " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			conn.Open();
			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				statusList.Add( new Status( reader.GetUInt32( 0 ), reader.GetString( 1 ) ) );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return statusList;
		}

		public static void fillItens( UInt32 codigoOrdemDeServico, ref List<Item> itens, MySqlConnection conn ) {

			MySqlCommand cmd = new MySqlCommand( SELECT_ITEM + " WHERE cod_ordem_de_servico = @codOrdemDeServico", conn );
			cmd.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = codigoOrdemDeServico;
			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				Item item = new Item();
				item.codigo = reader.GetUInt32("cod_item_os");
				item.codigoOrdemDeServico = codigoOrdemDeServico;
				item.tapete.codigo = reader.GetUInt32("cod_tapete");
				item.comprimento = reader.GetFloat("flt_comprimento");
				item.comprimento = reader.GetFloat("flt_largura");
				try { item.observacoes = reader.GetString("txt_observacoes"); } catch{}
				
				itens.Add(item);
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();

			foreach(Item item in itens) {
				Tapete tapAux = item.tapete;
				MySqlTapetesDao.fillTapete( item.tapete.codigo, ref tapAux, conn );

				List<ServicoDoItem> servicosDoItemAux = item.servicosDoItem;
				fillServicosDoItem( item.codigo, ref servicosDoItemAux, conn);
			}

		}

		public static void fillServicosDoItem( UInt32 codigoItem, ref List<ServicoDoItem> servicosDoItem, MySqlConnection conn ) {

			MySqlCommand cmd = new MySqlCommand( SELECT_ITENS_SERVICOS + " WHERE cod_item_os = @codItemOS", conn );
			cmd.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = codigoItem;
			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				ServicoDoItem servicoDoItem = new ServicoDoItem();
			
				servicoDoItem.codigo = reader.GetUInt32("cod_item_servico");
				servicoDoItem.codigoItem = codigoItem;
				servicoDoItem.servico.codigo = reader.GetUInt32("cod_servico");
				servicoDoItem.quantidade_m_m2 = reader.GetDouble("qtd_m_m2");
				servicoDoItem.valor = reader.GetDouble("val_item_servico");

				servicosDoItem.Add(servicoDoItem);
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();

			foreach(ServicoDoItem servicoDoItem in servicosDoItem) {
				Servico servAux = servicoDoItem.servico;
				MySqlServicosDao.fillServico( servicoDoItem.servico.codigo, ref servAux, conn );
			}
		}

		private static MySqlFilter getFilter( List<Filter> filters ) {

			MySqlFilter mySqlfilter = new MySqlFilter();

			return mySqlfilter;
		}

		private static String getSort( List<Sorter> sorters ) {

			return "";
		}
	}
}
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

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.ordensDeServico {
	public class MySqlOrdensDeServicoDao {

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
			StringBuilder sql = new StringBuilder();
			StringBuilder sqlItem = new StringBuilder();
			StringBuilder sqlServItem = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_ordens_de_servico (" );
			sql.AppendLine( "	cod_cliente" );
			sql.AppendLine( "	,cod_usuario" );
			sql.AppendLine( "	,cod_status_os" );
			sql.AppendLine( "	,num_os" );
			sql.AppendLine( "	,val_original" );
			sql.AppendLine( "	,val_final" );
			sql.AppendLine( "	,dat_abertura" );
			sql.AppendLine( "	,dat_prev_entrega" );
			sql.AppendLine( "	,data_fechamento" );
			sql.AppendLine( "	,txt_observacoes" );
			sql.AppendLine( "	,dat_atualizacao" );
			sql.AppendLine( ") VALUES (" );
			sql.AppendLine( "	@codCliente" );
			sql.AppendLine( "	,@codUsuario" );
			sql.AppendLine( "	,@codStatusOS" );
			sql.AppendLine( "	,@numOs" );
			sql.AppendLine( "	,@valOriginal" );
			sql.AppendLine( "	,@valFinal" );
			sql.AppendLine( "	,@datAbertura" );
			sql.AppendLine( "	,@datPrevEntrega" );
			sql.AppendLine( "	,@dataFechamento" );
			sql.AppendLine( "	,@txtObservacoes" );
			sql.AppendLine( "	,NOW()); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID() " );


			sqlItem.AppendLine( "INSERT INTO tb_itens_os (" );
			sqlItem.AppendLine( "	cod_tapete" );
			sqlItem.AppendLine( "	,cod_ordem_de_servico" );
			sqlItem.AppendLine( "	,flt_comprimento" );
			sqlItem.AppendLine( "	,flt_largura" );
			sqlItem.AppendLine( "	,dbl_area" );
			sqlItem.AppendLine( "	,txt_observacoes" );
			sqlItem.AppendLine( ") VALUES (" );
			sqlItem.AppendLine( "	@codTapete" );
			sqlItem.AppendLine( "	,@codOrdemDeServico" );
			sqlItem.AppendLine( "	,@fltComprimento" );
			sqlItem.AppendLine( "	,@fltLargura" );
			sqlItem.AppendLine( "	,@dblArea" );
			sqlItem.AppendLine( "	,@txtObservacoes);" );
			sqlItem.AppendLine( "SELECT LAST_INSERT_ID()" );


			sqlServItem.AppendLine( "INSERT INTO tb_itens_servicos (" );
			sqlServItem.AppendLine( "	cod_item_os" );
			sqlServItem.AppendLine( "	,cod_servico" );
			sqlServItem.AppendLine( "	,qtd_m_m2" );
			sqlServItem.AppendLine( "	,val_item_servico" );
			sqlServItem.AppendLine( ") VALUES (" );
			sqlServItem.AppendLine( "	@codItemOS" );
			sqlServItem.AppendLine( "	,@codServico" );
			sqlServItem.AppendLine( "	,@qtdMm2" );
			sqlServItem.AppendLine( "	,@valItemServico);" );
			sqlServItem.AppendLine( "SELECT LAST_INSERT_ID()" );

			return erros;
		}

		public static OrdemDeServico select( List<Filter> filters ) {
			OrdemDeServico ordemDeServico = new OrdemDeServico();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT" );
			sql.AppendLine( "	cod_ordem_de_servico" );
			sql.AppendLine( "	,cod_cliente" );
			sql.AppendLine( "	,cod_usuario" );
			sql.AppendLine( "	,cod_status_os" );
			sql.AppendLine( "	,num_os" );
			sql.AppendLine( "	,val_original" );
			sql.AppendLine( "	,val_final" );
			sql.AppendLine( "	,dat_abertura" );
			sql.AppendLine( "	,dat_prev_entrega" );
			sql.AppendLine( "	,data_fechamento" );
			sql.AppendLine( "	,txt_observacoes" );
			sql.AppendLine( "	,dat_cadastro" );
			sql.AppendLine( "	,dat_atualizacao" );
			sql.AppendLine( "FROM tb_ordens_de_servico" );

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

		private static MySqlFilter getFilter( List<Filter> filters ) {

			MySqlFilter mySqlfilter = new MySqlFilter();

			return mySqlfilter;
		}

		private static String getSort( List<Sorter> sorters ) {

			return "";
		}
	}
}
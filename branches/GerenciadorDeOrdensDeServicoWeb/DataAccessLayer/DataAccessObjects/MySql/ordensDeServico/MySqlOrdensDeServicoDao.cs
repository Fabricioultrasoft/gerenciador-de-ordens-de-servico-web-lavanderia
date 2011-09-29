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
			
			return erros;
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
			String sql = "DELETE FROM tb_ordens_de_servico WHERE cod_ordem_de_servico = @codOrdemDeServico ";

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
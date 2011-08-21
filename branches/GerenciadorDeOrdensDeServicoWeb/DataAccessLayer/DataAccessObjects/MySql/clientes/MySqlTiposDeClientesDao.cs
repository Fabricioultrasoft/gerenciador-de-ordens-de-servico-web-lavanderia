using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using System.Text;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes {
	public class MySqlTiposDeClientesDao {

		public static long countTiposDeClientes() {
			String sql = "SELECT COUNT(cod_tipo_cliente) FROM tb_tipos_clientes";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<TipoDeCliente> getTiposDeClientes() {
			return getTiposDeClientes( 0, 0, null );
		}

		public static List<TipoDeCliente> getTiposDeClientes( UInt32 start, UInt32 limit, bool? ativo ) {
			List<TipoDeCliente> tiposDeClientes = new List<TipoDeCliente>();

			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_tipo_cliente " );
			sql.AppendLine( "	,nom_tipo_cliente " );
			sql.AppendLine( "	,flg_ativo " );
			sql.AppendLine( "FROM tb_tipos_clientes " );
			if( ativo != null ) 
				sql.AppendFormat( "WHERE flg_ativo = {0} ", ( (bool) ativo ) ? 1 : 0 );
			sql.AppendLine( "ORDER BY nom_tipo_cliente " );
			if( limit > 0 ) 
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				tiposDeClientes.Add( new TipoDeCliente( reader.GetUInt32( 0 ), reader.GetString( 1 ), reader.GetBoolean(2) ) );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return tiposDeClientes;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

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

		public static List<Erro> inserirListaDeTiposDeClientes( ref List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_tipos_clientes(nom_tipo_cliente,flg_ativo) VALUES(@nomTipoCliente, @ativo); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nomTipoCliente", MySqlDbType.VarChar ).Value = tipoDeCliente.nome;
				cmd.Parameters.Add( "@ativo", MySqlDbType.Bit ).Value = (tipoDeCliente.ativo) ? 1 : 0;
				
				tipoDeCliente.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o tipo de cliente foi inserido ou nao
				if( tipoDeCliente.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o tipo de cliente: " + tipoDeCliente.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDeTiposDeClientes( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_tipos_clientes SET nom_tipo_cliente=@nomTipoCliente, flg_ativo=@ativo WHERE cod_tipo_cliente = @codTipoCliente ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nomTipoCliente", MySqlDbType.VarChar ).Value = tipoDeCliente.nome;
				cmd.Parameters.Add( "@ativo", MySqlDbType.Bit ).Value = ( tipoDeCliente.ativo ) ? 1 : 0;
				cmd.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = tipoDeCliente.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o tipo de cliente: " + tipoDeCliente.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluirListaDeTiposDeClientes( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_tipos_clientes WHERE cod_tipo_cliente = @codTipoCliente ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeCliente tipoDeCliente in tiposDeClientes ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = tipoDeCliente.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o tipo de cliente: " + tipoDeCliente.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
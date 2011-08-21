using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using System.Text;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos {
	public class MySqlTiposDeLogradourosDao {

		public static List<TipoDeLogradouro> getTiposDeLogradouros() {
			return getTiposDeLogradouros( 0, 0 );
		}

		public static List<TipoDeLogradouro> getTiposDeLogradouros( UInt32 start, UInt32 limit ) {
			List<TipoDeLogradouro> tiposDeLogradouros = new List<TipoDeLogradouro>();

			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_tipo_logradouro " );
			sql.AppendLine( "	,nom_tipo_logradouro " );
			sql.AppendLine( "FROM tb_tipos_logradouros " );
			sql.AppendLine( "ORDER BY nom_tipo_logradouro " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				tiposDeLogradouros.Add( new TipoDeLogradouro( reader.GetUInt32( 0 ), reader.GetString( 1 ) ) );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return tiposDeLogradouros;
		}

		public static List<Erro> inserirListaDeTiposDeLogradouros( ref List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_tipos_logradouros(nom_tipo_logradouro) VALUES(@nom_tipo_logradouro); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_tipo_logradouro", MySqlDbType.VarChar ).Value = tipoDeLogradouro.nome;
				tipoDeLogradouro.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o pais foi inserido ou nao
				if( tipoDeLogradouro.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o tipo de Logradouro: " + tipoDeLogradouro.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDeTiposDeLogradouros( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_tipos_logradouros SET nom_tipo_logradouro = @nom_tipo_logradouro WHERE cod_tipo_logradouro = @cod_tipo_logradouro ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nom_tipo_logradouro", MySqlDbType.VarChar ).Value = tipoDeLogradouro.nome;
				cmd.Parameters.Add( "@cod_tipo_logradouro", MySqlDbType.UInt32 ).Value = tipoDeLogradouro.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o tipo de Logradouro: " + tipoDeLogradouro.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}


		public static List<Erro> excluirListaDeTiposDeLogradouros( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_tipos_logradouros WHERE cod_tipo_logradouro = @cod_tipo_logradouro ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( TipoDeLogradouro tipoDeLogradouro in tiposDeLogradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_tipo_logradouro", MySqlDbType.UInt32 ).Value = tipoDeLogradouro.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o tipos de Logradouro: " + tipoDeLogradouro.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
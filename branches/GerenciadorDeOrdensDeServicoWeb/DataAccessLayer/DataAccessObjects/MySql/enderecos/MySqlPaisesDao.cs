using System;
using System.Text;
using System.Collections.Generic;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos {
	public class MySqlPaisesDao {

		public static long countPaises() {
			String sql = "SELECT COUNT(cod_pais) FROM tb_paises";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<Pais> getPaises() {
			return getPaises( 0, 0 );
		}

		public static List<Pais> getPaises( UInt32 start, UInt32 limit ) {
			List<Pais> paises = new List<Pais>();

			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_pais " );
			sql.AppendLine( "	,nom_pais " );
			sql.AppendLine( "FROM tb_paises " );
			sql.AppendLine( "ORDER BY nom_pais " );
			if( limit > 0 )
				sql.AppendFormat("LIMIT {0},{1}",start,limit);

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				paises.Add( new Pais( reader.GetUInt32( 0 ), reader.GetString( 1 ) ) );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return paises;
		}

		public static bool inserirPais(ref Pais pais) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("INSERT INTO tb_paises(nom_pais) VALUES(@nom_pais); ");
			sql.AppendLine("SELECT LAST_INSERT_ID()");

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@nom_pais", MySqlDbType.UInt32 ).Value = pais.nome;

			// abre a conexao
			conn.Open();
			pais.codigo = UInt32.Parse(cmd.ExecuteScalar().ToString());
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			if( pais.codigo > 0 ) 
				return true;
			else 
				return false;
		}

		public static List<Erro> inserirListaDePaises( ref List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_paises(nom_pais) VALUES(@nom_pais); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach(Pais pais in paises) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_pais", MySqlDbType.VarChar ).Value = pais.nome;
				pais.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();
				
				// registra se o pais foi inserido ou nao
				if( pais.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o país: " + pais.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDePaises( List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_paises SET nom_pais = @nom_pais WHERE cod_pais = @cod_pais ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Pais pais in paises ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nom_pais", MySqlDbType.VarChar ).Value = pais.nome;
				cmd.Parameters.Add( "@cod_pais", MySqlDbType.UInt32 ).Value = pais.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o país: " + pais.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}


		public static List<Erro> excluirListaDePaises( List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_paises WHERE cod_pais = @cod_pais ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Pais pais in paises ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_pais", MySqlDbType.UInt32 ).Value = pais.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o país: " + pais.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
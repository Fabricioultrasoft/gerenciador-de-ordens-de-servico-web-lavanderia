using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos {
	public class MySqlEstadosDao {

		public static long countEstados() {
			String sql = "SELECT COUNT(cod_estado) FROM tb_estados";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand(sql,conn);
			conn.Open();
			
			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<Estado> getEstados() {
			return getEstados( 0, 0, 0 );
		}

		public static List<Estado> getEstados( UInt32 start, UInt32 limit, UInt32 codigoPais ) {
			List<Estado> estados = new List<Estado>();

			StringBuilder sql = new StringBuilder();
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_estado " ); // 0
			sql.AppendLine( "	,nom_estado " ); // 1
			sql.AppendLine( "	,B.cod_pais " ); // 2
			sql.AppendLine( "	,B.nom_pais " ); // 3
			sql.AppendLine( "FROM tb_estados A " );
			sql.AppendLine( "INNER JOIN tb_paises B ON B.cod_pais = A.cod_pais " );
			if( codigoPais > 0 ) {
				sql.AppendFormat( "WHERE A.cod_pais = @cod_pais " );
				cmd.Parameters.Add( "@cod_pais", MySqlDbType.UInt32 ).Value = codigoPais;
			}
			sql.AppendLine( "ORDER BY nom_estado " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );


			cmd.Connection = conn;
			cmd.CommandText = sql.ToString();
			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Estado estado = new Estado( reader.GetUInt32( 0 ), reader.GetString( 1 ) );
				estado.pais.codigo = reader.GetUInt32( 2 );
				estado.pais.nome = reader.GetString( 3 );
				estados.Add( estado );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return estados;
		}

		public static List<Erro> inserirListaDeEstados( ref List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_estados(nom_estado,cod_pais) VALUES(@nom_estado,@cod_pais); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Estado estado in estados ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_estado", MySqlDbType.VarChar ).Value = estado.nome;
				cmd.Parameters.Add( "@cod_pais", MySqlDbType.UInt32 ).Value = estado.pais.codigo;
				estado.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o pais foi inserido ou nao
				if( estado.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o estado: " + estado.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDeEstados( List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_estados SET nom_estado = @nom_estado, cod_pais = @cod_pais WHERE cod_estado = @cod_estado ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Estado estado in estados ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nom_estado", MySqlDbType.VarChar ).Value = estado.nome;
				cmd.Parameters.Add( "@cod_estado", MySqlDbType.UInt32 ).Value = estado.codigo;
				cmd.Parameters.Add( "@cod_pais", MySqlDbType.UInt32 ).Value = estado.pais.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o estado: " + estado.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluirListaDeEstados( List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_estados WHERE cod_estado = @cod_estado ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Estado estado in estados ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_estado", MySqlDbType.UInt32 ).Value = estado.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o estado: " + estado.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
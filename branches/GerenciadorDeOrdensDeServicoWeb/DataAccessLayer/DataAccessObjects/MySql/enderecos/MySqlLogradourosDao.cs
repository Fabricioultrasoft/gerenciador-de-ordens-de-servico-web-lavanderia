using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos {
	public class MySqlLogradourosDao {

		public static long count() {
			String sql = "SELECT COUNT(cod_logradouro) FROM tb_logradouros";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static Logradouro getLogradouro( UInt32 codLogradouro ) {
			Logradouro logradouro = new Logradouro();
			preencherLogradouro( codLogradouro, logradouro );
			return logradouro;
		}

		public static List<Logradouro> getLogradouros() {
			return getLogradouros( 0, 0, 0 );
		}

		public static List<Logradouro> getLogradouros( UInt32 start, UInt32 limit, UInt32 codigoBairro ) {
			List<Logradouro> logradouros = new List<Logradouro>();

			StringBuilder sql = new StringBuilder();
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_logradouro " ); // 0
			sql.AppendLine( "	,nom_logradouro " ); // 1
			sql.AppendLine( "	,txt_cep " ); // 2
			sql.AppendLine( "	,B.cod_bairro " ); // 3
			sql.AppendLine( "	,B.nom_bairro " ); // 4
			sql.AppendLine( "	,C.cod_cidade " ); // 5
			sql.AppendLine( "	,C.nom_cidade " ); // 6
			sql.AppendLine( "	,D.cod_estado " ); // 7
			sql.AppendLine( "	,D.nom_estado " ); // 8
			sql.AppendLine( "	,E.cod_pais " ); // 9
			sql.AppendLine( "	,E.nom_pais " ); // 10
			sql.AppendLine( "	,F.cod_tipo_logradouro " ); // 11
			sql.AppendLine( "	,F.nom_tipo_logradouro " ); // 12
			sql.AppendLine( "FROM tb_logradouros A " );
			sql.AppendLine( "INNER JOIN tb_bairros B ON B.cod_bairro = A.cod_bairro " );
			sql.AppendLine( "INNER JOIN tb_cidades C ON C.cod_cidade = B.cod_cidade " );
			sql.AppendLine( "INNER JOIN tb_estados D ON D.cod_estado = C.cod_estado " );
			sql.AppendLine( "INNER JOIN tb_paises E ON E.cod_pais = D.cod_pais " );
			sql.AppendLine( "INNER JOIN tb_tipos_logradouros F ON F.cod_tipo_logradouro = A.cod_tipo_logradouro " );
			if( codigoBairro > 0 ) {
				sql.AppendFormat( "WHERE B.cod_bairro = @cod_bairro " );
				cmd.Parameters.Add( "@cod_bairro", MySqlDbType.UInt32 ).Value = codigoBairro;
			}
			sql.AppendLine( "ORDER BY nom_logradouro " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			cmd.Connection = conn;
			cmd.CommandText = sql.ToString();
			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Logradouro logradouro = new Logradouro( reader.GetUInt32( 0 ), reader.GetString( 1 ), reader.GetString( 2 ) );
				logradouro.bairro.codigo = reader.GetUInt32( 3 );
				logradouro.bairro.nome = reader.GetString( 4 );
				logradouro.bairro.cidade.codigo = reader.GetUInt32( 5 );
				logradouro.bairro.cidade.nome = reader.GetString( 6 );
				logradouro.bairro.cidade.estado.codigo = reader.GetUInt32( 7 );
				logradouro.bairro.cidade.estado.nome = reader.GetString( 8 );
				logradouro.bairro.cidade.estado.pais.codigo = reader.GetUInt32( 9 );
				logradouro.bairro.cidade.estado.pais.nome = reader.GetString( 10 );
				logradouro.tipoDeLogradouro.codigo = reader.GetUInt32( 11 );
				logradouro.tipoDeLogradouro.nome = reader.GetString( 12 );
				logradouros.Add( logradouro );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return logradouros;
		}

		public static void preencherLogradouro( UInt32 codLogradouro, Logradouro logradouro ) {
			
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_logradouro " ); // 0
			sql.AppendLine( "	,nom_logradouro " ); // 1
			sql.AppendLine( "	,txt_cep " ); // 2
			sql.AppendLine( "	,B.cod_bairro " ); // 3
			sql.AppendLine( "	,B.nom_bairro " ); // 4
			sql.AppendLine( "	,C.cod_cidade " ); // 5
			sql.AppendLine( "	,C.nom_cidade " ); // 6
			sql.AppendLine( "	,D.cod_estado " ); // 7
			sql.AppendLine( "	,D.nom_estado " ); // 8
			sql.AppendLine( "	,E.cod_pais " ); // 9
			sql.AppendLine( "	,E.nom_pais " ); // 10
			sql.AppendLine( "	,F.cod_tipo_logradouro " ); // 11
			sql.AppendLine( "	,F.nom_tipo_logradouro " ); // 12
			sql.AppendLine( "FROM tb_logradouros A " );
			sql.AppendLine( "INNER JOIN tb_bairros B ON B.cod_bairro = A.cod_bairro " );
			sql.AppendLine( "INNER JOIN tb_cidades C ON C.cod_cidade = B.cod_cidade " );
			sql.AppendLine( "INNER JOIN tb_estados D ON D.cod_estado = C.cod_estado " );
			sql.AppendLine( "INNER JOIN tb_paises E ON E.cod_pais = D.cod_pais " );
			sql.AppendLine( "INNER JOIN tb_tipos_logradouros F ON F.cod_tipo_logradouro = A.cod_tipo_logradouro " );
			sql.AppendFormat( "WHERE A.cod_logradouro = @codLogradouro " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@codLogradouro", MySqlDbType.UInt32 ).Value = codLogradouro;
			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();

			if( reader.Read() ) {
				logradouro.codigo = reader.GetUInt32( 0 );
				logradouro.nome = reader.GetString( 1 );
				logradouro.cep = reader.GetString( 2 );
				logradouro.bairro.codigo = reader.GetUInt32( 3 );
				logradouro.bairro.nome = reader.GetString( 4 );
				logradouro.bairro.cidade.codigo = reader.GetUInt32( 5 );
				logradouro.bairro.cidade.nome = reader.GetString( 6 );
				logradouro.bairro.cidade.estado.codigo = reader.GetUInt32( 7 );
				logradouro.bairro.cidade.estado.nome = reader.GetString( 8 );
				logradouro.bairro.cidade.estado.pais.codigo = reader.GetUInt32( 9 );
				logradouro.bairro.cidade.estado.pais.nome = reader.GetString( 10 );
				logradouro.tipoDeLogradouro.codigo = reader.GetUInt32( 11 );
				logradouro.tipoDeLogradouro.nome = reader.GetString( 12 );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();
		}

		public static List<Erro> inserir( ref List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_logradouros(nom_logradouro, txt_cep, cod_tipo_logradouro, cod_bairro) ");
			sql.AppendLine( "VALUES(@nom_logradouro, @txt_cep, @cod_tipo_logradouro, @cod_bairro); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Logradouro logradouro in logradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_logradouro", MySqlDbType.VarChar ).Value = logradouro.nome;
				cmd.Parameters.Add( "@txt_cep", MySqlDbType.VarChar ).Value = logradouro.cep;
				cmd.Parameters.Add( "@cod_tipo_logradouro", MySqlDbType.UInt32 ).Value = logradouro.tipoDeLogradouro.codigo;
				cmd.Parameters.Add( "@cod_bairro", MySqlDbType.UInt32 ).Value = logradouro.bairro.codigo;
				logradouro.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o pais foi inserido ou nao
				if( logradouro.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o logradouro: " + logradouro.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizar( List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE tb_logradouros SET ");
			sql.AppendLine("	 nom_logradouro = @nom_logradouro ");
			sql.AppendLine( "	,txt_cep = @txt_cep " );
			sql.AppendLine( "	,cod_tipo_logradouro = @cod_tipo_logradouro " );
			sql.AppendLine("	,cod_bairro = @cod_bairro ");
			sql.AppendLine("WHERE cod_logradouro = @cod_logradouro ");

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Logradouro logradouro in logradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_logradouro", MySqlDbType.VarChar ).Value = logradouro.nome;
				cmd.Parameters.Add( "@txt_cep", MySqlDbType.VarChar ).Value = logradouro.cep;
				cmd.Parameters.Add( "@cod_tipo_logradouro", MySqlDbType.UInt32 ).Value = logradouro.tipoDeLogradouro.codigo;
				cmd.Parameters.Add( "@cod_bairro", MySqlDbType.UInt32 ).Value = logradouro.bairro.codigo;
				cmd.Parameters.Add( "@cod_logradouro", MySqlDbType.UInt32 ).Value = logradouro.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o logradouro: " + logradouro.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluir( List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_logradouros WHERE cod_logradouro = @cod_logradouro ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Logradouro logradouro in logradouros ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_logradouro", MySqlDbType.UInt32 ).Value = logradouro.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o logradouro: " + logradouro.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
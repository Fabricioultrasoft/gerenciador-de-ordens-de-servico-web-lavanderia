using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos {
	public class MySqlCidadesDao {

		public static long count() {
			String sql = "SELECT COUNT(cod_cidade) FROM tb_cidades";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<Cidade> getCidades() {
			return getCidades( 0, 0, 0 );
		}

		public static List<Cidade> getCidades( UInt32 start, UInt32 limit, UInt32 codigoEstado ) {
			List<Cidade> cidades = new List<Cidade>();

			StringBuilder sql = new StringBuilder();
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_cidade " ); // 0
			sql.AppendLine( "	,nom_cidade " ); // 1
			sql.AppendLine( "	,B.cod_estado " ); // 2
			sql.AppendLine( "	,B.nom_estado " ); // 3
			sql.AppendLine( "	,C.cod_pais " ); // 4
			sql.AppendLine( "	,C.nom_pais " ); // 5
			sql.AppendLine( "FROM tb_cidades A " );
			sql.AppendLine( "INNER JOIN tb_estados B ON B.cod_estado = A.cod_estado " );
			sql.AppendLine( "INNER JOIN tb_paises C ON C.cod_pais = B.cod_pais " );
			if( codigoEstado > 0 ) {
				sql.AppendFormat( "WHERE A.cod_estado = @cod_estado " );
				cmd.Parameters.Add( "@cod_estado", MySqlDbType.UInt32 ).Value = codigoEstado;
			}
			sql.AppendLine( "ORDER BY nom_cidade " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );


			cmd.Connection = conn;
			cmd.CommandText = sql.ToString();
			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Cidade cidade = new Cidade( reader.GetUInt32( 0 ), reader.GetString( 1 ) );
				cidade.estado.codigo = reader.GetUInt32( 2 );
				cidade.estado.nome = reader.GetString( 3 );
				cidade.estado.pais.codigo = reader.GetUInt32( 4 );
				cidade.estado.pais.nome = reader.GetString( 5 );
				cidades.Add( cidade );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return cidades;
		}

		public static List<Erro> inserir( ref List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_cidades(nom_cidade,cod_estado) VALUES(@nom_cidade,@cod_estado); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cidade cidade in cidades ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_cidade", MySqlDbType.VarChar ).Value = cidade.nome;
				cmd.Parameters.Add( "@cod_estado", MySqlDbType.UInt32 ).Value = cidade.estado.codigo;
				cidade.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o pais foi inserido ou nao
				if( cidade.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir a cidade: " + cidade.nome, "Tente inseri-la novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizar( List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_cidades SET nom_cidade = @nom_cidade, cod_estado = @cod_estado WHERE cod_cidade = @cod_cidade ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cidade cidade in cidades ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nom_cidade", MySqlDbType.VarChar ).Value = cidade.nome;
				cmd.Parameters.Add( "@cod_cidade", MySqlDbType.UInt32 ).Value = cidade.codigo;
				cmd.Parameters.Add( "@cod_estado", MySqlDbType.UInt32 ).Value = cidade.estado.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar a cidade: " + cidade.nome, "Tente atualiza-la novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluir( List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_cidades WHERE cod_cidade = @cod_cidade ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cidade cidade in cidades ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				try {
					cmd.Parameters.Add( "@cod_cidade", MySqlDbType.UInt32 ).Value = cidade.codigo;
					if( cmd.ExecuteNonQuery() <= 0 ) {
						erros.Add( new Erro( 0, "Não foi possível excluir a cidade: " + cidade.nome, "Tente excluí-la novamente" ) );
					}
				} catch( MySqlException ex ) {
					if( ex.Number == (int) MySqlErrorCode.RowIsReferenced2 ) {
						erros.Add( new Erro( ex.Number, "N&atilde;o foi poss&iacute;vel excluir a cidade: " + cidade.nome + ", ela est&aacute; sendo usada por um <i>Bairro</i>",
							"Exclua ou altere todos os Bairros que fazem uso desta <i>Cidade</i> para que ela possa ser exclu&iacute;da" ) );
					}
				} finally {
					cmd.Dispose();
				}
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
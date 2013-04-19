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
	public class MySqlBairrosDao {

		public static long count() {
			String sql = "SELECT COUNT(cod_bairro) FROM tb_bairros";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<Bairro> getBairros() {
			return getBairros( 0, 0, 0 );
		}

		public static List<Bairro> getBairros( UInt32 start, UInt32 limit, UInt32 codigoCidade ) {
			List<Bairro> bairros = new List<Bairro>();

			StringBuilder sql = new StringBuilder();
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_bairro " ); // 0
			sql.AppendLine( "	,nom_bairro " ); // 1
			sql.AppendLine( "	,B.cod_cidade " ); // 2
			sql.AppendLine( "	,B.nom_cidade " ); // 3
			sql.AppendLine( "	,C.cod_estado " ); // 4
			sql.AppendLine( "	,C.nom_estado " ); // 5
			sql.AppendLine( "	,D.cod_pais " ); // 6
			sql.AppendLine( "	,D.nom_pais " ); // 7
			sql.AppendLine( "FROM tb_bairros A " );
			sql.AppendLine( "INNER JOIN tb_cidades B ON B.cod_cidade = A.cod_cidade " );
			sql.AppendLine( "INNER JOIN tb_estados C ON C.cod_estado = B.cod_estado " );
			sql.AppendLine( "INNER JOIN tb_paises D ON D.cod_pais = C.cod_pais " );
			if( codigoCidade > 0 ) {
				sql.AppendFormat( "WHERE A.cod_cidade = @cod_cidade " );
				cmd.Parameters.Add( "@cod_cidade", MySqlDbType.UInt32 ).Value = codigoCidade;
			}
			sql.AppendLine( "ORDER BY nom_bairro " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			cmd.Connection = conn;
			cmd.CommandText = sql.ToString();
			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Bairro bairro = new Bairro( reader.GetUInt32( 0 ), reader.GetString( 1 ) );
				bairro.cidade.codigo = reader.GetUInt32( 2 );
				bairro.cidade.nome = reader.GetString( 3 );
				bairro.cidade.estado.codigo = reader.GetUInt32( 4 );
				bairro.cidade.estado.nome = reader.GetString( 5 );
				bairro.cidade.estado.pais.codigo = reader.GetUInt32( 6 );
				bairro.cidade.estado.pais.nome = reader.GetString( 7 );
				bairros.Add( bairro );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return bairros;
		}

		public static List<Erro> inserir( ref List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_bairros(nom_bairro,cod_cidade) VALUES(@nom_bairro,@cod_cidade); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Bairro bairro in bairros ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_bairro", MySqlDbType.VarChar ).Value = bairro.nome;
				cmd.Parameters.Add( "@cod_cidade", MySqlDbType.UInt32 ).Value = bairro.cidade.codigo;
				bairro.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o pais foi inserido ou nao
				if( bairro.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o bairro: " + bairro.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizar( List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			String sql = "UPDATE tb_bairros SET nom_bairro = @nom_bairro, cod_cidade = @cod_cidade WHERE cod_bairro = @cod_bairro ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Bairro bairro in bairros ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@nom_bairro", MySqlDbType.VarChar ).Value = bairro.nome;
				cmd.Parameters.Add( "@cod_bairro", MySqlDbType.UInt32 ).Value = bairro.codigo;
				cmd.Parameters.Add( "@cod_cidade", MySqlDbType.UInt32 ).Value = bairro.cidade.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o bairro: " + bairro.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluir( List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_bairros WHERE cod_bairro = @cod_bairro ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Bairro bairro in bairros ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				try {
					cmd.Parameters.Add( "@cod_bairro", MySqlDbType.UInt32 ).Value = bairro.codigo;
					if( cmd.ExecuteNonQuery() <= 0 ) {
						erros.Add( new Erro( 0, "Não foi possível excluir o bairro: " + bairro.nome, "Tente excluí-lo novamente" ) );
					}
				} catch( MySqlException ex ) {
					if( ex.Number == (int) MySqlErrorCode.RowIsReferenced2 ) {
						erros.Add( new Erro( ex.Number, "N&atilde;o foi poss&iacute;vel excluir o bairro: " + bairro.nome + ", ele est&aacute; sendo usado por um <i>Logradouro</i>",
							"Exclua ou altere todos os Logradouros que fazem uso deste <i>Bairro</i> para que ele possa ser exclu&iacute;do" ) );
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes {
	public class MySqlTapetesDao {

		public static long countTapetes() {
			String sql = "SELECT COUNT(cod_tapete) FROM tb_tapetes";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static List<Tapete> getTapetes() {
			return getTapetes( 0, 0 );
		}

		public static List<Tapete> getTapetes( UInt32 start, UInt32 limit ) {
			List<Tapete> tapetes = new List<Tapete>();

			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_tapete " );
			sql.AppendLine( "	,nom_tapete " );
			sql.AppendLine( "	,txt_descricao " );
			sql.AppendLine( "	,flg_ativo " );
			sql.AppendLine( "FROM tb_tapetes " );
			sql.AppendLine( "ORDER BY nom_tapete " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Tapete tapete = new Tapete();
				tapete.codigo = reader.GetUInt32( 0 );
				tapete.nome = reader.GetString( 1 );
				try { tapete.descricao = reader.GetString( 2 ); } catch { }
				tapete.ativo = reader.GetBoolean( 3 );
				tapetes.Add( tapete );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return tapetes;
		}

		public static List<Erro> inserirListaDeTapetes( ref List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_tapetes(nom_tapete,txt_descricao,flg_ativo) " );
			sql.AppendLine( "VALUES(@nom_tapete,@txt_descricao,@flg_ativo); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Tapete tapete in tapetes ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_tapete", MySqlDbType.VarChar ).Value = tapete.nome;
				cmd.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = tapete.descricao;
				cmd.Parameters.Add( "@flg_ativo", MySqlDbType.Bit ).Value = (tapete.ativo) ?1:0;
				tapete.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o tapete foi inserido ou nao
				if( tapete.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o tapete: " + tapete.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDeTapetes( List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();
			sql.AppendLine( "UPDATE tb_tapetes SET ");
			sql.AppendLine( "	 nom_tapete = @nom_tapete " );
			sql.AppendLine( "	,txt_descricao = @txt_descricao " );
			sql.AppendLine( "	,flg_ativo = @flg_ativo " );
			sql.AppendLine( "WHERE cod_tapete = @cod_tapete " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Tapete tapete in tapetes ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nom_tapete", MySqlDbType.VarChar ).Value = tapete.nome;
				cmd.Parameters.Add( "@cod_tapete", MySqlDbType.UInt32 ).Value = tapete.codigo;
				cmd.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = tapete.descricao;
				cmd.Parameters.Add( "@flg_ativo", MySqlDbType.Bit ).Value = (tapete.ativo) ? 1 : 0;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o tapete: " + tapete.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}


		public static List<Erro> excluirListaDeTapetes( List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_tapetes WHERE cod_tapete = @cod_tapete ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Tapete tapete in tapetes ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_tapete", MySqlDbType.UInt32 ).Value = tapete.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o tapete: " + tapete.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}
	}
}
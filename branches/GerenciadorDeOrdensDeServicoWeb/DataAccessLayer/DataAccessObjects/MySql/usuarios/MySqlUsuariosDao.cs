using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using System.Text;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.usuarios {
	public class MySqlUsuariosDao {

		public static long count() {
			String sql = "SELECT COUNT(cod_usuario) FROM tb_usuarios";
			long qtd = 0;

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			conn.Open();

			qtd = (long) cmd.ExecuteScalar();

			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return qtd;
		}

		public static void preencherUsuario( UInt32 codigo, ref Usuario usuario, MySqlConnection conn ) {
			
			MySqlCommand cmd = new MySqlCommand( "SELECT cod_usuario, nom_usuario, txt_senha FROM tb_usuarios WHERE cod_usuario = @codUsuario", conn );
			cmd.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = codigo;
			MySqlDataReader reader = cmd.ExecuteReader();
			if( reader.Read() ) {
				usuario.codigo = reader.GetUInt32( 0 );
				usuario.nome = reader.GetString( 1 );
				usuario.senha = reader.GetString( 2 );
			}
			reader.Close(); reader.Dispose();
			cmd.Dispose();
		}

		public static Usuario getUsuario( UInt32 codigo ) {
			Usuario usuario = new Usuario();

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();
			preencherUsuario( codigo, ref usuario, conn );
			conn.Close(); conn.Dispose();

			return usuario;
		}

		public static Usuario getUsuario( String nome, String senha ) {
			StringBuilder sql = new StringBuilder();
			Usuario usuario;

			sql.AppendLine( "SELECT ");
			sql.AppendLine( "	 cod_usuario ");
			sql.AppendLine( "	,nom_usuario ");
			sql.AppendLine( "	,txt_senha ");
			sql.AppendLine( "FROM tb_usuarios ");
			sql.AppendLine( "WHERE ");
			sql.AppendLine( "	nom_usuario = @nomUsuario ");
			sql.AppendLine( "AND");
			sql.AppendLine( "	txt_senha = SHA1(@txtSenha) ");

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@nomUsuario", MySqlDbType.VarChar ).Value = nome;
			cmd.Parameters.Add( "@txtSenha", MySqlDbType.VarChar ).Value = senha;

			MySqlDataReader reader = cmd.ExecuteReader();

			if( reader.Read() ) {
				usuario = new Usuario( reader.GetUInt32( 0 ), reader.GetString( 1 ), reader.GetString( 2 ) );
			} else {
				usuario = new Usuario();
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return usuario;
		}

		public static List<Usuario> getUsuarios() {
			return getUsuarios( 0, 0 );
		}

		public static List<Usuario> getUsuarios( UInt32 start, UInt32 limit ) {
			List<Usuario> usuarios = new List<Usuario>();

			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT " );
			sql.AppendLine( "	 cod_usuario " );
			sql.AppendLine( "	,nom_usuario " );
			sql.AppendLine( "	,txt_senha " );
			sql.AppendLine( "FROM tb_usuarios " );
			sql.AppendLine( "ORDER BY nom_usuario " );
			if( limit > 0 )
				sql.AppendFormat( "LIMIT {0},{1}", start, limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				Usuario usu = new Usuario(reader.GetUInt32( 0 ),reader.GetString( 1 ), reader.GetString( 2 ));
				usuarios.Add( usu );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return usuarios;
		}

		public static List<Erro> inserir( ref List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "INSERT INTO tb_usuarios(nom_usuario,txt_senha) " );
			sql.AppendLine( "VALUES(@nomUsuario,SHA1(@txtSenha)); " );
			sql.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Usuario usu in usuarios ) {

				if( getUsuario( usu.nome, usu.senha ).codigo > 0 ) {
					erros.Add( new Erro( 0, "O usu&aacute;rio: " + usu.nome + " j&aacute; est&aacute; cadastrado!", "Entre com um nome de usu&aacute;rio diferente" ) );
					continue;
				}

				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@nomUsuario", MySqlDbType.VarChar ).Value = usu.nome;
				cmd.Parameters.Add( "@txtSenha", MySqlDbType.VarChar ).Value = usu.senha;
				usu.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				// registra se o usuario foi inserido ou nao
				if( usu.codigo <= 0 )
					erros.Add( new Erro( 0, "Não foi possível inserir o usu&aacute;rio: " + usu.nome, "Tente inseri-lo novamente" ) );
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizar( List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sql = new StringBuilder();
			sql.AppendLine( "UPDATE tb_usuarios SET " );
			sql.AppendLine( "	 nom_usuario = @nomUsuario " );
			sql.AppendLine( "	,txt_senha = SHA1(@txtSenha) " );
			sql.AppendLine( "WHERE cod_usuario = @codUsuario " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Usuario usu in usuarios ) {
				MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
				cmd.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = usu.codigo;
				cmd.Parameters.Add( "@nomUsuario", MySqlDbType.VarChar ).Value = usu.nome;
				cmd.Parameters.Add( "@txtSenha", MySqlDbType.VarChar ).Value = usu.senha;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar o usu&aacute;rio: " + usu.nome, "Tente atualiza-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluir( List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_usuarios WHERE cod_usuario = @codUsuario ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Usuario usu in usuarios ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = usu.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o usu&aacute;rio: " + usu.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

	}
}
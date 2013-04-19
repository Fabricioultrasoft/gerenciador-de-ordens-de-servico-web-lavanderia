using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.usuarios {
	public class GerenciadorDeUsuarios {

		public static bool autenticar( ref Usuario usuario ) {
			usuario.codigo = MySqlUsuariosDao.getUsuario( usuario.nome, usuario.senha ).codigo;
			if( usuario.codigo > 0 ) {
				return true;
			} else {
				return false;
			}
		}

		public static long count() {
			try {
				return MySqlUsuariosDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Usuario> usuarios, UInt32 start, UInt32 limit ) {
			List<Erro> erros = new List<Erro>();
			try {
				usuarios = MySqlUsuariosDao.getUsuarios( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				usuarios = new List<Usuario>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( (int) MySqlErrorCode.UnableToConnectToHost ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlUsuariosDao.inserir( ref usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( (int) MySqlErrorCode.UnableToConnectToHost ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlUsuariosDao.atualizar( usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( (int) MySqlErrorCode.UnableToConnectToHost ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Usuario> usuarios ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlUsuariosDao.excluir( usuarios ) );
			} catch( MySqlException ex ) {
				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( (int) MySqlErrorCode.UnableToConnectToHost ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

	}
}
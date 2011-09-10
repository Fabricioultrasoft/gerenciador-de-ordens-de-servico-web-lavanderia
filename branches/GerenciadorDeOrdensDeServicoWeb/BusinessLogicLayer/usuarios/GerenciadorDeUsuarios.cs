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

		public static long countUsuarios() {
			try {
				return MySqlUsuariosDao.countUsuarios();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeUsuarios( out List<Usuario> usuarios, UInt32 start, UInt32 limit ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				usuarios = MySqlUsuariosDao.getUsuarios( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				usuarios = new List<Usuario>();

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042 ) );
				} else {
					Erro erro = new Erro( 0 );
					erro.mensagem = ex.Message;
					listaDeErros.Add( erro );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> cadastrarListaDeUsuarios( ref List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.inserirListaDeUsuarios( ref usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042 ) );
				} else {
					Erro erro = new Erro( 0 );
					erro.mensagem = ex.Message;
					listaDeErros.Add( erro );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> atualizarListaDeUsuarios( List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.atualizarListaDeUsuarios( usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042 ) );
				} else {
					Erro erro = new Erro( 0 );
					erro.mensagem = ex.Message;
					listaDeErros.Add( erro );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> excluirListaDeUsuarios( List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.excluirListaDeUsuarios( usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042 ) );
				} else {
					Erro erro = new Erro( 0 );
					erro.mensagem = ex.Message;
					listaDeErros.Add( erro );
				}
			}
			return listaDeErros;
		}

	}
}
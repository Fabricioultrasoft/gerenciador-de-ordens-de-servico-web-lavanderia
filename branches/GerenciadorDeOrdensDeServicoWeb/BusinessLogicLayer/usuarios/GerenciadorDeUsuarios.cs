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

		public static List<Erro> cadastrar( ref List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.inserir( ref usuarios ) );
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

		public static List<Erro> atualizar( List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.atualizar( usuarios ) );
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

		public static List<Erro> excluir( List<Usuario> usuarios ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlUsuariosDao.excluir( usuarios ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1451 ) {
					listaDeErros.Add( new Erro( 1451, "N&atilde;o foi poss&iacute;vel excluir este registro, ele est&aacute; sendo usado por uma <i>Ordem de servi&ccedil;o</i>",
						"Exclua ou altere todos as Ordens de Servi&ccedil;o que fazem uso deste <i>Usuario</i> para que ele possa ser exclu&iacute;do" ) );
				} else if( ex.Number == 1042 ) {
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
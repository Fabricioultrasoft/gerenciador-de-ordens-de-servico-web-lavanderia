using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDePaises {

		public static long count() {
			try {
				return MySqlPaisesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Pais> paises, UInt32 start, UInt32 limit ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				paises = MySqlPaisesDao.getPaises( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				paises = new List<Pais>();

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

		public static List<Erro> cadastrar( ref List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.inserir( ref paises));
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

		public static List<Erro> atualizar( List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.atualizar( paises ) );
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

		public static List<Erro> excluir( List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.excluir( paises ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1451 ) {
					listaDeErros.Add( new Erro( 1451, "N&atilde;o foi poss&iacute;vel excluir este registro, ele est&aacute; sendo usado por um <i>Estado</i>",
						"Exclua ou altere todos os Estados que fazem uso deste <i>Pa&iacute;s</i> para que ele possa ser exclu&iacute;do" ) );
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
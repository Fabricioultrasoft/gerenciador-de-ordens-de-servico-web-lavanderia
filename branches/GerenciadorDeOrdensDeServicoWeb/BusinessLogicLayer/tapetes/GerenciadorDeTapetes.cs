using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.tapetes {
	public class GerenciadorDeTapetes {

		public static long count() {
			try {
				return MySqlTapetesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Tapete> tapetes, UInt32 start, UInt32 limit ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				tapetes = MySqlTapetesDao.getTapetes( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tapetes = new List<Tapete>();

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

		public static List<Erro> cadastrar( ref List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.inserir( ref tapetes ) );
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

		public static List<Erro> atualizar( List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.atualizar( tapetes ) );
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

		public static List<Erro> excluir( List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.excluir( tapetes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1451 ) {
					listaDeErros.Add( new Erro( 1451, "N&atilde;o foi poss&iacute;vel excluir este registro, ele est&aacute; sendo usado por uma <i>Ordem de servi&ccedil;o</i>",
						"Exclua ou altere todos as Ordens de Servi&ccedil;o que fazem uso deste <i>Tapete</i> para que ele possa ser exclu&iacute;do ou marque este registro como inativo" ) );
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
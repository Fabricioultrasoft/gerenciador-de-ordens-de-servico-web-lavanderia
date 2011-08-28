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

		public static long countTapetes() {
			try {
				return MySqlTapetesDao.countTapetes();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeTapetes( out List<Tapete> tapetes, UInt32 start, UInt32 limit ) {
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

		public static List<Erro> cadastrarListaDeTapetes( ref List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.inserirListaDeTapetes( ref tapetes ) );
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

		public static List<Erro> atualizarListaDeTapetes( List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.atualizarListaDeTapetes( tapetes ) );
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

		public static List<Erro> excluirListaDeTapetes( List<Tapete> tapetes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTapetesDao.excluirListaDeTapetes( tapetes ) );
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
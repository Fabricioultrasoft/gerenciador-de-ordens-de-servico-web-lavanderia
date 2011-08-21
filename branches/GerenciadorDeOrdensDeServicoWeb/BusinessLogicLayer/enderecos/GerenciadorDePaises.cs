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

		public static long countPaises() {
			try {
				return MySqlPaisesDao.countPaises();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDePaises( out List<Pais> paises, UInt32 start, UInt32 limit ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				paises = MySqlPaisesDao.getPaises( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				paises = new List<Pais>();

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> cadastrarListaDePaises( ref List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.inserirListaDePaises( ref paises));
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> atualizarListaDePaises( List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.atualizarListaDePaises( paises ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> excluirListaDePaises( List<Pais> paises ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlPaisesDao.excluirListaDePaises( paises ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}
	}
}
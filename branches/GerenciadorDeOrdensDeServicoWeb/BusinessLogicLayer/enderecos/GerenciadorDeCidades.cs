using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeCidades {

		public static long countCidades() {
			try {
				return MySqlCidadesDao.countCidades();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeCidades( out List<Cidade> cidades, UInt32 start, UInt32 limit, UInt32 codigoEstado ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				cidades = MySqlCidadesDao.getCidades( start, limit, codigoEstado );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				cidades = new List<Cidade>();

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> cadastrarListaDeCidades( ref List<Cidade> cidades ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlCidadesDao.inserirListaDeCidades( ref cidades ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> atualizarListaDeCidades( List<Cidade> cidades ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlCidadesDao.atualizarListaDeCidades( cidades ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> excluirListaDeCidades( List<Cidade> cidades ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlCidadesDao.excluirListaDeCidades( cidades ) );
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
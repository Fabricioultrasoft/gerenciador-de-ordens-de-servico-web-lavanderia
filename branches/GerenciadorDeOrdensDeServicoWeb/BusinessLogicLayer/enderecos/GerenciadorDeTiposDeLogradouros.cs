using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeTiposDeLogradouros {

		public static List<Erro> preencherListaDeTiposDeLogradouros( out List<TipoDeLogradouro> tiposDeLogradouros, UInt32 start, UInt32 limit ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				tiposDeLogradouros = MySqlTiposDeLogradourosDao.getTiposDeLogradouros( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tiposDeLogradouros = new List<TipoDeLogradouro>();

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> cadastrarListaDeTiposDeLogradouros( ref List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeLogradourosDao.inserirListaDeTiposDeLogradouros( ref tiposDeLogradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> atualizarListaDeTiposDeLogradouros( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeLogradourosDao.atualizarListaDeTiposDeLogradouros( tiposDeLogradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1042 ) {
					listaDeErros.Add( new Erro( 1042, "Não foi possivel estabelecer uma conexão com o banco de dados", "Verifique se o banco de dados encontra-se em execução" ) );
				} else {
					listaDeErros.Add( new Erro( 0, ex.Message, "Contate o Fornecedor" ) );
				}
			}
			return listaDeErros;
		}

		public static List<Erro> excluirListaDeTiposDeLogradouros( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeLogradourosDao.excluirListaDeTiposDeLogradouros( tiposDeLogradouros ) );
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeLogradouros {

		public static long countLogradouros() {
			try {
				return MySqlLogradourosDao.countLogradouros();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeLogradouros( out List<Logradouro> logradouros, UInt32 start, UInt32 limit, UInt32 codigoBairro ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				logradouros = MySqlLogradourosDao.getLogradouros( start, limit, codigoBairro );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				logradouros = new List<Logradouro>();

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

		public static List<Erro> cadastrarListaDeLogradouros( ref List<Logradouro> logradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlLogradourosDao.inserirListaDeLogradouros( ref logradouros ) );
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

		public static List<Erro> atualizarListaDeLogradouros( List<Logradouro> logradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlLogradourosDao.atualizarListaDeLogradouros( logradouros ) );
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

		public static List<Erro> excluirListaDeLogradouros( List<Logradouro> logradouros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlLogradourosDao.excluirListaDeLogradouros( logradouros ) );
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
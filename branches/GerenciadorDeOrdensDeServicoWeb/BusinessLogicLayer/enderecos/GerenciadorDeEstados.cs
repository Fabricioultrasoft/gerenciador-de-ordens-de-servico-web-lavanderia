using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeEstados {

		public static long countEstados() {
			try {
				return MySqlEstadosDao.countEstados();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeEstados( out List<Estado> estados, UInt32 start, UInt32 limit, UInt32 codigoPais ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				estados = MySqlEstadosDao.getEstados( start, limit, codigoPais );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				estados = new List<Estado>();

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

		public static List<Erro> cadastrarListaDeEstados( ref List<Estado> estados ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlEstadosDao.inserirListaDeEstados( ref estados ) );
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

		public static List<Erro> atualizarListaDeEstados( List<Estado> estados ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlEstadosDao.atualizarListaDeEstados( estados ) );
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

		public static List<Erro> excluirListaDeEstados( List<Estado> estados ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlEstadosDao.excluirListaDeEstados( estados ) );
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
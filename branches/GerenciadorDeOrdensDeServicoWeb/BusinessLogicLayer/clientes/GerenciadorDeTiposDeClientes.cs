using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes {
	public class GerenciadorDeTiposDeClientes {

		public static long countTiposDeClientes() {
			try {
				return MySqlTiposDeClientesDao.countTiposDeClientes();
			} catch {
				return 0;
			}
		}

		public static List<Erro> cadastrarListaDeTiposDeClientes( ref List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeClientesDao.inserirListaDeTiposDeClientes( ref tiposDeClientes ) );
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

		public static List<Erro> preencherListaDeTiposDeClientes( out List<TipoDeCliente> tiposDeClientes, UInt32 start, UInt32 limit, bool? ativo ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				tiposDeClientes = MySqlTiposDeClientesDao.getTiposDeClientes( start, limit, ativo );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tiposDeClientes = new List<TipoDeCliente>();

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

		public static List<Erro> atualizarListaDeTiposDeClientes( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeClientesDao.atualizarListaDeTiposDeClientes( tiposDeClientes ) );
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

		public static List<Erro> excluirListaDeTiposDeClientes( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlTiposDeClientesDao.excluirListaDeTiposDeClientes( tiposDeClientes ) );
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
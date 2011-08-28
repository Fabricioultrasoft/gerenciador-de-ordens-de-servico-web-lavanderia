using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes {
	public class GerenciadorDeClientes {

		public static long countTiposDeClientes() {
			try {
				return MySqlTiposDeClientesDao.countTiposDeClientes();
			} catch {
				return 0;
			}
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

		public static long countClientes() {
			try {
				return MySqlClientesDao.countClientes();
			} catch {
				return 0;
			}
		}

		public static long countClientes( List<Filter> filters ) {
			try {
				return MySqlClientesDao.countClientes( filters );
			} catch {
				return 0;
			}
		}

		public static List<Erro> cadastrarListaDeClientes( ref List<Cliente> clientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlClientesDao.inserirListaDeClientes( ref clientes ) );
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

		public static List<Erro> preencherListaDeClientes( out List<Cliente> clientes, UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				clientes = MySqlClientesDao.getClientes( start, limit, filters, sorters );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				clientes = new List<Cliente>();

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

		public static List<Erro> atualizarListaDeClientes( ref List<Cliente> clientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlClientesDao.atualizarListaDeClientes( ref clientes ) );
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

		public static List<Erro> excluirListaDeClientes( List<Cliente> clientes ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlClientesDao.excluirListaDeClientes( clientes ) );
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
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

		public static long count() {
			try {
				return MySqlClientesDao.count();
			} catch {
				return 0;
			}
		}

		public static long count( List<Filter> filters ) {
			try {
				return MySqlClientesDao.count( filters );
			} catch {
				return 0;
			}
		}

		public static List<Erro> cadastrar( ref List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlClientesDao.inserir( ref clientes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro(ex.Number, ex.Message) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( out List<Cliente> clientes, UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<Erro> erros = new List<Erro>();
			try {
				clientes = MySqlClientesDao.getClientes( start, limit, filters, sorters );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				clientes = new List<Cliente>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( ref List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlClientesDao.atualizar( ref clientes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlClientesDao.excluir( clientes ) );
			} catch( MySqlException ex ) {
				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}
	}
}
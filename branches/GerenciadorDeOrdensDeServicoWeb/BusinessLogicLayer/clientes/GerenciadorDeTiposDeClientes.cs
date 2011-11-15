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

		public static long count() {
			try {
				return MySqlTiposDeClientesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> cadastrar( ref List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeClientesDao.inserir( ref tiposDeClientes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( out List<TipoDeCliente> tiposDeClientes, UInt32 start, UInt32 limit, bool? ativo ) {
			List<Erro> erros = new List<Erro>();
			try {
				tiposDeClientes = MySqlTiposDeClientesDao.getTiposDeClientes( start, limit, ativo );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tiposDeClientes = new List<TipoDeCliente>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeClientesDao.atualizar( tiposDeClientes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<TipoDeCliente> tiposDeClientes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeClientesDao.excluir( tiposDeClientes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}
	}
}
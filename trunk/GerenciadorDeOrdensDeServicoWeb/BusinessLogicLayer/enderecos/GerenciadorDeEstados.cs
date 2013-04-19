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

		public static long count() {
			try {
				return MySqlEstadosDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Estado> estados, UInt32 start, UInt32 limit, UInt32 codigoPais ) {
			List<Erro> erros = new List<Erro>();
			try {
				estados = MySqlEstadosDao.getEstados( start, limit, codigoPais );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				estados = new List<Estado>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlEstadosDao.inserir( ref estados ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlEstadosDao.atualizar( estados ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Estado> estados ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlEstadosDao.excluir( estados ) );
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
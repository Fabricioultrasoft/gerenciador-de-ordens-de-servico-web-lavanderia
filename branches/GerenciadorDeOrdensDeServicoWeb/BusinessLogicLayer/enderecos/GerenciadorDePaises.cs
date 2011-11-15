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

		public static long count() {
			try {
				return MySqlPaisesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Pais> paises, UInt32 start, UInt32 limit ) {
			List<Erro> erros = new List<Erro>();
			try {
				paises = MySqlPaisesDao.getPaises( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				paises = new List<Pais>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlPaisesDao.inserir( ref paises));
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlPaisesDao.atualizar( paises ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Pais> paises ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlPaisesDao.excluir( paises ) );
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
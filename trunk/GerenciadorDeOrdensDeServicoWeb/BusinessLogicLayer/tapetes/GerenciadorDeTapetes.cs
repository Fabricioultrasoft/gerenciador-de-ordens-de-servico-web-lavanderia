using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.tapetes {
	public class GerenciadorDeTapetes {

		public static long count() {
			try {
				return MySqlTapetesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Tapete> tapetes, UInt32 start, UInt32 limit ) {
			List<Erro> erros = new List<Erro>();
			try {
				tapetes = MySqlTapetesDao.getTapetes( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tapetes = new List<Tapete>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTapetesDao.inserir( ref tapetes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTapetesDao.atualizar( tapetes ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Tapete> tapetes ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTapetesDao.excluir( tapetes ) );
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
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

		public static long count() {
			try {
				return MySqlLogradourosDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Logradouro> logradouros, UInt32 start, UInt32 limit, UInt32 codigoBairro ) {
			List<Erro> erros = new List<Erro>();
			try {
				logradouros = MySqlLogradourosDao.getLogradouros( start, limit, codigoBairro );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				logradouros = new List<Logradouro>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlLogradourosDao.inserir( ref logradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlLogradourosDao.atualizar( logradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Logradouro> logradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlLogradourosDao.excluir( logradouros ) );
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
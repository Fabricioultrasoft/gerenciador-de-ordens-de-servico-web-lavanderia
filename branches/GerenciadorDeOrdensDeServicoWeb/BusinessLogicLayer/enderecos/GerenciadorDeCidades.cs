using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeCidades {

		public static long count() {
			try {
				return MySqlCidadesDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Cidade> cidades, UInt32 start, UInt32 limit, UInt32 codigoEstado ) {
			List<Erro> erros = new List<Erro>();
			try {
				cidades = MySqlCidadesDao.getCidades( start, limit, codigoEstado );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				cidades = new List<Cidade>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlCidadesDao.inserir( ref cidades ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlCidadesDao.atualizar( cidades ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Cidade> cidades ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlCidadesDao.excluir( cidades ) );
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
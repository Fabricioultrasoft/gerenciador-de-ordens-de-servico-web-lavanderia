using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeBairros {

		public static long count() {
			try {
				return MySqlBairrosDao.count();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Bairro> bairros, UInt32 start, UInt32 limit, UInt32 codigoCidade ) {
			List<Erro> erros = new List<Erro>();
			try {
				bairros = MySqlBairrosDao.getBairros( start, limit, codigoCidade);
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				bairros = new List<Bairro>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlBairrosDao.inserir( ref bairros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlBairrosDao.atualizar( bairros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Bairro> bairros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlBairrosDao.excluir( bairros ) );
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
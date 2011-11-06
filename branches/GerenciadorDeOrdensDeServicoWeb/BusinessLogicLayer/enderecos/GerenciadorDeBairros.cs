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
			List<Erro> listaDeErros = new List<Erro>();
			try {
				bairros = MySqlBairrosDao.getBairros( start, limit, codigoCidade);
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				bairros = new List<Bairro>();

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

		public static List<Erro> cadastrar( ref List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.inserir( ref bairros ) );
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

		public static List<Erro> atualizar( List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.atualizar( bairros ) );
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

		public static List<Erro> excluir( List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.excluir( bairros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == 1451 ) {
					listaDeErros.Add( new Erro( 1451, "N&atilde;o foi poss&iacute;vel excluir este registro, ele est&aacute; sendo usado por um <i>Logradouro</i>",
						"Exclua ou altere todos os Logradouros que fazem uso deste <i>Bairro</i> para que ele possa ser exclu&iacute;do" ) );
				} else if( ex.Number == 1042 ) {
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
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

		public static long countBairros() {
			try {
				return MySqlBairrosDao.countBairros();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeBairros( out List<Bairro> bairros, UInt32 start, UInt32 limit, UInt32 codigoCidade ) {
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

		public static List<Erro> cadastrarListaDeBairros( ref List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.inserirListaDeBairros( ref bairros ) );
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

		public static List<Erro> atualizarListaDeBairros( List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.atualizarListaDeBairros( bairros ) );
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

		public static List<Erro> excluirListaDeBairros( List<Bairro> bairros ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlBairrosDao.excluirListaDeBairros( bairros ) );
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.servicos {
	public class GerenciadorDeServicos {

		public static long countServicos() {
			try {
				return MySqlServicosDao.countServicos();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencherListaDeServicos( out List<Servico> servicos, UInt32 start, UInt32 limit ) {
			return preencherListaDeServicos( out servicos, start, limit, false );
		}

		public static List<Erro> preencherListaDeServicos( out List<Servico> servicos, UInt32 start, UInt32 limit, bool apenasDadosBasicos ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				servicos = MySqlServicosDao.getServicos( start, limit, apenasDadosBasicos );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				servicos = new List<Servico>();

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

		public static List<Erro> preencherListaDeServicosEspecificos( out List<Servico> servicos, UInt32 codigoTapete, UInt32 codigoTipoDeCliente ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				servicos = MySqlServicosDao.getServicosEspecificos( codigoTapete, codigoTipoDeCliente );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				servicos = new List<Servico>();

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

		public static List<Erro> preencherServico( UInt32 codigoServico, out Servico servico ) {
			List<Erro> listaDeErros = new List<Erro>();

			try {
				if( codigoServico == 0 ) {
					servico = new Servico();
					List<Tapete> tapetes = MySqlTapetesDao.getTapetes();
					foreach( Tapete tapete in tapetes ) {
						ValorDeServico val = new ValorDeServico();
						val.tapete.codigo = tapete.codigo;
						val.tapete.nome = tapete.nome;
						servico.valores.Add(val);
					}
				} else {
					servico = MySqlServicosDao.getServico( codigoServico );
				}
			} catch( MySqlException ex ) {

				servico = new Servico( codigoServico );

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

		public static List<Erro> cadastrarListaDeServicos( ref List<Servico> servicos ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlServicosDao.inserirListaDeServicos( ref servicos ) );
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

		public static List<Erro> atualizarListaDeServicos( ref List<Servico> servicos ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlServicosDao.atualizarListaDeServicos( ref servicos ) );
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

		public static List<Erro> excluirListaDeServicos( List<Servico> servicos ) {
			List<Erro> listaDeErros = new List<Erro>();
			try {
				listaDeErros.AddRange( MySqlServicosDao.excluirListaDeServicos( servicos ) );
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
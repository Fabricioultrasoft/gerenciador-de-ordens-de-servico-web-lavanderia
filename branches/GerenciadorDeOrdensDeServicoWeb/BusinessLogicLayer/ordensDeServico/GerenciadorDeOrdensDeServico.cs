using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.ordensDeServico;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.ordensDeServico {
	public class GerenciadorDeOrdensDeServico {

		private static Dictionary<int, Erro> erros = getDicErros();

		public static long count() {
			try {
				return MySqlOrdensDeServicoDao.count();
			} catch {
				return 0;
			}
		}

		public static long count( List<Filter> filters ) {
			try {
				return MySqlOrdensDeServicoDao.count( filters );
			} catch {
				return 0;
			}
		}

		public static List<Erro> cadastrar( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			try {
				foreach( OrdemDeServico os in ordensDeServico ) {
					if( os.status.codigo == 0 ) {
						os.status.codigo = 1; // Status "Aberto"
					}

					if( MySqlOrdensDeServicoDao.numeroJaExiste( os.numero ) ) {
						erros.Add( erros[1] );
					}
						// SE Status diferente de "Aberto"
						// ENTAO nao pode atualizar OS
					else if( os.status.codigo != 1 ) {
						erros.Add( erros[2] );
					}
				}

				if( erros.Count == 0 ) {
					erros.AddRange( MySqlOrdensDeServicoDao.cadastrar( ref ordensDeServico ) );
				}
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( UInt32 codigo, out OrdemDeServico ordemDeServico ) {
			List<Erro> erros = new List<Erro>();
			try {
				ordemDeServico = MySqlOrdensDeServicoDao.getOrdemDeServicoByCod( codigo );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche um Objeto vazio
				ordemDeServico = new OrdemDeServico();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( out OrdemDeServico ordemDeServico, UInt32 numero ) {
			List<Erro> erros = new List<Erro>();
			try {
				ordemDeServico = MySqlOrdensDeServicoDao.getOrdemDeServicoByNum( numero );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche um Objeto vazio
				ordemDeServico = new OrdemDeServico();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( out List<OrdemDeServico> ordensDeServico, UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<Erro> erros = new List<Erro>();
			try {
				ordensDeServico = MySqlOrdensDeServicoDao.getOrdensDeServico( start, limit, filters, sorters );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				ordensDeServico = new List<OrdemDeServico>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( out List<Status> statusList ) {
			List<Erro> erros = new List<Erro>();
			try {
				statusList = MySqlOrdensDeServicoDao.getStatus();
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				statusList = new List<Status>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			try {

				foreach( OrdemDeServico os in ordensDeServico ) {

					if( MySqlOrdensDeServicoDao.numeroJaExiste( os.numero, os.codigo ) ) {
						erros.Add( erros[1] );
					}
						// SE Status diferente de "Aberto"
						// ENTAO nao pode atualizar OS
					else if( MySqlOrdensDeServicoDao.getStatus( os.codigo ).codigo != 1 ) {
						erros.Add( erros[2] );
					}
				}

				if( erros.Count == 0 ) {
					erros.AddRange( MySqlOrdensDeServicoDao.atualizar( ref ordensDeServico ) );
				}
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlOrdensDeServicoDao.excluir( ordensDeServico ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}


		public static Dictionary<int, Erro> getDicErros() {
			Dictionary<int, Erro> erros = new Dictionary<int, Erro>();

			erros.Add( 1, new Erro( 1, "O N&uacute;mero desta Ordem de Servi&ccedil;o j&aacute; est&aacute; sendo usado por outro registro", "Informe um novo n&uacute;mero para a Ordem de Servi&ccedil;o" ) );
			erros.Add( 2, new Erro( 2, "Status da Ordem de Servi&ccedil;o n&atilde;o permite altera&ccedil;&otilde;es", "Somente Ordens de Servi&ccedil;o com o status <b>Aberto</b> podem ser alteradas" ) );

			return erros;
		}
	}
}
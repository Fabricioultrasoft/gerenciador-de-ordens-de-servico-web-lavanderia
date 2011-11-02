using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.ordensDeServico {
	public class MySqlOrdensDeServicoDao {

		#region INSERT
		private const String INSERT_OS
			= "INSERT INTO tb_ordens_de_servico ("
			+ "	cod_cliente "
			+ "	,cod_usuario "
			+ "	,cod_status_os "
			+ "	,num_os "
			+ "	,val_original "
			+ "	,val_final "
			+ "	,dat_abertura "
			+ "	,dat_prev_conclusao "
			+ "	,txt_observacoes "
			+ "	,dat_atualizacao "
			+ ") VALUES ( "
			+ "	@codCliente "
			+ "	,@codUsuario "
			+ "	,@codStatusOS "
			+ "	,@numOS "
			+ "	,@valOriginal "
			+ "	,@valFinal "
			+ "	,@datAbertura "
			+ "	,@datPrevConclusao "
			+ "	,@txtObservacoes "
			+ "	,NOW()); "
			+ "SELECT LAST_INSERT_ID() ";

		private const String INSERT_ITENS
			= "INSERT INTO tb_itens_os ( "
			+ "	cod_tapete "
			+ "	,cod_ordem_de_servico "
			+ "	,flt_comprimento "
			+ "	,flt_largura "
			+ "	,dbl_area "
			+ "	,val_item "
			+ "	,txt_observacoes "
			+ ") VALUES ( "
			+ "	@codTapete "
			+ "	,@codOrdemDeServico "
			+ "	,@fltComprimento "
			+ "	,@fltLargura "
			+ "	,@dblArea "
			+ "	,@valItem "
			+ "	,@txtObservacoes); "
			+ "SELECT LAST_INSERT_ID() ";

		private const String INSERT_ITENS_SERVICOS 
			= "INSERT INTO tb_itens_servicos ( "
			+ "	cod_item_os "
			+ "	,cod_servico "
			+ "	,qtd_m_m2 "
			+ "	,val_item_servico "
			+ ") VALUES ( "
			+ "	@codItemOS "
			+ "	,@codServico "
			+ "	,@qtdMm2 "
			+ "	,@valItemServico); "
			+ "SELECT LAST_INSERT_ID() ";
		#endregion

		#region SELECT
		private const String SELECT_OS
			= "SELECT "
			+ "	cod_ordem_de_servico "
			+ "	,tb_clientes.cod_cliente "
			+ "	,cod_usuario "
			+ "	,tb_status_os.cod_status_os "
			+ "	,tb_status_os.nom_status_os "
			+ "	,num_os "
			+ "	,val_original "
			+ "	,val_final "
			+ "	,dat_abertura "
			+ "	,dat_prev_conclusao "
			+ "	,dat_fechamento "
			+ "	,tb_ordens_de_servico.txt_observacoes "
			+ "	,tb_ordens_de_servico.dat_cadastro "
			+ "	,tb_ordens_de_servico.dat_atualizacao "
			+ "FROM tb_ordens_de_servico "
			+ "INNER JOIN tb_status_os ON tb_status_os.cod_status_os = tb_ordens_de_servico.cod_status_os "
			+ "INNER JOIN tb_clientes ON tb_clientes.cod_cliente = tb_ordens_de_servico.cod_cliente ";

		private const String SELECT_ITENS
			= "SELECT "
			+ "	cod_item_os "
			+ "	,cod_tapete "
			+ "	,cod_ordem_de_servico "
			+ "	,flt_comprimento "
			+ "	,flt_largura "
			+ "	,dbl_area "
			+ " ,val_item "
			+ "	,txt_observacoes "
			+ "FROM tb_itens_os ";

		private const String SELECT_ITENS_SERVICOS
			= "SELECT "
			+ "	cod_item_servico "
			+ "	,cod_item_os "
			+ "	,cod_servico "
			+ "	,qtd_m_m2 "
			+ "	,val_item_servico "
			+ "FROM tb_itens_servicos ";
		#endregion

		#region UPDATE
		private const String UPDATE_OS
			= "UPDATE tb_ordens_de_servico SET "
			+ "	cod_cliente = @codCliente "
			+ "	,cod_usuario = @codUsuario "
			+ "	,cod_status_os = @codStatusOS "
			+ "	,num_os = @numOS "
			+ "	,val_original = @valOriginal "
			+ "	,val_final = @valFinal "
			+ "	,dat_abertura = @datAbertura "
			+ "	,dat_prev_conclusao = @datPrevConclusao "
			+ "	,dat_fechamento = @datFechamento "
			+ "	,txt_observacoes = @txtObservacoes "
			+ "	,dat_atualizacao = NOW() "
			+ "WHERE cod_ordem_de_servico = @codOrdemDeServico ";

		private const String UPDATE_ITENS
			= "UPDATE tb_itens_os SET "
			+ "	cod_tapete = @codTapete "
			+ "	,flt_comprimento = @fltComprimento "
			+ "	,flt_largura = @fltLargura "
			+ "	,dbl_area = @dblArea "
			+ "	,val_item = @valItem "
			+ "	,txt_observacoes = @txtObservacoes "
			+ "WHERE cod_item_os = @codItemOS AND cod_ordem_de_servico = @codOrdemDeServico ";

		private const String UPDATE_ITENS_SERVICOS
			= "UPDATE tb_itens_servicos SET "
			+ "	cod_servico = @codServico "
			+ "	,qtd_m_m2 = @qtdMm2 "
			+ "	,val_item_servico = @valItemServico "
			+ "WHERE cod_item_servico = @codItemServico AND cod_item_os = @codItemOS ";
		#endregion

		public static long count() {
			long count = 0;

			String sql = " SELECT COUNT(cod_ordem_de_servico) "
					+ " FROM tb_ordens_de_servico "; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );

			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static long count( List<Filter> filters ) {
			long count = 0;
			MySqlFilter mySqlFilter = getFilter( filters );

			String sql = " SELECT COUNT(cod_ordem_de_servico) "
					+ " FROM tb_ordens_de_servico "+ mySqlFilter.whereClause; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			if( mySqlFilter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( mySqlFilter.parametersList.ToArray() );
			}
			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static List<Erro> insert( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( OrdemDeServico os in ordensDeServico ) {

				MySqlCommand cmd = new MySqlCommand( INSERT_OS, conn );
				cmd.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = os.cliente.codigo;
				cmd.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = os.usuario.codigo;
				cmd.Parameters.Add( "@codStatusOS", MySqlDbType.UInt32 ).Value = os.status.codigo;
				cmd.Parameters.Add( "@numOS", MySqlDbType.UInt32 ).Value = os.numero;
				cmd.Parameters.Add( "@valOriginal", MySqlDbType.Double ).Value = os.valorOriginal;
				cmd.Parameters.Add( "@valFinal", MySqlDbType.Double ).Value = os.valorFinal;
				cmd.Parameters.Add( "@datAbertura", MySqlDbType.DateTime ).Value = os.dataDeAbertura;
				cmd.Parameters.Add( "@datPrevConclusao", MySqlDbType.DateTime ).Value = os.previsaoDeConclusao;
				cmd.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = os.observacoes;
				os.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
				cmd.Dispose();

				if( os.codigo <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível inserir a Ordem de Servi&ccedil;o: " + os.numero, "Tente inseri-la novamente" ) );
					continue;
				}

				foreach( Item item in os.itens ) {

					MySqlCommand cmd2 = new MySqlCommand( INSERT_ITENS, conn );
					cmd2.Parameters.Add( "@codTapete", MySqlDbType.UInt32 ).Value = item.tapete.codigo;
					cmd2.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = os.codigo;
					cmd2.Parameters.Add( "@fltComprimento", MySqlDbType.Float ).Value = item.comprimento;
					cmd2.Parameters.Add( "@fltLargura", MySqlDbType.Float ).Value = item.largura;
					cmd2.Parameters.Add( "@dblArea", MySqlDbType.Double ).Value = item.area;
					cmd2.Parameters.Add( "@valItem", MySqlDbType.Double ).Value = item.valor;
					cmd2.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = item.observacoes;
					item.codigo = UInt32.Parse( cmd2.ExecuteScalar().ToString() );
					cmd2.Dispose();

					if( item.codigo <= 0 ) {
						erros.Add( new Erro( 0, "Não foi possível inserir o item: " + item.tapete.nome, "Tente inseri-lo novamente" ) );
						continue;
					}


					foreach( ServicoDoItem servItem in item.servicosDoItem ) {

						MySqlCommand cmd3 = new MySqlCommand( INSERT_ITENS_SERVICOS, conn );
						cmd3.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = item.codigo;
						cmd3.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servItem.servico.codigo;
						cmd3.Parameters.Add( "@qtdMm2", MySqlDbType.Double ).Value = servItem.quantidade_m_m2;
						cmd3.Parameters.Add( "@valItemServico", MySqlDbType.Double ).Value = servItem.valor;
						servItem.codigo = UInt32.Parse( cmd3.ExecuteScalar().ToString() );
						cmd3.Dispose();

						if( servItem.codigo <= 0 ) {
							erros.Add( new Erro( 0, "Não foi possível inserir o servi&ccedil;o: " + servItem.servico.nome + " do item: " + item.tapete.nome, "Tente inseri-lo novamente" ) );
						}
					}
				}
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static OrdemDeServico selectByCod( UInt32 codigo ) {
			List<Filter> filters = new List<Filter>();
			filters.Add( new Filter( "codigo", codigo.ToString() ) );

			List<OrdemDeServico> osAux = select( filters );
			if( osAux.Count > 0 ) {
				return osAux[0];
			} else {
				return new OrdemDeServico();
			}
		}

		public static OrdemDeServico selectByNum( UInt32 numero ) {
			List<Filter> filters = new List<Filter>();
			filters.Add( new Filter( "numero", numero.ToString() ) );

			List<OrdemDeServico> osAux = select( filters );
			if( osAux.Count > 0 ) {
				return osAux[0];
			} else {
				return new OrdemDeServico();
			}
		}

		public static List<OrdemDeServico> select( List<Filter> filters ) {
			return select( 0, 0, filters, new List<Sorter>() );
		}

		public static List<OrdemDeServico> select( UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<OrdemDeServico> ordensDeServico = new List<OrdemDeServico>();
			Cliente cliAux;
			Usuario usuAux;
			
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlConnection connAux = MySqlConnectionWizard.getConnection();

			conn.Open();
			connAux.Open();

			MySqlFilter mySqlFilter = getFilter( filters );

			MySqlCommand cmd = new MySqlCommand( SELECT_OS + mySqlFilter.whereClause + getSort(sorters) 
				+ ((limit > 0) ? " LIMIT " + start + "," + limit : ""), conn );
			
			if( mySqlFilter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( mySqlFilter.parametersList.ToArray() );
			}

			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				OrdemDeServico os = new OrdemDeServico();

				os.codigo = reader.GetUInt32( "cod_ordem_de_servico" );
				os.status.codigo = reader.GetUInt32( "cod_status_os" );
				os.status.nome = reader.GetString( "nom_status_os" );
				os.numero = reader.GetUInt32( "num_os" );
				os.valorOriginal = reader.GetDouble( "val_original" );
				os.valorFinal = reader.GetDouble( "val_final" );
				os.dataDeAbertura = reader.GetDateTime( "dat_abertura" );
				os.previsaoDeConclusao = reader.GetDateTime( "dat_prev_conclusao" );
				try { os.dataDeFechamento = reader.GetDateTime( "data_fechamento" ); } catch { }
				try { os.observacoes = reader.GetString( "txt_observacoes" ); } catch { }

				os.dataDeCadastro = reader.GetDateTime( "dat_cadastro" );
				os.dataDeAtualizacao = reader.GetDateTime( "dat_atualizacao" );

				cliAux = os.cliente;
				MySqlClientesDao.preencherCliente( reader.GetUInt32( "cod_cliente" ), ref cliAux, connAux );
				usuAux = os.usuario;
				MySqlUsuariosDao.preencherUsuario( reader.GetUInt32( "cod_usuario" ), ref usuAux, connAux );
				List<Item> itensAux = os.itens;
				MySqlOrdensDeServicoDao.preencherItens( os.codigo, ref itensAux, connAux );

				ordensDeServico.Add( os );
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();
			connAux.Close(); connAux.Dispose();

			return ordensDeServico;
		}

		public static List<Erro> update( ref List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( OrdemDeServico os in ordensDeServico ) {

				#region ATUALIZA OS
				// so atualiza OS com status ATIVO
				MySqlCommand cmd1 = new MySqlCommand( UPDATE_OS + " AND cod_status_os = 1", conn );
				cmd1.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = os.codigo;
				cmd1.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = os.cliente.codigo;
				cmd1.Parameters.Add( "@codUsuario", MySqlDbType.UInt32 ).Value = os.usuario.codigo;
				cmd1.Parameters.Add( "@codStatusOS", MySqlDbType.UInt32 ).Value = os.status.codigo;
				cmd1.Parameters.Add( "@numOS", MySqlDbType.UInt32 ).Value = os.numero;
				cmd1.Parameters.Add( "@valOriginal", MySqlDbType.Double ).Value = os.valorOriginal;
				cmd1.Parameters.Add( "@valFinal", MySqlDbType.Double ).Value = os.valorFinal;
				cmd1.Parameters.Add( "@datAbertura", MySqlDbType.Timestamp ).Value = os.dataDeAbertura;
				cmd1.Parameters.Add( "@datPrevConclusao", MySqlDbType.Timestamp ).Value = os.previsaoDeConclusao;
				if(os.dataDeFechamento.CompareTo(DateTime.MinValue) > 0) {
					cmd1.Parameters.Add( "@datFechamento", MySqlDbType.Timestamp ).Value = os.dataDeFechamento;
				} else {
					cmd1.Parameters.Add( "@datFechamento", MySqlDbType.Timestamp ).Value = DBNull.Value;
				}
				cmd1.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = os.observacoes;

				if( cmd1.ExecuteNonQuery() == 0 ) {
					erros.Add( new Erro( 0, "Não foi possível atualizar a Ordem de Servi&ccedil;o: " + os.numero, "Tente atualiza-la novamente, mas lembre-se que n&atilde;o &eacute; poss&iacute;vel alterar Ordens de Servi&ccedil;o com Status <b>Finalizado</b> ou <b>Cancelado</b>" ) );
					continue;
				}
				cmd1.Dispose();
				#endregion

				#region ATUALIZA ITENS

				List<UInt32> codItens = new List<UInt32>();

				foreach( Item item in os.itens ) {

					MySqlCommand cmd2 = new MySqlCommand();
					cmd2.Connection = conn;

					cmd2.Parameters.Add( "@codTapete", MySqlDbType.UInt32 ).Value = item.tapete.codigo;
					cmd2.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = os.codigo;
					cmd2.Parameters.Add( "@fltComprimento", MySqlDbType.Float ).Value = item.comprimento;
					cmd2.Parameters.Add( "@fltLargura", MySqlDbType.Float ).Value = item.largura;
					cmd2.Parameters.Add( "@dblArea", MySqlDbType.Double ).Value = item.area;
					cmd2.Parameters.Add( "@valItem", MySqlDbType.Double ).Value = item.valor;
					cmd2.Parameters.Add( "@txtObservacoes", MySqlDbType.VarChar ).Value = item.observacoes;

					if( item.codigo == 0 ) {
						cmd2.CommandText = INSERT_ITENS;
						item.codigo = UInt32.Parse( cmd2.ExecuteScalar().ToString() );
					} else {
						cmd2.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = item.codigo;
						cmd2.CommandText = UPDATE_ITENS;
						cmd2.ExecuteNonQuery();
					}
					cmd2.Dispose();


					if( item.codigo == 0 ) {
						erros.Add( new Erro( 0, "Não foi possível atualizar o item de tapete: " + item.tapete.nome + " da Ordem de Servi&ccedil;o: " + os.numero, "Tente atualiza-lo novamente" ) );
						continue;
					} else {
						codItens.Add( item.codigo );
					}

					#region ATUALIZA SERVICOS DOS ITENS

					List<UInt32> codServItens = new List<UInt32>();

					foreach( ServicoDoItem servItem in item.servicosDoItem ) {
						MySqlCommand cmd3 = new MySqlCommand();
						cmd3.Connection = conn;

						cmd3.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servItem.servico.codigo;
						cmd3.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = item.codigo;
						cmd3.Parameters.Add( "@qtdMm2", MySqlDbType.Double ).Value = servItem.quantidade_m_m2;
						cmd3.Parameters.Add( "@valItemServico", MySqlDbType.Double ).Value = servItem.valor;

						if( servItem.codigo == 0 ) {
							cmd3.CommandText = INSERT_ITENS_SERVICOS;
							servItem.codigo = UInt32.Parse( cmd3.ExecuteScalar().ToString() );
						} else {
							cmd3.Parameters.Add( "@codItemServico", MySqlDbType.UInt32 ).Value = servItem.codigo;
							cmd3.CommandText = UPDATE_ITENS_SERVICOS;
							cmd3.ExecuteNonQuery();
						}
						cmd3.Dispose();


						if( servItem.codigo == 0 ) {
							erros.Add( new Erro( 0, "Não foi possível atualizar o servi&ccedil;o: " + servItem.servico.nome + " do item de tapete: " + item.tapete.nome + " da Ordem de Servi&ccedil;o: " + os.numero, "Tente atualiza-lo novamente" ) );
						} else {
							codServItens.Add( servItem.codigo );
						}
					}

					#region EXCLUI SERVICOS NAO USADOS
					MySqlCommand cmdDelServ = new MySqlCommand();
					cmdDelServ.Connection = conn;
					cmdDelServ.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = item.codigo;
					if( codServItens.Count > 0 ) {
						StringBuilder codigos = new StringBuilder();
						for( int i = 0; i < codServItens.Count; i++ ) {
							String param = "@codItemServico_" + i;
							codigos.Append( param + "," );
							cmdDelServ.Parameters.Add( new MySqlParameter( param, MySqlDbType.UInt32 ) ).Value = codServItens[i];
						}
						codigos.Replace( ",", "", codigos.Length - 1, 1 );// remove a ultima virgula

						cmdDelServ.CommandText = "DELETE FROM tb_itens_servicos WHERE cod_item_os = @codItemOS AND cod_item_servico NOT IN ( " + codigos.ToString() + " )";
					} else {
						cmdDelServ.CommandText = "DELETE FROM tb_itens_servicos WHERE cod_item_os = @codItemOS";
					}
					cmdDelServ.ExecuteNonQuery();
					cmdDelServ.Dispose();
					#endregion

					#endregion
				}

				#region EXCLUI ITENS NAO USADOS
				MySqlCommand cmdDelItens = new MySqlCommand();
				cmdDelItens.Connection = conn;
				cmdDelItens.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = os.codigo;
				if( codItens.Count > 0 ) {
					StringBuilder codigos = new StringBuilder();
					for( int i = 0; i < codItens.Count; i++ ) {
						String param = "@codItemOS_" + i;
						codigos.Append( param + "," );
						cmdDelItens.Parameters.Add( new MySqlParameter( param, MySqlDbType.UInt32 ) ).Value = codItens[i];
					}
					codigos.Replace( ",", "", codigos.Length - 1, 1 );// remove a ultima virgula

					cmdDelItens.CommandText = "DELETE FROM tb_itens_os WHERE cod_ordem_de_servico = @codOrdemDeServico AND cod_item_os NOT IN ( " + codigos.ToString() + " )";
				} else {
					cmdDelItens.CommandText = "DELETE FROM tb_itens_os WHERE cod_ordem_de_servico = @codOrdemDeServico";
				}
				cmdDelItens.ExecuteNonQuery();
				cmdDelItens.Dispose();
				#endregion

				#endregion
			}

			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> delete( List<OrdemDeServico> ordensDeServico ) {
			List<Erro> erros = new List<Erro>();
			// so podem ser excluidas as ordens de servico que nao foram canceladas ou finalizadas
			String sql = "DELETE FROM tb_ordens_de_servico WHERE cod_ordem_de_servico = @codOrdemDeServico AND cod_status_os = 1 ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( OrdemDeServico ordem in ordensDeServico ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = ordem.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir a Ordem de Servi&ccedil;o: " + ordem.numero, "Tente excluí-la novamente, mas lembre-se que n&atilde;o &eacute; poss&iacute;vel excluir ordens de servi&ccedil;o que j&aacute; foram <b>canceladas</b> ou <b>finalizadas</b>" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Status> selectStatus() {
			List<Status> statusList = new List<Status>();
			StringBuilder sql = new StringBuilder();

			sql.Append( "SELECT " );
			sql.Append( "	cod_status_os " );
			sql.Append( "	,nom_status_os " );
			sql.Append( "FROM tb_status_os " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			conn.Open();
			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				statusList.Add( new Status( reader.GetUInt32( 0 ), reader.GetString( 1 ) ) );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return statusList;
		}

		public static Status selectStatus(UInt32 codigoOS ) {
			Status status = new Status();
			StringBuilder sql = new StringBuilder();

			sql.Append( "SELECT " );
			sql.Append( "	tb_status_os.cod_status_os " );
			sql.Append( "	,tb_status_os.nom_status_os " );
			sql.Append( "FROM tb_status_os " );
			sql.Append( "INNER JOIN tb_ordens_de_servico ON tb_ordens_de_servico.cod_status_os = tb_status_os.cod_status_os " );
			sql.Append( "WHERE tb_ordens_de_servico.cod_ordem_de_servico = @codOS " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@codOS", MySqlDbType.UInt32 ).Value = codigoOS;

			conn.Open();
			MySqlDataReader reader = cmd.ExecuteReader();

			while( reader.Read() ) {
				status.codigo = reader.GetUInt32( 0 );
				status.nome = reader.GetString( 1 );
			}

			reader.Close(); reader.Dispose();
			cmd.Dispose();
			conn.Close(); conn.Dispose();

			return status;
		}

		public static void preencherItens( UInt32 codigoOrdemDeServico, ref List<Item> itens, MySqlConnection conn ) {

			MySqlCommand cmd = new MySqlCommand( SELECT_ITENS + " WHERE cod_ordem_de_servico = @codOrdemDeServico", conn );
			cmd.Parameters.Add( "@codOrdemDeServico", MySqlDbType.UInt32 ).Value = codigoOrdemDeServico;
			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				Item item = new Item();
				item.codigo = reader.GetUInt32("cod_item_os");
				item.codigoOrdemDeServico = codigoOrdemDeServico;
				item.tapete.codigo = reader.GetUInt32("cod_tapete");
				item.comprimento = reader.GetFloat("flt_comprimento");
				item.largura = reader.GetFloat("flt_largura");
				item.valor = reader.GetDouble( "val_item" );
				try { item.observacoes = reader.GetString("txt_observacoes"); } catch{}
				
				itens.Add(item);
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();

			foreach(Item item in itens) {
				Tapete tapAux = item.tapete;
				MySqlTapetesDao.preencherTapete( item.tapete.codigo, ref tapAux, conn );

				List<ServicoDoItem> servicosDoItemAux = item.servicosDoItem;
				preencherServicosDoItem( item.codigo, ref servicosDoItemAux, conn);
			}

		}

		public static void preencherServicosDoItem( UInt32 codigoItem, ref List<ServicoDoItem> servicosDoItem, MySqlConnection conn ) {

			MySqlCommand cmd = new MySqlCommand( SELECT_ITENS_SERVICOS + " WHERE cod_item_os = @codItemOS", conn );
			cmd.Parameters.Add( "@codItemOS", MySqlDbType.UInt32 ).Value = codigoItem;
			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				ServicoDoItem servicoDoItem = new ServicoDoItem();
			
				servicoDoItem.codigo = reader.GetUInt32("cod_item_servico");
				servicoDoItem.codigoItem = codigoItem;
				servicoDoItem.servico.codigo = reader.GetUInt32("cod_servico");
				servicoDoItem.quantidade_m_m2 = reader.GetDouble("qtd_m_m2");
				servicoDoItem.valor = reader.GetDouble("val_item_servico");

				servicosDoItem.Add(servicoDoItem);
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();

			foreach(ServicoDoItem servicoDoItem in servicosDoItem) {
				Servico servAux = servicoDoItem.servico;
				MySqlServicosDao.preencherServico( servicoDoItem.servico.codigo, ref servAux, conn, false );
			}
		}

		public static bool numeroJaExiste( UInt32 numero ) {
			return numeroJaExiste( numero, 0 );
		}

		public static bool numeroJaExiste( UInt32 numero, UInt32 codOSDesconsiderado ) {
			bool existe = false;

			String sql = "SELECT num_os FROM tb_ordens_de_servico WHERE num_os = @numOS AND cod_ordem_de_servico <> @codOS LIMIT 1";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );
			cmd.Parameters.Add( "@numOS", MySqlDbType.UInt32 ).Value = numero;
			cmd.Parameters.Add( "@codOS", MySqlDbType.UInt32 ).Value = codOSDesconsiderado;

			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();
			if( reader.Read() ) {
				existe = true;
			}
			reader.Close(); reader.Dispose();
			cmd.Dispose();

			conn.Close();
			conn.Dispose();

			return existe;
		}

		public static MySqlFilter getFilter( List<Filter> filters ) {

			// clausula WHERE SQL
			StringBuilder whereClause = new StringBuilder();
			List<String> filterList = new List<String>();
			MySqlFilter mySqlfilter = new MySqlFilter();

			// se nao houver filtros, retorna uma string vazia
			if( filters.Count <= 0 ) return mySqlfilter;

			#region filtros

			foreach( Filter filter in filters ) {

				switch( filter.property ) {
					case "codigo":
						UInt32 codigo = 0;
						UInt32.TryParse( filter.value, out codigo );
						if( codigo > 0 ) {
							filterList.Add( "tb_ordens_de_servico.cod_ordem_de_servico = @codOS" );
							MySqlParameter codOS = new MySqlParameter( "@codOS", MySqlDbType.UInt32 );
							codOS.Value = codigo;
							mySqlfilter.parametersList.Add( codOS );
						}
						break;

					case "numero":
						UInt32 numero = 0;
						UInt32.TryParse( filter.value, out numero );
						if( numero > 0 ) {
							filterList.Add( "tb_ordens_de_servico.num_os = @numOS" );
							MySqlParameter numOS = new MySqlParameter( "@numOS", MySqlDbType.UInt32 );
							numOS.Value = numero;
							mySqlfilter.parametersList.Add( numOS );
						}
						break;

					case "codigoStatus":
						UInt32 codigoStatus = 0;
						UInt32.TryParse( filter.value, out codigoStatus );
						if( codigoStatus > 0 ) {
							filterList.Add( "tb_status_os.cod_status_os = @codStatusOS" );
							MySqlParameter codStatus = new MySqlParameter( "@codStatusOS", MySqlDbType.UInt32 );
							codStatus.Value = codigoStatus;
							mySqlfilter.parametersList.Add( codStatus );
						}
						break;

					case "valorOriginal":
						Double valorOriginal = 0;
						Double.TryParse( filter.value, out valorOriginal );
						if( valorOriginal > 0 ) {
							filterList.Add( "tb_ordens_de_servico.val_original = @val_original" );
							MySqlParameter valOriginal = new MySqlParameter( "@val_original", MySqlDbType.Double );
							valOriginal.Value = valorOriginal;
							mySqlfilter.parametersList.Add( valOriginal );
						}
						break;

					case "valorFinal":
						Double valorFinal = 0;
						Double.TryParse( filter.value, out valorFinal );
						if( valorFinal > 0 ) {
							filterList.Add( "tb_ordens_de_servico.val_final = @valFinal" );
							MySqlParameter valFinal = new MySqlParameter( "@valFinal", MySqlDbType.Double );
							valFinal.Value = valorFinal;
							mySqlfilter.parametersList.Add( valFinal );
						}
						break;

					case "dataDeAbertura":
						try {
							DateTime dataDeAbertura = DateTime.Parse( filter.value );
							filterList.Add( "tb_ordens_de_servico.dat_abertura = @datAbertura" );
							MySqlParameter datAbertura = new MySqlParameter( "@datAbertura", MySqlDbType.Timestamp );
							datAbertura.Value = dataDeAbertura;
							mySqlfilter.parametersList.Add( datAbertura );
						} catch { }
						break;

					case "previsaoDeConclusao":
						try {
							DateTime previsaoDeConclusao = DateTime.Parse( filter.value );
							filterList.Add( "tb_ordens_de_servico.dat_prev_conclusao = @datPrevConclusao" );
							MySqlParameter prevConclusao = new MySqlParameter( "@datPrevConclusao", MySqlDbType.Timestamp );
							prevConclusao.Value = previsaoDeConclusao;
							mySqlfilter.parametersList.Add( prevConclusao );
						} catch { }
						break;

					case "dataDeFechamento":
						try {
							DateTime dataDeFechamento = DateTime.Parse( filter.value );
							filterList.Add( "tb_ordens_de_servico.dat_fechamento = @datFechamento" );
							MySqlParameter datFechamento = new MySqlParameter( "@datFechamento", MySqlDbType.Timestamp );
							datFechamento.Value = dataDeFechamento;
							mySqlfilter.parametersList.Add( datFechamento );
						} catch { }
						break;

					case "codigoCliente":
						UInt32 codigoCliente = 0;
						UInt32.TryParse( filter.value, out codigoCliente );
						if( codigoCliente > 0 ) {
							filterList.Add( "tb_clientes.cod_cliente = @codCliente" );
							MySqlParameter codCliente = new MySqlParameter( "@codCliente", MySqlDbType.UInt32 );
							codCliente.Value = codigoCliente;
							mySqlfilter.parametersList.Add( codCliente );
						}
						break;

					case "nomeCliente":
						if( !String.IsNullOrEmpty( filter.value ) ) {
							filterList.Add( "tb_clientes.nom_cliente LIKE @nomCliente" );
							MySqlParameter nomCliente = new MySqlParameter( "@nomCliente", MySqlDbType.VarChar );
							nomCliente.Value = filter.value;
							mySqlfilter.parametersList.Add( nomCliente );
						}
						break;
				}
			}
			#endregion

			#region constroi a clausula WHERE
			for( int i = 0; i < filterList.Count; i++ ) {
				whereClause.Append( filterList[i] );
				// adiciona o condicional AND caso tenha mais filtros
				if( i < ( filterList.Count - 1 ) ) whereClause.Append( " AND " );
			}

			if( filterList.Count > 0 ) {
				// adiciona o conteudo do stringBuilder dentro do objeto sqlFilter
				mySqlfilter.whereClause = " WHERE " + whereClause.ToString() + " ";
			}
			#endregion

			return mySqlfilter;
		}

		public static String getSort( List<Sorter> sorters ) {
			StringBuilder sortSql = new StringBuilder();

			foreach( Sorter sorter in sorters ) {
				switch( sorter.property ) {
					case "codigo":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.cod_ordem_de_servico", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "numero":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.num_os", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "valorOriginal":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.val_original", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "valorFinal":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.val_final", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "dataDeAbertura":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.dat_abertura", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "previsaoDeConclusao":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.dat_prev_conclusao", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "dataDeFechamento":
						sortSql.AppendFormat( "{0} {1},", "tb_ordens_de_servico.dat_fechamento", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "nomeStatus":
						sortSql.AppendFormat( "{0} {1},", "tb_status_os.nom_status_os", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "codigoCliente":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.cod_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "nomeCliente":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.nom_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
				}
			}
			if( sorters.Count > 0 ) sortSql.Remove( sortSql.Length - 1, 1 );

			return ( ( sortSql.ToString().Trim().Length > 0 ) ? " ORDER BY " + sortSql.ToString() : "" );
		}
	}
}
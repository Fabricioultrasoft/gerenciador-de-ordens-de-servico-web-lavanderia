using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using System.Text;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes {
	public class MySqlClientesDao {

		#region SQL
		private const String SQL_CLIENTE
			= "SELECT "
			+ "     cod_cliente "
			+ "    ,nom_cliente "
			+ "    ,nom_conjuge "
			+ "    ,tb_tipos_clientes.cod_tipo_cliente "
			+ "    ,tb_tipos_clientes.nom_tipo_cliente "
			+ "    ,int_sexo "
			+ "    ,dat_nascimento "
			+ "    ,txt_rg "
			+ "    ,txt_cpf "
			+ "    ,txt_observacoes "
			+ "    ,tb_clientes.flg_ativo "
			+ "    ,dat_cadastro "
			+ "    ,dat_atualizacao "
			+ "FROM tb_clientes "
			+ "INNER JOIN tb_tipos_clientes ON tb_tipos_clientes.cod_tipo_cliente = tb_clientes.cod_tipo_cliente "
			+ "WHERE cod_cliente = @codCliente";

		private const String SQL_MEIOS_DE_CONTATO 
			= "SELECT "
			+ "     cod_meio_de_contato "
			+ "    ,tb_tipos_meios_de_contato.cod_tipo_meio_de_contato "
			+ "    ,tb_tipos_meios_de_contato.nom_tipo_meio_de_contato "
			+ "    ,txt_meio_de_contato "
			+ "    ,int_meio_de_contato "
			+ "    ,txt_descricao "
			+ "FROM tb_meios_de_contato "
			+ "INNER JOIN tb_tipos_meios_de_contato ON tb_tipos_meios_de_contato.cod_tipo_meio_de_contato = tb_meios_de_contato.cod_tipo_meio_de_contato "
			+ "WHERE cod_cliente = @codCliente";

		private const String SQL_ENDERECOS
			= "SELECT "
			+ "     cod_endereco "
			+ "    ,cod_logradouro "
			+ "    ,txt_complemento "
			+ "    ,txt_ponto_referencia "
			+ "    ,int_numero "
			+ "FROM tb_enderecos "
			+ "WHERE cod_cliente = @codCliente";

		#endregion

		public static long countClientes() {
			long count = 0;

			String sql = " SELECT COUNT(cod_cliente) "
					+ " FROM tb_clientes "; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );

			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static long countClientes( List<Filter> filters ) {
			long count = 0;
			MySqlFilter mySqlFilter = createMySqlFilter( filters );

			String sql = " SELECT COUNT(cod_cliente) "
					+ " FROM tb_clientes "+ mySqlFilter.whereClause; // Filtro

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

		public static List<Erro> inserirListaDeClientes( ref List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sqlCliente = new StringBuilder();
			StringBuilder sqlMeioDeContato = new StringBuilder();
			StringBuilder sqlEndereco = new StringBuilder();

			sqlCliente.AppendLine( "INSERT INTO tb_clientes(" );
			sqlCliente.AppendLine( "	 cod_tipo_cliente " );
			//sqlCliente.AppendLine( "	,cod_usuario " );
			sqlCliente.AppendLine( "	,nom_cliente " );
			sqlCliente.AppendLine( "	,nom_conjuge " );
			sqlCliente.AppendLine( "	,int_sexo " );
			sqlCliente.AppendLine( "	,dat_nascimento " );
			sqlCliente.AppendLine( "	,txt_rg " );
			sqlCliente.AppendLine( "	,txt_cpf " );
			sqlCliente.AppendLine( "	,txt_observacoes " );
			sqlCliente.AppendLine( "	,flg_ativo " );
			sqlCliente.AppendLine( "	,dat_atualizacao " );
			sqlCliente.AppendLine( ") " );
			sqlCliente.AppendLine( "VALUES(" );
			sqlCliente.AppendLine( "	 @cod_tipo_cliente " );
			//sqlCliente.AppendLine( "	,@cod_usuario " );
			sqlCliente.AppendLine( "	,@nom_cliente " );
			sqlCliente.AppendLine( "	,@nom_conjuge " );
			sqlCliente.AppendLine( "	,@int_sexo " );
			sqlCliente.AppendLine( "	,@dat_nascimento " );
			sqlCliente.AppendLine( "	,@txt_rg " );
			sqlCliente.AppendLine( "	,@txt_cpf " );
			sqlCliente.AppendLine( "	,@txt_observacoes " );
			sqlCliente.AppendLine( "	,@flg_ativo " );
			sqlCliente.AppendLine( "	,NOW() " );
			sqlCliente.AppendLine( "); " );
			sqlCliente.AppendLine( "SELECT LAST_INSERT_ID()" );
			//-------------------------------------------------------------------------------------
			sqlMeioDeContato.AppendLine( "INSERT INTO tb_meios_de_contato(" );
			sqlMeioDeContato.AppendLine( "	 cod_cliente " );
			sqlMeioDeContato.AppendLine( "	,cod_tipo_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,txt_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,int_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,txt_descricao " );
			sqlMeioDeContato.AppendLine( ") " );
			sqlMeioDeContato.AppendLine( "VALUES(" );
			sqlMeioDeContato.AppendLine( "	 @cod_cliente " );
			sqlMeioDeContato.AppendLine( "	,@cod_tipo_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,@txt_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,@int_meio_de_contato " );
			sqlMeioDeContato.AppendLine( "	,@txt_descricao " );
			sqlMeioDeContato.AppendLine( "); " );
			sqlMeioDeContato.AppendLine( "SELECT LAST_INSERT_ID()" );
			//-------------------------------------------------------------------------------------
			sqlEndereco.AppendLine( "INSERT INTO tb_enderecos(" );
			sqlEndereco.AppendLine( "	 cod_cliente " );
			sqlEndereco.AppendLine( "	,cod_logradouro " );
			sqlEndereco.AppendLine( "	,txt_complemento " );
			sqlEndereco.AppendLine( "	,txt_ponto_referencia " );
			sqlEndereco.AppendLine( "	,int_numero " );
			sqlEndereco.AppendLine( ") " );
			sqlEndereco.AppendLine( "VALUES(" );
			sqlEndereco.AppendLine( "	 @cod_cliente " );
			sqlEndereco.AppendLine( "	,@cod_logradouro " );
			sqlEndereco.AppendLine( "	,@txt_complemento " );
			sqlEndereco.AppendLine( "	,@txt_ponto_referencia " );
			sqlEndereco.AppendLine( "	,@int_numero " );
			sqlEndereco.AppendLine( "); " );
			sqlEndereco.AppendLine( "SELECT LAST_INSERT_ID()" );


			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cliente cliente in clientes ) {
				MySqlCommand cmdCliente = new MySqlCommand( sqlCliente.ToString(), conn );
				cmdCliente.Parameters.Add( "@cod_tipo_cliente", MySqlDbType.UInt32 ).Value = cliente.tipoDeCliente.codigo;
				//cmdCliente.Parameters.Add( "@cod_usuario", MySqlDbType.UInt32 ).Value = cliente.usuario.codigo;
				cmdCliente.Parameters.Add( "@nom_cliente", MySqlDbType.VarChar ).Value = cliente.nome;
				cmdCliente.Parameters.Add( "@int_sexo", MySqlDbType.Int16 ).Value = cliente.sexo;
				cmdCliente.Parameters.Add( "@flg_ativo", MySqlDbType.Bit ).Value = cliente.ativo;

				if( String.IsNullOrEmpty( cliente.conjuge.Trim() ) == false ) {
					cmdCliente.Parameters.Add( "@nom_conjuge", MySqlDbType.VarChar ).Value = cliente.conjuge;
				} else {
					cmdCliente.Parameters.Add( "@nom_conjuge", MySqlDbType.VarChar ).Value = DBNull.Value;
				}

				if( cliente.dataDeNascimento != null ) {
					cmdCliente.Parameters.Add( "@dat_nascimento", MySqlDbType.Timestamp ).Value = cliente.dataDeNascimento.Value.ToString( "yyyy-MM-dd" );
				} else {
					cmdCliente.Parameters.Add( "@dat_nascimento", MySqlDbType.Timestamp ).Value = DBNull.Value;
				}

				if( String.IsNullOrEmpty( cliente.rg.Trim() ) == false ) {
					cmdCliente.Parameters.Add( "@txt_rg", MySqlDbType.VarChar ).Value = cliente.rg;
				} else {
					cmdCliente.Parameters.Add( "@txt_rg", MySqlDbType.VarChar ).Value = DBNull.Value;
				}

				if( String.IsNullOrEmpty( cliente.cpf.Trim() ) == false ) {
					cmdCliente.Parameters.Add( "@txt_cpf", MySqlDbType.VarChar ).Value = cliente.cpf;
				} else {
					cmdCliente.Parameters.Add( "@txt_cpf", MySqlDbType.VarChar ).Value = DBNull.Value;
				}

				if( String.IsNullOrEmpty( cliente.observacoes.Trim() ) == false ) {
					cmdCliente.Parameters.Add( "@txt_observacoes", MySqlDbType.VarChar ).Value = cliente.observacoes;
				} else {
					cmdCliente.Parameters.Add( "@txt_observacoes", MySqlDbType.VarChar ).Value = DBNull.Value;
				}


				cliente.codigo = UInt32.Parse( cmdCliente.ExecuteScalar().ToString() );
				cmdCliente.Dispose();

				// registra se o tapete foi inserido ou nao
				if( cliente.codigo <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível inserir o cliente: " + cliente.nome, "Tente inseri-lo novamente" ) );
					continue;
				}

				foreach( MeioDeContato meioDeContato in cliente.meiosDeContato ) {

					MySqlCommand cmdMeioDeContato = new MySqlCommand( sqlMeioDeContato.ToString(), conn );
					cmdMeioDeContato.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
					cmdMeioDeContato.Parameters.Add( "@cod_tipo_meio_de_contato", MySqlDbType.UInt32 ).Value = meioDeContato.tipoDeContato.codigo;
					cmdMeioDeContato.Parameters.Add( "@txt_meio_de_contato", MySqlDbType.VarChar ).Value = meioDeContato.contato;
					if( meioDeContato.intContato > 0 ) {
						cmdMeioDeContato.Parameters.Add( "@int_meio_de_contato", MySqlDbType.UInt32 ).Value = meioDeContato.intContato;
					} else {
						cmdMeioDeContato.Parameters.Add( "@int_meio_de_contato", MySqlDbType.UInt32 ).Value = DBNull.Value;
					}
					if( String.IsNullOrEmpty( meioDeContato.descricao.Trim() ) == false ) {
						cmdMeioDeContato.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = meioDeContato.descricao;
					} else {
						cmdMeioDeContato.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = DBNull.Value;
					}

					meioDeContato.codigo = UInt32.Parse( cmdMeioDeContato.ExecuteScalar().ToString() );
					cmdMeioDeContato.Dispose();

					if( meioDeContato.codigo <= 0 )
						erros.Add( new Erro( 0, "Não foi possível inserir o meio de contato: " + meioDeContato.contato, "Tente inseri-lo novamente" ) );
				}

				foreach( Endereco endereco in cliente.enderecos ) {

					MySqlCommand cmdEndereco = new MySqlCommand( sqlEndereco.ToString(), conn );
					cmdEndereco.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
					cmdEndereco.Parameters.Add( "@cod_logradouro", MySqlDbType.UInt32 ).Value = endereco.logradouro.codigo;
					if( String.IsNullOrEmpty( endereco.complemento.Trim() ) == false ) {
						cmdEndereco.Parameters.Add( "@txt_complemento", MySqlDbType.VarChar ).Value = endereco.complemento;
					} else {
						cmdEndereco.Parameters.Add( "@txt_complemento", MySqlDbType.VarChar ).Value = DBNull.Value;
					}
					if( String.IsNullOrEmpty( endereco.pontoDeReferencia.Trim() ) == false ) {
						cmdEndereco.Parameters.Add( "@txt_ponto_referencia", MySqlDbType.VarChar ).Value = endereco.pontoDeReferencia;
					} else {
						cmdEndereco.Parameters.Add( "@txt_ponto_referencia", MySqlDbType.VarChar ).Value = DBNull.Value;
					}
					if( endereco.numero > 0 ) {
						cmdEndereco.Parameters.Add( "@int_numero", MySqlDbType.UInt32 ).Value = endereco.numero;
					} else {
						cmdEndereco.Parameters.Add( "@int_numero", MySqlDbType.UInt32 ).Value = DBNull.Value;
					}

					endereco.codigo = UInt32.Parse( cmdEndereco.ExecuteScalar().ToString() );
					cmdEndereco.Dispose();

					if( endereco.codigo <= 0 )
						erros.Add( new Erro( 0, "Não foi possível inserir o endere&ccedil;o: " + endereco.logradouro.nome + ", " + endereco.numero, "Tente inseri-lo novamente" ) );
				}
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static Cliente getCliente( UInt32 codigo ) {
			Cliente cliente;

			MySqlConnection connCliente = MySqlConnectionWizard.getConnection();
			connCliente.Open();
			MySqlCommand cmdCliente = new MySqlCommand( SQL_CLIENTE, connCliente );
			cmdCliente.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = codigo;


			MySqlDataReader readerCliente = cmdCliente.ExecuteReader();
			if( readerCliente.Read() ) {
				cliente = getCliente( readerCliente );
			} else {
				cliente = new Cliente();
			}
			readerCliente.Close(); readerCliente.Dispose(); cmdCliente.Dispose();
			connCliente.Close(); connCliente.Dispose();

			return cliente;
		}

		private static Cliente getCliente( MySqlDataReader readerCliente ) {
			Cliente cliente = new Cliente();

			cliente.codigo = readerCliente.GetUInt32( "cod_cliente" );
			try { cliente.nome = readerCliente.GetString( "nom_cliente" ); } catch { }
			try { cliente.conjuge = readerCliente.GetString( "nom_conjuge" ); } catch { }
			try { cliente.tipoDeCliente.codigo = readerCliente.GetUInt32( "cod_tipo_cliente" ); } catch { }
			try { cliente.tipoDeCliente.nome = readerCliente.GetString( "nom_tipo_cliente" ); } catch { }
			try { cliente.sexo = (Sexo) Enum.Parse( typeof( Sexo ), readerCliente.GetUInt32( "int_sexo" ).ToString(), true ); } catch { }
			try { cliente.dataDeNascimento = readerCliente.GetDateTime( "dat_nascimento" ); } catch { }
			try { cliente.rg = readerCliente.GetString( "txt_rg" ); } catch { }
			try { cliente.cpf = readerCliente.GetString( "txt_cpf" ); } catch { }
			try { cliente.observacoes = readerCliente.GetString( "txt_observacoes" ); } catch { }
			try { cliente.ativo = readerCliente.GetBoolean( "flg_ativo" ); } catch { }
			try { cliente.dataDeCadastro = readerCliente.GetDateTime( "dat_cadastro" ); } catch { }
			try { cliente.dataDeAtualizacao = readerCliente.GetDateTime( "dat_atualizacao" ); } catch { }

			preencherMeiosDeContato( ref cliente );
			preencherEnderecos( ref cliente );

			return cliente;
		}

		public static List<Cliente> getClientes( UInt32 start, UInt32 limit, List<Filter> filters, List<Sorter> sorters ) {
			List<Cliente> clientes = new List<Cliente>();
			StringBuilder sqlClientes = new StringBuilder();

			MySqlFilter mySqlFilter = createMySqlFilter( filters );
			String sortClause = construirSortClause( sorters );


			sqlClientes.AppendLine( "SELECT" );
			sqlClientes.AppendLine( "	 cod_cliente" );
			sqlClientes.AppendLine( "	,nom_cliente" );
			sqlClientes.AppendLine( "	,nom_conjuge" );
			sqlClientes.AppendLine( "	,tb_tipos_clientes.cod_tipo_cliente" );
			sqlClientes.AppendLine( "	,tb_tipos_clientes.nom_tipo_cliente" );
			sqlClientes.AppendLine( "	,int_sexo" );
			sqlClientes.AppendLine( "	,dat_nascimento" );
			sqlClientes.AppendLine( "	,txt_rg" );
			sqlClientes.AppendLine( "	,txt_cpf" );
			sqlClientes.AppendLine( "	,txt_observacoes" );
			sqlClientes.AppendLine( "	,tb_clientes.flg_ativo" );
			sqlClientes.AppendLine( "	,dat_cadastro" );
			sqlClientes.AppendLine( "	,dat_atualizacao" );
			sqlClientes.AppendLine( "FROM tb_clientes " );
			sqlClientes.AppendLine( "INNER JOIN tb_tipos_clientes ON tb_tipos_clientes.cod_tipo_cliente = tb_clientes.cod_tipo_cliente  " );
			sqlClientes.AppendLine( mySqlFilter.whereClause );
			sqlClientes.AppendLine( ( sortClause.Trim().Length > 0 ) ? sortClause : " ORDER BY nom_cliente " );
			if( limit > 0 )
				sqlClientes.AppendLine( " LIMIT " + start + "," + limit );


			MySqlConnection connClientes = MySqlConnectionWizard.getConnection();
			connClientes.Open();
			MySqlCommand cmdCliente = new MySqlCommand( sqlClientes.ToString(), connClientes );
			if( mySqlFilter.parametersList.Count > 0 ) {
				cmdCliente.Parameters.AddRange( mySqlFilter.parametersList.ToArray() );
			}

			MySqlDataReader readerCliente = cmdCliente.ExecuteReader();
			while( readerCliente.Read() ) { clientes.Add( getCliente( readerCliente ) ); }
			readerCliente.Close(); readerCliente.Dispose(); cmdCliente.Dispose();
			connClientes.Close(); connClientes.Dispose();

			return clientes;
		}

		public static List<Erro> atualizarListaDeClientes( ref List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sqlCliente = new StringBuilder();
			StringBuilder sqlUpdateMeioDeContato = new StringBuilder();
			StringBuilder sqlInsertMeioDeContato = new StringBuilder();
			StringBuilder sqlUpdateEndereco = new StringBuilder();
			StringBuilder sqlInsertEndereco = new StringBuilder();

			#region SQL UPDATE CLIENTE
			sqlCliente.AppendLine( "UPDATE tb_clientes SET" );
			sqlCliente.AppendLine( "  cod_tipo_cliente = @cod_tipo_cliente " );
			sqlCliente.AppendLine( " ,nom_cliente = @nom_cliente " );
			sqlCliente.AppendLine( " ,nom_conjuge = @nom_conjuge " );
			sqlCliente.AppendLine( " ,int_sexo = @int_sexo " );
			sqlCliente.AppendLine( " ,dat_nascimento = @dat_nascimento " );
			sqlCliente.AppendLine( " ,txt_rg = @txt_rg " );
			sqlCliente.AppendLine( " ,txt_cpf = @txt_cpf " );
			sqlCliente.AppendLine( " ,txt_observacoes = @txt_observacoes " );
			sqlCliente.AppendLine( " ,flg_ativo = @flg_ativo " );
			sqlCliente.AppendLine( " ,dat_atualizacao = NOW() " );
			sqlCliente.AppendLine( "WHERE cod_cliente = @cod_cliente" );
			#endregion

			#region SQL UPDATE MEIO DE CONTATO
			sqlUpdateMeioDeContato.AppendLine( "UPDATE tb_meios_de_contato SET" );
			sqlUpdateMeioDeContato.AppendLine( "  cod_tipo_meio_de_contato = @cod_tipo_meio_de_contato " );
			sqlUpdateMeioDeContato.AppendLine( " ,txt_meio_de_contato = @txt_meio_de_contato " );
			sqlUpdateMeioDeContato.AppendLine( " ,int_meio_de_contato = @int_meio_de_contato " );
			sqlUpdateMeioDeContato.AppendLine( " ,txt_descricao = @txt_descricao " );
			sqlUpdateMeioDeContato.AppendLine( "WHERE cod_meio_de_contato = @cod_meio_de_contato " );
			sqlUpdateMeioDeContato.AppendLine( "  AND cod_cliente = @cod_cliente" );
			#endregion

			#region SQL INSERT MEIO DE CONTATO
			sqlInsertMeioDeContato.AppendLine( "INSERT INTO tb_meios_de_contato(" );
			sqlInsertMeioDeContato.AppendLine( "	 cod_cliente " );
			sqlInsertMeioDeContato.AppendLine( "	,cod_tipo_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,txt_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,int_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,txt_descricao " );
			sqlInsertMeioDeContato.AppendLine( ") " );
			sqlInsertMeioDeContato.AppendLine( "VALUES(" );
			sqlInsertMeioDeContato.AppendLine( "	 @cod_cliente " );
			sqlInsertMeioDeContato.AppendLine( "	,@cod_tipo_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,@txt_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,@int_meio_de_contato " );
			sqlInsertMeioDeContato.AppendLine( "	,@txt_descricao " );
			sqlInsertMeioDeContato.AppendLine( "); " );
			sqlInsertMeioDeContato.AppendLine( "SELECT LAST_INSERT_ID()" );
			#endregion

			#region SQL UPDATE ENDERECO
			sqlUpdateEndereco.AppendLine( "UPDATE tb_enderecos SET" );
			sqlUpdateEndereco.AppendLine( "  cod_logradouro = @cod_logradouro " );
			sqlUpdateEndereco.AppendLine( " ,txt_complemento = @txt_complemento " );
			sqlUpdateEndereco.AppendLine( " ,txt_ponto_referencia = @txt_ponto_referencia " );
			sqlUpdateEndereco.AppendLine( " ,int_numero = @int_numero " );
			sqlUpdateEndereco.AppendLine( "WHERE cod_endereco = @cod_endereco " );
			sqlUpdateEndereco.AppendLine( "  AND cod_cliente = @cod_cliente" );
			#endregion

			#region SQL INSERT ENDERECO
			sqlInsertEndereco.AppendLine( "INSERT INTO tb_enderecos(" );
			sqlInsertEndereco.AppendLine( "	 cod_cliente " );
			sqlInsertEndereco.AppendLine( "	,cod_logradouro " );
			sqlInsertEndereco.AppendLine( "	,txt_complemento " );
			sqlInsertEndereco.AppendLine( "	,txt_ponto_referencia " );
			sqlInsertEndereco.AppendLine( "	,int_numero " );
			sqlInsertEndereco.AppendLine( ") " );
			sqlInsertEndereco.AppendLine( "VALUES(" );
			sqlInsertEndereco.AppendLine( "	 @cod_cliente " );
			sqlInsertEndereco.AppendLine( "	,@cod_logradouro " );
			sqlInsertEndereco.AppendLine( "	,@txt_complemento " );
			sqlInsertEndereco.AppendLine( "	,@txt_ponto_referencia " );
			sqlInsertEndereco.AppendLine( "	,@int_numero " );
			sqlInsertEndereco.AppendLine( "); " );
			sqlInsertEndereco.AppendLine( "SELECT LAST_INSERT_ID()" );
			#endregion

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cliente cliente in clientes ) {

				List<UInt32> codMeiosDeContatoList = new List<UInt32>();
				List<UInt32> codEnderecosList = new List<UInt32>();

				#region ATUALIZA CLIENTE
				MySqlCommand cmdCliente = new MySqlCommand( sqlCliente.ToString(), conn );
				cmdCliente.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
				cmdCliente.Parameters.Add( "@cod_tipo_cliente", MySqlDbType.UInt32 ).Value = cliente.tipoDeCliente.codigo;
				cmdCliente.Parameters.Add( "@nom_cliente", MySqlDbType.VarChar ).Value = cliente.nome;
				cmdCliente.Parameters.Add( "@int_sexo", MySqlDbType.Int16 ).Value = cliente.sexo;
				cmdCliente.Parameters.Add( "@flg_ativo", MySqlDbType.Bit ).Value = cliente.ativo;
				if( String.IsNullOrEmpty( cliente.conjuge.Trim() ) == false ) { cmdCliente.Parameters.Add( "@nom_conjuge", MySqlDbType.VarChar ).Value = cliente.conjuge; } else { cmdCliente.Parameters.Add( "@nom_conjuge", MySqlDbType.VarChar ).Value = DBNull.Value; }
				if( cliente.dataDeNascimento != null ) { cmdCliente.Parameters.Add( "@dat_nascimento", MySqlDbType.Timestamp ).Value = cliente.dataDeNascimento.Value.ToString( "yyyy-MM-dd" ); } else { cmdCliente.Parameters.Add( "@dat_nascimento", MySqlDbType.Timestamp ).Value = DBNull.Value; }
				if( String.IsNullOrEmpty( cliente.rg.Trim() ) == false ) { cmdCliente.Parameters.Add( "@txt_rg", MySqlDbType.VarChar ).Value = cliente.rg; } else { cmdCliente.Parameters.Add( "@txt_rg", MySqlDbType.VarChar ).Value = DBNull.Value; }
				if( String.IsNullOrEmpty( cliente.cpf.Trim() ) == false ) { cmdCliente.Parameters.Add( "@txt_cpf", MySqlDbType.VarChar ).Value = cliente.cpf; } else { cmdCliente.Parameters.Add( "@txt_cpf", MySqlDbType.VarChar ).Value = DBNull.Value; }
				if( String.IsNullOrEmpty( cliente.observacoes.Trim() ) == false ) { cmdCliente.Parameters.Add( "@txt_observacoes", MySqlDbType.VarChar ).Value = cliente.observacoes; } else { cmdCliente.Parameters.Add( "@txt_observacoes", MySqlDbType.VarChar ).Value = DBNull.Value; }
				cmdCliente.ExecuteNonQuery();
				cmdCliente.Dispose();
				#endregion

				#region ATUALIZA MEIOS DE CONTATO
				foreach( MeioDeContato meioDeContato in cliente.meiosDeContato ) {

					MySqlCommand cmd = new MySqlCommand();
					cmd.Connection = conn;
					cmd.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
					cmd.Parameters.Add( "@cod_tipo_meio_de_contato", MySqlDbType.UInt32 ).Value = meioDeContato.tipoDeContato.codigo;
					cmd.Parameters.Add( "@txt_meio_de_contato", MySqlDbType.VarChar ).Value = meioDeContato.contato;
					if( meioDeContato.intContato > 0 ) { cmd.Parameters.Add( "@int_meio_de_contato", MySqlDbType.UInt32 ).Value = meioDeContato.intContato; } else { cmd.Parameters.Add( "@int_meio_de_contato", MySqlDbType.UInt32 ).Value = DBNull.Value; }
					if( String.IsNullOrEmpty( meioDeContato.descricao.Trim() ) == false ) { cmd.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = meioDeContato.descricao; } else { cmd.Parameters.Add( "@txt_descricao", MySqlDbType.VarChar ).Value = DBNull.Value; }

					if( meioDeContato.codigo == 0 ) {
						cmd.CommandText = sqlInsertMeioDeContato.ToString();
						meioDeContato.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
					} else {
						cmd.Parameters.Add( "@cod_meio_de_contato", MySqlDbType.UInt32 ).Value = meioDeContato.codigo;
						cmd.CommandText = sqlUpdateMeioDeContato.ToString();
						cmd.ExecuteNonQuery();
					}
					cmd.Dispose();


					if( meioDeContato.codigo == 0 ) { 
						erros.Add( new Erro( 0, "Não foi possível atualizar o meio de contato: " + meioDeContato.contato, "Tente inseri-lo novamente" ) ); 
					} else {
						codMeiosDeContatoList.Add( meioDeContato.codigo );
					}
				}

				#region EXCLUI MEIOS DE CONTATO NAO USADOS
				MySqlCommand cmdDeleteMeiosDeContato = new MySqlCommand();
				cmdDeleteMeiosDeContato.Connection = conn;
				cmdDeleteMeiosDeContato.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
				if( codMeiosDeContatoList.Count > 0 ) {
					StringBuilder codigos = new StringBuilder();
					for( int i = 0; i < codMeiosDeContatoList.Count; i++ ) {
						String param = "@cod_meio_de_contato_" + i;
						codigos.Append( param + "," );
						cmdDeleteMeiosDeContato.Parameters.Add( new MySqlParameter( param, MySqlDbType.UInt32 ) ).Value = codMeiosDeContatoList[i];
					}
					codigos.Replace( ",", "", codigos.Length - 1, 1 );// remove a ultima virgula

					cmdDeleteMeiosDeContato.CommandText = "DELETE FROM tb_meios_de_contato WHERE cod_cliente = @cod_cliente AND cod_meio_de_contato NOT IN ( " + codigos.ToString() + " )";
				} else {
					cmdDeleteMeiosDeContato.CommandText = "DELETE FROM tb_meios_de_contato WHERE cod_cliente = @cod_cliente";
				}
				cmdDeleteMeiosDeContato.ExecuteNonQuery();
				cmdDeleteMeiosDeContato.Dispose();
				#endregion
				
				#endregion

				#region ATUALIZA ENDERECOS
				foreach( Endereco endereco in cliente.enderecos ) {

					MySqlCommand cmd = new MySqlCommand();
					cmd.Connection = conn;
					cmd.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
					cmd.Parameters.Add( "@cod_logradouro", MySqlDbType.UInt32 ).Value = endereco.logradouro.codigo;
					if( String.IsNullOrEmpty( endereco.complemento.Trim() ) == false ) { cmd.Parameters.Add( "@txt_complemento", MySqlDbType.VarChar ).Value = endereco.complemento; } else { cmd.Parameters.Add( "@txt_complemento", MySqlDbType.VarChar ).Value = DBNull.Value; }
					if( String.IsNullOrEmpty( endereco.pontoDeReferencia.Trim() ) == false ) { cmd.Parameters.Add( "@txt_ponto_referencia", MySqlDbType.VarChar ).Value = endereco.pontoDeReferencia; } else { cmd.Parameters.Add( "@txt_ponto_referencia", MySqlDbType.VarChar ).Value = DBNull.Value; }
					if( endereco.numero > 0 ) { cmd.Parameters.Add( "@int_numero", MySqlDbType.UInt32 ).Value = endereco.numero; } else { cmd.Parameters.Add( "@int_numero", MySqlDbType.UInt32 ).Value = DBNull.Value; }


					if( endereco.codigo == 0 ) {
						cmd.CommandText = sqlInsertEndereco.ToString();
						endereco.codigo = UInt32.Parse( cmd.ExecuteScalar().ToString() );
					} else {
						cmd.Parameters.Add( "@cod_endereco", MySqlDbType.UInt32 ).Value = endereco.codigo;
						cmd.CommandText = sqlUpdateEndereco.ToString();
						cmd.ExecuteNonQuery();
					}
					cmd.Dispose();


					if( endereco.codigo == 0 ) {
						erros.Add( new Erro( 0, "Não foi possível atualizar o endere&ccedil;o: " + endereco.logradouro.nome + ", " + endereco.numero, "Tente atualiza-lo novamente" ) );
					} else {
						codEnderecosList.Add( endereco.codigo );
					}
				}

				#region EXCLUI ENDERECOS NAO USADOS
				MySqlCommand cmdDeleteEnderecos = new MySqlCommand();
				cmdDeleteEnderecos.Connection = conn;
				cmdDeleteEnderecos.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
				if( codEnderecosList.Count > 0 ) {
					StringBuilder codigos = new StringBuilder();
					for( int i = 0; i < codEnderecosList.Count; i++ ) {
						String param = "@cod_endereco_" + i;
						codigos.Append( param + "," );
						cmdDeleteEnderecos.Parameters.Add( new MySqlParameter( param, MySqlDbType.UInt32 ) ).Value = codEnderecosList[i];
					}
					codigos.Replace( ",", "", codigos.Length - 1, 1 );// remove a ultima virgula

					cmdDeleteEnderecos.CommandText = "DELETE FROM tb_enderecos WHERE cod_cliente = @cod_cliente AND cod_endereco NOT IN ( " + codigos.ToString() + " )";
				} else {
					cmdDeleteEnderecos.CommandText = "DELETE FROM tb_enderecos WHERE cod_cliente = @cod_cliente";
				}
				cmdDeleteEnderecos.ExecuteNonQuery();
				cmdDeleteEnderecos.Dispose();
				#endregion

				#endregion
			}

			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> excluirListaDeClientes( List<Cliente> clientes ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_clientes WHERE cod_cliente = @cod_cliente ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Cliente cliente in clientes ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@cod_cliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o cliente: " + cliente.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static void preencherMeiosDeContato( ref Cliente cliente ) {

			// Meios de Contato
			MySqlConnection connMeiosDeContato = MySqlConnectionWizard.getConnection();
			connMeiosDeContato.Open();
			MySqlCommand cmdMeiosDeContato = new MySqlCommand( SQL_MEIOS_DE_CONTATO, connMeiosDeContato );
			cmdMeiosDeContato.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
			MySqlDataReader readerMeiosDeContato = cmdMeiosDeContato.ExecuteReader();
			while( readerMeiosDeContato.Read() ) {
				MeioDeContato meioDeContato = new MeioDeContato();
				meioDeContato.codigo = readerMeiosDeContato.GetUInt32( "cod_meio_de_contato" );
				meioDeContato.tipoDeContato.codigo = readerMeiosDeContato.GetUInt32( "cod_tipo_meio_de_contato" );
				meioDeContato.tipoDeContato.nome = readerMeiosDeContato.GetString( "nom_tipo_meio_de_contato" );
				meioDeContato.contato = readerMeiosDeContato.GetString( "txt_meio_de_contato" );
				try { meioDeContato.intContato = readerMeiosDeContato.GetUInt32( "int_meio_de_contato" ); } catch { }
				try { meioDeContato.descricao = readerMeiosDeContato.GetString( "txt_descricao" ); } catch { }
				cliente.meiosDeContato.Add( meioDeContato );
			}
			readerMeiosDeContato.Close(); readerMeiosDeContato.Dispose();
			cmdMeiosDeContato.Dispose();
			connMeiosDeContato.Close(); connMeiosDeContato.Dispose();
		}

		public static void preencherEnderecos( ref Cliente cliente ) {
			// Enderecos
			MySqlConnection connEnderecos = MySqlConnectionWizard.getConnection();
			connEnderecos.Open();
			MySqlCommand cmdEnderecos = new MySqlCommand( SQL_ENDERECOS, connEnderecos );
			cmdEnderecos.Parameters.Add( "@codCliente", MySqlDbType.UInt32 ).Value = cliente.codigo;
			MySqlDataReader readerEnderecos = cmdEnderecos.ExecuteReader();
			while( readerEnderecos.Read() ) {
				Endereco endereco = new Endereco();
				UInt32 codLogradouro;
				endereco.codigo = readerEnderecos.GetUInt32( "cod_endereco" );
				codLogradouro = readerEnderecos.GetUInt32( "cod_logradouro" );
				try { endereco.complemento = readerEnderecos.GetString( "txt_complemento" ); } catch { }
				try { endereco.pontoDeReferencia = readerEnderecos.GetString( "txt_ponto_referencia" ); } catch { }
				try { endereco.numero = readerEnderecos.GetUInt32( "int_numero" ); } catch { }

				MySqlLogradourosDao.fillLogradouro( codLogradouro, endereco.logradouro );
				endereco.bairro = endereco.logradouro.bairro;
				endereco.cidade = endereco.logradouro.bairro.cidade;
				endereco.estado = endereco.logradouro.bairro.cidade.estado;
				endereco.pais = endereco.logradouro.bairro.cidade.estado.pais;

				cliente.enderecos.Add( endereco );
			}
			readerEnderecos.Close(); readerEnderecos.Dispose();
			cmdEnderecos.Dispose();
			connEnderecos.Close(); connEnderecos.Dispose();
		}

		private static MySqlFilter createMySqlFilter( List<Filter> filters ) {
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
							filterList.Add( "tb_clientes.cod_cliente = @codCliente" );
							MySqlParameter codCliente = new MySqlParameter( "@codCliente", MySqlDbType.UInt32 );
							codCliente.Value = codigo;
							mySqlfilter.parametersList.Add( codCliente );
						}
						break;

					case "ativo":
						try {
							bool ativo = bool.Parse( filter.value );
							filterList.Add( "tb_clientes.flg_ativo = @flgAtivo" );
							MySqlParameter flgAtivo = new MySqlParameter( "@flgAtivo", MySqlDbType.Bit );
							flgAtivo.Value = ( ativo ) ? 1 : 0;
							mySqlfilter.parametersList.Add( flgAtivo );
						} catch { }
						break;

					case "nome":
						if( !String.IsNullOrEmpty( filter.value ) ) {
							filterList.Add( "tb_clientes.nom_cliente LIKE @nomCliente" );
							MySqlParameter nomCliente = new MySqlParameter( "@nomCliente", MySqlDbType.VarChar );
							nomCliente.Value = filter.value;
							mySqlfilter.parametersList.Add( nomCliente );
						}
						break;

					case "conjuge":
						if( !String.IsNullOrEmpty( filter.value ) ) {
							filterList.Add( "tb_clientes.nom_conjuge LIKE @nomConjuge" );
							MySqlParameter nomConjuge = new MySqlParameter( "@nomConjuge", MySqlDbType.VarChar );
							nomConjuge.Value = filter.value;
							mySqlfilter.parametersList.Add( nomConjuge );
						}
						break;

					case "codigoTipoDeCliente":
						UInt32 codTipoCliente = 0;
						UInt32.TryParse( filter.value, out codTipoCliente );
						if( codTipoCliente > 0 ) {
							filterList.Add( "tb_clientes.cod_tipo_cliente = @codTipoCliente" );
							MySqlParameter tipoCliente = new MySqlParameter( "@codTipoCliente", MySqlDbType.UInt32 );
							tipoCliente.Value = codTipoCliente;
							mySqlfilter.parametersList.Add( tipoCliente );
						}
						break;

					case "dataDeNascimento":
						try {
							DateTime data = DateTime.Parse( filter.value );
							filterList.Add( "tb_clientes.dat_nascimento = @dataNascimento" );
							MySqlParameter dataNascimento = new MySqlParameter( "@dataNascimento", MySqlDbType.Timestamp );
							dataNascimento.Value = data;
							mySqlfilter.parametersList.Add( dataNascimento );
						} catch { }
						break;

					case "sexo":
						int intSexo = 0;
						int.TryParse( filter.value, out intSexo );
						if( intSexo > 0 ) {
							filterList.Add( "tb_clientes.int_sexo = @sexo" );
							MySqlParameter sexo = new MySqlParameter( "@sexo", MySqlDbType.UInt32 );
							sexo.Value = intSexo;
							mySqlfilter.parametersList.Add( sexo );
						}
						break;

					case "rg":
						if( !String.IsNullOrEmpty( filter.value ) ) {
							filterList.Add( "tb_clientes.txt_rg = @rg" );
							MySqlParameter rg = new MySqlParameter( "@rg", MySqlDbType.VarChar );
							rg.Value = filter.value;
							mySqlfilter.parametersList.Add( rg );
						}
						break;

					case "cpf":
						if( !String.IsNullOrEmpty( filter.value ) ) {
							filterList.Add( "tb_clientes.txt_cpf = @cpf" );
							MySqlParameter cpf = new MySqlParameter( "@cpf", MySqlDbType.VarChar );
							cpf.Value = filter.value;
							mySqlfilter.parametersList.Add( cpf );
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

		private static String construirSortClause( List<Sorter> sorters ) {

			StringBuilder sortSql = new StringBuilder();

			foreach( Sorter sorter in sorters ) {
				switch( sorter.property ) {
					case "codigo":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.cod_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "nome":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.nom_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "conjuge":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.nom_conjuge", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "codigoTipoDeCliente":
						sortSql.AppendFormat( "{0} {1},", "tb_tipos_clientes.cod_tipo_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "nomeTipoDeCliente":
						sortSql.AppendFormat( "{0} {1},", "tb_tipos_clientes.nom_tipo_cliente", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "sexo":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.int_sexo", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "strSexo":
						// invertido por M ser depois de F e na programacao o M=1 e F=2, ou seja, M antes do F
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.int_sexo", ( sorter.direction == "DESC" ) ? "ASC" : "DESC" );
						break;
					case "dataDeNascimento":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.dat_nascimento", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "rg":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.txt_rg", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "cpf":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.txt_cpf", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "ativo":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.flg_ativo", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "dataDeCadastro":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.dat_cadastro", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
					case "dataDeAtualizacao":
						sortSql.AppendFormat( "{0} {1},", "tb_clientes.dat_atualizacao", ( sorter.direction == "DESC" ) ? "DESC" : "ASC" );
						break;
				}
			}
			if( sorters.Count > 0 ) sortSql.Remove( sortSql.Length - 1, 1 );

			return ( ( sortSql.ToString().Trim().Length > 0 ) ? " ORDER BY " + sortSql.ToString() : "" );
		}
	}
}
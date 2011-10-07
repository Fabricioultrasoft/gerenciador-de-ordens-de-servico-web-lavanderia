using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos {
	public class MySqlServicosDao {

		#region SQL
		private const String SELECT_SERVICOS
 			= "SELECT "
			+ "	cod_servico "
			+ "	,nom_servico "
			+ "	,int_cobrado_por "
			+ "	,txt_descricao "
			+ "FROM tb_servicos ";

		private const String SQL_VALORES
			= "SELECT "
			+ "		 cod_valor_servico "
			+ "		,cod_valor_servico_pai "
			+ "		,cod_servico "
			+ "		,tb_tapetes.cod_tapete "
			+ "		,tb_tapetes.nom_tapete "
			+ "		,tb_tipos_clientes.cod_tipo_cliente "
			+ "		,tb_tipos_clientes.nom_tipo_cliente "
			+ "		,val_inicial "
			+ "		,val_acima_10m2 "
			+ "FROM tb_tapetes "
			+ "LEFT JOIN tb_valores_servicos "
			+ "		  ON tb_valores_servicos.cod_tapete = tb_tapetes.cod_tapete "
			+ "LEFT JOIN tb_tipos_clientes "
			+ "		  ON tb_tipos_clientes.cod_tipo_cliente = tb_valores_servicos.cod_tipo_cliente "
			+ "WHERE (cod_servico = @codServico OR cod_servico IS NULL) ";

		private const String SQL_INSERT_VALOR
			= "INSERT INTO tb_valores_servicos ( "
			+ "	 cod_valor_servico_pai "
			+ "	,cod_servico "
			+ "	,cod_tapete "
			+ "	,cod_tipo_cliente "
			+ "	,val_inicial "
			+ "	,val_acima_10m2 "
			+ ") VALUES ( "
			+ "	 @codValorServicoPai "
			+ "	,@codServico "
			+ "	,@codTapete "
			+ "	,@codTipoCliente "
			+ "	,@valInicial "
			+ "	,@valAcima10m2 ); "
			+ "SELECT LAST_INSERT_ID() ";

		private const String SQL_UPDATE_VALOR
			= "UPDATE tb_valores_servicos SET "
			+ "	 cod_tipo_cliente = @codTipoCliente "
			+ "	,val_inicial = @valInicial "
			+ "	,val_acima_10m2 = @valAcima10m2 "
			+ "WHERE cod_valor_servico = @codValorServico ";
		#endregion

		public static long countServicos() {
			long count = 0;

			String sql = " SELECT COUNT(cod_servico) "
					+ " FROM tb_servicos "; // Filtro

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql, conn );

			conn.Open();

			count = (long) cmd.ExecuteScalar();

			cmd.Dispose();

			conn.Close();
			conn.Dispose();
			return count;
		}

		public static void fillServico( UInt32 codigo,ref Servico servico, MySqlConnection conn, bool apenasDadosBasicos ) {
			
			MySqlCommand cmd = new MySqlCommand( SELECT_SERVICOS + " WHERE cod_servico = @codServico", conn );
			cmd.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = codigo;
			
			servico.codigo = codigo;

			MySqlDataReader reader = cmd.ExecuteReader();
			if( reader.Read() ) {
				servico.nome = reader.GetString( "nom_servico" );
				servico.cobradoPor = (CobradoPor) Enum.Parse( typeof( CobradoPor ), reader.GetUInt32( "int_cobrado_por" ).ToString(), true );
				try { servico.descricao = reader.GetString( "txt_descricao" ); } catch { }
			}

			if( !apenasDadosBasicos ) {
				preencherValores( ref servico );
			}

			reader.Close(); reader.Dispose(); cmd.Dispose();
		}
		public static void fillServicos( UInt32 start, UInt32 limit, ref List<Servico> servicos, MySqlConnection conn, bool apenasDadosBasicos ) {
			StringBuilder sqlServicos = new StringBuilder();
			sqlServicos.AppendLine( SELECT_SERVICOS );
			sqlServicos.AppendLine( " ORDER BY nom_servico " );
			if( limit > 0 )
				sqlServicos.AppendLine( " LIMIT " + start + "," + limit );

			MySqlCommand cmd = new MySqlCommand( sqlServicos.ToString(), conn );

			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				Servico servico = new Servico();

				servico.codigo = reader.GetUInt32( "cod_servico" );
				servico.nome = reader.GetString( "nom_servico" );
				servico.cobradoPor = (CobradoPor) Enum.Parse( typeof( CobradoPor ), reader.GetUInt32( "int_cobrado_por" ).ToString(), true );
				try { servico.descricao = reader.GetString( "txt_descricao" ); } catch { }

				if( !apenasDadosBasicos ) {
					preencherValores( ref servico );
				}

				servicos.Add( servico );
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();
		}

		public static Servico getServico( UInt32 codigoServico ) {
			return getServico( codigoServico, false );
		}
		public static Servico getServico( UInt32 codigoServico, bool apenasDadosBasicos ) {
			Servico servico = new Servico( codigoServico );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			fillServico( codigoServico, ref servico, conn, apenasDadosBasicos );

			conn.Close(); conn.Dispose();

			return servico;
		}

		public static List<Servico> getServicos( UInt32 start, UInt32 limit ) {
			return getServicos( start, limit, false );
		}
		public static List<Servico> getServicos( UInt32 start, UInt32 limit, bool apenasDadosBasicos ) {
			List<Servico> servicos = new List<Servico>();
			
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();

			fillServicos( start, limit, ref servicos, conn, apenasDadosBasicos );

			conn.Close(); conn.Dispose();

			return servicos;
		}

		public static void fillServicosEspecificos( UInt32 codigoTapete, UInt32 codigoTipoDeCliente, ref List<Servico> servicosEspecificos, MySqlConnection conn ) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine( "SELECT" );
			sql.AppendLine( "     tb_servicos.cod_servico" );
			sql.AppendLine( "    ,tb_servicos.nom_servico" );
			sql.AppendLine( "    ,tb_servicos.int_cobrado_por" );
			sql.AppendLine( "    ,tb_servicos.txt_descricao" );
			sql.AppendLine( "    ,tb_valores_servicos.cod_valor_servico" );
			sql.AppendLine( "    ,tb_valores_servicos.cod_valor_servico_pai" );
			sql.AppendLine( "    ,tb_valores_servicos.val_inicial" );
			sql.AppendLine( "    ,tb_valores_servicos.val_acima_10m2" );
			sql.AppendLine( "    ,tb_tapetes.cod_tapete" );
			sql.AppendLine( "    ,tb_tapetes.nom_tapete" );
			sql.AppendLine( "    ,tb_tipos_clientes.cod_tipo_cliente" );
			sql.AppendLine( "    ,tb_tipos_clientes.nom_tipo_cliente" );
			sql.AppendLine( "  FROM tb_servicos" );
			sql.AppendLine( " INNER JOIN tb_valores_servicos ON tb_servicos.cod_servico = tb_valores_servicos.cod_servico" );
			sql.AppendLine( " INNER JOIN tb_tapetes ON tb_valores_servicos.cod_tapete = tb_tapetes.cod_tapete" );
			sql.AppendLine( "  LEFT JOIN tb_tipos_clientes ON tb_valores_servicos.cod_tipo_cliente = tb_tipos_clientes.cod_tipo_cliente" );
			sql.AppendLine( " WHERE tb_valores_servicos.cod_tapete = @codTapete " );
			sql.AppendLine( "   AND tb_valores_servicos.cod_tipo_cliente = @codTipoCliente " );
			sql.AppendLine( "UNION" );
			sql.AppendLine( "SELECT" );
			sql.AppendLine( "     tb_servicos.cod_servico" );
			sql.AppendLine( "    ,tb_servicos.nom_servico" );
			sql.AppendLine( "    ,tb_servicos.int_cobrado_por" );
			sql.AppendLine( "    ,tb_servicos.txt_descricao" );
			sql.AppendLine( "    ,tb_valores_servicos.cod_valor_servico" );
			sql.AppendLine( "    ,tb_valores_servicos.cod_valor_servico_pai" );
			sql.AppendLine( "    ,tb_valores_servicos.val_inicial" );
			sql.AppendLine( "    ,tb_valores_servicos.val_acima_10m2" );
			sql.AppendLine( "    ,tb_tapetes.cod_tapete" );
			sql.AppendLine( "    ,tb_tapetes.nom_tapete" );
			sql.AppendLine( "    ,tb_tipos_clientes.cod_tipo_cliente" );
			sql.AppendLine( "    ,tb_tipos_clientes.nom_tipo_cliente" );
			sql.AppendLine( "  FROM tb_servicos" );
			sql.AppendLine( " INNER JOIN tb_valores_servicos ON tb_servicos.cod_servico = tb_valores_servicos.cod_servico" );
			sql.AppendLine( " INNER JOIN tb_tapetes ON tb_valores_servicos.cod_tapete = tb_tapetes.cod_tapete" );
			sql.AppendLine( "  LEFT JOIN tb_tipos_clientes ON tb_valores_servicos.cod_tipo_cliente = tb_tipos_clientes.cod_tipo_cliente" );
			sql.AppendLine( " WHERE tb_valores_servicos.cod_tapete = @codTapete " );
			sql.AppendLine( "   AND tb_valores_servicos.cod_tipo_cliente IS NULL" );
			sql.AppendLine( "   AND tb_servicos.cod_servico NOT IN " );
			sql.AppendLine( "   (" );
			sql.AppendLine( "       SELECT cod_servico FROM tb_valores_servicos" );
			sql.AppendLine( "       WHERE cod_tapete = @codTapete AND cod_tipo_cliente = @codTipoCliente" );
			sql.AppendLine( "   )" );
			sql.AppendLine( "ORDER BY nom_servico" );

			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@codTapete", MySqlDbType.UInt32 ).Value = codigoTapete;
			cmd.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = codigoTipoDeCliente;

			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				Servico servico = new Servico();
				servico.valores.Add( new ValorDeServico() );

				servico.codigo = reader.GetUInt32( "cod_servico" );
				servico.nome = reader.GetString( "nom_servico" );
				servico.cobradoPor = (CobradoPor) Enum.Parse( typeof( CobradoPor ), reader.GetUInt32( "int_cobrado_por" ).ToString(), true );
				try { servico.descricao = reader.GetString( "txt_descricao" ); } catch { }
				servico.valores[0].codigo = reader.GetUInt32( "cod_valor_servico" );
				try { servico.valores[0].codigoPai = reader.GetUInt32( "cod_valor_servico_pai" ); } catch { }
				servico.valores[0].valorInicial = reader.GetDouble( "val_inicial" );
				servico.valores[0].valorAcima10m2 = reader.GetDouble( "val_acima_10m2" );
				servico.valores[0].tapete.codigo = reader.GetUInt32( "cod_tapete" );
				servico.valores[0].tapete.nome = reader.GetString( "nom_tapete" );
				try { servico.valores[0].tipoDeCliente.codigo = reader.GetUInt32( "cod_tipo_cliente" ); } catch { }
				try { servico.valores[0].tipoDeCliente.nome = reader.GetString( "nom_tipo_cliente" ); } catch { }
				servicosEspecificos.Add( servico );
			}
			reader.Close(); reader.Dispose(); cmd.Dispose();
		}

		public static List<Servico> getServicosEspecificos( UInt32 codigoTapete, UInt32 codigoTipoDeCliente ) {
			List<Servico> servicos = new List<Servico>();
			
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			
			conn.Open();
			fillServicosEspecificos( codigoTapete, codigoTipoDeCliente, ref servicos, conn );
			conn.Close(); conn.Dispose();

			return servicos;
		}

		public static void preencherValores( ref Servico servico ) {
			List<ValorDeServico> valoresAux = new List<ValorDeServico>();
			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( SQL_VALORES, conn );
			cmd.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servico.codigo;

			conn.Open();

			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				ValorDeServico val = new ValorDeServico();

				try { val.codigo = reader.GetUInt32( "cod_valor_servico" ); } catch { }
				try { val.codigoPai = reader.GetUInt32( "cod_valor_servico_pai" ); } catch { }
				try { val.codigoServico = reader.GetUInt32( "cod_servico" ); } catch { }
				try { val.tapete.codigo = reader.GetUInt32( "cod_tapete" ); } catch { }
				try { val.tapete.nome = reader.GetString( "nom_tapete" ); } catch { }
				try { val.tipoDeCliente.codigo = reader.GetUInt32( "cod_tipo_cliente" ); } catch { }
				try { val.tipoDeCliente.nome = reader.GetString( "nom_tipo_cliente" ); } catch { }
				try { val.valorInicial = reader.GetDouble( "val_inicial" ); } catch { }
				try { val.valorAcima10m2 = reader.GetDouble( "val_acima_10m2" ); } catch { }

				valoresAux.Add( val );
			}

			servico.valores.AddRange( estreturarValores( valoresAux ) );

			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();
		}

		public static List<Erro> inserirListaDeServicos( ref List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sqlServico = new StringBuilder();
			StringBuilder sqlValor = new StringBuilder();

			sqlServico.AppendLine( "INSERT INTO tb_servicos (" );
			sqlServico.AppendLine( "	 nom_servico" );
			sqlServico.AppendLine( "	,int_cobrado_por" );
			sqlServico.AppendLine( "	,txt_descricao" );
			sqlServico.AppendLine( ") VALUES (" );
			sqlServico.AppendLine( "	 @nomServico" );
			sqlServico.AppendLine( "	,@intCobradoPor" );
			sqlServico.AppendLine( "	,@txtDescricao" );
			sqlServico.AppendLine( ");" );
			sqlServico.AppendLine( "SELECT LAST_INSERT_ID()" );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Servico servico in servicos ) {
				MySqlCommand cmdServico = new MySqlCommand( sqlServico.ToString(), conn );
				cmdServico.Parameters.Add( "@nomServico", MySqlDbType.VarChar ).Value = servico.nome;
				cmdServico.Parameters.Add( "@intCobradoPor", MySqlDbType.UInt32 ).Value = (int) servico.cobradoPor;

				if( String.IsNullOrEmpty( servico.descricao.Trim() ) == false ) {
					cmdServico.Parameters.Add( "@txtDescricao", MySqlDbType.VarChar ).Value = servico.descricao;
				} else {
					cmdServico.Parameters.Add( "@txtDescricao", MySqlDbType.VarChar ).Value = DBNull.Value;
				}

				servico.codigo = UInt32.Parse( cmdServico.ExecuteScalar().ToString() );
				cmdServico.Dispose();

				if( servico.codigo <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível inserir o servi&ccedil;o: " + servico.nome, "Tente inseri-lo novamente" ) );
					continue;
				}

				erros.AddRange( inserirValores( servico.valores, servico.codigo, conn ) );

			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<Erro> atualizarListaDeServicos( ref List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			StringBuilder sqlServico = new StringBuilder();

			sqlServico.AppendLine( "UPDATE tb_servicos SET " );
			sqlServico.AppendLine( "	 nom_servico = @nomServico " );
			sqlServico.AppendLine( "	,int_cobrado_por = @intCobradoPor " );
			sqlServico.AppendLine( "	,txt_descricao = @txtDescricao " );
			sqlServico.AppendLine( "WHERE cod_servico = @codServico " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Servico servico in servicos ) {

				#region ATUALIZA SERVICO
				MySqlCommand cmdServico = new MySqlCommand( sqlServico.ToString(), conn );
				cmdServico.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servico.codigo;
				cmdServico.Parameters.Add( "@intCobradoPor", MySqlDbType.UInt32 ).Value = (int) servico.cobradoPor;
				cmdServico.Parameters.Add( "@nomServico", MySqlDbType.VarChar ).Value = servico.nome;
				if( String.IsNullOrEmpty( servico.descricao.Trim() ) == false ) { cmdServico.Parameters.Add( "@txtDescricao", MySqlDbType.VarChar ).Value = servico.descricao; } else { cmdServico.Parameters.Add( "@txtDescricao", MySqlDbType.VarChar ).Value = DBNull.Value; }
				cmdServico.ExecuteNonQuery();
				cmdServico.Dispose();
				#endregion

				#region ATUALIZA VALORES
				foreach( ValorDeServico val in servico.valores ) {
					if( val.codigo == 0 ) {
						inserirValor( val, servico.codigo, conn, ref erros );
					} else {
						atualizarValor( val, servico.codigo, conn, ref erros );
					}
				}

				List<UInt32> codValoresList = new List<UInt32>();
				preencherCodValoreslist( servico.valores, ref codValoresList );

				#region EXCLUI VALORES NAO USADOS
				MySqlCommand cmdDeleteValores = new MySqlCommand();
				cmdDeleteValores.Connection = conn;
				cmdDeleteValores.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servico.codigo;
				if( codValoresList.Count > 0 ) {
					StringBuilder codigos = new StringBuilder();
					for( int i = 0; i < codValoresList.Count; i++ ) {
						String param = "@codValor_" + i;
						codigos.Append( param + "," );
						cmdDeleteValores.Parameters.Add( new MySqlParameter( param, MySqlDbType.UInt32 ) ).Value = codValoresList[i];
					}
					codigos.Replace( ",", "", codigos.Length - 1, 1 );// remove a ultima virgula

					cmdDeleteValores.CommandText = "DELETE FROM tb_valores_servicos WHERE cod_servico = @codServico AND cod_valor_servico NOT IN ( " + codigos.ToString() + " )";
				} else {
					cmdDeleteValores.CommandText = "DELETE FROM tb_valores_servicos WHERE cod_servico = @codServico";
				}
				cmdDeleteValores.ExecuteNonQuery();
				cmdDeleteValores.Dispose();
				#endregion

				#endregion
			}

			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static void inserirValor( ValorDeServico val, UInt32 codigoServico, MySqlConnection conn, ref List<Erro> erros ) {
			MySqlCommand cmdValor = new MySqlCommand( SQL_INSERT_VALOR, conn );

			val.codigoServico = codigoServico;

			if( val.codigoPai > 0 ) {
				cmdValor.Parameters.Add( "@codValorServicoPai", MySqlDbType.UInt32 ).Value = val.codigoPai;
			} else {
				cmdValor.Parameters.Add( "@codValorServicoPai", MySqlDbType.UInt32 ).Value = DBNull.Value;
			}

			if( val.tipoDeCliente.codigo > 0 ) {
				cmdValor.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = val.tipoDeCliente.codigo;
			} else {
				cmdValor.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = DBNull.Value;
			}

			cmdValor.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = val.codigoServico;
			cmdValor.Parameters.Add( "@codTapete", MySqlDbType.UInt32 ).Value = val.tapete.codigo;
			cmdValor.Parameters.Add( "@valInicial", MySqlDbType.Double ).Value = val.valorInicial;
			cmdValor.Parameters.Add( "@valAcima10m2", MySqlDbType.Double ).Value = val.valorAcima10m2;

			val.codigo = UInt32.Parse( cmdValor.ExecuteScalar().ToString() );
			cmdValor.Dispose();

			if( val.codigo <= 0 ) {
				erros.Add( new Erro( 0, "Não foi possível inserir o valor " + val.valorInicial + "/" + val.valorAcima10m2 + " para o tapete: " + val.tapete.nome, "Tente inseri-lo novamente" ) );
			} else {
				foreach( ValorDeServico valAdicional in val.valoresEspeciais ) {
					valAdicional.codigoPai = val.codigo;
					valAdicional.tapete.codigo = val.tapete.codigo;
				}
			}
			erros.AddRange( inserirValores( val.valoresEspeciais, codigoServico, conn ) );
		}

		public static List<Erro> inserirValores( List<ValorDeServico> valores, UInt32 codigoServico, MySqlConnection conn ) {
			List<Erro> erros = new List<Erro>();
			foreach( ValorDeServico val in valores ) {
				inserirValor( val, codigoServico, conn, ref erros );
			}
			return erros;
		}

		public static void atualizarValor( ValorDeServico val, UInt32 codigoServico, MySqlConnection conn, ref List<Erro> erros ) {
			MySqlCommand cmdValor = new MySqlCommand( SQL_UPDATE_VALOR, conn );

			cmdValor.Parameters.Add( "@codValorServico", MySqlDbType.UInt32 ).Value = val.codigo;
			cmdValor.Parameters.Add( "@valInicial", MySqlDbType.Double ).Value = val.valorInicial;
			cmdValor.Parameters.Add( "@valAcima10m2", MySqlDbType.Double ).Value = val.valorAcima10m2;

			if( val.tipoDeCliente.codigo > 0 ) {
				cmdValor.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = val.tipoDeCliente.codigo;
			} else {
				cmdValor.Parameters.Add( "@codTipoCliente", MySqlDbType.UInt32 ).Value = DBNull.Value;
			}

			int atualizado = cmdValor.ExecuteNonQuery();
			cmdValor.Dispose();
			if( atualizado <= 0 ) {
				erros.Add( new Erro( 0, "Não foi possível atualizar o valor " + val.valorInicial + "/" + val.valorAcima10m2 + " para o tapete: " + val.tapete.nome, "Tente atualiza-lo novamente" ) );
			} else {
				foreach( ValorDeServico valAdicional in val.valoresEspeciais ) {
					valAdicional.codigoPai = val.codigo;
					valAdicional.tapete.codigo = val.tapete.codigo;
				}
			}

			erros.AddRange( atualizarValores( val.valoresEspeciais, codigoServico, conn ) );

		}

		public static List<Erro> atualizarValores( List<ValorDeServico> valores, UInt32 codigoServico, MySqlConnection conn ) {
			List<Erro> erros = new List<Erro>();
			foreach( ValorDeServico val in valores ) {
				if( val.codigo == 0 ) {
					inserirValor( val, codigoServico, conn, ref erros );
				} else {
					atualizarValor( val, codigoServico, conn, ref erros );
				}
			}
			return erros;
		}

		public static List<Erro> excluirListaDeServicos( List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			String sql = "DELETE FROM tb_servicos WHERE cod_servico = @codServico ";

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			// abre a conexao
			conn.Open();

			foreach( Servico servico in servicos ) {
				MySqlCommand cmd = new MySqlCommand( sql, conn );
				cmd.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servico.codigo;
				if( cmd.ExecuteNonQuery() <= 0 ) {
					erros.Add( new Erro( 0, "Não foi possível excluir o servi&ccedil;o: " + servico.nome, "Tente excluí-lo novamente" ) );
				}
				cmd.Dispose();
			}
			// fecha a conexao e libera recursos
			conn.Close(); conn.Dispose();

			return erros;
		}

		public static List<ValorDeServico> estreturarValores( List<ValorDeServico> valores ) {

			List<ValorDeServico> valFilhos = valores.FindAll( delegate( ValorDeServico val ) { return val.codigo != 0 && val.codigoPai != 0; } );
			if( valFilhos == null ) { return valores; }

			List<ValorDeServico> valPais = valores.FindAll( delegate( ValorDeServico val ) { return val.codigo != 0 && val.codigoPai == 0; } );
			if( valPais == null ) { return valores; }

			Dictionary<UInt32, ValorDeServico> dic = new Dictionary<UInt32, ValorDeServico>();
			foreach( ValorDeServico val in valPais ) {
				dic.Add( val.codigo, val );
			}

			foreach( ValorDeServico val in valFilhos ) {
				dic[val.codigoPai].valoresEspeciais.Add( val );
			}

			List<ValorDeServico> valAux = new List<ValorDeServico>();

			foreach( KeyValuePair<UInt32, ValorDeServico> val in dic ) {
				valAux.Add( val.Value );
			}

			valAux.AddRange( valores.FindAll( delegate( ValorDeServico val ) { return val.codigo == 0; } ) );

			return valAux;
		}

		//======================= STATIC =======================

		private static void preencherCodValoreslist( List<ValorDeServico> valores, ref List<UInt32> codigos ) {
			foreach( ValorDeServico val in valores ) {
				codigos.Add( val.codigo );
				preencherCodValoreslist( val.valoresEspeciais, ref codigos );
			}
		}
	}
}
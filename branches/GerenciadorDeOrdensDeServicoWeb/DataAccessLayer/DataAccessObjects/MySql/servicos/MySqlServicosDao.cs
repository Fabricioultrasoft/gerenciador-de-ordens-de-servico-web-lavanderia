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
			+ "	,@valAcima10m2 "
			+ "); "
			+ "SELECT LAST_INSERT_ID() ";
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

		public static Servico getServico( UInt32 codigoServico ) {
			return getServico( codigoServico, false );
		}

		public static Servico getServico( UInt32 codigoServico, bool apenasDadosBasicos ) {
			Servico servico = new Servico( codigoServico );
			StringBuilder sql = new StringBuilder();

			sql.AppendLine( "SELECT" );
			sql.AppendLine( "	 nom_servico" );
			sql.AppendLine( "	,int_cobrado_por" );
			sql.AppendLine( "	,txt_descricao" );
			sql.AppendLine( "FROM tb_servicos " );
			sql.AppendLine( "WHERE cod_servico = @codServico " );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			cmd.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = codigoServico;

			conn.Open();

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
			conn.Close(); conn.Dispose();

			return servico;
		}

		public static List<Servico> getServicos( UInt32 start, UInt32 limit ) {
			return getServicos( start, limit, false );
		}

		public static List<Servico> getServicos( UInt32 start, UInt32 limit, bool apenasDadosBasicos ) {
			List<Servico> servicos = new List<Servico>();
			StringBuilder sqlServicos = new StringBuilder();

			sqlServicos.AppendLine( "SELECT" );
			sqlServicos.AppendLine( "	 cod_servico" );
			sqlServicos.AppendLine( "	,nom_servico" );
			sqlServicos.AppendLine( "	,int_cobrado_por" );
			sqlServicos.AppendLine( "	,txt_descricao" );
			sqlServicos.AppendLine( "FROM tb_servicos " );
			sqlServicos.AppendLine( " ORDER BY nom_servico " );
			if( limit > 0 )
				sqlServicos.AppendLine( " LIMIT " + start + "," + limit );

			MySqlConnection connServicos = MySqlConnectionWizard.getConnection();
			connServicos.Open();
			MySqlCommand cmdServicos = new MySqlCommand( sqlServicos.ToString(), connServicos );

			MySqlDataReader reader = cmdServicos.ExecuteReader();
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
			reader.Close(); reader.Dispose(); cmdServicos.Dispose();
			connServicos.Close(); connServicos.Dispose();

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

		public static List<Erro> inserirValores( List<ValorDeServico> valores, UInt32 codigoServico, MySqlConnection conn ) {
			List<Erro> erros = new List<Erro>();
			foreach( ValorDeServico val in valores ) {

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
	}
}
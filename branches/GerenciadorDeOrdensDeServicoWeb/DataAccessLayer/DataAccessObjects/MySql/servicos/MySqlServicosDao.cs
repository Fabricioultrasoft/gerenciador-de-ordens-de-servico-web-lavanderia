using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using System.Text;

namespace GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos {
	public class MySqlServicosDao {

		#region SQL
		private const String SQL_VALORES
			= "SELECT "
			+ "		 cod_valor_servico "
			+ "		,cod_servico "
			+ "		,tb_tapetes.cod_tapete "
			+ "		,tb_tapetes.nom_tapete "
			+ "		,val_servico_tapete "
			+ "		,val_servico_tapete_acima_10m2 "
			+ "FROM tb_tapetes "
			+ "LEFT JOIN tb_valores_servicos ON tb_tapetes.cod_tapete = tb_valores_servicos.cod_tapete "
			+ "WHERE cod_servico = @codServico OR cod_servico IS NULL ";

		private const String SQL_VALORES_ESPECIAIS
			= "SELECT "
			+ "		 cod_valor_especial "
			+ "		,tb_valores_servicos.cod_valor_servico "
			+ "		,tipos_clientes.cod_tipo_cliente "
			+ "		,tipos_clientes.nom_tipo_cliente "
			+ "		,val_especial "
			+ "		,val_especial_acima_10m2 "
			+ "FROM tb_valores_especiais "
			+ "INNER JOIN tb_valores_servicos ON tb_valores_especiais.cod_valor_servico = tb_valores_servicos.cod_valor_servico "
			+ "INNER JOIN tipos_clientes ON tb_valores_especiais.cod_tipo_cliente = tipos_clientes.cod_tipo_cliente "
			+ "WHERE tb_valores_especiais.cod_valor_servico = @codValorServico ";
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
			sql.AppendLine( "	,flg_valor_unico" );
			sql.AppendLine( "	,val_base" );
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
				servico.flgValorUnico = reader.GetBoolean( "flg_valor_unico" );
				servico.valorBase = reader.GetDouble( "val_base" );
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
			sqlServicos.AppendLine( "	,flg_valor_unico" );
			sqlServicos.AppendLine( "	,val_base" );
			sqlServicos.AppendLine( "	,txt_descricao" );
			sqlServicos.AppendLine( "FROM tb_servicos " );
			sqlServicos.AppendLine( " ORDER BY nom_servico " );
			sqlServicos.AppendLine( " LIMIT " + start + "," + limit );

			MySqlConnection connServicos = MySqlConnectionWizard.getConnection();
			connServicos.Open();
			MySqlCommand cmdServicos = new MySqlCommand( sqlServicos.ToString(), connServicos );

			MySqlDataReader reader = cmdServicos.ExecuteReader();
			while( reader.Read() ) {
				Servico servico = new Servico();

				servico.codigo = reader.GetUInt32( "cod_servico" );
				servico.nome = reader.GetString( "nom_servico" );
				servico.flgValorUnico = reader.GetBoolean( "flg_valor_unico" );
				servico.valorBase = reader.GetDouble( "val_base" );
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

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			MySqlConnection connValEspec = MySqlConnectionWizard.getConnection();
			conn.Open();
			connValEspec.Open();

			MySqlCommand cmd = new MySqlCommand( SQL_VALORES, conn );
			cmd.Parameters.Add( "@codServico", MySqlDbType.UInt32 ).Value = servico.codigo;
			MySqlDataReader reader = cmd.ExecuteReader();
			while( reader.Read() ) {
				ValorDeServico valor = new ValorDeServico();
				valor.codigoServico = servico.codigo;

				// dados do tapete
				valor.tapete.codigo = reader.GetUInt32( "cod_tapete" );
				valor.tapete.nome = reader.GetString( "nom_tapete" );
				
				// dados do serviço se houver
				try { valor.codigo = reader.GetUInt32( "cod_valor_servico" ); } catch { }
				try { valor.valor = reader.GetDouble( "val_servico_tapete" ); } catch { }
				try { valor.valorAcima10m2 = reader.GetDouble( "val_servico_tapete_acima_10m2" ); } catch { }

				// busca por valores especiais
				if( valor.codigo > 0 ) {
					MySqlCommand cmdValEspec = new MySqlCommand( SQL_VALORES_ESPECIAIS, connValEspec );
					cmdValEspec.Parameters.Add( "@codValorServico", MySqlDbType.UInt32 ).Value = valor.codigo;
					MySqlDataReader readerValEspec = cmdValEspec.ExecuteReader();
					while( readerValEspec.Read() ) {
						ValorEspecial valorEspecial = new ValorEspecial();

						valorEspecial.codigo = reader.GetUInt32("cod_valor_especial");
						valorEspecial.codigoValorDeServico = reader.GetUInt32("cod_valor_servico");
						valorEspecial.tipoDeCliente.codigo = reader.GetUInt32("cod_tipo_cliente");
						valorEspecial.tipoDeCliente.nome = reader.GetString("nom_tipo_cliente");
						valorEspecial.valor = reader.GetDouble("val_especial");
						valorEspecial.valorAcima10m2 = reader.GetDouble( "val_especial_acima_10m2" );

						// adiciona o valor especial na lista de valores especiais do valor base do tapete
						valor.valoresEspeciais.Add(valorEspecial);
					}
					readerValEspec.Close(); readerValEspec.Dispose(); cmdValEspec.Dispose();
				}

				// adiciona o valor referente ao tapete na lista de valores do servico
				servico.valores.Add( valor );
			}
			reader.Close(); reader.Dispose();cmd.Dispose(); conn.Close(); conn.Dispose();
			connValEspec.Close(); connValEspec.Dispose();
		}
	}
}
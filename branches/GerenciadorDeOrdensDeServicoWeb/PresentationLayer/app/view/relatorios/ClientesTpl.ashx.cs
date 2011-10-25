using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.clientes;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections;
using System.Data;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.view.relatorios {
	/// <summary>
	/// Summary description for ClientesTpl
	/// </summary>
	public class ClientesTpl : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {

			JavaScriptSerializer js = new JavaScriptSerializer();
			String colunas = context.Request.QueryString["colunas"];
			UInt32 start = 0;
			UInt32 limit = 0;
			long count = 0;
			List<Filter> filters;

			if( String.IsNullOrEmpty( colunas ) ) {
				context.Response.Write( "Nenhuma coluna do relat&oacute;rio foi selecionada!" );
				return;
			}

			UInt32.TryParse( context.Request.QueryString["start"], out start );
			UInt32.TryParse( context.Request.QueryString["limit"], out limit );

			if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
				filters = js.Deserialize<List<Filter>>( context.Request.QueryString["filter"] );
			} else {
				filters = new List<Filter>();
			}

			if( context.Request.QueryString["reportView"] == "xls" ) {
				// relatorio em excel
				context.Response.ContentType = "application/vnd.ms-excel";
			} else {
				// relatorio em html
				context.Response.ContentType = "text/html";
			}


			StringBuilder sql = new StringBuilder();
			MySqlFilter filter = MySqlClientesDao.getFilter( filters );

			sql.AppendFormat( "SELECT {0} FROM tb_clientes ", construirColunasSql( colunas ) );
			sql.AppendLine( "INNER JOIN tb_tipos_clientes ON tb_tipos_clientes.cod_tipo_cliente = tb_clientes.cod_tipo_cliente " );
			sql.AppendLine( filter.whereClause );
			if( limit > 0 )
				sql.AppendLine( " LIMIT " + start + "," + limit );

			MySqlConnection conn = MySqlConnectionWizard.getConnection();
			conn.Open();
			MySqlCommand cmd = new MySqlCommand( sql.ToString(), conn );
			if( filter.parametersList.Count > 0 ) {
				cmd.Parameters.AddRange( filter.parametersList.ToArray() );
			}

			MySqlDataReader reader = cmd.ExecuteReader();

			context.Response.Write( "<html><head><title>Relatório de Clientes</title>"
				+ "<link rel='icon' href='/PresentationLayer/resources/images/favicon.png' />"
				+ "<link rel='stylesheet' type='text/css' href='/PresentationLayer/resources/css/relatorios.css' /></head><body>" );

			context.Response.Write( "<table width=100% border=0 cellspacing=1 ><caption id='caption'>Gerando...</caption><thead><tr>" );

			// escreve o NOME das colunas de acordo com os ALIAS DO SELECT
			for( int i = 0; i < reader.FieldCount; i++ ) {
				context.Response.Write( "<th>" + reader.GetName( i ) + "</th>" );
			}
			context.Response.Write( "</tr></thead><tbody>" );

			// escreve os DADOS de cada registro
			while( reader.Read() ) {
				context.Response.Write( "<tr>" );
				for( int i = 0; i < reader.FieldCount; i++ ) {
					context.Response.Write( "<td>" + reader[i] + "</td>" );
				}
				context.Response.Write( "</tr>" );
				count++;
			}
			context.Response.Write( "</tbody></table>" );
			context.Response.Write( "<script type='text/javascript'>document.getElementById('caption').innerHTML = 'Total: "+ count +" registro(s) encontrado(s)';window.print();</script>" );
			context.Response.Write( "</body></html>" );

			reader.Close(); reader.Dispose(); cmd.Dispose();
			conn.Close(); conn.Dispose();

		}


		private String construirColunasSql( String colunasAlias ) {

			List<String> colunasSql = new List<String>();
			String[] cols = colunasAlias.Split( new char[] { ',' } );

			for( int i = 0; i < cols.Length; i++ ) {
				switch( cols[i] ) {
					case "nome":
						colunasSql.Add( "nom_cliente AS Nome" );
						break;
					case "conjuge":
						colunasSql.Add( "nom_conjuge AS Conjuge" );
						break;
					case "tipoCliente":
						colunasSql.Add( "nom_tipo_cliente AS Tipo" );
						break;
					case "nascimento":
						colunasSql.Add( "dat_nascimento AS 'Nasc.' " );
						break;
					case "sexo":
						colunasSql.Add( "( CASE WHEN int_sexo = 1 THEN 'Masc.' ELSE 'Femi.' END ) AS Sexo" );
						break;
					case "rg":
						colunasSql.Add( "txt_rg AS RG" );
						break;
					case "cpf":
						colunasSql.Add( "txt_cpf AS CPF" );
						break;
					case "observacoes":
						colunasSql.Add( "txt_observacoes AS 'Obs.' " );
						break;
					case "meiosDeContato":
						colunasSql.Add( "( SELECT GROUP_CONCAT(CONCAT(nom_tipo_meio_de_contato,': ',txt_meio_de_contato, '; ')) FROM tb_meios_de_contato A INNER JOIN tb_tipos_meios_de_contato B ON A.cod_tipo_meio_de_contato = B.cod_tipo_meio_de_contato WHERE A.cod_cliente = tb_clientes.cod_cliente ) AS 'Meios Cont.' " );
						break;
					case "Endereco":
						colunasSql.Add( "( SELECT GROUP_CONCAT(CONCAT(nom_logradouro,', ', CAST(int_numero AS CHAR), '. ', nom_bairro, ' - ', nom_cidade, '; ')) FROM tb_enderecos A INNER JOIN tb_logradouros B ON A.cod_logradouro = B.cod_logradouro INNER JOIN tb_bairros C ON B.cod_bairro = C.cod_bairro INNER JOIN tb_cidades D ON C.cod_cidade = D.cod_cidade WHERE A.cod_cliente = tb_clientes.cod_cliente ) AS 'Ends.' " );
						break;
				}
			}

			return String.Join( ",", colunasSql.ToArray() );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
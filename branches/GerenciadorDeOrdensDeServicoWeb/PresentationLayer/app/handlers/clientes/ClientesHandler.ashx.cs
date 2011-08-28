using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Web.Script.Serialization;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.clientes;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer;
using System.Web.SessionState;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.sql;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.clientes {
	/// <summary>
	/// Summary description for ClientesHandler
	/// </summary>
	public class ClientesHandler : IHttpHandler, IRequiresSessionState {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createClientes( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;
					String filters = String.Empty;
					String sorters = String.Empty;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
						filters = context.Request.QueryString["filter"];
					}
					if( String.IsNullOrEmpty( context.Request.QueryString["sort"] ) == false ) {
						sorters = context.Request.QueryString["sort"];
					}
					context.Session["readClientes_start"] = start;
					context.Session["readClientes_limit"] = limit;
					context.Session["readClientes_filters"] = filters;
					context.Session["readClientes_sorters"] = sorters;

					response = readClientes( start, limit, filters, sorters );
					break;
				case "update":
					response = updateClientes( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyClientes( context.Request.Form["records"], context );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createClientes( String records ) {
			List<Cliente> clientes = jsonToClientes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeClientes.cadastrarListaDeClientes( ref clientes );

			#region CONSTROI O JSON
			formatarSaida( ref clientes );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + clientes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Cliente cliente in clientes ) {
				jsonResposta.Append( "{" );
				construirParteDoJsonDadosPrimarios( ref jsonResposta, cliente );
				construirParteDoJsonMeiosDeContato( ref jsonResposta, cliente.meiosDeContato ); jsonResposta.Append( ", " );
				construirParteDoJsonEnderecos( ref jsonResposta, cliente.enderecos );
				jsonResposta.Append( " }\n," );
			}
			if( clientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readClientes( UInt32 start, UInt32 limit, String jsonFilters, String jsonSorters ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Cliente> clientes;
			List<Erro> erros;
			List<Filter> filters;
			List<Sorter> sorters;

			filters = js.Deserialize<List<Filter>>( jsonFilters );
			// se 'filters' for continuar sendo uma referencia nula, cria um objeto vazio dela
			if( filters == null ) {
				filters = new List<Filter>();
			}

			sorters = js.Deserialize<List<Sorter>>( jsonSorters );
			// se 'sorters' for continuar sendo uma referencia nula, cria um objeto vazio dela
			if( sorters == null ) {
				sorters = new List<Sorter>();
			}


			erros = GerenciadorDeClientes.preencherListaDeClientes( out clientes, start, limit, filters, sorters );
			long qtdRegistros = GerenciadorDeClientes.countClientes( filters );
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaida( ref clientes );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + qtdRegistros + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Foram encontrados " + qtdRegistros + " registros\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Cliente cliente in clientes ) {
				jsonResposta.Append( "{" );
				construirParteDoJsonDadosPrimarios( ref jsonResposta, cliente );
				construirParteDoJsonMeiosDeContato( ref jsonResposta, cliente.meiosDeContato ); jsonResposta.Append( ", " );
				construirParteDoJsonEnderecos( ref jsonResposta, cliente.enderecos );
				jsonResposta.Append( " }\n," );
			}
			if( clientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String updateClientes( String records ) {
			List<Cliente> clientes = jsonToClientes( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeClientes.atualizarListaDeClientes( ref clientes );

			#region CONSTROI O JSON
			formatarSaida( ref clientes );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + clientes.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Cliente cliente in clientes ) {
				jsonResposta.Append( "{" );
				construirParteDoJsonDadosPrimarios( ref jsonResposta, cliente );
				construirParteDoJsonMeiosDeContato( ref jsonResposta, cliente.meiosDeContato ); jsonResposta.Append( ", " );
				construirParteDoJsonEnderecos( ref jsonResposta, cliente.enderecos );
				jsonResposta.Append( " }\n," );
			}
			if( clientes.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyClientes( String records, HttpContext context ) {
			List<Cliente> clientes = jsonToClientes( records );
			List<Erro> erros = GerenciadorDeClientes.excluirListaDeClientes( clientes );

			if( erros.Count == 0 ) {
				UInt32 start = 0;
				UInt32 limit = 25;
				String filters = String.Empty;
				String sorters = String.Empty;

				UInt32.TryParse( context.Session["readClientes_start"].ToString(), out start );
				UInt32.TryParse( context.Session["readClientes_limit"].ToString(), out limit );
				if( String.IsNullOrEmpty( context.Session["readClientes_filters"].ToString() ) == false ) {
					filters = context.Session["readClientes_filters"].ToString();
				}
				if( String.IsNullOrEmpty( context.Session["readClientes_sorters"].ToString() ) == false ) {
					sorters = context.Session["readClientes_sorters"].ToString();
				}

				return readClientes( start, limit, filters, sorters );
			} else {
				StringBuilder jsonResposta = new StringBuilder();
				
				formatarSaida( ref clientes );
				jsonResposta.AppendLine( "{" );
				jsonResposta.AppendLine( "    \"total\": " + clientes.Count + "," );
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
				jsonResposta.AppendLine( "    \"data\": []" );
				jsonResposta.AppendLine( "}" );
				
				return jsonResposta.ToString();
			}
		}

		public static List<Cliente> jsonToClientes( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Cliente> clientes = new List<Cliente>();

			List<Dictionary<String, Object>> list = js.Deserialize<List<Dictionary<String, Object>>>( json );

			foreach( Dictionary<String, Object> clienteTemp in list ) {
				Cliente cliente = new Cliente();

				cliente.codigo = UInt32.Parse( clienteTemp["codigo"].ToString() );
				cliente.ativo = bool.Parse( clienteTemp["ativo"].ToString() );
				cliente.nome = clienteTemp["nome"].ToString();
				cliente.conjuge = clienteTemp["conjuge"].ToString();
				cliente.tipoDeCliente.codigo = UInt32.Parse( clienteTemp["codigoTipoDeCliente"].ToString() );
				cliente.tipoDeCliente.nome = clienteTemp["nomeTipoDeCliente"].ToString();
				cliente.tipoDeCliente.ativo = bool.Parse( clienteTemp["ativoTipoDeCliente"].ToString() );
				cliente.sexo = (Sexo) Enum.Parse( typeof( Sexo ), clienteTemp["sexo"].ToString(), true );
				try { cliente.dataDeNascimento = DateTime.Parse( clienteTemp["dataDeNascimento"].ToString() ); } catch { }
				cliente.rg = clienteTemp["rg"].ToString();
				cliente.cpf = clienteTemp["cpf"].ToString();
				cliente.observacoes = clienteTemp["observacoes"].ToString();
				try { cliente.dataDeCadastro = DateTime.Parse( clienteTemp["dataDeCadastro"].ToString() ); } catch { }
				try { cliente.dataDeAtualizacao = DateTime.Parse( clienteTemp["dataDeAtualizacao"].ToString() ); } catch { }


				StringBuilder meiosDeContatoJson = new StringBuilder();
				js.Serialize( clienteTemp["meiosDeContato"], meiosDeContatoJson );
				foreach( Dictionary<String, String> meioDeContatoTemp in js.Deserialize<List<Dictionary<String, String>>>( meiosDeContatoJson.ToString() ) ) {
					MeioDeContato meioDeContato = new MeioDeContato();

					meioDeContato.codigo = UInt32.Parse(meioDeContatoTemp["codigo"]);
					meioDeContato.contato = meioDeContatoTemp["contato"];
					meioDeContato.intContato = (UInt32) Util.getNumeros( meioDeContato.contato );
					meioDeContato.descricao = meioDeContatoTemp["descricao"];
					meioDeContato.tipoDeContato = new TipoDeContato(UInt32.Parse(meioDeContatoTemp["codigoTipoDeContato"]), meioDeContatoTemp["nomeTipoDeContato"]);

					cliente.meiosDeContato.Add(meioDeContato);
				}

				StringBuilder enderecosJson = new StringBuilder();
				js.Serialize( clienteTemp["enderecos"], enderecosJson );
				foreach( Dictionary<String, String> enderecoTemp in js.Deserialize<List<Dictionary<String, String>>>( enderecosJson.ToString() ) ) {
					Endereco endereco = new Endereco();

					endereco.codigo = UInt32.Parse( enderecoTemp["codigo"] );
					endereco.numero = UInt32.Parse( enderecoTemp["numero"] );
					endereco.complemento = enderecoTemp["complemento"];
					endereco.pontoDeReferencia = enderecoTemp["pontoDeReferencia"];

					endereco.logradouro.codigo = UInt32.Parse( enderecoTemp["codigoLogradouro"] );
					endereco.logradouro.nome = enderecoTemp["nomeLogradouro"];
					endereco.logradouro.cep = enderecoTemp["cep"];
					endereco.logradouro.tipoDeLogradouro.codigo = UInt32.Parse( enderecoTemp["codigoTipoDeLogradouro"] );
					endereco.logradouro.tipoDeLogradouro.nome = enderecoTemp["nomeTipoDeLogradouro"];
					endereco.logradouro.bairro.codigo = UInt32.Parse( enderecoTemp["codigoBairro"] );
					endereco.logradouro.bairro.nome = enderecoTemp["nomeBairro"];
					endereco.logradouro.bairro.cidade.codigo = UInt32.Parse( enderecoTemp["codigoCidade"] );
					endereco.logradouro.bairro.cidade.nome = enderecoTemp["nomeCidade"];
					endereco.logradouro.bairro.cidade.estado.codigo = UInt32.Parse( enderecoTemp["codigoEstado"] );
					endereco.logradouro.bairro.cidade.estado.nome = enderecoTemp["nomeEstado"];
					endereco.logradouro.bairro.cidade.estado.pais.codigo = UInt32.Parse( enderecoTemp["codigoPais"] );
					endereco.logradouro.bairro.cidade.estado.pais.nome = enderecoTemp["nomePais"];

					endereco.bairro = endereco.logradouro.bairro;
					endereco.cidade = endereco.logradouro.bairro.cidade;
					endereco.estado = endereco.logradouro.bairro.cidade.estado;
					endereco.pais = endereco.logradouro.bairro.cidade.estado.pais;

					cliente.enderecos.Add(endereco);
				}
				
				clientes.Add( cliente );
			}

			return clientes;
		}

		public static void construirParteDoJsonDadosPrimarios( ref StringBuilder json, Cliente cliente ) {
			json.AppendFormat( " \"codigo\": {0}, ", cliente.codigo );
			json.AppendFormat( " \"nome\": \"{0}\", ", cliente.nome );
			json.AppendFormat( " \"conjuge\": \"{0}\", ", cliente.conjuge );
			json.AppendFormat( " \"codigoTipoDeCliente\": {0}, ", cliente.tipoDeCliente.codigo );
			json.AppendFormat( " \"nomeTipoDeCliente\": \"{0}\", ", cliente.tipoDeCliente.nome );
			json.AppendFormat( " \"ativoTipoDeCliente\": {0}, ", cliente.tipoDeCliente.ativo.ToString().ToLower() );
			json.AppendFormat( " \"sexo\": {0}, ", (int) cliente.sexo );
			json.AppendFormat( " \"strSexo\": \"{0}\", ", cliente.sexo );
			json.AppendFormat( " \"dataDeNascimento\": \"{0}\", ", ( cliente.dataDeNascimento != null ) ? cliente.dataDeNascimento.Value.ToString( "dd/MM/yyyy" ): "" );
			json.AppendFormat( " \"rg\": \"{0}\", ", cliente.rg );
			json.AppendFormat( " \"cpf\": \"{0}\", ", cliente.cpf );
			json.AppendFormat( " \"observacoes\": \"{0}\", ", cliente.observacoes );
			json.AppendFormat( " \"ativo\": {0}, ", cliente.ativo.ToString().ToLower() );
			json.AppendFormat( " \"dataDeCadastro\": \"{0}\", ", cliente.dataDeCadastro.ToString() );
			json.AppendFormat( " \"dataDeAtualizacao\": \"{0}\", ", cliente.dataDeAtualizacao.ToString() );
		}

		public static void construirParteDoJsonMeiosDeContato( ref StringBuilder json, List<MeioDeContato> meiosDeContato ) {
			json.AppendLine( " \"meiosDeContato\": [" );
			foreach( MeioDeContato meioDeContato in meiosDeContato ) {
				json.Append(" { ");
				json.AppendFormat( " \"codigo\": {0}, ", meioDeContato.codigo );
				json.AppendFormat( " \"codigoTipoDeContato\": {0}, ", meioDeContato.tipoDeContato.codigo );
				json.AppendFormat( " \"nomeTipoDeContato\": \"{0}\", ", meioDeContato.tipoDeContato.nome );
				json.AppendFormat( " \"contato\": \"{0}\", ", meioDeContato.contato );
				json.AppendFormat( " \"descricao\": \"{0}\" ", meioDeContato.descricao );
				json.Append( " }," );
			}
			if( meiosDeContato.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.AppendLine( "]" );
		}

		public static void construirParteDoJsonEnderecos( ref StringBuilder json, List<Endereco> enderecos ) {
			json.AppendLine( " \"enderecos\": [" );
			foreach( Endereco endereco in enderecos ) {
				json.Append( " { " );
				json.AppendFormat( " \"codigo\": {0}, ", endereco.codigo );
				json.AppendFormat( " \"complemento\": \"{0}\", ", endereco.complemento );
				json.AppendFormat( " \"pontoDeReferencia\": \"{0}\", ", endereco.pontoDeReferencia );
				json.AppendFormat( " \"numero\": {0}, ", endereco.numero );
				json.AppendFormat( " \"cep\": \"{0}\", ", endereco.logradouro.cep );
				json.AppendFormat( " \"codigoLogradouro\": {0}, ", endereco.logradouro.codigo );
				json.AppendFormat( " \"nomeLogradouro\": \"{0}\", ", endereco.logradouro.nome );
				json.AppendFormat( " \"codigoTipoDeLogradouro\": {0}, ", endereco.logradouro.tipoDeLogradouro.codigo );
				json.AppendFormat( " \"nomeTipoDeLogradouro\": \"{0}\", ", endereco.logradouro.tipoDeLogradouro.nome );
				json.AppendFormat( " \"codigoBairro\": {0}, ", endereco.logradouro.bairro.codigo );
				json.AppendFormat( " \"nomeBairro\": \"{0}\", ", endereco.logradouro.bairro.nome );
				json.AppendFormat( " \"codigoCidade\": {0}, ", endereco.logradouro.bairro.cidade.codigo );
				json.AppendFormat( " \"nomeCidade\": \"{0}\", ", endereco.logradouro.bairro.cidade.nome );
				json.AppendFormat( " \"codigoEstado\": {0}, ", endereco.logradouro.bairro.cidade.estado.codigo );
				json.AppendFormat( " \"nomeEstado\": \"{0}\", ", endereco.logradouro.bairro.cidade.estado.nome );
				json.AppendFormat( " \"codigoPais\": {0}, ", endereco.logradouro.bairro.cidade.estado.pais.codigo );
				json.AppendFormat( " \"nomePais\": \"{0}\" ", endereco.logradouro.bairro.cidade.estado.pais.nome );
				json.Append( " }," );
			}
			if( enderecos.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.AppendLine( "]" );
		}

		public static void formatarSaida( ref List<Cliente> clientes ) {
			for( int i = 0; i < clientes.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Cliente>( clientes[i] );
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
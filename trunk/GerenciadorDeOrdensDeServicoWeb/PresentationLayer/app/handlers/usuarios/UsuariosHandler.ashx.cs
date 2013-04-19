using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.usuarios;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Security.Cryptography;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.usuarios {
	/// <summary>
	/// Summary description for UsuariosHandler
	/// </summary>
	public class UsuariosHandler : IHttpHandler, IRequiresSessionState {

		private static SHA1 sha1 = new SHA1CryptoServiceProvider();
		private HttpContext thisContext;

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			thisContext = context;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createUsuarios( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					context.Session["readUsuarios_start"] = start;
					context.Session["readUsuarios_limit"] = limit;

					response = readUsuarios( start, limit );
					break;
				case "update":
					response = updateUsuarios( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyUsuarios( context.Request.Form["records"] );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createUsuarios( String records ) {

			List<Usuario> usuarios = jsonToUsuarios( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeUsuarios.cadastrar( ref usuarios );

			#region CONSTROI O JSON
			formatarSaida( ref usuarios );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + usuarios.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendFormat( " \"data\": {0}", usuariosToJson( usuarios ) );
			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readUsuarios( UInt32 start, UInt32 limit ) {

			List<Usuario> usuarios;
			List<Erro> erros = GerenciadorDeUsuarios.preencher( out usuarios, start, limit );
			long qtdRegistros = GerenciadorDeUsuarios.count();
			StringBuilder json = new StringBuilder();

			formatarSaida( ref usuarios );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", qtdRegistros );

			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.AppendFormat( " \"message\": [\"Foram encontrados {0} registros\"],", qtdRegistros );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}
			json.AppendFormat( " \"data\": {0}", usuariosToJson( usuarios ) );
			json.AppendLine( "}" );

			return json.ToString();
		}

		private String updateUsuarios( String records ) {
			List<Usuario> usuarios = jsonToUsuarios( records );
			StringBuilder json = new StringBuilder();
			List<Erro> erros = GerenciadorDeUsuarios.atualizar( usuarios );

			formatarSaida( ref usuarios );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", usuarios.Count );
			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.Append( " \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}
			json.AppendFormat( " \"data\": {0}", usuariosToJson( usuarios ) );
			json.Append( "}" );

			return json.ToString();
		}

		private String destroyUsuarios( String records ) {
			List<Usuario> usuarios = jsonToUsuarios( records );
			List<Erro> erros = new List<Erro>();

			Usuario usuarioLogado = (Usuario) thisContext.Session["usuario"];
			foreach( Usuario usuTemp in usuarios ) {
				if( usuTemp.codigo == usuarioLogado.codigo ) {
					usuarioLogado = usuTemp;
					erros.Add( new Erro( 0, "O usu&aacute;rio <b>" + usuTemp.nome + "</b> n&atilde;o pode ser exclu&iacute;do",
						"Para excluir este usu&aacute;rio, &eacute; preciso estar logado com outro usu&aacute;rio." ) );
				}
			}
			// retira o usuario logado da lista de usuarios para excluir
			usuarios.Remove( usuarioLogado );

			erros.AddRange( GerenciadorDeUsuarios.excluir( usuarios ) );

			StringBuilder json = new StringBuilder();
			formatarSaida( ref usuarios );
			json.Append( "{" );
			json.AppendFormat( " \"total\": {0}, ", usuarios.Count );
			if( erros.Count == 0 ) {
				json.Append( " \"success\": true," );
				json.Append( " \"message\": [\"Dados exclu&iacute;dos com sucesso\"]," );
			} else {
				json.Append( " \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref json, erros );
			}
			json.Append( " \"data\": [] }" );

			return json.ToString();
		}

		public static String usuarioToJson( Usuario usuario ) {
			StringBuilder json = new StringBuilder();
			json.Append( "{" );
			json.AppendFormat( " \"codigo\": {0}, ", usuario.codigo );
			json.AppendFormat( " \"nome\": \"{0}\", ", usuario.nome );
			json.AppendFormat( " \"senha\": \"{0}\" ", Util.bytesToHex( sha1.ComputeHash( Util.stringToBytes( usuario.senha ) ) ) );
			json.Append( "}" );
			return json.ToString();
		}

		public static String usuariosToJson( List<Usuario> usuarios ) {
			StringBuilder json = new StringBuilder();

			json.Append( "[" );
			foreach( Usuario usu in usuarios ) {
				json.AppendFormat( "{0},", usuarioToJson( usu ) );
			}
			if( usuarios.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.Append( "]" );

			return json.ToString();
		}

		public static Usuario jsonToUsuario( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			Usuario usuario = js.Deserialize<Usuario>( json );
			return usuario;
		}

		public static List<Usuario> jsonToUsuarios( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Usuario> usuarios = js.Deserialize<List<Usuario>>( json );
			return usuarios;
		}

		public static void formatarSaida( ref List<Usuario> usuarios ) {
			for( int i = 0; i < usuarios.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Usuario>( usuarios[i] );
			}
		}


		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}